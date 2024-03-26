using RabbitMQ.Shared.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared.Events.Stock
{
    public class LoanStateEvent : Event
    {
        public string loanId { get; set; }
        public LoanStateEnum state { get; set; }
    }
}
