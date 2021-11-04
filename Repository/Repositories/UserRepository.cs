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
    public class UserRepository : IUserRepository, IDisposable
    {
        private readonly AppDbContext _context;
        private readonly DbSet<User> _dbSet;

        public UserRepository(AppDbContext context) {
            _context = context;
            _dbSet = context.Set<User>();
        }
        public async Task CreateUserAsync(User entity) {
            _dbSet.Add(entity);
            await Task.CompletedTask;
        }

        public async Task DeleteUserAsync(int id) {
            var entity = await GetUserAsync(id);
            DeleteUser(entity);
        }

        public void DeleteUser(User entity) {
            _context.Remove(entity);
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync() {
            return await _dbSet.ToListAsync();
        }

         public async Task<User> GetUserAsync(int id) {
            var entity = await _dbSet.FindAsync(id);
            return entity;
        }

        public async Task UpdateUserAsync(User entity) {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await Task.CompletedTask;
        }

        public async Task<User> UserExistsAsync(string email) {
            return await _dbSet.Where(w => w.Email == email).FirstOrDefaultAsync();
        }

        // Dispose
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

        public async Task<bool> UserExistsAsync(int id) {
            return await _dbSet.AnyAsync(a => a.UserId == id); 
        }

        public async Task<User> GetUserWithAllDataAsync(int id) {
            var entity = await _dbSet.Include(i=>i.Articles)
                                     .ThenInclude(i=>i.Comments)
                                     .FirstOrDefaultAsync(f => f.UserId == id);
            return entity;
        }
    }
}
