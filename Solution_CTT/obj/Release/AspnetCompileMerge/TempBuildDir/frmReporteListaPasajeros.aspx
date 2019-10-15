<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReporteListaPasajeros.aspx.cs" Inherits="Solution_CTT.frmReporteListaPasajeros" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtDate" runat="server" class="form-control"></asp:TextBox>
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
                                    OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="10">
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
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Reporte de Pasajeros</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-10">
                                    <rsweb:ReportViewer ID="rptListaPasajeros" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="400px" Width="900px">
                                    <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.RVPruebas.rdlc">
                                    </LocalReport>
                                    </rsweb:ReportViewer>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnImprimirReporte" runat="server" Text="Imprimir" class="btn btn btn-success" OnClick="btnImprimirReporte_Click" />
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
