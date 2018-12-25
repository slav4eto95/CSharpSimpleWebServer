namespace MyCoolWebServer.Server.Handlers
{

    using Common;
    using Contracts;
    using Http.Contracts;
    using Http.Response;
    using Routing.Contracts;
    using System.Text.RegularExpressions;

    public class HttpHandler : IRequestHandler
    {
        private readonly IServerRouteConfig ServerRouteConfig;

        public HttpHandler(IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            ServerRouteConfig = serverRouteConfig;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            var requestMethod = httpContext.Request.Method;
            var requestPath = httpContext.Request.Path;
            var registeredRoutes = ServerRouteConfig.Routes[requestMethod];

            foreach (var registeredRoute in registeredRoutes)
            {
                var routePattern = registeredRoute.Key;
                var routingContext = registeredRoute.Value;

                Regex routeRegex = new Regex(routePattern);
                Match match = routeRegex.Match(requestPath);

                if (!match.Success)
                {
                    continue;
                }

                var parameters = routingContext.Parameters;

                foreach (var parameter in parameters)
                {
                    var parameterValue = match.Groups[parameter].Value;
                    httpContext.Request.AddUrlParameters(parameter, parameterValue);
                }

                return routingContext.RequestHandler.Handle(httpContext);
            }

            return new NotFoundResponse();
        }
    }
}
