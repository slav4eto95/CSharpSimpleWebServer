
namespace MyCoolWebServer
{
    using Server;
    using Server.Contracts;
    using Server.Routing;
    using Server.Routing.Contracts;

    public class Launcher : IRunnable
    {
        private WebServer webServer;

        public static void Main()
        {
            new Launcher().Run();
        }

        public void Run()
        {
            IAppRouteConfig routeConfig = new AppRouteConfig();
            webServer = new WebServer(8888, routeConfig);
            webServer.Run();
        }
    }
}
