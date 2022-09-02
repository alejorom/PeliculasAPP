using Microsoft.EntityFrameworkCore;
using PeliculasAPI.Models;

namespace PeliculasAPI.Data
{
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :base(options)
        {
        }

        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Pelicula> Pelicula { get; set; }
    }
}
