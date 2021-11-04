using Domain.Constants;
using System.ComponentModel.DataAnnotations;

namespace Domain.Models.Dtos
{
    public class ArticleDto
    {
        [Required(ErrorMessage = ErrorMessage.Title)]
        public string Title { get; set; }

        [Required(ErrorMessage = ErrorMessage.Keyword)]
        public string Keyword { get; set; }
        
        [Required(ErrorMessage = ErrorMessage.Content)]
        public string Content { get; set; }
    }
}
