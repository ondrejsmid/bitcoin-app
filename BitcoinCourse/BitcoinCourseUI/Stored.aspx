<%@ Page Title="Stored Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stored.aspx.cs" Inherits="BitcoinCourseUI.Stored" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section>
            <h2>Stored Data - Last Snapshot</h2>
            
            <asp:Label ID="NoSnapshotMessage" runat="server" CssClass="alert alert-info d-block" Visible="false">
                No snapshot saved yet
            </asp:Label>
            
            <asp:Panel ID="SnapshotInfoPanel" runat="server" Visible="false" CssClass="mb-3">
                <strong>Note:</strong> <asp:Label ID="SnapshotNoteLabel" runat="server" />
            </asp:Panel>
            
            <asp:GridView ID="GridViewStored" runat="server" AutoGenerateColumns="false" CssClass="table table-striped" EmptyDataText="No data">
                <Columns>
                    <asp:BoundField DataField="Label" HeaderText="Name" />
                    <asp:BoundField DataField="Value" HeaderText="Value" />
                </Columns>
            </asp:GridView>
        </section>
    </main>
</asp:Content>