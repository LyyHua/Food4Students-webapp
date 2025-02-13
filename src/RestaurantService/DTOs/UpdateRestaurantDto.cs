using System;

namespace RestaurantService.DTOs;

public class UpdateRestaurantDto
{
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string LogoUrl { get; set; }
    public string BannerUrl { get; set; }
    public string Description { get; set; }
}
