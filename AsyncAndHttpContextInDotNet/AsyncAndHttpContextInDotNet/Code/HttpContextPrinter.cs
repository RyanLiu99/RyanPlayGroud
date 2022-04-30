using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace AsyncAndHttpContextInDotNet.Code
{
    public static class HttpContextPrinter
    {
        private static StringBuilder sb = new StringBuilder();
        internal static void PrintHttpContext()
        {
            if(HttpContext.Current == null) 
                sb.AppendLineHtml("HttpContext.Current is null");
            else
                sb.AppendLineHtml($"HttpContext.Current is valid, and AllowAsyncDuringSyncStages is { HttpContext.Current.AllowAsyncDuringSyncStages}");
        }


        internal static string Result()
        {
            var result = sb.ToString();
            sb.Clear();
            return result;
        }

        private static StringBuilder AppendLineHtml(this StringBuilder sb, string str)
        {
            return  sb.AppendLine(str.Format());
        }

        private static string Format(this string str)
        {
            return $"\r\n{str}\r\n";
        }

    }
}