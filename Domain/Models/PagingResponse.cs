using System.Collections.Generic;

namespace Domain.Models
{
    public class PagingResponse<T> where T : class
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public IEnumerable<T> ListItems { get; set; }
        public bool HasNextPage { get; set; } // Puede ser util indicarle si hay más páginas o si ya se llegó a la última.
    }
}
