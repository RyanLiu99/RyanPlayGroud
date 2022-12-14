using System;

namespace Medrio.Caching.Abstraction
{
    /// <summary>
    /// CacheEntryOptions like expiration.
    /// <remarks>System.Runtime.Caching does not support both absoluteExpiration and slidingExpiration,
    /// so no constructor to take both for now.</remarks>
    /// </summary>
    public class CacheEntryOptions
    {
        private TimeSpan? _slidingExpiration;

        /// <summary>
        /// Gets or sets an absolute expiration date for the cache entry.
        /// </summary>
        public DateTimeOffset? AbsoluteExpiration { get; }

    
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

        public CacheEntryOptions(DateTimeOffset? absoluteExpiration)
        {
            AbsoluteExpiration = absoluteExpiration;
        }

        public CacheEntryOptions(TimeSpan? slidingExpiration)
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
