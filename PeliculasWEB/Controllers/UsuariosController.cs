using Microsoft.AspNetCore.Mvc;
using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;
using PeliculasWEB.Utilities;

namespace PeliculasWEB.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioRepository _usuarioRepo;

        public UsuariosController(IUsuarioRepository usuarioRepo)
        {
            _usuarioRepo = usuarioRepo;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new UsuarioU() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodosUsuarios()
        {
            return Json(new { data = await _usuarioRepo.GetTodoAsync(Constantes.RutaUsuariosApi) });
        }
    }
}
