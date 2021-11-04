
namespace Domain.Models
{
    public class PagingRequest
    {
        public int Page { get; set; } = 1;
        public int Quantity { get; set; } = 1;
    }
}
