using System;

namespace SmallTests.Helpers.Serialization
{
    public static class SerializerFactory
    {
        public static ISerializer<T> GetSerializer<T>(SerializerTypeEnum type)
        {
            switch (type)
            {
                case SerializerTypeEnum.MessagePack:
                    return new MessagePackSerializer<T>();
                case SerializerTypeEnum.NewtonSoft:
                    return new NewtonSoftSerializer<T>();
                default:
                    throw new NotSupportedException($"SerializerTypeEnum type not supported: {type}");

            }
        }
    }
}
