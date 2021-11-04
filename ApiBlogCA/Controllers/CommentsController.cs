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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ApiBlogCA.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize(Roles = Roles.AdminOrUser)]
    public class CommentsController : ControllerBase
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CommentsController(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/v1/Comments
        [HttpGet]
        public async Task<IEnumerable<CommentDto>> GetAsync([FromServices] CommentsHandler handler, CommentDto commentDto) {
            return await handler.GetCommentsAsync(commentDto);
        }

        // GET: api/v1/Comments/5
        [HttpGet("{id}")]
        public async Task<Comment> GetCommentAsync([FromServices] CommentsHandler handler, int id) {
            return await handler.GetCommentAsync(id);
        }


        // GET: api/v1/Comments/Reactions
        [HttpGet("Reactions")]
        public async Task<ActionResult<Reactions>> GetReactionsAsync([FromServices] CommentsHandler handler, int id) {
            var reactions = await handler.GetReactionsAsync(id);
            if (reactions == null)
                return NotFound();
            return Ok(reactions);
        }

        // POST api/v1/Comments
        [HttpPost]
        public async Task<ActionResult> PostAsync([FromServices] CommentsHandler handler, CommentDto commentDto) {
            if (commentDto == null)
                BadRequest();
            var userId = Convert.ToInt32(User.Claims.First(s => s.Type == "id").Value);
            var commentId = await handler.CreateCommentAsync(commentDto, userId);

            return Created("GetComment", new { id = commentId });
        }

        // PUT: api/v1/Comments/Reactions
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Reactions")]
        public async Task<IActionResult> PutReactions([FromServices] CommentsHandler handler, Reactions reactions) {
            if (reactions == null)
                return BadRequest();
            await handler.UpdateReactionsAsync(reactions);
            return NoContent();
        }

        // PUT api/v1/Comments/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put([FromServices] CommentsHandler handler, int id, CommentDto commentDto) {
            if (commentDto  == null)
                return BadRequest();
            var comment = await handler.GetCommentAsync(id);
            int userId = Convert.ToInt32(User.Claims.First(s => s.Type == "id").Value);
            if (comment.UserId == userId || User.IsInRole(Roles.Admin))
            {
                comment.Content = commentDto.Content;
                await handler.UpdateCommentAsync(comment);
                return NoContent();
            }
            return BadRequest();
        }

        // DELETE api/v1/Comments/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete([FromServices] CommentsHandler handler, int id) {
            var comment = await handler.GetCommentAsync(id);
            if (comment == null)
                return NotFound();
            int userId = Convert.ToInt32(User.Claims.First(s => s.Type == "id").Value);
            if (userId == comment.UserId || User.IsInRole(Roles.Admin))
            {
                await handler.DeleteCommentAsync(id);
                return NoContent();
            }
            return NotFound();
        }
    }
}
