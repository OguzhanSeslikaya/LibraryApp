using MediatR;
using MongoDB.Driver;
using Stock.Shared.Services.Abstractions;
using Stock.Shared.Services.Concretes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.CQRS.Queries.GetStocksForAdmin
{
    public class GetStocksForAdminQueryHandler(IMongoDbService mongoDbService) : IRequestHandler<GetStocksForAdminQueryRequest, GetStocksForAdminQueryResponse>
    {
        public async Task<GetStocksForAdminQueryResponse> Handle(GetStocksForAdminQueryRequest request, CancellationToken cancellationToken)
        {
            return new GetStocksForAdminQueryResponse() { stocks = await mongoDbService
                .GetCollection<Shared.Entities.Models.Stock>("stock")
                .Find(Builders<Shared.Entities.Models.Stock>.Filter.Empty)
                .ToListAsync()
        };
        }
    }
}
