﻿using System;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using System.Web;
using Medrio.ActivityAuditLog;
using Medrio.ActivityAuditLog.Gcp;
using HttpContext = System.Web.HttpContext;

namespace Net48LogWebApi
{
    public class ActivityAuditModule : IHttpModule
    {
        private readonly IActivityAuditLogger _activityAuditLogger = new ActivityAuditLoggerGcp(new GcpLogRequestBuilder());

        /// <summary>
        /// You will need to configure this module in the Web.config file of your
        /// web and register it with IIS before being able to use it. For more information
        /// see the following link: https://go.microsoft.com/?linkid=8101007
        /// </summary>
        #region IHttpModule Members

        public void Dispose()
        {
            //clean-up code here.
        }

        public void Init(HttpApplication app)
        {
            // Below is an example of how you can handle LogRequest event and provide 
            // custom logging implementation for it
            //app.LogRequest += new EventHandler(OnLogRequest);

            //app.PostAuthenticateRequest += App_PostAuthenticateRequest;
            app.PostMapRequestHandler += (sender, e) => PostMapRequestHandler();

        }

        [HandleProcessCorruptedStateExceptions]
        public void PostMapRequestHandler()
        {
            Log();
        }

        //private void App_PostAuthenticateRequest(object sender, EventArgs e)
        //{
        //}

        #endregion

        //public void OnLogRequest(Object source, EventArgs e)
        //{
        //    //custom logging logic can go here

        //}

        private string Log()
        {            
           // IHttpContextAccessor accessor = new HttpContextAccessor(); //this is not work, just return null 
           var result = _activityAuditLogger.WriteLog(HttpContext.Current);
            return result.Result;
           //throw new NotImplementedException();
        }
    }
}
