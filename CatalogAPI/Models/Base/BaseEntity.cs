namespace CatalogAPI.Models.Base
{
    public class BaseEntity
    {
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}
