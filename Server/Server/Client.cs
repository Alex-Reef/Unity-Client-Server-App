using System.Net.Sockets;
using System.Text;
using Packets;

namespace GameServer
{
    public class Client
    {
        public TcpClient socket { get; private set; }
        
        private readonly int ID;
        private NetworkStream _stream;

        private const int BUFFER_SIZE = 4096;
        private byte[] _receiveBuffer;

        public Client(int id)
        {
            ID = id;
        }

        public void Connect(TcpClient client)
        {
            socket = client;

            socket.ReceiveBufferSize = BUFFER_SIZE;
            socket.SendBufferSize = BUFFER_SIZE;

            _stream = socket.GetStream();
            _receiveBuffer = new byte[BUFFER_SIZE];

            _stream.BeginRead(_receiveBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
        }

        public void SendToClient(object Message)
        {
            if (socket == null)
                return;

            var packet = PacketFormatter.Pack(Message);
            _stream.WriteAsync(packet.Data, 0, packet.Data.Length);
        }

        private void ReceiveCallback(IAsyncResult _result)
        {
            try
            {
                int _byteLength = _stream.EndRead(_result);
                if (_byteLength <= 0)
                {
                    Disconnect();
                    return;
                }

                object message = PacketFormatter.Unpack(_receiveBuffer);
                if (message is PacketsTypes)
                {
                    var obj = (PacketsTypes)message;
                    Console.WriteLine(obj.Info);
                }

                if (message is string)
                {
                    Console.WriteLine(message);
                }

                _stream.BeginRead(_receiveBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                Disconnect();
            }
        }

        public void Disconnect()
        {
            Console.WriteLine($"Player ({ID} ID) disconnected");
            socket.Close();
            _stream = null;
            socket = null;
            _receiveBuffer = null;
        }
    }
}
