using System;
using System.Net.Sockets;
using ReefNetworking.Message;

namespace ReefNetworking.Client
{
    public class NetworkClient
    {
        public Action OnConnected;
        public Action<string> OnDisconnected;
        public Action<Packet> OnReceived;

        private TcpClient _client;
        private NetworkStream _stream;

        private const int BUFFER_SIZE = 4096;
        private byte[] _recieveBuffer;

        public void Connect(string host, int port) 
        {
            _client = new TcpClient() {
                ReceiveBufferSize = BUFFER_SIZE,
                SendBufferSize = BUFFER_SIZE
            };

            _recieveBuffer = new byte[BUFFER_SIZE];

            _client.BeginConnect(host, port, ConnectionCallback, null);
        }

        private void ConnectionCallback(IAsyncResult result)
        {
            _client.EndConnect(result);
            if (!_client.Connected)
            {
                Disconnect("Не удалось подключится");
                return;
            }

            _stream = _client.GetStream();
            _stream.BeginRead(_recieveBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);

            OnConnected.Invoke();
        }

        public void Disconnect(string reason) 
        {
            _client = null;
            _stream = null;
            _recieveBuffer = null;

            OnDisconnected?.Invoke(reason);
        }

        public void ReceiveCallback(IAsyncResult result) 
        {
            try
            {
                int _byteLength = _stream.EndRead(result);
                if (_byteLength <= 0)
                {
                    Disconnect("Потеряно соединение с сервером");
                    return;
                }

                OnReceived.Invoke(new Packet() { Data = _recieveBuffer });

                _stream.BeginRead(_recieveBuffer, 0, BUFFER_SIZE, ReceiveCallback, null);
            }
            catch (Exception e)
            {
                Disconnect(e.Message);
            }
        }

        public void Send<T>(T data)
        {
            var packet = PacketFormatter.Pack(data);
            _stream.WriteAsync(packet.Data, 0, packet.Data.Length);
        }
    }
}
