namespace IVoive_WEB.Services
{
    public interface IProductServices
    {
        IEnumerable<Product> GetAll();

        Product? getById(int id);
        public Task AddProduct(ProductViewModel vm);
        public Task<Product> UpdateProduct(UpdateProductViewModel updateProduct);

        public Task<Product> Details(int id);

        bool Delete(int id);


    }
}
