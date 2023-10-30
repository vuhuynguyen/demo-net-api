using Autofac.Extensions.DependencyInjection;
using Autofac;
using Newtonsoft.Json;
using Serilog;
using WebApi.Infrastructure;
using Infrastructure.DependencyInjection;
using Infrastructure.Persistence;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy",
                builder => builder
                .SetIsOriginAllowed((host) => true)
                .AllowAnyMethod()
                .AllowAnyHeader()
                .AllowCredentials()
    );
});

// Serilog
var logger = new LoggerConfiguration()
    .Enrich.WithProperty("App", "Web App Logging")
    .MinimumLevel.Debug()
    .WriteTo.File("Logs\\Log-.txt", rollingInterval: RollingInterval.Day, retainedFileCountLimit: 7)
  .CreateLogger();
builder.Logging.ClearProviders();
builder.Logging.AddSerilog(logger);

// Autofac
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(container =>
{
    container.RegisterModule(new ApplicationModule(builder.Configuration));
});

// MVC
builder.Services.AddControllers(opts =>
{
    opts.Filters.Add(typeof(GlobalExceptionFilter));
    opts.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true;
}).AddNewtonsoftJson(x =>
{
    x.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
    x.SerializerSettings.DateTimeZoneHandling = DateTimeZoneHandling.Utc;
    x.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
    x.SerializerSettings.Formatting = Formatting.Indented;
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

// Seed Sample data
using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetService<DatabaseContext>();
    context.SeedData();
}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");
app.UseAuthorization();

app.MapControllers();

app.Run();
