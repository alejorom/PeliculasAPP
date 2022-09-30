using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;

namespace PeliculasWEB.Repository
{
    public class CategoriaRepository : Repository<Categoria>, ICategoriaRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public CategoriaRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
