<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReimprimirCaja.aspx.cs" Inherits="Solution_CTT.frmReimprimirCaja" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-md-8">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Itinerarios Cerrados</h3>
                                <div class="box-tools pull-right">
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="id_ctt_programacion" HeaderText="No." />
                                        <asp:BoundField DataField="hora_salida" HeaderText="HORA VIAJE" />
                                        <asp:BoundField DataField="fecha_grid" HeaderText="FECHA VIAJE" />
                                        <asp:BoundField DataField="vehiculo" HeaderText="VEHICULO" />
                                        <asp:BoundField DataField="ruta" HeaderText="RUTA" />
                                        <asp:BoundField DataField="asientos_ocupados" HeaderText="PASAJEROS" />
                                        <asp:BoundField DataField="tipo_viaje" HeaderText="VIAJE" />
                                        <asp:BoundField DataField="valor" HeaderText="TOTAL" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                            <div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblSuma" runat="server" Text="Total Cobrado: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>

                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Itinerarios Vigentes</h3>
                                <div class="box-tools pull-right">
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatosVigentes" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="id_ctt_programacion" HeaderText="No." />
                                        <asp:BoundField DataField="hora_salida" HeaderText="HORA VIAJE" />
                                        <asp:BoundField DataField="fecha_grid" HeaderText="FECHA VIAJE" />
                                        <asp:BoundField DataField="vehiculo" HeaderText="VEHICULO" />
                                        <asp:BoundField DataField="ruta" HeaderText="RUTA" />
                                        <asp:BoundField DataField="asientos_ocupados" HeaderText="PASAJEROS" />
                                        <asp:BoundField DataField="tipo_viaje" HeaderText="VIAJE" />
                                        <asp:BoundField DataField="valor" HeaderText="TOTAL" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                            <div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblSumaVigentes" runat="server" Text="Total Cobrado: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>

                            <%--PAGOS ADMINISTRATIVOS--%>
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_PAGOS_ADMINISTRATIVOS %></h3>

                                <div class="box-tools pull-right">                                    
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvPagosAdministrativos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="descripcion" HeaderText="DESCRIPCIÓN DEL PAGO" />
                                        <asp:BoundField DataField="valor" HeaderText="VALOR RECAUDADO" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                            <%--<div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblSumaAdministrativo" runat="server" Text="Total Cobrado Administrativo: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>--%>

                            <%--PAGOS ATRASADOS PAGADOS--%>
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title">Abonos Parciales - Pagos Pendientes</h3>

                                <div class="box-tools pull-right">                                    
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvPagosAtrasados" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                        <asp:BoundField DataField="IFECHA" HeaderText="FECHA DE VIAJE" />
                                        <asp:BoundField DataField="IHORA" HeaderText="HORA DE VIAJE" />
                                        <asp:BoundField DataField="IVEHICULO" HeaderText="VEHÍCULO" />
                                        <asp:BoundField DataField="IVALOR" HeaderText="VALOR RECAUDADO" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                            <div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblPagosAdministrativos" runat="server" Text="Total Pagos Administrativos: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>

                            <%--VIAJES ACTIVOS CREADOS--%>
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title">Viajes Activos Cobrados</h3>

                                <div class="box-tools pull-right">                                    
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvViajesActivos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                        <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA DE VIAJE" />
                                        <asp:BoundField DataField="IHORAVIAJE" HeaderText="HORA DE VIAJE" />
                                        <asp:BoundField DataField="IRUTA" HeaderText="RUTA" />
                                        <asp:BoundField DataField="ICANTIDAD" HeaderText="CANTIDAD" />
                                        <asp:BoundField DataField="IVALOR" HeaderText="VALOR" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                            <div class="box-footer">
                                <span class="pull-right">
                                    <asp:Label ID="lblTotalViajesActivos" runat="server" Text="Total Viajes Activos: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                </span>
                            </div>
                        </div>
                        <!-- /.box -->
                    </div>

                    <%--REGISTER--%>
                    <div class="col-md-4">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATOS_CORTE %></h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>
                            <%--FORM--%>
                            <div class="box-body">
                                <div class="register-box-body">
                                    <div class="row">
                                        <div class="col-md-offset-1 col-md-10">
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtFechaApertura" runat="server" class="form-control pull-right" autocomplete="off" placeholder="Buscar"></asp:TextBox>
                                                <%--<ajaxToolkit:MaskedEditExtender ID="txtFechaApertura_MaskedEditExtender" runat="server" BehaviorID="txtFechaApertura_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaApertura" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaApertura_CalendarExtender" runat="server" BehaviorID="txtFechaApertura_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaApertura" />--%>
                                            </div>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtHoraApertura" runat="server" CssClass="form-control" placeholder="HORA APERTURA" ReadOnly="true" BackColor="White" ></asp:TextBox>
                                            </div>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtOficinista" runat="server" CssClass="form-control" placeholder="OFICINISTA" ReadOnly="true" BackColor="White" ></asp:TextBox>
                                            </div>

                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtSaldoFinal" runat="server" CssClass="form-control" placeholder="Efectivo a registrar" Text="0.00" BackColor="White" ></asp:TextBox>
                                            </div>

                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Button ID="btnCerrarCaja" runat="server" Text="Cerrar Caja y Enviar Reporte" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnCerrarCaja_Click" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Button ID="btnImprimir" runat="server" Text="Imprimir Reporte" class="btn btn-sm btn-warning btn-block pull-right" OnClick="btnImprimir_Click" />
                                                    </div>
                                                </div>
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Button ID="btnEnviarMail" runat="server" visible="false" Text="Enviar Reporte" class="btn btn-sm btn-danger btn-block pull-right" OnClick="btnEnviarMail_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <br />
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

                <%--MODAL PARA CERRAR EL VIAJE--%>
                <div class="modal fade" id="QuestionModalCierre" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label11" runat="server" Text="Información."></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label12" runat="server" Text="¿Está seguro que desea realizar el cierre de caja?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNoCerrar" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAceptarCerrar" runat="server" Text="Sí, confirmar" class="btn btn-info" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptarCerrar_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL PARA CERRAR EL VIAJE--%>

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
