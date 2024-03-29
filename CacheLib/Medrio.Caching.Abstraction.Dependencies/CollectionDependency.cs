﻿using System;
using System.Collections;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;

namespace Medrio.Caching.Dependencies
{
    public class CollectionDependency  : IEquatable<CollectionDependency>
    {
        [JsonPropertyName("T")]
        public string CollectionItemTypeName { get; }

        public CollectionDependency(string collectionItemTypeName)
        {
            if (string.IsNullOrEmpty(collectionItemTypeName))
            {
                throw new ArgumentException(nameof(collectionItemTypeName));
            }

            CollectionItemTypeName = collectionItemTypeName;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            if (ReferenceEquals(this, obj))
            {
                return true;
            }

            if (obj.GetType() != this.GetType())
            {
                return false;
            }

            return Equals((CollectionDependency)obj);
        }

        public override int GetHashCode()
        {
            return CollectionItemTypeName.GetHashCode();
        }

        public bool Equals(CollectionDependency? other)
        {
            if (ReferenceEquals(null, other))
            {
                return false;
            }

            if (ReferenceEquals(this, other))
            {
                return true;
            }

            return CollectionItemTypeName == other.CollectionItemTypeName;
        }

        public override string ToString()
        {
            return this.CollectionItemTypeName;
        }
    }

    public class CollectionDependency<T> : CollectionDependency
    {
        public CollectionDependency() : base(typeof(T).FullName)
        {
        }
    }
}