using Domain.Repository;
using Domain.Repository.Entities;
using Repository.Repositories.EntityDbContext;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Repository.Repositories
{
    public class CommentRepository : ICommentRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Comment> _dbSet;

        public CommentRepository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<Comment>();
        }

        public void DeleteComment(Comment entity) {
            _context.Remove(entity);
        }

        public async Task DeleteCommentAsync(int id) {
            var entity = await GetCommentAsync(id);
            DeleteComment(entity);
        }

        public async Task<IEnumerable<Comment>> GetCommentsAsync(int articleId) {
            return await _dbSet.Where(w=>w.ArticleId == articleId).ToListAsync();
        }

        public async Task<Comment> GetCommentAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateCommentAsync(Comment entity) {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        private bool _disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed)
            {
                if (disposing)
                {
                    try
                    {
                        _context.Dispose();
                    } catch (Exception) { }
                }
            }
            _disposed = true;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task CreateCommentAsync(Comment entity) {
            _dbSet.Add(entity);
            await Task.CompletedTask;
        }
    }
}
