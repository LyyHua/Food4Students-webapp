using System.ComponentModel.DataAnnotations;

namespace RestaurantService.DTOs;

public class CreateAndUpdateRestaurantDto
{
    [Required] public string Name { get; set; }
    [Required] public string Address { get; set; }
    [Required] public string PhoneNumber { get; set; }
    public string Description { get; set; }
}