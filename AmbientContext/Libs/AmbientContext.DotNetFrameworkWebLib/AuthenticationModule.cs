using System;
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
            var wrapper = new EventHandlerTaskAsyncHelper(AuthAsync);
            context.AddOnAuthenticateRequestAsync(wrapper.BeginEventHandler, wrapper.EndEventHandler);
            context.EndRequest += Context_EndRequest;
        }


        async Task AuthAsync(object sender, EventArgs e)
        {
            var app = (HttpApplication)sender;
            var ctx = app.Context;

            await AsyncActor.DoSthAsync().ConfigureAwait(false);

            Verifier.Assert(HttpContext.Current == null, "After async call, HttpContext.Current should be null but not.");

            if (!Verifier.IsVerifyRequired(ctx.Request.Url.AbsolutePath)) return;

            var data = TestHelper.GetDataFromRequest(ctx.Request);

           AuthHelper.SetThreadData(data);
           
           await AsyncActor.DoSthAsync().ConfigureAwait(false);
            ctx.User = Thread.CurrentPrincipal;

            TestHelper.Verify(ctx);
        }

        private void Context_EndRequest(object sender, EventArgs e)
        {
            var context = ((HttpApplication)sender).Context;
            
            TestHelper.Verify(context);
        }

        public void Dispose()
        {
            //do nothing
        }

      
    }
}
