using Flaschenpost.Models;
using Machine.Fakes;
using Machine.Specifications;

namespace Flaschenpost.Specs.Models;

[Subject(typeof(Mapper))]
class When_mapping_beer : WithSubject<Mapper>
{
    Establish context = () =>
    {
        Beer = new Beer
        {
            Id = 1,
            BrandName = "Test",
            DescriptionText = "bla",
            Name = "Test Bier",
            Articles = new []
            {
                new BeerArticle
                {
                    Price = 1.99m,
                    PricePerUnitText = "(1,80 €/Liter)",
                    Id = 1,
                    ShortDescription = "20 x 0,5L (Glas)",
                    Unit = Unit.Liter
                },
                new BeerArticle
                {
                    Price = 2.99m,
                    PricePerUnitText = "(1,90 €/Liter)",
                    Id = 2,
                    ShortDescription = "24 x 0,5L (Glas)",
                    Unit = Unit.Liter
                }
            }
        };
    };

    Because of = () => Result = Subject.Map(Beer!, _ => true);

    It should_return_not_null = () => Result.ShouldNotBeNull();

    It should_return_two_results = () => Result!.Count().ShouldEqual(2);

    It should_map_brandname = () => Result!.Select(x => x.BrandName).All(y => y == "Test").ShouldBeTrue();
    It should_map_description = () => Result!.Select(x => x.DescriptionText).All(y => y == "bla").ShouldBeTrue();
    It should_map_name = () => Result!.Select(x => x.Name).All(y => y == "Test Bier").ShouldBeTrue();

    It should_map_1_99_price_for_beer_1 = () => Result!.ElementAt(0).Price.ShouldEqual(1.99m);
    It should_map_1_80_price_per_unit_for_beer_1 = () => Result!.ElementAt(0).PricePerUnit.ShouldEqual(1.8m);
    It should_map_20_bottle_count_for_beer_1 = () => Result!.ElementAt(0).BottleCount.ShouldEqual(20);
    It should_map_unit_liter_for_beer_1 = () => Result!.ElementAt(0).Unit.ShouldEqual(Unit.Liter);

    It should_map_1_99_price_for_beer_2 = () => Result!.ElementAt(1).Price.ShouldEqual(2.99m);
    It should_map_1_80_price_per_unit_for_beer_2 = () => Result!.ElementAt(1).PricePerUnit.ShouldEqual(1.9m);
    It should_map_20_bottle_count_for_beer_2 = () => Result!.ElementAt(1).BottleCount.ShouldEqual(24);
    It should_map_unit_liter_for_beer_2 = () => Result!.ElementAt(1).Unit.ShouldEqual(Unit.Liter);

    static IEnumerable<BeerArticleResponse>? Result;
    static Beer? Beer;
}