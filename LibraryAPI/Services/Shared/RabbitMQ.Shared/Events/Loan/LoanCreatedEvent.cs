using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared.Events.Loan
{
    public class LoanCreatedEvent : Event
    {
        public string bookId { get; set; }
        public string loanId { get; set; }

    }
}
