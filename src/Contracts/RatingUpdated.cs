using System;

namespace Contracts;

public class RatingUpdated
{
    public string RestaurantId { get; set; }
    public int OldStars { get; set; }
    public int NewStars { get; set; }
    public string Comment { get; set; }
}
