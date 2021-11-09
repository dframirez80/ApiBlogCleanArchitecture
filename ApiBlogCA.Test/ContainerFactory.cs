using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;

namespace ApiBlogCA.Test
{
    public static class ContainerFactory
    {
        public static IServiceProvider Create() {
            return CreateHostBuilder(null)
                .Build()
                .Services
                .CreateScope()
                .ServiceProvider;
        }

        private static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration(Startup.ConfigureAppConfiguration)
                .ConfigureServices(Startup.ConfigureServices);
    }
}
