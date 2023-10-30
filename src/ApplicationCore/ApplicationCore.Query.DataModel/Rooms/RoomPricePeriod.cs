namespace ApplicationCoreQuery.DataModel.Rooms;

public class RoomPricePeriod
{
    public Guid Id { get; set; }
    
    public decimal Price { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime EndDate { get; set; }
}