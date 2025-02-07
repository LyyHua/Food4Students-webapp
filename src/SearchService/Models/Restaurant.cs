using System;
using MongoDB.Entities;

namespace SearchService.Models;

public class Restaurant : Entity
{
    public string Owner { get; set; }
    public string Status { get; set; }
    public List<FoodCategory> FoodCategories { get; set; }
    public DateTime CreatedAt { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public int TotalRating { get; set; }
    public double AverageRating { get; set; }
}
