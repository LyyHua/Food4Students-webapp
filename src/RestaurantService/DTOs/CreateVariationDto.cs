using System;

namespace RestaurantService.DTOs;

public class CreateVariationDto
{
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<CreateVariationOptionDto> VariationOptions { get; set; }
}
