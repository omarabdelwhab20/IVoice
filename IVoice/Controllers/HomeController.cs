using IVoice.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Stripe.Checkout;
using Stripe;
using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;

namespace IVoice.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StripeSettings _stripeSettings;

        public HomeController(IOptions<StripeSettings> stripeSettings, ILogger<HomeController> logger)
        {
            _stripeSettings = stripeSettings.Value;
            _logger = logger;

        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AboutUs()
        {
            return View();
        }

        public IActionResult CreateCheckoutSession(string amount)
        {
            try
            {
                var currency = "usd"; // Currency code
                var successUrl = "http://localhost:25467/Cart/Checkout";
                var cancelUrl = "http://localhost:25467/Cart/GetUserCart";
                StripeConfiguration.ApiKey = _stripeSettings.SecretKey;

                var options = new SessionCreateOptions
                {
                    PaymentMethodTypes = new List<string>
            {
                "card"
            },
                    LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = currency,
                        UnitAmount = Convert.ToInt32(amount) * 100,  // Amount in smallest currency unit (e.g., cents)
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Total Price",

                        }
                    },
                    Quantity = 1
                }
            },
                    Mode = "payment",
                    SuccessUrl = successUrl,
                    CancelUrl = cancelUrl
                };

                var service = new SessionService();
                var session = service.Create(options);

                return Redirect(session.Url);
            }
            catch (StripeException ex)
            {
                // Log the error or handle it appropriately
                // For example, you can return a view with an error message
                return View("Error", ex.Message);
            }
            catch (Exception ex)
            {
                // Handle other types of exceptions (e.g., network issues) here
                // For example, you can return a view with a generic error message
                return View("InternetErrorPage");
                     
            }
        }




        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
