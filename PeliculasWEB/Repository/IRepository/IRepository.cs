﻿using System.Collections;

namespace PeliculasWEB.Repository.IRepository
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable?> GetTodoAsync(string url);
        //Método para filtrar películas en una categoría
        Task<IEnumerable?> GetPeliculasEnCategoriaAsync(string url, int categoriaId);
        //Método para buscar peliculas por nombre
        Task<IEnumerable?> BuscarAsync(string url, string nombre);
        Task<T?> GetAsync(string url, int Id);
        Task<bool> CrearAsync(string url, T itemCrear, string token);
        Task<bool> ActualizarAsync(string url, T itemActualizar, string token);
        Task<bool> BorrarAsync(string url, int Id, string token);
    }
}
