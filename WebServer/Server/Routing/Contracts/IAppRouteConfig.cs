
namespace MyCoolWebServer.Server.Routing.Contracts
{
    using Enums;
    using Handlers;
    using System.Collections.Generic;

    public interface IAppRouteConfig
    {
        IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes
        {
            get;
        }

        void AddRoute(string route, RequestHandler httpHandler);
    }
}
