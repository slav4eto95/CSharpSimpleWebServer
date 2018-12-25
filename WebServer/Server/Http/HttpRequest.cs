
namespace MyCoolWebServer.Server.Http
{
    using Common;
    using Contracts;
    using Enums;
    using Exceptions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;

    public class HttpRequest : IHttpRequest
    {
        public HttpRequest(string requestString)
        {
            CoreValidator.ThrowIfNullOrEmpty(requestString, nameof(requestString));

            FormData = new Dictionary<string, string>();
            Headers = new HttpHeaderCollection();
            QueryParameters = new Dictionary<string, string>();
            UrlParameters = new Dictionary<string, string>();

            ParseRequest(requestString);
        }

        public IDictionary<string, string> FormData { get; private set; }

        public HttpHeaderCollection Headers { get; private set; }

        public string Path { get; private set; }

        public IDictionary<string, string> QueryParameters { get; private set; }

        public HttpRequestMethod Method { get; private set; }

        public string Url { get; private set; }

        public IDictionary<string, string> UrlParameters { get; private set; }

        public void AddUrlParameters(string key, string value)
        {
            CoreValidator.ThrowIfNullOrEmpty(key, nameof(key));
            CoreValidator.ThrowIfNullOrEmpty(value, nameof(value));

            UrlParameters[key] = value;
        }

        public void ParseRequest(string requestString)
        {
            string[] requestLines = requestString.Split(Environment.NewLine);

            if (!requestLines.Any())
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            string[] requestLine =
                requestLines
                    .First()
                    .Trim()
                    .Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

            if (requestLine.Length != 3
                || requestLine[2].ToLower() != "http/1.1")
            {
                BadRequestException.ThrowFromInvalidRequest();
            }

            Method = ParseRequestMethod(requestLine.First().ToUpper());
            Url = requestLine[1];
            Path = ParsePath(Url);
            ParseHeaders(requestLines);
            ParseParameters();

            ParseFormData(requestLines.Last());
        }

        private HttpRequestMethod ParseRequestMethod(string method)
        {
            HttpRequestMethod parsedMethod;
            if (!Enum.TryParse(method, true, out parsedMethod))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
            return parsedMethod;
        }

        private string ParsePath(string url)
            => url.Split(new[] { '?', '#' }, StringSplitOptions.RemoveEmptyEntries)[0];

        private void ParseHeaders(string[] requestLines)
        {
            int endIndex = Array.IndexOf(requestLines, string.Empty);
            for (int i = 1; i < endIndex; i++)
            {
                string[] headerArgs = requestLines[i]
                    .Split(new[] { ": " }, StringSplitOptions.None);

                if (headerArgs.Length != 2)
                {
                    BadRequestException.ThrowFromInvalidRequest();
                }

                var headerKey = headerArgs[0];
                var headerValue = headerArgs[1].Trim();

                // Create a new HttpHeader and add it to the collection
                Headers.Add(new HttpHeader(headerKey, headerValue));
            }

            // Check if there is a header with a key named "Host".
            // If there isn't one, throw an exception.
            if (!Headers.ContainsKey("Host"))
            {
                BadRequestException.ThrowFromInvalidRequest();
            }
        }

        private void ParseParameters()
        {
            if (!Url.Contains("?"))
                return;

            var query = Url
                            .Split(new[] { "?" }, StringSplitOptions.RemoveEmptyEntries)
                            .Last();

            ParseQuery(query, UrlParameters);
        }

        private void ParseQuery(string query, IDictionary<string, string> dict)
        {


            if (!query.Contains("="))
                return;

            string[] queryPairs = query.Split(new[] { "&" }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var queryPair in queryPairs)
            {
                string[] queryArgs = queryPair.Split(new[] { "=" }, StringSplitOptions.RemoveEmptyEntries);
                if (queryArgs.Length != 2)
                    continue;

                var queryKey = WebUtility.UrlDecode(queryArgs.First());
                var queryValue = WebUtility.UrlDecode(queryArgs.Last());

                dict.Add(queryKey, queryValue);
            }
        }

        private void ParseFormData(string formDataLine)
        {
            if (Method == HttpRequestMethod.GET)
                return;

            ParseQuery(formDataLine, QueryParameters);
        }
    }
}
