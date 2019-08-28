<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametroCorreo.aspx.cs" Inherits="Solution_CTT.frmParametroCorreo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">        
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Configuración de Correo Electrónico</h3>
                            </div>

                            <asp:Panel ID="pnlRegistro_1" runat="server">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Correo Emisor *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCorreoEmisor" runat="server" class="form-control" autocomplete="off" placeholder="Correo Emisor" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="SMTP *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtSmtp" runat="server" class="form-control" autocomplete="off" placeholder="SMTP del correo" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Puerto *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPuerto" runat="server" class="form-control" autocomplete="off" placeholder="Puerto del correo" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Password del Correo Emisor"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPassword" runat="server" class="form-control" autocomplete="off" placeholder="Contraseña del correo emisor" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnVerPassword" runat="server" Text="" OnClick="btnVerPassword_Click" tooltip="Clic para visualizar la clave"><i class="fa fa-eye"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Control de SSL"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="TextBox3" runat="server" class="form-control" Text="Habilitar" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon"><asp:CheckBox ID="chkSSL" runat="server" Text="" /></span>                                                        
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                    </div>
                </div>


                <%--CORREOS DESTINOS--%>
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Correos destinatarios</h3>
                            </div>

                            <asp:Panel ID="pnlRegistro_2" runat="server">
                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Correo Destinatario 1 *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCorreoDestino_1" runat="server" class="form-control" autocomplete="off" placeholder="Correo Destinatario 1" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Correo Destinatario 2"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCorreoDestino_2" runat="server" class="form-control" autocomplete="off" placeholder="Correo Destinatario 2" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Correo Destinatario 3"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCorreoDestino_3" runat="server" class="form-control" autocomplete="off" placeholder="Correo Destinatario 3" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </asp:Panel>                            

                            <div class="box-footer pull-right">
                                <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn btn-info" OnClick="btnGuardar_Click" />
                                <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" data-backdrop="false" class="btn btn btn-danger" OnClick="btnCancelar_Click" />
                            </div>

                            <div class="row">
                                <div class="col-md-offset-1 col-md-10">
                                    <div class="alert  alert-warning" id="MsjValidarCampos" runat="server" visible="false">
                                        <button type="button" class="close" data-dismiss="alert" aria-hidden="true">&times;</button>
                                        <h4><i class="icon fa fa-warning"></i>Alerta.!</h4>
                                            Complete los campos, por favor.!
                                    </div>                                            
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN CORREOS DESTINOS--%>

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
