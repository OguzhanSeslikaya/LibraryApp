namespace Loan.Shared.Entities.Models
{
    public class StockInbox
    {
        public string idempotentToken { get; set; }
        public string payload { get; set; }
        public bool processed { get; set; }
        public string typeName { get; set; }
    }
}
