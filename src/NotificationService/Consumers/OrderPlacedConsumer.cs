using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderPlacedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderPlaced>
{
    public async Task Consume(ConsumeContext<OrderPlaced> context)
    {
        Console.WriteLine("==> Order created message received");

        await hubContext.Clients.All.SendAsync("OrderPlaced", context.Message);
    }
}