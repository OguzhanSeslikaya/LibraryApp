using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared.Events.Loan
{
    public class ReturnLoanEvent : Event
    {
        public string stockId { get; set; }
        public int count { get; set; } = 1;
    }
}
