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
using API.Repositories.Account;

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

            // Services
            services.AddScoped<ITokenService, TokenService>();
            services.AddScoped<IImageService, ImageService>();

            // BLLs
            services.AddScoped<IAccountBL, AccountBL>();
            services.AddScoped<ICategoryBL, CategoryBL>();

            // Repositories
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IUserRepository, UserRepository>();

            // Helpers
            services.AddScoped<Validations>(); // *** maybe add an interface to this 
            services.AddScoped<AccountBLHelper>();
            services.AddScoped<AccountRepositoryHelper>();

            // Database & Cloudinary
            services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
            services.AddDbContext<DataContext>(options =>
            {
                options.UseSqlite(config.GetConnectionString("DefaultConnection"));
            });
            return services;
        }
    }
}