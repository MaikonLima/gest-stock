using Avalonia.Controls;
using GerenciamentoEstoque.Services;
using GerenciamentoEstoque.ViewModels;

namespace GerenciamentoEstoque.Views;

// Resolves the owner window lazily: the MainWindow isn't assigned yet when
// MainViewModel (and its child ViewModels) are constructed in App.axaml.cs.
public class DialogService(Func<Window> ownerProvider) : IDialogService
{
    public Task<bool> ShowNovaCategoriaAsync(CategoriasViewModel viewModel) =>
        ShowAsync(new NovaCategoriaWindow { DataContext = viewModel });

    public Task<bool> ShowNovoFornecedorAsync(FornecedoresViewModel viewModel) =>
        ShowAsync(new NovoFornecedorWindow { DataContext = viewModel });

    public Task<bool> ShowNovoProdutoAsync(ProdutosViewModel viewModel) =>
        ShowAsync(new NovoProdutoWindow { DataContext = viewModel });

    public Task<bool> ShowNovaMovimentacaoAsync(MovimentacoesViewModel viewModel) =>
        ShowAsync(new NovaMovimentacaoWindow { DataContext = viewModel });

    private Task<bool> ShowAsync(Window dialog) => dialog.ShowDialog<bool>(ownerProvider());
}
