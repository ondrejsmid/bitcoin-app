<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Stored.aspx.cs" Inherits="BitcoinCourseUI.Stored" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Save Snapshot</title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Button runat="server" ID="BtnSave" Text="Save Snapshot" OnClick="BtnSave_Click" />
        </div>
    </form>
</body>
</html>