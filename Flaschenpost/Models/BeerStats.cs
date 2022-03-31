using System.Text.Json.Serialization;

namespace Flaschenpost.Models;

public class BeerStats
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<BeerArticleResponse>? ByPrice { get; set; }

    public IEnumerable<BeerArticleResponse>? MinPrice { get; set; }

    public IEnumerable<BeerArticleResponse>? MaxPrice { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public IEnumerable<BeerArticleResponse>? MaxBottles { get; set; }
}