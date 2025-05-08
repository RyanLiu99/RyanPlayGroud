
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestProjectForNet48
{

    public class BaseKeyItemJsonConverter : JsonConverter<BaseKeyItem>
    {
        private readonly Type _typeToExclude;

        public BaseKeyItemJsonConverter(Type typeToExclude = null)
        {
            _typeToExclude = typeToExclude;
        }

        public override bool CanConvert(Type typeToConvert)
              => typeToConvert != _typeToExclude
                && typeof(BaseKeyItem).IsAssignableFrom(typeToConvert);

        public override BaseKeyItem Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, BaseKeyItem value, JsonSerializerOptions options)
        {
            Console.WriteLine($"Commited: {writer.BytesCommitted};  Pending: {writer.BytesPending}" );


            if (value == null)
            {
                writer.WriteNullValue();
                return;
            }

            //Fetched form db, truncate it.        
            if (value.generation.HasValue)
            {
                writer.WriteStartObject();
                writer.WriteNumber("Id", value.Id);

                if (!string.IsNullOrEmpty(value.Name))
                {
                    writer.WriteString("Name", value.Name);
                }

                writer.WriteNumber("_t_gen", value.generation.Value);
                writer.WriteString("_excludeType", _typeToExclude?.Name);

                writer.WriteEndObject();

            }
            else if (writer.BytesCommitted + writer.BytesPending > PreDefinedOptions.MaxSize)
            {
                Console.WriteLine("Max size reached when doing " + value.Id);
                //writer.WriteNullValue();

                writer.WriteStartObject();

                writer.WriteNumber("Id", value.Id);
                if (!string.IsNullOrEmpty(value.Name))
                {
                    writer.WriteString("Name", value.Name);
                }

                writer.WriteBoolean("_cap", true);

                writer.WriteEndObject();
                return;
            }
            else
            {
                var newOptions = PreDefinedOptions.GetOptions(value.GetType());                
                JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
            }
        }
    }
}

