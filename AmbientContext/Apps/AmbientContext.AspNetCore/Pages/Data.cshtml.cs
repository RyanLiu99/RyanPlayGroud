using AmbientContext.Shared.DotNetStandardLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AmbientContext_AspNetCore.Pages
{
    [IgnoreAntiforgeryToken]
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;


        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public Task OnGetAsync()
        {
            return Task.CompletedTask;
        }

        [IgnoreAntiforgeryToken]
        public async Task<IActionResult> OnPostAsync()
        {
            //Read from Thread, double it and put it back
            var old = AuthHelper.GetCurrentStudyIdFromThread();
            await AuthHelper.OverwriteThreadStudyIdInManualTask(old * 2).ConfigureAwait(false);
            return Page();
        }
    }
}