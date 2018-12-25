
namespace MyCoolWebServer.Server
{
    using Contracts;
    using Routing;
    using Routing.Contracts;
    using System;
    using System.Net;
    using System.Net.Sockets;
    using System.Threading.Tasks;

    public class WebServer : IRunnable
    {
        private const string LocalHostIpAddress = "127.0.0.1";

        private readonly int Port;

        private readonly IServerRouteConfig ServerRouteConfig;

        private readonly TcpListener TcpListener;

        private bool IsRunningServer;

        public WebServer(int port, IAppRouteConfig appRouteConfig)
        {
            Port = port;
            ServerRouteConfig = new ServerRouteConfig(appRouteConfig);

            var ipAddress = IPAddress.Parse(LocalHostIpAddress);
            TcpListener = new TcpListener(ipAddress, port);
        }

        public void Run()
        {
            TcpListener.Start();
            IsRunningServer = true;

            Console.WriteLine($"Server running on {LocalHostIpAddress}:{Port}");

            Task.Run(ListenLoop)
                .Wait();
        }

        private async Task ListenLoop()
        {
            while(IsRunningServer)
            {
                var client = await TcpListener.AcceptSocketAsync();
                var connectionHandler = new ConnectionHandler(client, ServerRouteConfig);
                await connectionHandler.ProcessRequestAsync();
            }
        }
    }
}
