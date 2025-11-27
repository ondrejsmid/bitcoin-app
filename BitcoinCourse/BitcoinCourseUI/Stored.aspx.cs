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

        private string SnapshotSortExpression
        {
            get { return ViewState["SnapshotSortExpression"] as string ?? "Id"; }
            set { ViewState["SnapshotSortExpression"] = value; }
        }

        private string SnapshotSortDirection
        {
            get { return ViewState["SnapshotSortDirection"] as string ?? "DESC"; }
            set { ViewState["SnapshotSortDirection"] = value; }
        }

        private string DataSortExpression
        {
            get { return ViewState["DataSortExpression"] as string ?? "Label"; }
            set { ViewState["DataSortExpression"] = value; }
        }

        private string DataSortDirection
        {
            get { return ViewState["DataSortDirection"] as string ?? "ASC"; }
            set { ViewState["DataSortDirection"] = value; }
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

            var dt = new DataTable();
            dt.Columns.Add("Id", typeof(int));
            dt.Columns.Add("Note", typeof(string));

            foreach (var snapshot in snapshots)
            {
                dt.Rows.Add(snapshot.Id, snapshot.Note ?? string.Empty);
            }

            // Apply filter
            var filterText = SnapshotFilterTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(filterText))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = $"Note LIKE '%{filterText.Replace("'", "''")}%'";
                dt = dv.ToTable();
            }

            // Apply sorting
            var dv2 = dt.DefaultView;
            dv2.Sort = $"{SnapshotSortExpression} {SnapshotSortDirection}";

            GridViewSnapshots.DataSource = dv2;
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

            // Apply filter
            var filterText = DataFilterTextBox.Text.Trim();
            if (!string.IsNullOrEmpty(filterText))
            {
                var dv = dt.DefaultView;
                dv.RowFilter = $"Label LIKE '%{filterText.Replace("'", "''")}%' OR Value LIKE '%{filterText.Replace("'", "''")}%'";
                dt = dv.ToTable();
            }

            // Apply sorting
            var dv2 = dt.DefaultView;
            dv2.Sort = $"{DataSortExpression} {DataSortDirection}";

            GridViewStored.DataSource = dv2;
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

        protected void GridViewSnapshots_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (SnapshotSortExpression == e.SortExpression)
            {
                SnapshotSortDirection = SnapshotSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                SnapshotSortExpression = e.SortExpression;
                SnapshotSortDirection = "ASC";
            }
            RegisterAsyncTask(new PageAsyncTask(LoadSnapshots));
        }

        protected void GridViewStored_Sorting(object sender, System.Web.UI.WebControls.GridViewSortEventArgs e)
        {
            if (DataSortExpression == e.SortExpression)
            {
                DataSortDirection = DataSortDirection == "ASC" ? "DESC" : "ASC";
            }
            else
            {
                DataSortExpression = e.SortExpression;
                DataSortDirection = "ASC";
            }
            RegisterAsyncTask(new PageAsyncTask(async () => await LoadSnapshotDetails(CurrentSnapshotId)));
        }

        protected void SnapshotFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(LoadSnapshots));
        }

        protected void DataFilterTextBox_TextChanged(object sender, EventArgs e)
        {
            RegisterAsyncTask(new PageAsyncTask(async () => await LoadSnapshotDetails(CurrentSnapshotId)));
        }
    }
}