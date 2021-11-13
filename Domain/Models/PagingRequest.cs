
namespace Domain.Models
{
    public class PagingRequest
    {
        public const int QuantityMin = 1;
        public int Page { get; set; } = 1;
        public int Quantity { get; set; } = QuantityMin;
        public int PageSize { get; set; } // Al paginar, es recomendable que el consumir pueda decidir el tamaño de página.
    }
}
