using Flaschenpost.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Flaschenpost.Repository;

public class BeerRepository : IBeerRepository
{
    readonly IWebGrabber _webGrabber;
    readonly IMemoryCache _memoryCache;

    public BeerRepository(IWebGrabber webGrabber, IMemoryCache memoryCache)
    {
        _webGrabber = webGrabber;
        _memoryCache = memoryCache;
    }


    public IEnumerable<Beer> GetBeers(string url)
    {
        if (_memoryCache.TryGetValue(url, out IEnumerable<Beer> beerData))
            return beerData;

        beerData = (_webGrabber.Grab(url).GetAwaiter().GetResult() ?? Enumerable.Empty<Beer>()).ToArray();

        var cacheOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromMinutes(1));

        _memoryCache.Set(url, beerData, cacheOptions);

        return beerData;
    }
}