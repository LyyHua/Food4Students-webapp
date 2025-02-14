using System;
using MongoDB.Entities;

namespace RatingService.Models;

public class Restaurant : Entity
{
    public string Status { get; set; }
}
