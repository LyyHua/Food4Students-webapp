using System;
using Contracts;
using MassTransit;
using RestaurantService.Data;

namespace RestaurantService.Consumers;

public class RatingDeletedConsumer(RestaurantDbContext dbContext) : IConsumer<RatingDeleted>
{
    public async Task Consume(ConsumeContext<RatingDeleted> context)
    {
        Console.WriteLine("--> Consuming deleted rating");

        var restaurant = await dbContext.Restaurants.FindAsync(Guid.Parse(context.Message.RestaurantId));

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating - context.Message.Stars) / (restaurant.TotalRating - 1);
        restaurant.TotalRating--;

        await dbContext.SaveChangesAsync();
    }
}
