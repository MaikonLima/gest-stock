namespace GerenciamentoEstoque.Services;

public enum ToastKind { Success, Error, Info }

public record ToastMessage(string Title, string Message, ToastKind Kind);

public static class NotificationService
{
    public static event Action<ToastMessage>? Requested;

    public static void Success(string message) => Requested?.Invoke(new ToastMessage("Sucesso", message, ToastKind.Success));
    public static void Error(string message) => Requested?.Invoke(new ToastMessage("Erro", message, ToastKind.Error));
    public static void Info(string message) => Requested?.Invoke(new ToastMessage("Atenção", message, ToastKind.Info));
}
