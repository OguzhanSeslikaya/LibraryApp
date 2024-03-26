using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Shared.Entities.Models
{
    public class LoanOutbox
    {
        public string idempotentToken { get; set; }
        public DateTime occuredOn { get; set; }
        public DateTime? processedDate { get; set; }
        public string type { get; set; }
        public string payload { get; set; }
    }
}
