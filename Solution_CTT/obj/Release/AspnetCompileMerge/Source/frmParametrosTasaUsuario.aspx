<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametrosTasaUsuario.aspx.cs" Inherits="Solution_CTT.frmParametrosTasaUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">
                <%--INICIO DE PANEL DE REGISTRO--%>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Parámetros de Tasas de Usuario - DEVESOFFT</h3>
                                </div>

                                <asp:Panel ID="pnlRegistro" runat="server">
                                <div class="box-body">
                                    <div class="form-group">
                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Terminal:"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTerminal" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="Ambiente a Trabajar"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbAmbiente" runat="server" class="form-control input-sm">
                                                            <asp:ListItem Value="0">Pruebas</asp:ListItem>
                                                            <asp:ListItem Value="1">Producción</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Valor Tasa Usuario"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtValorTasa" runat="server" class="form-control input-sm" placeholder="Valor Tasa Usuario" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Id Oficina"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtIdOficina" runat="server" class="form-control input-sm" placeholder="Id Oficina" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="Id Cooperativa"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtIdCooperativa" runat="server" class="form-control input-sm" placeholder="Id Cooperativa" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN PRIMERA FILA--%>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Web Service Pruebas"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtWsPruebas" runat="server" CssClass="form-control input-sm" placeholder="Web Service de Pruebas - Tasas de Usuario" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Web Service Producción"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtWsProduccion" runat="server" CssClass="form-control input-sm" placeholder="Web Service de Producción - Tasas de Usuario" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN SEGUNDA FILA--%>

                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="URL Credenciales"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlCredendiales" runat="server" CssClass="form-control input-sm" placeholder="URL Credenciales" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="URL Emisión Tasa Usuario *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlEmisionTasaUsuario" runat="server" CssClass="form-control input-sm" placeholder="URL Emisión Tasa Usuario" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="URL Anulación Tasa *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlAnulacionTasaUsuario" runat="server" CssClass="form-control input-sm" placeholder="URL Anulación Tasa" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label16" runat="server" Text="URL Obtener Tokens *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlGetTokens" runat="server" CssClass="form-control input-sm" placeholder="URL Obtener Tokens" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN TERCERA FILA--%>

                                        <%--CUARTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="URL Verificar Token *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlVerificarToken" runat="server" CssClass="form-control input-sm" placeholder="URL Verificar Token" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="URL Emisión en Lote *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlEmisionLote" runat="server" CssClass="form-control input-sm" placeholder="URL Emisión en Lote" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label14" runat="server" Text="URL Detalle de Transacciones *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlDetalleTransacciones" runat="server" CssClass="form-control input-sm" placeholder="URL Detalle de Transacciones" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label15" runat="server" Text="URL Enviar Notificación *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlEnviarNotificacion" runat="server" CssClass="form-control input-sm" placeholder="URL Enviar Notificación" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN CUARTA FILA--%>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkAnularTasa" runat="server" class="form-control input-sm" Text="&nbsp&nbspPermite Anular Tasa de Usuario" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkImprimirTasaUsuario" runat="server" class="form-control input-sm" Text="&nbsp&nbspImprimir Tasa de Usuario" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkNotificacionEmergente" runat="server" class="form-control input-sm" Text="&nbsp&nbspMostrar Notificación Emergente de Porcentaje de Tasas de Usuario" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN QUINTA FILA--%>

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
