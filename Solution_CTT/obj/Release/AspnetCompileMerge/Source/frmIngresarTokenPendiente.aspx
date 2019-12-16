<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmIngresarTokenPendiente.aspx.cs" Inherits="Solution_CTT.frmIngresarTokenPendiente" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">
                <%--INICIO DE PANEL DE REGISTRO--%>
                <asp:Panel ID="pnlRegistro" runat="server">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Ingreso de Token sin Sincronización</h3>

                                   <%-- <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            
                                        </div>
                                    </div>--%>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">

                                        <%--SECCION: OFICINA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="TERMINAL"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTerminal" runat="server" class="form-control input-sm" placeholder="TERMINAL" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="AMBIENTE"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtAmbiente" runat="server" class="form-control input-sm" placeholder="AMBIENTE" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="TOKEN GENERADO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTokenGenerado" runat="server" class="form-control input-sm" placeholder="TOKEN GENERADO" BackColor="White" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="CANTIDAD ADQUIRIDA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCantidad" runat="server" class="form-control input-sm" placeholder="CANTIDAD MAXIMA" BackColor="White" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>                                       

                                <div class="box-footer">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Token Generado" class="btn btn btn-success" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn btn-danger" OnClick="btnLimpiar_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                
                <%--FIN DE PANEL DE REGISTRO--%>

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

                <%--MODAL PARA CREAR EL MODAL DE CONFIRMACION--%>
                <div class="modal fade" id="QuestionModalConfirmacion" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label11" runat="server" Text="Información."></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="lblAlertaMensajeConfirmacion" runat="server" Text="¿Está seguro que desea ingresar el token?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNoCerrar" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAceptarCerrar" runat="server" Text="Sí, confirmar" class="btn btn-info" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptarCerrar_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL PARA CREAR EL MODAL DE CONFIRMACION--%>
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
