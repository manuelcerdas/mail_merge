using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using MailMerge.Config;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace MailMerge.Extension
{
    public static  class ServiceCollectionExtensions
    {
        public static void AddSwaggerGenerate(this IServiceCollection services, IWebHostEnvironment environment, IConfiguration Configuration)
        {
            var section = Configuration
                .GetSection("SwaggerSettings");
            services.Configure<SwaggerSettings>(section);

            var ss = section.Get<SwaggerSettings>();

            //if (environment.EnvironmentName == "Production")
            //{
            //    return;
            //}

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = GetProductVersion(),
                    Title = ss.Title,
                    Description = ss.Description,

                    Contact = new OpenApiContact
                    {
                        Name = ss.Team,
                        Email = ss.Email,
                        Url = new Uri(ss.Url)
                    }
                });

                c.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                    {
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then the token",
                        Name = "Authorization",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    });

                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                        {
                        {
                            new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme, Id = "Bearer"
                                    },
                                    Scheme = "oauth2",
                                    Name = "Bearer",
                                    In = ParameterLocation.Header
                                },
                            new List<string>()
                        }
                        });

                // Set the comments path for the Swagger JSON and UI.
                var name = Assembly.GetExecutingAssembly().GetName().Name;
                var xmlFile = $"{name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.IgnoreObsoleteActions();
                c.IgnoreObsoleteProperties();
            });
        }

        private static string GetProductVersion()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            var version = $"v{fvi.ProductVersion}";
            return version;
        }
    }
}
