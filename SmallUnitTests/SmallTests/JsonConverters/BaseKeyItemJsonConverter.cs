using SmallTests.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SmallTests.JsonConverters
{
    public class BaseKeyItemJsonConverter1 : BaseKeyItemJsonConverter
    {
        public BaseKeyItemJsonConverter1() : base(1)
        {
        }
    }
    public class BaseKeyItemJsonConverter2 : BaseKeyItemJsonConverter
    {
        public BaseKeyItemJsonConverter2() : base(2)
        {
        }
    }
    public class BaseKeyItemJsonConverter3 : BaseKeyItemJsonConverter
    {
        public BaseKeyItemJsonConverter3() : base(3)
        {
        }
    }



    public class BaseKeyItemJsonConverter : JsonConverter<BaseKeyItem>
    {
        public int Id{ get; }

        public BaseKeyItemJsonConverter(int id)
        {
            Id = id;
        }
        

        public override bool CanConvert(Type typeToConvert)
        {
             bool result = typeToConvert.IsSubclassOf(typeof(BaseKeyItem));
            //or
            //bool result = typeof(BaseKeyItem).IsAssignableFrom(typeToConvert);

            return result;
        }


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

            if (value.Id == Id)
            {
                writer.WriteStartObject();
                writer.WriteNumber("Id", value.Id);
                writer.WriteNumber("Converter", Id);
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
