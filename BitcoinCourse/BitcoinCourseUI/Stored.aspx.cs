using BitcoinCourseUI.Services;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace BitcoinCourseUI
{
    public partial class Stored : Page
    {
        private readonly ICondeskService _condeskService = new CondeskService();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                RegisterAsyncTask(new PageAsyncTask(LoadSnapshots));
            }
        }

        private async Task LoadSnapshots()
        {
            var snapshots = await _condeskService.GetAllSnapshotsAsync();

            if (snapshots == null || snapshots.Count == 0)
            {
                NoSnapshotMessage.Visible = true;
                SnapshotContentPanel.Visible = false;
                return;
            }

            NoSnapshotMessage.Visible = false;
            SnapshotContentPanel.Visible = true;

            GridViewSnapshots.DataSource = snapshots;
            GridViewSnapshots.DataBind();
        }

        protected async void GridViewSnapshots_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (GridViewSnapshots.SelectedDataKey != null)
            {
                int snapshotId = (int)GridViewSnapshots.SelectedDataKey.Value;
                await LoadSnapshotDetails(snapshotId);
            }
        }

        private async Task LoadSnapshotDetails(int snapshotId)
        {
            var snapshot = await _condeskService.GetSnapshotByIdAsync(snapshotId);

            if (snapshot == null)
            {
                SnapshotDetailPanel.Visible = false;
                return;
            }

            SnapshotDetailPanel.Visible = true;
            SnapshotNoteLabel.Text = string.IsNullOrEmpty(snapshot.Note) ? "(no note)" : snapshot.Note;

            // Bind data to GridView
            var dt = new DataTable();
            dt.Columns.Add("Label");
            dt.Columns.Add("Value");

            foreach (var item in snapshot.Data)
            {
                dt.Rows.Add(item.FieldName, item.Value);
            }

            GridViewStored.DataSource = dt;
            GridViewStored.DataBind();
        }
    }
}