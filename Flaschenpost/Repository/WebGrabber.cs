using System.Text.Json;
using System.Text.Json.Serialization;
using Flaschenpost.Models;

namespace Flaschenpost.Repository;

public class WebGrabber : IWebGrabber
{
    public async Task<IEnumerable<Beer>?> Grab(string url)
    {
        try
        {
            using var httpClient = new HttpClient();
            return await httpClient.GetFromJsonAsync<IEnumerable<Beer>>(url, new JsonSerializerOptions{ PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() }});
        }
        catch (Exception ex)
        {
            throw new NotSupportedException(ex.Message, ex);
        }
    }

    public async Task<string> GrabImageAsBase64(string url)
    {
        if (!Uri.IsWellFormedUriString(url, UriKind.Absolute) || !url.EndsWith("png"))
        {
            throw new NotSupportedException("File Type not supported");
        }

        try
        {
            using var httpClient = new HttpClient();
            var image = await httpClient.GetByteArrayAsync(url);
            return Convert.ToBase64String(image);
        }
        catch (Exception ex)
        {
            throw new NotSupportedException(ex.Message, ex);
        }
    }
}