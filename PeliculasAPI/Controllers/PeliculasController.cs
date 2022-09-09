using AutoMapper;
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
    [Route("api/[controller]")]
    [ApiController]
    public class PeliculasController : ControllerBase
    {
        private readonly IPeliculaRepository _peliculaRepo;
        private readonly IMapper _mapper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="peliculaRepo"></param>
        /// <param name="mapper"></param>
        public PeliculasController(IPeliculaRepository peliculaRepo, IMapper mapper)
        {
            _peliculaRepo = peliculaRepo;
            _mapper = mapper;
        }

        /// <summary>
        /// Obtener todas las películas
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult GetPeliculas()
        {
            var listaPeliculas = _peliculaRepo.GetPeliculas();

            var listaPeliculasDTO = _mapper.Map<List<PeliculaDTO>>(listaPeliculas);

            return Ok(listaPeliculasDTO);
        }

        /// <summary>
        /// Obtener una película individual
        /// </summary>
        /// <param name="peliculaId">ID de la película</param>
        /// <returns></returns>
        [HttpGet("{peliculaId:int}", Name = "GetPelicula")]
        public IActionResult GetPelicula(int peliculaId)
        {
            var itemPelicula = _peliculaRepo.GetPelicula(peliculaId);

            if (itemPelicula == null)
            {
                return NotFound();
            }

            var itemPeliculaDto = _mapper.Map<PeliculaDTO>(itemPelicula);
            return Ok(itemPeliculaDto);
        }

        /// <summary>
        /// Obtener las películas de una categoría
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns></returns>
        [HttpGet("GetPeliculasEnCategoria/{categoriaId:int}")]
        public IActionResult GetPeliculasEnCategoria(int categoriaId)
        {
            var listaPeliculas = _peliculaRepo.GetPeliculasEnCategoria(categoriaId);

            var listaPeliculasDTO = _mapper.Map<List<PeliculaDTO>>(listaPeliculas);

            return Ok(listaPeliculasDTO);
        }

        /// <summary>
        /// Buscar películas por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        [HttpGet("Buscar")]
        public IActionResult Buscar(string nombre)
        {
            try
            {
                var resultado = _peliculaRepo.BuscarPelicula(nombre);
                if (resultado.Any())
                {
                    return Ok(resultado);
                }

                return NotFound();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error recuperando datos de la aplicación");
            }
        }

        /// <summary>
        /// Crear una nueva película
        /// </summary>
        /// <param name="peliculaDTO"></param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult CrearPelicula([FromBody] PeliculaCreateDTO peliculaDTO)
        {
            if (peliculaDTO == null)
            {
                return BadRequest(ModelState);
            }

            if (_peliculaRepo.ExistePelicula(peliculaDTO.Nombre))
            {
                ModelState.AddModelError("", "La película ya existe");
                return StatusCode(404, ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDTO);

            if (!_peliculaRepo.CrearPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal guardando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return CreatedAtRoute("GetPelicula", new { peliculaId = pelicula.Id }, pelicula);
        }

        /// <summary>
        /// Actualizar una película existente
        /// </summary>
        /// <param name="peliculaId">ID de la película</param>
        /// <param name="peliculaDTO"></param>
        /// <returns></returns>
        [HttpPatch("{peliculaId:int}", Name = "ActualizarPelicula")]
        public IActionResult ActualizarPelicula(int peliculaId, [FromBody] PeliculaUpdateDTO peliculaDTO)
        {
            if (peliculaDTO == null || peliculaId != peliculaDTO.Id)
            {
                return BadRequest(ModelState);
            }

            var pelicula = _mapper.Map<Pelicula>(peliculaDTO);

            if (!_peliculaRepo.ActualizarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal actualizando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        /// <summary>
        /// Eliminar una película existente
        /// </summary>
        /// <param name="peliculaId">ID de la película</param>
        /// <returns></returns>
        [HttpDelete("{peliculaId:int}", Name = "BorrarPelicula")]
        public IActionResult BorrarPelicula(int peliculaId)
        {
            if (!_peliculaRepo.ExistePelicula(peliculaId))
            {
                return NotFound();
            }

            var pelicula = _peliculaRepo.GetPelicula(peliculaId);

            if (!_peliculaRepo.BorrarPelicula(pelicula))
            {
                ModelState.AddModelError("", $"Algo salio mal borrando el registro {pelicula.Nombre}");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }
    }

}
