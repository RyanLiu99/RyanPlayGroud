using System;
using System.Reflection;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmallTests.Entities
{
    public class EntityDependencyJsonConverterFactory : JsonConverterFactory
    {
        public override bool CanConvert(Type typeToConvert)
        {
            var t = typeof(EntityDependency);
            return typeToConvert == t || typeToConvert.IsSubclassOf(t);
        }

        public override JsonConverter? CreateConverter(Type typeToConvert, JsonSerializerOptions options)
        {
            var typeParameters = typeToConvert.GetGenericArguments();

            Type? convertType = null;
            if (typeParameters.Length == 0)
            {
                convertType = typeof(EntityDependencyConverterInner);
            }
            else if (typeParameters.Length == 1 || typeParameters.Length == 2)
            {
                convertType = typeof(EntityDependencyConverterInner<>).MakeGenericType(
                    new Type[] { typeParameters[0] });
            }
            else
            {
                throw new SerializationException(
                    "EntityDependency has more than 2 type parameters which is not supported.");
            }

            JsonConverter converter = (JsonConverter)Activator.CreateInstance(
                convertType!,
                BindingFlags.Instance | BindingFlags.Public,
                binder: null,
                args: new object[] { options },
                culture: null)!;
            return converter;
        }

        private class EntityDependencyConverterInner : JsonConverter<EntityDependency>
        {

          
            public EntityDependencyConverterInner(JsonSerializerOptions options)
            {
                    
            }
            public override EntityDependency? Read(ref Utf8JsonReader reader, Type typeToConvert,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, EntityDependency value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        private class EntityDependencyConverterInner<TId> : JsonConverter<EntityDependency<TId>>
        {
            public override EntityDependency<TId>? Read(ref Utf8JsonReader reader, Type typeToConvert,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }

            public override void Write(Utf8JsonWriter writer, EntityDependency<TId> value,
                JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }

        //private class EntityDependencyConverterInner<TEntity, TId> : JsonConverter<EntityDependency<TEntity, TId>>
        //{
        //    public override EntityDependency<TEntity, TId>? Read(ref Utf8JsonReader reader, Type typeToConvert,
        //        JsonSerializerOptions options)
        //    {
        //        throw new NotImplementedException();
        //    }

        //    public override void Write(Utf8JsonWriter writer, EntityDependency<TEntity, TId> value,
        //        JsonSerializerOptions options)
        //    {
        //        throw new NotImplementedException();
        //    }
        //}
    }
}
