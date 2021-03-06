﻿
namespace MyCoolWebServer.Server.Http
{
    using Contracts;
    using Common;

    public class HttpContext : IHttpContext
    {
        private readonly IHttpRequest request;

        public HttpContext(IHttpRequest request)
        {
            CoreValidator.ThrowIfNull(request, nameof(request));
            this.request = request;
        }

        public IHttpRequest Request => this.request;
    }
}
