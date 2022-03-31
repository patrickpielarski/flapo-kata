namespace Flaschenpost.Models;

public interface IMapper
{
    IEnumerable<BeerArticleResponse> Map(Beer beer, Func<BeerArticle, bool> articleFilter);
}