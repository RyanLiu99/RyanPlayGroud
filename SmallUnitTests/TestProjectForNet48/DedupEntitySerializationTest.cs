using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;


namespace TestProjectForNet48
{
    public class DedupEntitySerializationTest
    {     
        [Test]
        /*
         *   Standard Output, 
            {"Id":1}
            {"Id":2,"Converter":2}
            {"Id":3}
         */
        public void PrintSerializationResultNet48()
        {
            var jsonSerializerOptions = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                MaxDepth = 120,
            };

            jsonSerializerOptions.Converters.Add(new BaseKeyItemJsonConverter(2));

            jsonSerializerOptions.Converters.Add(new BaseKeyItemJsonConverter(3));

            var obj1 = new KeyItem() { Id = 1 };
            var obj1Str = JsonSerializer.Serialize(obj1, jsonSerializerOptions);
            Console.WriteLine(obj1Str);

            var obj2 = new KeyItem() { Id = 2 };
            var obj2Str = JsonSerializer.Serialize(obj2, jsonSerializerOptions.CloneExlcudeConverter());
            Console.WriteLine(obj2Str);

            var obj3 = new KeyItem() { Id = 3 };
            var obj3Str = JsonSerializer.Serialize(obj3, jsonSerializerOptions.CloneExcludeConverterType());
            Console.WriteLine(obj3Str);


        }

    }
}
