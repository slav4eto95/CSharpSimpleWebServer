
namespace MyCoolWebServer.Application
{
    using Controllers;
    using Server.Contracts;
    using Server.Routing.Contracts;

    public class MainApplication : IApplication
    {
        public void Configure(IAppRouteConfig appRouteConfig)
        {
            appRouteConfig
                .Get("/", request => new HomeController().Index());
        }
    }
}
