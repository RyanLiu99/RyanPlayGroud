using SimpleInjector.Lifestyles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TestSimpleInjector.Services;

namespace TestSimpleInjector.Controllers
{
    public class HomeController : Controller
    {
        private int i = 0;

        public HomeController()
        {
            
        }
        public ActionResult Index()
        {
            var locationService = MvcApplication.TheContainer.GetInstance<ILocationService>(); //How Medrio does, it does respect scope, container is smart
            ILocationService locationService2 = null;
            ILocationService locationService3 = null;

            using (var scope = AsyncScopedLifestyle.BeginScope(MvcApplication.TheContainer))
            {
                locationService2 = MvcApplication.TheContainer.GetInstance<ILocationService>();
                locationService3= scope.GetInstance<ILocationService>();
            }

            //return Content(locationService.GetCityByZip(i++));
            //return Content("S1 and S2 are same " + (locationService == locationService2)); //true
            return Content("S2 and S3 are same " + (locationService2 == locationService3)); //false  
            //Above prove you have to use returned scope. Simple BeginScope does not set ambient scope.
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
    }
}