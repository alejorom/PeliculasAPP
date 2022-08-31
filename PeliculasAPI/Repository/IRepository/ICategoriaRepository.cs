using PeliculasAPI.Models;

namespace PeliculasAPI.Repository.IRepository
{
    public interface ICategoriaRepository
    {
        ICollection<Categoria> GetCategorias();
        Categoria GetCategoria(int CategoriaId);
        bool ExisteCategoria(string nombre);
        bool ExisteCategoria(int id);
        bool CrearCategoria(Categoria categoria);
        bool ActualizarCategoria(Categoria categoria);
        bool BorrarCategoria(Categoria categoria);
        bool Guardar();

    }
}
