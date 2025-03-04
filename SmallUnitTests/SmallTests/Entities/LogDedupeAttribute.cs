using System;

namespace Ringba.Models.DataModelAttributes
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface | AttributeTargets.Struct, AllowMultiple = false, Inherited = true)]
    public class LogDedupeAttribute : Attribute
    {
        private readonly string _idProperty;

        /// <summary>
        /// the property name for the property that contains the ID or away to diffentiat the item from
        /// other items of the same type
        /// </summary>
        public string IdProperty => _idProperty;

        public LogDedupeAttribute(string idProperty = "Id")
        {
            _idProperty = idProperty;
        }
    }
}
