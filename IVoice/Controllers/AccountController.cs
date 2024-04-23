using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;


namespace IVoice.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly ApplicationDbContext context;
        private readonly ILogger<AccountController> logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,ApplicationDbContext context,
            ILogger<AccountController> logger)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.context = context;
            this.logger = logger;
        }
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(Registerviwemodel newuservm)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser();
                userModel.UserName = newuservm.Username;
                userModel.Email = newuservm.Email;
                userModel.PasswordHash = newuservm.Password;
                userModel.Address = newuservm.Address;

                IdentityResult result = await userManager.CreateAsync(userModel, newuservm.Password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(userModel, "User");
                    await signInManager.SignInAsync(userModel, false);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        // Add errors to the correct property in the ModelState
                        if (erroritem.Code == "DuplicateUserName")
                        {
                            ModelState.AddModelError("Username", erroritem.Description);
                        }
                        else
                        {
                            ModelState.AddModelError(string.Empty, erroritem.Description);
                        }
                    }
                }
            }
            return View(newuservm);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViweModel loginViweModel)
        {
            if (ModelState.IsValid)
            {
                //save db
                ApplicationUser userModel = await userManager.FindByNameAsync(loginViweModel.UserName);
                if (userModel != null)
                {
                    bool found = await userManager.CheckPasswordAsync(userModel, loginViweModel.Password);
                    if (found == true)
                    {
                        //creat cookie
                        await signInManager.SignInAsync(userModel, false);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ModelState.AddModelError("", "UserName Or Password is Wrong");
            }
            return View(loginViweModel);
        }
        public async Task<IActionResult> Logout()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }

        [HttpGet]
        public async Task<IActionResult> ForgotPassword()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {


            return View(model);
        }

    }
}
