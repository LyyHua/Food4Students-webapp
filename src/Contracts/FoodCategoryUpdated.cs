using System;

namespace Contracts;

public class FoodCategoryUpdated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public List<FoodItemUpdated> FoodItems { get; set; }
}
