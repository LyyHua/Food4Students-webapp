using System;

namespace OrderService.DTOs;

public class UpdateOrderStatusDto
{
    public string Id { get; set; }
    public string OrderStatus { get; set; }
}
