using Stock.Shared.Entities.ViewModels;

namespace Stock.Shared.CQRS.Queries.GetStocks
{
    public class GetStocksQueryResponse
    {
        public List<StockVM> stocks { get; set; }
    }
}