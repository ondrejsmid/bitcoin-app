<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="BitcoinCourseUI._Default" Async="true"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <main>
        <section class="row" aria-labelledby="staticTableTitle">
            <h2 id="staticTableTitle">Sample Dynamic Table</h2>
            <asp:Panel ID="PanelTable" runat="server">
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
