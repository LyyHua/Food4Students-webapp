using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using RatingService.Models;

namespace RatingService.Consumers;

public class OrderPlacedConsumer() : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine("==> Consuming order placed: " + context.Message.Id);

        var order = new FoodOrder
        {
            ID = context.Message.Id,
            RestaurantId = context.Message.RestaurantId,
        };

        await order.SaveAsync();
    }
}
