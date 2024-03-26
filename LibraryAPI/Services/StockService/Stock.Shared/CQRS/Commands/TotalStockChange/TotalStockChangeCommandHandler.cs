using MediatR;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static EventStore.Client.StreamMessage;

namespace Stock.Shared.CQRS.Commands.TotalStockChange
{
    public class TotalStockChangeCommandHandler(IEventStoreService eventStoreService) : IRequestHandler<TotalStockChangeCommandRequest, TotalStockChangeCommandResponse>
    {
        public async Task<TotalStockChangeCommandResponse> Handle(TotalStockChangeCommandRequest request, CancellationToken cancellationToken)
        {
            await eventStoreService.appendToStreamAsync("stock-stream", [eventStoreService.generateEventData(request)]);
            return new TotalStockChangeCommandResponse() { succeeded = true, message = "Toplam stoğu değiştirme işleminiz başarı ile gerçekleşti."};
        }
    }
}
