<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReportePrueba.aspx.cs" Inherits="Solution_CTT.frmReportePrueba" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir" OnClick="btnImprimir_Click" />
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>
        <rsweb:ReportViewer ID="rptPruebas" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="400px" Width="900px">
        <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.rptFactura.rdlc">
        </LocalReport>
        </rsweb:ReportViewer>
    </div>
    </form>
</body>
</html>
