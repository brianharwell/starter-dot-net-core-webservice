using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;

namespace Starter.Webservice
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureContainer(ContainerBuilder builder)
        {
            builder.RegisterModule(new AutofacModule());
        }
        
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc()
                .AddFluentValidation()
                .AddMvcOptions(options =>
                    {
                        options.Filters.Add(typeof(Core.Filters.ModelValidationFilter), 15);
                    });

            services.AddAutoMapper();

            services.AddSwaggerGen(options =>
                {
                    options.SwaggerDoc("v1", new Info() { Title = "Starter", Version = "v1" }); // TODO Replace this with the name of the application
                });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                    {
                        // TODO Set jwt options
                    });
        }

        public void ConfigureDevelopment(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseDeveloperExceptionPage();

            app.UseSwagger();

            app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200"));

            app.UseAuthentication();

            app.UseMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials()
                .WithOrigins("http://localhost:4200")); // TODO Replace this with the public facing urls

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
