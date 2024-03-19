using IVoice.Data;
using IVoice.Models;
using IVoice.Reopsitories;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NToastNotify;
using Stripe;
using static IVoice.Reopsitories.CartRepository;

namespace IVoice
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllersWithViews();
            builder.Services.AddMvc().AddNToastNotifyToastr(new ToastrOptions()
            {
                ProgressBar = true,
                PositionClass = ToastPositions.TopFullWidth,
                PreventDuplicates = true,
                CloseButton = true,
            });
            builder.Services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
   
            builder.Services
             .AddIdentity<ApplicationUser, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
             .AddEntityFrameworkStores<ApplicationDbContext>()
             .AddDefaultTokenProviders();
            builder.Services.AddControllersWithViews();
            builder.Services.AddTransient<IProduct, ProductRepo>();
            builder.Services.AddTransient<ICartRepository, CartRepository>();
            builder.Services.AddTransient<IUserOrderRepository, UserOrderRepository>();
            builder.Services.Configure<StripeSettings>(builder.Configuration.GetSection("StripeSettings"));
            builder.Configuration.GetValue<string>("StripeSettings:SecretKey");
            builder.Services.AddHostedService<OrderStatusUpdaterService>();
          

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
