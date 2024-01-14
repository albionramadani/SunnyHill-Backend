using Application.Services;
using Application.Validators;
using Autofac;
using Domain.Interfaces;
using Infrastructure.Config;
using Infrastructure.DTOs;
using Infrastructure.Security;
using System.Runtime.CompilerServices;

namespace Backend.Modules
{
    public class ApiModule : Module
    {
        private readonly IConfigurationRoot _configurationRoot;

        public ApiModule(IConfigurationRoot configurationRoot)
        {
            _configurationRoot = configurationRoot;
        }
        protected override void Load(ContainerBuilder builder)
        {
            ApiConfig apiConfig = _configurationRoot.GetSection(typeof(ApiConfig).Name).Get<ApiConfig>();
            builder.Register(x => apiConfig).As<IApiConfig>();

            MailSettings mailSetting = _configurationRoot.GetSection(typeof(MailSettings).Name).Get<MailSettings>();

            builder.RegisterType<AuthorizationMenager>().As<IAuthorizationManager>()
                .InstancePerLifetimeScope();

            builder.Register(x => mailSetting).As<IMailSettings>();
            builder.RegisterType<UserService>()
                .As<IUserService>().InstancePerLifetimeScope();
            builder.RegisterType<ProductValidator>().AsSelf().InstancePerLifetimeScope();
            builder.RegisterType<ProductsService>().As<IProductService>().InstancePerLifetimeScope();


            builder.RegisterType<UserValidator>().AsSelf().InstancePerLifetimeScope();
            base.Load(builder);
        }
    }

    public static class Container
    {
        public static void AddApiModule(this ContainerBuilder builder, IConfigurationRoot config)
        {
            ApiModule module = new ApiModule(config);

            builder.RegisterModule(module);

        }
    }


}
