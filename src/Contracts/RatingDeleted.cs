using System;

namespace Contracts;

public class RatingDeleted
{
    public string RestaurantId { get; set; }
    public int Stars { get; set; }
}
