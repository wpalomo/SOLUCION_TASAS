<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmSincronizarTasasUsuario.aspx.cs" Inherits="Solution_CTT.frmSincronizarTasasUsuario" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">

                <%--INICIO DEL PANEL DE GRID--%>
                <asp:Panel ID="pnlGrid" runat="server">                    
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_TASA_NO_ENVIADA %></h3>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" >
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDFACTURA" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IIDENTIFICACION" HeaderText="IDENTIFICACIÓN" />
                                            <asp:BoundField DataField="ICLIENTE" HeaderText="CLIENTE" />
                                            <asp:BoundField DataField="IFECHAFACTURA" HeaderText="FECHA DE FACTURA" />
                                            <asp:BoundField DataField="IFACTURA" HeaderText="FACTURA" />
                                            <asp:BoundField DataField="ITASAUSUARIO" HeaderText="TASA DE USUARIO" />
                                            <asp:BoundField DataField="ICANTIDADTASAS" HeaderText="CANTIDAD_TASAS" />
                                            <asp:BoundField DataField="IDIRECCIONFACTURA" HeaderText="DIRECCION" />
                                            <asp:BoundField DataField="ITELEFONOFACTURA" HeaderText="TELEFONO" />
                                            <asp:BoundField DataField="ICORREOFACTURA" HeaderText="CORREO" />
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>

                                <div class="box-footer">
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:Button ID="btnSincronizar" runat="server" Text="Sincronizar" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSincronizar_Click" />
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:Button ID="btnActualizar" runat="server" Text="Actualizar" class="btn btn-sm btn-warning btn-block pull-right" OnClick="btnActualizar_Click" />
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                <%--FINAL DE PANEL DEL GRID--%>

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label10" runat="server" Text="Información"></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="lblMensajeError" runat="server"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnCancelarError" runat="server" Text="Aceptar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false"/>
                            </div>
                        </div>
                    </div>
                </div>
                <%--FIN MODAL DE ERRORES--%>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

    <%--PROGRESS--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal-backdrop">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <%--<input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />--%>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
