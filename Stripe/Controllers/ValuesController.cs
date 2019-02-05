using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Stripe;

namespace Stripe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly StripeOptions options;

        public ValuesController(IOptions<StripeOptions>options)
        {
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }
      

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChargeWithStripeAsync()
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(options.ApiKey);

            var chargeCreateOptions = new ChargeCreateOptions
            {
                Amount = 999,
                Currency = "usd",
                SourceId = "tok_chargeDeclinedExpiredCard",
                ReceiptEmail = "danchi101@gmail.com",
                Metadata = new Dictionary<string, string>() { { "OrderId", "6735" } },
            };
            var service = new ChargeService();
            service.ExpandBalanceTransaction = true;
            service.ExpandCustomer = true;
            service.ExpandInvoice = true;
            Charge charge;

            //stripe api errors
            try
            {
                // Use Stripe's library to make request
                charge = service.Create(chargeCreateOptions);
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case "card_error":
                        Console.WriteLine("Code: " + e.StripeError.Code);
                        Console.WriteLine("Message: " + e.StripeError.Message);
                        break;
                    case "api_connection_error":
                        break;
                    case "api_error":
                        break;
                    case "authentication_error":
                        break;
                    case "invalid_request_error":
                        break;
                    case "rate_limit_error":
                        break;
                    case "validation_error":
                        break;
                    default:
                        // Unknown Error Type
                        break;
                }
            }

            return Ok();
        }
    }
}
