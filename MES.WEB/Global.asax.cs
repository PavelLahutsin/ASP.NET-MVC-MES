using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using AutoMapper;
using MES.BLL.Infrastructure;
using MES.DAL.EF;
using Ninject;
using Ninject.Modules;
using Ninject.Web.Mvc;

namespace MES.WEB
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            Database.SetInitializer(new MesDbInitializer());
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // внедрение зависимостей
            NinjectModule registrations = new NinjectRegistrations();
            var kernel = new StandardKernel(registrations);
            DependencyResolver.SetResolver(new NinjectDependencyResolver(kernel));

            Mapper.Initialize(x =>
            {
                AutoMapperConfigWeb.Config.Invoke(x);
                AutmapperConfigBLL.Configure.Invoke(x);
            });
        }
    }
}
