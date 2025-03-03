using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Repositories;

namespace CatalogAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _mapper = mapper;
        }

        public List<CategoriaDTO> ObterTodos()
        {
            var categorias = _categoriaRepository.ObterTodos();
            return _mapper.Map<List<CategoriaDTO>>(categorias);
        }

        public CategoriaDTO ObterPorId(Guid id)
        {
            var categoria = _categoriaRepository.ObterPorId(id);
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public CategoriaDTO CriarCategoria(PostCategoriaDTO criarCategoriaDTO)
        {
            var categoria = _mapper.Map<Categoria>(criarCategoriaDTO);
            _categoriaRepository.Adicionar(categoria);
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public CategoriaDTO AtualizarCategoria(Guid id, PostCategoriaDTO atualizarCategoriaDTO)
        {
            var categoriaExistente = _categoriaRepository.ObterPorId(id);
            if (categoriaExistente == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            _mapper.Map(atualizarCategoriaDTO, categoriaExistente);
            var categoria = _categoriaRepository.Atualizar(categoriaExistente);
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public void RemoverCategoria(Guid id)
        {
            var categoria = _categoriaRepository.ObterPorId(id);
            if (categoria == null)
            {
                throw new Exception("Categoria não encontrada.");
            }

            _categoriaRepository.Remover(categoria);
        }

        public List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId)
        {
            var produtos = _categoriaRepository.ObterProdutosPorCategoria(categoriaId);
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }
    }
}
