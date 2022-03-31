namespace Flaschenpost.Models;

public class BeerArticle
{
    public long Id { get; set; }
    public string? ShortDescription { get; set; }
    public decimal Price { get; set; }
    public Unit Unit { get; set; }  
    public string? PricePerUnitText { get; set; }
    public string? Image { get; set; }

    public int BottleCount
    {
        get
        {
            var bottlesString = ShortDescription?[..ShortDescription.IndexOf(" ", StringComparison.Ordinal)];
            return int.TryParse(bottlesString, out int bottleCount) 
                ? bottleCount 
                : 0;
        }
    }

    public decimal PricePerUnit
    {
        get
        {
            var pricePerUnitString = PricePerUnitText?[1..PricePerUnitText.IndexOf(" ", StringComparison.Ordinal)];
            return decimal.TryParse(pricePerUnitString, out decimal pricePerUnit)
                ? pricePerUnit
                : 0;
        }
    }
}

public enum Unit
{
    Liter
}