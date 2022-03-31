using Flaschenpost.Models;
using Flaschenpost.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Flaschenpost.Controllers;

[ApiController]
[Route("api/[action]")]
public class BeerController : ControllerBase
{
    readonly IMapper _mapper;
    readonly IBeerRepository _repository;

    public BeerController(IBeerRepository repository, IMapper mapper)
    {
        _mapper = mapper;
        _repository = repository;
    }

    [HttpGet(Name = "CheapestAndMostExpensivePerUnit")]
    public IActionResult CheapestAndMostExpensivePerUnit(string url)
    {
        if (!IsValidUrl(url, out var actionResult)) 
            return actionResult;

        var beers = _repository.GetBeers(url).ToArray();

        var minPrice = beers!.SelectMany(x => x.Articles!).Min(y => y.PricePerUnit);
        var maxPrice = beers!.SelectMany(x => x.Articles!).Max(y => y.PricePerUnit);

        var stats = new BeerStats
        {
            MinPrice = beers!
                .Where(b => b.Articles!.Any(ar => ar.PricePerUnit == minPrice))
                .SelectMany(beer => _mapper.Map(beer, art => art.PricePerUnit == minPrice)),
            MaxPrice = beers!
                .Where(b => b.Articles!.Any(ar => ar.PricePerUnit == maxPrice))
                .SelectMany(beer => _mapper.Map(beer, art => art.PricePerUnit == maxPrice))
        };

        return Ok(stats);
    }

    [HttpGet(Name = "ByPrice")]
    public IActionResult ByPrice(string url, decimal price)
    {
        if (!IsValidUrl(url, out var actionResult))
            return actionResult;

        if (price < 0)
            return BadRequest("Price is required");

        var result = _repository.GetBeers(url)
            .Where(x => x.Articles!.Any(y => y.Price == price))
            .SelectMany(beer => _mapper.Map(beer, art => art.Price == price))
            .OrderBy(x => x.PricePerUnit);

        return Ok(result);
    }

    [HttpGet(Name = "MaxBottles")]
    public IActionResult MaxBottles(string url)
    {
        if (!IsValidUrl(url, out var actionResult))
            return actionResult;

        var beers = _repository.GetBeers(url).ToArray();
        var maxBottleCount = beers!
            .SelectMany(x => x.Articles!)
            .Max(y => y.BottleCount);
        
        var result = beers!
            .Where(z => z.Articles!
                .Any(ar => ar.BottleCount == maxBottleCount))
            .SelectMany(x => _mapper.Map(x, art => art.BottleCount == maxBottleCount));

        return Ok(result);
    }

    [HttpGet(Name = "Stats")]
    public IActionResult Stats(string url, decimal price)
    {
        if (!IsValidUrl(url, out var actionResult))
            return actionResult;

        if (price < 0)
            return BadRequest("Price is required");

        if ((CheapestAndMostExpensivePerUnit(url) as OkObjectResult)?.Value is not BeerStats stats)
            return BadRequest();

        stats.ByPrice = (ByPrice(url, price) as OkObjectResult)?.Value as IEnumerable<BeerArticleResponse>;
        stats.MaxBottles = (MaxBottles(url) as OkObjectResult)?.Value as IEnumerable<BeerArticleResponse>;
        return Ok(stats);
    }

    bool IsValidUrl(string url, out IActionResult actionResult)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) || !url.EndsWith("json"))
        {
            actionResult = BadRequest("Not a valid JSON URL");
            return false;
        }

        actionResult = Ok();
        return true;
    }
}