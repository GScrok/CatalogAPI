using CatalogAPI.Models;
using CatalogAPI.Models.Context;
using CatalogAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace Tests
{
    public class CategoriaRepositoryTests
    {
        private readonly Mock<AppDbContext> _mockContext;
        private readonly CategoriaRepository _repository;

        public CategoriaRepositoryTests()
        {
            // Configurar o mock do DbContext
            _mockContext = new Mock<AppDbContext>();

            // Inicializar o repositório com o mock do DbContext
            _repository = new CategoriaRepository(_mockContext.Object);
        }

        [Fact]
        public void ObterTodos_DeveRetornarTodasAsCategorias()
        {
            
            var categorias = new List<Categoria>
            {
                new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" },
                new Categoria { Id = Guid.NewGuid(), Nome = "Roupas" }
            };

            var mockDbSet = GetMockDbSet(categorias);
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.ObterTodos();

            
            Assert.Equal(2, result.Count);
            Assert.Equal("Eletrônicos", result[0].Nome);
            Assert.Equal("Roupas", result[1].Nome);
        }

        [Fact]
        public void ObterPorNome_DeveRetornarCategoriaCorreta()
        {
            
            var categorias = new List<Categoria>
            {
                new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" },
                new Categoria { Id = Guid.NewGuid(), Nome = "Roupas" }
            };

            var mockDbSet = GetMockDbSet(categorias);
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.ObterPorNome("Eletrônicos");

            
            Assert.NotNull(result);
            Assert.Equal("Eletrônicos", result.Nome);
        }

        [Fact]
        public void ObterPorNome_DeveRetornarNuloSeNaoEncontrar()
        {
            
            var categorias = new List<Categoria>
            {
                new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" }
            };

            var mockDbSet = GetMockDbSet(categorias);
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.ObterPorNome("Roupas");

            
            Assert.Null(result);
        }

        [Fact]
        public void ObterPorId_DeveRetornarCategoriaCorreta()
        {
            
            var categoriaId = Guid.NewGuid();
            var categorias = new List<Categoria>
            {
                new Categoria { Id = categoriaId, Nome = "Eletrônicos" }
            };

            var mockDbSet = GetMockDbSet(categorias);
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.ObterPorId(categoriaId);

            
            Assert.NotNull(result);
            Assert.Equal(categoriaId, result.Id);
        }

        [Fact]
        public void ObterPorId_DeveRetornarNuloSeNaoEncontrar()
        {
            
            var categorias = new List<Categoria>
            {
                new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" }
            };

            var mockDbSet = GetMockDbSet(categorias);
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.ObterPorId(Guid.NewGuid());

            
            Assert.Null(result);
        }

        [Fact]
        public void Adicionar_DeveAdicionarCategoria()
        {
            
            var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };
            var mockDbSet = new Mock<DbSet<Categoria>>();
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.Adicionar(categoria);

            
            mockDbSet.Verify(m => m.Add(It.IsAny<Categoria>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.Equal(categoria, result);
        }

        [Fact]
        public void Atualizar_DeveAtualizarCategoria()
        {
            
            var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };
            var mockDbSet = new Mock<DbSet<Categoria>>();
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            var result = _repository.Atualizar(categoria);

            
            mockDbSet.Verify(m => m.Update(It.IsAny<Categoria>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
            Assert.Equal(categoria, result);
        }

        [Fact]
        public void Remover_DeveRemoverCategoria()
        {
            
            var categoria = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };
            var mockDbSet = new Mock<DbSet<Categoria>>();
            _mockContext.Setup(c => c.Categorias).Returns(mockDbSet.Object);

            
            _repository.Remover(categoria);

            
            mockDbSet.Verify(m => m.Remove(It.IsAny<Categoria>()), Times.Once);
            _mockContext.Verify(m => m.SaveChanges(), Times.Once);
        }

        [Fact]
        public void ObterProdutosPorCategoria_DeveRetornarProdutosCorretos()
        {
            
            var categoriaId = Guid.NewGuid();
            var produtos = new List<Produto>
            {
                new Produto { Id = Guid.NewGuid(), Nome = "Smartphone", CategoriaId = categoriaId },
                new Produto { Id = Guid.NewGuid(), Nome = "Notebook", CategoriaId = categoriaId }
            };

            var mockDbSet = GetMockDbSet(produtos);
            _mockContext.Setup(c => c.Produtos).Returns(mockDbSet.Object);

            
            var result = _repository.ObterProdutosPorCategoria(categoriaId);

            
            Assert.Equal(2, result.Count);
            Assert.Equal("Smartphone", result[0].Nome);
            Assert.Equal("Notebook", result[1].Nome);
        }

        private Mock<DbSet<T>> GetMockDbSet<T>(List<T> data) where T : class
        {
            var queryable = data.AsQueryable();
            var mockDbSet = new Mock<DbSet<T>>();
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Provider).Returns(queryable.Provider);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.Expression).Returns(queryable.Expression);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.ElementType).Returns(queryable.ElementType);
            mockDbSet.As<IQueryable<T>>().Setup(m => m.GetEnumerator()).Returns(queryable.GetEnumerator());
            return mockDbSet;
        }
    }
}