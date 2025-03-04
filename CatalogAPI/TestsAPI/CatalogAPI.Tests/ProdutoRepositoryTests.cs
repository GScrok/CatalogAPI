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
    public class ProdutoRepositoryTests
    {
        private readonly AppDbContext _context;
        private readonly ProdutoRepository _produtoRepository;

        public ProdutoRepositoryTests()
        {
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseInMemoryDatabase(databaseName: "CatalogApiInMemoryDb")
                .Options;

            _context = new AppDbContext(options);
            _produtoRepository = new ProdutoRepository(_context);

            // Limpar o banco de dados e adicionar os produtos esperados antes de cada teste
            _context.Produtos.RemoveRange(_context.Produtos); // Limpa qualquer dado existente
            _context.SaveChanges();

            // Adicionar os produtos esperados
            if (!_context.Produtos.Any())
            {
                _context.Produtos.AddRange(new List<Produto>
                {
                    new Produto { Id = Guid.NewGuid(), Nome = "Produto 1", CategoriaId = Guid.NewGuid() },
                    new Produto { Id = Guid.NewGuid(), Nome = "Produto 2", CategoriaId = Guid.NewGuid() }
                });

                _context.SaveChanges();
            }
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodosOsProdutos()
        {
            var produtos = _produtoRepository.ObterTodos();

            // Imprimir as categorias para depuração
            Console.WriteLine($"Total de Produtos: {produtos.Count()}");
            foreach (var produto in produtos)
            {
                Console.WriteLine($"Id: {produto.Id}, Nome: {produto.Nome}");
            }

            Assert.NotNull(produtos);
            Assert.Equal(2, produtos.Count()); // Agora deve retornar 2
        }

        [Fact]
        public void ObterPorNome_DeveRetornarProdutoQuandoNomeExistir()
        {
            var produtoRetornado = _produtoRepository.ObterPorNome("Produto 1");

            // Imprimir para depuração
            Console.WriteLine($"Produto Retornado: Id: {produtoRetornado?.Id}, Nome: {produtoRetornado?.Nome}");

            Assert.NotNull(produtoRetornado);
            Assert.Equal("Produto 1", produtoRetornado.Nome); // Verifica se o nome é correto
        }

        [Fact]
        public void ObterPorNome_DeveRetornarNullQuandoNomeNaoExistir()
        {
            var produtoRetornado = _produtoRepository.ObterPorNome("Produto Inexistente");

            Assert.Null(produtoRetornado);
        }

        [Fact]
        public void ObterPorId_DeveRetornarProdutoQuandoIdExistir()
        {
            var produto = _context.Produtos.FirstOrDefault();
            Assert.NotNull(produto);

            var produtoRetornado = _produtoRepository.ObterPorId(produto.Id);

            Assert.NotNull(produtoRetornado);
            Assert.Equal(produto.Id, produtoRetornado.Id);
        }

        [Fact]
        public void ObterPorId_DeveRetornarNullQuandoIdNaoExistir()
        {
            var produtoRetornado = _produtoRepository.ObterPorId(Guid.NewGuid());

            Assert.Null(produtoRetornado);
        }

        [Fact]
        public void Adicionar_DeveAdicionarProduto()
        {
            var produto = new Produto { Id = Guid.NewGuid(), Nome = "Produto 3", CategoriaId = Guid.NewGuid() };

            var produtoAdicionado = _produtoRepository.Adicionar(produto);

            Assert.NotNull(produtoAdicionado);
            Assert.Equal("Produto 3", produtoAdicionado.Nome);
        }

        [Fact]
        public void Atualizar_DeveAtualizarProduto()
        {
            var produto = _context.Produtos.FirstOrDefault();
            Assert.NotNull(produto);

            produto.Nome = "Produto Atualizado";
            var produtoAtualizado = _produtoRepository.Atualizar(produto);

            Assert.NotNull(produtoAtualizado);
            Assert.Equal("Produto Atualizado", produtoAtualizado.Nome);
        }

        [Fact]
        public void Remover_DeveRemoverProduto()
        {
            var produto = _context.Produtos.FirstOrDefault();
            Assert.NotNull(produto);

            _produtoRepository.Remover(produto);

            var produtoRemovido = _produtoRepository.ObterPorId(produto.Id);

            Assert.Null(produtoRemovido);
        }

        [Fact]
        public void ObterPorCategoria_DeveRetornarProdutos()
        {
            var categoria = _context.Categorias.FirstOrDefault();
            Assert.NotNull(categoria);

            var produtos = _produtoRepository.ObterPorCategoria(categoria.Id);

            Assert.NotNull(produtos);
        }
    }
}
