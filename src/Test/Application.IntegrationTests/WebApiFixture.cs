using ApplicationCore.Common.Shared;
using ApplicationCore.Domain.Entities.Room;
using ApplicationCore.Query.Interfaces;
using Autofac;
using Infrastructure.Persistence;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Respawn;
using Xunit;
using static ApplicationCore.Common.Shared.ApplicationContext;

namespace Application.IntegrationTests
{
    public class WebApiFixture : IAsyncLifetime
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly WebApplicationFactory<Program> _factory;
        private readonly Checkpoint _checkpoint;
        
        public List<dynamic> RoomTypes { get; set; } = new();
        public List<dynamic> RoomAvailabilities { get; set; } = new();
        public List<dynamic> RoomPricePeriods { get; set; } = new();

        public WebApiFixture()
        {
            _factory = new WebApiFactory();
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            _checkpoint = new Checkpoint
            {
                TablesToIgnore = new[]
                {
                    "__EFMigrationsHistory",
                },
            };
        }

        public async Task InitializeAsync()
        {
            // reset test db to initial state
            await _checkpoint.Reset(_configuration.GetConnectionString("DefaultConnection"));

            await PrepareTestDataAsync();
        }

        private async Task PrepareTestDataAsync()
        {
            // room type
            var roomTypes = new List<RoomType>
            {
                new RoomType("Single", 1),
                new RoomType("Double", 2),
                new RoomType("Family", 4)
            };
            
            foreach (var type in roomTypes)
            {
                await InsertDbAsync(FakePrincipals.Admin, type);
                RoomTypes.Add(type);
            }
            
            // room avaibility
            var roomTypesDict = RoomTypes.ToDictionary(rt => rt.Name);
            if (roomTypesDict.TryGetValue("Single", out var singleType) &&
                roomTypesDict.TryGetValue("Double", out var doubleType) &&
                roomTypesDict.TryGetValue("Family", out var familyType))
            {
                // Seed Room Availabilities
                var roomAvailabilities = new List<RoomAvailability>
                {
                    new RoomAvailability
                    {
                        RoomTypeId = singleType.Id,
                        TotalRooms = 2,
                        AvailableRooms = 2,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    },
                    new RoomAvailability
                    {
                        RoomTypeId = doubleType.Id,
                        TotalRooms = 3,
                        AvailableRooms = 3,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    },
                    new RoomAvailability
                    {
                        RoomTypeId = familyType.Id,
                        TotalRooms = 1,
                        AvailableRooms = 1,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    }
                };
                
                foreach (var roomAvailability in roomAvailabilities)
                {
                    await InsertDbAsync(FakePrincipals.Admin, roomAvailability);
                    RoomAvailabilities.Add(roomAvailability);
                }
            }
            
            var roomAvailabilityDict = RoomAvailabilities.ToDictionary(ra => ra.Type.Name);
            if (roomAvailabilityDict.TryGetValue("Single", out var singleAvailability) &&
                roomAvailabilityDict.TryGetValue("Double", out var doubleAvailability) &&
                roomAvailabilityDict.TryGetValue("Family", out var familyAvailability))
            {
                // Seed Room Price Periods
                var roomPricePeriods = new List<RoomPricePeriod>
                {
                    new RoomPricePeriod
                    {
                        RoomAvailabilityId = singleAvailability.Id,
                        Price = 30,
                        StartDate = DateTime.MinValue,
                        EndDate = DateTime.MaxValue,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    },
                    new RoomPricePeriod
                    {
                        RoomAvailabilityId = doubleAvailability.Id,
                        Price = 50,
                        StartDate = DateTime.MinValue,
                        EndDate = DateTime.MaxValue,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    },
                    new RoomPricePeriod
                    {
                        RoomAvailabilityId = familyAvailability.Id,
                        Price = 85,
                        StartDate = DateTime.MinValue,
                        EndDate = DateTime.MaxValue,
                        CreationBy = "Admin",
                        CreationDate = DateTime.UtcNow
                    }
                };
                
                foreach (var roomPricePeriod in roomPricePeriods)
                {
                    await InsertDbAsync(FakePrincipals.Admin, roomPricePeriod);
                    RoomPricePeriods.Add(roomPricePeriod);
                }
            }
        }

        public Task DisposeAsync()
        {
            _factory.Dispose();
            return Task.CompletedTask;
        }

        public async Task InsertDbAsync<T>(UserPrincipal principal, params T[] entities) where T : class
        {
            await ExecuteDbContextAsync(async (db, applicationContext) =>
            {
                AttachTestData(db);

                applicationContext.Principal = principal;

                foreach (var entity in entities)
                {
                    db.Set<T>().Add(entity);
                }
                await db.SaveChangesAsync();

                return;
            });
        }

        private void AttachTestData(DatabaseContext dbContext)
        {
            RoomTypes.ForEach(x => dbContext.Attach(x));
            RoomAvailabilities.ForEach(x => dbContext.Attach(x));
            RoomPricePeriods.ForEach(x => dbContext.Attach(x));
        }

        public Task<T> QueryDbAsync<T>(Func<IQueryDbContext, Task<T>> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<IQueryDbContext>()));

        public Task ExecuteDbContextAsync(Func<DatabaseContext, ApplicationContext, Task> action)
            => ExecuteScopeAsync(sp => action(sp.GetRequiredService<DatabaseContext>(), sp.GetRequiredService<ApplicationContext>()));

        public async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using var scope = _scopeFactory.CreateScope();

            await action(scope.ServiceProvider);
        }

        public async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using var scope = _scopeFactory.CreateScope();

            var result = await action(scope.ServiceProvider);

            return result;
        }

        public Task<TResponse> SendMediatorAsync<TResponse>(UserPrincipal principal, IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                applicationContext.Principal = principal;

                var mediator = sp.GetRequiredService<IMediator>();

                return mediator.Send(request);
            });
        }
    }

    class WebApiFactory : WebApplicationFactory<Program>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            builder.ConfigureContainer<ContainerBuilder>(b => { /* test overrides here */ });
            return base.CreateHost(builder);
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((_, configBuilder) =>
            {
                configBuilder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("appsettings.test.json", optional: true, reloadOnChange: true);
            });
        }
    }
}
