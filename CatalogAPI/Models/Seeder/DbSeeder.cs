using CatalogAPI.Models.Context;

namespace CatalogAPI.Models.Seeder
{
    public class DbSeeder
    {
        public static void Initialize(AppDbContext dbContext)
        {
            if (!dbContext.Categorias.Any())
            {
                var categoriaEletronicos = new Categoria { Id = Guid.NewGuid(), Nome = "Eletrônicos" };
                var categoriaRoupas = new Categoria { Id = Guid.NewGuid(), Nome = "Roupas" };

                dbContext.Categorias.Add(categoriaEletronicos);
                dbContext.Categorias.Add(categoriaRoupas);
                dbContext.SaveChanges();

                dbContext.Produtos.Add(new Produto
                {
                    Id = Guid.NewGuid(),
                    Nome = "Smartphone",
                    Preco = 1500.00m,
                    Estoque = 10,
                    CategoriaId = categoriaEletronicos.Id
                });

                dbContext.Produtos.Add(new Produto
                {
                    Id = Guid.NewGuid(),
                    Nome = "Notebook",
                    Preco = 3500.00m,
                    Estoque = 5,
                    CategoriaId = categoriaEletronicos.Id
                });

                dbContext.SaveChanges();
            }
        }
    }
}
