using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PeliculasWEB.Models;
using PeliculasWEB.Models.ViewModels;
using PeliculasWEB.Repository.IRepository;
using PeliculasWEB.Utilities;

namespace PeliculasWEB.Controllers
{
    public class PeliculasController : Controller
    {
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IPeliculaRepository _peliculaRepo;

        public PeliculasController(ICategoriaRepository categoriaRepo, IPeliculaRepository peliculaRepo)
        {
            _categoriaRepo = categoriaRepo;
            _peliculaRepo = peliculaRepo;  
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View(new Pelicula() { });
        }

        [HttpGet]
        public async Task<IActionResult> GetTodasPeliculas()
        {
            return Json(new { data = await _peliculaRepo.GetTodoAsync(Constantes.RutaPeliculasApi) });
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            IEnumerable<Categoria>? npList = await _categoriaRepo.GetTodoAsync(Constantes.RutaCategoriasApi) as IEnumerable<Categoria>;

            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            return View(objVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Pelicula pelicula)
        {

            IEnumerable<Categoria>? npList = await _categoriaRepo.GetTodoAsync(Constantes.RutaCategoriasApi) as IEnumerable<Categoria>;

            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[]? p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    pelicula.RutaImagen = p1;
                }
                else
                {
                    return View(objVM);
                }

                await _peliculaRepo.CrearAsync(Constantes.RutaPeliculasApi, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View(objVM);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            IEnumerable<Categoria>? npList = await _categoriaRepo.GetTodoAsync(Constantes.RutaCategoriasApi) as IEnumerable<Categoria>;

            PeliculasVM objVM = new()
            {
                ListaCategorias = npList.Select(i => new SelectListItem
                {
                    Text = i.Nombre,
                    Value = i.Id.ToString()
                }),

                Pelicula = new Pelicula()
            };

            if (id == null)
            {
                return NotFound();
            }

            //Para mostrar los datos en el formulario Edit
            objVM.Pelicula = await _peliculaRepo.GetAsync(Constantes.RutaPeliculasApi, id.GetValueOrDefault());
            if (objVM.Pelicula == null)
            {
                return NotFound();
            }

            return View(objVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Update(Pelicula pelicula)
        {

            if (ModelState.IsValid)
            {
                var files = HttpContext.Request.Form.Files;
                if (files.Count > 0)
                {
                    byte[]? p1 = null;
                    using (var fs1 = files[0].OpenReadStream())
                    {
                        using var ms1 = new MemoryStream();
                        fs1.CopyTo(ms1);
                        p1 = ms1.ToArray();
                    }
                    pelicula.RutaImagen = p1;
                }
                else
                {
                    var peliculaFromDb = await _peliculaRepo.GetAsync(Constantes.RutaPeliculasApi, pelicula.Id);
                    pelicula.RutaImagen = peliculaFromDb.RutaImagen;
                }

                await _peliculaRepo.ActualizarAsync(Constantes.RutaPeliculasApi + pelicula.Id, pelicula, HttpContext.Session.GetString("JWToken"));
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool status = await _peliculaRepo.BorrarAsync(Constantes.RutaPeliculasApi, id, HttpContext.Session.GetString("JWToken"));

            if (status)
            {
                return Json(new { success = true, message = "Borrado correctamente" });
            }

            return Json(new { success = false, message = "No se pudo borrar" });
        }

        [HttpGet]
        public async Task<IActionResult> GetPeliculasEnCategoria(int id)
        {
            return Json(new { data = await _peliculaRepo.GetPeliculasEnCategoriaAsync(Constantes.RutaPeliculasEnCategoriaApi, id) });
        }
    }
}
