using CatalogAPI.Models.Context;
using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public class ProdutoRepository : IProdutoRepository
    {
        private readonly AppDbContext _context;

        public ProdutoRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Produto> ObterTodos()
        {
            return _context.Produtos.ToList();
        }

        public Produto ObterPorId(Guid id)
        {
            return _context.Produtos.FirstOrDefault(p => p.Id == id);
        }

        public Produto Adicionar(Produto produto)
        {
            _context.Produtos.Add(produto);
            _context.SaveChanges();
            return produto;
        }

        public Produto Atualizar(Produto produto)
        {
            _context.Produtos.Update(produto);
            _context.SaveChanges();
            return produto;
        }

        public void Remover(Produto produto)
        {
            _context.Produtos.Remove(produto);
            _context.SaveChanges();
        }

        public List<Produto> ObterPorCategoria(Guid categoriaId)
        {
            return _context.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .ToList();
        }
    }
}
