using F_ckCreditSlavery.Entities;
using F_ckCreditSlavery.Entities.Models;
using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Contracts.Repositories;
using F_ckCreditSlavery.LoggerService;
using F_ckCreditSlavery.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore;
using AspNetCoreRateLimit;

// using Marvin.Cache.Headers;

namespace F_ckCreditSlavery.WebApi.Extensions;

public static class ServiceExtensions
{
    public static void ConfigureLoggerService(this IServiceCollection services) =>
        services.AddScoped<ILoggerManager, LoggerManager>();
        
    public static void ConfigureSqlContext(this IServiceCollection services,
        IConfiguration configuration) =>
        services.AddDbContext<RepositoryContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection")));
        
    public static void ConfigureRepositoryManager(this IServiceCollection services) =>
        services.AddScoped<IRepositoryManager, RepositoryManager>();
    
    public static void ConfigureResponseCaching(this IServiceCollection services) =>
        services.AddResponseCaching();
    
    public static void ConfigureHttpCacheHeaders(this IServiceCollection services) =>
        services.AddHttpCacheHeaders(
            // expirationOptions =>
            // {
            //     expirationOptions.MaxAge = 85;
            //     expirationOptions.CacheLocation = CacheLocation.Private;
            // },
            // validationOptions =>
            // {
            //     validationOptions.MustRevalidate = true;
            // }
            );
    
    public static void ConfigureRateLimitingOptions(this IServiceCollection services)
    {
        var rateLimitRules = new List<RateLimitRule>
        {
            new RateLimitRule
            {
                Endpoint = "*",
                Limit= 100,
                Period = "5m"
            }
        };
        
        services.Configure<IpRateLimitOptions>(opt => { opt.GeneralRules = rateLimitRules; });
        services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
        services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
        services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();
    }
    
    public static void AddCustomMediaTypes(this IServiceCollection services)
    {
        services.Configure<MvcOptions>(config =>
        {
            var systemTextJsonOutputFormatter = config.OutputFormatters
                .OfType<SystemTextJsonOutputFormatter>()?.FirstOrDefault();

            systemTextJsonOutputFormatter?.SupportedMediaTypes.Add("application/vnd.fcs.hateoas+json");

            var xmlOutputFormatter = config.OutputFormatters
                .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();

            xmlOutputFormatter?.SupportedMediaTypes.Add("application/vnd.fcs.hateoas+xml");
        });
    }
    
    public static void ConfigureIdentity(this IServiceCollection services)
    {
        var builder = services.AddIdentityCore<User>(o =>
        {
            o.Password.RequireDigit = true;
            o.Password.RequireLowercase = false;
            o.Password.RequireUppercase = false;
            o.Password.RequireNonAlphanumeric = false;
            o.Password.RequiredLength = 10;
            o.User.RequireUniqueEmail = true;
        });
        builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
        builder.AddEntityFrameworkStores<RepositoryContext>().AddDefaultTokenProviders();
    }
}