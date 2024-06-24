using Aerospike.Client;
using Neo.IronLua;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroSpikeXUnit.Utils
{
    public class MapUtils
    {
        private readonly AsyncClient _client;

        public MapUtils(AsyncClient client)
        {
            this._client = client;
        }

        /// <summary>
        /// Get operations to drop map entries(if exist) from map bins
        /// </summary>
        /// <typeparam name="TId">type of map key, like string or intger</typeparam>
        /// <param name="binAndMapIdsArray">A dictionarty whose key is binName and value is array of map Ids</param>
        /// <returns>Operation array can be executed to drop map entries</returns>
        public static Operation[]? GetOpsToRemoveMapEntriesIfExists<TId>(Dictionary<string, TId[]> binAndMapIdsArray)
        {
            return binAndMapIdsArray?.SelectMany(binMapKeys =>
            GetOpsToRemoveMapEntriesIfExists(binMapKeys.Value.Select(mapKey => (binMapKeys.Key, mapKey)).ToArray())
            ).Where(x => x != null).ToArray();
        }

        public static Operation[]? GetOpsToRemoveMapEntriesIfExists<TId>(params (string binName, TId mapKey)[] binAndMapIdArray)
        {
            return binAndMapIdArray?.Select(x =>
                  MapOperation.RemoveByKey(x.binName, Value.Get(x.mapKey), MapReturnType.NONE)
              ).ToArray();
        }

        public Task<Aerospike.Client.Record[]> Exec(Key[] keys, Operation[]? ops, CancellationToken token = default)
        {            
            return Task.WhenAll(keys.Select( key => _client.Operate(new WritePolicy(), token, key, ops)));
        }

        public Task<Aerospike.Client.Record[]> RemoveMapEntriesIfExists<TId>(Key[] keys, Dictionary<string, TId[]> binAndMapIdsArray, CancellationToken token = default)
        {
            var ops = GetOpsToRemoveMapEntriesIfExists(binAndMapIdsArray);
            return Exec(keys, ops, token);
        }

        #region async version, might need upgrade server 

        /// <summary>
        /// Note: Need server support batch write (6.0+)
        /// </summary>
        public Task<BatchResults> ExecBatch(Key[] keys, Operation[] ops, CancellationToken token = default)
        {
            BatchWritePolicy batchWritePolicy = new BatchWritePolicy() { };
            return _client.Operate(BatchPolicy.WriteDefault(), batchWritePolicy, token, keys, ops);
        }

        /// <summary>
        /// Note: Need server support batch write (6.0+)
        /// </summary>
        public Task<BatchResults> RemoveMapEntriesIfExistsBatch<TId>(Key[] keys,  Dictionary<string, TId[]> binAndMapIdsArray, CancellationToken token = default)
        {
            return ExecBatch(keys, GetOpsToRemoveMapEntriesIfExists(binAndMapIdsArray), token);
        }

      
        #endregion
    }

}
