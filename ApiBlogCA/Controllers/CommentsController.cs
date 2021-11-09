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
using ApiBlogCA.Helpers;
using Microsoft.AspNetCore.Http;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBlogCA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.AdminOrUser)]
    [Produces("application/json")]
    public class CommentsController : ControllerBase
    {
        // GET: api/v1/Comments
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetAsync([FromServices] CommentsHandler handler, CommentDto commentDto) {
            return Ok(await handler.GetCommentsAsync(commentDto));
        }

        // GET: api/v1/Comments/5
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Comment>> GetCommentAsync([FromServices] CommentsHandler handler, int id) {
            return Ok(await handler.GetCommentAsync(id));
        }


        // GET: api/v1/Comments/Reactions
        [HttpGet("Reactions")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<Reactions>> GetReactionsAsync([FromServices] CommentsHandler handler, int id) {
            var reactions = await handler.GetReactionsAsync(id);
            if (reactions == null)
                return NotFound();
            return Ok(reactions);
        }

        // POST api/v1/Comments
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> PostAsync([FromServices] CommentsHandler handler, CommentDto commentDto) {
            if (commentDto == null)
                BadRequest();
            int userId = Authorization.GetTokenId(User);
            if (userId == 0)
                Unauthorized();
            int commentId = await handler.CreateCommentAsync(commentDto, userId);
            if (commentId==0)
                ValidationProblem();
            return Created("GetComment", new { id = commentId });
        }

        // PUT: api/v1/Comments/Reactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Reactions")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> PutReactions([FromServices] CommentsHandler handler, Reactions reactions) {
            if (reactions == null)
                return BadRequest();
            var resp = await handler.UpdateReactionsAsync(reactions);
            if (!resp)
                ValidationProblem();
            return NoContent();
        }

        // PUT api/v1/Comments/5
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Put([FromServices] CommentsHandler handler, int id, CommentDto commentDto) {
            if (commentDto  == null)
                return BadRequest();
            var comment = await handler.GetCommentAsync(id);
            int userId = Authorization.GetTokenId(User);
            if (userId == 0)
                Unauthorized();
            if (comment.UserId == userId || User.IsInRole(Roles.Admin)){
                comment.Content = commentDto.Content;
                var resp = await handler.UpdateCommentAsync(comment);
                if (!resp) 
                    ValidationProblem();
                return NoContent();
            }
            return BadRequest();
        }

        // DELETE api/v1/Comments/5
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<ActionResult> Delete([FromServices] CommentsHandler handler, int id) {
            var comment = await handler.GetCommentAsync(id);
            if (comment == null)
                return NotFound();
            int userId = Authorization.GetTokenId(User);
            if (userId == 0)
                Unauthorized();
            if (userId == comment.UserId || User.IsInRole(Roles.Admin)){
                await handler.DeleteCommentAsync(id);
                return NoContent();
            }
            return NotFound();
        }
    }
}
