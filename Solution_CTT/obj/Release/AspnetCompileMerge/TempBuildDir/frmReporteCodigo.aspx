﻿<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="frmReporteCodigo.aspx.cs" Inherits="Solution_CTT.frmReporteCodigo" %>

<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <rsweb:ReportViewer ID="rptReporte" runat="server">
        </rsweb:ReportViewer>
    
    </div>
    </form>
</body>
</html>
