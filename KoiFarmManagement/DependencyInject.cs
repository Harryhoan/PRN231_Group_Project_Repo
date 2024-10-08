using Application.IRepositories;
using Application;
using Infrastructure.Repositories;
using Infrastructure;
using System.Diagnostics;
using Application.IService;
using Application.Services;
using System.Text.Json;

namespace KoiFarmManagement
{
    public static class DependencyInject
    {
        public static IServiceCollection AddWebAPIService(this IServiceCollection services)
        {
            //services.AddControllers().AddJsonOptions(option =>
            //{
            //    option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
            //});
            //services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicyExtensions.KebabCaseLower());
            //services.AddControllers().AddJsonOptions(option => option.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.KebabCaseLower);
            services.AddControllers().AddJsonOptions(option =>
            {
                option.JsonSerializerOptions.PropertyNamingPolicy = new KebabCaseNamingPolicy();
            });
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IKoiRepo, KoiRepo>();
            services.AddScoped<IAuthenService, AuthenService>();
            services.AddScoped<IKoiService, KoiService>();
            services.AddScoped<cIUserService, cUserService>();
            services.AddControllers();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();
            services.AddHealthChecks();
            services.AddSingleton<Stopwatch>();
            services.AddScoped<ITokenRepo, TokenRepo>();
            
            return services;
        }
    }
}
