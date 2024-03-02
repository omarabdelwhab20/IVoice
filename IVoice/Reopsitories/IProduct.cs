using IVoice.Models;

namespace IVoice.Reopsitories
{
    public interface IProduct
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "");

    }
}
