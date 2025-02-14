using System;
using Contracts;
using MassTransit;

namespace RatingService.Consumers;

public class RestaurantCreatedConsumer : IConsumer<RestaurantCreated>
{
    public async Task Consume(ConsumeContext<RestaurantCreated> context)
    {
        throw new NotImplementedException();
    }
}
