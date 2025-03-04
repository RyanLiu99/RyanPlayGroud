using System;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmallTests.JsonConverters
{

    public class TupleJsonConverter : JsonConverter<ITuple>
    {
        private readonly JsonSerializerOptions _options;

        public TupleJsonConverter(JsonSerializerOptions options)
        {
            _options = options;
        }

        public override ITuple? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override void Write(Utf8JsonWriter writer, ITuple value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();

            for (var p = 0; p < value.Length; p++)
            {

                var v = value[p];
                var dataType = v.GetType();

                try
                {
                    var converter = _options.GetConverter(dataType);
                    // ((JsonConverter<string>)converter).Write();
                }
                catch (Exception e)
                {

                    throw;
                }



            }

            writer.WriteEndArray();
        }
    }



}
