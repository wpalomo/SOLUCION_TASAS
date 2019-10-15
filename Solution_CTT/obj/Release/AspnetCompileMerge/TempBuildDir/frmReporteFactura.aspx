<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReporteFactura.aspx.cs" Inherits="Solution_CTT.frmReporteFactura" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rptFactura" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="1024px" Width="400px">
        <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.rptFactura.rdlc">
        </LocalReport>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
