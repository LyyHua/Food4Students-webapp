using System;

namespace SearchService.Models;

public class FoodItem
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public int BasePrice { get; set; }
    public List<Variation> Variations { get; set; }
}