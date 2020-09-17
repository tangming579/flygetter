using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;

namespace FlyGetter
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public static IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowSpecificOrigin",
                    builder =>
                    {
                        builder.AllowAnyOrigin()
                                .AllowAnyMethod() // �����������󷽷�
                                .AllowAnyHeader(); // ������������ͷ
                    });
            });
            // ע��Swagger����
            services.AddSwaggerGen(options =>
            {
                // ����ĵ���Ϣ
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Api Swagger",
                    Description = "����.NET Core 3.1 ��Api Swagger"
                });
                // ���س��򼯵�xml�����ĵ�
                var baseDirectory = Environment.CurrentDirectory;
                //var xmlFile = System.AppDomain.CurrentDomain.FriendlyName + ".xml";
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.XML";
                var xmlPath = Path.Combine(baseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                    options.IncludeXmlComments(xmlPath);

            });
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            loggerFactory.AddLog4Net();

            app.UseStaticFiles();

            app.UseDirectoryBrowser();

            //app.UseHttpsRedirection();

            // ����Swagger�м��
            app.UseSwagger();
            // ����SwaggerUI
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "API Demo v1");
                //c.RoutePrefix = string.Empty;
            });

            app.UseRouting();

            app.UseCors("AllowSpecificOrigin");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
