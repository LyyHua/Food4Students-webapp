using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using RatingService.Models;

namespace RatingService.Consumers;

public class OrderStatusUpdatedConsumer () : IConsumer<OrderStatusUpdated>
{
    public async Task Consume(ConsumeContext<OrderStatusUpdated> context)
    {
        Console.WriteLine("==> Consuming order status updated: " + context.Message.Id);

        var result = await DB.Update<FoodOrder>()
            .Match(i => i.ID == context.Message.Id)
            .Modify(r => r.Status, context.Message.Status)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(RestaurantUpdated), "Problem updating mongoDb");
    }
}