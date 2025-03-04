using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Ringba.Infrastructure.Impl.LogSerializer;
using SmallTests.Entities;

namespace SmallTests
{
    public class DedupEntitySerializationTest
    {
        [Test]
        public void ShouldPass()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                MaxDepth = 120,
            };

            jsonSerializerOptions.Converters.Add(new BaseKeyItemJsonConverter1());

            //BaseKeyItemJsonConverter1 will be used on obj1
            //BaseKeyItemJsonConverter2 will not be used even for obj2, which will use system default
            //So obj2str = obj3str
            jsonSerializerOptions.Converters.Add(new BaseKeyItemJsonConverter2());  //BaseKeyItemJsonConverter2 itself is good, if you move it up, it will be used

            var obj1 = new KeyItem() { Id = 1 };
            var obj1Str = JsonSerializer.Serialize(obj1, jsonSerializerOptions);
            Console.WriteLine(obj1Str);

            var obj2 = new KeyItem() { Id = 2 };
            var obj2Str = JsonSerializer.Serialize(obj2, jsonSerializerOptions);
            Console.WriteLine(obj2Str);

            var obj3 = new KeyItem() { Id = 3 };
            var obj3Str = JsonSerializer.Serialize(obj3, jsonSerializerOptions);
            Console.WriteLine(obj3Str);

        }

    }
}
