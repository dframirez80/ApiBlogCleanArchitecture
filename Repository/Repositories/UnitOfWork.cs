using Domain.Repository;
using Repository.Repositories.EntityDbContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly AppDbContext _context;
        public IArticleRepository Articles { get; }
        public IUserRepository Users { get; }
        public ICommentRepository Comments { get; }

        public UnitOfWork(AppDbContext dbContext) {
            _context = dbContext;
            Comments = new CommentRepository(_context);
            Articles = new ArticleRepository(_context);
            Users = new UserRepository(_context);
        }

        public async Task CommitAsync() {
            await _context.SaveChangesAsync();
        }
        public void DiscardChanges() {
            _context.ChangeTracker.Clear();
        }
        public void Dispose() {
            _context.Dispose();
        }

    }
}
