using Avalonia.Controls;
using Avalonia.Controls.Notifications;
using GerenciamentoEstoque.Services;

namespace GerenciamentoEstoque.Views;

public partial class MainWindow : Window
{
    private readonly WindowNotificationManager _notifications;

    public MainWindow()
    {
        InitializeComponent();

        _notifications = new WindowNotificationManager(this) { Position = NotificationPosition.TopRight, MaxItems = 3 };
        NotificationService.Requested += OnNotificationRequested;
        Closed += (_, _) => NotificationService.Requested -= OnNotificationRequested;
    }

    private void OnNotificationRequested(ToastMessage toast)
    {
        var type = toast.Kind switch
        {
            ToastKind.Success => NotificationType.Success,
            ToastKind.Error => NotificationType.Error,
            _ => NotificationType.Information,
        };
        _notifications.Show(new Notification(toast.Title, toast.Message, type));
    }
}
