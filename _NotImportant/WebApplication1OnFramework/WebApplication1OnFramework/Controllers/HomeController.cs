using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.WebSockets;

namespace WebApplication1OnFramework.Controllers
{
    public class HomeController : Controller
    {
        private static List<string> _reports = new List<string>();
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }


        
        public async Task<ActionResult> Report()
        {
            Request.InputStream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(Request.InputStream)) 
            {
                var result = await reader.ReadToEndAsync();

                _reports.Add(result);
                return new EmptyResult();
            }
        }


        public ActionResult Reports()
        {
            return new ContentResult() { Content = "<pre>" +  string.Join("<br/><br/>", _reports) + "</pre>" };
        }

        public ActionResult Reset()
        {
          _reports.Clear();
          return new EmptyResult();
        }
    }
}