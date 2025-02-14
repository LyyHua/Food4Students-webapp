using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderDeliveringConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderDelivering>
{
    public async Task Consume(ConsumeContext<OrderDelivering> context)
    {
        Console.WriteLine("==> Order delivering message received");

        await hubContext.Clients.All.SendAsync("OrderDelivering", context.Message);
    }
}
