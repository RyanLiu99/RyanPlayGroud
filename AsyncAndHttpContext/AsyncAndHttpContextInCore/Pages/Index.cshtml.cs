using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AsyncAndHttpContextInCore.Code;
using AsyncAndHttpContextInDotNet.Code;

namespace AsyncAndHttpContextInCore.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly HttpContextPrinter _httpContextPrinter;

        public string Message { get; set; }

        public IndexModel(ILogger<IndexModel> logger, HttpContextPrinter httpContextPrinter)
        {
            _logger = logger;
            _httpContextPrinter = httpContextPrinter;
        }

        public async Task OnGet()
        {
            var content = await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait();

            _httpContextPrinter.PrintHttpContext("Right after await");

            await Task.Run(() =>
            {
                _httpContextPrinter.PrintHttpContext("Inside task, before a call");
                DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().GetAwaiter().GetResult();
                _httpContextPrinter.PrintHttpContext("Inside task, after a call");
            });
            _httpContextPrinter.PrintHttpContext("Right after Task");

            await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().ConfigureAwait(true);
            _httpContextPrinter.PrintHttpContext("After await which ConfigureAwait is true");

            await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().ConfigureAwait(false);
            _httpContextPrinter.PrintHttpContext("After await which ConfigureAwait is false");

            Message = _httpContextPrinter.Result() + "\r\n" + content;

            
        }
    }
}
