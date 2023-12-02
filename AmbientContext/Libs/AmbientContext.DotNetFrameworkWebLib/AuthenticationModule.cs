using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using AmbientContext.Shared.DotNetStandardLib;


namespace AmbientContextDotNetFrameworkWebLib
{
    internal class AuthenticationModule :IHttpModule
    {
        public void Init(HttpApplication context)
        {
            #region chose either one
            //1, this one works for aspx file, but for MVC controller, still cannot get study back from store
            //context.AuthenticateRequest += Context_AuthenticateRequest;

            //2, this one will make Context_EndRequest cannot access data in ThreadDataStore, but always can access data in Thread.Principle 
            //no matter there is await code in AuthAsync() or not
            var wrapper = new EventHandlerTaskAsyncHelper(AuthAsync);
            context.AddOnAuthenticateRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
            #endregion

            context.EndRequest += Context_EndRequest;
        }

        private void Context_AuthenticateRequest(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var ctx = app.Context;

            if (!AuthHelper.IsMainRequest(ctx.Request.Url.AbsolutePath)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

            AuthHelper.SetThreadData(data);

            ctx.User = Thread.CurrentPrincipal;

            TestHelper.Verify(ctx);
        }

        async Task AuthAsync(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var ctx = app.Context;

            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            Verifier.Assert(HttpContext.Current == null, "After async call, HttpContext.Current should be null but not.");

            if (!AuthHelper.IsMainRequest(ctx.Request.Url.AbsolutePath)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

            AuthHelper.SetThreadData(data);

            await AsyncActor.DoSthAsync().ConfigureAwait(false);
            ctx.User = Thread.CurrentPrincipal;

            TestHelper.Verify(ctx);
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            var notVerifyAtEnd = context.Request.QueryString.AllKeys.Contains("notVerifyAtEndRequest");

            if (notVerifyAtEnd)
                return;  // for tests that try to change study
            else 
                TestHelper.Verify(context); 
        }

        public void Dispose()
        {
            //do nothing
        }

      
    }
}
