using MassTransit;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Entities;

namespace RestaurantService.Data;

public class RestaurantDbContext (DbContextOptions options) : DbContext(options)
{
    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<FoodCategory> FoodCategories { get; set; }
    public DbSet<FoodItem> FoodItems { get; set; }
    public DbSet<Variation> Variations { get; set; }
    public DbSet<VariationOption> VariationOptions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.AddInboxStateEntity();
        modelBuilder.AddOutboxMessageEntity();
        modelBuilder.AddOutboxStateEntity();
    }
}
