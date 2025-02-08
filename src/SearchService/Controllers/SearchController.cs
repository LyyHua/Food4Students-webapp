using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Entities;
using SearchService.Models;
using SearchService.RequestHelpers;

namespace SearchService.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SearchController : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<Restaurant>>> SearchRestaurants([FromQuery] SearchParams searchParams)
    {
        var query = DB.PagedSearch<Restaurant, Restaurant>();

        if (!string.IsNullOrEmpty(searchParams.SearchTerm))
        {
            query.Match(Search.Full, searchParams.SearchTerm).SortByTextScore();
        }

        query = searchParams.OrderBy switch
        {
            "rating" => query.Sort(x => x.Descending(y => y.AverageRating)),
            "newest" => query.Sort(x => x.Descending(y => y.CreatedAt)),
            _ => query.Sort(x => x.Descending(y => y.TotalRating))
        };

        query = searchParams.FilterBy switch
        {
            "closed" => query.Match(x => x.Status == "Closed"),
            _ => query.Match(x => x.Status == "Open")
        };

        query.PageNumber(searchParams.PageNumber);
        query.PageSize(searchParams.PageSize);

        var result = await query.ExecuteAsync();

        return Ok(new
        {
            results = result.Results,
            pageCount = result.PageCount,
            totalCount = result.TotalCount
        });
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Restaurant>> GetRestaurant(string id)
    {
        var restaurant = await DB.Find<Restaurant>().OneAsync(id);
        if (restaurant == null) return NotFound();
        return Ok(restaurant);
    }
}
