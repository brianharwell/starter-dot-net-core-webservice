using Microsoft.Extensions.DependencyInjection;

namespace Starter.Webservice.Core.Services
{
    public static class CustomCors
    {
        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
                {
                    options.AddPolicy("Development", builder =>
                        {
                            builder.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("http://localhost:4200");
                        });

                    options.AddPolicy("Staging", builder =>
                        {
                            builder.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("http://localhost:4200"); // TODO Set origin for staging environment
                        });

                    options.AddPolicy("Production", builder =>
                        {
                            builder.AllowAnyHeader()
                                .AllowAnyMethod()
                                .AllowCredentials()
                                .WithOrigins("http://localhost:4200"); // TODO Set origin for production environment
                        });
                });

            return services;
        }
    }
}
