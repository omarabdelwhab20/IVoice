using IVoice.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IVoice.Controllers
{
    [Authorize]
    public class ProductController : Controller
    {
        private readonly IProduct _product;

        public ProductController(IProduct product)
        {
            _product = product;
        }
        [HttpGet]
        
        public async Task<IActionResult> Index(string sTerm="")
        {
            IEnumerable<Product> products=await _product.GetProducts(sTerm);
            ProductDisplayModel model = new()
            {
                products = products,
                STerm=sTerm
            };
            return View(model);
        }
        [HttpGet]
        [Authorize(Roles ="Admin")]
        public IActionResult Products()
        {

            return View(_product.GetAll());
        }

        [HttpGet]
        public IActionResult Details(int? id)
        {

            if (id == null)
                return BadRequest();
            var Pproduct = _product.getById(id);
            if (Pproduct == null)
                return NotFound();
            return  View( _product.getById(id));
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(ProductViewModel productViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(productViewModel);
            }
            await _product.AddProduct(productViewModel);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var product = _product.getById(id);

            if (product is null)
                return NotFound();

            UpdateProductViewModel updateProduct = new()
            {
                Id = id,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                CurrentCover = product.Cover
            };

            return View(updateProduct);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(UpdateProductViewModel updateProductViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(updateProductViewModel);
            }

            var product = await _product.UpdateProduct(updateProductViewModel);

            if (product is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var isDeleted = _product.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }


    }
}
