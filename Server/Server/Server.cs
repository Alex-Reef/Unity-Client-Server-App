using System.Net;
using System.Net.Sockets;
using System.Linq;
using Packets;

namespace GameServer
{
    public class Server
    {
        private static int PORT;
        private static int MAX_PLAYERS;

        private static TcpListener _listener;

        public static Dictionary<int, Client> Clients;

        public static void Create(int port, int maxPlayers)
        {
            PORT = port;
            MAX_PLAYERS = maxPlayers;

            _listener = new TcpListener(IPAddress.Any, PORT);

            InitClients();

            _listener.Start();

            Console.WriteLine("Server started");

            _listener.BeginAcceptTcpClient(ConnectCallback, null);
        }

        private static void InitClients()
        {
            Console.WriteLine("Initializing...");
            Clients = new Dictionary<int, Client>();

            for(int i = 0; i < MAX_PLAYERS; i++)
            {
                Clients.Add(i, new Client(i));
            }

            Console.WriteLine("Initializing done.");
        }

        private static void ConnectCallback(IAsyncResult result)
        {
            TcpClient tcpClient = _listener.EndAcceptTcpClient(result);
            _listener.BeginAcceptTcpClient(ConnectCallback, null);

            var client = Clients.FirstOrDefault(client => client.Value.socket == null);
            if(client.Value != null)
            {
                client.Value.Connect(tcpClient);
                client.Value.SendToClient($"Hello. Your ID: {client.Key}");
                client.Value.SendToClient(new PacketsTypes() { Info = "Hi"});

                Console.WriteLine($"Client #{client.Key} connected");
            }
        }
    }
}
