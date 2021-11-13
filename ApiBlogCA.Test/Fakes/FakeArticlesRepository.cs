using System.Threading.Tasks;
using Domain.Repository;
using System;
using Domain.Repository.Entities;
using System.Collections.Generic;

namespace ApiBlogCA.Test.Fakes
{
    public partial class UserControllerTest
    {
        public class FakeArticlesRepository : IArticleRepository
        {
            public Task CreateArticleAsync(Article entity)
            {
                throw new NotImplementedException();
            }

            public void DeleteArticle(Article entity)
            {
                throw new NotImplementedException();
            }

            public Task DeleteArticleAsync(int id)
            {
                throw new NotImplementedException();
            }

            public async Task<IEnumerable<Article>> GetAllArticlesAsync()
            {
                // Hacer que siempre devuelva lo mismo
                return await Task.FromResult(new List<Article>() {
                    new () { ArticleId = 1, Title = "Titulo", Content = "Contenido" },
                });
            }

            public Task<Article> GetArticleAsync(int id)
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Article>> GetArticlesByKeywordAsync(string keyword)
            {
                throw new NotImplementedException();
            }

            public int GetCount()
            {
                throw new NotImplementedException();
            }

            public Task<IEnumerable<Article>> GetPagingAsync(int page, int quantity)
            {
                throw new NotImplementedException();
            }

            public Task UpdateArticleAsync(Article entity)
            {
                throw new NotImplementedException();
            }
        }

    }
}