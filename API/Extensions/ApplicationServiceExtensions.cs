using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using Microsoft.EntityFrameworkCore;
using API.Interfaces;
using API.Services;
using API.Helpers;
using API.Repositories;
using API.BLL;
using API.BLL.Account;

namespace API.Extensions
{
    public static class ApplicationServiceExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddControllersWithViews()
                .AddNewtonsoftJson(options =>
                options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore
            );

            // Account
            services.AddScoped<IAccountBL, AccountBL>();
            services.AddScoped<IAccountValidations, AccountValidations>();
            services.AddScoped<IAccountMappers, AccountMappers>();
            services.AddScoped<IAccountRepository, AccountRepository>();

            // User
            services.AddScoped<IUserRepository, UserRepository>();

            // Category
            services.AddScoped<ICategoryBL, CategoryBL>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();

            // ***May be redundent***
            services.AddScoped<Validations>(); // *** maybe add an interface to this 

            // Database, Cloudinary, Token
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IImageService, ImageService>();
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}