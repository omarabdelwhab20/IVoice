using IVoice.Models.DTO;
using Microsoft.EntityFrameworkCore;

namespace IVoice.Reopsitories
{
    public class AdminRepository : IAdminRepository
    {
        ApplicationDbContext dbContext { get; set; }
        public AdminRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }


        public List<Product> GetProductsRunningOutOfStock()
        {
            return dbContext.products.Where(p => p.Quantity < 10).ToList();
        }

        public List<UsersDisplayModel> UsersDisplay()
        {
            var usersWithOrderCount = from user in dbContext.Users
                                      join order in dbContext.Orders on user.Id equals order.UserId into userOrders
                                      select new UsersDisplayModel
                                      {
                                          Name = user.UserName,
                                          Email = user.Email,
                                          Address = user.Address,
                                          OrdersCount = userOrders.Count() // Count the orders for each user
                                      };

            return usersWithOrderCount.ToList();


        }

        public List<OrdersDisplayModel> OrdersDisplay()
        {
            var ordersWithDetails = dbContext.Orders
                .Select(order => new OrdersDisplayModel
                {
                    OrderId = order.Id,
                    UserName = dbContext.Users.FirstOrDefault(u => u.Id == order.UserId).UserName,
                    CreatedAt = order.CreateDate,
                    //Status = order.OrderStatus.Name // Assuming you have an OrderStatus entity associated with each order
                    // You can add more properties as needed
                }).ToList();

            return ordersWithDetails;
        }
    }
}
