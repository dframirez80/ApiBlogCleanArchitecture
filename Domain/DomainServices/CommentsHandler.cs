using AutoMapper;
using Domain.Constants;
using Domain.Models;
using Domain.Models.Dtos;
using Domain.Repository;
using Domain.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DomainServices
{
    public class CommentsHandler
    {
        private readonly IUnitOfWork _uow;
        private readonly IMapper _mapper;
        public CommentsHandler(IUnitOfWork uow, IMapper mapper) {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(CommentDto commentDto) {
            var comments = await _uow.Comments.GetCommentsAsync(commentDto.ArticleId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<Comment> GetCommentAsync(int id) {
            return await _uow.Comments.GetCommentAsync(id);
        }

        public async Task<Reactions> GetReactionsAsync(int id) {
            var comment = await _uow.Comments.GetCommentAsync(id);
            if (comment == null)
                return null;
            Reactions reactions = new()
            {
                Id = comment.CommentId,
                Likes = comment.Likes,
                Dislikes = comment.Dislikes
            };
            return reactions;
        }

        public async Task<int> CreateCommentAsync(CommentDto commentDto, int userComment) {
            var comment = _mapper.Map<Comment>(commentDto); 
            comment.Created = DateTime.UtcNow.AddHours(UTC.GmtBuenosAires);
            comment.UserId = userComment;
            await _uow.Comments.CreateCommentAsync(comment);
            await _uow.CommitAsync();
            return comment.CommentId;
        }

        public async Task UpdateReactionsAsync(Reactions reactions) {
            if (reactions == null)
                return;
            var comment = await _uow.Comments.GetCommentAsync(reactions.Id);
            if (reactions.Likes > 0)
                comment.Likes++;
            if (reactions.Dislikes > 0)
                comment.Dislikes++;
            await _uow.CommitAsync();
        }

        public async Task UpdateCommentAsync(Comment comment) {
            await _uow.Comments.UpdateCommentAsync(comment);
            await _uow.CommitAsync();
        }

        public async Task DeleteCommentAsync(int id) {
            await _uow.Comments.DeleteCommentAsync(id);
        }
    }
}
