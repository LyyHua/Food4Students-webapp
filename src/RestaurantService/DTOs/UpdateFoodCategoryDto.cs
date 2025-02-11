namespace RestaurantService.DTOs;

public class UpdateFoodCategoryDto
{
    public string Name { get; set; }
    public List<FoodItemDto> FoodItems { get; set; }
}
