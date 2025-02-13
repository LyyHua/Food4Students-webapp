namespace Contracts;

public class MenuUpdated
{
    public string Id { get; set; }
    public List<FoodCategoryUpdated> FoodCategories { get; set; }
}
