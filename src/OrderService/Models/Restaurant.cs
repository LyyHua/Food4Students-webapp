using System;
using MongoDB.Driver;
using MongoDB.Entities;

namespace OrderService.Models;

public class Restaurant : Entity
{
    public string Status { get; set; }
    public string Owner { get; set; }
    public List<FoodCategory> FoodCategories { get; set; }
    public DateTime CreatedAt { get; set; }
}
