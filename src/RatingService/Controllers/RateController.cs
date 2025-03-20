using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using RatingService.DTOs;
using RatingService.Models;

namespace RatingService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RateController(IPublishEndpoint publishEndpoint) : ControllerBase
{
    [Authorize]
    [HttpPost("{id}")]
    public async Task<ActionResult> Rate(CreateRateDto createRateDto, string id)
    {
        var order = await DB.Find<FoodOrder>().OneAsync(id);

        if (order is null)
            return NotFound("Order not found.");

        if (order.Status != "Delivered" && order.Status != "Finished")
            return BadRequest("Order is not delivered or finished yet.");

        var rate = new Rate
        {
            RestaurantId = order.RestaurantId,
            Name = User.Identity.Name,
            OrderId = id,
            Comment = createRateDto.Comment,
            Stars = createRateDto.Stars
        };

        await rate.SaveAsync();
        await publishEndpoint.Publish(new Rating{
            RestaurantId = rate.RestaurantId,
            Stars = rate.Stars,
            Comment = rate.Comment,
        });
        return Ok();
    }

    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult> GetOrderRating(string id)
    {
        var rates = await DB.Find<Rate>()
            .Match(r => r.RestaurantId == id)
            .ExecuteAsync();

        if (rates is null)
            return NotFound("No ratings found.");
        
        if (rates[0].Name != User.Identity.Name)
            return Unauthorized("Rate does not belong to the user.");

        return Ok(rates);
    }

    [HttpGet("restaurants/{id}")]
    public async Task<ActionResult> GetRestaurantRating(string id)
    {
        var rates = await DB.Find<Rate>()
            .Match(r => r.RestaurantId == id)
            .ExecuteAsync();

        return Ok(rates);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRate(string id, CreateRateDto createRateDto)
    {
        var order = await DB.Find<FoodOrder>().OneAsync(id);
        var rate = await DB.Find<Rate>()
            .Match(r => r.OrderId == id).ExecuteFirstAsync();

        if (rate is null)
            return NotFound("Rate not found.");

        if (rate.Name != User.Identity.Name)
            return Unauthorized("Rate does not belong to the user.");

        var OldStars = rate.Stars;
        rate.Comment = createRateDto.Comment;
        rate.Stars = createRateDto.Stars;

        await rate.SaveAsync();
        await publishEndpoint.Publish(new RatingUpdated{
            RestaurantId = rate.RestaurantId,
            OldStars = OldStars,
            NewStars = rate.Stars,
            Comment = rate.Comment,
        });

        return Ok();
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRate(string id)
    {
        var rate = await DB.Find<Rate>().OneAsync(id);

        if (rate is null)
            return NotFound("Rate not found.");

        if (rate.Name != User.Identity.Name)
            return Unauthorized("Rate does not belong to the user.");

        var OldStars = rate.Stars;

        await rate.DeleteAsync();
        await publishEndpoint.Publish(new RatingDeleted{
            RestaurantId = rate.RestaurantId,
            Stars = OldStars,
        });

        return Ok();
    }
}
