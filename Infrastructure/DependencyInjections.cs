using Application.IRepositories;
using Application;
using Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Services;
using Application.IService;

namespace Infrastructure
{
    public static class DependencyInjections
    {
        public static IServiceCollection AddInfrastructuresService(this IServiceCollection services)
        {
            services.AddScoped<IUserRepo, UserRepo>();
            services.AddScoped<ITokenRepo, TokenRepo>();
            services.AddScoped<IKoiRepo, KoiRepo>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepo, CategoryRepo>();
            return services;
        }
    }
}
