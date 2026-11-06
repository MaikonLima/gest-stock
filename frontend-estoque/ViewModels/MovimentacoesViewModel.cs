using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GerenciamentoEstoque.Models;
using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.ViewModels;

public partial class MovimentacoesViewModel(ApiClient api, ProdutosViewModel produtosViewModel, IDialogService dialogService) : ViewModelBase
{
    public ObservableCollection<MovimentacaoEstoque> Items { get; } = [];
    public ObservableCollection<Produto> Produtos => produtosViewModel.Items;
    public string[] Tipos { get; } = ["ENTRADA", "SAIDA"];

    [ObservableProperty] public partial Produto? SelectedProduto { get; set; }
    [ObservableProperty] public partial string SelectedTipo { get; set; } = "ENTRADA";
    [ObservableProperty] public partial string NovaQuantidade { get; set; } = "1";
    [ObservableProperty] public partial string NovoMotivo { get; set; } = "";

    [RelayCommand]
    private Task OpenNewAsync() => dialogService.ShowNovaMovimentacaoAsync(this);

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            var list = await api.GetListAsync<MovimentacaoEstoque>("movimentacoes");
            Items.Clear();
            foreach (var m in list) Items.Add(m);
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }

    [RelayCommand]
    public async Task<bool> AddAsync()
    {
        if (SelectedProduto is null)
        {
            NotificationService.Info("Selecione um produto");
            return false;
        }
        if (!int.TryParse(NovaQuantidade, out var quantidade) || quantidade <= 0)
        {
            NotificationService.Info("Informe uma quantidade válida");
            return false;
        }

        try
        {
            var request = new CreateMovimentacaoRequest(
                SelectedProduto.Id,
                SelectedTipo,
                quantidade,
                string.IsNullOrWhiteSpace(NovoMotivo) ? null : NovoMotivo);
            var created = await api.PostAsync<MovimentacaoEstoque>("movimentacoes", request);
            Items.Insert(0, created);
            NotificationService.Success($"Movimentação de {SelectedTipo.ToLowerInvariant()} registrada");
            NovaQuantidade = "1";
            NovoMotivo = "";
            SelectedProduto = null;
            await produtosViewModel.LoadAsync();
            return true;
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); return false; }
    }
}
