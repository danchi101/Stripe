using System;
using System.Collections.Generic;

namespace Stripe.Stripe
{
    /// <summary>
    /// Response model from stripe
    /// </summary>
    public class StripeResponse
    {
        /// <summary>
        /// an indicator on the success of a stripe charge request
        /// </summary>
        public bool success { get; set; } = false;

        /// <summary>
        /// Error model 
        /// </summary>
        public ErrorModel ErrorModel { get; set; }

        /// <summary>
        /// Amount to be charged 
        /// </summary>
        public long AmountCharged { get; set; }

        /// <summary>
        /// unique identifier for a charge
        /// </summary>
        public string ChargeId { get; set; }

        /// <summary>
        /// date time the charge was created 
        /// </summary>
        public DateTime CreatedAt { get; set; }

        /// <summary>
        /// meta-data that can be added to a request
        /// </summary>
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();
    }
}
