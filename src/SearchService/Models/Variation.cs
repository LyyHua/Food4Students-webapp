using System;

namespace SearchService.Models;

public class Variation
{
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOption> VariationOptions { get; set; }
}