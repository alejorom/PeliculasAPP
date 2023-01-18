using Newtonsoft.Json;
using PeliculasWEB.Models;
using PeliculasWEB.Repository.IRepository;
using System.Text;

namespace PeliculasWEB.Repository
{
    public class AccountRepository : Repository<UsuarioM>, IAccountRepository
    {
        private readonly IHttpClientFactory _clientFactory;

        public AccountRepository(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<UsuarioM> LoginAsync(string url, UsuarioM itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                request.Content = new StringContent(
                        JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return new UsuarioM();
            }

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(request);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<UsuarioM>(jsonString);
            }
            else
            {
                return new UsuarioM();
            }

        }

        public async Task<bool> RegisterAsync(string url, UsuarioM itemCrear)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, url);
            if (itemCrear != null)
            {
                request.Content = new StringContent(
                        JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(request);

            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
