using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GerenciamentoEstoque.Models;
using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.ViewModels;

public partial class ProdutosViewModel : ViewModelBase
{
    private readonly ApiClient _api;

    public ObservableCollection<Produto> Items { get; } = [];
    public ObservableCollection<Categoria> Categorias { get; }
    public ObservableCollection<Fornecedor> Fornecedores { get; }

    [ObservableProperty] public partial string NovoNome { get; set; } = "";
    [ObservableProperty] public partial string NovoSku { get; set; } = "";
    [ObservableProperty] public partial string NovoDescricao { get; set; } = "";
    [ObservableProperty] public partial string NovoPreco { get; set; } = "";
    [ObservableProperty] public partial string NovaQuantidade { get; set; } = "0";
    [ObservableProperty] public partial Categoria? SelectedCategoria { get; set; }
    [ObservableProperty] public partial Fornecedor? SelectedFornecedor { get; set; }

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial Produto? SelectedItem { get; set; }

    private readonly IDialogService _dialogService;

    public ProdutosViewModel(
        ApiClient api,
        CategoriasViewModel categoriasViewModel,
        FornecedoresViewModel fornecedoresViewModel,
        IDialogService dialogService)
    {
        _api = api;
        _dialogService = dialogService;
        Categorias = categoriasViewModel.Items;
        Fornecedores = fornecedoresViewModel.Items;
    }

    [RelayCommand]
    private Task OpenNewAsync() => _dialogService.ShowNovoProdutoAsync(this);

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            var list = await _api.GetListAsync<Produto>("produtos");
            Items.Clear();
            foreach (var p in list) Items.Add(p);
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }

    [RelayCommand]
    public async Task<bool> AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NovoNome) || string.IsNullOrWhiteSpace(NovoSku))
        {
            NotificationService.Info("Informe nome e SKU do produto");
            return false;
        }
        if (!decimal.TryParse(NovoPreco, NumberStyles.Number, CultureInfo.InvariantCulture, out var preco) || preco <= 0)
        {
            NotificationService.Info("Informe um preço válido");
            return false;
        }
        int.TryParse(NovaQuantidade, out var quantidade);

        try
        {
            var request = new CreateProdutoRequest(
                NovoNome,
                NovoSku,
                string.IsNullOrWhiteSpace(NovoDescricao) ? null : NovoDescricao,
                preco,
                quantidade,
                SelectedCategoria?.Id,
                SelectedFornecedor?.Id);
            var created = await _api.PostAsync<Produto>("produtos", request);
            Items.Add(created);
            NotificationService.Success($"Produto \"{created.Nome}\" criado");
            NovoNome = ""; NovoSku = ""; NovoDescricao = ""; NovoPreco = ""; NovaQuantidade = "0";
            SelectedCategoria = null; SelectedFornecedor = null;
            return true;
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); return false; }
    }

    private bool CanDelete() => SelectedItem is not null;

    [RelayCommand(CanExecute = nameof(CanDelete))]
    private async Task DeleteAsync()
    {
        if (SelectedItem is null) return;
        try
        {
            await _api.DeleteAsync("produtos", SelectedItem.Id);
            Items.Remove(SelectedItem);
            NotificationService.Success("Produto excluído");
            SelectedItem = null;
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }
}
