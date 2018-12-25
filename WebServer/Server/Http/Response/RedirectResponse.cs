
namespace MyCoolWebServer.Server.Http.Response
{
    using Server.Common;
    using Enums;

    public class RedirectResponse : HttpResponse
    {
        public RedirectResponse(string redirectUrl)
        {
            CoreValidator.ThrowIfNullOrEmpty(redirectUrl, nameof(redirectUrl));

            StatusCode = HttpStatusCode.Found;
            Headers.Add(new HttpHeader("Location", redirectUrl));
        }
    }
}
