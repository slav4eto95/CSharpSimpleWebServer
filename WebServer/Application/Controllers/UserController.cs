namespace MyCoolWebServer.Application.Controllers
{
    using MyCoolWebServer.Server;
    using Server.Enums;
    using Server.Http.Contracts;
    using Server.Http.Response;
    using Views;

    public class UserController
    {
        public IHttpResponse RegisterGet()
        {
            return new ViewResponse(HttpStatusCode.OK, new RegisterView());
        }

        public IHttpResponse RegisterPost(string name)
        {
            return new RedirectResponse($"/user/{name}");
        }

        public IHttpResponse Details(string name)
        {
            Model model = new Model();
            return new ViewResponse(HttpStatusCode.OK, new UserDetailsView(model));
        }
    }
}
