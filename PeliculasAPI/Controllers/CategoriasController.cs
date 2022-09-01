using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Models.DTOS;
using PeliculasAPI.Repository.IRepository;

namespace PeliculasAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IMapper _mapper;

        public CategoriasController(ICategoriaRepository categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las categorías de películas.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepo.GetCategorias();
            
            var listaCategoriasDTO = _mapper.Map<List<CategoriaDTO>>(listaCategorias);

            return Ok(listaCategoriasDTO);
        }
    }
}
