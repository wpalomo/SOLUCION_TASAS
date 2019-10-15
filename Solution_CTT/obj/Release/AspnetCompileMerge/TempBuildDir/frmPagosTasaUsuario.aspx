<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPagosTasaUsuario.aspx.cs" Inherits="Solution_CTT.frmPagosTasaUsuario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">TASAS DE USUARIO</h3>
                                <div class="box-tools pull-right">
                                    <%--<div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>--%>
                                </div>
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
                                            <asp:Label ID="Label3" runat="server" Text="Fecha Final:"></asp:Label>
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control" AutoComplete="off"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaHasta_MaskedEditExtender" runat="server" BehaviorID="txtFechaHasta_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaHasta" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaHasta_CalendarExtender" runat="server" BehaviorID="txtFechaHasta_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label4" runat="server" Text="Filtrar por Estados"></asp:Label>
                                            <asp:DropDownList ID="cmbEstado" runat="server" class="form-control">
                                                <asp:ListItem>TODOS</asp:ListItem>
                                                <asp:ListItem>PAGADA</asp:ListItem>
                                                <asp:ListItem>PENDIENTE</asp:ListItem>
                                            </asp:DropDownList>
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label5" runat="server" Text="Filtrar por Jornadas"></asp:Label>
                                            <asp:DropDownList ID="cmbJornada" runat="server" class="form-control"></asp:DropDownList>
                                        </div>                                        
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:LinkButton ID="btnConsultar" runat="server" Text="" class="btn btn-warning" OnClick="btnConsultar_Click" ><i class="fa fa-search"> CONSULTAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" Text="" class="btn btn-danger" OnClick="btnLimpiar_Click" ><i class="fa fa-eraser"> LIMPIAR</i></asp:LinkButton>
                                </div>

                                <div class="form-group">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid"
                                    AutoGenerateColumns="False"
                                    EmptyDataText="No hay Registros o Coindicencias..!!"
                                    OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="7">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                        <asp:BoundField DataField="IIDJORNADA" HeaderText="ID JORNADA" />
                                        <asp:BoundField DataField="IFECHA" HeaderText="FECHA GENERACIÓN" />
                                        <asp:BoundField DataField="IJORNADA" HeaderText="JORNADA" />
                                        <asp:BoundField DataField="ICANTIDAD" HeaderText="CANTIDAD" />
                                        <asp:BoundField DataField="IVALOR" HeaderText="VALOR GENERADO" />
                                        <asp:BoundField DataField="IESTADOMOVIMIENTO" HeaderText="ESTADO ACTUAL" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER DETALLE">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnVer" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnVer_Click"><i class="fa fa-folder-open"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="PAGAR">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnPagar" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnPagar_Click"><i class="fa fa-dollar"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="IMPRIMIR PAGO">
                                        <ItemTemplate>
                                            <asp:LinkButton ID="lbtnImprimir" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnImprimir_Click"><i class="fa fa-print"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                                </div>

                                
                            </div>

                            <%--LEYENDA--%>
                            <div class="box-footer">
                                <%--<div class="box-header text-center">
                                    <h3 class="box-title">Totales Generados por Tasas de Usuario</h3>
                                </div>
                                <div class="row center-block">
                                    <div class="col-md-12 ">
                                        <div class="col-md-6">
                                            <asp:Label ID="lblSumaPagadas" runat="server" Text="Total Pagado Tasas de Usuario: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                        </div>
                                        <div class="col-md-6">
                                            <asp:Label ID="lblSumaPendientes" runat="server" Text="Total Pendiente Tasas de Usuario: 0.00 $" class="badge bg-lime"></asp:Label>
                                        </div>
                                    </div>
                                </div>  --%>                                 
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

    <%--MODAL DE FACTURAS EMITIDAS--%>

    <asp:Button ID="btnVerTasa" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Tasas" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnVerTasa" 
        PopupControlID="pnlDetalleTasa" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlDetalleTasa" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModalTasa" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalTasa_Click" />
                        <h4 class="modal-title" id="myModalLabel7">Tasas de Usuario Generadas</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">

                            </div>   
                        </div>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvTasa" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        AllowPaging="True" OnPageIndexChanging="dgvTasa_PageIndexChanging" PageSize="7">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                            <asp:BoundField DataField="IFECHA" HeaderText="FECHA GENERACIÓN" />
                                            <asp:BoundField DataField="IHORA" HeaderText="HORA GENERACIÓN" />
                                            <asp:BoundField DataField="ICANTIDAD" HeaderText="CANTIDAD" />
                                            <asp:BoundField DataField="IVALOR" HeaderText="VALOR" />
                                            <asp:BoundField DataField="ITASAUSUARIO" HeaderText="TASA DE USUARIO" />
                                            <asp:BoundField DataField="IESTADOMOVIMIENTO" HeaderText="ESTADO ACTUAL" />
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

    <%--FIN DE MODAL DE FACTURAS EMITIDAS--%>

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
