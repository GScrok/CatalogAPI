using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using CatalogAPI.Repositories;

namespace CatalogAPI.Services
{
    public class ProdutoService : IProdutoService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IMapper _mapper;

        public ProdutoService(IProdutoRepository produtoRepository, ICategoriaRepository categoriaRepository, IMapper mapper)
        {
            _produtoRepository = produtoRepository;
            _categoriaRepository = categoriaRepository;
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
            if (produto == null)
            {
                throw new ProdutoNaoEncontradoException("O produto com o ID informado não foi encontrado.");
            }
            return _mapper.Map<ProdutoDTO>(produto);
        }
        public List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId)
        {
            var produtos = _produtoRepository.ObterPorCategoria(categoriaId);
            return _mapper.Map<List<ProdutoDTO>>(produtos);
        }

        public ProdutoDTO CriarProduto(PostProdutoDTO criarProdutoDTO)
        {
            var categoria = _categoriaRepository.ObterPorId(criarProdutoDTO.CategoriaId);
            if (categoria == null)
            {
                throw new CategoriaNaoEncontradaException("A categoria informada não existe.");
            }

            var produtoExistente = _produtoRepository.ObterPorNome(criarProdutoDTO.Nome);
            if (produtoExistente != null)
            {
                throw new ProdutoDuplicadoException("Já existe um produto com esse nome.");
            }

            var produto = _mapper.Map<Produto>(criarProdutoDTO);
            _produtoRepository.Adicionar(produto);

            return _mapper.Map<ProdutoDTO>(produto);
        }

        public ProdutoDTO AtualizarProduto(Guid id, PostProdutoDTO atualizarProdutoDTO, bool requisicaoEspecial = false)
        {
            var produtoExistente = _produtoRepository.ObterPorId(id);
            if (produtoExistente == null)
            {
                throw new ProdutoNaoEncontradoException("O produto com o ID informado não foi encontrado.");
            }

            var categoria = _categoriaRepository.ObterPorId(atualizarProdutoDTO.CategoriaId);
            if (categoria == null)
            {
                throw new CategoriaNaoEncontradaException("A categoria informada não existe.");
            }

            var produtoComMesmoNome = _produtoRepository.ObterPorNome(atualizarProdutoDTO.Nome);
            if (produtoComMesmoNome != null && produtoComMesmoNome.Id != id)
            {
                throw new ProdutoDuplicadoException("Já existe um produto com esse nome.");
            }

            if (atualizarProdutoDTO.Estoque < 5 && !requisicaoEspecial)
            {
                throw new EstoqueInvalidoException("O estoque não pode ser menor que 5 unidades.");
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
                throw new ProdutoNaoEncontradoException("O produto com o ID informado não foi encontrado.");
            }

            if (produto.Estoque > 0)
            {
                throw new RemocaoProdutoComEstoqueException("Não é possível remover um produto com estoque maior que zero.");
            }

            _produtoRepository.Remover(produto);
        }

        public ProdutoDTO AtualizarEstoque(Guid id, int novoEstoque, bool requisicaoEspecial = false)
        {
            var produto = _produtoRepository.ObterPorId(id);
            if (produto == null)
            {
                throw new ProdutoNaoEncontradoException("O produto com o ID informado não foi encontrado.");
            }

            if (novoEstoque < 5 && !requisicaoEspecial)
            {
                throw new EstoqueInvalidoException("O estoque não pode ser menor que 5 unidades.");
            }

            produto.Estoque = novoEstoque;
            _produtoRepository.Atualizar(produto);

            return _mapper.Map<ProdutoDTO>(produto);
        }
    }
}
