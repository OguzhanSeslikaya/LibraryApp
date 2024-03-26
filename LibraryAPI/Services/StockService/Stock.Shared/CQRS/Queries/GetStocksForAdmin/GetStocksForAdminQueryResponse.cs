namespace Stock.Shared.CQRS.Queries.GetStocksForAdmin
{
    public class GetStocksForAdminQueryResponse
    {
        public List<Entities.Models.Stock> stocks { get; set; }
    }
}