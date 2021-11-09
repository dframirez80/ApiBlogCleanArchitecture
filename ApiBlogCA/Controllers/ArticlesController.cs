
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
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBlogCA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.AdminOrUser)]
    [Produces("application/json")]
    public class ArticlesController : ControllerBase
    {
        // GET: api/v1/Articles/paging/?page=1&quantity=10
        [HttpGet("paging")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<PagingResponse<Article>>> GetArticlesPaging([FromServices] ArticlesHandler handler,[FromQuery]PagingRequest paging) {
            if (paging == null) 
                return NotFound();
            return Ok(await handler.GetArticlesByPagingAsync(paging));
        }

        // GET: api/v1/Articles/keyword/tecnologia
        [HttpGet("keyword/{keyword}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesByKeyword([FromServices] ArticlesHandler handler, string keyword) {
            var articles = await handler.GetArticlesByKeywordAsync(keyword);
            if (articles == null) 
                return NotFound();
            return Ok(articles);
        }
        
        // GET: api/v1/Articles
        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<Article>>> GetArticlesAsync([FromServices] ArticlesHandler handler) {
            return Ok(await handler.GetArticlesAsync());
        }

        // GET: api/v1/Articles/5
        [HttpGet("{id}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Article>> GetArticleAsync([FromServices] ArticlesHandler handler, int id) {
            var article = await handler.GetArticleAsync(id);
            if (article == null)
                return NotFound();
            return Ok(article);
        }

        // GET: api/v1/Articles/Reactions
        [HttpGet("Reactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reactions>> GetReactionsAsync([FromServices] ArticlesHandler handler, int id) {
            var reactions = await handler.GetReactionsAsync(id);
            if (reactions == null)
                return NotFound();
            return Ok(reactions);
        }

        // PUT: api/v1/Articles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutReactions([FromServices] ArticlesHandler handler, Reactions reactions) {
            if (reactions == null)
                return BadRequest();
            await handler.UpdateReactions(reactions);
            return NoContent();
        }

        // POST: api/v1/Articles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status201Created)]
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
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
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
