using Contracts;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.EnterpriseServices;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace BitcoinCourseUI.Services
{
    internal interface ICondeskService
    {
        Task<List<BtcRecord>> GetBtcRecordsAsync();
        Task SaveAsync();
        Task<LastSnapshotResponse> GetLastSnapshotAsync();
    }

    internal class CondeskService : ICondeskService
    {
        private const string LocalBase = "http://localhost:5041";

        public async Task<List<BtcRecord>> GetBtcRecordsAsync()
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

        public async Task SaveAsync()
        {
            var records = await GetBtcRecordsAsync();

            var apiBase = "http://localhost:5041"; // adjust port if API runs on different port
            using (var wc = new WebClient())
            {
                wc.Headers[HttpRequestHeader.ContentType] = "application/json";

                string jsonBody = JsonConvert.SerializeObject(records);

                var json = await wc.UploadStringTaskAsync(new Uri(apiBase + "/api/Snapshots/Save"), "POST", jsonBody);
                var js = new JavaScriptSerializer();
                var obj = js.Deserialize<Dictionary<string, object>>(json);
                var saved = obj != null && obj.ContainsKey("saved") ? obj["saved"].ToString() : "0";
            }
        }

        public async Task<LastSnapshotResponse> GetLastSnapshotAsync()
        {
            using (var wc = new WebClient())
            {
                var url = LocalBase + "/api/Snapshots/Last";
                var json = await wc.DownloadStringTaskAsync(new Uri(url));
                
                if (string.IsNullOrWhiteSpace(json) || json == "null")
                {
                    return null;
                }

                var js = new JavaScriptSerializer();
                var snapshot = js.Deserialize<LastSnapshotResponse>(json);
                return snapshot;
            }
        }
    }
}
