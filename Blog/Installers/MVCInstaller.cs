using Asp.Versioning;
using Blog.Contracts.Identity.Request;
using Blog.Globalizations;
using Blog.Helper.Middlewares;
using Blog.Helper.Security.Tokens;
using Blog.Options;
using Blog.Services;
using FluentValidation;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System.Globalization;
using System.Text;

namespace Blog.Installers
{
    public class MVCInstaller : IInstaller
    { 
     public void InstallServices(IServiceCollection services, IConfiguration configuration, IWebHostEnvironment env)
    {
        // Add Cors
        services.AddCors(options => {
            options.AddPolicy("sameOrigin", builder =>
            builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
            );
        });
            services.AddHttpLogging(httpLogging =>
            {
                httpLogging.LoggingFields = HttpLoggingFields.All;
                httpLogging.RequestHeaders.Add("Request-Header-Demo");
                httpLogging.ResponseHeaders.Add("Response-Header-Demo");
                httpLogging.MediaTypeOptions.
                AddText("application/javascript");
                httpLogging.RequestBodyLogLimit = 4096;
                httpLogging.ResponseBodyLogLimit = 4096;
            });

            // Add AutoMapper
            services.AddAutoMapper(typeof(Program));
            
  
            // version
            services.AddApiVersioning(setupAction =>
            {
                setupAction.ReportApiVersions = true;
                setupAction.AssumeDefaultVersionWhenUnspecified = true;
                setupAction.DefaultApiVersion = new ApiVersion(1, 0);
                setupAction.ApiVersionReader = new UrlSegmentApiVersionReader();
            }).AddMvc();

            services.AddValidatorsFromAssemblyContaining<UserRegistrationRequestDtoValidation>();
            // Add globalization
            services.AddLocalization();
           
            services.AddSingleton<LocalizationMiddleware>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

            // add JWT
            services.AddOptions();
            var tokenOption = configuration.GetSection("TokenOption").Get<TokenOption>();
            services.Configure<TokenOption>(configuration.GetSection("TokenOption"));
            var signingConfigurations = new SigningConfigurations();
 
            services.AddSingleton(signingConfigurations);


            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(option =>
            {

               
                option.SaveToken = true;
                option.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = signingConfigurations.Key,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidIssuer = tokenOption.Issuer,
                    ValidAudience = tokenOption.Audience,
                    ClockSkew=TimeSpan.Zero
                };
            });


            #region Bearer swagger configuration.
            services.AddSwaggerGen(option =>
            {
                option.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Blog API",
                    Version = "v1"
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0] }
                };



                option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "JWt Auth using bearer scheme",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey
                });

                option.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    { new OpenApiSecurityScheme { Reference = new OpenApiReference
                        {
                            Id = "Bearer",
                            Type = ReferenceType.SecurityScheme
                        }
                    }, new List<string>() }
                });
            });

            #endregion
            #region cahing
            services.AddResponseCaching();
            services.AddHttpCacheHeaders((expirationModelOptions) => 
            {
                expirationModelOptions.MaxAge = 60;
                expirationModelOptions.CacheLocation = Marvin.Cache.Headers.CacheLocation.Private;
            },
            (validationModelOptions) =>
            {
                validationModelOptions.MustRevalidate = true;
            }
            );
            #endregion
        }
    }
}

