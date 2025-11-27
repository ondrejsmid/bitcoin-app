<%@ Page Title="Stored Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stored.aspx.cs" Inherits="BitcoinCourseUI.Stored" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section>
            <h2>Saved Snapshots</h2>
            
            <asp:Label ID="NoSnapshotMessage" runat="server" CssClass="alert alert-info d-block" Visible="false">
                No snapshot saved yet
            </asp:Label>
            
            <div class="row" runat="server" id="SnapshotContentPanel" visible="false">
                <div class="col-md-4">
                    <h4>Snapshots</h4>
                    <asp:GridView ID="GridViewSnapshots" runat="server" AutoGenerateColumns="false" 
                        CssClass="table table-hover" EmptyDataText="No snapshots"
                        OnSelectedIndexChanged="GridViewSnapshots_SelectedIndexChanged"
                        DataKeyNames="Id">
                        <Columns>
                            <asp:CommandField ShowSelectButton="True" SelectText="View" ButtonType="Button" />
                            <asp:BoundField DataField="Id" HeaderText="ID" />
                            <asp:BoundField DataField="Note" HeaderText="Note" />
                        </Columns>
                        <SelectedRowStyle BackColor="#D1E8FF" />
                    </asp:GridView>
                </div>
                
                <div class="col-md-8">
                    <asp:Panel ID="SnapshotDetailPanel" runat="server" Visible="false">
                        <h4>Snapshot Details</h4>
                        <div class="mb-3">
                            <strong>Note:</strong> <asp:Label ID="SnapshotNoteLabel" runat="server" />
                        </div>
                        
                        <asp:GridView ID="GridViewStored" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" EmptyDataText="No data">
                            <Columns>
                                <asp:BoundField DataField="Label" HeaderText="Name" />
                                <asp:BoundField DataField="Value" HeaderText="Value" />
                            </Columns>
                        </asp:GridView>
                    </asp:Panel>
                </div>
            </div>
        </section>
    </main>
</asp:Content>