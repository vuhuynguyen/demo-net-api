using ApplicationCore.Domain.Entities.Room;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Infrastructure.Persistence
{
    public static class DbExtensions
    {
        public static void SeedData(this DatabaseContext context)
        {
            if (!context.Set<RoomType>().Any())
            {
                // Seed Room Types
                var roomTypes = new List<RoomType>
                {
                    new RoomType("Single", 1),
                    new RoomType("Double", 2),
                    new RoomType("Family", 4)
                };

                context.Set<RoomType>().AddRange(roomTypes);
                context.SaveChanges();
            }
            
            if (!context.Set<Room>().Any())
            {
                var roomNames = new List<string>
                {
                    "Ocean Breeze Suite",
                    "Mountain View Retreat",
                    "Sunset Paradise Villa",
                    "Tranquil Lakeside Cabin",
                    "City Lights Penthouse",
                    "Garden Oasis Room",
                    "Serenity Cottage",
                    "Tropical Rainforest Hideaway",
                };

                var roomTypesDict = context.Set<RoomType>().ToDictionary(rt => rt.Name);
                var random = new Random();
                var roomTypeNames = roomTypesDict.Keys.ToList();

                var rooms = roomNames.Select(roomName => new Room
                {
                    Name = roomName,
                    RoomTypeId = roomTypesDict[roomTypeNames[random.Next(0, roomTypeNames.Count)]].Id,
                    IsOccupied = false,
                    RowVersion = new byte[8],
                    CreationDate = DateTime.UtcNow,
                    CreationBy = "Admin"
                }).ToList();

                context.Set<Room>().AddRange(rooms);
                context.SaveChanges();
            }

            if (!context.Set<RoomAvailability>().Any())
            {
                var roomTypesDict = context.Set<RoomType>().ToDictionary(rt => rt.Name);

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

                    context.Set<RoomAvailability>().AddRange(roomAvailabilities);
                    context.SaveChanges();
                }
            }

            if (!context.Set<RoomPricePeriod>().Any())
            {
                var roomAvailabilityDict = context.Set<RoomAvailability>().Include(e => e.Type).ToDictionary(ra => ra.Type.Name);

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

                    context.Set<RoomPricePeriod>().AddRange(roomPricePeriods);
                    context.SaveChanges();
                }
            }
        }
    }
}
