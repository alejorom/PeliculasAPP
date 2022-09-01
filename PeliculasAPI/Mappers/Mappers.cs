using AutoMapper;
using PeliculasAPI.Models;
using PeliculasAPI.Models.DTOS;

namespace PeliculasAPI.Mappers
{
    public class Mappers : Profile
    {
        public Mappers()
        {
            CreateMap<Categoria, CategoriaDTO>().ReverseMap();
        }
    }
}
