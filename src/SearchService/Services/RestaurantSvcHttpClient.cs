using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class RestaurantSvcHttpClient(HttpClient httpClient, IConfiguration config)
{
    public async Task<List<Restaurant>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Restaurant, string>()
            .Sort(x => x.Descending(x => x.CreatedAt))
            .Project(x => x.CreatedAt.ToString())
            .ExecuteFirstAsync();

        var auctionURL = config["RestaurantServiceUrl"];

        var url = auctionURL + "/api/restaurants";

        if (!string.IsNullOrEmpty(lastUpdated))
        {
            url += $"?date={lastUpdated}";
        }

        var items = await httpClient.GetFromJsonAsync<List<Restaurant>>(url);

        return items ?? [];
    }
}