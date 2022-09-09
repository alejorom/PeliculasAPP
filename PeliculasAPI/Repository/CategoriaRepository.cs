using PeliculasAPI.Data;
using PeliculasAPI.Models;
using PeliculasAPI.Repository.IRepository;

namespace PeliculasAPI.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly ApplicationDbContext _bd;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bd"></param>
        public CategoriaRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        /// <summary>
        /// Actualizar categoria de películas
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ActualizarCategoria(Categoria categoria)
        {
            _bd.Categoria.Update(categoria);
            return Guardar();
        }

        /// <summary>
        /// Remover una categoría de películas
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool BorrarCategoria(Categoria categoria)
        {
            _bd.Categoria.Remove(categoria);
            return Guardar();
        }

        /// <summary>
        /// Adicionar una nueva categoría de pelícuas
        /// </summary>
        /// <param name="categoria"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool CrearCategoria(Categoria categoria)
        {
            _bd.Categoria.Add(categoria);
            return Guardar();
        }

        /// <summary>
        /// Verificar si existe categoria por nombre
        /// </summary>
        /// <param name="nombre"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ExisteCategoria(string nombre)
        {
            bool valor = _bd.Categoria.Any(c => c.Nombre.ToLower().Trim() == nombre.ToLower().Trim());
            return valor;
        }

        /// <summary>
        /// Verificar si existe categoria por id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ExisteCategoria(int id)
        {
            return _bd.Categoria.Any(c => c.Id == id);
        }

        /// <summary>
        /// Obtener una categoría
        /// </summary>
        /// <param name="CategoriaId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Categoria GetCategoria(int CategoriaId)
        {
            return _bd.Categoria.FirstOrDefault(c => c.Id == CategoriaId);
        }

        /// <summary>
        /// Obtener todas las categorías 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public ICollection<Categoria> GetCategorias()
        {
            return _bd.Categoria.OrderBy(c => c.Nombre).ToList();
        }

        /// <summary>
        /// Almacenar los cambios de la categoría
        /// </summary>
        /// <returns></returns>
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0;
        }
    }
}
