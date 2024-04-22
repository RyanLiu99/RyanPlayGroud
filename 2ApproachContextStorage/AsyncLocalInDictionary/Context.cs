using System.Collections.Concurrent;


public static class CallContext
{
    private static ConcurrentDictionary<string, AsyncLocal<object>> state = new ConcurrentDictionary<string, AsyncLocal<object>>();


    public static void LogicalSetData(string name, object data) =>
        state.GetOrAdd(name, _ => new AsyncLocal<object>()).Value = data;


    public static object LogicalGetData(string name) =>
        state.TryGetValue(name, out System.Threading.AsyncLocal<object> data) ? data.Value : null;
}
