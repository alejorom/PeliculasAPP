using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;

namespace PeliculasWEB.Repository
{
    public class UsuarioRepository : Repository<UsuarioU>, IUsuarioRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public UsuarioRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
