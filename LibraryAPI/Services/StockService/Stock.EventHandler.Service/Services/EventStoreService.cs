using EventStore.Client;
using MongoDB.Driver;
using Stock.Shared.CQRS.Commands.CreateStock;
using Stock.Shared.CQRS.Commands.CurrentStockChange;
using Stock.Shared.CQRS.Commands.TotalStockChange;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.EventHandler.Service.Services
{
    public class EventStoreService(IEventStoreService eventStoreService, IMongoDbService mongoDbService) : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await eventStoreService.subscribeToStreamAsync("stock-stream", 
                async (StreamSubscription, ResolvedEvent, CancellationToken) =>
            {
                string eventType = ResolvedEvent.Event.EventType;
                object @event = JsonSerializer.Deserialize(ResolvedEvent.Event.Data.ToArray(), Assembly.Load("Stock.Shared").GetTypes().FirstOrDefault(t => t.Name == eventType));

                var stockCollection = mongoDbService.GetCollection<Shared.Entities.Models.Stock>("stock");
                switch (@event)
                {
                    case CreateStockCommandRequest e:
                        Shared.Entities.Models.Stock stock = await(await stockCollection.FindAsync(a => a.stockId == e.stockId)).FirstOrDefaultAsync();
                        if (stock == null)
                        {
                            await stockCollection.InsertOneAsync(new Shared.Entities.Models.Stock()
                            {
                                stockId = e.stockId,
                                bookName = e.bookName,
                                currentQuantity = e.quantity,
                                totalQuantity = e.quantity,
                            });
                        }
                        else
                        {
                            stock.currentQuantity = e.quantity;
                            stock.totalQuantity = e.quantity;
                            await stockCollection.FindOneAndReplaceAsync(a => a.stockId == stock.stockId,stock);
                        }
                        break;
                    case TotalStockChangeCommandRequest e:
                        var stockForTotal = await getStockById(stockCollection,e.stockId);
                        if(stockForTotal != null)
                        {
                            stockForTotal.totalQuantity += e.count;
                            stockForTotal.currentQuantity += e.count;
                            await stockCollection.FindOneAndReplaceAsync(a => a.stockId == e.stockId, stockForTotal);
                        }
                        break;
                    case CurrentStockChangeCommandRequest e:
                        var stockForCurrent = await getStockById(stockCollection, e.stockId);
                        if (stockForCurrent != null)
                        {
                            stockForCurrent.currentQuantity += e.count;
                            await stockCollection.FindOneAndReplaceAsync(a => a.stockId == e.stockId, stockForCurrent);
                        }
                        break;
                }
            });
        }
        private async Task<Shared.Entities.Models.Stock> getStockById(IMongoCollection<Shared.Entities.Models.Stock> stockCollection,string stockId)
        {
            return await(await stockCollection.FindAsync(a => a.stockId == stockId)).FirstAsync();
        }
        private FilterDefinition<Shared.Entities.Models.Stock> generateStockFilter(string stockId)
        {
            return Builders<Shared.Entities.Models.Stock>.Filter.Eq(a => a.stockId, stockId);
        }
    }
}
