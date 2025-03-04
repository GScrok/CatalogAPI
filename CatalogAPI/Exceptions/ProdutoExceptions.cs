namespace CatalogAPI.Exceptions
{
    public class ProdutoDuplicadoException : Exception
    {
        public ProdutoDuplicadoException(string message) : base(message) { }
    }
    public class ProdutoNaoEncontradoException : Exception
    {
        public ProdutoNaoEncontradoException(string message) : base(message) { }
    }
    public class EstoqueInvalidoException : Exception
    {
        public EstoqueInvalidoException(string message) : base(message) { }
    }
    public class RemocaoProdutoComEstoqueException : Exception
    {
        public RemocaoProdutoComEstoqueException(string message) : base(message) { }
    }
}
