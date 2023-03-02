using AspNetMvc.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetMvc.Controllers
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }



        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CacheAny()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-DD HH:mm:ss.ffff"));
            //return View();
        }

        [ResponseCache(Duration = 120, Location = ResponseCacheLocation.Client, NoStore = false)]
        public IActionResult CacheClient()
        {
            return Content(DateTime.Now.ToString("yyyy-MM-DD HH:mm:ss.ffff"));
        }

        [ConditionalCacheAttribute(Duration = 120, Location = ResponseCacheLocation.Any, NoStore = false)]
        public IActionResult CacheConditional(string sgid)
        {
            return Content("Cache Conditional " + DateTime.Now.ToString("yyyy-MM-DD HH:mm:ss.ffff " ) + sgid);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
