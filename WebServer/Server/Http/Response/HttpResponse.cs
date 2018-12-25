
namespace MyCoolWebServer.Server.Http.Response
{
    using Contracts;
    using Enums;
    using Server.Common;
    using Server.Contracts;
    using System.Text;

    public abstract class HttpResponse : IHttpResponse
    {
        private string StatusCodeMessage => StatusCode.ToString();

        protected HttpResponse()
        {
            Headers = new HttpHeaderCollection();
        }

        public HttpHeaderCollection Headers { get; }

        public HttpStatusCode StatusCode { get; protected set; }
        
        public override string ToString()
        {
            StringBuilder response = new StringBuilder();
            response.AppendLine($"HTTP/1.1 {StatusCode} {StatusCodeMessage}");

            response.AppendLine(Headers.ToString());
            response.AppendLine();

            return response.ToString();
        }


        public void AddHeader(string location, string url)
        {
            CoreValidator.ThrowIfNullOrEmpty(location, nameof(location));
            CoreValidator.ThrowIfNullOrEmpty(url, nameof(url));

            Headers.Add(new HttpHeader(location, url));
        }
    }
}
