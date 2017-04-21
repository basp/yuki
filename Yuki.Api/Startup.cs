namespace Yuki.Api
{
    using System.Web.Http;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    using Yuki.Model;

    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle =
                new AsyncScopedLifestyle();

            var config = new HttpConfiguration();
            config.MapHttpAttributeRoutes();
            container.RegisterWebApiControllers(config);
            container.Register<DataContext>(Lifestyle.Scoped);
            container.Verify();

            config.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            app.UseWebApi(config);
        }
    }
}