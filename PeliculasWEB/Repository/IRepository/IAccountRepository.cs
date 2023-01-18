using PeliculasWEB.Models;

namespace PeliculasWEB.Repository.IRepository
{
    public interface IAccountRepository : IRepository<UsuarioM>
    {
        Task<UsuarioM> LoginAsync(string url, UsuarioM itemCrear);
        Task<bool> RegisterAsync(string url, UsuarioM itemCrear);
    }
}
