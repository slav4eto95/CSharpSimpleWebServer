
namespace MyCoolWebServer.Server.Http.Response
{
    using Enums;
    public class NotFoundResponse : HttpResponse
    {
        public NotFoundResponse()
        {
            StatusCode = HttpStatusCode.NotFound;
        }
    }
}
