using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Repositories;

namespace CatalogAPI.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _mapper = mapper;
        }

        public List<ProdutoDTO> ObterTodos()
        {
            var produtos = _produtoRepository.ObterTodos();
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }

        public ProdutoDTO ObterPorId(Guid id)
        {
            var produto = _produtoRepository.ObterPorId(id);
            return _mapper.Map<ProdutoDTO>(produto);
        }

        public ProdutoDTO CriarProduto(PostProdutoDTO criarProdutoDTO)
        {
            var produto = _mapper.Map<Produto>(criarProdutoDTO);
            _produtoRepository.Adicionar(produto);
            return _mapper.Map<ProdutoDTO>(produto);
        }

        public ProdutoDTO AtualizarProduto(Guid id, PostProdutoDTO atualizarProdutoDTO)
        {
            var produtoExistente = _produtoRepository.ObterPorId(id);
            if (produtoExistente == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            _mapper.Map(atualizarProdutoDTO, produtoExistente);
            var produto = _produtoRepository.Atualizar(produtoExistente);
            return _mapper.Map<ProdutoDTO>(produto);
        }

        public void RemoverProduto(Guid id)
        {
            var produto = _produtoRepository.ObterPorId(id);
            if (produto == null)
            {
                throw new Exception("Produto não encontrado.");
            }

            _produtoRepository.Remover(produto);
        }

        public List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId)
        {
            var produtos = _produtoRepository.ObterPorCategoria(categoriaId);
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }
    }
}
