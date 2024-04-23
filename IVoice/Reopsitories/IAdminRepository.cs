using IVoice.Models.DTO;

namespace IVoice.Reopsitories
{
    public interface IAdminRepository
    {
        public List<Product> GetProductsRunningOutOfStock();

        public List<UsersDisplayModel> UsersDisplay();

        public List<OrdersDisplayModel> OrdersDisplay();


    }
}
