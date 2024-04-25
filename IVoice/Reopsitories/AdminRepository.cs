using IVoice.Models.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace IVoice.Reopsitories
{
    public class AdminRepository : IAdminRepository
    {
        ApplicationDbContext dbContext { get; set; }
        UserManager<ApplicationUser> userManager { get; set; }
        public AdminRepository(ApplicationDbContext dbContext, UserManager<ApplicationUser> userManager)
        {
            this.dbContext = dbContext;
            this.userManager = userManager;
        }


        public List<Product> GetProductsRunningOutOfStock()
        {
            return dbContext.products.Where(p => p.Quantity < 10).ToList();
        }

        public List<UsersDisplayModel> UsersDisplay()
        {
            var usersWithOrderCount = from user in dbContext.Users
                                      join order in dbContext.Orders on user.Id equals order.UserId into userOrders
                                       // Exclude users with the "Admin" role
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
