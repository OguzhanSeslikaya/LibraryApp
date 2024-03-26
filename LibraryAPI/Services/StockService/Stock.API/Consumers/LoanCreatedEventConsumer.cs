using MassTransit;
using MongoDB.Driver;
using RabbitMQ.Shared.Enums;
using RabbitMQ.Shared.Events.Loan;
using RabbitMQ.Shared.Events.Stock;
using Stock.Shared.CQRS.Commands.CurrentStockChange;
using Stock.Shared.Entities.Models;
using Stock.Shared.Services.Abstractions;
using System.Text.Json;

namespace Stock.API.Consumers
{
    public class LoanCreatedEventConsumer(IMongoDbService mongoDbService, IEventStoreService eventStoreService) : IConsumer<LoanCreatedEvent>
    {
        public async Task Consume(ConsumeContext<LoanCreatedEvent> context)
        {
            var loanInboxCollection = mongoDbService.GetCollection<LoanInbox>("loanInbox");

            bool idempotentControl = await (await loanInboxCollection
                .FindAsync(a => a.idempotentToken == context.Message.idempotentToken))
                .AnyAsync();

            if (!idempotentControl)
            {
                await loanInboxCollection.InsertOneAsync(new LoanInbox()
                {
                    idempotentToken = context.Message.idempotentToken,
                    payload = JsonSerializer.Serialize(context.Message),
                    typeName = context.Message.typeName,
                    processed = false
                });
            }

            List<LoanInbox> loanInboxes = await (await loanInboxCollection.FindAsync(a => a.processed == false)).ToListAsync();

            foreach (var loanInbox in loanInboxes)
            {
                if (loanInbox.typeName != nameof(LoanCreatedEvent))
                    continue;

                LoanStateEvent loanStateEvent;
                LoanCreatedEvent loanCreatedEvent = JsonSerializer.Deserialize<LoanCreatedEvent>(loanInbox.payload);

                var stockCollection = mongoDbService.GetCollection<Shared.Entities.Models.Stock>("stock");
                var stock = await (await stockCollection.FindAsync(a => a.stockId == loanCreatedEvent.bookId)).FirstOrDefaultAsync();

                await loanInboxCollection.UpdateOneAsync(
                    Builders<LoanInbox>.Filter.Eq(a => a.idempotentToken, loanCreatedEvent.idempotentToken),
                    Builders<LoanInbox>.Update.Set(a => a.processed, true)
                    );

                var stockOutboxCollection = mongoDbService.GetCollection<StockOutbox>("stockOutboxes");

                if(stock == null)
                {
                    loanStateEvent = loanStateEventBuilder(LoanStateEnum.bookNotFound, loanCreatedEvent.loanId);
                    await stockOutboxCollection.InsertOneAsync(stockOutboxBuilder(loanStateEvent));
                    return;
                }

                if (stock.currentQuantity<=loanCreatedEvent.count)
                {
                    loanStateEvent = loanStateEventBuilder(LoanStateEnum.outOfStock, loanCreatedEvent.loanId);
                    await stockOutboxCollection.InsertOneAsync(stockOutboxBuilder(loanStateEvent));
                    return;
                }

                CurrentStockChangeCommandRequest @event = new()
                {
                    count = -loanCreatedEvent.count,
                    stockId = loanCreatedEvent.bookId
                };
                await eventStoreService.appendToStreamAsync("stock-stream", [eventStoreService.generateEventData(@event)]);

                loanStateEvent = loanStateEventBuilder(LoanStateEnum.borrowed, loanCreatedEvent.loanId);
                await stockOutboxCollection.InsertOneAsync(stockOutboxBuilder(loanStateEvent));
            }

        }

        private StockOutbox stockOutboxBuilder(LoanStateEvent loanStateEvent)
        {
            return new StockOutbox()
            {
                idempotentToken = loanStateEvent.idempotentToken,
                occuredOn = DateTime.UtcNow,
                payload = JsonSerializer.Serialize(loanStateEvent),
                type = nameof(LoanStateEvent)
            };
        }
        private LoanStateEvent loanStateEventBuilder(LoanStateEnum @enum,string loanId)
        {
            return new()
            {
                idempotentToken = Guid.NewGuid().ToString(),
                loanId = loanId,
                state = @enum,
                typeName = nameof(LoanStateEvent)
            };
        }
    }
}
