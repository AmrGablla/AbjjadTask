using Azure.Storage.Blobs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.Text;
using WepAPI.Mapping;
using WepAPI.Models;

namespace WepAPI.Services
{
    public static class ServiceCollectionExtensions
    {

        public static void RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Abjjad_API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    Description = "Input your Bearer token in this format - Bearer {your token here} to access this API",
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            Scheme = "Bearer",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddAuthorizationPolicyBuilder(this IServiceCollection services)
        {
            services.AddControllers(opt =>
            {
                var policy = new AuthorizationPolicyBuilder("Bearer").RequireAuthenticatedUser().Build();
                opt.Filters.Add(new AuthorizeFilter(policy));
            });
        }

        public static void AddDbContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<PostsDatabaseSettings>(configuration.GetSection(nameof(PostsDatabaseSettings)));

            services.AddSingleton<IPostsDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PostsDatabaseSettings>>().Value);
        }

        public static void AddAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
           .AddJwtBearer(obearer =>
           {
               obearer.RequireHttpsMetadata = false;
               obearer.SaveToken = false;
               obearer.TokenValidationParameters = new TokenValidationParameters
               {
                   ValidateIssuerSigningKey = true,
                   ValidateIssuer = false,
                   ValidateAudience = false,
                   ValidateLifetime = false,
                   ClockSkew = TimeSpan.Zero,
                   ValidIssuer = configuration["JWTSettings:Issuer"],
                   ValidAudience = configuration["JWTSettings:Audience"],
                   IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
               };
               obearer.Events = new JwtBearerEvents()
               {
                   OnAuthenticationFailed = c =>
                   {
                       c.NoResult();
                       c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                       c.Response.ContentType = "application/json";
                       if (c.Exception.GetType() == typeof(SecurityTokenExpiredException))
                       {
                           c.Response.StatusCode = (int)HttpStatusCode.NotAcceptable;
                       }
                       var result = JsonConvert.SerializeObject(c.Exception.ToString());
                       return c.Response.WriteAsync(result);
                   }
               };
           });
        }

        public static IServiceCollection AddAutoMapperService(this IServiceCollection services)
        {
            services.AddAutoMapper(c => c.AddProfile<MapProfile>(), typeof(Startup));
            return services;
        }

        public static void AddAzureBlobStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped(_ =>
            {
                return new BlobServiceClient(configuration["AzureBlobStorage"]);
            });
        }
    }
}