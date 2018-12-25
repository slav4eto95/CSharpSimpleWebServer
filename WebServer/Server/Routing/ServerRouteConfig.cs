
namespace MyCoolWebServer.Server.Routing
{
    using Contracts;
    using Enums;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;

    public class ServerRouteConfig : IServerRouteConfig
    {
        private readonly IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> routes;

        public ServerRouteConfig(IAppRouteConfig appRouteConfig)
        {
            routes = new Dictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>>();

            var availableMethods = Enum
                .GetValues(typeof(HttpRequestMethod))
                .Cast<HttpRequestMethod>();

            foreach (var method in availableMethods)
            {
                routes.Add(method, new Dictionary<string, IRoutingContext>());
            }

            InitializeServerConfig(appRouteConfig);
        }

        public IDictionary<HttpRequestMethod, IDictionary<string, IRoutingContext>> Routes => routes;

        public void InitializeServerConfig(IAppRouteConfig appRouteConfig)
        {
            foreach (var registeredRoute in appRouteConfig.Routes)
            {
                var requestMethod = registeredRoute.Key;
                var routesWithHandlers = registeredRoute.Value;

                foreach (var routeWithHandler in routesWithHandlers)
                {
                    var route = routeWithHandler.Key;
                    var handler = routeWithHandler.Value;

                    var parameters = new List<string>();

                    var parsedRouteRegex = ParseRoute(route, parameters);

                    IRoutingContext routingContext = new RoutingContext(handler, parameters);

                    routes[requestMethod].Add(parsedRouteRegex, routingContext);
                }
            }
        }

        private string ParseRoute(string route, List<string> parameters)
        {
            // If we have a regex like /user/{(?<name>[a-z]+)} and it starts with ^ and end with $, 
            // we guarantied that we have a url like /user/ivan will match the regex.
            var result = new StringBuilder();
            result.Append('^');

            if (route == "/")
            {
                result.Append("/$");
                return result.ToString();
            }

            var tokens = route.Split(new[] { '/' }, StringSplitOptions.RemoveEmptyEntries);

            ParseTokens(parameters, tokens, result);

            return result.ToString();
        }

        private void ParseTokens(List<string> parameters, string[] tokens, StringBuilder result)
        {
            for (int index = 0; index < tokens.Length; index++)
            {
                // If current token is the last, add dollar sign to the end of it.
                // Otherwise add a forward slash.
                var end = index == tokens.Length - 1 ? "$" : "/";

                var currentToken = tokens[index];
                // If the token is not a parameter (not surrounded with curly brackets),
                // just add it to the route regex. Otherwise, take the parameter name and the name only.
                if (!currentToken.StartsWith("{") && !currentToken.EndsWith("}"))
                {
                    result.Append($"{currentToken}{end}");
                    continue;
                }

                var pattern = "<\\w+>";
                Regex parameterRegex = new Regex(pattern);
                Match parameterMatch = parameterRegex.Match(currentToken);
                if (!parameterMatch.Success)
                {
                    throw new InvalidOperationException($"Route parameter in '{ currentToken}' is not valid");
                }

                var match = parameterMatch.Groups[0].Value;
                var parameter = match.Substring(1, match.Length - 2);

                parameters.Add(parameter);

                var currentTokenWithoutCurlyBrackets = currentToken.Substring(1, currentToken.Length - 2);

                result.Append($"{currentTokenWithoutCurlyBrackets}{end}");
            }
        }
    }
}
