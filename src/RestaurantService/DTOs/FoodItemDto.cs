namespace RestaurantService.DTOs;

public class FoodItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string PhotoUrl { get; set; }
    public int BasePrice { get; set; }
    public List<VariationDto> Variations { get; set; }
}
