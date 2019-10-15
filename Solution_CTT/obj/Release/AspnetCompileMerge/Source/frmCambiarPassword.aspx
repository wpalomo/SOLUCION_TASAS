<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmCambiarPassword.aspx.cs" Inherits="Solution_CTT.frmCambiarPassword" %>
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
                                    <h3 class="box-title">Cambio de Contraseña</h3>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">

                                        <div class="row">
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="NOMBRE DEL USUARIO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPersona" runat="server" class="form-control input-sm" placeholder="PERSONA" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="USUARIO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUsuario" runat="server" class="form-control input-sm" placeholder="USUARIO" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="CONTRASEÑA ACTUAL"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPasswordActual" runat="server" class="form-control input-sm" TextMode="Password" placeholder="CONTRASEÑA ACTUAL" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="NUEVA CONTRASEÑA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPasswordNueva" runat="server" class="form-control input-sm" TextMode="Password" placeholder="NUEVA CONTRASEÑA" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="CONFIRMAR CONTRASEÑA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPasswordConfirmar" runat="server" class="form-control input-sm" TextMode="Password" placeholder="CONFIRMAR CONTRASEÑA" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>                                       

                                <div class="box-footer">
                                    <asp:Button ID="btnCambiar" runat="server" Text="Cambiar Contraseña" class="btn btn btn-info" OnClick="btnCambiar_Click" />
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
