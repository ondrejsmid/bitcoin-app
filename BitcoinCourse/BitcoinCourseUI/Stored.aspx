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
                            <strong>Note:</strong>
                            <asp:Label ID="SnapshotNoteLabel" runat="server" />
                            <asp:LinkButton ID="EditNoteButton" runat="server" CssClass="btn btn-sm btn-outline-primary ms-2" OnClick="EditNoteButton_Click">
                                <i class="bi bi-pencil"></i> Edit
                            </asp:LinkButton>
                        </div>
                        
                        <asp:Panel ID="EditNotePanel" runat="server" Visible="false" CssClass="mb-3">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:TextBox ID="EditNoteTextBox" runat="server" CssClass="form-control" />
                                    <asp:RequiredFieldValidator ID="EditNoteRequiredValidator" runat="server" 
                                        ControlToValidate="EditNoteTextBox" 
                                        ErrorMessage="Note is required" 
                                        CssClass="text-danger" 
                                        Display="Dynamic"
                                        ValidationGroup="EditNote" />
                                </div>
                                <div class="col-md-6">
                                    <asp:Button ID="SaveNoteButton" runat="server" CssClass="btn btn-success" Text="Save" OnClick="SaveNoteButton_Click" ValidationGroup="EditNote" />
                                    <asp:Button ID="CancelEditButton" runat="server" CssClass="btn btn-secondary ms-2" Text="Cancel" OnClick="CancelEditButton_Click" CausesValidation="false" />
                                    <asp:Label ID="EditStatusLabel" runat="server" CssClass="ms-2" />
                                </div>
                            </div>
                        </asp:Panel>
                        
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