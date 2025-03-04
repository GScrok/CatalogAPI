using CatalogAPI.Models.Context;
using CatalogAPI.Models;
using System.Linq;

namespace CatalogAPI.Repositories
{
    public class CategoriaRepository : ICategoriaRepository
    {
        private readonly AppDbContext _context;

        public CategoriaRepository(AppDbContext context)
        {
            _context = context;
        }

        public List<Categoria> ObterTodos()
        {
            return _context.Categorias.ToList();
        }

        public Categoria ObterPorNome(string nome)
        {
            return _context.Categorias
                .Where(c => c.Nome.Equals(nome, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();
        }

        public Categoria ObterPorId(Guid id)
        {
            return _context.Categorias.FirstOrDefault(c => c.Id == id);
        }

        public Categoria Adicionar(Categoria categoria)
        {
            _context.Categorias.Add(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public Categoria Atualizar(Categoria categoria)
        {
            _context.Categorias.Update(categoria);
            _context.SaveChanges();
            return categoria;
        }

        public void Remover(Categoria categoria)
        {
            _context.Categorias.Remove(categoria);
            _context.SaveChanges();
        }

        public List<Produto> ObterProdutosPorCategoria(Guid categoriaId)
        {
            return _context.Produtos
                .Where(p => p.CategoriaId == categoriaId)
                .ToList();
        }
    }
}
