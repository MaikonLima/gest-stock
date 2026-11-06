using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GerenciamentoEstoque.Models;
using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.ViewModels;

public partial class FornecedoresViewModel(ApiClient api, IDialogService dialogService) : ViewModelBase
{
    public ObservableCollection<Fornecedor> Items { get; } = [];

    [ObservableProperty] public partial string NovoNome { get; set; } = "";
    [ObservableProperty] public partial string NovoCnpj { get; set; } = "";
    [ObservableProperty] public partial string NovoTelefone { get; set; } = "";
    [ObservableProperty] public partial string NovoEmail { get; set; } = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial Fornecedor? SelectedItem { get; set; }

    [RelayCommand]
    private Task OpenNewAsync() => dialogService.ShowNovoFornecedorAsync(this);

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            var list = await api.GetListAsync<Fornecedor>("fornecedores");
            Items.Clear();
            foreach (var f in list) Items.Add(f);
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }

    [RelayCommand]
    public async Task<bool> AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NovoNome))
        {
            NotificationService.Info("Informe o nome do fornecedor");
            return false;
        }
        try
        {
            var request = new CreateFornecedorRequest(
                NovoNome,
                string.IsNullOrWhiteSpace(NovoCnpj) ? null : NovoCnpj,
                string.IsNullOrWhiteSpace(NovoTelefone) ? null : NovoTelefone,
                string.IsNullOrWhiteSpace(NovoEmail) ? null : NovoEmail);
            var created = await api.PostAsync<Fornecedor>("fornecedores", request);
            Items.Add(created);
            NotificationService.Success($"Fornecedor \"{created.Nome}\" criado");
            NovoNome = ""; NovoCnpj = ""; NovoTelefone = ""; NovoEmail = "";
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
            await api.DeleteAsync("fornecedores", SelectedItem.Id);
            Items.Remove(SelectedItem);
            NotificationService.Success("Fornecedor excluído");
            SelectedItem = null;
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }
}
