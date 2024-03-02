
namespace IVoive_WEB.Services
{
    public class ProductServices : IProductServices
    {
        public ApplicationDbcontext _context { get; }
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly string _imagesPath;

        public ProductServices(ApplicationDbcontext context , IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _imagesPath = $"{_webHostEnvironment.WebRootPath}{FileSettings.ImagePath}";
        }

        public IEnumerable<Product> GetAll()
        {
            return _context.Products
               .AsNoTracking()
               .ToList();
        }

        public Product getById(int id)
        {
            var product = _context.Products.SingleOrDefault(p => p.Id == id);
            return product;
        }

        public async Task AddProduct (ProductViewModel vm)
        {
            var coverName = await saveCover(vm.ImageFile);

            Product product = new()
            {
                Name = vm.Name,
                Description = vm.Description,
                Price = vm.Price,
                Cover = coverName,
            };

             _context.Add(product);
             _context.SaveChanges();
        }

        public async Task<Product> UpdateProduct(UpdateProductViewModel updateProduct)
        {
            var product = _context.Products
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

            var effectedRows = _context.SaveChanges();

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

            var product = _context.Products.Find(id);
            if (product is null)
                return isDeleted;

            _context.Products.Remove(product);

            var effectedRows = _context.SaveChanges();

            if (effectedRows > 0)
            {
                isDeleted = true;
                var cover = Path.Combine(_imagesPath, product.Cover);
                File.Delete(cover);
            }

            return isDeleted;
        }

        public async Task<Product> Details (int id)
        {
            if (id == null)
                return null;

            var product = await  _context.Products.SingleOrDefaultAsync(i => i.Id == id);
            if (product == null)
                return null;

            return product;
        }

        private async Task<string> saveCover(IFormFile cover)
        {
            var coverName = cover?.FileName != null
            ? $"{Guid.NewGuid()}{Path.GetExtension(cover.FileName)}"
            :"defaultFileName";

            var path = Path.Combine(_imagesPath, coverName);

            using var stream = File.Create(path);

            await cover.CopyToAsync(stream);

            
            return coverName;
        }
    }
    
}
