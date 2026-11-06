using Avalonia.Controls;
using Avalonia.Interactivity;
using GerenciamentoEstoque.ViewModels;

namespace GerenciamentoEstoque.Views;

public partial class NovoFornecedorWindow : Window
{
    public NovoFornecedorWindow()
    {
        InitializeComponent();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(false);

    private async void OnSalvarClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is FornecedoresViewModel vm && await vm.AddAsync())
            Close(true);
    }
}
