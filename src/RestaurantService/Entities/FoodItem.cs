using System;

namespace RestaurantService.Entities;

public class FoodItem
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public int BasePrice { get; set; }
    public List<Variation> Variations { get; set; }
    // Navigation Property
    public Guid FoodCategoryId { get; set; }
    public FoodCategory FoodCategory { get; set; }
}
