using Amazon.Runtime.Internal;
using MediatR;

namespace Stock.Shared.CQRS.Queries.GetStocksForAdmin
{
    public class GetStocksForAdminQueryRequest : IRequest<GetStocksForAdminQueryResponse>
    {
    }
}