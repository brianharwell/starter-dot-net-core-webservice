using Autofac;
using AutoMapper;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Starter.Webservice.Core.Services;
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

                        var authorizePolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
                        options.Filters.Add(new AuthorizeFilter(authorizePolicy));
                    });

            services.AddAutoMapper();

            services.AddCustomCors();

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

            app.UseCors("Development");

            app.UseAuthentication();

            app.UseMvc();
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            app.UseCors(env.EnvironmentName);

            app.UseRewriter(new RewriteOptions().AddRedirectToHttpsPermanent());

            app.UseAuthentication();

            app.UseMvc();
        }
    }
}
