using Flaschenpost.Models;

namespace Flaschenpost.Repository;

public interface IWebGrabber
{
    Task<IEnumerable<Beer>?> Grab(string url);

    Task<string> GrabImageAsBase64(string url);
}