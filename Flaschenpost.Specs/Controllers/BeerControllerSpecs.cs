using System.Text.Json;
using System.Text.Json.Serialization;
using Flaschenpost.Controllers;
using Flaschenpost.Models;
using Flaschenpost.Repository;
using Flaschenpost.Specs.Properties;
using Machine.Fakes;
using Machine.Specifications;
using Microsoft.AspNetCore.Mvc;

namespace Flaschenpost.Specs.Controllers;

[Subject(typeof(BeerController))]
class When_CheapestAndMostExpensivePerUnit_is_called_with_valid_url : WithStaticTestData
{
    Because of = () => Result = (Subject.CheapestAndMostExpensivePerUnit("http://www.test.com/test.json") as OkObjectResult)?.Value as BeerStats;

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_Oettinger_as_cheapest = () => Result!.MinPrice!.Select(beer => beer.BrandName).ShouldContain("Oettinger");
    It should_return_one_beer_as_cheapest = () => Result!.MinPrice!.Count().ShouldEqual(1);
    It should_return_cheapest_with_price_per_unit_1 = () => Result!.MinPrice!.ElementAt(0).PricePerUnit.ShouldEqual(1m);
    
    It should_return_Finne_as_most_expensive = () => Result!.MaxPrice!.ElementAt(0).BrandName.ShouldEqual("Finne");
    It should_return_one_beer_as_most_expensive = () => Result!.MaxPrice!.Count().ShouldEqual(1);
    It should_return_most_expensive_with_price_per_unit_3_78 = () => Result!.MaxPrice!.ElementAt(0).PricePerUnit.ShouldEqual(3.78m);

    static BeerStats? Result;
}

[Subject(typeof(BeerController))]
class When_CheapestAndMostExpensivePerUnit_is_called_with_invalid_url_missing_json_extension : WithStaticTestData
{
    Because of = () => Result = Subject.CheapestAndMostExpensivePerUnit("http://www.test.com/test.jso");

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_CheapestAndMostExpensivePerUnit_is_called_with_invalid_url_string : WithStaticTestData
{
    Because of = () => Result = Subject.CheapestAndMostExpensivePerUnit("abc");

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_ByPrice_is_called_with_valid_url_and_price_17_99 : WithStaticTestData
{
    Because of = () => Result = (Subject.ByPrice("http://www.test.com/test.json", 17.99m) as OkObjectResult)?.Value as IEnumerable<BeerArticleResponse>;

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_3_beers = () => Result!.Count().ShouldEqual(3);

    It should_return_Krombacher_and_Bueble = () => Result!.Select(x=>x.BrandName).ShouldContain("Krombacher", "Büble");

    It should_return_1_80_per_unit_as_first_beer = () => Result!.ElementAt(0).PricePerUnit.ShouldEqual(1.8m);
    It should_return_1_80_per_unit_as_second_beer = () => Result!.ElementAt(1).PricePerUnit.ShouldEqual(1.8m);
    It should_return_2_27_per_unit_as_third_beer = () => Result!.ElementAt(2).PricePerUnit.ShouldEqual(2.27m);

    static IEnumerable<BeerArticleResponse>? Result;
}

[Subject(typeof(BeerController))]
class When_ByPrice_is_called_with_invalid_url_missing_json_extension : WithStaticTestData
{
    Because of = () => Result = Subject.ByPrice("http://www.test.com/test.jso", 17.99m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_ByPrice_is_called_with_invalid_url_string : WithStaticTestData
{
    Because of = () => Result = Subject.ByPrice("abc", 17.99m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_ByPrice_is_called_with_negative_price : WithStaticTestData
{
    Because of = () => Result = Subject.ByPrice("http://www.test.com/test.json", -1m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_MaxBottles_is_called_with_valid_url : WithStaticTestData
{
    Because of = () => Result = (Subject.MaxBottles("http://www.test.com/test.json") as OkObjectResult)?.Value as IEnumerable<BeerArticleResponse>;

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_2_beers = () => Result!.Count().ShouldEqual(2);

    It should_return_Finne_and_Krombacher = () => Result!.Select(beer => beer.BrandName).ShouldContain("Finne", "Krombacher");

    It should_return_a_max_bottle_count_of_24 =
        () => Result!
            .All(art => art.BottleCount == 24)
            .ShouldBeTrue();

    static IEnumerable<BeerArticleResponse>? Result;
}

[Subject(typeof(BeerController))]
class When_MaxBottles_is_called_with_invalid_url_missing_json_extension : WithStaticTestData
{
    Because of = () => Result = Subject.MaxBottles("http://www.test.com/test.jso");

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_MaxBottles_is_called_with_invalid_url_string : WithStaticTestData
{
    Because of = () => Result = Subject.MaxBottles("abc");

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_Stats_is_called_with_valid_url_and_price_17_99 : WithStaticTestData
{
    Because of = () => Result = (Subject.Stats("http://www.test.com/test.json", 17.99m) as OkObjectResult)?.Value as BeerStats;

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_max_bottles = () => Result!.MaxBottles.ShouldNotBeEmpty();

    It should_return_min_price = () => Result!.MinPrice.ShouldNotBeEmpty();

    It should_return_max_price = () => Result!.MaxPrice.ShouldNotBeEmpty();

    It should_return_by_price = () => Result!.ByPrice.ShouldNotBeEmpty();

    static BeerStats? Result;
}

[Subject(typeof(BeerController))]
class When_Stats_is_called_with_invalid_url_missing_json_extension : WithStaticTestData
{
    Because of = () => Result = Subject.Stats("http://www.test.com/test.jso", 17.99m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_Stats_is_called_with_invalid_url_string : WithStaticTestData
{
    Because of = () => Result = Subject.Stats("abc", 17.99m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

[Subject(typeof(BeerController))]
class When_Stats_is_called_with_negative_price : WithStaticTestData
{
    Because of = () => Result = Subject.Stats("http://www.test.com/test.json", -1m);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_bad_request = () => Result.ShouldBeOfExactType<BadRequestObjectResult>();

    static IActionResult? Result;
}

abstract class WithStaticTestData : WithSubject<BeerController>
{
    Establish context = () =>
    {
        The<IBeerRepository>()
            .WhenToldTo(x => x.GetBeers(Param<string>.IsAnything))
            .Return(JsonSerializer.Deserialize<IEnumerable<Beer>>(
                Resources.TestData,
                new JsonSerializerOptions
                    { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } }) ?? Enumerable.Empty<Beer>());
        Configure(x => x.For<IMapper>().Use<Mapper>());
    };
}