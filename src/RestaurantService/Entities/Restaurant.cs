namespace RestaurantService.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public RestaurantStatus RestaurantStatus { get; set; }
}
