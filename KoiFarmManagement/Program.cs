using Application.Commons;
using Infrastructure;
using Microsoft.EntityFrameworkCore;
using KoiFarmManagement.Middlewares;
using Infrastructure.Mappers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.OpenApi.Models;
using System.Reflection;
namespace KoiFarmManagement
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var configuration = builder.Configuration;
            builder.Services.AddControllers();
            var myConfig = new AppConfiguration(); 
            configuration.Bind(myConfig);
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddDbContext<ApiContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            }); 
            builder.Services.AddSingleton(myConfig);
            builder.Services.AddInfrastructuresService();
            builder.Services.AddWebAPIService();
            builder.Services.AddAutoMapper(typeof(MapperConfigurationsProfile));
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                           builder =>
                           {
                               builder.AllowAnyOrigin()
                                .AllowAnyMethod()
                                .AllowAnyHeader();
                           });
            }); 
            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
                options.AddPolicy("Staff", policy => policy.RequireRole("Staff"));
                options.AddPolicy("Customer", policy => policy.RequireRole("Customer"));
            }); 
            builder.Services.AddDistributedMemoryCache();

            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30); // Session timeout
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    IConfiguration config = (IConfiguration)configuration;
                     options.TokenValidationParameters = new TokenValidationParameters
                        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = configuration["JWTSection:Issuer"],
            ValidAudience = configuration["JWTSection:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JWTSection:SecretKey"]))
                        };
                        });

            builder.Services.AddSwaggerGen(c =>
            {

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "KoiFarmShop.API",
                });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme. " +
                                        "\n\nEnter your token in the text input below. " +
                                          "\n\nExample: '12345abcde'",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });             
            });
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("swagger/v1/swagger.json", "KoiFarmShopApI v1");
                c.RoutePrefix = string.Empty;
            });
            app.UseCors("AllowAll");
            app.UseHttpsRedirection();

            app.UseAuthorization();
            app.UseMiddleware<ConfirmationTokenMiddleware>();

            app.MapControllers();

            app.Run();
        }
    }
}
