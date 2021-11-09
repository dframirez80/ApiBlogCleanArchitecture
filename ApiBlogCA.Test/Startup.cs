using Domain;
using Mail;
using Repository;
using Security;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ApiBlogCA.Startups;
using Repository.Repositories.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Domain.Repository;
using Repository.Repositories;
using Domain.Models;
using Domain.Email;
using Domain.Security;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace ApiBlogCA.Test
{
    public static class Startup
    {
        public static void ConfigureAppConfiguration(HostBuilderContext host, IConfigurationBuilder config) {
            config.AddJsonFile("appsettings.json",
                optional: false,
                reloadOnChange: true);
        }

        public static void ConfigureServices(HostBuilderContext host,
            IServiceCollection services) {
            var config = host.Configuration;
            //Infrastructure
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("ApiBlogCADB")));
            services.Configure<EmailSettings>(config.GetSection("EmailSettings"));
            services.AddTransient<IMailService, MailService>();

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
        }
    }
}
