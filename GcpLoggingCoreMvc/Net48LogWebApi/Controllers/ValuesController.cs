using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Reflection;
using System.Text;
using System.Web.Hosting;
using System.Web.Http;
using System.Web.Http.Results;
using System.Web.Mvc;

namespace Net48LogWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public JsonResult<AppInfoResult> Get()
        {
            var obj = new AppInfoResult
            {
                ExecutingAssemblyName = Assembly.GetExecutingAssembly().GetName().Name, //"Net48LogWebApi",
                EntryAssemblyName = Assembly.GetEntryAssembly()?.GetName().Name, //null
                AppHostSiteName = HostingEnvironment.ApplicationHost.GetSiteName(), //"Net48LogWebApi"
                HostEnvSiteName = HostingEnvironment.SiteName //preferred . "Net48LogWebApi"
            };

            return Json(obj);
            
            //return new JsonResult()
            //{
            //    Data = obj,
            //    ContentType = "text/json",
            //    JsonRequestBehavior = JsonRequestBehavior.AllowGet
            //};
        }

        // GET api/values/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }

    public class AppInfoResult
    {

        public string ExecutingAssemblyName { get; set; }
        public string EntryAssemblyName { get; set; }
        public string AppHostSiteName { get; set; }
        public string HostEnvSiteName { get; set; }
    }

}
