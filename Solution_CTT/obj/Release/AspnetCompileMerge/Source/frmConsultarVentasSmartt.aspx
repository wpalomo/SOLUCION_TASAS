<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmConsultarVentasSmartt.aspx.cs" Inherits="Solution_CTT.frmConsultarVentasSmartt" %>
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
                                <h3 class="box-title">Consulta de Ventas - SMARTT</h3>
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
                                    <asp:Panel ID="pnlRegistros" runat="server" Visible="true">
                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label18" runat="server" Text="Buscar por:"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:RadioButton ID="rdbFechas" Checked="true" class="form-control input-sm" Text="&nbsp&nbspRango de Fechas" runat="server" GroupName="buscarViajesSmartt" OnCheckedChanged="rdbFechas_CheckedChanged" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="Fecha Inicial: *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaInicial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                        <ajaxToolkit:MaskedEditExtender ID="txtFechaInicial_MaskedEditExtender" runat="server" BehaviorID="txtFechaInicial_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicial" />
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" BehaviorID="txtFechaInicial_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaInicial" />
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label10" runat="server" Text="Fecha Final: *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaFinal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                        <ajaxToolkit:MaskedEditExtender ID="txtFechaFinal_MaskedEditExtender" runat="server" BehaviorID="txtFechaFinal_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFinal" />
                                                        <ajaxToolkit:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" BehaviorID="txtFechaFinal_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaFinal" />
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="Buscar por:"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:RadioButton ID="rdbNumeroDocumento" class="form-control input-sm" Text="&nbsp&nbspNúmero de Documento" runat="server" GroupName="buscarViajesSmartt" OnCheckedChanged="rdbNumeroDocumento_CheckedChanged" AutoPostBack="true" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Número de Documento: *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNumeroDocumento" runat="server" ReadOnly="true" class="form-control input-sm" placeholder="Favor ingrese el número de documento" AutoComplete="off" ToolTip="Numero Documento"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-paper-plane-o"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="Clic aquí"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:Button ID="btnConsultar" runat="server" Text="Consultar" class="btn btn btn-info" OnClick="btnConsultar_Click" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-1">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Clic aquí"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn btn-warning" OnClick="btnLimpiar_Click" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" >
                                                <Columns>
                                                    <asp:BoundField DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="fecha_hora_venta" HeaderText="FECHA HORA VENTA" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="numero_documento_tasa" HeaderText="N° DOCUMENTO TASA" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="total_tasas" HeaderText="VALOR TASAS" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="viaje" HeaderText="N° VIAJE" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:BoundField DataField="estado_nombre" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
                                                    <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-info"><i class="fa fa-bus"></i></asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                                        </div>

                                    </asp:Panel>

                                    <%--CUARTA FILA--%>
                                    <%--<div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                            <Columns>
                                                <asp:BoundField DataField="disco" HeaderText="DISCO" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="conductor" HeaderText="CONDUCTOR" />
                                                <asp:BoundField DataField="capacidad" HeaderText="CAPACIDAD" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="anio_fabricacion" HeaderText="AÑO FABRICACION" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="placa" HeaderText="PLACA" />
                                                <asp:BoundField DataField="marca_nombre" HeaderText="MARCA" />
                                                <asp:BoundField DataField="fecha_emision_matricula" HeaderText="FECHA EMI. MATRÍCULA" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="fecha_vencimiento_matricula" HeaderText="FECHA CAD. MATRÍCULA" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div> --%>                       
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="form-group">
                                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" class="btn btn btn-warning" Visible="false" OnClick="btnAnterior_Click" />
                                    <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" class="btn btn btn-success" Visible="false" OnClick="btnSiguiente_Click" />
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
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, confirmar" class="btn btn-success" data-dismiss="modal" UseSubmitBehavior="false" />
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

    <%--MODAL DE ITINERARIOS--%>

    <asp:Button ID="btnVendidos" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Vendidos" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnVendidos" 
        PopupControlID="pnlVendidos" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlVendidos" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
            <div class="modal-dialog modal-lg">
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalVendidos" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalVendidos_Click" />
                        <h4 class="modal-title" id="myModalLabel17">Boletos Vendidos</h4>
                    </div>
                    <div class="modal-body">                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvVendidos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnPageIndexChanging="dgvVendidos_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="id" HeaderText="ID" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="asiento" HeaderText="ASIENTO" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="identificacion" HeaderText="IDENTIFICACIÓN" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="nombre" HeaderText="PASAJERO" />
                                            <asp:BoundField DataField="valor" HeaderText="VALOR PASAJE" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="tasa" HeaderText="VALOR" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="estado_nombre" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />                                            
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>                                 
                            </div>                                           
                        </div>
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
