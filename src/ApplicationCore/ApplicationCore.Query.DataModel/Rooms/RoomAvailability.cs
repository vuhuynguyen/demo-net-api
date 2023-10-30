namespace ApplicationCoreQuery.DataModel.Rooms;

public class RoomAvailability
{
    public Guid Id { get; set; }
    
    public RoomType Type { get; set; }
    
    public int RoomTypeId { get; set; }
    
    public int TotalRooms { get; set; }
    
    public int AvailableRooms { get; set; }
    
    public List<RoomPricePeriod> PricePeriods { get; set; }
    
    public decimal GetPrice()
    {
        // Find the corresponding RoomPricePeriod for the provided RoomAvailability
        var pricePeriod = PricePeriods.FirstOrDefault(period =>
            DateTime.Now >= period.StartDate && DateTime.Now <= period.EndDate);

        return pricePeriod?.Price ?? 0;
    }
}