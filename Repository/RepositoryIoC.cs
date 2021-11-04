using Domain.Repository;
using Repositories;
using Repository.Repositories;
using Repository.Repositories.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Repository
{
    public static class RepositoryIoC
    {
        public static IServiceCollection AddAppRepository(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddDbContext<AppDbContext>(options => options.UseSqlServer(config.GetConnectionString("ApiBlogCADB")));
            return services;
        }

        public static void DeleteAndMigrateDatabase(IServiceScope scope)
        {
            // for demo purposes, delete the database & migrate on startup so we can start with a clean state.
            var context = scope.ServiceProvider.GetService<AppDbContext>();
            context.Database.EnsureDeleted();
            context.Database.Migrate();
        }
    }
}
