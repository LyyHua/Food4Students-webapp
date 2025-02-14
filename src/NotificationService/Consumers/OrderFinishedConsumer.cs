using System;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using NotificationService.Hubs;

namespace NotificationService.Consumers;

public class OrderFinishedConsumer(IHubContext<NotificationHub> hubContext) : IConsumer<OrderFinished>
{
    public async Task Consume(ConsumeContext<OrderFinished> context)
    {
        Console.WriteLine("==> Order finished message received");

        await hubContext.Clients.All.SendAsync("OrderFinished", context.Message);
    }
}