using BlazorTableApp.Models;
using System.Text.Json;

namespace BlazorTableApp.Services;

public class DataService
{
    private List<ValueItem> _items = new();
    private readonly HttpClient _http;

    public DataService(HttpClient http)
    {
        _http = http;
    }

    public IReadOnlyList<ValueItem> GetAll() => _items;

    public async Task FetchFromCoinDeskAsync()
    {
        try
        {
            var url = "https://data-api.coindesk.com/spot/v1/latest/tick?market=coinbase&instruments=BTC-EUR";
            var response = await _http.GetAsync(url);
            if (!response.IsSuccessStatusCode)
            {
                _items = new List<ValueItem> { new() { Key = "Error", Value = "Failed to fetch data" } };
                return;
            }

            var json = await response.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            _items = new List<ValueItem>();

            // Navigate to Data -> BTC-EUR
            if (root.TryGetProperty("Data", out var dataElement) && dataElement.TryGetProperty("BTC-EUR", out var btcEurElement))
            {
                // Extract all key-value pairs from BTC-EUR object
                foreach (var prop in btcEurElement.EnumerateObject())
                {
                    _items.Add(new ValueItem
                    {
                        Key = prop.Name,
                        Value = FormatJsonValue(prop.Value)
                    });
                }
            }
            else
            {
                _items = new List<ValueItem> { new() { Key = "Error", Value = "BTC-EUR data not found in response" } };
            }
        }
        catch (Exception ex)
        {
            _items = new List<ValueItem> { new() { Key = "Error", Value = ex.Message } };
        }
    }

    private string FormatJsonValue(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.String => element.GetString() ?? string.Empty,
            JsonValueKind.Number => element.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            JsonValueKind.Null => "null",
            JsonValueKind.Object => "[Object]",
            JsonValueKind.Array => "[Array]",
            _ => element.GetRawText()
        };
    }
}
