using GerenciamentoEstoque.ViewModels;

namespace GerenciamentoEstoque.Services;

public interface IDialogService
{
    Task<bool> ShowNovaCategoriaAsync(CategoriasViewModel viewModel);
    Task<bool> ShowNovoFornecedorAsync(FornecedoresViewModel viewModel);
    Task<bool> ShowNovoProdutoAsync(ProdutosViewModel viewModel);
    Task<bool> ShowNovaMovimentacaoAsync(MovimentacoesViewModel viewModel);
}
