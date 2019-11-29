<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmIngresoPagosPendientes.aspx.cs" Inherits="Solution_CTT.frmIngresoPagosPendientes" %>
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
                                        <div class="col-md-2">
                                            <asp:Label ID="Label6" runat="server" Text="Vehículo"></asp:Label>
                                            <asp:DropDownList ID="cmbVehiculos" runat="server" class="form-control"></asp:DropDownList>
                                        </div>
                                        <div class="col-md-2">
                                            <asp:Label ID="Label1" runat="server" Text="Fecha Frecuencia:"></asp:Label>
                                            <asp:TextBox ID="txtFecha" runat="server" class="form-control" AutoComplete="off"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFecha_MaskedEditExtender" runat="server" BehaviorID="txtFecha_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFecha" />
                                            <ajaxToolkit:CalendarExtender ID="txtFecha_CalendarExtender" runat="server" BehaviorID="txtFecha_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFecha" />
                                        </div>                                        
                                        <div class="col-md-2">
                                            <asp:Label ID="Label3" runat="server" Text="Valor a Ingresar"></asp:Label>
                                            <asp:TextBox ID="txtValor" runat="server" class="form-control" Text="0.00" AutoComplete="off" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                        </div>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Seleccione el itinerario"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtItinerario" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Itinerario *" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalItinerario" runat="server" Text="" OnClick="btnAbrirModalItinerario_Click" tooltip="Seleccionar el itinerario"><i class="fa fa-send"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                        </div>                                        
                                    </div>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="Seleccione el chofer"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtChofer" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Chofer *" BackColor="White"></asp:TextBox>
                                                    <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalChofer" runat="server" Text="" OnClick="btnAbrirModalChofer_Click" tooltip="Seleccionar Chofer"><i class="fa fa-male"></i></asp:LinkButton></span>
                                                </div>    
                                            </div>
                                        </div>

                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:Label ID="Label5" runat="server" Text="Observaciones"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control input-sm" placeholder="Observaciones*" BackColor="White" MaxLength="500" Style="text-transform: uppercase"></asp:TextBox>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                </div>                                                
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:LinkButton ID="btnGuardar" runat="server" Text="" class="btn btn-warning" OnClick="btnGuardar_Click" ><i class="fa fa-save"> GUARDAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" Text="" class="btn btn-danger" OnClick="btnLimpiar_Click" ><i class="fa fa-eraser"> LIMPIAR</i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvDetalle" runat="server" class="mGrid" AllowPaging="True" AutoGenerateColumns="False" PageSize="10" EmptyDataText="No hay Registros o Coindicencias..!!" OnPageIndexChanging="dgvDetalle_PageIndexChanging" OnRowDataBound="dgvDetalle_RowDataBound">
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
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                     </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

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
                                            <asp:Label ID="Label12" runat="server" Text="¿Está seguro que desea ingresar el pago?"></asp:Label>
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

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label4" runat="server" Text="Información"></asp:Label>
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

    <%--MODAL DE ITINERARIOS--%>

    <asp:Button ID="btnItinerarios" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Itinerarios" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnItinerarios" 
        PopupControlID="pnlItinerarios" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlItinerarios" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel5" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalItinerario" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalItinerario_Click" />
                        <h4 class="modal-title" id="myModalLabel17">Registros</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <asp:Panel ID="Panel3" runat="server" DefaultButton="btnFiltarItinearios">
                                    <div class="col-md-12">
                                        <div class="col-md-8">
                                            <asp:TextBox ID="txtFiltrarItinerarios" runat="server" class="form-control input-sm" placeholder="BÚSQUEDA DE ITINERARIOS" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                        </div>
                                        <div class="col-md-4">
                                            <asp:Button ID="btnFiltarItinearios" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltarItinearios_Click" />
                                        </div>
                                    </div>
                                </asp:Panel>
                            </div>

                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvItinerarios" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="12" OnSelectedIndexChanged="dgvItinerarios_SelectedIndexChanged" OnPageIndexChanging="dgvItinerarios_PageIndexChanging" OnRowDataBound="dgvItinerarios_RowDataBound" >
                                        <Columns>
                                            <asp:BoundField DataField="IIDITINERARIO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDRUTA" HeaderText="ID RUTA" />
                                            <asp:BoundField DataField="IIDHORARIO" HeaderText="ID HORARIO" />
                                            <%--<asp:BoundField DataField="IIDTIPOVIAJE" HeaderText="ID TIPO VIAJE" />--%>
                                            <asp:BoundField DataField="ICODIGO" HeaderText="CÓDIGO" />
                                            <asp:BoundField DataField="IDESCRIPCION" HeaderText="DESCRIPCIÓN" />
                                            <asp:BoundField DataField="IDESTINO" HeaderText="DESTINO" />
                                            <asp:BoundField DataField="IHORASALIDA" HeaderText="HORA SALIDA" />
                                            <%--<asp:BoundField DataField="ITIPOVIAJE" HeaderText="VIAJE" />--%>
                                            <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionItinerario" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccionItinerario_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                                                  
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DE MODAL DE VEHICULOS--%>

    <%--MODAL DE PERSONAS--%>

    <asp:Button ID="btnAsistentesI" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_AsistentesChofer" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnAsistentesI" 
        PopupControlID="pnlPersonas" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlPersonas" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalPersonas" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalPersonas_Click" />
                        <h4 class="modal-title" id="myModalLabel6">Registros</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltarChoferAsistente">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarChoferAsistente" runat="server" class="form-control input-sm" placeholder="BÚSQUEDA" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltarChoferAsistente" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltarChoferAsistente_Click" />
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvAsistentesChofer" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="10" OnSelectedIndexChanged="dgvAsistentesChofer_SelectedIndexChanged" OnPageIndexChanging="dgvAsistentesChofer_PageIndexChanging" OnRowDataBound="dgvAsistentesChofer_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No."  />
                                            <asp:BoundField DataField="IIDREGISTRO" HeaderText="ID ASISTENTE" />
                                            <asp:BoundField DataField="IDESCRIPCION" HeaderText="DESCRIPCIÓN" />
                                            <asp:BoundField DataField="ICODIGO" HeaderText="CÓDIGO" />
                                            <asp:BoundField DataField="INOMBRE" HeaderText="NOMBRE COMPLETO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionAsistenteChofer" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccionAsistenteChofer_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                                                  
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

    <%--FIN DE MODAL DE PERSONAS--%>

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
