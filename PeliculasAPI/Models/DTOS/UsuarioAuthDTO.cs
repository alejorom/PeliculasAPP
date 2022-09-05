using System.ComponentModel.DataAnnotations;

namespace PeliculasAPI.Models.DTOS
{
    public class UsuarioAuthDTO
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "El usuario es obligatorio")]
        public string Usuario { get; set; }
        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(20, MinimumLength = 8, ErrorMessage = "La contraseña debe ser entre 8 y 20 caracteres")]
        public string Password { get; set; }
    }
}
