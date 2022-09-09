using Microsoft.AspNetCore.Identity;
using PeliculasAPI.Data;
using PeliculasAPI.Models;
using PeliculasAPI.Repository.IRepository;

namespace PeliculasAPI.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly ApplicationDbContext _bd;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="bd"></param>
        public UsuarioRepository(ApplicationDbContext bd)
        {
            _bd = bd;
        }

        /// <summary>
        /// Existe usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <returns></returns>
        public bool ExisteUsuario(string usuario)
        {
            if (_bd.Usuario.Any(x => x.UsuarioA == usuario))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Obtener usuario individual
        /// </summary>
        /// <param name="UsuarioId">ID del usuario</param>
        /// <returns></returns>
        public Usuario GetUsuario(int UsuarioId)
        {
            return _bd.Usuario.FirstOrDefault(c => c.Id == UsuarioId);
        }

        /// <summary>
        /// Obtener todos los usuarios
        /// </summary>
        /// <returns></returns>
        public ICollection<Usuario> GetUsuarios()
        {
            return _bd.Usuario.OrderBy(c => c.UsuarioA).ToList();
        }

        /// <summary>
        /// Almacenar cambios
        /// </summary>
        /// <returns></returns>
        public bool Guardar()
        {
            return _bd.SaveChanges() >= 0;
        }

        /// <summary>
        /// Realizar login del usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Usuario Login(string usuario, string password)
        {
            var user = _bd.Usuario.FirstOrDefault(x => x.UsuarioA == usuario);

            if (user == null)
            {
                return null;
            }

            if (!VerificaPasswordHash(password, user.PasswordHash, user.PasswordSalt))
            {
                return null;
            }

            return user;
        }

        /// <summary>
        /// Crear nuevo usuario
        /// </summary>
        /// <param name="usuario"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public Usuario Registro(Usuario usuario, string password)
        {

            CrearPasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            usuario.PasswordHash = passwordHash;
            usuario.PasswordSalt = passwordSalt;

            _bd.Usuario.Add(usuario);
            Guardar();
            return usuario;
        }

        /// <summary>
        /// Verificar password hash
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        /// <returns></returns>
        private bool VerificaPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var hashComputado = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

                for (int i = 0; i < hashComputado.Length; i++)
                {
                    if (hashComputado[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Crear password hast
        /// </summary>
        /// <param name="password"></param>
        /// <param name="passwordHash"></param>
        /// <param name="passwordSalt"></param>
        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new System.Security.Cryptography.HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
}
