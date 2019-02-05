using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stripe.Stripe
{
    /// <summary>
    /// stripe request model 
    /// </summary>
    public class StripeRequest
    {
        /// <summary>
        /// email to send stripe receipt to 
        /// </summary>      
        public string Email { get; set; }

        /// <summary>
        /// amount to be charged in USD
        /// </summary>
        public long Amount { get; set; }

        /// <summary>
        /// stripe token from front end client
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// metadata to be added for richer information in a charge
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
