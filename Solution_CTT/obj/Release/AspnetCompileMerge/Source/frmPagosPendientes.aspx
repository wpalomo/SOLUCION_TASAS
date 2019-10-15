<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPagosPendientes.aspx.cs" Inherits="Solution_CTT.frmPagosPendientes" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_PAGOS_PENDIENTES %></h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="row">                                
                                        <div class="col-md-3">
                                            <asp:Label ID="Label1" runat="server" Text="Fecha Inicial:"></asp:Label>
                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control" AutoComplete="off"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaDesde_MaskedEditExtender" runat="server" BehaviorID="txtFechaDesde_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaDesde" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaDesde_CalendarExtender" runat="server" BehaviorID="txtFechaDesde_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaDesde" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label2" runat="server" Text="Fecha Final:"></asp:Label>                                            
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control" AutoComplete="off"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaHasta_MaskedEditExtender" runat="server" BehaviorID="txtFechaHasta_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaHasta" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaHasta_CalendarExtender" runat="server" BehaviorID="txtFechaHasta_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label3" runat="server" Text="Transporte:"></asp:Label>
                                            <asp:DropDownList ID="cmbVehiculos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label4" runat="server" Text="Propietario:"></asp:Label>
                                            <asp:DropDownList ID="cmbPropietario" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:LinkButton ID="btnConsultar" runat="server" Text="" class="btn btn-warning" OnClick="btnConsultar_Click" ><i class="fa fa-search"> CONSULTAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" Text="" class="btn btn-danger" OnClick="btnLimpiar_Click" ><i class="fa fa-eraser"> LIMPIAR</i></asp:LinkButton>
                                </div>

                                <div class="form-group">                                    
                                    <asp:GridView ID="dgvDetalle" runat="server" class="mGrid" AllowPaging="True" 
                                        AutoGenerateColumns="False" PageSize="10" EmptyDataText="No hay Registros o Coindicencias..!!" 
                                        OnPageIndexChanging="dgvDetalle_PageIndexChanging" OnSelectedIndexChanged="dgvDetalle_SelectedIndexChanged" >
                                        <Columns>
                                            <asp:BoundField HeaderText="IDPedido" DataField="IIDPEDIDO" />
                                            <asp:BoundField HeaderText="No. Viaje" DataField="INUMEROVIAJE" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField HeaderText="Fecha Viaje" DataField="IFECHAVIAJE" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IHORAVIAJE" HeaderText="Hora Viaje" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IVEHICULO" HeaderText="Vehículo" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IPROPIETARIO" HeaderText="Propietario" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="IRUTA" HeaderText="Ruta" ItemStyle-HorizontalAlign="Left" />
                                            <asp:BoundField DataField="IVALOR" HeaderText="Valor Abonado" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="IPRECIO" HeaderText="Valor Debido" ItemStyle-HorizontalAlign="Right" />
                                            <asp:BoundField DataField="IIDPERSONA" HeaderText="Id Persona" />
                                            <asp:BoundField DataField="IPRECIOUNITARIO" HeaderText="Precio Unitario" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Observación" >
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtObservacion" runat="server" class="form-control input-sm" placeholder="Observaciones" Text=""></asp:TextBox>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Pagar" >
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnEdit_Click" ToolTip="Clic aquí para cobrar"><i class="fa fa-dollar"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                       <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView> 
                                </div>

                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                         <%--<asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="Ingrese parámetros de búsqueda...!!" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="7">
                             <Columns>
                                <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                <asp:BoundField DataField="IIDENTIFICACION" HeaderText="IDENTIFICACIÓN" />
                                <asp:BoundField DataField="IPASAJERO" HeaderText="PASAJERO" />
                                <asp:BoundField DataField="ITIPOCLIENTE" HeaderText="TIPO CLIENTE" />
                                <asp:BoundField DataField="ICANTIDAD" HeaderText="CANTIDAD" />
                                <asp:BoundField DataField="IVALOR" HeaderText="VALOR" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>--%>
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

                <%--MODAL DE CONFIRMACION DE ACCION--%>
                <div class="modal fade" id="modalConfirmacion" data-backdrop="static" data-keyboard="true">
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
                                            <asp:Label ID="lblMensajeConfirmacion" runat="server" Text=""></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnAceptar" runat="server" Text="Sí, confirmar" class="btn btn-success" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptar_Click"/>
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL DE CONFIRMACION DE ACCION--%>

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
