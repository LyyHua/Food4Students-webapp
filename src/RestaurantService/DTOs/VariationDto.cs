namespace RestaurantService.DTOs;

public class VariationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOptionDto> VariationOptions { get; set; }
}
