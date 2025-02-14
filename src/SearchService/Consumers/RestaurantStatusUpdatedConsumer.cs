using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RestaurantStatusUpdatedConsumer : IConsumer<RestaurantStatusUpdated>
{
    public async Task Consume(ConsumeContext<RestaurantStatusUpdated> context)
    {
        Console.WriteLine("--> Consuming restaurant status updated: " + context.Message.Id);

        var restaurant = await DB.Find<Restaurant>().OneAsync(context.Message.Id);

        restaurant.Status = context.Message.Status;

        await restaurant.SaveAsync();
    }
}
