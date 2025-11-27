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
        private int CurrentSnapshotId
        {
            get { return ViewState["CurrentSnapshotId"] != null ? (int)ViewState["CurrentSnapshotId"] : 0; }
            set { ViewState["CurrentSnapshotId"] = value; }
        }

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
            CurrentSnapshotId = snapshotId;
            var snapshot = await _condeskService.GetSnapshotByIdAsync(snapshotId);

            if (snapshot == null)
            {
                SnapshotDetailPanel.Visible = false;
                return;
            }

            SnapshotDetailPanel.Visible = true;
            EditNotePanel.Visible = false;
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

        protected void EditNoteButton_Click(object sender, EventArgs e)
        {
            EditNoteTextBox.Text = SnapshotNoteLabel.Text == "(no note)" ? "" : SnapshotNoteLabel.Text;
            EditNotePanel.Visible = true;
            EditStatusLabel.Text = "";
        }

        protected async void SaveNoteButton_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                return;
            }

            var newNote = EditNoteTextBox.Text.Trim();
            var success = await _condeskService.UpdateSnapshotNoteAsync(CurrentSnapshotId, newNote);

            if (success)
            {
                SnapshotNoteLabel.Text = newNote;
                EditNotePanel.Visible = false;
                EditStatusLabel.Text = "";
                
                // Refresh the snapshots list to show updated note
                await LoadSnapshots();
                // Reselect the current snapshot in the grid
                for (int i = 0; i < GridViewSnapshots.Rows.Count; i++)
                {
                    if ((int)GridViewSnapshots.DataKeys[i].Value == CurrentSnapshotId)
                    {
                        GridViewSnapshots.SelectedIndex = i;
                        break;
                    }
                }
            }
            else
            {
                EditStatusLabel.Text = "Failed to update note";
                EditStatusLabel.CssClass = "ms-2 text-danger";
            }
        }

        protected void CancelEditButton_Click(object sender, EventArgs e)
        {
            EditNotePanel.Visible = false;
            EditStatusLabel.Text = "";
        }
    }
}