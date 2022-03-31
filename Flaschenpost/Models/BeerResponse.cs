using System.Text.Json.Serialization;

namespace Flaschenpost.Models;

public class BeerArticleResponse
{
    public string? Name { get; set; }
    public string? BrandName { get; set; }
    public string? DescriptionText { get; set; }
    public decimal Price { get; set; }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public Unit Unit { get; set; }
    public string? ImageBase64 { get; set; }
    public int? BottleCount { get; set; }
    public decimal? PricePerUnit { get; set; }
}