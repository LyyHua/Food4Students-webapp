using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantService.DTOs;

public class CreateFoodItemDto
{
    [Required] public string Name { get; set; }
    public string Description { get; set; }
    [Required] public int BasePrice { get; set; }
    public List<CreateVariationDto> Variations { get; set; }
}
