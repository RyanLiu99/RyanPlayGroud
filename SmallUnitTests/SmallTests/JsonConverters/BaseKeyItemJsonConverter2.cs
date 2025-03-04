using Ringba.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Ringba.Infrastructure.Impl.LogSerializer
{
    public class BaseKeyItemJsonConverter2 : JsonConverter<BaseKeyItem>
    {
        public override bool CanConvert(Type typeToConvert)
              => typeToConvert.IsSubclassOf(typeof(BaseKeyItem));


        public override BaseKeyItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BaseKeyItem value, JsonSerializerOptions options)
        {
            if (value == null)
            {
                writer.WriteNullValue(); 
                return;
            }

            if(value.Id == 2)  
            {
                writer.WriteStartObject();
                writer.WriteNumber("Id", value.Id);
                writer.WriteString("Name", value.Name);                
                writer.WriteString("Converter", nameof(BaseKeyItemJsonConverter2));

                writer.WriteEndObject();
                return;
            }
            else
            {
                //return; // simple will not be seralized

                JsonSerializer.Serialize(writer, value, options);//Delegate serialization to system converter  as far as I test

            }
        }
    }
}
