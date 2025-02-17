using System;

namespace Contracts;

public class OrderStatusUpdated
{
    public string Id { get; set; }
    public string Status { get; set; }
}
