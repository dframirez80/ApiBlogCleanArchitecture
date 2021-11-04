using Domain.Email;
using Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Mail
{
    public static class MailServiceIoC
    {
        public static IServiceCollection AddAppMailService (this IServiceCollection services, IConfiguration config)
        {
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddTransient<IMailService, MailService>();
            return services;
        }

    }
}
