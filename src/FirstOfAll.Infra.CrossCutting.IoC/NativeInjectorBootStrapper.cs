using FirstOfAll.Application.Interfaces;
using FirstOfAll.Application.Services;
using FirstOfAll.Domain.Interfaces;
using FirstOfAll.Infra.CrossCutting.Identity.Authorization;
using FirstOfAll.Infra.CrossCutting.Identity.Models;
using FirstOfAll.Infra.CrossCutting.Identity.Services;
using FirstOfAll.Infra.Data.Context;
using FirstOfAll.Infra.Data.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FirstOfAll.Infra.CrossCutting.IoC
{
    public class NativeInjectorBootStrapper
    {
        public static void RegisterServices(IServiceCollection services)
        {
            // ASP.NET HttpContext dependency
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ASP.NET Authorization Polices
            services.AddSingleton<IAuthorizationHandler, ClaimsRequirementHandler>();

            // Application
            services.AddScoped<ICustomerAppService, CustomerAppService>();
            
            // Infra - Data
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<FirstOfAllContext>();

            // Infra - Identity Services
            services.AddTransient<IEmailSender, AuthEmailMessageSender>();
            services.AddTransient<ISmsSender, AuthSMSMessageSender>();

            // Infra - Identity
            services.AddScoped<IUser, AspNetUser>();
        }
    }
}