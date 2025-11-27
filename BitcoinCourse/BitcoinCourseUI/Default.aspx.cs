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
using BitcoinCourseUI.Services;
using System.Threading.Tasks;

namespace BitcoinCourseUI
{
    public partial class _Default : Page
    {
        private readonly ICondeskService _condeskService = new CondeskService(); // TODO use DI

        // Control declaration for GridView
        protected global::System.Web.UI.WebControls.GridView GridViewData;
        protected global::System.Web.UI.WebControls.Button SaveSnapshotButton;
        protected global::System.Web.UI.WebControls.Label SnapshotStatus;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(LoadData));
            }
        }

        private async Task LoadData()
        {
            var dt = new DataTable();
            dt.Columns.Add("Label");
            dt.Columns.Add("Value");

            var condeskData = await _condeskService.GetBtcRecordsAsync();
            foreach (var condeskItem in condeskData)
            {
                dt.Rows.Add(condeskItem.FieldName, condeskItem.Value);
            }

            GridViewData.DataSource = dt;
            GridViewData.DataBind();
        }

        protected void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Run the same async load on timer tick
            RegisterAsyncTask(new PageAsyncTask(LoadData));
        }

        protected async void SaveSnapshotButton_Click(object sender, EventArgs e)
        {
            try
            {
                SnapshotStatus.Text = "Saving...";
                var apiBase = "http://localhost:5041"; // adjust port if API runs on different port
                using (var wc = new WebClient())
                {
                    wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                    // POST with empty body
                    var json = await wc.UploadStringTaskAsync(new Uri(apiBase + "/api/Snapshots/Save"), "POST", string.Empty);
                    var js = new JavaScriptSerializer();
                    var obj = js.Deserialize<Dictionary<string, object>>(json);
                    var saved = obj != null && obj.ContainsKey("saved") ? obj["saved"].ToString() : "0";
                    SnapshotStatus.Text = $"Saved {saved} records";
                }
            }
            catch (Exception ex)
            {
                SnapshotStatus.Text = "Error: " + ex.Message;
            }
        }
    }
}