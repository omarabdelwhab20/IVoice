using IVoice.Models.DTO;
using Microsoft.AspNetCore.Mvc;

namespace IVoice.Controllers
{
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
        public IActionResult Products()
        {

            return View(_product.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            return View( _product.getById(id));
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public IActionResult Delete(int id)
        {
            var isDeleted = _product.Delete(id);
            return isDeleted ? Ok() : BadRequest();
        }


    }
}
