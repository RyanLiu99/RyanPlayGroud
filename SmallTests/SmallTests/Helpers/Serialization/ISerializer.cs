using SmallTests.Entities;

namespace SmallTests.Helpers.Serialization
{
    public interface ISerializer<T>
    {
        T SerializeDeSerialize(T input);

        ValueDependencies SerializeDeSerializeTuple(ValueDependencies input);
    }
}
