<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPruebas.aspx.cs" Inherits="Solution_CTT.frmPruebas" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="ajaxToolkit" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script src="assets/js/jquery.min.js"></script>
    <script src="plugins/timepicker/bootstrap-timepicker.js"></script>
    <script type="text/javascript">
        $(function () {
            $('[id*=txtDate]').datepicker({
                changeMonth: true,
                changeYear: true,
                format: "dd/mm/yyyy",
                language: "tr"
            });
        });
    </script>
    <style>
.cargando {
width: 100%;height: 100%;
overflow: hidden; 
top: 0px;
left: 0px;
z-index: 10000;
text-align: center;
position:absolute; 
background-color: #FFFFFF;
opacity:0.6;
filter:alpha(opacity=40);
}

    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
   <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>--%>
    <section class="content">
        <div class="row">
            <div class='col-sm-9'>
                <div class="form-group">
                    <asp:TextBox ID="txtDate" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:TextBox ID="txtJsonLote" placeholder="JSON RECIBIDO" runat="server"></asp:TextBox>
                </div>

                <div class="form-group">
                    <asp:TextBox ID="txtWebService" placeholder="WEB SERVICE" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtRespuestaWeb" placeholder="RESPUESTA WEB SERVICE" runat="server"></asp:TextBox>
                </div>

                <div class="form-group row">
                  <%--<label for="example-date-input" class="col-2 col-form-label">Date</label>--%>
                  <div class="col-10">
                      <asp:TextBox ID="TextBox4" runat="server" class="form-control" type="date" value="2019-01-02"></asp:TextBox>
                    <input class="form-control" type="date" value="2011-08-19" id="example-date-input">
                  </div>
                </div>
                <div class="form-group">
                    <asp:TextBox ID="TextBox5" runat="server" ReadOnly="True"></asp:TextBox>
                    <ajaxToolkit:CalendarExtender ID="TextBox5_CalendarExtender" runat="server" BehaviorID="TextBox5_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="TextBox5" />
                </div>
            </div>
            <asp:Button ID="btnFondo" runat="server" Text="Button" OnClick="btnFondo_Click"/>
            <asp:Button ID="Button1" runat="server" Text="HOLA MUNDO :)" />
            <asp:Button ID="btnDeserializar" runat="server" Text="Deserializar" OnClick="btnDeserializar_Click" />
            <asp:Button ID="btnWebService" runat="server" Text="Ver Web Service" OnClick="btnWebService_Click" />

            <div id="bloquea" class="cargando" style="display:none;">
                </div>
    </section>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
