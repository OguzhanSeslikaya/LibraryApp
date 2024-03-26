using MediatR;

namespace Stock.Shared.CQRS.Commands.CurrentStockChange
{
    public class CurrentStockChangeCommandRequest : IRequest<CurrentStockChangeCommandResponse>
    {
        public string stockId { get; set; }
        public int count { get; set; }
    }
}