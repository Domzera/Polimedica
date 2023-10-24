using Polimedica.Models;
using Microsoft.AspNetCore.Mvc;
using Polimedica.Data;
using Polimedica.ViewModel;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics.Eventing.Reader;

namespace Polimedica.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<Pessoa> _userManager;
        private readonly SignInManager<Pessoa> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly PolimedicaDbContetxt _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AccountController(
            UserManager<Pessoa> userManager,
            SignInManager<Pessoa> signInManager,
            RoleManager<IdentityRole> roleManager,
            PolimedicaDbContetxt context,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _context = context;
            _httpContextAccessor = httpContextAccessor;
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
                        var optionsCookie = new CookieOptions
                        {
                            Expires = DateTime.Now.AddMinutes(5),
                            HttpOnly = true,
                            Secure = true,
                        };
                        _httpContextAccessor.HttpContext.Response.Cookies.Append("Polime", user.Id, optionsCookie);
                        return RedirectToAction("Criar", "Roteiro");
                    }
                }
                TempData["Error"] = "Wrong Credentials. Please, try again";
                return View(loginVM);
            }
            TempData["Error"] = "Wrong Credentials. Please, try again";
            return View(loginVM);
        }

        public IActionResult Register()
        {
            var response = new RegisterVM();
            return View(response);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid)
            {
                return View(registerVM);
            }
            var user = await _userManager.FindByNameAsync(registerVM.Nome);
            if(user == null)
            {
                var newUser = new Pessoa()
                {
                    PrimeiroNome = registerVM.Nome,
                    UserName = "Luiz",
                    funcao = registerVM.Funcao
                };
                var newUserResponse = await _userManager.CreateAsync(newUser, registerVM.Senha);
                if(newUserResponse.Succeeded)
                {
                    var role = await _roleManager.RoleExistsAsync(registerVM.Funcao);
                    if (!role) await _roleManager.CreateAsync(new IdentityRole(newUser.funcao));//Aqui eu poderia usar um data e deixar as funções previamente definidas(new IdentityRole(UserRolers.Gerente)) por exemplo
                    await _userManager.AddToRoleAsync(newUser, newUser.funcao);//Aqui tinha que acompanhar a linha de cima
                }
                return RedirectToAction("Index", "Roteiro");//Alterar aqui para enviar para pagina de detalhes pessoal
            }
            return RedirectToAction("index", "home");
        }
    }
}
