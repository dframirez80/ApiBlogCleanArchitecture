using AutoMapper;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Repository;
using Domain.Repository.Entities;
using Domain.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class ArticlesHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        private readonly ITokenJwt _tokenJwt;

        public ArticlesHandler(IUnitOfWork uow, IMapper mapper, ITokenJwt tokenJwt) {
            _uow = uow;
            _mapper = mapper;
            _tokenJwt = tokenJwt;
        }
        public async Task<PagingResponse<Article>> GetArticlesByPagingAsync(PagingRequest paging) {
            if (paging.Quantity < Paging.QuantityMin)
                paging.Quantity = Paging.QuantityMin;
            var count = _uow.Articles.GetCount();
            var totalPages = (int)Math.Floor((decimal)count / paging.Quantity);
            var nextPage = false;
            if ((count % paging.Quantity) > 0)
                totalPages++;
            if (paging.Page > totalPages)
                paging.Page = 1;

            if (paging.Page < totalPages)
                nextPage = true;
             else
                nextPage = false;
            PagingResponse<Article> pagingResponse = new()
            {
                TotalItems = count,
                CurrentPage = paging.Page,
                NextPage = nextPage,
                ListItems = await _uow.Articles.GetPagingAsync(paging.Page, paging.Quantity)
            };
            return pagingResponse;
        }
        public async Task<IEnumerable<Article>> GetArticlesByKeywordAsync(string keyword) {
            var articles = await _uow.Articles.GetArticlesByKeywordAsync(keyword);
            return articles;
        }
        public async Task<IEnumerable<Article>> GetArticlesAsync() {
            return await _uow.Articles.GetAllArticlesAsync();
        }
        public async Task<Article> GetArticleAsync(int id) {
            return await _uow.Articles.GetArticleAsync(id);
        }
        public async Task<Reactions> GetReactionsAsync(int id) {
            var article = await _uow.Articles.GetArticleAsync(id);
            if (article == null)
                return null;
            Reactions reactions = new()
            {
                Id = article.ArticleId,
                Likes = article.Likes,
                Dislikes = article.Dislikes
            };
            return reactions;
        }
        public async Task UpdateArticleAsync(Article article) {
            await _uow.Articles.UpdateArticleAsync(article);
            await _uow.CommitAsync();
        }
        public async Task UpdateReactions(Reactions reactions) {
            if (reactions == null) return;
            var article = await _uow.Articles.GetArticleAsync(reactions.Id);
            if (article == null) return;
            if (reactions.Likes > 0)
                article.Likes++;
            if (reactions.Dislikes > 0)
                article.Dislikes++;
            await _uow.CommitAsync();
        }
        public async Task<int> CreateArticleAsync(ArticleDto articleDto, int userId) {
            var article = _mapper.Map<Article>(articleDto);
            article.Created = DateTime.UtcNow.AddHours(UTC.GmtBuenosAires);
            article.UserId = userId;
            await _uow.Articles.CreateArticleAsync(article);
            await _uow.CommitAsync();
            return article.ArticleId;
        }
        public async Task DeleteArticleAsync(int id) {
            await _uow.Articles.DeleteArticleAsync(id);
            await _uow.CommitAsync();
        }
    }
}
