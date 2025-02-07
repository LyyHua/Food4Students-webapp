using AutoMapper;
using AutoMapper.QueryableExtensions;
using Contracts;
using MassTransit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Data;
using RestaurantService.DTOs;
using RestaurantService.Entities;

namespace RestaurantService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RestaurantsController(RestaurantDbContext context,
    IMapper mapper, IPublishEndpoint publishEndpoint) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IEnumerable<RestaurantDto>>> GetRestaurants(string date)
    {
        var query = context.Restaurants.AsQueryable();

        if (!string.IsNullOrEmpty(date))
        {
            query = query.Where(x => x.CreatedAt.CompareTo(DateTime.Parse(date).ToUniversalTime()) > 0);
        }

        return await query.ProjectTo<RestaurantDto>(mapper.ConfigurationProvider).ToListAsync();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RestaurantDto>> GetRestaurantById(Guid id)
    {
        var restaurant = await context.Restaurants
            .FirstOrDefaultAsync(x => x.Id == id);

        if (restaurant == null) return NotFound();

        return mapper.Map<RestaurantDto>(restaurant);
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Restaurant>> CreateRestaurant(CreateAndUpdateRestaurantDto restaurantDto)
    {
        var restaurant = mapper.Map<Restaurant>(restaurantDto);

        restaurant.Owner = User.Identity.Name;

        context.Restaurants.Add(restaurant);

        var newRestaurant = mapper.Map<RestaurantDto>(restaurant);

        await publishEndpoint.Publish(mapper.Map<RestaurantCreated>(newRestaurant));

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not save changes to the database");

        return CreatedAtAction(nameof(GetRestaurantById), new { id = restaurant.Id }, restaurant);
    }

    [Authorize]
    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateRestaurant(Guid id, CreateAndUpdateRestaurantDto restaurantDto)
    {
        var restaurant = await context.Restaurants.FirstOrDefaultAsync(x => x.Id == id);

        if (restaurant == null) return NotFound();

        if (restaurant.Owner != User.Identity.Name) return Forbid();

        restaurant.Name = restaurantDto.Name;
        restaurant.Description = restaurantDto.Description;
        restaurant.Address = restaurantDto.Address;
        restaurant.PhoneNumber = restaurantDto.PhoneNumber;

        await publishEndpoint.Publish(mapper.Map<RestaurantUpdated>(restaurant));

        var result = await context.SaveChangesAsync() > 0;

        if (result) return Ok();

        return BadRequest("Could not save changes to the database");
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteRestaurant(Guid id)
    {
        var restaurant = await context.Restaurants.FindAsync(id);

        if (restaurant == null) return NotFound();

        if (restaurant.Owner != User.Identity.Name) return Forbid();

        context.Restaurants.Remove(restaurant);

        await publishEndpoint.Publish<RestaurantDeleted>(new { Id = id.ToString() });

        var result = await context.SaveChangesAsync() > 0;

        if (!result) return BadRequest("Could not update DB");

        return Ok();
    }
}
