using System;

namespace OrderService.Models;

public class VariationOption
{
    public string Id { get; set; }
    public int PriceAdjustment { get; set; }
    public string Name { get; set; }
}
