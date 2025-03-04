using CatalogAPI.Exceptions;
using System.Net;

public interface IExceptionManager
{
    (HttpStatusCode statusCode, ErrorResponse response) HandleException(Exception exception);
}

public class ExceptionManager : IExceptionManager
{
    public (HttpStatusCode statusCode, ErrorResponse response) HandleException(Exception exception)
    {
        if (exception is ProdutoDuplicadoException)
        {
            return (HttpStatusCode.Conflict, new ErrorResponse("Produto duplicado.", 409, exception.Message));
        }
        else if (exception is ProdutoNaoEncontradoException)
        {
            return (HttpStatusCode.NotFound, new ErrorResponse("Produto não encontrado.", 404, exception.Message));
        }
        else if (exception is EstoqueInvalidoException)
        {
            return (HttpStatusCode.BadRequest, new ErrorResponse("Estoque inválido.", 400, exception.Message));
        }
        else if (exception is RemocaoProdutoComEstoqueException)
        {
            return (HttpStatusCode.BadRequest, new ErrorResponse("Remover produto com estoque maior que zero.", 400, exception.Message));
        }
        else if (exception is CategoriaDuplicadaException)
        {
            return (HttpStatusCode.BadRequest, new ErrorResponse("Categoria duplicada.", 400, exception.Message));
        }
        else if (exception is CategoriaNaoEncontradaException)
        {
            return (HttpStatusCode.NotFound, new ErrorResponse("Categoria não encontrada.", 404, exception.Message));
        }
        else
        {
            return (HttpStatusCode.InternalServerError, new ErrorResponse("Erro interno do servidor.", 500, exception.Message));
        }
    }
}