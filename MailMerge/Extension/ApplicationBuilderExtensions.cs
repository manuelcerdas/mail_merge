using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace MailMerge.Extension
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseCustomSwagger(this IApplicationBuilder app, IWebHostEnvironment environment, IConfiguration Configuration)
        {
            var endpoint = Configuration.GetValue<string>("SwaggerSettings:EndPoint");
            //if (environment.EnvironmentName == "Production" || string.IsNullOrEmpty(endpoint))
            //{
            //    return;
            //}
            var version = GetProductVersion();
            app.UseSwagger();
            app.UseSwaggerUI(o =>
            {
                o.SwaggerEndpoint(endpoint, $"Mail Merge API {version} ({environment.EnvironmentName})");
                o.RoutePrefix = "swagger";
            });
        }

        private static string GetProductVersion()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();
            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);
            string version = $"v{fvi.ProductVersion}";
            return version;
        }
    }
}
