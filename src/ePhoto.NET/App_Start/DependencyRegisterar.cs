using System.Reflection;
using System.Web.Mvc;

using Autofac;
using Autofac.Integration.Mvc;

using ePhoto.NET.Models;

using MvcSiteMapProvider.DI;

namespace ePhoto.NET {
    public class DependencyRegisterar {
        public static void RegisterDependencies() {
            var builder = new ContainerBuilder();

            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterModule<AutofacWebTypesModule>();
            builder.RegisterType<PhotoSharingContext>().As<IPhotoSharingContext>().InstancePerRequest();

            DependencyResolver.SetResolver(new DependencyResolverDecorator(new AutofacDependencyResolver(builder.Build()), new ConfigurationSettings()));
        }
    }
}