namespace MyCoolWebServer.Server.Routing
{
    using Contracts;
    using Enums;
    using Handlers;
    using MyCoolWebServer.Server.Http.Contracts;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class AppRouteConfig : IAppRouteConfig
    {
        private readonly Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> routes;

        public AppRouteConfig()
        {
            routes = new Dictionary<HttpRequestMethod, IDictionary<string, RequestHandler>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                routes.Add(method, new Dictionary<string, RequestHandler>());
            }
        }

        public IReadOnlyDictionary<HttpRequestMethod, IDictionary<string, RequestHandler>> Routes => routes;

        public void Get(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            AddRoute(route, new GetHandler(handler));
        }

        public void Post(string route, Func<IHttpRequest, IHttpResponse> handler)
        {
            AddRoute(route, new GetHandler(handler));
        }

        public void AddRoute(string route, RequestHandler httpHandler)
        {
            var handlerName = httpHandler.GetType().Name.ToLower();

            if (handlerName.Contains("get"))
            {
                routes[HttpRequestMethod.GET].Add(route, httpHandler);
            }
            else if (handlerName.Contains("post"))
            {
                routes[HttpRequestMethod.POST].Add(route, httpHandler);
            }
            else
            {
                throw new InvalidOperationException("Invalid handler!");
            }
        }
    }
}
