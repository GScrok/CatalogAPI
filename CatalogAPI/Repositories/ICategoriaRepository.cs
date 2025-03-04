using CatalogAPI.Models;

namespace CatalogAPI.Repositories
{
    public interface ICategoriaRepository
    {
        List<Categoria> ObterTodos();
        Categoria ObterPorId(Guid id);
        Categoria ObterPorNome(string nome);
        Categoria Adicionar(Categoria categoria);
        Categoria Atualizar(Categoria categoria);
        void Remover(Categoria categoria);
        List<Produto> ObterProdutosPorCategoria(Guid categoriaId);

    }
}
