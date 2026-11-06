namespace GerenciamentoEstoque.Models;

public class Categoria
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class Fornecedor
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public string? Cnpj { get; set; }
    public string? Telefone { get; set; }
    public string? Email { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class Produto
{
    public string Id { get; set; } = "";
    public string Nome { get; set; } = "";
    public string Sku { get; set; } = "";
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int Quantidade { get; set; }
    public string? CategoriaId { get; set; }
    public string? FornecedorId { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

public class MovimentacaoEstoque
{
    public string Id { get; set; } = "";
    public string ProdutoId { get; set; } = "";
    public string Tipo { get; set; } = "ENTRADA";
    public int Quantidade { get; set; }
    public string? Motivo { get; set; }
    public DateTime CreatedAt { get; set; }
}

public record CreateCategoriaRequest(string Nome);

public record CreateFornecedorRequest(string Nome, string? Cnpj, string? Telefone, string? Email);

public record CreateProdutoRequest(
    string Nome,
    string Sku,
    string? Descricao,
    decimal Preco,
    int Quantidade,
    string? CategoriaId,
    string? FornecedorId);

public record CreateMovimentacaoRequest(string ProdutoId, string Tipo, int Quantidade, string? Motivo);
