namespace CatalogAPI.Exceptions
{
    public class CategoriaNaoEncontradaException : Exception
    {
        public CategoriaNaoEncontradaException(string message) : base(message) { }
    }
    public class CategoriaDuplicadaException : Exception
    {
        public CategoriaDuplicadaException(string message) : base(message) { }
    }
}
