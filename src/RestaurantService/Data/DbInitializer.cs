using System;
using Microsoft.EntityFrameworkCore;
using RestaurantService.Entities;

namespace RestaurantService.Data;

public class DbInitializer
{
    public static void InitDb(WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        
        SeedData(scope.ServiceProvider.GetService<RestaurantDbContext>());
    }

    private static void SeedData(RestaurantDbContext context)
    {
        context.Database.Migrate();

        if (context.Restaurants.Any())
        {
            Console.WriteLine("Data already exists in the database - no seeding");
            return;
        }

        var variationOptions = new List<VariationOption>()
        {
            new(){
                Id = Guid.Parse("7805360d-df7c-4a8f-8e16-b6903613e8d8"),
                Name = "Size L",
                PriceAdjustment = 6000,
                VariationId = Guid.Parse("9a58c1b6-eaae-4bad-abe4-c5cef043671a")
            },
            new(){
                Id = Guid.Parse("6e6af6e0-d225-4914-ba8d-9c6169fc6075"),
                Name = "Size M",
                PriceAdjustment = 0,
                VariationId = Guid.Parse("9a58c1b6-eaae-4bad-abe4-c5cef043671a")
            },
            new(){
                Id = Guid.Parse("9af03782-ae31-4453-9d21-54a433a6e15c"),
                Name = "Ngọt bình thường",
                PriceAdjustment = 0,
                VariationId = Guid.Parse("33dd2721-7b1c-455c-8f50-346cb3bde7ca")
            },
            new(){
                Id = Guid.Parse("955da65c-c3b4-4184-a0a7-ad5bcefeffac"),
                Name = "Giảm ngọt 50%",
                PriceAdjustment = 0,
                VariationId = Guid.Parse("33dd2721-7b1c-455c-8f50-346cb3bde7ca")
            }
        };

        var variations = new List<Variation>()
        {
            new(){
                Id = Guid.Parse("9a58c1b6-eaae-4bad-abe4-c5cef043671a"),
                Name = "Size",
                MinSelect = 1,
                MaxSelect = 1,
                VariationOptions = variationOptions[0..1],
                FoodItemId = Guid.Parse("ff6fe205-c3cb-4878-b777-e48e693bda33")
            },
            new(){
                Id = Guid.Parse("33dd2721-7b1c-455c-8f50-346cb3bde7ca"),
                Name = "Mức ngọt",
                MinSelect = 1,
                MaxSelect = 1,
                VariationOptions = variationOptions[2..3],
                FoodItemId = Guid.Parse("ff6fe205-c3cb-4878-b777-e48e693bda33")
            }
        };

        var foodItems = new List<FoodItem>()
        {
            new(){
                Id = Guid.Parse("ff6fe205-c3cb-4878-b777-e48e693bda33"),
                Name = "Trà Sữa Đào Phô Mai Đào",
                BasePrice = 45000,
                PhotoUrl = "https://maycha.com.vn/wp-content/uploads/2025/01/TS-Dao-PM-Dao-768x768.png",
                Description = "Sự hòa quyện giữa Trà Olong ngọt dịu và nước ép đào tươi giòn mọng, thêm topping Phô Mai Đào ngọt thanh béo ngậy ngon vượt trên kỳ vọng",
                Variations = variations,
                FoodCategoryId = Guid.Parse("d7058196-0112-4d97-9f0c-6604e1dff369")
            },
            new(){
                Id = Guid.Parse("3cc5d9cb-f55b-4b9b-9c9f-f54df00a503c"),
                Name = "Trà Sữa Trân Châu Trắng",
                BasePrice = 35000,
                PhotoUrl = "https://maycha.com.vn/wp-content/uploads/2023/10/tra-sua-tran-chau-trang-1.png",
                Description = "Trà sữa trân châu trắng là một trong những loại trà sữa được ưa chuộng nhất hiện nay. Với hương vị thơm ngon, hấp dẫn, trà sữa trân châu trắng đã trở thành một trong những thức uống được nhiều người yêu thích.",
                FoodCategoryId = Guid.Parse("d7058196-0112-4d97-9f0c-6604e1dff369")
            }
        };

        var foodCategories = new List<FoodCategory>()
        {
            new(){
                Id = Guid.Parse("d7058196-0112-4d97-9f0c-6604e1dff369"),
                Name = "Món mới",
                FoodItems = foodItems
            }
        };

        var restaurants = new List<Restaurant>()
        {
            // 1 Maycha
            new() {
                Id = Guid.Parse("afbee524-5972-4075-8800-7d1f9d7b0a0c"),
                Status = Status.Open,
                Owner = "bob",
                Name = "Trà Sữa Maycha",
                Address = "47B Phan Huy Ích",
                PhoneNumber = "0123456789",
                Description = "Trà Sữa MayCha là một trong những thương hiệu trà sữa “top of mind” của giới trẻ với những sản phẩm chất lượng, sáng tạo và giá cả hợp lý. Với phương châm “Hạnh phúc trong từng lần hút”, MayCha luôn không ngừng phát triển để trao tận tay khách hàng sản phẩm ngon nhất cũng như những giá trị hạnh phúc khi thưởng thức trà sữa tại MayCha.",
                LogoUrl = "https://static.ybox.vn/2023/6/2/1686035147740-348235363_1612468545939577_6728433341266735944_n.jpg",
                BannerUrl = "https://mms.img.susercontent.com/vn-11134513-7ras8-m38rifvrt5nw1e@resize_ss1242x600!@crop_w1242_h600_cT",
                TotalRating = 2487,
                AverageRating = 4.7,
                FoodCategories = foodCategories,
            }
        };

        context.VariationOptions.AddRange(variationOptions);
        context.Variations.AddRange(variations);
        context.FoodItems.AddRange(foodItems);
        context.FoodCategories.AddRange(foodCategories);
        context.Restaurants.AddRange(restaurants);

        context.SaveChanges();

    }
}
