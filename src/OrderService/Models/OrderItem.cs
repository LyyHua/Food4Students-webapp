using System;
using MongoDB.Entities;

namespace OrderService.Models;

public class OrderItem
{
    public required string FoodName { get; set; }
    public string FoodDescription { get; set; }
    public required int Price { get; set; }
    public required int Quantity { get; set; }
    public string FoodItemPhotoUrl { get; set; }
    public string Variations { get; set; }
    public string FoodItemId { get; set; }
}
