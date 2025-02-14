using System;
using MongoDB.Entities;

namespace RatingService.Models;

public class Rate : Entity
{
    public required string RestaurantId { get; set; }
    public required string User { get; set; }
    public required string OrderId { get; set; }
}
