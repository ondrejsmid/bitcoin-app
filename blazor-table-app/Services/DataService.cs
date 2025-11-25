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

            // Navigate to Data -> BTC-EUR: simple single-pass collection
            if (root.TryGetProperty("Data", out var dataElement) && dataElement.TryGetProperty("BTC-EUR", out var btcEurElement))
            {
                var keys = new[] { "PRICE", "LAST_PROCESSED_TRADE_PRICE", "BEST_BID", "BEST_ASK" };
                var keySet = new HashSet<string>(keys, StringComparer.OrdinalIgnoreCase);

                // Single pass: keep original property order. For predefined keys, multiply numeric values by CNB rate.
                var rate = await GetEurToCzkRateAsync();
                var multiplier = rate ?? 1000m;
                foreach (var prop in btcEurElement.EnumerateObject())
                {
                    var name = prop.Name;
                    var elem = prop.Value;

                        if (keySet.Contains(name))
                        {
                        // multiplier is already resolved
                            string valueStr = string.Empty;
                            if (decimal.TryParse(elem.GetRawText(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var n))
                                valueStr = (n * multiplier).ToString("0.##", System.Globalization.CultureInfo.InvariantCulture);

                            _items.Add(new ValueItem { Key = name, Value = valueStr });
                        }
                    else
                    {
                        Console.WriteLine($"{name} is NOT price-like");
                        _items.Add(new ValueItem { Key = name, Value = FormatJsonValue(elem) });
                    }
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

    private async Task<decimal?> GetEurToCzkRateAsync()
    {
        try
        {
            // Call local API endpoint (API service handles CNB fetch)
            var url = "http://localhost:5000/api/exchangerate/eur-czk";
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var txt = await resp.Content.ReadAsStringAsync();
            using var doc = JsonDocument.Parse(txt);
            var root = doc.RootElement;
            if (root.TryGetProperty("rate", out var rateElem))
            {
                if (rateElem.ValueKind == JsonValueKind.Number && rateElem.TryGetDecimal(out var d))
                    return d;
            }
        }
        catch
        {
            // ignore
        }

        return null;
    }
}
