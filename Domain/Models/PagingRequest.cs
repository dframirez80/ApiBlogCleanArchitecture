
namespace Domain.Models
{
    public class PagingRequest
    {
        public int Page { get; set; } = Constants.Paging.DefaultPaging;
        public int Quantity { get; set; } = Constants.Paging.DefaultQuantity;
    }
}
