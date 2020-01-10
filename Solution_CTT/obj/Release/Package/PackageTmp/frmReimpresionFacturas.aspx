<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReimpresionFacturas.aspx.cs" Inherits="Solution_CTT.frmReimpresionFacturas" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <asp:Panel ID="pnlGrid" runat="server">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                                    <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <%--<asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtDate" runat="server" class="form-control pull-right" placeholder="dd/MM/yyyy"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server" BehaviorID="txtDate_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate" />
                                            <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" BehaviorID="txtDate_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="10" OnRowDataBound="dgvDatos_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID" />
                                            <asp:BoundField DataField="INUMEROVIAJE" HeaderText="No. VIAJE" />
                                            <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" />
                                            <asp:BoundField DataField="IVEHICULO" HeaderText="TRANSPORTE" />
                                            <asp:BoundField DataField="IRUTA" HeaderText="RUTA" />
                                            <asp:BoundField DataField="IHORASALIDA" HeaderText="SALIDA" />
                                            <asp:BoundField DataField="IASIENTOSOCUPADOS" HeaderText="ASIENTOS OCUP." />
                                            <asp:BoundField DataField="ITIPOVIAJE" HeaderText="TIPO DE VIAJE" />
                                            <asp:BoundField DataField="IESTADOVIAJE" HeaderText="ESTADO" />
                                            <asp:BoundField DataField="ICHOFER" HeaderText="CHOFER" />
                                            <asp:BoundField DataField="IASISTENTE" HeaderText="ASISTENTE" />
                                            <asp:BoundField DataField="IANDEN" HeaderText="ANDEN" />
                                            <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDREEMPLAZO" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-success"><i class="fa fa-bus"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>                                
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Viajes Extras</h3>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatosExtras" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        OnSelectedIndexChanged="dgvDatosExtras_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatosExtras_PageIndexChanging" PageSize="7" OnRowDataBound="dgvDatosExtras_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID" />
                                            <asp:BoundField DataField="INUMEROVIAJE" HeaderText="No. VIAJE" />
                                            <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" />
                                            <asp:BoundField DataField="IVEHICULO" HeaderText="TRANSPORTE" />
                                            <asp:BoundField DataField="IRUTA" HeaderText="RUTA" />
                                            <asp:BoundField DataField="IHORASALIDA" HeaderText="SALIDA" />
                                            <asp:BoundField DataField="IASIENTOSOCUPADOS" HeaderText="ASIENTOS OCUP." />
                                            <asp:BoundField DataField="ITIPOVIAJE" HeaderText="TIPO DE VIAJE" />
                                            <asp:BoundField DataField="IESTADOVIAJE" HeaderText="ESTADO" />
                                            <asp:BoundField DataField="ICHOFER" HeaderText="CHOFER" />
                                            <asp:BoundField DataField="IASISTENTE" HeaderText="ASISTENTE" />
                                            <asp:BoundField DataField="IANDEN" HeaderText="ANDEN" />
                                            <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDREEMPLAZO" HeaderText="ID" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditarExtra" runat="server" CommandName="Select" class="btn btn-xs btn-success"><i class="fa fa-bus"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>

                </asp:Panel>

                <asp:Panel ID="pnlVendidos" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">
                                        <asp:Label ID="lblDetalleBus" runat="server" Text=""></asp:Label></h3>
                                    </h3>
                                    <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <asp:TextBox ID="txtBuscarVendido" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <button id="btnFiltrarVendido" runat="server" class="btn btn-default"><i class="fa fa-search"></i></button>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvVendidos" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        OnSelectedIndexChanged="dgvVendidos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvVendidos_PageIndexChanging" PageSize="10">
                                        <Columns>
                                            <asp:BoundField DataField="IIDPEDIDO" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID PROGRAMACION" />
                                            <asp:BoundField DataField="IIDFACTURA" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IIDPERSONA" HeaderText="ID PERSONA" />
                                            <asp:BoundField DataField="IIDENTIFICACION" HeaderText="IDENTIFICACION" />
                                            <asp:BoundField DataField="ICLIENTE" HeaderText="CLIENTE" />
                                            <asp:BoundField DataField="IFACTURA" HeaderText="No. FACTURA" />
                                            <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA DE VIAJE" />
                                            <asp:BoundField DataField="IITIPOCOMPROBANTE" HeaderText="TIPO COMPROBANTE" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditarFactura" runat="server" CommandName="Select" class="btn btn-xs btn-warning" ToolTip="Clic aquí para reimprimir la factura."><i class="fa fa-print"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                                <div class="box-footer">
                                    <asp:Button ID="btnCancelarGrid" runat="server" Text="Cancelar" data-backdrop="false" class="btn btn btn-danger" OnClick="btnCancelarGrid_Click" />
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>



                <div class="modal fade" id="myModal1" data-backdrop="static" data-keyboard="true">
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
                                            <asp:Label ID="lblMensajeModal" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal" />
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click" />
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Modal MENSAJE-->
                <div id="dialogCancelNotification" class="modal">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <a href="#" data-dismiss="modal" aria-hidden="true" class="close">X</a>
                                <h3 class="alert alert-info"><span class="glyphicon glyphicon-warning-sign"></span>Información</h3>
                            </div>
                            <div class="modal-body">
                                <p>
                                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                                </p>
                            </div>

                            <div class="col-md-offset-8">
                                <%--<button type="button" class="btn btn-success" id="btnYesCancelNotification"><span class="glyphicon glyphicon-ok"></span>Sí</button>--%>
                                <button type="submit" class="btn btn-success" data-dismiss="modal" id="btnNoCancelNotification"><span class="glyphicon glyphicon-remove"></span>OK</button>
                            </div>
                            <br />
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
                                    <asp:Label ID="Label5" runat="server" Text="Información"></asp:Label>
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

    <asp:Button ID="btnInicial" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial"
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Detalle de Boletos Vendidos</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:Label ID="Label3" runat="server" Text="Fecha Factura:"></asp:Label>
                                    <asp:Label ID="lblFechaFactura" runat="server" Text=""></asp:Label>
                                </div>

                                <div class="col-md-8">
                                    <asp:Label ID="Label1" runat="server" Text="C.I. / RUC:"></asp:Label>
                                    <asp:Label ID="lblIdentificacion" runat="server" Text=""></asp:Label>
                                </div>

                                <div class="col-md-8">
                                    <asp:Label ID="Label2" runat="server" Text="Razón Social:"></asp:Label>
                                    <asp:Label ID="lblRazonSocial" runat="server" Text=""></asp:Label>
                                </div>
                                <div class="col-md-8">
                                    <asp:Label ID="Label4" runat="server" Text="Clave Acceso:"></asp:Label>
                                    <asp:Label ID="lblClaveAcceso" runat="server" Text=""></asp:Label>
                                </div>
                            </div>   
                        </div>
                        <div class="form-group"></div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvDetalle" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" >
                                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="chkSeleccionar" runat="server" CommandName="Select" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="IIDPEDIDO" HeaderText="ID PEDIDO" />
                                            <asp:BoundField DataField="IIDDETPEDIDO" HeaderText="ID DET PEDIDO" />
                                            <asp:BoundField DataField="IIDPASAJERO" HeaderText="ID PASAJERO" />
                                            <asp:BoundField DataField="IIDASIENTO" HeaderText="ID ASIENTO" />
                                            <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID PUEBLO" />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID PROGRAMACION" />
                                            <asp:BoundField DataField="IIDPRODUCTO" HeaderText="ID PRODUCTO" />
                                            <asp:BoundField DataField="IIDTIPOCLIENTE" HeaderText="ID TIPO CLIENTE" />
                                            <asp:BoundField DataField="INUMEROASIENTO" HeaderText="No. ASIENTO" />
                                            <asp:BoundField DataField="IIDENTIFICACIONPASAJERO" HeaderText="IDENTIFICACIÓN" />
                                            <asp:BoundField DataField="INOMBREPASAJERO" HeaderText="NOMBRE DEL PASAJERO" />
                                            <asp:BoundField DataField="INOMBREPRODUCTO" HeaderText="DESCRIPCIÓN" />
                                            <asp:BoundField DataField="ITIPOCLIENTE" HeaderText="TIPO CLIENTE" />
                                            <asp:BoundField DataField="IPRECIOUNITARIO" HeaderText="PRECIO UNITARIO" />
                                            <asp:BoundField DataField="IDESCUENTO" HeaderText="DESCUENTO" />
                                            <asp:BoundField DataField="IIVA" HeaderText="IVA" />
                                            <asp:BoundField DataField="ITOTAL" HeaderText="VALOR" />
                                        </Columns>
                                        <FooterStyle BackColor="Tan" />
                                        <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" />
                                        <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                        <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                        <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                        <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                        <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                    </asp:GridView>
                                    <asp:Label ID="lblAdvertencia" runat="server" ForeColor="Red" Text=""></asp:Label>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="btnProcesar" runat="server" Text="Procesar Registros" class="btn btn-success" data-dismiss="modal" OnClick="btnProcesar_Click" />
                            <asp:Button ID="btnCancelarModal" runat="server" Text="Cancelar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnCancelarModal_Click" />
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

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
