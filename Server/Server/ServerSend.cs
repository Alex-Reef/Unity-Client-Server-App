using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameServer
{
    public static class ServerSend
    {
        public static void SendToAll<T>(T Message)
        {
            for(int i = 0; i < Server.Clients.Count; i++)
            {
                SendToClient(Message, Server.Clients[i]);
            }
        }

        public static void SendWithoutOne<T>(T Message, Client client)
        {
            for (int i = 0; i < Server.Clients.Count; i++)
            {
                if (Server.Clients[i] != client)
                    SendToClient(Message, Server.Clients[i]);
            }
        }

        public static void SendToClient<T>(T Message, Client client)
        {
            client.SendToClient(Message);
        }
    }
}
