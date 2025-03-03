namespace CatalogAPI.DTOs
{
    public class ProdutoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
