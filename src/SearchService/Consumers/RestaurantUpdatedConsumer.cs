using System;
using AutoMapper;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Consumers;

public class RestaurantUpdatedConsumer() : IConsumer<RestaurantUpdated>
{
    public async Task Consume(ConsumeContext<RestaurantUpdated> context)
    {
        Console.WriteLine("--> Consuming restaurant updated: " + context.Message.Id);

        var result = await DB.Update<Restaurant>()
            .Match(i => i.ID == context.Message.Id)
            .Modify(i => i.Name, context.Message.Name)
            .Modify(i => i.Address, context.Message.Address)
            .Modify(i => i.Description, context.Message.Description)
            .Modify(i => i.PhoneNumber, context.Message.PhoneNumber)
            .Modify(i => i.LogoUrl, context.Message.LogoUrl)
            .Modify(i => i.BannerUrl, context.Message.BannerUrl)
            .ExecuteAsync();

        if (!result.IsAcknowledged)
            throw new MessageException(typeof(RestaurantUpdated), "Problem updating mongoDb");
    }
}