using MongoDB.Entities;

namespace OrderService.Models;

public class FoodOrder : Entity
{
    public required List<OrderItem> OrderItems { get; set; } = [];
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public OrderStatus OrderStatus { get; set; } = OrderStatus.Pending;
    public required string ShippingAddress { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Name { get; set; }
    public required string Orderer { get; set; }
    public string Note { get; set; }
    public int TotalPrice { get; set; }
    public string RestaurantId { get; set; }
    // BIGASS TODO: FIND A WAY TO DO THE REORDER THINGY
    // public bool CanReOrder { get; set; }
}
