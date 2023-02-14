using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace ReefNetworking.Message
{
    public class Packet
    {
        public byte[] Data { get; set; }
    }

    public static class PacketFormatter
    {
        public static Packet Pack(object Object)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(stream, Object);
                return new Packet() { Data = stream.ToArray() };
            }
        }

        public static object Unpack(byte[] data)
        {
            using (MemoryStream stream = new MemoryStream(data))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                return formatter.Deserialize(stream);
            }
        }
    }
}
