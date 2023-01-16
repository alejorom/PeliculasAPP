namespace PeliculasWEB.Utilities
{
    public static class Constantes
    {
        private const string Url = "https://localhost:7081/";
        private static string urlBaseAPI = Url;
        private static string rutaCategoriasApi = UrlBaseAPI + "api/Categorias/";
        private static string rutaPeliculasApi = UrlBaseAPI + "api/Peliculas/";
        private static string rutaUsuariosApi = UrlBaseAPI + "api/Usuarios/";
        private static string rutaPeliculasEnCategoriaApi = UrlBaseAPI + "api/Peliculas/GetPeliculasEnCategoria/";
        private static string rutaPeliculasApiBusqueda = UrlBaseAPI + "api/Peliculas/Buscar?nombre=";

        public static string UrlBaseAPI { get => urlBaseAPI; set => urlBaseAPI = value; }
        public static string RutaCategoriasApi { get => rutaCategoriasApi; set => rutaCategoriasApi = value; }
        public static string RutaPeliculasApi { get => rutaPeliculasApi; set => rutaPeliculasApi = value; }
        public static string RutaUsuariosApi { get => rutaUsuariosApi; set => rutaUsuariosApi = value; }
        public static string RutaPeliculasEnCategoriaApi { get => rutaPeliculasEnCategoriaApi; set => rutaPeliculasEnCategoriaApi = value; }
        public static string RutaPeliculasApiBusqueda { get => rutaPeliculasApiBusqueda; set => rutaPeliculasApiBusqueda = value; }

    }
}
