using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Repository.Entities
{
    public class Reaction
    {
        public int ReactionId { get; set; }
        
        public int ArticleId { get; set; }
        public Article Article { get; set; }

        public int CommentId { get; set; }
        public Comment Comment { get; set; }

        public bool Like { get; set; } // Gustó o no gustó.
        public User User { get; set; } // Usuario que indicó que le gusta
    }
}
