using Domain.Repository.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface IArticleRepository
    {
        Task CreateArticleAsync(Article entity);
        int GetCount();
        Task<IEnumerable<Article>> GetPagingAsync(int page, int quantity);
        Task<IEnumerable<Article>> GetArticlesByKeywordAsync(string keyword);
        Task<IEnumerable<Article>> GetAllArticlesAsync();
        Task<Article> GetArticleAsync(int id);
        Task UpdateArticleAsync(Article entity);
        Task DeleteArticleAsync(int id);
        void DeleteArticle(Article entity);
    }
}
