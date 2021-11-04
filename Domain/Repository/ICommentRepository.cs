using Domain.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository
{
    public interface ICommentRepository
    {
        Task<IEnumerable<Comment>> GetCommentsAsync(int articleId);
        Task<Comment> GetCommentAsync(int id);
        Task CreateCommentAsync(Comment entity);
        Task UpdateCommentAsync(Comment entity);
        Task DeleteCommentAsync(int id);
        void DeleteComment(Comment entity);

    }
}
