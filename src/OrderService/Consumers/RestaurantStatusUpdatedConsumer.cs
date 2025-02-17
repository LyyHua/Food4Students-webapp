using Contracts;
using MassTransit;
using MongoDB.Entities;
using OrderService.Models;

namespace OrderService.Consumers;

public class RestaurantStatusUpdatedConsumer() : IConsumer<RestaurantStatusUpdated>
{
    public async Task Consume(ConsumeContext<RestaurantStatusUpdated> context)
    {
        Console.WriteLine("--> Consuming restaurant updated: " + context.Message.Id);

        var result = await DB.Update<Restaurant>()
            .Match(i => i.ID == context.Message.Id)
            .Modify(r => r.Status, context.Message.Status)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(RestaurantUpdated), "Problem updating mongoDb");
    }
}