using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RabbitMQ.Shared
{
    public static class RabbitMQSettings
    {
        public const string stock_LoanCreatedEventQueue = "stock-LoanCreatedEventQueue";
        public const string loan_StockStateEventQueue = "loan-StockStateEventQueue";
        public const string stock_LoanReturnEventQueue = "stock-LoanReturnEventQueue";
    }
}
