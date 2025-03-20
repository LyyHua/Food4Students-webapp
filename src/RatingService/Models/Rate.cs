using System;
using MongoDB.Entities;

namespace RatingService.Models;

public class Rate : Entity
{
    public required string RestaurantId { get; set; }
    public required string Name { get; set; }
    public required string OrderId { get; set; }
    public string Comment { get; set; }
    public required int Stars { get; set; }
}
