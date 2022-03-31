namespace Flaschenpost.Models;

public class Beer
{
    public long Id { get; set; }
    public string? Name { get; set; }
    public string? BrandName { get; set; }
    public string? DescriptionText { get; set; }
    public IEnumerable<BeerArticle>? Articles { get; set; } = Enumerable.Empty<BeerArticle>();
}