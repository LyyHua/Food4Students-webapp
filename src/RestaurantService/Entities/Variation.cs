namespace RestaurantService.Entities;

public class Variation
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOption> VariationOptions { get; set; }
    // Navigation Property
    public Guid FoodItemId { get; set; }
    public FoodItem FoodItem { get; set; }
}