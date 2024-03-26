using Loan.Shared.Entities.Models;
using MassTransit;
using MassTransit.Transports;
using Quartz;
using RabbitMQ.Shared;
using RabbitMQ.Shared.Events.Loan;
using System.Dynamic;
using System.Text.Json;

namespace Loan.Outbox.Publisher.Service.Jobs
{
    public class LoanOutboxPublishJob(ISendEndpointProvider sendEndpointProvider) : IJob
    {
        public async Task Execute(IJobExecutionContext context)
        {
            if (LoanOutboxSingletonDatabase.DataReaderState)
            {
                LoanOutboxSingletonDatabase.DataReaderBusy();

                List<LoanOutbox> loanOutboxes = (await LoanOutboxSingletonDatabase.QueryAsync<LoanOutbox>
                    ($@"
SELECT * FROM ""loanOutboxes""
WHERE ""processedDate"" IS NULL
ORDER BY ""occuredOn"" ASC
"                   )).ToList();

                foreach (var loanOutbox in loanOutboxes)
                {
                    switch (loanOutbox.type)
                    {
                        case nameof(LoanCreatedEvent):
                            LoanCreatedEvent loanCreatedEvent = JsonSerializer.Deserialize<LoanCreatedEvent>(loanOutbox.payload);
                            if (loanCreatedEvent != null)
                            {
                                await (await getSendEndPoint(RabbitMQSettings.stock_LoanCreatedEventQueue)).Send(loanCreatedEvent);
                                await executeProcessedDate(loanOutbox.idempotentToken);
                            }
                            break;
                        case nameof(ReturnLoanEvent):
                            ReturnLoanEvent returnLoanEvent = JsonSerializer.Deserialize<ReturnLoanEvent>(loanOutbox.payload);
                            if (returnLoanEvent != null)
                            {
                                await (await getSendEndPoint(RabbitMQSettings.stock_LoanReturnEventQueue)).Send(returnLoanEvent);
                                await executeProcessedDate(loanOutbox.idempotentToken);
                            }
                            break;
                    }
                   
                }
                LoanOutboxSingletonDatabase.DataReaderReady();
            }
        }


        /// 
        

        private async Task<ISendEndpoint> getSendEndPoint(string queue)
        {
            return await sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{queue}"));
        }
        private async Task executeProcessedDate(string idempotentToken)
        {
            string sql = $@"
UPDATE ""loanOutboxes"" SET ""processedDate"" = NOW() 
WHERE ""idempotentToken"" = '{idempotentToken}'";
            await LoanOutboxSingletonDatabase.ExecuteAsync(sql);
        }
    }

    
}