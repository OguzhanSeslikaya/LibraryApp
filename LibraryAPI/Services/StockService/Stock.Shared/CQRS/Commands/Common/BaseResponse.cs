using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Stock.Shared.CQRS.Commands.Common
{
    public class BaseResponse
    {
        public bool succeeded { get; set; }
        public string message { get; set; }
    }
}
