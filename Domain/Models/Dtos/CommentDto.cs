using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Models.Dtos
{
    public class CommentDto
    {
        public string Content { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    }
}
