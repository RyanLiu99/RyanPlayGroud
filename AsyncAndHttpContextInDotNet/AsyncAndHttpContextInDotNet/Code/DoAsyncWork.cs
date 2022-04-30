using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;

namespace AsyncAndHttpContextInDotNet.Code
{
    internal static class DoAsyncWork
    {
        static  HttpClient _httpClient = new HttpClient();
        internal static async Task<string> GetUrlContentAsyncNoConfigureAwait()
        {
            var result = await _httpClient.GetStringAsync("https://www.google.com/");
            return result;
        }

        internal static void Cleanup()
        {
            _httpClient.Dispose();
        }
    }
}