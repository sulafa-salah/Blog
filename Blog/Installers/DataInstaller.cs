using Blog.Contracts.Identity.Request;
using Blog.Domain.Models;
using Blog.Helper.Security.Tokens;
using Blog.Persistence;
using Blog.Services;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace Blog.Installers
{
    public class DataInstaller : IInstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
        {
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), sqlOptions => sqlOptions.EnableRetryOnFailure()));


            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddRoles<ApplicationRole>()
                 .AddEntityFrameworkStores<AppDbContext>()
         .AddDefaultTokenProviders();  // add seprated config below to token provider
            #region Dependancy Injections

            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ITokenHandler, TokenHandler>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddScoped<IEmailService, EmailService>();
           // services.AddScoped<IValidator<UserRegistrationRequestDto>, UserRegistrationRequestDtoValidation>();
            #endregion
        }
    }
}
