using Demo.DAL.Authentication;
using Demo.PL.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager
            )
        {
            _userManager = userManager;
            _signInManager = signInManager;
            
        }

      
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel newUserVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser userModel = new ApplicationUser
                {
                    FirstName = newUserVM.FirstName,
                    LastName = newUserVM.LastName,
                    UserName = newUserVM.Email,
                    BirthDate = newUserVM.BirthDate,
                    Gender = newUserVM.Gender,
                    Address = newUserVM.Address,
                    PhoneNumber = newUserVM.PhoneNumber,
                    Email = newUserVM.Email
                };

                IdentityResult result = await _userManager.CreateAsync(userModel, newUserVM.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, "User");

                    await _signInManager.SignInAsync(userModel, false);

                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                }
            }

            return View(newUserVM);
            
        }
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel loginUser)
        {
            if (ModelState.IsValid)
            {
                //check db
                ApplicationUser userModel = await _userManager.FindByEmailAsync(loginUser.Email);
                if (userModel != null)
                {
                    bool found = await _userManager.CheckPasswordAsync(userModel, loginUser.Password);
                    if (found == true)
                    {
                        //cookie
                        await _signInManager.SignInAsync(userModel, loginUser.RememberMe);
                        return RedirectToAction("Index", "Department");
                    }
                    else
                    {
                        ModelState.AddModelError("", "User name or Password Wrong");
                    }
                }
            }
            return View(loginUser);
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
        [HttpGet]
        public IActionResult AddAdmin()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAdmin(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Mapping from VM to model
                ApplicationUser userModel = new ApplicationUser();
                userModel.FirstName = model.FirstName;
                userModel.LastName = model.LastName;
                userModel.UserName = model.Email;
                userModel.BirthDate = model.BirthDate;
                userModel.Gender = model.Gender;
                userModel.Address = model.Address;
                userModel.PhoneNumber = model.PhoneNumber;
                userModel.Role = "Admin";
                userModel.Email = model.Email;
                userModel.PasswordHash = model.Password;
                // save in db
                IdentityResult result = await _userManager.CreateAsync(userModel, model.Password!);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(userModel, "Admin");
                    // create cookies
                    await _signInManager.SignInAsync(userModel, false); 
                    return RedirectToAction("Index", "Department");
                }
                else
                {
                    foreach (var errorItem in result.Errors)
                    {
                        ModelState.AddModelError("Password", errorItem.Description);
                    }
                }
            }
            return View(model);
        }


    }
}
