namespace PeliculasAPI.Models.DTOS
{
    public class UsuarioDTO
    {
        public int Id { get; set; }
        public string UsuarioA { get; set; }
        public byte[] PasswordHash { get; set; }
    }
}
