using System;
using System.Collections.Generic;
using AutoMapper;
using CatalogAPI.DTOs;
using CatalogAPI.Exceptions;
using CatalogAPI.Models;
using CatalogAPI.Repositories;
using CatalogAPI.Services;
using Moq;
using Xunit;

namespace CatalogAPI.Tests
{
    public class CategoriaServiceTests
    {
        private readonly Mock<ICategoriaRepository> _mockCategoriaRepository;
        private readonly Mock<IProdutoRepository> _mockProdutoRepository;
        private readonly Mock<IMapper> _mockMapper;
        private readonly CategoriaService _categoriaService;

        public CategoriaServiceTests()
        {
            _mockCategoriaRepository = new Mock<ICategoriaRepository>();
            _mockProdutoRepository = new Mock<IProdutoRepository>();
            _mockMapper = new Mock<IMapper>();

            _categoriaService = new CategoriaService(
                _mockCategoriaRepository.Object,
                _mockProdutoRepository.Object,
                _mockMapper.Object
            );
        }

        [Fact]
        public void ObterTodos_DeveRetornarListaDeCategorias()
        {
            
            var categorias = new List<Categoria>
            {
                new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" },
                new Categoria { Id = Guid.NewGuid(), Nome = "Roupas" }
            };

            var categoriasDTO = new List<CategoriaDTO>
            {
                new CategoriaDTO { Id = categorias[0].Id, Nome = "Eletrônicos" },
                new CategoriaDTO { Id = categorias[1].Id, Nome = "Roupas" }
            };

            _mockCategoriaRepository.Setup(repo => repo.ObterTodos()).Returns(categorias);
            _mockMapper.Setup(mapper => mapper.Map<List<CategoriaDTO>>(categorias)).Returns(categoriasDTO);

            
            var result = _categoriaService.ObterTodos();

            
            Assert.Equal(2, result.Count);
            Assert.Equal("Eletrônicos", result[0].Nome);
            Assert.Equal("Roupas", result[1].Nome);
        }

        [Fact]
        public void ObterPorId_DeveRetornarCategoriaCorreta()
        {
            
            var categoriaId = Guid.NewGuid();
            var categoria = new Categoria { Id = categoriaId, Nome = "Eletrônicos" };
            var categoriaDTO = new CategoriaDTO { Id = categoriaId, Nome = "Eletrônicos" };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(categoriaId)).Returns(categoria);
            _mockMapper.Setup(mapper => mapper.Map<CategoriaDTO>(categoria)).Returns(categoriaDTO);

            
            var result = _categoriaService.ObterPorId(categoriaId);

            
            Assert.NotNull(result);
            Assert.Equal(categoriaId, result.Id);
            Assert.Equal("Eletrônicos", result.Nome);
        }

        [Fact]
        public void ObterPorId_DeveLancarExcecaoSeCategoriaNaoEncontrada()
        {
            
            var categoriaId = Guid.NewGuid();
            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(categoriaId)).Returns((Categoria)null);
            
             
            var exception = Assert.Throws<CategoriaNaoEncontradaException>(() => _categoriaService.ObterPorId(categoriaId));
            Assert.Equal("A categoria com o ID informado não foi encontrado.", exception.Message);
        }

        [Fact]
        public void CriarCategoria_DeveCriarCategoriaComSucesso()
        {
            
            var criarCategoriaDTO = new PostCategoriaDTO { Nome = "Eletrônicos" };
            var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };
            var categoriaDTO = new CategoriaDTO { Id = categoria.Id, Nome = "Eletrônicos" };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorNome("Eletrônicos")).Returns((Categoria)null);
            _mockMapper.Setup(mapper => mapper.Map<Categoria>(criarCategoriaDTO)).Returns(categoria);
            _mockMapper.Setup(mapper => mapper.Map<CategoriaDTO>(categoria)).Returns(categoriaDTO);

            
            var result = _categoriaService.CriarCategoria(criarCategoriaDTO);

            
            Assert.NotNull(result);
            Assert.Equal("Eletrônicos", result.Nome);
            _mockCategoriaRepository.Verify(repo => repo.Adicionar(categoria), Times.Once);
        }

        [Fact]
        public void CriarCategoria_DeveLancarExcecaoSeNomeMenorQue3Caracteres()
        {
            
            var criarCategoriaDTO = new PostCategoriaDTO { Nome = "Ei" };

             
            var exception = Assert.Throws<ArgumentException>(() => _categoriaService.CriarCategoria(criarCategoriaDTO));
            Assert.Equal("O nome da categoria deve ter no mínimo 3 caracteres.", exception.Message);
        }

        [Fact]
        public void CriarCategoria_DeveLancarExcecaoSeNomeJaExistir()
        {
            
            var criarCategoriaDTO = new PostCategoriaDTO { Nome = "Eletrônicos" };
            var categoriaExistente = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorNome("Eletrônicos")).Returns(categoriaExistente);

             
            var exception = Assert.Throws<CategoriaDuplicadaException>(() => _categoriaService.CriarCategoria(criarCategoriaDTO));
            Assert.Equal("Já existe uma categoria com esse nome.", exception.Message);
        }

        [Fact]
        public void RemoverCategoria_DeveRemoverCategoriaComSucesso()
        {
            
            var categoriaId = Guid.NewGuid();
            var categoria = new Categoria { Id = categoriaId, Nome = "Eletrônicos" };
            var categoriaPadrao = new Categoria { Id = Guid.NewGuid(), Nome = "Sem Categoria" };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(categoriaId)).Returns(categoria);
            _mockCategoriaRepository.Setup(repo => repo.ObterPorNome("Sem Categoria")).Returns(categoriaPadrao);
            _mockProdutoRepository.Setup(repo => repo.ObterPorCategoria(categoriaId)).Returns(new List<Produto>());

            
            _categoriaService.RemoverCategoria(categoriaId);

            
            _mockCategoriaRepository.Verify(repo => repo.Remover(categoria), Times.Once);
        }

        [Fact]
        public void ObterProdutosPorCategoria_DeveRetornarProdutosCorretos()
        {
            
            var categoriaId = Guid.NewGuid();
            var categoria = new Categoria { Id = categoriaId, Nome = "Eletrônicos" };
            var produtos = new List<Produto>
            {
                new Produto { Id = Guid.NewGuid(), Nome = "Smartphone", CategoriaId = categoriaId },
                new Produto { Id = Guid.NewGuid(), Nome = "Notebook", CategoriaId = categoriaId }
            };
            var produtosDTO = new List<ProdutoDTO>
            {
                new ProdutoDTO { Id = produtos[0].Id, Nome = "Smartphone" },
                new ProdutoDTO { Id = produtos[1].Id, Nome = "Notebook" }
            };

            _mockCategoriaRepository.Setup(repo => repo.ObterPorId(categoriaId)).Returns(categoria);
            _mockProdutoRepository.Setup(repo => repo.ObterPorCategoria(categoriaId)).Returns(produtos);
            _mockMapper.Setup(mapper => mapper.Map<List<ProdutoDTO>>(produtos)).Returns(produtosDTO);

            
            var result = _categoriaService.ObterProdutosPorCategoria(categoriaId);

            
            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Nome);
            Assert.Equal("Notebook", result[1].Nome);
        }
    }
}