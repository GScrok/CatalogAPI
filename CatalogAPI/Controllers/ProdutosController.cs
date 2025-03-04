using CatalogAPI.DTOs;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly IProdutoService _produtoService;

        public ProdutosController(IProdutoService produtoService)
        {
            _produtoService = produtoService;
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            var produtos = _produtoService.ObterTodos();
            return Ok(produtos);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(Guid id)
        {
            var produto = _produtoService.ObterPorId(id);
            if (produto == null)
            {
                return NotFound();
            }

            return Ok(produto);
        }

        [HttpPost]
        public IActionResult CriarProduto([FromBody] PostProdutoDTO criarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var produto = _produtoService.CriarProduto(criarProdutoDTO);
            return Ok(produto);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarProduto(Guid id, [FromBody] PostProdutoDTO atualizarProdutoDTO, bool requisicaoEspecial = false)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var produto = _produtoService.AtualizarProduto(id, atualizarProdutoDTO, requisicaoEspecial);
            return Ok(produto);
        }

        [HttpPatch("{id}/estoque")]
        public IActionResult AtualizarEstoque(Guid id, int novoEstoque, bool requisicaoEspecial = false)
        {
            var produto = _produtoService.AtualizarEstoque(id, novoEstoque, requisicaoEspecial);
            return Ok(produto);
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverProduto(Guid id)
        {
            _produtoService.RemoverProduto(id);
            return NoContent();
        }

        [HttpGet("{id}/categorias")]
        public IActionResult ObterCategoriasDoProduto(Guid id)
        {
            var categorias = _produtoService.ObterProdutosPorCategoria(id);
            return Ok(categorias);
        }
    }
}

