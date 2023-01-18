using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;
using PeliculasWEB.Utilities;
using System.Diagnostics;
using System.Security.Claims;

namespace PeliculasWEB.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IAccountRepository _accountRepo;
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IPeliculaRepository _peliculaRepo;

        public HomeController(ILogger<HomeController> logger, IAccountRepository accountRepo, ICategoriaRepository categoriaRepo, IPeliculaRepository peliculaRepo)
        {
            _logger = logger;
            _accountRepo = accountRepo;
            _categoriaRepo = categoriaRepo;
            _peliculaRepo = peliculaRepo;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet]
        public IActionResult Login()
        {
            UsuarioM usuario = new();
            return View(usuario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(UsuarioM obj)
        {
            if (ModelState.IsValid)
            {
                UsuarioM objUser = await _accountRepo.LoginAsync(Constantes.RutaUsuariosApi + "Login", obj);
                if (objUser.Token == null)
                {
                    TempData["alert"] = "Usuario y/o contraseña son incorrectos.";
                    return View();
                }

                ClaimsIdentity identity = new(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(ClaimTypes.Name, objUser.Usuario));

                ClaimsPrincipal principal = new(identity);
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                HttpContext.Session.SetString("JWToken", objUser.Token);
                TempData["alert"] = "Bienvenido/a " + objUser.Usuario;
                return RedirectToAction("Index");
            }
            else
            {
                return View();
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            HttpContext.Session.SetString("JWToken", "");
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Registro(UsuarioM obj)
        {
            bool result = await _accountRepo.RegisterAsync(Constantes.RutaUsuariosApi + "Registro", obj);
            if (result == false)
            {
                return View();
            }

            TempData["alert"] = "Registro correcto";
            return RedirectToAction("Login");
        }
    }
}