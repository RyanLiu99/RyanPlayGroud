using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElasticSearchConsole1
{
    public static class DebugHelper
    {
        public static string GetErrorInfo(this IResponse result)
        {
            if (!result.IsValid)
            {                
                var errorMessage = new StringBuilder();

                if (result.TryGetServerErrorReason(out var serverError))
                {
                    errorMessage.Append(serverError).Append(". ");                
                }

                errorMessage.Append(result.DebugInformation);  //It contais OriginalException which is client side error and URI

                return errorMessage.ToString();
            }
            else
            {
                return string.Empty;
            }
        }
    }
}
