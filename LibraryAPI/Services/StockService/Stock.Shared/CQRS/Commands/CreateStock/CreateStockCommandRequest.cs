using Amazon.Runtime.Internal;
using MediatR;

namespace Stock.Shared.CQRS.Commands.CreateStock
{
    public class CreateStockCommandRequest : IRequest<CreateStockCommandResponse>
    {
        public string? stockId { get; set; }
        public string bookName { get; set; }
        public int quantity { get; set; }
    }
}