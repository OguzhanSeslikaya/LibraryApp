using Amazon.Runtime.Internal;
using MediatR;

namespace Stock.Shared.CQRS.Queries.GetStocks
{
    public class GetStocksQueryRequest : IRequest<GetStocksQueryResponse>
    {
    }
}