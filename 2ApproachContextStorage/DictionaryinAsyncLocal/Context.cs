using System.Collections.Concurrent;


public static class CallContext
{
     private static readonly AsyncLocal<ConcurrentDictionary<string, object>> _asyncLocalDictionary = new();

       public static void LogicalSetData(string key, object value)
       {
           if (_asyncLocalDictionary.Value == null)
           {
               _asyncLocalDictionary.Value = new();
           }

           _asyncLocalDictionary.Value[key] = value;
       }

       public static object LogicalGetData(string key)
       {
           return _asyncLocalDictionary.Value?[key];
       }

}
