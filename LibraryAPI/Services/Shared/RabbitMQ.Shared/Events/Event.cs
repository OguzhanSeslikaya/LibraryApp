using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared.Events
{
    public class Event
    {
        public string idempotentToken { get; set; }
        public int count { get; set; } = 1;
        public string typeName { get; set; }
    }
}
