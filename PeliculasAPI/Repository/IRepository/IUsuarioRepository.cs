﻿using PeliculasAPI.Models;

namespace PeliculasAPI.Repository.IRepository
{
    public interface IUsuarioRepository
    {
        ICollection<Usuario> GetUsuarios();
        Usuario GetUsuario(int UsuarioId);
        bool ExisteUsuario(string usuario);
        Usuario Registro(Usuario usuario, string password);
        Usuario Login(string usuario, string password);
        bool Guardar();
    }
}
