using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Medrio.Caching.Abstraction
{
    public class CacheEntryOption
    {
        private TimeSpan? _slidingExpiration;

        /// <summary>
        /// Gets or sets an absolute expiration date for the cache entry.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; private set; }

    
        /// <summary>
        /// Gets or sets how long a cache entry can be inactive (e.g. not accessed) before it will be removed.
        /// This will not extend the entry lifetime beyond the absolute expiration (if set).
        /// </summary>
        public TimeSpan? SlidingExpiration
        {
            get => _slidingExpiration;
            private set
            {
                if (value <= TimeSpan.Zero)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(SlidingExpiration),
                        value,
                        "The sliding expiration value must be positive.");
                }
                _slidingExpiration = value;
            }
        }

        //System.Runtime.Caching does not support both absoluteExpiration and slidingExpiration, so no constructor to take both for now.

        public CacheEntryOption(DateTimeOffset? absoluteExpiration)
        {
            AbsoluteExpiration = absoluteExpiration;
        }

        public CacheEntryOption(TimeSpan? slidingExpiration)
        {
            SlidingExpiration = slidingExpiration;
        }


        //public TimeSpan? AbsoluteExpirationRelativeToNow
        //{
        //    set
        //    {
        //        if (value <= TimeSpan.Zero)
        //        {
        //            throw new ArgumentOutOfRangeException(
        //                nameof(AbsoluteExpirationRelativeToNow),
        //                value,
        //                "The relative expiration value must be positive.");
        //        }

        //        AbsoluteExpiration = DateTime.UtcNow + value;
        //    }
        //}

    }
}
