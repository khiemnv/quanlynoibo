<%@ Page Language="C#" AutoEventWireup="true" CodeFile="congviec.aspx.cs" Inherits="congviec" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <h3>Công việc</h3>
    <form id="form1" runat="server">

        <div style="float: left; width: 90px;height:100%">
            <asp:Menu ID="MenuBan" runat="server" OnMenuItemClick="MenuBan_MenuItemClick">
            </asp:Menu>
        </div>
        <div style="margin-left:90px">
            Ngày bắt đầu
            <table>
                <tr>
                    <td><asp:Button ID="preDay" runat="server" Text="Prev" /></td>
                    <td>
                        <asp:TextBox ID="startDate" runat="server">2018/01/03</asp:TextBox></td>
                    <td>
                        <asp:Button ID="nextDay" runat="server" Text="Next" /></td>
                </tr>
            </table>
            <br />
            Công việc trong ngày:
            <asp:GridView ID="congviecGV" runat="server">
            </asp:GridView>
            <br />
            Kế hoạch:
            <asp:GridView ID="kehoachGV" runat="server">
            </asp:GridView>
        </div>
    </form>
</body>
</html>
