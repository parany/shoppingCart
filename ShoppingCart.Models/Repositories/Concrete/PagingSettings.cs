namespace ShoppingCart.Models.Repositories.Concrete
{
    public class PagingSettings
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public int NumberOfRecords { get; set; }
        public int TotalNumberOfRecords { get; set; }
    }
}