using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using SimpleInjector;
using SimpleInjector.Integration.Web;
using SimpleInjector.Integration.Web.Mvc;
using SimpleInjector.Lifestyles;
using TestSimpleInjector.Services;

namespace TestSimpleInjector
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static Container _container;

        internal static Container TheContainer => _container ?? (_container = new Container());


        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);


            if(_container == null) _container = new Container();
            _container.Options.DefaultScopedLifestyle = new WebRequestLifestyle(); //AsyncScopedLifestyle or WebRequestLifestyle

            //Will see this error if use AsyncScopedLifeStyle: LocationService is registered as 'Async Scoped' lifestyle, but the instance is requested outside the context of an active (Async Scoped) scope. Please see https://simpleinjector.org/scoped for more information about how to manage scope
            _container.Register<ILocationService, LocationService>(Lifestyle.Scoped); //new WebRequestLifestyle(). Or Lifestyle.Scoped will be smart one depends on DefaultScopedLifestyle
            _container.RegisterMvcControllers(Assembly.GetExecutingAssembly());
            _container.Verify();

           // DependencyResolver.SetResolver(new SimpleInjectorDependencyResolver(_container)); //only needed when controllers has depedencies


        }
    }
}
