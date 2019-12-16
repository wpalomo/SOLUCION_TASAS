<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametrosContifico.aspx.cs" Inherits="Solution_CTT.frmParametrosContifico" %>
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
                                    <h3 class="box-title">Parámetros de Tasas de Usuario - CONTÍFICO</h3>
                                </div>

                                <asp:Panel ID="pnlRegistro" runat="server">
                                <div class="box-body">
                                    <div class="form-group">
                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="URL Autenticación"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlAutenticacion" runat="server" CssClass="form-control input-sm" placeholder="URL de Autenticación" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="URL Localidades"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlLocalidades" runat="server" CssClass="form-control input-sm" placeholder="URL para las Localidades" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN PRIMERA FILA--%>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="URL Conductores"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlConductores" runat="server" CssClass="form-control input-sm" placeholder="URL para Conductores" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="URL Frecuencias"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlFrecuencias" runat="server" CssClass="form-control input-sm" placeholder="URL para las Frecuencias" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN SEGUNDA FILA--%>

                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="URL Buses"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlBuses" runat="server" CssClass="form-control input-sm" placeholder="URL para los Buses" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="URL Rutas"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlRutas" runat="server" CssClass="form-control input-sm" placeholder="URL para las Rutas" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN TERCERA FILA--%>

                                        <%--CUARTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="URL Ventas"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlVentas" runat="server" CssClass="form-control input-sm" placeholder="URL para la Venta" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="URL Viajes"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlViajes" runat="server" CssClass="form-control input-sm" placeholder="URL para los viajes" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN CUARTA FILA--%>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="URL Cambiar Bus"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlCambiarBus" runat="server" CssClass="form-control input-sm" placeholder="URL para el Cambio de Bus" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="URL Anular Asiento"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUrlAnularAsiento" runat="server" CssClass="form-control input-sm" placeholder="URL para Anular Asientos" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>                                        
                                        <%--FIN QUINTA FILA--%>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Tiempo de Respuesta en Segundos"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTiempoRespuesta" runat="server" CssClass="form-control input-sm" Onkeypress="return ValidaDecimal(this.value);" placeholder="Tiempo de respuesta" BackColor="White" MaxLength="2"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Proveedor de Tasas de Usuario"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbProveedor" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
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
