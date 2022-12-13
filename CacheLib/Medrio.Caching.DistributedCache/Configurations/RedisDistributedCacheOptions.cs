using Medrio.Infrastructure.Ioc.ConfigurationOption;

namespace Medrio.Caching.RedisDistributedCache.Configurations
{
    internal class RedisDistributedCacheOptions : AutoBindOption<RedisDistributedCacheOptions>
    {
        public override string SectionName => "Medrio:RedisCache";

        /// <summary>
        /// The Redis instance name. Allows partitioning a single backend cache for use with multiple apps/services.
        /// If set, the cache keys are prefixed with this value.
        /// </summary>
        public string? InstanceName { get; set; }


        public string MessageBrokerIdentifier { get; set; }
    }
}
