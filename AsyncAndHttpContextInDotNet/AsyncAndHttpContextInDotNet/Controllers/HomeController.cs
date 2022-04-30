using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using AsyncAndHttpContextInDotNet.Code;

namespace AsyncAndHttpContextInDotNet.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public async Task<ActionResult> Test()
        {
            var content = await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait();

            HttpContextPrinter.PrintHttpContext();
            ViewBag.Message = HttpContextPrinter.Result() + "\r\n" + content;
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}