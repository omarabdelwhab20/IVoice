using IVoice.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
    public class AdminController : Controller
    {
        IAdminRepository adminRepository;
        public AdminController(IAdminRepository adminRepository) 
        {
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
