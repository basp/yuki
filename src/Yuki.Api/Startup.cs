namespace Yuki.Api
{
    using System.Web.Http;
    using AutoMapper;
    using IdentityServer3.AccessTokenValidation;
    using IdentityServer3.Core.Configuration;
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
                cfg.AddProfile(new Clients.MappingProfile());
                cfg.AddProfile(new Groups.MappingProfile());
                cfg.AddProfile(new Projects.MappingProfile());
                cfg.AddProfile(new Tags.MappingProfile());
                cfg.AddProfile(new TimeEntries.MappingProfile());
            });
        }

        private static Container CreateContainer()
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle =
                new AsyncScopedLifestyle();

            return container;
        }

        public void Configuration(IAppBuilder app)
        {
            var container = CreateContainer();

            InitializeAutoMapper();

            app.UseIdentityServer(new IdentityServerOptions
            {
                Factory = new IdentityServerServiceFactory()
                    .UseInMemoryClients(ApiClients.Get())
                    .UseInMemoryScopes(ApiScopes.Get())
                    .UseInMemoryUsers(ApiUsers.Get()),

                RequireSsl = false,
            });

            var config = new HttpConfiguration();

            container.RegisterWebApiControllers(config);
            container.Register<DataContext>(Lifestyle.Scoped);
            container.Verify();

            config.MapHttpAttributeRoutes();
            config.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

            // config.Filters.Add(new AuthorizeAttribute());

            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = "http://localhost:52946/",
                    ValidationMode = ValidationMode.ValidationEndpoint,
                    RequiredScopes = new[] { "api" },
                    DelayLoadMetadata = true,
                });

            app.UseWebApi(config);
        }
    }
}