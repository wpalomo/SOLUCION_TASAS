<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmRutasContifico.aspx.cs" Inherits="Solution_CTT.frmRutasContifico" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <!-- Main content -->
            <section class="content">
                <div class="row">
                      <%--REGISTER--%>
                  <div class="col-md-12">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">Localidades del Sistema</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>

                            <%--FORM--%>
                            <div class="box-body">
                                <div class="form-group">
                                    <%--PRIMERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label18" runat="server" Text="Seleccione la Vía"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbVia" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbVia_SelectedIndexChanged" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="Nombre del Destino"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNombreDestino" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="NOMBRE DEL DESTINO" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Número del Andén"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNumeroAnden" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="ANDEN" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="Seleccione la Parada"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbParadas" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbParadas_SelectedIndexChanged" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label6" runat="server" Text="Orden de Llegada"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtOrdenLlegada" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="TIPO SERVICIO" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="Habilitado"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkHabilitado" Enabled="false" Text="Habilitado" class="form-control input-sm" runat="server" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <asp:Panel ID="pnlRegistros" runat="server" Visible="false">
                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Tipo de Servicio"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTipoServicio" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="Nombre Comercial" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-9">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Nombre del Servicio"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNombreServicio" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="NOMBRE DEL SERVICIO" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--CUARTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Tipo de Cliente"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTipoCliente" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="TIPO CLIENTE" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-9">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="Cliente Nombre"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtClienteNombre" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="Cliente Nombre" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Tarifa"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTarifa" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="TARIFA" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label15" runat="server" Text="ID"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtIdTarifa" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="ID TARIFA" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label14" runat="server" Text="Actualización"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtActualizacion" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="FECHA DE ACTUALIZACIÓN" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkEspecialTarifa" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspTarifa Especial" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkActivoTarifa" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspTarifa Activa" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkHabilitadoTarifa" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspTarifa Habilitada" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--SEXTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                            <Columns>
                                                <asp:BoundField DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="tipo_servicio" HeaderText="TIPO SERVICIO" />
                                                <asp:BoundField DataField="tipo_servicio_nombre" HeaderText="NOMBRE SERVICIO" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="tipo_cliente" HeaderText="TIPO CLIENTE" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="tipo_cliente_nombre" HeaderText="NOMBRE CLIENTE" />
                                                <asp:BoundField DataField="tarifa" HeaderText="TARIFA" />
                                                <asp:BoundField DataField="especial" HeaderText="ESPECIAL" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="is_active" HeaderText="ACTIVO" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="is_enable" HeaderText="HABILITADO" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="actualizacion" HeaderText="FECHA" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="form-group">
                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
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

                        </div>
                    </div>              
                </div>

                <div class="modal fade" id="QuestionModal" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="lbAccion" runat="server" Text="Información"></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label1" runat="server" Text="¿Está seguro que desea actualizar el registro?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal" />
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, confirmar" class="btn btn-success" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label2" runat="server" Text="Información"></asp:Label>
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
            <!-- /.content -->
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
