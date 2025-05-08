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
     
        public void PrintSerializationResultNet48()
        {

            var options = PreDefinedOptions.Default;

            var child1 = new KeyItem() { Id = 1, generation = 1, Name = "child1" };
            var child2 = new KeyItem2() { Id = 1,  Name = "child2" };

            //BaseKeyItemJsonConverter will be used on obj1, child will not be printe due to gen
            //     obj1: {"Id":1,"Name":null,"_t_gen":1,"ExcludeType":null}
            var obj1 = new KeyItem() { Id = 1, generation =1, Child1 = child1, Child2 = child2 };  
            var obj1Str = JsonSerializer.Serialize(obj1, options);
            Console.WriteLine("obj1: " + obj1Str);

            //Obj2: system converter since no gen
            //child 1:  BaseKeyItemJsonConverter(exclude KeyItem2 which is type of Obj2 ) will be used since child 1 has gen
            //child 2: no gen, systm default used 

            /*
             * {
                  "Id": 2,

                  "Child1": {
                    "Id": 1,
                    "Name": "child1",
                    "_t_gen": 1,
                    "ExcludeType": "KeyItem2"
                  },

                  "Child2": {
                    "Id": 1,
                    "Name": "child2"
                  }
                }
             * */

            var obj2 = new KeyItem2() { Id = 2 , Child1 = child1, Child2 = child2 };
            var obj2Str = JsonSerializer.Serialize(obj2, options);
            Console.WriteLine("obj2: " + obj2Str);


            //has gen
            //   obj3: {"Id":3,"Name":null,"_t_gen":3,"ExcludeType":null}
            var obj3 = new KeyItem() { Id = 3, generation =3 };
            var obj3Str = JsonSerializer.Serialize(obj3, options);
            Console.WriteLine("obj3: " + obj3Str);


            //    obj4: {"Id":4}, no gen
            var obj4 = new KeyItem2() { Id = 4 };  
            var obj4Str = JsonSerializer.Serialize(obj4, options);
            Console.WriteLine("obj4: " + obj4Str);
        }

    }
}
