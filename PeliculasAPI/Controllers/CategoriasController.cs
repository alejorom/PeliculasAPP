using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PeliculasAPI.Models;
using PeliculasAPI.Models.DTOS;
using PeliculasAPI.Repository.IRepository;

namespace PeliculasAPI.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    [ApiController]
    [ApiExplorerSettings(GroupName = "PeliculasAPICategorias")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaRepository _categoriaRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="categoriaRepo"></param>
        /// <param name="mapper"></param>
        public CategoriasController(ICategoriaRepository categoriaRepo, IMapper mapper)
        {
            _categoriaRepo = categoriaRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las categorías de películas.
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet]
        [ProducesResponseType(200, Type = typeof(List<CategoriaDTO>))]
        [ProducesResponseType(400)]
        public IActionResult GetCategorias()
        {
            var listaCategorias = _categoriaRepo.GetCategorias();
            
            var listaCategoriasDTO = _mapper.Map<List<CategoriaDTO>>(listaCategorias);

            return Ok(listaCategoriasDTO);
        }

        /// <summary>
        /// Obtener una categoría de película.
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("{categoriaId:int}", Name = "GetCategoria")]
        [ProducesResponseType(200, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(404)]
        [ProducesDefaultResponseType]
        public IActionResult GetCategoria(int categoriaId)
        {
            var itemCategoria = _categoriaRepo.GetCategoria(categoriaId);

            if (itemCategoria == null)
            {
                return NotFound();
            }

            var itemCategoriaDto = _mapper.Map<CategoriaDTO>(itemCategoria);
            return Ok(itemCategoriaDto);
        }

        /// <summary>
        /// Crear una nueva categoría
        /// </summary>
        /// <param name="categoriaDTO"></param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(201, Type = typeof(CategoriaDTO))]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult CrearCategoria([FromBody] CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (_categoriaRepo.ExisteCategoria(categoriaDTO.Nombre))
            {
                ModelState.AddModelError("", "La categoría ya existe");
                return StatusCode(404, ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (!_categoriaRepo.CrearCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetCategoria", new { categoriaId = categoria.Id }, categoria);
        }

        /// <summary>
        /// Actualiza una categoría existente
        /// </summary>
        /// <param name="categoriaId"></param>
        /// <param name="categoriaDTO"></param>
        /// <returns></returns>
        [HttpPatch("{categoriaId:int}", Name = "ActualizarCategoria")]
        [ProducesResponseType(204)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult ActualizarCategoria(int categoriaId, [FromBody] CategoriaDTO categoriaDTO)
        {
            if (categoriaDTO == null || categoriaId != categoriaDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var categoria = _mapper.Map<Categoria>(categoriaDTO);

            if (!_categoriaRepo.ActualizarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Elimina una categoría existente
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns></returns>
        [HttpDelete("{categoriaId:int}", Name = "BorrarCategoria")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public IActionResult BorrarCategoria(int categoriaId)
        {
            if (!_categoriaRepo.ExisteCategoria(categoriaId))
            {
                return NotFound();
            }

            var categoria = _categoriaRepo.GetCategoria(categoriaId);

            if (!_categoriaRepo.BorrarCategoria(categoria))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {categoria.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }
}
