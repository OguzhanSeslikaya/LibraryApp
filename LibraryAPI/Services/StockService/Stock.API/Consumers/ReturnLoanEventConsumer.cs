using MassTransit;
using MongoDB.Driver;
using RabbitMQ.Shared.Enums;
using RabbitMQ.Shared.Events.Loan;
using RabbitMQ.Shared.Events.Stock;
using Stock.Shared.CQRS.Commands.CurrentStockChange;
using Stock.Shared.Entities.Models;
using Stock.Shared.Services.Abstractions;
using Stock.Shared.Services.Concretes;
using System.Text.Json;

namespace Stock.API.Consumers
{
    public class ReturnLoanEventConsumer(IMongoDbService mongoDbService, IEventStoreService eventStoreService) : IConsumer<ReturnLoanEvent>
    {
        public async Task Consume(ConsumeContext<ReturnLoanEvent> context)
        {
            var loanInboxCollection = mongoDbService.GetCollection<LoanInbox>("loanInbox");

            bool idempotentControl = await(await loanInboxCollection
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

            List<LoanInbox> loanInboxes = await(await loanInboxCollection.FindAsync(a => a.processed == false)).ToListAsync();

            foreach (var loanInbox in loanInboxes)
            {
                if (loanInbox.typeName != nameof(ReturnLoanEvent))
                    continue;
                ReturnLoanEvent returnLoanEvent = JsonSerializer.Deserialize<ReturnLoanEvent>(loanInbox.payload);
                

                var stockCollection = mongoDbService.GetCollection<Shared.Entities.Models.Stock>("stock");

                CurrentStockChangeCommandRequest @event = new()
                {
                    count = returnLoanEvent.count,
                    stockId = returnLoanEvent.stockId
                };
                await eventStoreService.appendToStreamAsync("stock-stream", [eventStoreService.generateEventData(@event)] );

                await loanInboxCollection.UpdateOneAsync(
                    Builders<LoanInbox>.Filter.Eq(a => a.idempotentToken, returnLoanEvent.idempotentToken),
                    Builders<LoanInbox>.Update.Set(a => a.processed, true)
                    );
            }

        }
    }
}
