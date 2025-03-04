using CatalogAPI.Models;
using CatalogAPI.Models.Context;
using CatalogAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace CatalogAPI.Tests
{
    public class CategoriaRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly CategoriaRepository _categoriaRepository;

        public CategoriaRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CatalogApiInMemoryDb")
                .Options;

            _context = new AppDbContext(options);
            _categoriaRepository = new CategoriaRepository(_context);

            _context.Categorias.RemoveRange(_context.Categorias);
            _context.SaveChanges();

            if (!_context.Categorias.Any())
            {
                _context.Categorias.AddRange(new List<Categoria>
                {
                    new Categoria { Id = Guid.NewGuid(), Nome = "Categoria 1" },
                    new Categoria { Id = Guid.NewGuid(), Nome = "Categoria 2" }
                });

                _context.SaveChanges();
            }
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodasAsCategorias()
        {
            var categorias = _categoriaRepository.ObterTodos();

            Assert.NotNull(categorias);
            Assert.Equal(2, categorias.Count());
        }

        [Fact]
        public void ObterPorNome_DeveRetornarCategoriaQuandoNomeExistir()
        {
            var categoriaRetornada = _categoriaRepository.ObterPorNome("Categoria 1");

            Assert.NotNull(categoriaRetornada);
            Assert.Equal("Categoria 1", categoriaRetornada.Nome);
        }

        [Fact]
        public void ObterPorNome_DeveRetornarNullQuandoNomeNaoExistir()
        {
            var categoriaRetornada = _categoriaRepository.ObterPorNome("Categoria Inexistente");

            Assert.Null(categoriaRetornada);
        }

        [Fact]
        public void ObterPorId_DeveRetornarCategoriaQuandoIdExistir()
        {
            var categoria = _context.Categorias.FirstOrDefault();
            Assert.NotNull(categoria);

            var categoriaRetornada = _categoriaRepository.ObterPorId(categoria.Id);

            Assert.NotNull(categoriaRetornada);
            Assert.Equal(categoria.Id, categoriaRetornada.Id);
        }

        [Fact]
        public void ObterPorId_DeveRetornarNullQuandoIdNaoExistir()
        {
            var categoriaRetornada = _categoriaRepository.ObterPorId(Guid.NewGuid());

            Assert.Null(categoriaRetornada);
        }

        [Fact]
        public void Adicionar_DeveAdicionarCategoria()
        {
            var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Categoria 3" };

            var categoriaAdicionada = _categoriaRepository.Adicionar(categoria);

            Assert.NotNull(categoriaAdicionada);
            Assert.Equal("Categoria 3", categoriaAdicionada.Nome);
        }

        [Fact]
        public void Atualizar_DeveAtualizarCategoria()
        {
            var categoria = _context.Categorias.FirstOrDefault();
            Assert.NotNull(categoria);

            categoria.Nome = "Categoria Atualizada";
            var categoriaAtualizada = _categoriaRepository.Atualizar(categoria);

            Assert.NotNull(categoriaAtualizada);
            Assert.Equal("Categoria Atualizada", categoriaAtualizada.Nome);
        }

        [Fact]
        public void Remover_DeveRemoverCategoria()
        {
            var categoria = _context.Categorias.FirstOrDefault();
            Assert.NotNull(categoria);

            _categoriaRepository.Remover(categoria);

            var categoriaRemovida = _categoriaRepository.ObterPorId(categoria.Id);

            Assert.Null(categoriaRemovida);
        }

        [Fact]
        public void ObterProdutosPorCategoria_DeveRetornarProdutos()
        {
            var categoria = _context.Categorias.FirstOrDefault();
            Assert.NotNull(categoria);

            var produtos = _categoriaRepository.ObterProdutosPorCategoria(categoria.Id);

            Assert.NotNull(produtos);
        }
    }
}
