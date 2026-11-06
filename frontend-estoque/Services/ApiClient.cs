using System.Globalization;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace GerenciamentoEstoque.Services;

public class ApiException(string message) : Exception(message);

// Prisma serializes Decimal fields (e.g. Produto.Preco) as JSON strings.
public class FlexibleDecimalConverter : JsonConverter<decimal>
{
    public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType == JsonTokenType.String
            ? decimal.Parse(reader.GetString()!, CultureInfo.InvariantCulture)
            : reader.GetDecimal();

    public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options) =>
        writer.WriteNumberValue(value);
}

public class ApiClient(string baseUrl = "http://localhost:3000/")
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new FlexibleDecimalConverter() },
    };

    private readonly HttpClient _http = new() { BaseAddress = new Uri(baseUrl) };

    public Task<List<T>> GetListAsync<T>(string resource) => SendAsync<List<T>>(HttpMethod.Get, resource);

    public Task<T> PostAsync<T>(string resource, object body) => SendAsync<T>(HttpMethod.Post, resource, body);

    public Task<T> PatchAsync<T>(string resource, string id, object body) =>
        SendAsync<T>(HttpMethod.Patch, $"{resource}/{id}", body);

    public Task DeleteAsync(string resource, string id) => SendAsync<object?>(HttpMethod.Delete, $"{resource}/{id}");

    private async Task<T> SendAsync<T>(HttpMethod method, string path, object? body = null)
    {
        using var request = new HttpRequestMessage(method, path);
        if (body is not null)
            request.Content = JsonContent.Create(body, options: JsonOptions);

        using var response = await _http.SendAsync(request);
        // Read raw bytes instead of ReadAsStringAsync: JSON is always UTF-8 per spec, and
        // decoding via the response's charset header (or lack of one) can mangle PT-BR accents.
        var bytes = await response.Content.ReadAsByteArrayAsync();

        if (!response.IsSuccessStatusCode)
            throw new ApiException(ExtractErrorMessage(bytes, response.StatusCode));

        return bytes.Length == 0 ? default! : JsonSerializer.Deserialize<T>(bytes, JsonOptions)!;
    }

    private static string ExtractErrorMessage(byte[] body, System.Net.HttpStatusCode status)
    {
        try
        {
            var message = JsonDocument.Parse(body).RootElement.GetProperty("message");
            return message.ValueKind == JsonValueKind.Array
                ? string.Join("; ", message.EnumerateArray().Select(e => e.ToString()))
                : message.ToString();
        }
        catch
        {
            return status.ToString();
        }
    }
}
