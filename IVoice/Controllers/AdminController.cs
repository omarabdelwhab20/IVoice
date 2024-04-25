using IVoice.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository adminRepository;
        UserManager<ApplicationUser> userManager;
        public AdminController(IAdminRepository adminRepository , UserManager<ApplicationUser> userManager) 
        {
            this.adminRepository = adminRepository ;
            this.adminRepository = adminRepository;
        }

        public IActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public IActionResult OutOfStock()
        {
            var outOfStockProducts = adminRepository.GetProductsRunningOutOfStock();
            return Json(outOfStockProducts);
        }

        public IActionResult UsersDisplay()
        {
            var usersWithUserRole = userManager.GetUsersInRoleAsync("User").Result;
            var users = adminRepository.UsersDisplay();
            return View(users);
        }

        public IActionResult OrdersDisplay()
        {
            var orders = adminRepository.OrdersDisplay();
            return View(orders);
        }

    }
}
