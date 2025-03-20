using System;

namespace Contracts;

public class OrderFinished
{
    public string Id { get; set; }
    public List<OrderItemUpdated> OrderItems { get; set; }
    public DateTime CreatedAt { get; set; }
    public string OrderStatus { get; set; }
    public string ShippingAddress { get; set; }
    public string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Orderer { get; set; }
    public string Note { get; set; }
    public int TotalPrice { get; set; }
}
