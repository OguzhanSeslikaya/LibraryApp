using Loan.API.Contexts;
using Loan.Shared.Entities.Models;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Shared.Events.Loan;
using RabbitMQ.Shared.Events.Stock;
using System.Text.Json;

namespace Loan.API.Consumers
{
    public class LoanStateEventConsumer(LoanAPIDbContext loanAPIDbContext) : IConsumer<LoanStateEvent>
    {
        public async Task Consume(ConsumeContext<LoanStateEvent> context)
        {
            if (!(await loanAPIDbContext.stockInboxes.AnyAsync(a => a.idempotentToken == context.Message.idempotentToken)))
            {
                await loanAPIDbContext.stockInboxes.AddAsync(new()
                {
                    idempotentToken = context.Message.idempotentToken,
                    payload = JsonSerializer.Serialize(context.Message),
                    processed = false,
                    typeName = context.Message.typeName
                });
            }

            await loanAPIDbContext.SaveChangesAsync();

            var stockInboxes = await loanAPIDbContext.stockInboxes.Where(a => !a.processed).ToListAsync();

            if (stockInboxes.Any())
            {
                foreach (var item in stockInboxes)
                {
                    if (item.typeName != nameof(LoanStateEvent))
                        continue;

                    LoanStateEvent loanStateEvent = JsonSerializer.Deserialize<LoanStateEvent>(item.payload);

                    if (loanStateEvent != null)
                    {
                        Loan.Shared.Entities.Models.Loan? loan = await loanAPIDbContext.loans
                        .Where(a => a.id == loanStateEvent.loanId)
                        .FirstOrDefaultAsync();

                        if (loan != null)
                        {
                            loan.state = loanStateEvent.state;
                            StockInbox? stockInbox = await loanAPIDbContext.stockInboxes
                            .Where(a => a.idempotentToken == loanStateEvent.idempotentToken)
                            .FirstOrDefaultAsync();

                            if (stockInbox != null)
                            {
                                stockInbox.processed = true;
                            }
                        }
                    }
                }
                await loanAPIDbContext.SaveChangesAsync();
            }

        }
    }
}
