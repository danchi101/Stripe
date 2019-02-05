using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace Stripe.Stripe
{
    public class StripeService : IStripeService
    {
        private readonly StripeOptions options;
        public StripeService(IOptions<StripeOptions> options)
        {
            this.options = options.Value ?? throw new ArgumentNullException(nameof(options));
        }
        /// <summary>
        /// Process a stripe payment 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public async Task<StripeResponse> ProcessStripePaymentAsync(StripeRequest request)
        {
            // Set your secret key: remember to change this to your live secret key in production
            // See your keys here: https://dashboard.stripe.com/account/apikeys
            StripeConfiguration.SetApiKey(options.ApiKey);

            //create new charge 
            //todo:configure application costs 
            var chargeOptions = new ChargeCreateOptions
            {
                Amount = request.Amount,
                Currency = "usd",
                SourceId = request.Token,
                ReceiptEmail = request.Email,
                Metadata = request.Metadata,
                //immediately capture the charge
                Capture = true,
                Description = "",//todo:charge description that can be added to the web interface
            };

            //create a charge 
            var service = new ChargeService();

            Charge charge;
            StripeResponse response = new StripeResponse();

            //stripe api errors
            try
            {
                // Use Stripe's library to make request
                //todo:check out for outcome
                charge = await service.CreateAsync(chargeOptions);

                //return response
                response.success = true;
                response.Metadata = charge.Metadata;
                response.ChargeId = charge.Id;
                response.CreatedAt = charge.Created;

                return response;
            }
            catch (StripeException e)
            {
                switch (e.StripeError.ErrorType)
                {
                    case ErrorCodes.CardError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.CardError,
                            ErrorAction = e.StripeError.Code,
                            ErrorDescription = e.StripeError.Message
                        };
                        break;

                    case ErrorCodes.ApiConnectionError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.ApiConnectionError,
                            ErrorDescription = "Network Error Please retry again."
                        };
                        break;

                    case ErrorCodes.ApiError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.ApiError,
                            ErrorAction = e.StripeError.Code,
                            ErrorDescription = "Network Error Please retry again."
                        };
                        break;

                    case ErrorCodes.AuthenticationError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.AuthenticationError,
                            ErrorDescription = "A wrong api key was supplied."
                        };
                        break;

                    case ErrorCodes.InvalidRequestError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.InvalidRequestError,
                            ErrorDescription = "Bad request, check supplied parameters."
                        };
                        break;

                    case ErrorCodes.RateLimitError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.RateLimitError,
                            ErrorDescription = "Too many requests."
                        };
                        break;

                    case ErrorCodes.IdempotencyError:
                        response.ErrorModel = new ErrorModel
                        {
                            ErrorCode = ErrorCodes.IdempotencyError,
                            ErrorDescription = "Duplicate idempotency key."
                        };
                        break;

                    default:
                        // Unknown Error Type
                        break;
                }
                return response;
            }
        }

        /// <summary>
        /// Get details of a charge
        /// </summary>
        /// <param name="ChargeId"></param>
        /// <returns></returns>
        public async Task<Charge> RetrieveChargeAsync(string ChargeId)
        {
            StripeConfiguration.SetApiKey(options.ApiKey);

            var service = new ChargeService();
            return await service.GetAsync(ChargeId);
        }
    }
}
