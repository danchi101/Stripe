using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Stripe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        //// GET api/values
        //[HttpGet]
        //public ActionResult<IEnumerable<string>> Get()
        //{
        //    return new string[] { "value1", "value2" };
        //}

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet]
        [ProducesResponseType(200)]
        public async Task<IActionResult> ChargeWithStripeAsync()
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey("sk_test_Px0EX1L0tqD9ZBKHT2HmaFTI");

            var options = new ChargeCreateOptions
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
                charge = service.Create(options);
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
