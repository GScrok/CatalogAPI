using Moq;
using Xunit;
using CatalogAPI.Services;
using CatalogAPI.Repositories;
using CatalogAPI.Models;
using CatalogAPI.DTOs;
using AutoMapper;
using CatalogAPI.Exceptions;
using System;
using System.Collections.Generic;

namespace CatalogAPI.Tests
{
    public class ProdutoServiceTests
    {
        private readonly Mock<IProdutoRepository> _mockProdutoRepository;
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly ProdutoService _produtoService;

        public ProdutoServiceTests()
        {
            _mockProdutoRepository = new Mock<IProdutoRepository>();
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();
            _mockMapper = new Mock<IMapper>();
            _produtoService = new ProdutoService(
                _mockProdutoRepository.Object,
                _mockCategoriaRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void ObterTodos_DeveRetornarListaDeProdutoDTO()
        {
            var produtos = new List<Produto>
        {
            new Produto { Id = Guid.NewGuid(), Nome = "Produto 1", Estoque = 10 },
            new Produto { Id = Guid.NewGuid(), Nome = "Produto 2", Estoque = 20 }
        };

            var produtosDTO = new List<ProdutoDTO>
        {
            new ProdutoDTO { Id = produtos[0].Id, Nome = produtos[0].Nome, Estoque = produtos[0].Estoque },
            new ProdutoDTO { Id = produtos[1].Id, Nome = produtos[1].Nome, Estoque = produtos[1].Estoque }
        };

            _mockProdutoRepository
                .Setup(repo => repo.ObterTodos())
                .Returns(produtos);

            _mockMapper
                .Setup(mapper => mapper.Map<List<ProdutoDTO>>(produtos))
                .Returns(produtosDTO);

            var resultado = _produtoService.ObterTodos();

            // Assert
            Assert.NotNull(resultado);
            Assert.Equal(2, resultado.Count);
            Assert.Equal(produtos[0].Nome, resultado[0].Nome);
            Assert.Equal(produtos[1].Nome, resultado[1].Nome);
        }

        [Fact]
        public void ObterPorId_ProdutoNaoEncontrado_DeveLancarExcecao()
        {
            var idProduto = Guid.NewGuid();
            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns((Produto)null);

            Assert.Throws<ProdutoNaoEncontradoException>(() => _produtoService.ObterPorId(idProduto));
        }

        [Fact]
        public void ObterPorId_DeveRetornarProdutoDTO()
        {
            var idProduto = Guid.NewGuid();
            var produto = new Produto { Id = idProduto, Nome = "Produto 1", Estoque = 10 };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns(produto);
            _mockMapper.Setup(mapper => mapper.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO());

            var resultado = _produtoService.ObterPorId(idProduto);


            Assert.IsType<ProdutoDTO>(resultado);
        }

        [Fact]
        public void CriarProduto_DeveRetornarProdutoDTO()
        {
            var categoriaId = Guid.NewGuid();
            var categoria = new Categoria { Id = categoriaId, Nome = "Categoria 1" };
            var criarProdutoDTO = new PostProdutoDTO
            {
                Nome = "Produto Novo",
                Estoque = 10,
                CategoriaId = categoriaId
            };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(categoriaId)).Returns(categoria);
            _mockProdutoRepository.Setup(repo => repo.ObterPorNome(criarProdutoDTO.Nome)).Returns((Produto)null);

            var produto = new Produto { Id = Guid.NewGuid(), Nome = criarProdutoDTO.Nome, Estoque = criarProdutoDTO.Estoque };
            _mockMapper.Setup(mapper => mapper.Map<Produto>(criarProdutoDTO)).Returns(produto);
            _mockMapper.Setup(mapper => mapper.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO());

            var resultado = _produtoService.CriarProduto(criarProdutoDTO);

            Assert.IsType<ProdutoDTO>(resultado);
            _mockProdutoRepository.Verify(repo => repo.Adicionar(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public void AtualizarProduto_ProdutoNaoEncontrado_DeveLancarExcecao()
        {
            var idProduto = Guid.NewGuid();
            var atualizarProdutoDTO = new PostProdutoDTO { Nome = "Produto Atualizado", Estoque = 10 };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns((Produto)null);

            Assert.Throws<ProdutoNaoEncontradoException>(() => _produtoService.AtualizarProduto(idProduto, atualizarProdutoDTO));
        }

        [Fact]
        public void AtualizarProduto_DeveRetornarProdutoDTO()
        {
            var idProduto = Guid.NewGuid();
            var produtoExistente = new Produto { Id = idProduto, Nome = "Produto Antigo", Estoque = 10 };
            var atualizarProdutoDTO = new PostProdutoDTO { Nome = "Produto Atualizado", Estoque = 15 };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns(produtoExistente);
            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(atualizarProdutoDTO.CategoriaId)).Returns(new Categoria());
            _mockProdutoRepository.Setup(repo => repo.ObterPorNome(atualizarProdutoDTO.Nome)).Returns((Produto)null);
            _mockMapper.Setup(mapper => mapper.Map(atualizarProdutoDTO, produtoExistente)).Verifiable();
            _mockProdutoRepository.Setup(repo => repo.Atualizar(produtoExistente)).Returns(produtoExistente);
            _mockMapper.Setup(mapper => mapper.Map<ProdutoDTO>(produtoExistente)).Returns(new ProdutoDTO());

            var resultado = _produtoService.AtualizarProduto(idProduto, atualizarProdutoDTO);

            Assert.IsType<ProdutoDTO>(resultado);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(It.IsAny<Produto>()), Times.Once);
        }

        [Fact]
        public void RemoverProduto_ProdutoNaoEncontrado_DeveLancarExcecao()
        {
            var idProduto = Guid.NewGuid();
            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns((Produto)null);

            Assert.Throws<ProdutoNaoEncontradoException>(() => _produtoService.RemoverProduto(idProduto));
        }

        [Fact]
        public void RemoverProduto_ProdutoComEstoque_DeveLancarExcecao()
        {
            var idProduto = Guid.NewGuid();
            var produto = new Produto { Id = idProduto, Nome = "Produto", Estoque = 10 };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns(produto);

            Assert.Throws<RemocaoProdutoComEstoqueException>(() => _produtoService.RemoverProduto(idProduto));
        }

        [Fact]
        public void AtualizarEstoque_DeveRetornarProdutoDTO()
        {
            var idProduto = Guid.NewGuid();
            var produto = new Produto { Id = idProduto, Nome = "Produto", Estoque = 10 };

            _mockProdutoRepository.Setup(repo => repo.ObterPorId(idProduto)).Returns(produto);
            _mockProdutoRepository.Setup(repo => repo.Atualizar(produto)).Returns(produto);
            _mockMapper.Setup(mapper => mapper.Map<ProdutoDTO>(produto)).Returns(new ProdutoDTO());

            var resultado = _produtoService.AtualizarEstoque(idProduto, 20);

            Assert.IsType<ProdutoDTO>(resultado);
            _mockProdutoRepository.Verify(repo => repo.Atualizar(It.IsAny<Produto>()), Times.Once);
        }
    }
}
