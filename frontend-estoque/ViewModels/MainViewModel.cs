using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    public CategoriasViewModel CategoriasViewModel { get; }
    public FornecedoresViewModel FornecedoresViewModel { get; }
    public ProdutosViewModel ProdutosViewModel { get; }
    public MovimentacoesViewModel MovimentacoesViewModel { get; }

    // Used only by the XAML previewer's Design.DataContext, which never opens real dialogs.
    public MainViewModel() : this(new ApiClient(), new DesignTimeDialogService())
    {
    }

    public MainViewModel(ApiClient api, IDialogService dialogService)
    {
        CategoriasViewModel = new CategoriasViewModel(api, dialogService);
        FornecedoresViewModel = new FornecedoresViewModel(api, dialogService);
        ProdutosViewModel = new ProdutosViewModel(api, CategoriasViewModel, FornecedoresViewModel, dialogService);
        MovimentacoesViewModel = new MovimentacoesViewModel(api, ProdutosViewModel, dialogService);

        _ = InitializeAsync();
    }

    private async Task InitializeAsync()
    {
        await CategoriasViewModel.LoadAsync();
        await FornecedoresViewModel.LoadAsync();
        await ProdutosViewModel.LoadAsync();
        await MovimentacoesViewModel.LoadAsync();
    }

    private sealed class DesignTimeDialogService : IDialogService
    {
        public Task<bool> ShowNovaCategoriaAsync(CategoriasViewModel viewModel) => Task.FromResult(false);
        public Task<bool> ShowNovoFornecedorAsync(FornecedoresViewModel viewModel) => Task.FromResult(false);
        public Task<bool> ShowNovoProdutoAsync(ProdutosViewModel viewModel) => Task.FromResult(false);
        public Task<bool> ShowNovaMovimentacaoAsync(MovimentacoesViewModel viewModel) => Task.FromResult(false);
    }
}
