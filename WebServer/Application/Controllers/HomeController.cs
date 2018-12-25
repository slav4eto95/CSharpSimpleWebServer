
namespace MyCoolWebServer.Application.Controllers
{
    using Server.Http.Contracts;
    using Server.Http.Response;

    public class HomeController
    {
        public IHttpResponse Index()
        {
            return new ViewResponse(Server.Enums.HttpStatusCode.OK, new HomeIndexView());
        }
    }
}
