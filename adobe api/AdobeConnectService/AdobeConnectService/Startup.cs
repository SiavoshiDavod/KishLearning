using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Swagger;

namespace AdobeConnectService
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSwaggerGen(swagger =>
            {
                swagger.SwaggerDoc("v1", new Info
                {
                    Title = "AdobeConnect ApiHelper"
                });
                // swagger.IncludeXmlComments(Path.Combine(Directory.GetCurrentDirectory(), @"bin\Debug\netcoreapp2.1", "EshopApi.xml"));
            });
            //services.AddResponseCaching();
            //services.AddMemoryCache();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseResponseCaching();



            app.UseMiddleware<ErrorHandlingMiddleware>();

            app.UseSwagger(c =>
            {
                c.RouteTemplate = "HelperApi/swagger/{documentName}/swagger.json";
            });
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/HelperApi/swagger/v1/swagger.json", "AdobeConnect ApiHelper");
                c.RoutePrefix = "HelperApi/swagger";
                c.DocumentTitle = "AdobeConnect ApiHelper";
            });

            //app.Use(async (context, next) =>
            //{
            //    await next.Invoke();
            //    context.Response.Redirect("/HelperApi/swagger");
            //});

            app.UseMvc();
           
            
        }
    }
}
