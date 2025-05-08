using System.Text.Encodings.Web;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Collections.Generic;
using System;

namespace TestProjectForNet48
{
    public static class PreDefinedOptions
    {
        public static int MaxSize = int.MaxValue;
        public static JsonSerializerOptions Default { get; private set; }

        private static Dictionary<Type, JsonSerializerOptions> _optionsExcludeConveter  = new Dictionary<Type, JsonSerializerOptions>();

        static PreDefinedOptions()
        {
            var prototype = new JsonSerializerOptions
            {
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = false,
                MaxDepth = 120,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };


            Default = new JsonSerializerOptions(prototype);
            Default.Converters.Add(new BaseKeyItemJsonConverter());


            var excludeKeyItem = new JsonSerializerOptions(prototype);
            excludeKeyItem.Converters.Add(new BaseKeyItemJsonConverter(typeof(KeyItem)));
            _optionsExcludeConveter[typeof(KeyItem)] = excludeKeyItem;


            var excludeKeyItem2 = new JsonSerializerOptions(prototype);
            excludeKeyItem2.Converters.Add(new BaseKeyItemJsonConverter(typeof(KeyItem2)));
            _optionsExcludeConveter[typeof(KeyItem2)] = excludeKeyItem2;

        }

        public static JsonSerializerOptions GetOptions(Type excludedType) => _optionsExcludeConveter[excludedType];
       

    }



   
}
