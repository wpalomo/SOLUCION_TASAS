<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReporteViajesActivos.aspx.cs" Inherits="Solution_CTT.frmReporteViajesActivos" %>
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
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="row">                                
                                        <div class="col-md-3">
                                            <asp:Label ID="Label1" runat="server" Text="Fecha Inicial:"></asp:Label>
                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaDesde_MaskedEditExtender" runat="server" BehaviorID="txtFechaDesde_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaDesde" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaDesde_CalendarExtender" runat="server" BehaviorID="txtFechaDesde_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaDesde" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label2" runat="server" Text="Fecha Final:"></asp:Label>
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaHasta_MaskedEditExtender" runat="server" BehaviorID="txtFechaHasta_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaHasta" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaHasta_CalendarExtender" runat="server" BehaviorID="txtFechaHasta_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label3" runat="server" Text="Transporte"></asp:Label>
                                            <asp:DropDownList ID="cmbVehiculos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label4" runat="server" Text="Usuarios"></asp:Label>
                                            <asp:DropDownList ID="cmbUsuarios" runat="server" class="form-control"></asp:DropDownList>
                                        </div>                                  
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:LinkButton ID="btnConsultar" runat="server" Text="" class="btn btn-warning" OnClick="btnConsultar_Click" ><i class="fa fa-search"> CONSULTAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" Text="" class="btn btn-danger" OnClick="btnLimpiar_Click" ><i class="fa fa-eraser"> LIMPIAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnImprimir" runat="server" Text="" class="btn btn-success" OnClick="btnImprimir_Click" Visible="false" ><i class="fa fa-print"> IMPRIMIR</i></asp:LinkButton>                                    
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                         <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="Ingrese parámetros de búsqueda...!!" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="7">
                             <Columns>
                                <asp:BoundField DataField="INUMERO" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="INUMEROVIAJE" HeaderText="NO. VIAJE" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IVEHICULO" HeaderText="DISCO - PLACA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IHORASALIDA" HeaderText="HORA SALIDA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ITIPOVIAJE" HeaderText="TIPO DE VIAJE" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IUSUARIO" HeaderText="USUARIO INGRESO" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="ICANTIDAD" HeaderText="TOTAL PASAJEROS" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="IVALOR" HeaderText="TOTAL INGRESOS" ItemStyle-HorizontalAlign="Right" />
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </div>

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label6" runat="server" Text="Información"></asp:Label>
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

    <%--INICIO DEL MODAL PARA REPORTE--%>

    <asp:Button ID="btnInicial" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Reporte" runat="server"
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
                        <h4 class="modal-title" id="myModalLabel5">Reporte de Pasajeros</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-10">
                                    <rsweb:ReportViewer ID="rptViajesActivos" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="400px" Width="800px">
                                    <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.rptViajesActivos.rdlc">
                                    </LocalReport>
                                    </rsweb:ReportViewer>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnImprimirReporte" runat="server" Text="Imprimir" class="btn btn btn-success" />
                        <asp:Button ID="btnCerrarModalReporte" runat="server" Text="Salir" class="btn btn btn-warning" OnClick="btnCerrarModalReporte_Click" />                        
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DEL MODAL REPORTE--%>

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
