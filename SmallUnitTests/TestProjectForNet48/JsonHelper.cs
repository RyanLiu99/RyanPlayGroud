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
            var newOptions = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = options.IgnoreReadOnlyProperties,
                AllowTrailingCommas = options.AllowTrailingCommas,
                DefaultBufferSize = options.DefaultBufferSize,
                DictionaryKeyPolicy = options.DictionaryKeyPolicy,
                MaxDepth = options.MaxDepth,
                Encoder = options.Encoder,
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
               PropertyNamingPolicy = options.PropertyNamingPolicy,
                ReadCommentHandling = options.ReadCommentHandling,
                WriteIndented = options.WriteIndented
            };

            foreach (var c in options.Converters.Where(c => converterTypesToSkip == null || converterTypesToSkip.Length == 0
            || !converterTypesToSkip.Contains(c.GetType())))
                newOptions.Converters.Add(c);

            return newOptions;
        }

        public static JsonSerializerOptions CloneExlcudeConverter(this JsonSerializerOptions options, params JsonConverter[] convertersToSkip)
        {
            var newOptions = new JsonSerializerOptions
            {
                IgnoreReadOnlyProperties = options.IgnoreReadOnlyProperties,
                AllowTrailingCommas = options.AllowTrailingCommas,
                DefaultBufferSize = options.DefaultBufferSize,
                DictionaryKeyPolicy = options.DictionaryKeyPolicy,
                MaxDepth = options.MaxDepth,
                Encoder = options.Encoder,
                PropertyNameCaseInsensitive = options.PropertyNameCaseInsensitive,
                PropertyNamingPolicy = options.PropertyNamingPolicy,
                ReadCommentHandling = options.ReadCommentHandling,
                WriteIndented = options.WriteIndented
            };

            foreach (var c in options.Converters.Where(c => convertersToSkip == null || convertersToSkip.Length == 0
            || !convertersToSkip.Contains(c)))
                newOptions.Converters.Add(c);

            return newOptions;
        }
    }
}
