using System;
using System.Text.Json;

namespace ConsoleApp1.Utilities
{

    public static class LogUtilities
    {
        /// <summary>
        /// Return JSON representation of <paramref name="obj"/>. If fail to Serialize, return obj.ToString().
        /// Only intent for debug and logging purpose.
        /// </summary>        
        public static string ToDebugString<T>(this T obj)
        {
            if (obj == null) return string.Empty;
            try
            {
                return JsonSerializer.Serialize(obj);
            }
            catch (Exception e)
            {
                return $"[Fail to JSON] {obj}\r\n{e}";
            }
        }
    }
}
