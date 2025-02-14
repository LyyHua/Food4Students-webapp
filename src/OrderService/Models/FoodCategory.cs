using System;

namespace OrderService.Models;

public class FoodCategory
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<FoodItem> FoodItems { get; set; }
}
