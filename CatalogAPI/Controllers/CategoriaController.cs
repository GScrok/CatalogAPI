using CatalogAPI.DTOs;
using CatalogAPI.Models;
using CatalogAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CatalogAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriasController : ControllerBase
    {
        private readonly ICategoriaService _categoriaService;

        public CategoriasController(ICategoriaService categoriaService)
        {
            _categoriaService = categoriaService;
        }

        [HttpGet]
        public IActionResult ObterTodos()
        {
            var categorias = _categoriaService.ObterTodos();
            return Ok(categorias);
        }

        [HttpGet("{id}")]
        public IActionResult ObterPorId(Guid id)
        {
            var categoria = _categoriaService.ObterPorId(id);
            if (categoria == null)
            {
                return NotFound();
            }

            return Ok(categoria);
        }

        [HttpPost]
        public IActionResult CriarCategoria([FromBody] PostCategoriaDTO criarCategoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoria = _categoriaService.CriarCategoria(criarCategoriaDTO);
            return Ok(categoria);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCategoria(Guid id, [FromBody] PostCategoriaDTO atualizarCategoriaDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var categoria = _categoriaService.AtualizarCategoria(id, atualizarCategoriaDTO);
                return Ok(categoria);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public IActionResult RemoverCategoria(Guid id)
        {
            try
            {
                _categoriaService.RemoverCategoria(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{id}/produtos")]
        public IActionResult ObterProdutosDaCategoria(Guid id)
        {
            var produtos = _categoriaService.ObterProdutosPorCategoria(id);
            return Ok(produtos);
        }
    }
}
