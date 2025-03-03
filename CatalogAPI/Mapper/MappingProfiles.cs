using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;

namespace CatalogAPI.mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Mapeamento Produto
            CreateMap<Produto, ProdutoDTO>();
            CreateMap<PostProdutoDTO, Produto>();

            // Mapeamento Categoria
            CreateMap<Categoria, CategoriaDTO>();
            CreateMap<PostCategoriaDTO, Categoria>();
        }
    }
}