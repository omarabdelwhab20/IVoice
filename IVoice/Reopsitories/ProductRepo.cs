using IVoice.Data;
using IVoice.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.Intrinsics.Arm;

namespace IVoice.Reopsitories
{
    public class ProductRepo : IProduct
    {
        private readonly ApplicationDbContext dbContext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public ProductRepo(ApplicationDbContext dbContext, IWebHostEnvironment webHostEnvironment)
        {
            this.dbContext = dbContext;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
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

        public IEnumerable<Product> GetAll()
        {
            return dbContext.products
                 .AsNoTracking()
                 .ToList();
        }
        public Product getById(int? id)
        {
            var product = dbContext.products.SingleOrDefault(p => p.Id == id);
            return product;
        }
        public async Task AddProduct(ProductViewModel vm)
        {
            var coverName = await saveCover(vm.ImageFile);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Cover = coverName,
            };

            dbContext.Add(product);
            dbContext.SaveChanges();
        }
        public async Task<Product> UpdateProduct(UpdateProductViewModel updateProduct)
        {
            var product = dbContext.products
                .SingleOrDefault(g => g.Id == updateProduct.Id);

            if (product == null)
                return null;

            var hasNewCover = updateProduct.Cover is not null;
            var oldCover = product.Cover;

            product.Name = updateProduct.Name;
            product.Description = updateProduct.Description;
            product.Price = updateProduct.Price;

            if (hasNewCover)
            {
                product.Cover = await saveCover(updateProduct.Cover!);
            }

            var effectedRows = dbContext.SaveChanges();

            if (effectedRows > 0)
            {
                if (hasNewCover)
                {
                    var cover = Path.Combine(_imagesPath, oldCover);
                    File.Delete(cover);
                }
                return product;
            }
            else
            {
                var cover = Path.Combine(_imagesPath, product.Cover);
                File.Delete(cover);
                return null;
            }
        }

        public bool Delete(int id)
        {
            var isDeleted = false;

            var product = dbContext.products.Find(id);
            if (product is null)
                return isDeleted;

            dbContext.products.Remove(product);

            var effectedRows = dbContext.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_imagesPath, product.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }
        private async Task<string> saveCover(IFormFile cover)
        {
            var coverName = cover?.FileName != null
            ? $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}"
            : "defaultFileName";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);

            await cover.CopyToAsync(stream);

            return coverName;
        }
    }
}
