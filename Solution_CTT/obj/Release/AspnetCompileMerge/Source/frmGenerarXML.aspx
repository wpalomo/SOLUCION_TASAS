<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmGenerarXML.aspx.cs" Inherits="Solution_CTT.frmGenerarXML"%>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="box-default">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-file-text-o"></i>
                                <h3 class="box-title">Generar XML</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-6 form-group">
                                        <label>N° de factura:</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNumeroFacturaBuscar" runat="server" class="form-control input-sm" placeholder="01" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm">N°</i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnBuscar" runat="server" class="form-control btn btn-flat btn-success" ToolTip="Clic aquí, para buscar según N° de factura" OnClick="lbtnBuscar_Click"><i class="fa fa-search"></i>   BUSCAR</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnBuscarAbrirModal" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para abrir modal de facturas" OnClick="lbtnBuscarAbrirModal_Click"><i class="fa fa-file-archive-o"></i>   ABRIR MODAL</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar"><i class="fa fa-circle-o-notch"></i>   Cancelar</asp:LinkButton>
                                        </div>
                                    </div>
                                </div>

                                <div>
                                    <ul class="nav nav-tabs" role="tablist">
                                        <li id="l1" class="active"><a href="#home" aria-controls="home" role="tab" data-toggle="tab">Datos del Registro</a></li>
                                        <li id="l2"><a href="#details" aria-controls="profile" role="tab" data-toggle="tab">Especificaciones</a></li>
                                    </ul>
                                    <div class="tab-content">
                                        <div role="tabpanel" class="tab-pane active" id="home">
                                            <p></p>
                                            <div class="row">
                                                <div class="col-md-3 form-group">
                                                    <label>Fecha</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFecha" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Vendedor</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtVendedor" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Formato</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFormato" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-archive"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Tipo de Cliente</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTipoCliente" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>N° Documento Cliente</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtDocumentoCliente" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-file"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Nombre del Cliente</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNombreCliente" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-6 form-group">
                                                    <label>Dirección</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtDireccion" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-map"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Ciudad</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCiudad" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-address-card"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Teléfono</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-phone"></i></span>
                                                    </div>
                                                </div>

                                                <div class="col-md-3 form-group">
                                                    <label>Moneda</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMoneda" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-money"></i></span>
                                                    </div>
                                                </div>

                                                <div class="col-md-3 form-group">
                                                    <label>% Descuento</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPorcentajeDescuento" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm">-</span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Tipo de Pago</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTipoPago" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Fecha Vencimiento</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaVcto" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <div role="tabpanel" class="tab-pane" id="details">
                                            <p></p>
                                            <div class="row">
                                                <div class="col-md-3 form-group">
                                                    <label>Fabricante</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFabricante" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>txtRefOt</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtRefOt" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Observación</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtObser" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Peso Neto</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPesoNeto" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Peso Bruto</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPesoBruto" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>N° de Exportación</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNExportacion" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Partida Arancelaria</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPartidaArancelaria" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Autorización SRI</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="cmbAutSri" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Localidad</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtLocalidad" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 form-group">
                                                    <label>Correo</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMail" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-header with-border">
                                <h4 class="box-title">Detalle de Factura</h4>
                            </div>
                            <p></p>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="true" PageSize="10">
                                    <Columns>
                                        <asp:BoundField DataField="codigo" HeaderText="Cóodigo" />
                                        <asp:BoundField DataField="nombre" HeaderText="Producto" />
                                        <asp:BoundField DataField="Unidad" HeaderText="Unidad" />
                                        <asp:BoundField DataField="Cantidad" HeaderText="Cantidad" />
                                        <asp:BoundField DataField="precio_unitario" HeaderText="Valor U." />
                                        <asp:BoundField DataField="Pct_Dscto" HeaderText="% Descuento" />
                                        <asp:BoundField DataField="valor_Dscto" HeaderText="Descuento" />
                                        <asp:BoundField DataField="valor_otro" HeaderText="Valor Ser." />
                                        <asp:BoundField DataField="ValorTotal" HeaderText="Valor Total" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>


                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-2 form-group">
                                        <label>Valor Bruto</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtValorBruto" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>Descuento</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtDescuento" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>SubTotal</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtSubTotal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>I.V.A.</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtIva" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>Servicio</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtServicio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>Total a Pagar</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtTotalPagar" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>Serie 1 N°</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNSerie1" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>Serie 2 N°</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNSerie2" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label>N° Autorización</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNAut" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Campo generado automáticamente"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-font"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <div class="col-md-2 form-group">
                                    <label></label>
                                    <div class="input-group col-sm-12">
                                        <asp:LinkButton ID="lbtnGenerarXML" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para generar XML" OnClick="lbtnGenerarXML_Click"><i class="fa fa-pencil-square-o"></i>  Generar XML</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-md-2 form-group">
                                    <label></label>
                                    <div class="input-group col-sm-12">
                                        <asp:LinkButton ID="lbtnGenerarRIDE" runat="server" class="form-control btn btn-flat btn-success" ToolTip="Clic aquí, para generar RIDE" OnClick="lbtnGenerarRIDE_Click"><i class="fa fa-pencil-square-o"></i>  Generar RIDE</asp:LinkButton>
                                    </div>
                                </div>
                                <div class="col-md-2 form-group">
                                    <label></label>
                                    <div class="input-group col-sm-12">
                                        <asp:LinkButton ID="lbtnCancelar2" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="lbtnCancelar2_Click"><i class="fa fa-circle-o-notch"></i>   Cancelar</asp:LinkButton>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--MODAL CONFIRMACION ELIMINAR--%>
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
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal" />
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--FIN--%>

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
                                <button type="button" class="btn btn-default" data-dismiss="modal">Aceptar</button>
                            </div>
                        </div>
                    </div>
                </div>
                <%--FIN MODAL DE ERRORES--%>
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>
    <%--MODAL--%>
    <asp:Button ID="btnInicial" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalBuscarFacturas" runat="server"
        Enabled="True" TargetControlID="btnInicial"
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <div class="modal-content modal-lg">
                    <div class="modal-header">
                        <asp:LinkButton ID="lbtnCerrarModalBuscarFacturas" runat="server" CssClass="close" OnClick="lbtnCerrarModalBuscarFacturas_Click"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h4 class="modal-title" id="myModalLabel5">Facturas Registradas</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltarModalBuscarFacturas">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarModalBuscarFacturas" runat="server" class="form-control" placeholder="Número de Factura" Style="text-transform: uppercase"></asp:TextBox>
                                    </div>

                                    <div class="col-md-3">
                                        <asp:Button ID="btnFiltarModalBuscarFacturas" runat="server" Text="Buscar" class="btn btn-block btn-primary" OnClick="btnFiltarModalBuscarFacturas_Click" />
                                    </div>
                                </div>
                            </div>
                        </asp:Panel>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvFiltrarModalBuscarFacturas" runat="server" class="mGrid" 
                                        AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="true" PageSize="10" OnSelectedIndexChanged="dgvFiltrarModalBuscarFacturas_SelectedIndexChanged" OnPageIndexChanging="dgvFiltrarModalBuscarFacturas_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="N°" />
                                            <asp:BoundField DataField="IIDFACTURA" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IFECHAFACTURA" HeaderText="FECHA DE FACTURA" />
                                            <asp:BoundField DataField="ICLIENTE" HeaderText="CLIENTE" />
                                            <asp:BoundField DataField="IFACTURAEMITIDA" HeaderText="FACTURA" />
                                            <asp:BoundField DataField="ICLAVEACCESO" HeaderText="CLAVE DE ACCESO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccion_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
    <%--FIN MODAL--%>

    <%--INICIO DEL MODAL PARA REPORTE--%>

    <asp:Button ID="Button1" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Reporte" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="Button1"
        PopupControlID="pnlReporte" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlReporte" runat="server">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title">Previzualización del RIDE</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-10">
                                    <rsweb:ReportViewer ID="rptGenerarRIDE" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="400px" Width="900px">
                                        <LocalReport ReportEmbeddedResource="Solution_Encomiendas.Reportes.rptGenerarRIDE.rdlc">
                                        </LocalReport>
                                    </rsweb:ReportViewer>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnGenerarPDF" runat="server" Text="GENERAR PDF" class="btn btn btn-success" OnClick="btnGenerarPDF_Click" />
                        <asp:Button ID="btnCerrarModalReporte" runat="server" Text="Salir" class="btn btn btn-default" OnClick="btnCerrarModalReporte_Click" />
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DEL MODAL REPORTE--%>

    <%--PROGRESS--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="Sending">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <%--<br />
                    <input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />--%>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
