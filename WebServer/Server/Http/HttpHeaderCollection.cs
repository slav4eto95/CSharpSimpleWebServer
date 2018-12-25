
namespace MyCoolWebServer.Server.Http
{
    using Common;
    using Contracts;
    using System;
    using System.Collections.Generic;

    public class HttpHeaderCollection : IHttpHeaderCollection
    {
        private readonly IDictionary<string, HttpHeader> headers;

        public HttpHeaderCollection()
        {
            headers = new Dictionary<string, HttpHeader>();
        }

        public void Add(HttpHeader header)
        {
            CoreValidator.ThrowIfNull(header, nameof(header));
            headers[header.Key] = header;
        }

        public bool ContainsKey(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            return headers.ContainsKey(key);
        }

        public HttpHeader GetHeader(string key)
        {
            CoreValidator.ThrowIfNull(key, nameof(key));

            if (!headers.ContainsKey(key))
            {
                throw new InvalidOperationException($"The fiven key {key} is not present in the headers collection.");
            }

            return headers[key];
        }

        public override string ToString()
            => string.Join(Environment.NewLine, headers);

    }
}