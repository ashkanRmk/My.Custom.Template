using System;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using My.Custom.Template.DataLayer.Abstract;
using My.Custom.Template.DataLayer.Context;
using My.Custom.Template.DataLayer.Repository;
using My.Custom.Template.Misc;
using My.Custom.Template.Mapping;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace my.custom.template_backend
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
            services.AddEntityFrameworkNpgsql();
            services.AddDbContextPool<ApplicationDbContext>((serviceProvider, optionsBuilder) =>
            {
                optionsBuilder.UseNpgsql(Environment.GetEnvironmentVariable("ApplicationDb"),
                    b => b.MigrationsAssembly("My Services"));
            });

            services.AddTransient(typeof(IEntityBaseRepository<>), typeof(EntityBaseRepository<>));

            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapperConfiguration()); });
            services.AddSingleton<IMapper>(sp => config.CreateMapper());

            services.AddLogging(loggingBuilder =>
                loggingBuilder.AddSerilog(dispose: true));

            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .AllowCredentials());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info
                {
                    Version = "v1",
                    Title = "My Services",
                    Description = "My Services Open API",
                    TermsOfService = "None"
                });
                c.OperationFilter<SwaggerFileUploadOperation>();
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            // app.UseHttpsRedirection();
            app.UseMvc();

            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My Services");
                c.DocumentTitle = "My Services Open API";
            });
        }
    }
}
