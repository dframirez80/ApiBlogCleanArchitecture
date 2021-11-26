﻿using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace ApiBlogCA.Startups
{
    public static class SwaggerStartup
    {
        private const string TitleAPI = "API Blog Clean Architecture";
        public static IApplicationBuilder UseSwaggerAppMiddleware(this IApplicationBuilder app) {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.RoutePrefix = "swagger";
                options.SwaggerEndpoint(url: "/swagger/v1/swagger.json", TitleAPI);
            });
            return app;
        }

        public static IServiceCollection AddAppSwagger(this IServiceCollection services) {
            services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = TitleAPI,
                    Version = "v1",
                    Description = "A Blog example ASP.NET Core Web API by Dario Ramirez",
                    //TermsOfService = new Uri(""),
                    Contact = new OpenApiContact
                    {
                        Name = "Dario Ramirez",
                        Email = "dfr80@hotmail.com",
                        Url = new Uri("https://dfr80.com.ar"),
                    }
                });

                /*// Add xml comments generated by VStudio as description for the API endpoints.
                // (For complete configuration do this:
                //   1 - Right click on ApiBlogCA project.
                //   2 - Check "XML documentation file" in the 'Build' tab of the project properties.
                //   3 - Add ';1591' to the 'Supress warnings' field (Avoids warning to add XML comment on every public stuff!).
                    ejemplo
                    <PropertyGroup>
			            <GenerateDocumentationFile>true</GenerateDocumentationFile>
			            <NoWarn>$(NoWarn);1591</NoWarn>
		            </PropertyGroup>
                //  )*/

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                options.IncludeXmlComments(xmlPath);

                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT token \r\n Ingrese 'Bearer' [space] y luego el token.\r\n\r\nEjemplo: \"Bearer 12345abcdef\"",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    BearerFormat = "JWT",
                    Scheme = "Bearer"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme {
                                Reference = new OpenApiReference {
                                    Id = "Bearer",
                                    Type = ReferenceType.SecurityScheme
                                }
                        },
                        new string[] {}
                    }
                });
            });
            return services;
        }
    }
}
