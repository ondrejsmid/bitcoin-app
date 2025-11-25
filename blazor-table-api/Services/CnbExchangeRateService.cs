namespace BlazorTableApi.Services;

public class CnbExchangeRateService
{
    private readonly HttpClient _http;

    public CnbExchangeRateService(HttpClient http)
    {
        _http = http;
    }

    public async Task<decimal?> GetEurToCzkRateAsync()
    {
        try
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var url = $"https://api.cnb.cz/cnbapi/exrates/daily?date={today}&lang=EN";
            var resp = await _http.GetAsync(url);
            if (!resp.IsSuccessStatusCode) return null;

            var txt = await resp.Content.ReadAsStringAsync();
            using var doc = System.Text.Json.JsonDocument.Parse(txt);
            var root = doc.RootElement;
            if (root.TryGetProperty("rates", out var rates) && rates.ValueKind == System.Text.Json.JsonValueKind.Array)
            {
                foreach (var r in rates.EnumerateArray())
                {
                    if (r.TryGetProperty("currencyCode", out var cc) && string.Equals(cc.GetString(), "EUR", StringComparison.OrdinalIgnoreCase))
                    {
                        if (r.TryGetProperty("rate", out var rateElem))
                        {
                            if (rateElem.ValueKind == System.Text.Json.JsonValueKind.Number && rateElem.TryGetDecimal(out var d))
                                return d;
                        }
                    }
                }
            }
        }
        catch
        {
            // ignore
        }

        return null;
    }
}
