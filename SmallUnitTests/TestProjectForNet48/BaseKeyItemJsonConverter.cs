
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
                writer.WriteString("Name", value.Name);
                writer.WriteNumber("_t_gen", value.generation.Value);
                writer.WriteString("_excludeType", _typeToExclude?.Name);

                writer.WriteEndObject();

            }
            else
            {
                //return; // simple will not be seralized

                var newOptions = PreDefinedOptions.GetOptions(value.GetType());                
                JsonSerializer.Serialize(writer, value, value.GetType(), newOptions);
            }
        }
    }
}

