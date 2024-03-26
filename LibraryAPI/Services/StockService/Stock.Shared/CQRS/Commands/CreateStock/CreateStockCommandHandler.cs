using MediatR;
using Stock.Shared.Entities.ViewModels;
using Stock.Shared.Services.Abstractions;
using Stock.Shared.Services.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.CQRS.Commands.CreateStock
{
    public class CreateStockCommandHandler(IEventStoreService eventStoreService) : IRequestHandler<CreateStockCommandRequest, CreateStockCommandResponse>
    {
        public async Task<CreateStockCommandResponse> Handle(CreateStockCommandRequest request, CancellationToken cancellationToken)
        {
            request.stockId = Guid.NewGuid().ToString();
            await eventStoreService.appendToStreamAsync("stock-stream", [eventStoreService.generateEventData(request)]);
            return new CreateStockCommandResponse() { succeeded = true, message = "Stok oluşturma işleminiz başarı ile gerçekleştirildi."};
        }
    }
}
