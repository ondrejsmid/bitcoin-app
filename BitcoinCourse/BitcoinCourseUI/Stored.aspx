<%@ Page Title="Stored Data" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Stored.aspx.cs" Inherits="BitcoinCourseUI.Stored"%>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <main>
        <section>
            <h2>Stored Data</h2>
            <asp:GridView ID="GridViewStored" runat="server" AutoGenerateColumns="true" CssClass="table table-striped" EmptyDataText="No data">
            </asp:GridView>
        </section>
    </main>
</asp:Content>