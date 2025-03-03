using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public interface IProdutoRepository
    {
        List<Produto> ObterTodos();
        Produto ObterPorId(Guid id);
        Produto Adicionar(Produto produto);
        Produto Atualizar(Produto produto);
        void Remover(Produto produto);
        List<Produto> ObterPorCategoria(Guid categoriaId);
    }
}
