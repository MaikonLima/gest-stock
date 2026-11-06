using Avalonia.Controls;
using Avalonia.Interactivity;
using GerenciamentoEstoque.ViewModels;

namespace GerenciamentoEstoque.Views;

public partial class NovaCategoriaWindow : Window
{
    public NovaCategoriaWindow()
    {
        InitializeComponent();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(false);

    private async void OnSalvarClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is CategoriasViewModel vm && await vm.AddAsync())
            Close(true);
    }
}
