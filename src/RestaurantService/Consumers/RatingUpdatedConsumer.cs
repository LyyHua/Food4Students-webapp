using System;
using Contracts;
using MassTransit;
using RestaurantService.Data;

namespace RestaurantService.Consumers;

public class RatingUpdatedConsumer(RestaurantDbContext dbContext) : IConsumer<RatingUpdated>
{
    public async Task Consume(ConsumeContext<RatingUpdated> context)
    {
        Console.WriteLine("--> Consuming updated rating");

        var restaurant = await dbContext.Restaurants.FindAsync(Guid.Parse(context.Message.RestaurantId));

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating + context.Message.NewStars - context.Message.OldStars) / restaurant.TotalRating;

        await dbContext.SaveChangesAsync();
    }
}
