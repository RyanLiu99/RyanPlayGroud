﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;


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

        public dynamic this[int index] => _values[index];

        public int Length => _values.Length;


        public override bool Equals(object? obj)
        {
            if (obj is DynamicTuple && Equals((DynamicTuple)obj))
                return true;

            return obj is ITuple && Equals((ITuple)obj);
        }

        public bool Equals(DynamicTuple other)
        {
            if (Length != other.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (_values[i] != other[i])  //This won't cast to ITupe's   object? this[int index] { get; }
                {
                    return false;
                }
            }
            return true;
        }

        public bool Equals(ITuple? other)
        {
            if(other == null) return false;

            if (Length != other.Length) return false;

            for (int i = 0; i < Length; i++)
            {
                if (!_values[i].Equals(other[i])) //do not use  != which is by reference 
                {
                    return false;
                }
            }
            return true;
        }

        public static bool operator ==(DynamicTuple x, ITuple y) => x.Equals(y);
        public static bool operator !=(DynamicTuple x, ITuple y) => !x.Equals(y);

        public static bool operator ==(ITuple y, DynamicTuple x) => x.Equals(y);
        public static bool operator !=(ITuple y, DynamicTuple x) => !x.Equals(y);


        public override int GetHashCode()
        {
            var finalResult = _values.Aggregate(0, (result, next) => HashCode.Combine(result, next?.GetHashCode() ?? 0));
            return finalResult;
        }

        public string ToDebugString()
        {
            return string.Join('|', _values);
        }
    }
}
