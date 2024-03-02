using IVoive_WEB.Data;
using IVoive_WEB.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace IVoive_WEB.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly ApplicationDbcontext applicationDbcontext;

        public RoleController(RoleManager<IdentityRole> roleManager, ApplicationDbcontext applicationDbcontext)
        {
            this.roleManager = roleManager;
            this.applicationDbcontext = applicationDbcontext;
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Index()
        {
            ViewBag.Roles = applicationDbcontext.Roles.ToList();
            return View();
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]

        public IActionResult NewRole()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> NewRole(RoleViewModel roleViewModel)
        {
            if (ModelState.IsValid)
            {
                IdentityRole roleModel = new IdentityRole();
                roleModel.Name = roleViewModel.RoleName;
                //sv db
                IdentityResult result = await roleManager.CreateAsync(roleModel);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item.Description);
                    }
                }
            }
            return View(roleViewModel);
        }

    }
}
