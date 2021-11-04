using Domain.DomainServices;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Domain
{
    public static class DomainIoC
    {
        public static IServiceCollection AddAppDomain(this IServiceCollection services)
        {
            services
                .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
                .AddScoped<ArticlesHandler>()
                .AddScoped<CommentsHandler>()
                .AddScoped<UsersHandler>();
            return services;
        }
    }
}
