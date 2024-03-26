using Amazon.Runtime.Internal;
using MediatR;

namespace Stock.Shared.CQRS.Commands.TotalStockChange
{
    public class TotalStockChangeCommandRequest : IRequest<TotalStockChangeCommandResponse>
    {
        public string stockId { get; set; }
        public int count { get; set; }
    }
}