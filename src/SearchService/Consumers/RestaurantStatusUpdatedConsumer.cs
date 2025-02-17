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

        var result = await DB.Update<Restaurant>()
            .Match(i => i.ID == context.Message.Id)
            .Modify(i => i.Status, context.Message.Status)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(RestaurantStatusUpdated), "Problem updating mongoDb");
    }
}
