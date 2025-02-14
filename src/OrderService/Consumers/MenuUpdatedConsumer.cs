using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using OrderService.Models;


namespace OrderService.Consumers;

public class MenuUpdatedConsumer(IMapper mapper) : IConsumer<MenuUpdated>
{
    public async Task Consume(ConsumeContext<MenuUpdated> context)
    {
        Console.WriteLine("--> Consuming restaurant menu updated: " + context.Message.Id);

        var newRestaurant = mapper.Map<Restaurant>(context.Message);

        var result = await DB.Update<Restaurant>()
            .MatchID(context.Message.Id)
            .ModifyOnly(i => new { i.FoodCategories }, newRestaurant)
            .ExecuteAsync();

        // BIGASS TODO: FIND A WAY TO DO THE REORDER THINGY

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(MenuUpdated), "Problem updating mongoDb");
    }
}