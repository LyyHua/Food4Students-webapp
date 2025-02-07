using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RestaurantUpdatedConsumer(IMapper mapper) : IConsumer<RestaurantUpdated>
{
    public async Task Consume(ConsumeContext<RestaurantUpdated> context)
    {
        Console.WriteLine("--> Consuming restaurant updated: " + context.Message.Id);

        var restaurant = mapper.Map<Restaurant>(context.Message);

        var result = await DB.Update<Restaurant>()
            .Match(r => r.ID == restaurant.ID)
            .ModifyOnly(i => new { i.Name, i.Address, i.Description, i.PhoneNumber }, restaurant)
            .ExecuteAsync();

        await restaurant.SaveAsync();
    }
}