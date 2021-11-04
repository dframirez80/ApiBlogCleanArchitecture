using Domain.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserAsync(int id);
        Task<User> GetUserWithAllDataAsync(int id);
        Task<bool> UserExistsAsync(int id);
        Task<User> UserExistsAsync(string email);
        Task CreateUserAsync(User entity);
        Task UpdateUserAsync(User entity);
        Task DeleteUserAsync(int id);
        void DeleteUser(User entity);
    }
}
