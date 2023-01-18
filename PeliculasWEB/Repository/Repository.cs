using Newtonsoft.Json;
using PeliculasWEB.Repository.IRepository;
using System.Collections;
using System.Text;

namespace PeliculasWEB.Repository
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IHttpClientFactory _clientFactory;

        public Repository(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="itemActualizar"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> ActualizarAsync(string url, T itemActualizar, string token)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Patch, url);

            if (itemActualizar != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemActualizar), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            // Validar si se actualizo y retorna booleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Id"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> BorrarAsync(string url, int Id, string token)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Delete, url + Id);

            var cliente = _clientFactory.CreateClient();

            //Aquí valida token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna boleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="nombre"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<IEnumerable?> BuscarAsync(string url, string nombre)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + nombre);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="itemCrear"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<bool> CrearAsync(string url, T itemCrear, string token)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Post, url);

            if (itemCrear != null)
            {
                peticion.Content = new StringContent(
                    JsonConvert.SerializeObject(itemCrear), Encoding.UTF8, "application/json"
                    );
            }
            else
            {
                return false;
            }

            var cliente = _clientFactory.CreateClient();

            // Validar token
            if (token != null && token.Length != 0)
            {
                cliente.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            // Validar si se creó y retorna booleano
            if (respuesta.StatusCode == System.Net.HttpStatusCode.Created)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="Id"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task<T?> GetAsync(string url, int Id)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + Id);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<T?>(jsonString);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <param name="categoriaId"></param>
        /// <returns></returns>
        public async Task<IEnumerable?> GetPeliculasEnCategoriaAsync(string url, int categoriaId)
        {
            var peticion = new HttpRequestMessage(HttpMethod.Get, url + categoriaId);

            var cliente = _clientFactory.CreateClient();

            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se encontraron y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T>>(jsonString);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<IEnumerable?> GetTodoAsync(string url)
        {
            HttpRequestMessage peticion = new HttpRequestMessage(HttpMethod.Get, url);
            var cliente = _clientFactory.CreateClient();
            HttpResponseMessage respuesta = await cliente.SendAsync(peticion);

            //Validar si se actualizo y retorna los datos
            if (respuesta.StatusCode == System.Net.HttpStatusCode.OK)
            {
                var jsonString = await respuesta.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<IEnumerable<T?>>(jsonString);
            }
            else
            {
                return null;
            }
        }
    }
}
