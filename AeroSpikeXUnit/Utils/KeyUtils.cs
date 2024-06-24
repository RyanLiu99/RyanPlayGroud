using Aerospike.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AeroSpikeXUnit.Utils
{
    public static class KeyUtils
    {
        public static Key[] GetKeys<TUserKey>(string nameSpace, string setName, params TUserKey[] userKeys)
        {
            return userKeys?.Select(x => new Key(nameSpace, setName, Value.Get(x))).ToArray() ?? Array.Empty<Key>();
        }
    }
}
