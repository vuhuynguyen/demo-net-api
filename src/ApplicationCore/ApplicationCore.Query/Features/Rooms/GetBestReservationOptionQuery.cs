using ApplicationCore.Query.Interfaces;
using ApplicationCoreQuery.DataModel.Rooms;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using RoomType = ApplicationCoreQuery.DataModel.Rooms.RoomType;

namespace ApplicationCore.Query.Features.Rooms;

public class GetBestReservationOptionQuery : IRequest<string>
{
    public int NumberOfGuests { get; set; }

    private class Handler : IRequestHandler<GetBestReservationOptionQuery, string>
    {
        private readonly IQueryDbContext _dbContext;
        private readonly IMapper _mapper;

        public Handler(IQueryDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<string> Handle(GetBestReservationOptionQuery request, CancellationToken cancellationToken)
        {
            var roomAvailabilities = _dbContext.Set<RoomAvailability>()
                .Include(e => e.Type)
                .Include(e => e.PricePeriods)
                .ToList();
            
            var availableRoomTypes = roomAvailabilities
                .Where(ra => ra.AvailableRooms > 0)
                .Select(ra => ra.Type)
                .ToList();
            
            if (availableRoomTypes.Count == 0)
            {
                return "No option";
            }
            
            // Cache room price information for efficient access
            var roomPrices = roomAvailabilities.ToDictionary(ra => ra.Type.Name, ra => ra.GetPrice());
            var validOptions = new List<List<string>>();
            
            // Generate all possible combinations of room types for the given number of guests
            GenerateValidOptions(availableRoomTypes, request.NumberOfGuests, new List<string>(), validOptions, roomAvailabilities);
            
            if (validOptions.Count == 0)
            {
                return "No option";
            }
            
            var cheapestOption = FindCheapestOption(validOptions, roomPrices);
            var totalPrice = cheapestOption.Sum(roomTypeName => roomPrices[roomTypeName]);
            var result = string.Join(" ", cheapestOption) + " - $" + totalPrice.ToString("F2");
            
            return result;
        }
        
        private static void GenerateValidOptions(List<RoomType> availableRoomTypes, int numGuests,
            List<string> currentOption, List<List<string>> validOptions, IReadOnlyList<RoomAvailability> roomAvailabilities)
        {
            var roomTypeCount = availableRoomTypes.Count;
            var roomCounts = new int[roomTypeCount];
            var totalRooms = new int[roomTypeCount];
    
            for (var i = 0; i < roomTypeCount; i++)
            {
                totalRooms[i] = GetAvailableRoomForRoomType(availableRoomTypes[i].Name, roomAvailabilities);
            }
    
            while (true)
            {
                // Check if the current combination is valid
                var validCombination = true;
                var remainingGuests = numGuests;
                for (var i = 0; i < roomTypeCount; i++)
                {
                    remainingGuests -= roomCounts[i] * availableRoomTypes[i].Capacity;
                    if (remainingGuests < 0 || roomCounts[i] > totalRooms[i])
                    {
                        validCombination = false;
                        break;
                    }
                }
        
                if (remainingGuests == 0 && validCombination)
                {
                    var combination = new List<string>();
                    for (var i = 0; i < roomTypeCount; i++)
                    {
                        for (var j = 0; j < roomCounts[i]; j++)
                        {
                            combination.Add(availableRoomTypes[i].Name);
                        }
                    }
                    validOptions.Add(combination);
                }
        
                // Move to the next combination
                var k = roomTypeCount - 1;
                while (k >= 0 && roomCounts[k] >= totalRooms[k])
                {
                    k--;
                }
        
                if (k < 0)
                {
                    break; // All combinations generated
                }
        
                roomCounts[k]++;
                for (var i = k + 1; i < roomTypeCount; i++)
                {
                    roomCounts[i] = 0;
                }
            }
        }
        
        private static List<string> FindCheapestOption(IEnumerable<List<string>> validOptions, Dictionary<string, decimal> roomPrices)
        {
            return validOptions.OrderBy(option =>
            {
                var totalPrice = option.Sum(roomType => roomPrices[roomType]);
                return totalPrice;
            }).First();
        }

        private static int GetAvailableRoomForRoomType(string roomTypeName, IReadOnlyList<RoomAvailability> roomAvailabilities)
        {
            var availableRooms = roomAvailabilities.FirstOrDefault(ra => ra.Type.Name == roomTypeName && ra.AvailableRooms > 0);
            return availableRooms?.AvailableRooms ?? 0;
        }
    }
}