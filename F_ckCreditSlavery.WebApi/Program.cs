using AspNetCoreRateLimit;
using F_ckCreditSlavery.Contracts;
using F_ckCreditSlavery.Entities.DataTransferObjects;
using F_ckCreditSlavery.LoggerService;
using F_ckCreditSlavery.Repositories.DataShaping;
using F_ckCreditSlavery.WebApi.Extensions;
using F_ckCreditSlavery.WebApi.Filters.Action;
using F_ckCreditSlavery.WebApi.Filters.Schema;
using F_ckCreditSlavery.WebApi.Utility;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Web;
using Microsoft.OpenApi.Models;
using NLog;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
//     .AddMicrosoftIdentityWebApi(builder.Configuration.GetSection("AzureAd"));

LogManager.LoadConfiguration(
    string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));

builder.Services.AddControllers(config =>
    {
        config.RespectBrowserAcceptHeader = true;
        config.ReturnHttpNotAcceptable = true;
        // config.CacheProfiles.Add("120SecondsDuration", new CacheProfile { Duration = 120});
    })
    .AddXmlDataContractSerializerFormatters();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo {Title = "F_ckCreditSlavery.WebApi", Version = "v1"});
    c.OperationFilter<SwaggerSkipPropertyFilter>();
});

#region Service.Extensions
builder.Services.ConfigureLoggerService();
builder.Services.ConfigureSqlContext(builder.Configuration);
builder.Services.ConfigureRepositoryManager();
// builder.Services.ConfigureResponseCaching();
// builder.Services.ConfigureHttpCacheHeaders();

#region Rate Limiting
builder.Services.AddMemoryCache();
        
builder.Services.ConfigureRateLimitingOptions();
builder.Services.AddHttpContextAccessor();
#endregion

builder.Services.AddCustomMediaTypes();

builder.Services.AddAuthentication();
builder.Services.ConfigureIdentity();

#endregion

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});

#region Validation
builder.Services.AddScoped<ValidationFilterAttribute>();
builder.Services.AddScoped<ValidateMediaTypeAttribute>();
builder.Services.AddScoped<IfCreditAccountExistsAttribute>();
builder.Services.AddScoped<IfCreditAccountChangeExistsAttribute>();
#endregion

builder.Services.AddScoped <IDataShaper<CreditAccountGetDto>, DataShaper<CreditAccountGetDto>>();
builder.Services.AddScoped <IDataShaper<CreditAccountChangeGetDto>, DataShaper<CreditAccountChangeGetDto>>();
builder.Services.AddScoped<CreditAccountChangeLinks>();

builder.Services.AddSingleton<ILoggerManager, LoggerManager>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILoggerManager>();
    app.ConfigureExceptionHandler(logger);
}

app.UseCors(x => x
    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());

app.UseIpRateLimiting();

app.UseHttpsRedirection();

// app.UseAuthentication();
// app.UseAuthorization();

app.MapControllers();

app.Run();