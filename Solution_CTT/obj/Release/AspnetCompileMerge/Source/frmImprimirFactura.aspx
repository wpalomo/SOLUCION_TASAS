<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmImprimirFactura.aspx.cs" Inherits="Solution_CTT.frmImprimirFactura" %>

<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
        <asp:TextBox ID="txtIdPedido" runat="server"></asp:TextBox>
        <asp:Button ID="btnGenerar" runat="server" Text="Generar" OnClick="btnGenerar_Click" />
        <asp:Button ID="btnVer" runat="server" Text="VER" OnClick="btnVer_Click" />
        <asp:TextBox ID="txtVer" runat="server"></asp:TextBox>
        <asp:Button ID="btnManifiesto" runat="server" Text="Manifiesto" OnClick="btnManifiesto_Click" />
        <br />
        <asp:Button ID="btnImprimirSoloTasa" runat="server" Text="Imprimir Solo Tasa" OnClick="btnImprimirSoloTasa_Click" />
        <asp:TextBox ID="TxtFechaViaje" runat="server" class="form-control input-sm"  BackColor="White" ></asp:TextBox>                                                        
        <ajaxToolkit:MaskedEditExtender ID="TxtFechaViaje_MaskedEditExtender" runat="server" BehaviorID="TxtFechaViaje_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="TxtFechaViaje" />
        <ajaxToolkit:CalendarExtender ID="TxtFechaViaje_CalendarExtender" runat="server" BehaviorID="TxtFechaViaje_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="TxtFechaViaje" />
        <asp:Button ID="btnCierreCaja" runat="server" Text="Cierre de Caja" OnClick="btnCierreCaja_Click" />
        <rsweb:ReportViewer ID="rptFactura" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="800px" Width="400px">
        <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.rptFactura.rdlc">
        </LocalReport>
        </rsweb:ReportViewer>

        <asp:Image ID="imgVer" runat="server" />
    </div>
    </form>
</body>
</html>
