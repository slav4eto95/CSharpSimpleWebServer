
namespace MyCoolWebServer.Server
{
    using Common;
    using Handlers;
    using Http;
    using Http.Contracts;
    using Routing.Contracts;
    using System;
    using System.Net.Sockets;
    using System.Text;
    using System.Threading.Tasks;

    public class ConnectionHandler
    {
        private readonly Socket Client;
        private readonly IServerRouteConfig ServerRouteConfig;

        public ConnectionHandler(Socket client, IServerRouteConfig serverRouteConfig)
        {
            CoreValidator.ThrowIfNull(client, nameof(client));
            CoreValidator.ThrowIfNull(serverRouteConfig, nameof(serverRouteConfig));

            Client = client;
            ServerRouteConfig = serverRouteConfig;
        }

        public async Task ProcessRequestAsync()
        {
            var httpRequest = await ReadRequest();

            var httpContext = new HttpContext(httpRequest);

            var httpResponse = new HttpHandler(ServerRouteConfig).Handle(httpContext);

            var responseBytes = Encoding.UTF8.GetBytes(httpResponse.ToString());

            var byteSegments = new ArraySegment<byte>(responseBytes);
            
            await Client.SendAsync(responseBytes, SocketFlags.None);

            Console.WriteLine($"-----REQUEST-----");
            Console.WriteLine(httpRequest);
            Console.WriteLine($"-----RESPONSE-----");
            Console.WriteLine(httpResponse.ToString());
            Console.WriteLine();

            Client.Shutdown(SocketShutdown.Both);
        }

        public async Task<IHttpRequest> ReadRequest()
        {
            var request = new StringBuilder();
            var data = new ArraySegment<byte>(new byte[1024]);
            
            while (true)
            {
                int numberOfBytesRead = await Client.ReceiveAsync(data, SocketFlags.None);

                if (numberOfBytesRead == 0)
                    break;

                var bytesAsString = Encoding.UTF8.GetString(data.Array, 0, numberOfBytesRead);

                request.Append(bytesAsString);

                if (numberOfBytesRead < 1024)
                    break;
            }

            return new HttpRequest(request.ToString());
        }
    }
}
