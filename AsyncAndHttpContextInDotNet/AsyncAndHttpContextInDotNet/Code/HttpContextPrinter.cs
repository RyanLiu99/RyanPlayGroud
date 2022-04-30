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
        internal static void PrintHttpContext(string step)
        {
            if(HttpContext.Current == null) 
                sb.AppendLineHtml($"{step} -- HttpContext.Current is null");
            else
                sb.AppendLineHtml($"{step} -- HttpContext.Current is valid, and AllowAsyncDuringSyncStages is { HttpContext.Current.AllowAsyncDuringSyncStages}");
        }


        internal static string Result()
        {
            var result = sb.ToString() + "\r\n";
            sb.Clear();
            return result;
        }

        private static StringBuilder AppendLineHtml(this StringBuilder sb, string str)
        {
            return  sb.AppendLine(str.Format());
        }

        private static string Format(this string str)
        {
            return $"\r\n{str}";
        }

    }
}