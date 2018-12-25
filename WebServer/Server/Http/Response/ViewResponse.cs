
namespace MyCoolWebServer.Server.Http.Response
{
    using Enums;
    using Exceptions;
    using Server.Contracts;

    public class ViewResponse : HttpResponse
    {
        private readonly IView View;

        public ViewResponse(HttpStatusCode statusCode, IView view)
        {
            ValidateStatusCode(statusCode);
            View = view;
            StatusCode = statusCode;
        }

        private void ValidateStatusCode(HttpStatusCode statusCode)
        {
            if ((int)statusCode > 299 && (int)statusCode < 400)
            {
               throw new InvalidResponseException("View responses need a status code below 300 and above 400 (inclusive)");
            }
        }

        public override string ToString()
        {
            return $"{base.ToString()}{View.View()}";
        }
    }
}
