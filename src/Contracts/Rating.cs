using System;

namespace Contracts;

public class Rating
{
    public string RestaurantId { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
}
