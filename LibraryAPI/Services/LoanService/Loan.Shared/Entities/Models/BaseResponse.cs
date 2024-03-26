using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loan.Shared.Entities.Models
{
    public class BaseResponse
    {
        public bool succeeded { get; set; }
        public string message { get; set; }
    }
}
