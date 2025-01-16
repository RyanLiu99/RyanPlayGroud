using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTest
{
    class DescriptionAttribute<T> : Attribute
    {
        T _v;
        public DescriptionAttribute(T v)
        {
            _v = v;
        }

        public T Description => _v;
    }
}
