using IVoive_WEB.Data;
using IVoive_WEB.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace IVoive_WEB.ViewComponents
{
    public class CartViewComponent : ViewComponent
    {
        private readonly ApplicationDbcontext context;

        public CartViewComponent(ApplicationDbcontext context)
        {
            this.context = context;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
          
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

                if (claims != null)
                {
                    if (HttpContext.Session.GetInt32("SessionCart") != null)
                    {
                        return View(HttpContext.Session.GetInt32("SessionCart"));
                    }
                    else
                    {
                        HttpContext.Session.SetInt32("SessionCart", context
                            .Carts.Where(x => x.ApplicationUserId == claims.Value).ToList().Count);
                        return View(HttpContext.Session.GetInt32("SessionCart"));
                    }
                }
                else
                {
                    HttpContext.Session.Clear();
                    return View(0);
                }
                
        }
    }
}
