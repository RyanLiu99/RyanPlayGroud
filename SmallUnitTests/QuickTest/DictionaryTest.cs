using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    internal class DictionaryTest
    {
        [Test]
        public void IntIsObject()
        {
            Dictionary<string, int> stringInt = new Dictionary<string, int>();
            stringInt.Add("a", 1);

            Assert.IsTrue(stringInt is Dictionary<string, int>);
            Assert.IsTrue(stringInt is IDictionary<string, int>);

            Assert.IsFalse(stringInt is Dictionary<string, long>);
            Assert.IsFalse(stringInt is Dictionary<string, object>);
            Assert.IsFalse(stringInt is Dictionary<object, object>);

            Assert.IsFalse(stringInt is IDictionary<string, long>);
            Assert.IsFalse(stringInt is IDictionary<string, object>);
            Assert.IsFalse(stringInt is IDictionary<object, object>);

        }
    }
}
