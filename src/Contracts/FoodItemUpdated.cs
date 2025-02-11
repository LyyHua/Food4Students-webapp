using System;

namespace Contracts;

public class FoodItemUpdated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public int BasePrice { get; set; }
    public List<VariationUpdated> Variations { get; set; }
}
