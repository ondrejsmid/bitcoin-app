<%@ Page Title="Live Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BitcoinCourseUI._Default" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="staticTableTitle">
            <h2 id="staticTableTitle">Sample Dynamic Table</h2>

            <asp:Panel ID="PanelTable" runat="server">
                <!-- Save snapshot button -->
                <div class="mb-3">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox ID="SnapshotNoteTextBox" runat="server" CssClass="form-control" placeholder="Enter note for snapshot" />
                            <asp:RequiredFieldValidator ID="NoteRequiredValidator" runat="server" 
                                ControlToValidate="SnapshotNoteTextBox" 
                                ErrorMessage="Note is required" 
                                CssClass="text-danger" 
                                Display="Dynamic" />
                        </div>
                        <div class="col-md-8">
                            <asp:Button ID="SaveSnapshotButton" runat="server" CssClass="btn btn-primary" Text="Save Snapshot" OnClick="SaveSnapshotButton_Click" />
                            <asp:Label ID="SnapshotStatus" runat="server" CssClass="ms-3" />
                        </div>
                    </div>
                </div>

                <asp:UpdatePanel ID="upGrid" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Timer ID="RefreshTimer" runat="server" Interval="5000" OnTick="RefreshTimer_Tick" />

                        <asp:GridView ID="GridViewData" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" EmptyDataText="No data">
                            <Columns>
                                <asp:BoundField DataField="Label" HeaderText="Name" />
                                <asp:BoundField DataField="Value" HeaderText="Value" />
                            </Columns>
                        </asp:GridView>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="RefreshTimer" EventName="Tick" />
                    </Triggers>
                </asp:UpdatePanel>
            </asp:Panel>
        </section>
    </main>

</asp:Content>
