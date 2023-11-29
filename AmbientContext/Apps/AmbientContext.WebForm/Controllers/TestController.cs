using System.Threading.Tasks;
using System.Web.Mvc;
using AmbientContext.Shared.DotNetStandardLib;


namespace AmbientContextWebForm.Controllers
{
    public class TestController : Controller
    {
        // GET: Test
        public async Task<ActionResult> Index()
        {
            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            return View();
        }
    }
}