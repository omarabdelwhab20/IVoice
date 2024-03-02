using IVoice.Data;
using IVoice.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace IVoice.Reopsitories
{
    public class ProductRepo : IProduct
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment webHostEnvironment;

        public ProductRepo(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            this.webHostEnvironment = webHostEnvironment;
        }

        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "")
        {
            sTerm = sTerm.ToLower();
            var products= await (from product in dbContext.products
                                 where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.Name.ToLower().StartsWith(sTerm))
                                 select new Product
                                 {
                                     Id = product.Id,
                                     Name = product.Name,
                                     Price = product.Price,
                                     Description = product.Description,
                                     Cover= product.Cover,
                                 }).ToListAsync();

            return products;

        }
    }
}
