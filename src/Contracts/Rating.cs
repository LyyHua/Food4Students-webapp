using System;

namespace Contracts;

public class Rating
{
    public string Id { get; set; }
    public string OrderId { get; set; }
    public string RestaurantId { get; set; }
    public int Stars { get; set; }
    public string Comment { get; set; }
    public string Rater { get; set; }
}
