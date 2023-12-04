using System.Collections.Concurrent;
using System.Threading;

namespace AmbientContext.Shared.DotNetStandardLib.ThreadDataStores
{
    internal class AsyncLocalThreadDataStore : IImpThreadDataStore
    {
        private readonly AsyncLocal<ConcurrentDictionary<string, object>> _asyncLocalDictionary = new();

        public void AddObject(string key, object value)
        {
            if (_asyncLocalDictionary.Value == null)
            {
                _asyncLocalDictionary.Value = new();
            }

            _asyncLocalDictionary.Value[key] = value;
        }

        public object GetObject(string key)
        {
            return _asyncLocalDictionary.Value?[key];
        }


        public bool HasKey(string key) => _asyncLocalDictionary.Value?.ContainsKey(key) ?? false;

        public bool ClearObject(string key) => _asyncLocalDictionary.Value?.TryRemove(key, out _) ?? false;
    }
}
