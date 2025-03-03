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
        public IActionResult AtualizarProduto(Guid id, [FromBody] PostProdutoDTO atualizarProdutoDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var produto = _produtoService.AtualizarProduto(id, atualizarProdutoDTO);
                return Ok(produto);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverProduto(Guid id)
        {
            try
            {
                _produtoService.RemoverProduto(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/categorias")]
        public IActionResult ObterCategoriasDoProduto(Guid id)
        {
            var categorias = _produtoService.ObterProdutosPorCategoria(id);
            return Ok(categorias);
        }
    }
}

