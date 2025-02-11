using System;

namespace RestaurantService.DTOs;

public class CreateVariationOptionDto
{
    public int PriceAdjustment { get; set; }
    public string Name { get; set; }
}
