using System.Text;
using Confluent.Kafka;
using Newtonsoft.Json;

namespace BuildingBlocks.Messaging.Messaging.Kafka;

public class CustomSerializer<T> : ISerializer<T>
{
    public byte[] Serialize(T data, SerializationContext context)
    {
        if (data == null) return null;
        var json = JsonConvert.SerializeObject(data);
        return Encoding.UTF8.GetBytes(json);
    }
}