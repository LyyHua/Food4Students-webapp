using System;

namespace Contracts;

public class RestaurantUpdated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
    public string BannerUrl { get; set; }
    public string LogoUrl { get; set; }
    public string Status { get; set; }
    public DateTime CreatedAt  { get; set; }
    public List<FoodCategoryUpdated> FoodCategories { get; set; }
}
