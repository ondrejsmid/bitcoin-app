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
    }
}