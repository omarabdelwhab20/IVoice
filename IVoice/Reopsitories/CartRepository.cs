
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Net;

 

namespace IVoice.Reopsitories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _db;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartRepository(ApplicationDbContext dbContext,IHttpContextAccessor httpContextAccessor, UserManager<ApplicationUser> userManager)
        {
            _db = dbContext;
            _httpContextAccessor = httpContextAccessor;
            _userManager = userManager;
        }

        public async Task<ShoppingCart> GetCart(string userId)
        {    
            var cart =  await _db.ShoppingCarts.FirstOrDefaultAsync(x => x.UserId == userId);
            return cart;
        }


        public async Task<int> AddItem(int productId, int qty)
        {
            string userId = GetUserId();
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                {
                    cart = new ShoppingCart
                    {
                        UserId = userId
                    };
                    _db.ShoppingCarts.Add(cart);
                }
                _db.SaveChanges();
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is not null)
                {
                    cartItem.Quantity += qty;
                }
                else
                {
                    var product = _db.products.Find(productId);
                    cartItem = new CartDetail
                    {
                        ProductId = productId,
                        ShoppingCartId = cart.Id,
                        Quantity = qty,
                        UnitPrice=product.Price
                      
                        
                    };
                    _db.CartDetails.Add(cartItem);
                }
                _db.SaveChanges();
                transaction.Commit();
            }
            catch (Exception ex)
            {
            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }

        public async Task<int> RemoveItem(int productId)
        {
            //using var transaction = _db.Database.BeginTransaction();
            string userId = GetUserId();
            try
            {
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("user is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                // cart detail section
                var cartItem = _db.CartDetails
                                  .FirstOrDefault(a => a.ShoppingCartId == cart.Id && a.ProductId == productId);
                if (cartItem is null)
                    throw new Exception("Not items in cart");
                else if (cartItem.Quantity == 1)
                    _db.CartDetails.Remove(cartItem);
                else
                    cartItem.Quantity = cartItem.Quantity - 1;
                _db.SaveChanges();
            }
            catch (Exception ex)
            {

            }
            var cartItemCount = await GetCartItemCount(userId);
            return cartItemCount;
        }
        public async Task<ShoppingCart> GetUserCart()
        {
            var userId = GetUserId();
            if (userId == null)
                throw new Exception("Invalid userid");
            var shoppingCart= await _db.ShoppingCarts
                                     .Include(a => a.CartDetails)
                                     .ThenInclude(a => a.product)
                                     .Where(a => a.UserId == userId).FirstOrDefaultAsync();
            return shoppingCart;
        }

        public async Task<int> GetCartItemCount(string userId = "")
        {
            if (string.IsNullOrEmpty(userId)) // updated line
            {
                userId = GetUserId();
            }
            var data =  await (from cart in _db.ShoppingCarts
                             join cartDetail in _db.CartDetails
                             on cart.Id equals cartDetail.ShoppingCartId
                             where cart.UserId == userId // updated line
                             select new { cartDetail.Id }
                        ).ToListAsync();
            return data.Count;
        }


        public async Task<bool> DoCheckout()
        {
            using var transaction = _db.Database.BeginTransaction();
            try
            {
                // logic
                // move data from cartDetail to order and order detail then we will remove cart detail
                var userId = GetUserId();
                if (string.IsNullOrEmpty(userId))
                    throw new Exception("User is not logged-in");
                var cart = await GetCart(userId);
                if (cart is null)
                    throw new Exception("Invalid cart");
                var cartDetail = _db.CartDetails
                                    .Where(a => a.ShoppingCartId == cart.Id).ToList();
                if (cartDetail.Count == 0)
                    throw new Exception("Cart is empty");
                var order = new Order
                {
                    UserId = userId,
                    CreateDate = DateTime.UtcNow,
                    OrderStatusId = 1//pending
                };
                _db.Orders.Add(order);
                _db.SaveChanges();
                foreach (var item in cartDetail)
                {
                    var orderDetail = new OrderDetail
                    {
                        ProductId= item.ProductId,
                        OrderId = order.Id,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    _db.OrderDetails.Add(orderDetail);
                    var product = await _db.products.FindAsync(item.ProductId);
                    if (product is null)
                        throw new Exception("Product not found");
                    if (product.Quantity < item.Quantity)
                        throw new Exception("Insufficient quantity available");
                    product.Quantity -= item.Quantity;
                }
                _db.SaveChanges();

                // removing the cartdetails
                _db.CartDetails.RemoveRange(cartDetail);
                _db.SaveChanges();
                transaction.Commit();
                return true;
            }
            catch (Exception)
            {

                return false;
            }
        }
        //public class OrderStatusUpdaterService : IHostedService, IDisposable
        //{
        //    private readonly IServiceProvider _services;
        //    private Timer _timer;

        //    public OrderStatusUpdaterService(IServiceProvider services)
        //    {
        //        _services = services;
        //    }

        //    public Task StartAsync(CancellationToken cancellationToken)
        //    {
        //        _timer = new Timer(DoWork, null, TimeSpan.Zero, TimeSpan.FromMinutes(1)); // Run every Minute
        //        return Task.CompletedTask;
        //    }

        //    private void DoWork(object state)
        //    {
        //        using (var scope = _services.CreateScope())
        //        {
        //            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        //            var ordersPendingForMoreThanAnHour = dbContext.Orders
        //                .Where(o => o.OrderStatusId == 1 && DateTime.UtcNow > o.CreateDate.AddMinutes(1))
        //                .ToList();

        //            foreach (var order in ordersPendingForMoreThanAnHour)
        //            {
        //                order.OrderStatusId = 3; // Update order status to "processing" or whatever suits your business logic
        //            }

        //            dbContext.SaveChanges();
        //        }
        //    }

        //    public Task StopAsync(CancellationToken cancellationToken)
        //    {
        //        _timer?.Change(Timeout.Infinite, 0);
        //        return Task.CompletedTask;
        //    }

        //    public void Dispose()
        //    {
        //        _timer?.Dispose();
        //    }
        //}



        private string GetUserId()
        {
            var principal = _httpContextAccessor.HttpContext.User;
            string userId = _userManager.GetUserId(principal);
            return userId;
        }
    }
}
