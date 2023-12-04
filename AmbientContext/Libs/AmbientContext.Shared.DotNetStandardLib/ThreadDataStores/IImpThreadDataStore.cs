using System;
using System.Collections.Generic;
using System.Text;

namespace AmbientContext.Shared.DotNetStandardLib.ThreadDataStores
{
    internal interface IImpThreadDataStore
    {
        void AddObject(string key, object value);

        object GetObject(string key);
        
        
        bool HasKey(string key);

        bool ClearObject(string key);
    }
}
