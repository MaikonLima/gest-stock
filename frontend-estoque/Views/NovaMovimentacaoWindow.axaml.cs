using Avalonia.Controls;
using Avalonia.Interactivity;
using GerenciamentoEstoque.ViewModels;

namespace GerenciamentoEstoque.Views;

public partial class NovaMovimentacaoWindow : Window
{
    public NovaMovimentacaoWindow()
    {
        InitializeComponent();
    }

    private void OnCancelClick(object? sender, RoutedEventArgs e) => Close(false);

    private async void OnSalvarClick(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MovimentacoesViewModel vm && await vm.AddAsync())
            Close(true);
    }
}
