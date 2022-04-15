using DbFacade.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebApplicationDbFacadeBS5Template.DomainLayer.Connection;
using WebApplicationDbFacadeBS5Template.Services.Configuration;
using WebApplicationDbFacadeBS5Template.Services.Container;

namespace WebApplicationDbFacadeBS5Template
{
    public class MvcApplication : System.Web.HttpApplication
    {
        public override void Init()
        {
            ConfigureServices(ApplicationConfiguration.Environment);            
        }
        private void ConfigureServices(ApplicationEnvironment env)
        {
            if (env == ApplicationEnvironment.Production)
            {
                //ApplicationContainer.ConfigureService<ISampleService>(() => new SampleServiceProduction());
            }
            else if (env == ApplicationEnvironment.Evaluation)
            {
                //ApplicationContainer.ConfigureService<ISampleService>(() => new SampleServiceEvaluation());
            }
            else
            {
                //ApplicationContainer.ConfigureService<ISampleService>(() => new SampleService());
            }
            RegisterDbConnetions();
        }
        private void RegisterDbConnetions()
        {
            MainSQLDbConnection.Connection.Register();
        }
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }
    }
}
