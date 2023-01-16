using Microsoft.AspNetCore.Mvc.Rendering;

namespace PeliculasWEB.Models.ViewModels
{
    public class PeliculasVM
    {
        public IEnumerable<SelectListItem> ListaCategorias { get; set; }
        public Pelicula Pelicula { get; set; }
    }
}
