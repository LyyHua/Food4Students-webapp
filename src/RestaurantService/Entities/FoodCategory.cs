namespace RestaurantService.Entities;

public class FoodCategory
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<FoodItem> FoodItems { get; set; }
    // Navigation Property
    public Restaurant Restaurant { get; set; }
    public Guid RestaurantId { get; set; }
}