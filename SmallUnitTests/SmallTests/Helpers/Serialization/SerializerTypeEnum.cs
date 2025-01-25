namespace SmallTests.Helpers.Serialization
{
    public enum SerializerTypeEnum
    {
        MessagePack,
        NewtonSoft,
        DataContract
    }

    public static class SerializerTypeSources
    {
        public static SerializerTypeEnum[] Types =>

            new SerializerTypeEnum[]
            {
                SerializerTypeEnum.MessagePack,
                SerializerTypeEnum.NewtonSoft,
            };
    }
}
