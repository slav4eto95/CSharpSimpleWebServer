
namespace MyCoolWebServer.Server.Http.Contracts
{
    using Enums;

    public interface IHttpResponse
    {
        HttpStatusCode StatusCode { get; }
        HttpHeaderCollection Headers { get; }
        void AddHeader(string location, string url);
    }
}
