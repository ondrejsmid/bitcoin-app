using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Text;
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
        public async Task<List<BtcRecord>> GetBtcRecordsAsync()
        {
            const string url = "https://data-api.coindesk.com/spot/v1/latest/tick?market=coinbase&instruments=BTC-EUR";

            List<BtcRecord> records = new List<BtcRecord>();
            try
            {
                using (var wc = new WebClient())
                {
                    var json = await wc.DownloadStringTaskAsync(new Uri(url));
                    var js = new JavaScriptSerializer();
                    var root = js.Deserialize<Dictionary<string, object>>(json);

                    Dictionary<string, object> instrumentDict = null;

                    if (root != null && root.TryGetValue("Data", out var dataObj) && dataObj != null)
                    {
                        var dataDict = dataObj as Dictionary<string, object>;
                        if (dataDict != null)
                        {
                            if (dataDict.TryGetValue("BTC-EUR", out var instObj) && instObj != null)
                            {
                                instrumentDict = instObj as Dictionary<string, object>;
                            }

                            if (instrumentDict == null)
                            {
                                instrumentDict = dataDict.Values.OfType<Dictionary<string, object>>().FirstOrDefault();
                            }
                        }
                    }

                    if (instrumentDict != null)
                    {
                        // Add each property as a row: KEY | VALUE
                        foreach (var kvp in instrumentDict)
                        {
                            string key = kvp.Key;
                            string valueStr;

                            if (kvp.Value == null)
                            {
                                valueStr = string.Empty;
                            }
                            else if (kvp.Value is Dictionary<string, object> || kvp.Value is ArrayList)
                            {
                                // Serialize complex nested values back to JSON string for display
                                valueStr = js.Serialize(kvp.Value);
                            }
                            else
                            {
                                valueStr = kvp.Value.ToString();
                            }

                            records.Add(new BtcRecord
                            {
                                FieldName = key,
                                Value = valueStr
                            });
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //dt.Rows.Add("Error", ex.Message);
                // TODO logging
            }

            return records;
        }
    }

    internal class BtcRecord
    {
        public string FieldName;
        public string Value;
    }
}
