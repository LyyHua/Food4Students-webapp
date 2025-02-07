namespace RestaurantService.Entities;

public class Restaurant
{
    public Guid Id { get; set; }
    public string Owner { get; set; }
    public Status Status { get; set; }
    public List<FoodCategory> FoodCategories { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public Photo Logo { get; set; }
    public Photo Banner { get; set; }
    public int TotalRating { get; set; }
    public double AverageRating { get; set; }
    // TODO: The like restaurants
}