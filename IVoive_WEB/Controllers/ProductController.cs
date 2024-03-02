using IVoive_WEB.Data;
using IVoive_WEB.Models;
using IVoive_WEB.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace IVoive_WEB.Controllers
{
    public class ProductController : Controller
    {

        public readonly IProductServices productServices;
        private readonly ApplicationDbcontext _context;

        public ProductController(IProductServices ProductServices,ApplicationDbcontext context)
        {
            productServices = ProductServices;
            _context = context;
        }

        [HttpGet]
        public IActionResult Index()
        {

            return View(productServices.GetAll());
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {

            var productFromDb = await productServices.Details(id);
            var cart = new Cart()
            {
                Product = productFromDb,
                ProductId=productFromDb.Id,
                Count=1    
            };
             return View(cart);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public async Task<IActionResult> Details(Cart cart)
        {
            if (ModelState.IsValid)
            {
                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;
                cart.ApplicationUserId = userId;
                var cartFromDb = await _context.Carts.Where(x => x.ApplicationUserId == cart.ApplicationUserId
                && x.ProductId==cart.ProductId).FirstOrDefaultAsync();
                if (cartFromDb == null)
                {
                   _context.Carts.Add(cart);
                }
                else
                {
                   cartFromDb.Count=cartFromDb.Count + cart.Count;
                }
                _context.SaveChanges();
                return RedirectToAction("Index");


            }
            return RedirectToAction("Index");
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
            await productServices.AddProduct(productViewModel);
            return RedirectToAction(nameof(Index));
        }


        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Update(int id)
        {
            var product = productServices.getById(id);

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

            var product = await productServices.UpdateProduct(updateProductViewModel);

            if (product is null)
                return BadRequest();

            return RedirectToAction(nameof(Index));
        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            var isDeleted = productServices.Delete(id);



            return isDeleted ? Ok() : BadRequest();
        }       

        /*private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }*/

    }
}
