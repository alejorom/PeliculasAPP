using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Data;
using PeliculasAPI.Models;
using PeliculasAPI.Repository.IRepository;

namespace PeliculasAPI.Repository
{
    public class PeliculaRepository : IPeliculaRepository
    {
        private readonly ApplicationDbContext _bd;

        public PeliculaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        /// <summary>
        /// Actualizar pelicula
        /// </summary>
        /// <param name="pelicula"></param>
        /// <returns></returns>
        public bool ActualizarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Update(pelicula);
            return Guardar();
        }

        /// <summary>
        /// Remover una película
        /// </summary>
        /// <param name="pelicula"></param>
        /// <returns></returns>
        public bool BorrarPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Remove(pelicula);
            return Guardar();
        }

        /// <summary>
        /// Buscar pelicula por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public IEnumerable<Pelicula> BuscarPelicula(string nombre)
        {
            IQueryable<Pelicula> query = _bd.Pelicula;

            if (!string.IsNullOrEmpty(nombre))
            {
                query = query.Where(e => e.Nombre.Contains(nombre) || e.Descripcion.Contains(nombre));
            }

            return query.ToList();
        }

        /// <summary>
        /// Adicionar una nueva película
        /// </summary>
        /// <param name="pelicula"></param>
        /// <returns></returns>
        public bool CrearPelicula(Pelicula pelicula)
        {
            _bd.Pelicula.Add(pelicula);
            return Guardar();
        }

        /// <summary>
        /// Verificar si existe película por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        public bool ExistePelicula(string nombre)
        {
            bool valor = _bd.Pelicula.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        /// <summary>
        /// Verificar si existe película por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ExistePelicula(int id)
        {
            return _bd.Pelicula.Any(c => c.Id == id);
        }

        /// <summary>
        /// Obtener una pelica por id
        /// </summary>
        /// <param name="PeliculaId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Pelicula GetPelicula(int PeliculaId)
        {
            return _bd.Pelicula.FirstOrDefault(c => c.Id == PeliculaId);
        }

        /// <summary>
        /// Obtener todas las peliculas
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICollection<Pelicula> GetPeliculas()
        {
            return _bd.Pelicula.OrderBy(c => c.Nombre).ToList();
        }

        /// <summary>
        /// Obtener películas por categoria
        /// </summary>
        /// <param name="CategoriaId"></param>
        /// <returns></returns>
        public ICollection<Pelicula> GetPeliculasEnCategoria(int CategoriaId)
        {
            return _bd.Pelicula.Include(ca => ca.Categoria).Where(ca => ca.categoriaId == CategoriaId).ToList();
        }

        /// <summary>
        /// Almacenar los cambias de la pelíula
        /// </summary>
        /// <returns></returns>
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0;
        }
    }
}
