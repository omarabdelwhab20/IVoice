using IVoice.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProduct product;

        public ProductController(IProduct product)
        {
            this.product = product;
        }
        public async Task<IActionResult> Index(string sTerm="")
        {
            IEnumerable<Product> products=await product.GetProducts(sTerm);
            ProductDisplayModel model = new()
            {
                products = products,
                STerm=sTerm
            };
            return View(model);
        }
    }
}
