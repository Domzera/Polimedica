using Polimedica.Models;
using Microsoft.AspNetCore.Mvc;
using Polimedica.Data;
using Polimedica.ViewModel;
using Microsoft.AspNetCore.Identity;

namespace Polimedica.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Pessoa> _userManager;
        private readonly SignInManager<Pessoa> _signInManager;
        private readonly PolimedicaDbContetxt _context;

        public AccountController(UserManager<Pessoa> userManager, SignInManager<Pessoa> signInManager, PolimedicaDbContetxt context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        public IActionResult Login()
        {
            var response = new LoginVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid)
            {
                return View(loginVM);
            }
            var user = await _userManager.FindByNameAsync(loginVM.Nome);

            if (user != null)
            {
                var passWordChack = await _userManager.CheckPasswordAsync(user, loginVM.Password);
                if (passWordChack)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Criar", "Roteiro");
                    }
                }
                TempData["Error"] = "Wrong Credentials. Please, try again";
                return View(loginVM);
            }
            TempData["Error"] = "Wrong Credentials. Please, try again";
            return View(loginVM);
        }
    }
}
