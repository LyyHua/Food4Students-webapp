using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RatingDeletedConsumer : IConsumer<RatingDeleted>
{
    public async Task Consume(ConsumeContext<RatingDeleted> context)
    {
        Console.WriteLine("--> Consuming rating deleted: " + context.Message.RestaurantId);

        var restaurant = await DB.Find<Restaurant>()
            .OneAsync(context.Message.RestaurantId) ?? throw new MessageException(typeof(RatingDeletedConsumer), "Restaurant not found");

        restaurant.AverageRating = (restaurant.AverageRating * restaurant.TotalRating - context.Message.Stars) / (restaurant.TotalRating - 1);
        restaurant.TotalRating--;

        await restaurant.SaveAsync();
    }
}
