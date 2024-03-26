using RabbitMQ.Shared.Enums;

namespace Loan.Shared.Entities.ViewModels
{
    public class LoanVM
    {
        public string id { get; set; }
        public string userId { get; set; }
        public string username { get; set; }
        public string bookId { get; set; }
        public string state { get; set; }
        public DateTime loanDate { get; set; }
        public DateTime? returnDate { get; set; }
    }
}
