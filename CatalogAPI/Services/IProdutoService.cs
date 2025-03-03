﻿using CatalogAPI.DTOs;

namespace CatalogAPI.Services
{
    public interface IProdutoService
    {
        List<ProdutoDTO> ObterTodos();
        ProdutoDTO ObterPorId(Guid id);
        ProdutoDTO CriarProduto(PostProdutoDTO criarProdutoDTO);
        ProdutoDTO AtualizarProduto(Guid id, PostProdutoDTO atualizarProdutoDTO);
        void RemoverProduto(Guid id);
        List<ProdutoDTO> ObterProdutosPorCategoria(Guid categoriaId);
    }
}
