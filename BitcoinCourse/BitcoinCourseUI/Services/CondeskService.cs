using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using System.Web.Script.Serialization;
using Contracts;

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
            using (var wc = new WebClient())
            {
                var url = LocalBase + "/api/BtcData";
                var json = await wc.DownloadStringTaskAsync(new Uri(url));
                var js = new JavaScriptSerializer();
                var list = js.Deserialize<List<BtcRecord>>(json);
                return list ?? new List<BtcRecord>();
            }
        }
    }
}
