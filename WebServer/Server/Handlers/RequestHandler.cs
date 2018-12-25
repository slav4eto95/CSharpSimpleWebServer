
namespace MyCoolWebServer.Server.Handlers
{
    using Contracts;
    using Http.Contracts;
    using System;
    using Common;

    public abstract class RequestHandler : IRequestHandler
    {
        // modify
        private readonly Func<IHttpRequest, IHttpResponse> HandlingFunc;

        protected RequestHandler(Func<IHttpRequest, IHttpResponse> func)
        {
            CoreValidator.ThrowIfNull(func, nameof(func));

            HandlingFunc = func;
        }

        public IHttpResponse Handle(IHttpContext httpContext)
        {
            var httpResponse = HandlingFunc.Invoke(httpContext.Request);
            httpResponse.AddHeader("Content-Type", "text/plain");
            return httpResponse;
        }
    }
}
