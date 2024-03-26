using MassTransit;
using MassTransit.Transports;
using MongoDB.Driver;
using Quartz;
using RabbitMQ.Shared;
using RabbitMQ.Shared.Events.Stock;
using Stock.Shared.Entities.Models;
using Stock.Shared.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Stock.Outbox.Publisher.Service.Jobs
{
    public class StockOutboxPublishJob(IMongoDbService mongoDbService,ISendEndpointProvider sendEndpointProvider) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            var stockOutboxCollection = mongoDbService.GetCollection<StockOutbox>("stockOutboxes");

            var stockOutboxes = (await(await stockOutboxCollection
                .FindAsync(a => a.processedDate == null))
                .ToListAsync())
                .OrderBy(a => a.occuredOn);

            foreach (var stockOutbox in stockOutboxes)
            {
                if(stockOutbox.type == nameof(LoanStateEvent))
                {
                    LoanStateEvent loanStateEvent = JsonSerializer.Deserialize<LoanStateEvent>(stockOutbox.payload);
                    if (loanStateEvent != null)
                    {

                        var sendEndPoint = await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{RabbitMQSettings.loan_StockStateEventQueue}"));
                        await sendEndPoint.Send(loanStateEvent);
                        await stockOutboxCollection.UpdateOneAsync(
                            Builders<StockOutbox>.Filter.Eq(a => a.idempotentToken,loanStateEvent.idempotentToken),
                            Builders<StockOutbox>.Update.Set(a => a.processedDate,DateTime.UtcNow)
                            );
                    }
                }
            }
        }
    }
}
