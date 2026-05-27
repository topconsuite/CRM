using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Microsoft.Practices.Unity;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Owin;
using System;
using System.Net;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using TopSys.TopConWeb.API.Helpers;
using TopSys.TopConWeb.API.Security;
using TopSys.TopConWeb.API.Telemetry;
using TopSys.TopConWeb.Application.Commom.AutoMapper;
using TopSys.TopConWeb.Application.Interfaces;
using TopSys.TopConWeb.Domain.Interfaces.Repositories;
using TopSys.TopConWeb.Infra.CrossCuting;
using TopSys.TopConWeb.Infra.Data.Migrations;
using TopSys.TopConWeb.SharedKernel.Events;

namespace TopSys.TopConWeb.API
{
    public class Startup
    {

        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            UnityContainer container = new UnityContainer();

            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;

            ConfigureDependecyInjection(config, container);
            ConfigureWebApi(config);
            
            ConfigureOAuth(app, container.Resolve<IUsuarioApplicationService>(), container.Resolve<ISsoApplicationService>());
            ApiKeyAuthorizeAttribute.ConfigureApiKeyAuthorize(container.Resolve<IParametroApplicationService>());

            MapperConfig.RegisterMapping();

            //Aceitando requisições de todos os domínos
            app.UseCors(CorsOptions.AllowAll);

            //Informando que vamos utilizar WebApi
            app.UseWebApi(config);

			DbUpMigration.UpgradeDatabase();

            DbUpMigration.VerifyColumnsVersionTables(container.Resolve<IDatabaseRepository>());

        }

        public static void ConfigureWebApi(HttpConfiguration config)
        {
            MediaTypeFormatterCollection formatters = config.Formatters;
            formatters.Remove(formatters.XmlFormatter);
            
            JsonSerializerSettings jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Formatting.Indented;

            //Configurando o JSON para retornar as letras das propriedades em minúsculo
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

			formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
			formatters.JsonFormatter.SerializerSettings.PreserveReferencesHandling = PreserveReferencesHandling.None;

            config.Services.Add(typeof(IExceptionLogger), new TraceExceptionLogger());

            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute("DefaultApi", "api/v1/{controller}/{id}", new { id = RouteParameter.Optional });

            config.IncludeErrorDetailPolicy = IncludeErrorDetailPolicy.Always;


            


        }

        public static void ConfigureDependecyInjection(HttpConfiguration config, UnityContainer container)
        {
            
            DependencyResolver.Resolve(container);

            config.DependencyResolver = new UnityResolverHelper(container);
            DomainEvent.Container = new DomainEventsContainer(config.DependencyResolver);

        }

        public void ConfigureOAuth(IAppBuilder app, IUsuarioApplicationService usuarioAppService, ISsoApplicationService ssoAppService)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions()
            {

                //TODO: HABILITAR ESTA OPÇÃO QUANDO ENTRAR EM PRODUÇÃO COM HTTPS
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/api/security/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromHours(10),
                Provider = new AuthAuthorizationServerProvider(usuarioAppService, ssoAppService)
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            
            //Autenticação utilizando Token
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());
        }
    }
}
