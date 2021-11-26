using System.Collections.Generic;

namespace Domain.Models
{
    public class PagingResponse<T> where T : class
    {
        public int TotalItems { get; set; }
        public int CurrentPage { get; set; }
        public bool NextPage { get; set; }
        public IEnumerable<T> ListItems { get; set; }
    }
}
