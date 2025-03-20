using System;
using MongoDB.Entities;

namespace RatingService.Models;

public class FoodOrder : Entity
{
    public string RestaurantId { get; set; }
    public string Status { get; set; }
}
