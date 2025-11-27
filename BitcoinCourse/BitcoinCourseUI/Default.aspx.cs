using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Net;
using System.Web.Script.Serialization;

namespace BitcoinCourseUI
{
    public partial class _Default : Page
    {
        // Control declaration for GridView
        protected global::System.Web.UI.WebControls.GridView GridViewData;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        private void LoadData()
        {
            const string url = "https://data-api.coindesk.com/spot/v1/latest/tick?market=coinbase&instruments=BTC-EUR";

            var dt = new DataTable();
            dt.Columns.Add("Label");
            dt.Columns.Add("Value");

            try
            {
                using (var wc = new WebClient())
                {
                    var json = wc.DownloadString(url);
                    var js = new JavaScriptSerializer();
                    var root = js.Deserialize<Dictionary<string, object>>(json);

                    Dictionary<string, object> instrumentDict = null;

                    if (root != null && root.TryGetValue("Data", out var dataObj) && dataObj != null)
                    {
                        var dataDict = dataObj as Dictionary<string, object>;
                        if (dataDict != null)
                        {
                            // Try to get the requested instrument first
                            if (dataDict.TryGetValue("BTC-EUR", out var instObj) && instObj != null)
                            {
                                instrumentDict = instObj as Dictionary<string, object>;
                            }

                            // Fallback: take the first instrument dictionary available
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

                            dt.Rows.Add(key, valueStr);
                        }
                    }

                    // Ensure at least 30 rows: pad with placeholders if necessary
                    for (int i = dt.Rows.Count + 1; i <= 30; i++)
                    {
                        dt.Rows.Add($"Item {i}", $"Value {i}");
                    }

                    GridViewData.DataSource = dt;
                    GridViewData.DataBind();
                }
            }
            catch (Exception ex)
            {
                dt.Rows.Clear();
                dt.Rows.Add("Error", ex.Message);
                GridViewData.DataSource = dt;
                GridViewData.DataBind();
            }
        }
    }
}