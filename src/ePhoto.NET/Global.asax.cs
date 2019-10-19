using System.Data.Entity;
using System.Web.Mvc;
using System.Web.Routing;

using ePhoto.NET.Models;

namespace ePhoto.NET {
    public class MvcApplication : System.Web.HttpApplication {
        protected void Application_Start() {
            DependencyRegisterar.RegisterDependencies();
            Database.SetInitializer(new PhotoSharingInitializer());
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}