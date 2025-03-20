using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RatingUpdatedConsumer : IConsumer<RatingUpdated>
{
    public async Task Consume(ConsumeContext<RatingUpdated> context)
    {
        Console.WriteLine("--> Consuming rating updated: " + context.Message.RestaurantId);

        var restaurant = await DB.Find<Restaurant>()
            .OneAsync(context.Message.RestaurantId) ?? throw new MessageException(typeof(RatingUpdatedConsumer), "Restaurant not found");

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating + context.Message.NewStars - context.Message.OldStars) / restaurant.TotalRating;

        await restaurant.SaveAsync();
    }
}
