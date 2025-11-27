using Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BitcoinCourseAPI.Services
{

    public interface IBtcDataService
    {
        Task<List<BtcRecord>> GetBtcData();
    }

    public class BtcDataService : IBtcDataService
    {
        private const string Url = "https://data-api.coindesk.com/spot/v1/latest/tick?market=coinbase&instruments=BTC-EUR";
        private readonly HashSet<string> PriceLikeFields = new HashSet<string>() { "PRICE" };

        private readonly HttpClient _httpClient;
        private readonly ICnbConversionService _cnbConversionService;

        public BtcDataService(HttpClient httpClient, ICnbConversionService cnbConversionService)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            _cnbConversionService = cnbConversionService;
        }

        public async Task<List<BtcRecord>> GetBtcData()
        {
            var records = new List<BtcRecord>();

            decimal eurToCzkRate = await _cnbConversionService.GetEurToCzkRate();

            try
            {
                var json = await _httpClient.GetStringAsync(Url).ConfigureAwait(false);

                using var doc = JsonDocument.Parse(json);
                var root = doc.RootElement;

                if (root.TryGetProperty("Data", out var dataElement) && dataElement.ValueKind == JsonValueKind.Object)
                {
                    JsonElement instrumentElement = default;

                    // Prefer explicit instrument key if present
                    if (dataElement.TryGetProperty("BTC-EUR", out var btcEur))
                    {
                        instrumentElement = btcEur;
                    }
                    else
                    {
                        // fallback to first instrument object
                        var first = dataElement.EnumerateObject().FirstOrDefault();
                        if (first.Value.ValueKind == JsonValueKind.Object)
                        {
                            instrumentElement = first.Value;
                        }
                    }

                    if (instrumentElement.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var prop in instrumentElement.EnumerateObject())
                        {
                            string valueStr;

                            if (PriceLikeFields.Contains(prop.Name))
                            {
                                valueStr = (prop.Value.GetDecimal() * eurToCzkRate).ToString();
                            }
                            else
                                switch (prop.Value.ValueKind)
                                {
                                    case JsonValueKind.String:
                                        valueStr = prop.Value.GetString() ?? string.Empty;
                                        break;
                                    case JsonValueKind.Number:
                                        valueStr = prop.Value.GetRawText();
                                        break;
                                    case JsonValueKind.True:
                                    case JsonValueKind.False:
                                        valueStr = prop.Value.GetRawText();
                                        break;
                                    case JsonValueKind.Null:
                                        valueStr = string.Empty;
                                        break;
                                    default:
                                        // For arrays/objects preserve raw JSON
                                        valueStr = prop.Value.GetRawText();
                                        break;
                                }

                            records.Add(new BtcRecord
                            {
                                FieldName = prop.Name,
                                Value = valueStr
                            });
                        }
                    }
                }
            }
            catch (Exception)
            {
                // For now swallow exceptions and return an empty list; add logging as needed
            }

            return records;
        }
    }
}
