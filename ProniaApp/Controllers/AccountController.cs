using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProniaApp.Models;
using ProniaApp.ViewModels;

namespace ProniaApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager; 

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager)
        {
            _userManager=userManager;
            _signInManager=signInManager;
        }
        public ActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
         public async Task<ActionResult> Register(RegisterVM registerVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            
            AppUser user= new AppUser()
            {
                Name=registerVM.Name,
                Email=registerVM.Email,
                UserName=registerVM.UserName,
                Surname=registerVM.Surname,
            };
             


            IdentityResult result=await _userManager.CreateAsync(user,registerVM.Password);

        if (!result.Succeeded)
            {
                foreach (IdentityError error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty,error.Description);
                    return View();
                }
            }

            await _signInManager.SignInAsync(user,false);

            return RedirectToAction(nameof(HomeController.Index),"Home");
        }


        public async Task<ActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(HomeController.Index),"Home");
        }

        public async Task<ActionResult> Login(LoginVM loginVM)
        {

            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser user= await _userManager.Users.FirstOrDefaultAsync(u=>u.UserName == loginVM.UserNameOrEmail || u.Email==loginVM.UserNameOrEmail);

            if (user == null)
            {
                ModelState.AddModelError("","usernae or mail invalid");
                return View();
            }
            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, loginVM.IsPersisted, true);

            if (result.Succeeded)
            {
                ModelState.AddModelError("","password invalid");
                return View();
            }


        return View();
        }
    }
}