using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Repository.Entities
{
    public class Comment
    {
        public int CommentId { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
        public int ArticleId { get; set; }
        public Article Article { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
