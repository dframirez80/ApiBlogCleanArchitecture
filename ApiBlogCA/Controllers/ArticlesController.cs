
using Domain.Constants;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Repository;
using Domain.Repository.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DomainServices;
using System.Security.Claims;
using ApiBlogCA.Helpers;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBlogCA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.AdminOrUser)]
    public class ArticlesController : ControllerBase
    {
        // GET: api/v1/Articles/paging/?page=1&quantity=10
        [HttpGet("paging")]
        [AllowAnonymous]
        public async Task<ActionResult<PagingResponse<Article>>> GetArticlesPaging([FromServices] ArticlesHandler handler,[FromQuery]PagingRequest paging) {
            if (paging == null) 
                return NotFound();
            return Ok(await handler.GetArticlesByPagingAsync(paging));
        }

        // GET: api/v1/Articles/keyword/tecnologia
        [HttpGet("keyword/{keyword}")]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesByKeyword([FromServices] ArticlesHandler handler, string keyword) {
            var articles = await handler.GetArticlesByKeywordAsync(keyword);
            if (articles == null) 
                return NotFound();
            return Ok(articles);
        }
        
        // GET: api/v1/Articles
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesAsync([FromServices] ArticlesHandler handler) {
            return Ok(await handler.GetArticlesAsync());
        }

        // GET: api/v1/Articles/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<Article>> GetArticleAsync([FromServices] ArticlesHandler handler, int id) {
            var article = await handler.GetArticleAsync(id);
            if (article == null)
                return NotFound();
            return article;
        }

        // GET: api/v1/Articles/Reactions
        [HttpGet("Reactions")]
        public async Task<ActionResult<Reactions>> GetReactionsAsync([FromServices] ArticlesHandler handler, int id) {
            var reactions = await handler.GetReactionsAsync(id);
            if (reactions == null)
                return NotFound();
            return Ok(reactions);
        }

        // PUT: api/v1/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutArticle([FromServices] ArticlesHandler handler, int id, Article article) {
            if (id != article.ArticleId)
                return BadRequest();
            var art = await handler.GetArticleAsync(id);
            int userId = Authorization.GetTokenId(User);
            if (userId == 0)
                Unauthorized();
            if (userId == art.UserId || User.IsInRole(Roles.Admin)) { 
                await handler.UpdateArticleAsync(article);
                return NoContent();
            }
            return BadRequest();
        }

        // PUT: api/v1/Articles/Reactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Reactions")]
        public async Task<IActionResult> PutReactions([FromServices] ArticlesHandler handler, Reactions reactions) {
            if (reactions == null)
                return BadRequest();
            await handler.UpdateReactions(reactions);
            return NoContent();
        }

        // POST: api/v1/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult> PostArticle([FromServices] ArticlesHandler handler, [FromServices] UsersHandler handlerUsers, ArticleDto articleDto) {
            if (articleDto == null) 
                return BadRequest();
            int userId = Authorization.GetTokenId(User);
            if (userId==0)
                Unauthorized();
            if ((await handlerUsers.GetUserExistsAsync(userId)) != null) {
                var articleId = await handler.CreateArticleAsync(articleDto, userId);
                return Created("GetArticle", new { id = articleId });
            }
            return NotFound();
        }

        // DELETE: api/v1/Articles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle([FromServices] ArticlesHandler handler, int id) {
            var article = await handler.GetArticleAsync(id);
            if (article == null)
                return NotFound();
            int userId = Authorization.GetTokenId(User);
            if (userId == 0)
                Unauthorized();
            if (userId == article.UserId || User.IsInRole(Roles.Admin))
            {
                await handler.DeleteArticleAsync(id);
                return NoContent();
            }
            return NotFound();
        }

    }
}
