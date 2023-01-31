using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace SmallTests.Entities
{
    public struct DynamicTuple : ITuple, IEquatable<DynamicTuple>, IEquatable<ITuple>
    {
        private dynamic[] _values; //cannot be object[], it will cause boxing and equals will compare object references

        public DynamicTuple(IEnumerable<dynamic> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            _values = values.ToArray();

            if (_values.Length < 2)
                throw new ArgumentException("At least 2 values to construct composite data.", nameof(values));
        }

        public object? this[int index] => _values[index];

        public int Length => _values.Length;


        public override bool Equals(object? obj)
        {
            return obj is DynamicTuple && Equals((DynamicTuple)obj);
        }

        public bool Equals(DynamicTuple other)
        {
            if (Length != other.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (_values[i] != other._values[i])
                {
                    return false;
                }
            }

            return true;
        }

        public bool Equals(ITuple? other)
        {
            if (Length != other.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (_values[i] != other[i])
                {
                    return false;
                }
            }
            return true;
        }


        public override int GetHashCode()
        {
            var finalResult = _values.Aggregate(0, (result, next) => HashCode.Combine(result, next?.GetHashCode() ?? 0));
            return finalResult;
        }
    }
}
