using System;
using Contracts;
using MassTransit;
using MongoDB.Entities;
using OrderService.Models;

namespace OrderService.Services;

public class CheckFoodOrderFinished(ILogger<CheckFoodOrderFinished> logger, IServiceProvider services) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        logger.LogInformation("Starting to checked for finished orders");

        stoppingToken.Register(() => logger.LogInformation("==> Order check stopping"));

        while (!stoppingToken.IsCancellationRequested)
        {
            await CheckOrders(stoppingToken);

            await Task.Delay(5000, stoppingToken);
        }
    }

    private async Task CheckOrders(CancellationToken stoppingToken)
    {
        var deliveringOrders = await DB.Find<FoodOrder>()
            .Match(o => o.OrderStatus == OrderStatus.Delivering)
            .Match(o => o.CreatedAt < DateTime.UtcNow.AddDays(-7))
            .ExecuteAsync(stoppingToken);
        
        if (deliveringOrders.Count == 0) return;

        logger.LogInformation("==> Found {deliveringOrders.Count} orders that has finished", deliveringOrders.Count);

        using var scope = services.CreateScope();
        var endpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

        foreach (var order in deliveringOrders)
        {
            order.OrderStatus = OrderStatus.Finished;
            await order.SaveAsync(null, stoppingToken);

            await endpoint.Publish<OrderFinished>(order, stoppingToken);
        }
    }
}
