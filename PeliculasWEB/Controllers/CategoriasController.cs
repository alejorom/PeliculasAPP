using Microsoft.AspNetCore.Mvc;
using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;
using PeliculasWEB.Utilities;

namespace PeliculasWEB.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepo;

        public CategoriasController(ICategoriaRepository categoriaRepo)
        {
            _categoriaRepo = categoriaRepo; 
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Categoria() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasCategorias()
        {
            return Json(new { data = await _categoriaRepo.GetTodoAsync(Constantes.RutaCategoriasApi) });
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaRepo.CrearAsync(Constantes.RutaCategoriasApi, categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            Categoria itemCategoria = new Categoria();

            if (id == null)
            {
                return NotFound();
            }

            itemCategoria = await _categoriaRepo.GetAsync(Constantes.RutaCategoriasApi, id.GetValueOrDefault());

            if (itemCategoria == null)
            {
                return NotFound();
            }

            return View(itemCategoria);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Categoria categoria)
        {
            if (ModelState.IsValid)
            {
                await _categoriaRepo.ActualizarAsync(Constantes.RutaCategoriasApi + categoria.Id, categoria, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var status = await _categoriaRepo.BorrarAsync(Constantes.RutaCategoriasApi, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }
    }
}
