namespace CatalogAPI.DTOs
{
    public class PostProdutoDTO
    {
        public string Nome { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public Guid CategoriaId { get; set; }
    }
}
