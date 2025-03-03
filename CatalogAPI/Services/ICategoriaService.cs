using CatalogAPI.DTOs;

namespace CatalogAPI.Services
{
    public interface ICategoriaService
    {
        List<CategoriaDTO> ObterTodos();
        CategoriaDTO ObterPorId(Guid id);
        CategoriaDTO CriarCategoria(PostCategoriaDTO criarCategoriaDTO);
        CategoriaDTO AtualizarCategoria(Guid id, PostCategoriaDTO atualizarCategoriaDTO);
        void RemoverCategoria(Guid id);
        List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId);
    }
}
