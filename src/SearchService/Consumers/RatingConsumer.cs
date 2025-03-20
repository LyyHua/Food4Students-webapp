using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RatingConsumer : IConsumer<Rating>
{
    public async Task Consume(ConsumeContext<Rating> context)
    {
        Console.WriteLine("--> Consuming rating: " + context.Message.RestaurantId);

        var restaurant = await DB.Find<Restaurant>()
            .OneAsync(context.Message.RestaurantId) ?? throw new MessageException(typeof(RatingConsumer), "Restaurant not found");

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating + context.Message.Stars) / (restaurant.TotalRating + 1);
        restaurant.TotalRating++;

        await restaurant.SaveAsync();
    }
}
