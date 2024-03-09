
namespace IVoice.Reopsitories
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "");
        IEnumerable<Product> GetAll();
        Product? getById(int id);
        public Task AddProduct(ProductViewModel vm);
        public Task<Product> UpdateProduct(UpdateProductViewModel updateProduct);
        bool Delete(int id);

    }
}
