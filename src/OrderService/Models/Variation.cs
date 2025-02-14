using System;

namespace OrderService.Models;

public class Variation
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOption> VariationOptions { get; set; }
}
