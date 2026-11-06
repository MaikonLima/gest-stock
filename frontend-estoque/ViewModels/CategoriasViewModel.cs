using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GerenciamentoEstoque.Models;
using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.ViewModels;

public partial class CategoriasViewModel(ApiClient api, IDialogService dialogService) : ViewModelBase
{
    public ObservableCollection<Categoria> Items { get; } = [];

    [ObservableProperty] public partial string NovoNome { get; set; } = "";

    [ObservableProperty]
    [NotifyCanExecuteChangedFor(nameof(DeleteCommand))]
    public partial Categoria? SelectedItem { get; set; }

    [RelayCommand]
    private Task OpenNewAsync() => dialogService.ShowNovaCategoriaAsync(this);

    [RelayCommand]
    public async Task LoadAsync()
    {
        try
        {
            var list = await api.GetListAsync<Categoria>("categorias");
            Items.Clear();
            foreach (var c in list) Items.Add(c);
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }

    [RelayCommand]
    public async Task<bool> AddAsync()
    {
        if (string.IsNullOrWhiteSpace(NovoNome))
        {
            NotificationService.Info("Informe o nome da categoria");
            return false;
        }
        try
        {
            var created = await api.PostAsync<Categoria>("categorias", new CreateCategoriaRequest(NovoNome));
            Items.Add(created);
            NotificationService.Success($"Categoria \"{created.Nome}\" criada");
            NovoNome = "";
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
            await api.DeleteAsync("categorias", SelectedItem.Id);
            Items.Remove(SelectedItem);
            NotificationService.Success("Categoria excluída");
            SelectedItem = null;
        }
        catch (Exception ex) { NotificationService.Error(ex.Message); }
    }
}
