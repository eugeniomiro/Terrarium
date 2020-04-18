using System.Web.Http;
using SimpleInjector;
using SimpleInjector.Integration.WebApi;
using SimpleInjector.Lifestyles;
using Terrarium.Services.Abstract;
using Terrarium.Services.Concrete;

namespace Terrarium.Web.UI
{
    public class DependencyInjection
    {
        public static void Setup(HttpConfiguration configuration)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle = new AsyncScopedLifestyle();

            container.Register<ISystemStatusService, SystemStatusService>();

            container.RegisterWebApiControllers(configuration);

            container.Verify();

            configuration.DependencyResolver = new SimpleInjectorWebApiDependencyResolver(container);
        }
    }
}
