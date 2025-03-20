using Contracts;
using MassTransit;
using RestaurantService.Data;

namespace RestaurantService.Consumers;

public class RatingConsumer(RestaurantDbContext dbContext) : IConsumer<Rating>
{
    public async Task Consume(ConsumeContext<Rating> context)
    {
        Console.WriteLine("--> Consuming rating");

        var restaurant = await dbContext.Restaurants.FindAsync(Guid.Parse(context.Message.RestaurantId));

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating + context.Message.Stars) / (restaurant.TotalRating + 1);
        restaurant.TotalRating++;

        await dbContext.SaveChangesAsync();
    }
}
