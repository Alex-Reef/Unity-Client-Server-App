using UnityEngine;
using ReefNetworking.Client;
using ReefNetworking.Message;
using Packets;

public class Test : MonoBehaviour
{
    private NetworkClient client;

    public class PlayerInfo
    {
        public string Name;
        public Vector3 Position;
    }

    void Start()
    {
        client = new NetworkClient();
        client.OnConnected += OnConnected;
        client.OnDisconnected += OnDisconnected;
        client.OnReceived += OnRecieved;

        client.Connect("127.0.0.1", 7777);
    }

    private void OnDestroy()
    {
        client.OnConnected -= OnConnected;
        client.OnDisconnected -= OnDisconnected;
        client.OnReceived -= OnRecieved;
        client.Disconnect(null);
    }

    private void OnConnected()
    {
        Debug.Log("Connected");
        client.Send(new Data() { Info = "str" });
        client.Send("Hi, server");
    }

    private void OnDisconnected(string reason)
    {
        Debug.Log(reason);
    }

    private void OnRecieved(Packet packet)
    {
        object message = PacketFormatter.Unpack(packet.Data);
        if (message is Data)
        {
            var obj = (Data)message;
            Debug.Log(obj.Info);
        }
        if (message is string)
        {
            Debug.Log(message);
        }
    }
}
