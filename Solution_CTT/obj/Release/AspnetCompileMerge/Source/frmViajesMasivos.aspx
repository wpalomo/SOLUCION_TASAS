<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmViajesMasivos.aspx.cs" Inherits="Solution_CTT.frmViajesMasivos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                
                <asp:Panel ID="pnlGrid" runat="server">
                    
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                    <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <%--<asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>--%>
                                            <asp:TextBox ID="txtFecha" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                                            <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" BehaviorID="txtFecha_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFecha" />
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                         PageSize="7" OnRowDataBound="dgvDatos_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="numero" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="id_ctt_itinerario" HeaderText="ID" />
                                            <asp:BoundField DataField="hora_salida" HeaderText="HORA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="id_ctt_tipo_servicio" HeaderText="ID" />
                                            <asp:TemplateField HeaderText="VEHÍCULO">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="cmbListarVehiculo" runat="server" class="form-control input-sm">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="CHOFER">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="cmbListarChoferes" runat="server" class="form-control input-sm">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:TemplateField HeaderText="ASISTENTE">
                                                <ItemTemplate>
                                                    <asp:DropDownList ID="cmbListarAsistentes" runat="server" class="form-control input-sm">
                                                    </asp:DropDownList>
                                                </ItemTemplate>
                                            </asp:TemplateField>                                            
                                            <asp:templatefield headertext="anden">
                                                <itemtemplate>
                                                    <asp:dropdownlist id="cmblistarandenes" runat="server" class="form-control input-sm">
                                                    </asp:dropdownlist>
                                                </itemtemplate>
                                            </asp:templatefield>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                                <div class="box-footer">
                                    <asp:Button ID="btnGenerar" runat="server" Text="Crear Viajes Masivos" data-backdrop="false" class="btn btn btn-success" OnClick="btnGenerar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Regresar" class="btn btn btn-danger" OnClick="btnCancelar_Click" />
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
                <div class="modal fade" id="QuestionModalConfirmar" data-backdrop="static" data-keyboard="true">
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
                                            <asp:Label ID="Label12" runat="server" Text="¿Está seguro que desea crear los registros?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNoCerrar" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAceptarCerrar" runat="server" Text="Sí, confirmar" class="btn btn-info" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptarCrear_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL PARA CERRAR EL VIAJE--%>

                </asp:Panel>
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
