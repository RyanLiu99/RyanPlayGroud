using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;


namespace SmallTests.Entities
{
    public class CompositeData : ITuple, IEquatable<CompositeData>
    {
        private object[] _values;

        public CompositeData(IEnumerable<object> values)
        {
            if (values == null) throw new ArgumentNullException(nameof(values));
            _values = values.ToArray();

            if (_values.Length < 2)
                throw new ArgumentException("At least 2 values to construct composite data.", nameof(values));
        }

        public object? this[int index] => this._values[index];

        public int Length => this._values.Length;

        public override bool Equals(object? obj)
        {
            return obj is CompositeData && Equals((CompositeData)obj);
        }

        public bool Equals(CompositeData? other)
        {
            if (object.ReferenceEquals(this, other)) return true;
            if (other == null) return false;

            if (this.Length != other.Length) return false;

            for (int i = 0; i < this.Length; i++)
            {
                if (this._values[i] != other._values[i])
                {
                    return false;
                }
            }

            return true;
        }

        public override int GetHashCode()
        {
            throw new NotImplementedException();
        }
    }
}
