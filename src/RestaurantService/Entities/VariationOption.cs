namespace RestaurantService.Entities;

public class VariationOption
{
    public Guid Id { get; set; }
    public int PriceAdjustment { get; set; }
    public string Name { get; set; }
    // Navigation Property
    public Guid VariationId { get; set; }
    public Variation Variation { get; set; }
}