
namespace MyCoolWebServer
{
    using Application;
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    public class Launcher : IRunnable
    {
        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            IApplication application = new MainApplication();
            IAppRouteConfig routeConfig = new AppRouteConfig();
            application.Configure(routeConfig);

            var webServer = new WebServer(1337, routeConfig);
            webServer.Run();
        }
    }
}
