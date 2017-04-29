namespace Yuki.Api
{
    using System.Web.Http;
    using AutoMapper;
    using Owin;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    using Yuki.Data;

    public class Startup
    {
        private static void InitializeAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {

            });
        }

        public void Configuration(IAppBuilder app)
        {
            InitializeAutoMapper();

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

            //config.Filters.Add(new AuthorizeAttribute());

            //app.UseIdentityServerBearerTokenAuthentication(
            //    new IdentityServerBearerTokenAuthenticationOptions
            //    {
            //        Authority = "http://localhost:5000",
            //        ValidationMode = ValidationMode.ValidationEndpoint,
            //        RequiredScopes = new[] { "yuki" },
            //    });

            app.UseWebApi(config);
        }
    }
}