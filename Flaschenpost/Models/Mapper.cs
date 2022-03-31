using Flaschenpost.Repository;

namespace Flaschenpost.Models;

public class Mapper : IMapper
{
    readonly IWebGrabber _webGrabber;

    public Mapper(IWebGrabber webGrabber)
    {
        _webGrabber = webGrabber;
    }

    public IEnumerable<BeerArticleResponse> Map(Beer beer, Func<BeerArticle, bool> articleFilter)
    {
        return beer.Articles!
            .Where(articleFilter)
            .Select(beerArticle => new BeerArticleResponse
            {
                BrandName = beer.BrandName,
                DescriptionText = beer.DescriptionText,
                Name = beer.Name,
                Price = beerArticle.Price,
                Unit = beerArticle.Unit,
                BottleCount = beerArticle.BottleCount,
                PricePerUnit = beerArticle.PricePerUnit,
                ImageBase64 = beerArticle.Image != null
                    ? _webGrabber.GrabImageAsBase64(beerArticle.Image).Result
                    : string.Empty
            })
            .ToList();
    }
}