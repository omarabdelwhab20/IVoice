using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace IVoive_WEB.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbcontext context;
        [BindProperty]
        public CartOrderViewModel details { get; set; }
        public CartController(ApplicationDbcontext context)
        {
            this.context = context;
        }
        public IActionResult Index()
        {
            details = new CartOrderViewModel()
            {
                OrderHeader = new Order()
            };
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var UserId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
            details = new()
            {
                ListofCart = context.Carts
                    .Include(p => p.Product)
                    .Where(x => x.ApplicationUserId == UserId)
                    .ToList(),
            OrderHeader = new()
            };
            foreach (var cart in details.ListofCart)
            {
              
                details.OrderHeader.OrderTotal += (cart.Product.Price * cart.Count);

            }

            return View(details);
        }
 
    }
}
