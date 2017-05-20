namespace Yuki.Api
{
    using System.Web.Http;
    using AutoMapper;
    using IdentityServer3.AccessTokenValidation;
    using IdentityServer3.Core.Configuration;
    using IdentityServer3.Core.Services;
    using Owin;
    using Serilog;
    using SimpleInjector;
    using SimpleInjector.Integration.WebApi;
    using SimpleInjector.Lifestyles;
    using Yuki.Data;

    public class Startup
    {
        static Startup()
        {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Trace()
                .CreateLogger();

            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile(new Clients.MappingProfile());
                cfg.AddProfile(new Groups.MappingProfile());
                cfg.AddProfile(new Projects.MappingProfile());
                cfg.AddProfile(new Tags.MappingProfile());
                cfg.AddProfile(new TimeEntries.MappingProfile());
                cfg.AddProfile(new Workspaces.MappingProfile());
            });
        }

        private static Container CreateContainer(HttpConfiguration config)
        {
            var container = new Container();
            container.Options.DefaultScopedLifestyle =
                new AsyncScopedLifestyle();

            container.RegisterWebApiControllers(config);
            container.Register<DataContext>(Lifestyle.Scoped);
            container.Verify();

            return container;
        }

        private static IdentityServerOptions GetIdentityserverOptions()
        {
            var factory =
                new IdentityServerServiceFactory()
                    .UseInMemoryClients(ApiClients.Get())
                    .UseInMemoryScopes(ApiScopes.Get());

            factory.Register(new Registration<DataContext>());
            factory.Register(new Registration<UserRepository>());
            factory.UserService = new Registration<IUserService, UserService>();

            return new IdentityServerOptions
            {
                Factory = factory,
                RequireSsl = false,
            };
        }

        public void Configuration(IAppBuilder app)
        {
            var config = new HttpConfiguration();
            var container = CreateContainer(config);

            app.UseIdentityServer(GetIdentityserverOptions());

            config.MapHttpAttributeRoutes();
            config.DependencyResolver =
                new SimpleInjectorWebApiDependencyResolver(container);

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