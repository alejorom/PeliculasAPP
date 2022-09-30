using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;

namespace PeliculasWEB.Repository
{
    public class PeliculaRepository : Repository<Pelicula>, IPeliculaRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public PeliculaRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }
    }
}
