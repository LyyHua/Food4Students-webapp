using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantService.DTOs;

public class CreateFoodCategoryDto
{
    [Required] public string Name { get; set; }
    public List<CreateFoodItemDto> FoodItems { get; set; }
}
