<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmDetalleTransaccionesTasa.aspx.cs" Inherits="Solution_CTT.frmDetalleTransaccionesTasa" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <section class="box-default">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-file-code-o"></i>
                                    <h3 class="box-title">Detalle de Transacciones de Tasas de Usuario por Rango de Fechas</h3>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-2 form-group">
                                            <label>Terminal</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtTerminal" runat="server" class="form-control input-sm" ToolTip="Ambiente Configurado" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <label>Ambiente</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtAmbiente" runat="server" class="form-control input-sm" ToolTip="Ambiente Configurado" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label>Fecha de Inicio</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtFechaInicial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaInicial_MaskedEditExtender" runat="server" BehaviorID="txtFechaInicial_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicial" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" BehaviorID="txtFechaInicial_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaInicial" />
                                                <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label>Fecha de Final</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtFechaFinal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaFinal_MaskedEditExtender" runat="server" BehaviorID="txtFechaFinal_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFinal" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" BehaviorID="txtFechaFinal_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaFinal" />
                                                <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>                                       
                                        
                                        <div class="col-md-2 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="btnExtraerInformacion" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para realizar la consulta" OnClick="btnExtraerInformacion_Click"><i class="fa fa-pencil-square-o"></i>  Extraer Información</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row">
                        <div class="col-md-8">
                            <div class="box box-success">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Registros Encontrados
                                        <asp:Label ID="lblRegistrosEncontrados" runat="server" Text=""></asp:Label></h3>
                                </div>

                                <div class="box-body">
                                    <div class="scrolling-table-container" runat="server" id="Scroll" visible="false">
                                        <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                            <Columns>
                                                <asp:BoundField DataField="fecha_creacion" HeaderText="Fecha" />
                                                <asp:BoundField DataField="cantidad" HeaderText="Total Tasas Emitidas" />
                                                <asp:BoundField DataField="usos" HeaderText="Total Tasas Usadas" />
                                                <asp:BoundField DataField="total" HeaderText="Valor Total" />
                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>

                                <div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblSuma" runat="server" Text="Total: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>
                            </div>
                        </div>

                        <div class="col-md-4">
                            <div class="box box-warning">
                                <div class="box-header with-border">
                                    <h3 class="box-title">Tasas de Usuario sin Sincronizar</h3>
                                </div>

                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-12 form-group">
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtTotalTasasSinSincronizar" runat="server" class="form-control input-sm" ReadOnly="true" BackColor="White" Font-Size="XX-Large" style="text-align:center;" ForeColor="Red" Font-Bold="true" Text="0"></asp:TextBox>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <div class="col-md-6 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="btnImprimir" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí para imprimir el reporte" OnClick="btnImprimir_Click"><i class="fa fa-print"></i>Imprimir</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-md-6 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="btnLimpiar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí para limpiar" OnClick="btnLimpiar_Click"><i class="fa fa-circle-o-notch"></i>Limpiar</asp:LinkButton>
                                        </div>
                                    </div>
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
                                    <button type="button" class="btn btn-default" data-dismiss="modal">Aceptar</button>
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
