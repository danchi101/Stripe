using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stripe.Stripe
{
    public class ErrorModel
    {
        public string ErrorCode { get; set; }
        public string ErrorDescription { get; set; }
        public string ErrorAction { get; set; }
    }
}
