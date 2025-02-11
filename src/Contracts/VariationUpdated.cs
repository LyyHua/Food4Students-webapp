namespace Contracts;

public class VariationUpdated
{
    public string Id { get; set; }
    public string Name { get; set; }
    public int MinSelect { get; set; }
    public int? MaxSelect { get; set; }
    public List<VariationOptionUpdated> VariationOptions { get; set; }
}
