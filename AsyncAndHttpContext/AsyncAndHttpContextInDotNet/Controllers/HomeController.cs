﻿using System;
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

            HttpContextPrinter.PrintHttpContext("Right after await");

            await Task.Run(() =>
            {
                HttpContextPrinter.PrintHttpContext("Inside task, before a call");
                DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().GetAwaiter().GetResult();
                HttpContextPrinter.PrintHttpContext("Inside task, after a call");
            });
            HttpContextPrinter.PrintHttpContext("Right after Task");

            await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().ConfigureAwait(true);
            HttpContextPrinter.PrintHttpContext("After await which ConfigureAwait is true");

            await DoAsyncWork.GetUrlContentAsyncNoConfigureAwait().ConfigureAwait(false);
            HttpContextPrinter.PrintHttpContext("After await which ConfigureAwait is false");

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