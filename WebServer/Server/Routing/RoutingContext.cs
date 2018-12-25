namespace MyCoolWebServer.Server.Routing
{
    using System.Collections.Generic;
    using Contracts;
    using Handlers;
    using Common;

    public class RoutingContext : IRoutingContext
    {
        public RoutingContext(RequestHandler handler, IEnumerable<string> parameters)
        {
            CoreValidator.ThrowIfNull(handler, nameof(handler));
            CoreValidator.ThrowIfNull(parameters, nameof(parameters));

            RequestHandler = handler;
            Parameters = parameters;
        }

        public IEnumerable<string> Parameters { get; private set; }

        public RequestHandler RequestHandler { get; private set; }
    }
}
