using System.Text;
using Microsoft.AspNetCore.Http;

namespace AsyncAndHttpContextInCore.Code
{
    public  class HttpContextPrinter
    {
        private  readonly StringBuilder Sb = new StringBuilder();
        private readonly IHttpContextAccessor _accessor;

        public HttpContextPrinter(IHttpContextAccessor accessor)
        {
            this._accessor = accessor;
        }

        internal  StringBuilder AppendLineHtml( StringBuilder sb, string str)
        {
            return sb.AppendLine(Format(str));
        }

        internal  string Format( string str)
        {
            return $"\r\n{str}";
        }

        internal  void PrintHttpContext(string step)
        {
            HttpContext context = _accessor.HttpContext;
            if (context == null)
                AppendLineHtml(Sb, $"{step} -- _accessor.HttpContext is null");
            else
                AppendLineHtml(Sb, $"{step} -- _accessor.HttpContext is valid.");
        }


        internal  string Result()
        {
            var result = Sb.ToString() + "\r\n";
            Sb.Clear();
            return result;
        }
    }

}