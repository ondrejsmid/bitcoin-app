using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace BitcoinCourseAPI.Services
{
    public interface ICnbConversionService
    {
        Task<decimal> GetEurToCzkRate();
    }

    public class CnbConversionService : ICnbConversionService
    {
        public async Task<decimal> GetEurToCzkRate()
        {
            var today = DateTime.UtcNow.ToString("yyyy-MM-dd");
            var url = $"https://api.cnb.cz/cnbapi/exrates/daily?date={today}&lang=EN";

            using var http = new HttpClient();
            using var resp = await http.GetAsync(url);
            if (!resp.IsSuccessStatusCode)
            {
                throw new InvalidOperationException("Unsuccessful result from cnbapi");
            }

            await using var stream = await resp.Content.ReadAsStreamAsync();
            using var doc = await JsonDocument.ParseAsync(stream);

            if (doc.RootElement.TryGetProperty("rates", out var rates) && rates.ValueKind == JsonValueKind.Array)
            {
                foreach (var item in rates.EnumerateArray())
                {
                    if (item.TryGetProperty("currencyCode", out var code) && string.Equals(code.GetString(), "EUR", StringComparison.OrdinalIgnoreCase))
                    {
                        if (item.TryGetProperty("rate", out var rateEl) && rateEl.ValueKind == JsonValueKind.Number)
                        {
                            var rate = rateEl.GetDecimal();
                            return rate;
                        }
                    }
                }
            }

            throw new InvalidOperationException("EUR rate not found");
        }
    }
}
