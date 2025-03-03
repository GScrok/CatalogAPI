using CatalogAPI.Models.Base;

namespace CatalogAPI.Models
{
    public class Categoria : BaseEntity
    {
        public string Nome { get; set; }
        public List<Produto> Produtos { get; set; } = new();
    }
}
