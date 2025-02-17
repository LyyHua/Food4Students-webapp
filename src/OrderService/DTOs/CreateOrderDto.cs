using System.ComponentModel.DataAnnotations;

namespace OrderService.DTOs;

public class CreateOrderDto
{
    [Required(ErrorMessage = "At least one order item is required.")]
    [MinLength(1, ErrorMessage = "At least one order item is required.")]
    public List<CreateOrderItemDto> OrderItems { get; set; } = [];
    [Required(ErrorMessage = "ShippingAddress is required.")]
    public required string ShippingAddress { get; set; }
    [Required(ErrorMessage = "PhoneNumber is required.")]
    public required string PhoneNumber { get; set; }
    public string Name { get; set; }
    public string Note { get; set; }
}
