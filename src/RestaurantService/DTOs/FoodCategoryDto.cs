namespace RestaurantService.DTOs;

public class FoodCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<FoodItemDto> FoodItems { get; set; }
}
