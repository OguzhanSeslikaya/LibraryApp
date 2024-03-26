using MediatR;
using MongoDB.Driver;
using Stock.Shared.Entities.ViewModels;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.CQRS.Queries.GetStocks
{
    public class GetStocksQueryHandler(IMongoDbService mongoDbService) : IRequestHandler<GetStocksQueryRequest, GetStocksQueryResponse>
    {
        public async Task<GetStocksQueryResponse> Handle(GetStocksQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetStocksQueryResponse()
            {
                stocks = (await mongoDbService
                .GetCollection<Shared.Entities.Models.Stock>("stock")
                .Find(Builders<Shared.Entities.Models.Stock>.Filter.Empty)
                .ToListAsync())
                .Select(a => new StockVM()
                {
                    id = a.stockId,
                    bookName = a.bookName,
                    currentQuantity = a.currentQuantity
                })
                .ToList()
            };
        }
    }
}

