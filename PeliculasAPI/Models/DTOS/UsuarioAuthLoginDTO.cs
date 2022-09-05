using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models.DTOS
{
    public class UsuarioAuthLoginDTO
    {
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatorio")]
        public string Password { get; set; }
    }
}
