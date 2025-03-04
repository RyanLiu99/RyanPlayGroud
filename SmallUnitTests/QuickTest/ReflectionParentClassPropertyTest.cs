using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    public class ReflectionParentClassPropertyTest
    {
        [Test]
        public void GetPropertyWillReturnParentClassProperty()
        {
            Assert.True(typeof(ChildClass).IsSubclassOf(typeof(ParentClass)));

            var childProperties = typeof(ChildClass).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            Assert.True(childProperties.Any(p => p.Name == "ChildProperty"));
            Assert.True(childProperties.Any(p => p.Name == "ParentProperty"));

            //FlattenHierarchy only works for public and protected static, so no effect here
            var childPropertiesFlatten = typeof(ChildClass).GetProperties(BindingFlags.Public | BindingFlags.Instance|BindingFlags.FlattenHierarchy);
            Assert.That(childPropertiesFlatten.Length, Is.EqualTo(childProperties.Length));
        }       
    }

    internal class ParentClass
    {        
        public string ParentProperty { get; set; }
    }

    internal class ChildClass : ParentClass
    {
        public string ChildProperty { get; set; }
    }
}
