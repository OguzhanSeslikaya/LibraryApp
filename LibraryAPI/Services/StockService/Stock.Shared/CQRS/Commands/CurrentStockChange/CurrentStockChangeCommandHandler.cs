using MediatR;
using Stock.Shared.Services.Abstractions;
using Stock.Shared.Services.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.CQRS.Commands.CurrentStockChange
{
    public class CurrentStockChangeCommandHandler(IEventStoreService eventStoreService) : IRequestHandler<CurrentStockChangeCommandRequest, CurrentStockChangeCommandResponse>
    {
        public async Task<CurrentStockChangeCommandResponse> Handle(CurrentStockChangeCommandRequest request, CancellationToken cancellationToken)
        {
            await eventStoreService.appendToStreamAsync("stock-stream", [eventStoreService.generateEventData(request)]);
            return new CurrentStockChangeCommandResponse() { succeeded = true, message = "Kullanılabilir stoğu değiştirme işleminiz başarı ile gerçekleşmiştir." };
        }
    }
}
