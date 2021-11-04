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
    public class ArticleRepository : IArticleRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DbSet<Article> _dbSet;

        public ArticleRepository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<Article>();
        }
        public async Task CreateArticleAsync(Article entity) {
            _dbSet.Add(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteArticleAsync(int id) {
            var entity = await GetArticleAsync(id);
            DeleteArticle(entity);
        }

        public void DeleteArticle(Article entity) {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<Article>> GetAllArticlesAsync() {
            return await _dbSet.ToListAsync();
        }

        public async Task<IEnumerable<Article>> GetArticlesByKeywordAsync(string keyword) {
            var articles = await _dbSet.Where(w => w.Keyword.Contains(keyword))
                                       .OrderBy(o => o.ArticleId)
                                       .ToListAsync();
            return articles;
        }

        public async Task<Article> GetArticleAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateArticleAsync(Article entity) {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        // Dispose
        private bool _disposed = false;
        protected virtual void Dispose(bool disposing) {
            if (!_disposed)
            {
                if (disposing)
                {
                    _context.Dispose();
                }
            }
            _disposed = true;
        }
        public void Dispose() {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetCount() {
            return  _dbSet.Count();
        }

        public async Task<IEnumerable<Article>> GetPagingAsync(int page, int quantity) {
            return await _dbSet.Skip((page - 1) * quantity).Take(quantity).ToListAsync();
        }
    }
}
