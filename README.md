
# CatalogAPI Documentation

## Version 1.0

### Base URL

`/api`

---

## Endpoints

### **Categorias**

#### `GET /api/Categorias`
Recupera todas as categorias.

**Responses:**
- `200 OK`: Retorna a lista de categorias.

#### `POST /api/Categorias`
Cria uma nova categoria.

**Request Body (JSON):**
```json
{
  "nome": "string"
}
```

**Responses:**
- `200 OK`: Categoria criada com sucesso.

#### `GET /api/Categorias/{id}`
Recupera uma categoria pelo ID.

**Parameters:**
- `id` (path, required): O ID da categoria (UUID).

**Responses:**
- `200 OK`: Retorna a categoria solicitada.

#### `PUT /api/Categorias/{id}`
Atualiza uma categoria pelo ID.

**Parameters:**
- `id` (path, required): O ID da categoria (UUID).

**Request Body (JSON):**
```json
{
  "nome": "string"
}
```

**Responses:**
- `200 OK`: Categoria atualizada com sucesso.

#### `DELETE /api/Categorias/{id}`
Deleta uma categoria pelo ID.

**Parameters:**
- `id` (path, required): O ID da categoria (UUID).

**Responses:**
- `200 OK`: Categoria deletada com sucesso.

#### `GET /api/Categorias/{id}/produtos`
Recupera todos os produtos de uma categoria pelo ID.

**Parameters:**
- `id` (path, required): O ID da categoria (UUID).

**Responses:**
- `200 OK`: Retorna a lista de produtos da categoria.

---

### **Produtos**

#### `GET /api/Produtos`
Recupera todos os produtos.

**Responses:**
- `200 OK`: Retorna a lista de produtos.

#### `POST /api/Produtos`
Cria um novo produto.

**Request Body (JSON):**
```json
{
  "nome": "string",
  "preco": 10.99,
  "estoque": 100,
  "categoriaId": "uuid"
}
```

**Responses:**
- `200 OK`: Produto criado com sucesso.

#### `GET /api/Produtos/{id}`
Recupera um produto pelo ID.

**Parameters:**
- `id` (path, required): O ID do produto (UUID).

**Responses:**
- `200 OK`: Retorna o produto solicitado.

#### `PUT /api/Produtos/{id}`
Atualiza um produto pelo ID.

**Parameters:**
- `id` (path, required): O ID do produto (UUID).

**Request Body (JSON):**
```json
{
  "nome": "string",
  "preco": 10.99,
  "estoque": 100,
  "categoriaId": "uuid"
}
```

**Responses:**
- `200 OK`: Produto atualizado com sucesso.

#### `DELETE /api/Produtos/{id}`
Deleta um produto pelo ID.

**Parameters:**
- `id` (path, required): O ID do produto (UUID).

**Responses:**
- `200 OK`: Produto deletado com sucesso.

#### `PATCH /api/Produtos/{id}/estoque`
Atualiza o estoque de um produto.

**Parameters:**
- `id` (path, required): O ID do produto (UUID).
- `novoEstoque` (query, required): Novo valor do estoque.
- `requisicaoEspecial` (query, optional): Flag para requisitar uma ação especial.

**Responses:**
- `200 OK`: Estoque atualizado com sucesso.

#### `GET /api/Produtos/{id}/categorias`
Recupera todas as categorias associadas a um produto.

**Parameters:**
- `id` (path, required): O ID do produto (UUID).

**Responses:**
- `200 OK`: Retorna a lista de categorias associadas ao produto.

---

## Components

### **Schemas**

#### PostCategoriaDTO
```json
{
  "nome": "string"
}
```

#### PostProdutoDTO
```json
{
  "nome": "string",
  "preco": 10.99,
  "estoque": 100,
  "categoriaId": "uuid"
}
```

---

## Authentication

Esta API não requer autenticação.

---

## Error Handling

Erros são retornados com um código de status apropriado. Exemplos:

- `400 Bad Request`: Se os dados enviados forem inválidos.
- `404 Not Found`: Se o recurso não for encontrado.
- `500 Internal Server Error`: Se ocorrer um erro no servidor.

---

## License

Esta API é de uso livre. Consulte os termos de uso no [GitHub](https://github.com/).
