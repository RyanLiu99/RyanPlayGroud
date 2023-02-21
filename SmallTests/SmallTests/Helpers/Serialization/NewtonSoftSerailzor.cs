using SmallTests.Entities;

namespace SmallTests.Helpers.Serialization
{
    public class NewtonSoftSerializer<T>  : ISerializer<T>
    {
        public T SerializeDeSerialize(T input)
        {
            return SerializationHelper.WireUsingNewton(input);
        }

        public ValueDependencies SerializeDeSerializeTuple(ValueDependencies input)
        {
            return SerializationValueDependenciesHelper.WireUsingNewton(input);
        }
    }
}
