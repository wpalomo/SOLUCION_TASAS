<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmLocalidadesImpresoras.aspx.cs" Inherits="Solution_CTT.frmLocalidadesImpresoras" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-md-5">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title">Definición de Impresoras por Localidad</h3>

                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <%--<asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>--%>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging" >
                                    <Columns>
                                        <asp:BoundField DataField="id_localidad_impresora" HeaderText="ID"/>
                                        <asp:BoundField DataField="id_localidad" HeaderText="ID LOCALIDAD" />
                                        <asp:BoundField DataField="nombre_localidad" HeaderText="LOCALIDAD" />
                                        <asp:BoundField DataField="nombre_impresora" HeaderText="IMPRESORA" />
                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="puerto_impresora" HeaderText="PUERTO IMPRESORA" />
                                        <asp:BoundField DataField="numero_cotizacion" HeaderText="COTIZACION" />
                                        <asp:BoundField DataField="numero_pedido" HeaderText="PEDIDO" />
                                        <asp:BoundField DataField="numero_factura" HeaderText="FACTURA" />
                                        <asp:BoundField DataField="numero_nota_credito" HeaderText="NOTA DE CRÉDITO" />
                                        <asp:BoundField DataField="numero_nota_debito" HeaderText="NOTA DE DÉBITO" />
                                        <asp:BoundField DataField="numero_guia_remision" HeaderText="GUÍA DE REMISIÓN" />
                                        <asp:BoundField DataField="numero_pago" HeaderText="PAGO" />
                                        <asp:BoundField DataField="numeroanticipocliente" HeaderText="ANTICIPO CLIENTE" />
                                        <asp:BoundField DataField="numeropagoserieb" HeaderText="´PAGO SERIE B" />
                                        <asp:BoundField DataField="numeronotaventa" HeaderText="NOTA DE VENTA" />
                                        <asp:BoundField DataField="numeronotaentrega" HeaderText="NOTA DE ENTREGA" />
                                        <asp:BoundField DataField="numeromovimientocaja" HeaderText="MOVIMIENTO DE CAJA" />
                                        <asp:BoundField DataField="numeroencomienda" HeaderText="ENCOMIENDA" />
                                        <asp:BoundField DataField="estado_1" HeaderText="ESTADO" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEdit_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="BORRAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnDelete_Click"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- /.box -->
                    </div>
                    <%--REGISTER--%>
                    <div class="col-md-7">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">Datos del Registro</h3>
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
                                                <asp:Label ID="Label18" runat="server" Text="Localidad *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbLocalidadTerminal" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label3" runat="server" Text="Nombre Impresora *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNombreImpresora" runat="server" class="form-control input-sm" autocomplete="off" placeholder="Nombre Impresora" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label16" runat="server" Text="Puerto Impresora *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPuertoImpresora" runat="server" class="form-control input-sm" autocomplete="off" placeholder="Puerto Impresora" ></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEGUNDA FILA--%>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label4" runat="server" Text="N° Cotización *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtCotizacion" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Cotización" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label5" runat="server" Text="N° Pedido *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPedido" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Pedido" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label6" runat="server" Text="N° Factura *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtFactura" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Factura" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label17" runat="server" Text="N° Nota Crédito *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNotacredito" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Nota Crédito" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--TERCERA FILA--%>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="N° Nota Débito *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNotaDebito" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Nota Débito" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label8" runat="server" Text="N° Guía Remisión *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtGuiaRemision" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Guía Remisión" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="N° Pago *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPago" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Pago" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label19" runat="server" Text="N° Anticipo Cliente *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtAnticipoCliente" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Anticipo Cliente" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--CUARTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="N° Pago Serie B *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPagoSerieB" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Pago Serie B" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="N° Nota Venta *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNotaVenta" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Nota Venta" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="N° Nota Entrega *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNotaEntrega" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Nota Entrega" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label20" runat="server" Text="N° Movimiento *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtMovimientoCaja" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Movimiento" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--QUINTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label13" runat="server" Text="N° Encomienda *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtEncomienda" runat="server" class="form-control input-sm" autocomplete="off" placeholder="N° Encomienda" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label14" runat="server" Text="Estado *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbEstado" runat="server" class="form-control">
                                                        <asp:ListItem Value="A">ACTIVO</asp:ListItem>
                                                         <asp:ListItem Value="E">ELIMINADO</asp:ListItem>
                                                    </asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEXTA FILA--%>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="btn btn-sm btn-default btn-block pull-right" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnSave" runat="server" Text="Crear" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSave_Click" />
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
                                    <asp:Label ID="lbAccion" runat="server" Text="¿ Está seguro ?"></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label1" runat="server" Text="Desea eliminar el registro, puede que no sea recuperable"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click"/>
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
