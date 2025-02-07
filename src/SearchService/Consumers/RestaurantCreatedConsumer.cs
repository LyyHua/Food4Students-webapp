using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RestaurantCreatedConsumer(IMapper mapper) : IConsumer<RestaurantCreated>
{
    public async Task Consume(ConsumeContext<RestaurantCreated> context)
    {
        Console.WriteLine("--> Consuming restaurant created: " + context.Message.Id);

        var restaurant = mapper.Map<Restaurant>(context.Message);

        await restaurant.SaveAsync();
    }
}
