namespace Contracts;

public class OrderItemUpdated
{
    public string FoodName { get; set; }
    public string FoodDescription { get; set; }
    public int Price { get; set; }
    public int Quantity { get; set; }
    public string FoodItemPhotoUrl { get; set; }
    public string Variations { get; set; }
    public string FoodItemId { get; set; }
}
