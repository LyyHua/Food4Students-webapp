using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RestaurantDeletedConsumer : IConsumer<RestaurantDeleted>
{
    public async Task Consume(ConsumeContext<RestaurantDeleted> context)
    {
        Console.WriteLine("--> Consuming restaurant deleted: " + context.Message.Id);

        var result = await DB.DeleteAsync<Restaurant>(context.Message.Id);

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(RestaurantDeleted), "Problem deleting restaurant");
    }
}
