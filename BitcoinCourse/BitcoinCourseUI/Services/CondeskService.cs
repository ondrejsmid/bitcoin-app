using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BitcoinCourseUI.Services
{
    internal interface ICondeskService
    {
        Task<List<BtcRecord>> GetBtcRecordsAsync();
    }

    internal class CondeskService : ICondeskService
    {
        private const string LocalBase = "http://localhost:5041"; // adjust port if your API uses another one

        public async Task<List<BtcRecord>> GetBtcRecordsAsync()
        {
            // Only use local backend API
            var records = new List<BtcRecord>();

            try
            {
                var local = await GetBtcDataFromLocalApi();
                if (local != null && local.Any())
                {
                    records.AddRange(local);
                }
            }
            catch
            {
                // If local API fails, return empty list (or consider rethrowing/logging)
                return new List<BtcRecord>();
            }

            // Append EUR->CZK rate from local backend if available (optional)
            try
            {
                var rate = await EurToCzkRate();
                records.Add(new BtcRecord { FieldName = "EUR_TO_CZK", Value = rate.ToString(System.Globalization.CultureInfo.InvariantCulture) });
            }
            catch
            {
                // ignore rate retrieval failures
            }

            return records;
        }

        private async Task<List<BtcRecord>> GetBtcDataFromLocalApi()
        {
            using (var wc = new WebClient())
            {
                var url = LocalBase + "/api/BtcData";
                var json = await wc.DownloadStringTaskAsync(new Uri(url));
                var js = new JavaScriptSerializer();
                var list = js.Deserialize<List<BtcRecord>>(json);
                return list ?? new List<BtcRecord>();
            }
        }

        private async Task<decimal> EurToCzkRate()
        {
            using (var wc = new WebClient())
            {
                var url = LocalBase + "/api/EurToCzk";
                var json = await wc.DownloadStringTaskAsync(new Uri(url));
                var js = new JavaScriptSerializer();
                var dict = js.Deserialize<Dictionary<string, object>>(json);
                if (dict != null && dict.TryGetValue("rate", out var rateObj))
                {
                    if (decimal.TryParse(rateObj.ToString(), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out var rate))
                    {
                        return rate;
                    }
                }
            }

            throw new InvalidOperationException("Failed to get response from EurToCzk endpoint.");
        }
    }

    internal class BtcRecord
    {
        public string FieldName { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }
}
