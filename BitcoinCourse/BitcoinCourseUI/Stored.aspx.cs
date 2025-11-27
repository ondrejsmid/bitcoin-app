using BitcoinCourseUI.Services;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.UI;

namespace BitcoinCourseUI
{
    public partial class Stored : Page
    {
        private readonly ICondeskService _condeskService = new CondeskService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(LoadLastSnapshot));
            }
        }

        private async Task LoadLastSnapshot()
        {
            var lastSnapshot = await _condeskService.GetLastSnapshotAsync();

            if (lastSnapshot == null)
            {
                NoSnapshotMessage.Visible = true;
                SnapshotInfoPanel.Visible = false;
                GridViewStored.Visible = false;
                return;
            }

            // Display snapshot info
            NoSnapshotMessage.Visible = false;
            SnapshotInfoPanel.Visible = true;
            SnapshotNoteLabel.Text = string.IsNullOrEmpty(lastSnapshot.Note) ? "(no note)" : lastSnapshot.Note;

            // Bind data to GridView
            var dt = new DataTable();
            dt.Columns.Add("Label");
            dt.Columns.Add("Value");

            foreach (var item in lastSnapshot.Data)
            {
                dt.Rows.Add(item.FieldName, item.Value);
            }

            GridViewStored.DataSource = dt;
            GridViewStored.DataBind();
            GridViewStored.Visible = true;
        }
    }
}