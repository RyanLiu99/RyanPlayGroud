using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace TestProjectForNet48
{
    public static class JsonHelper
    {
        public static JsonSerializerOptions CloneExcludeConverterType(this JsonSerializerOptions options, params Type[] converterTypesToSkip)
        {
            var newOptions = new JsonSerializerOptions(options);

            if (converterTypesToSkip != null && converterTypesToSkip.Length > 0)
            {
                foreach (var c in options.Converters.Where(c => converterTypesToSkip.Contains(c.GetType())))
                {
                    newOptions.Converters.Remove(c);
                }
            }

            return newOptions;
        }

        public static JsonSerializerOptions CloneExlcudeConverter(this JsonSerializerOptions options, params JsonConverter[] convertersToSkip)
        {
 
            var newOptions = new JsonSerializerOptions(options);

            if(convertersToSkip != null)
                foreach(var c in convertersToSkip)
                    newOptions.Converters.Remove(c);    
            
            return newOptions;
        }
    }
}
