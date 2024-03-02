using IVoive_WEB.Models;
using IVoive_WEB.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IVoive_WEB.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
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
                //save db
                IdentityResult result = await userManager.CreateAsync(userModel, newuservm.Password);
                if (result.Succeeded)
                {
                    //register form add as a user
                    await userManager.AddToRoleAsync(userModel, "User");
                    //creat cookie
                    //await signInManager.SignInAsync(userModel, false);

                    return RedirectToAction("Login", "Account");
                }
                else
                {
                    foreach (var erroritem in result.Errors)
                    {
                        ModelState.AddModelError("Password", erroritem.Description);
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
                        await signInManager.SignInAsync(userModel,false);
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

    


    }
}
