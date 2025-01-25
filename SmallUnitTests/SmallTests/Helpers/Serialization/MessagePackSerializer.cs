using SmallTests.Entities;

namespace SmallTests.Helpers.Serialization
{
    public class MessagePackSerializer<T> : ISerializer<T>

    {
    public T SerializeDeSerialize(T input)
    {
        return SerializationHelper.WireUsingMessagePack(input);
    }

    public ValueDependencies SerializeDeSerializeTuple(ValueDependencies input)
    {
        return SerializationValueDependenciesHelper.WireUsingMessagePack(input);
    }
    }
}
