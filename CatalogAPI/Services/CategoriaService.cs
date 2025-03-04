using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using CatalogAPI.Repositories;

namespace CatalogAPI.Services
{
    public class CategoriaService : ICategoriaService
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public CategoriaService(ICategoriaRepository categoriaRepository, IProdutoRepository produtoRepository, IMapper mapper)
        {
            _categoriaRepository = categoriaRepository;
            _produtoRepository = produtoRepository;
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
            if (categoria == null)
            {
                throw new CategoriaNaoEncontradaException("A categoria com o ID informado não foi encontrado.");
            }
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public CategoriaDTO CriarCategoria(PostCategoriaDTO criarCategoriaDTO)
        {
            if (criarCategoriaDTO.Nome.Length < 3)
            {
                throw new ArgumentException("O nome da categoria deve ter no mínimo 3 caracteres.");
            }

            var categoriaExistente = _categoriaRepository.ObterPorNome(criarCategoriaDTO.Nome);
            if (categoriaExistente != null)
            {
                throw new CategoriaDuplicadaException("Já existe uma categoria com esse nome.");
            }

            var categoria = _mapper.Map<Categoria>(criarCategoriaDTO);
            _categoriaRepository.Adicionar(categoria);
            return _mapper.Map<CategoriaDTO>(categoria);
        }

        public CategoriaDTO AtualizarCategoria(Guid id, PostCategoriaDTO atualizarCategoriaDTO)
        {
            var categoriaExistente = _categoriaRepository.ObterPorId(id);
            if (categoriaExistente == null)
            {
                throw new CategoriaNaoEncontradaException("A categoria com o ID informado não foi encontrado.");
            }

            if (atualizarCategoriaDTO.Nome.Length < 3)
            {
                throw new ArgumentException("O nome da categoria deve ter no mínimo 3 caracteres.");
            }

            var categoriaComMesmoNome = _categoriaRepository.ObterPorNome(atualizarCategoriaDTO.Nome);
            if (categoriaComMesmoNome != null && categoriaComMesmoNome.Id != id)
            {
                throw new CategoriaDuplicadaException("Já existe uma categoria com esse nome.");
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
                throw new CategoriaNaoEncontradaException("A categoria com o ID informado não foi encontrado.");
            }

            var produtosDaCategoria = _produtoRepository.ObterPorCategoria(id);

            var categoriaPadrao = _categoriaRepository.ObterPorNome("Sem Categoria");

            if (categoriaPadrao == null)
            {
                categoriaPadrao = new Categoria { Nome = "Sem Categoria" };
                _categoriaRepository.Adicionar(categoriaPadrao);
            }

            foreach (var produto in produtosDaCategoria)
            {
                produto.CategoriaId = categoriaPadrao.Id;
                _produtoRepository.Atualizar(produto);
            }

            _categoriaRepository.Remover(categoria);
        }

        public List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId)
        {
            var categoria = _categoriaRepository.ObterPorId(categoriaId);
            if (categoria == null)
            {
                throw new CategoriaNaoEncontradaException("A categoria com o ID informado não foi encontrado.");
            }

            var produtos = _produtoRepository.ObterPorCategoria(categoriaId);

            var produtosDTO = _mapper.Map<List<ProdutoDTO>>(produtos);

            return produtosDTO;
        }
    }
}
