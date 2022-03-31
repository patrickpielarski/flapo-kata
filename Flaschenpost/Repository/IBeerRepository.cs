using Flaschenpost.Models;

namespace Flaschenpost.Repository;

public interface IBeerRepository
{
    IEnumerable<Beer> GetBeers(string url);
}