using System;

namespace Contracts;

public class RestaurantUpdated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Address { get; set; }
    public string PhoneNumber { get; set; }
    public string Description { get; set; }
}
