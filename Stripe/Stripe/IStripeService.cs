using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Stripe.Stripe
{
    public interface IStripeService
    {
        /// <summary>
        /// Process stripe request 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        Task<StripeResponse> ProcessStripePaymentAsync(StripeRequest request);

        /// <summary>
        /// retrieve a charge model
        /// </summary>
        /// <param name="ChargeId">id of the charge</param>
        /// <returns></returns>
        Task<Charge> RetrieveChargeAsync(string ChargeId);
    }
}
