using System;
using Contracts;
using MassTransit;

namespace RatingService.Consumers;

public class OrderPlacedConsumer : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine("==> Consuming order placed: " + context.Message.Id);
    }
}
