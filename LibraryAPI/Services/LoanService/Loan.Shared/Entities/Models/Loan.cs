

using RabbitMQ.Shared.Enums;

namespace Loan.Shared.Entities.Models
{
    public class Loan
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string userName { get; set; }
        public string bookId { get; set; }
        public int count { get; set; } = 1;
        public LoanStateEnum state { get; set; }
        public DateTime loanDate { get; set; }
        public DateTime? returnDate { get; set; }
    }
}
