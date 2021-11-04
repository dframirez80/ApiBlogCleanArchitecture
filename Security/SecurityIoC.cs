using Domain.Email;
using Domain.Models;
using Domain.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Security
{
    public static class SecurityIoC
    {
        public static IServiceCollection AddAppSecurity (this IServiceCollection services, IConfiguration config)
        {
            services.Configure<SecuritySettings>(config.GetSection("SecuritySettings"));
            services.AddTransient<ITokenJwt, TokenJwt>();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = config.GetValue<bool>("SecuritySettings:RequireHttpsMetadata");
                    options.SaveToken = config.GetValue<bool>("SecuritySettings:SaveToken");
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = config.GetValue<bool>("SecuritySettings:ValidateIssuer"),
                        ValidateAudience = config.GetValue<bool>("SecuritySettings:ValidateAudience"),
                        ValidAudience = config["SecuritySettings:ValidAudience"],
                        ValidIssuer = config["SecuritySettings:ValidIssuer"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(config["SecuritySettings:Secret"]))
                    };
                });
            return services;
        }

    }
}
