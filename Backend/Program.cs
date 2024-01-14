using Autofac;
using Autofac.Core;
using Autofac.Extensions.DependencyInjection;
using Backend.CodeAuth;
using Backend.CodeAuth.Middleware;
using Backend.Modules;
using Domain.Entities;
using Infrastructure.Config;
using Infrastructure.DTOs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NSwag;
using NSwag.Generation.Processors.Security;
using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Security.Claims;

namespace Backend;

public class Program
{
    public static void Main(string[] args)
    {

        WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
        IWebHostEnvironment environment = builder.Environment;

        builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

        // load config including environment specific versions
        IConfigurationBuilder configBuilder = new ConfigurationBuilder()
            .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .AddJsonFile($"appsettings.{environment.EnvironmentName}.json", optional: true, reloadOnChange: true)
            .AddUserSecrets<Program>();

        IConfigurationRoot config = configBuilder.Build();
        string apiSecret = config.GetSection("ApiConfig")["ApiSecret"]!;
        if (string.IsNullOrEmpty(apiSecret))
        {
            throw new ArgumentException("Missing ApiSecret");
        }

        builder.Host.ConfigureContainer<ContainerBuilder>(builder => builder.AddApiModule(config));

        builder.Services.AddDbContext<ApplicationDbContext>(opt => opt.UseSqlServer(config.GetConnectionString("DefaultConnection")));

        IdentityBuilder identityBuilder = builder.Services.AddIdentityCore<ApplicationUser>(
            options => options.SignIn.RequireConfirmedAccount = true)
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<ApplicationDbContext>();
       
        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = TokenHelper.GetValidationParameters(apiSecret);
            });

        builder.Services.AddControllers();

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerDocument(config =>
        {
            config.Title = "My API 1";
            config.Description = "My API 1";
            config.AddSecurity("JWT", new NSwag.OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = OpenApiSecuritySchemeType.ApiKey,
                In = OpenApiSecurityApiKeyLocation.Header,
                Description = "My API 1"
            });
            config.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
        });

        builder.Services.AddAppProblemDetails(builder.Environment);

        builder.Services.AddHttpContextAccessor();


        var principal = new ClaimsPrincipal();

        builder.Services.AddTransient(s => s.GetService<IHttpContextAccessor>()?.HttpContext?.User ?? principal);

        WebApplication app = builder.Build();


        app.UseOpenApi();
        app.UseSwaggerUi();

        app.UseAppProblemDetails();

        app.UseHttpsRedirection();

        IApiConfig apiConfig = app.Services.GetService<IApiConfig>()!;

        // and requests do not have the same origin.
        app.UseCors(options => options
            .WithOrigins(apiConfig.AllowedOrigin.Split(',').Select(o => o.Trim()).ToArray())
            .AllowAnyMethod()
            .AllowAnyHeader()
            .WithExposedHeaders("Content-Disposition")
            .AllowCredentials());

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
