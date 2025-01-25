using MessagePack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.IO;
using System.Runtime.Serialization;

namespace SmallTests.Helpers.Serialization
{
    public static class SerializationHelper
    {

        public static T WireUsingNewton<T>(T input)
        {
            //var serialized = JsonSerializer.Serialize(valueDependencies);  //cause exception
            var serialized = JsonConvert.SerializeObject(input); //ok

         //   TestHelpers.Logger.Value.LogInformation("serialized: {serialized}", serialized);

            var deserialized = JsonConvert.DeserializeObject<T>(serialized);

            return deserialized;
        }


        public static T WireUsingDataContract<T>(T input)
        {
            DataContractSerializer ser = new DataContractSerializer(typeof(T));

            var stream = new MemoryStream();
            ser.WriteObject(stream, input); //ok

            stream.Seek(0, SeekOrigin.Begin);

            var deserialized = (T)ser.ReadObject(stream);

            return deserialized;
        }


        public static T WireUsingMessagePack<T>(T input)
        {
            byte[] bytes = MessagePackSerializer.Serialize(input);
            T deserialized = MessagePackSerializer.Deserialize<T>(bytes);
            return deserialized;

        }
    }
}
