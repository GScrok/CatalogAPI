using CatalogAPI.Models.Base;

namespace CatalogAPI.Models
{
    public class Produto : BaseEntity
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }
    }
}
