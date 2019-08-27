<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmCrearViajeExtra.aspx.cs" Inherits="Solution_CTT.frmCrearViajeExtra" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">

                <%--INICIO DEL PANEL DE GRID--%>
                <asp:Panel ID="pnlGrid" runat="server">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="col-md-2">
                                <asp:LinkButton ID="lbtnNuevo" runat="server" class="btn btn-block btn-danger" OnClick="lbtnNuevo_Click"><span class="fa fa-plus"></span> Nuevo</asp:LinkButton>
                            </div>
                            <div class="col-md-3">
                                <asp:LinkButton ID="lbtnVentas" runat="server" class="btn btn-block btn-info" OnClick="lbtnVentas_Click"><span class="fa fa-send"></span> Módulo de Ventas</asp:LinkButton>
                            </div>
                        </div>
                    </div>

                    <br />

                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                    <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <asp:TextBox ID="txtDate" runat="server" class="form-control pull-right"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server" BehaviorID="txtDate_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate" />
                                            <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" BehaviorID="txtDate_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>

                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID" />
                                            <asp:BoundField DataField="INUMEROVIAJE" HeaderText="VIAJE" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ITRANSPORTE" HeaderText="TRANSPORTE" />
                                            <asp:BoundField DataField="IRUTADESTINO" HeaderText="RUTA DESTINO" />
                                            <asp:BoundField DataField="IDIA" HeaderText="DÍA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IHORASALIDA" HeaderText="HORA SALIDA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="INOMBRESERVICIO" HeaderText="TIPO VIAJE" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IESTADOVIAJE" HeaderText="E" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDCHOFER" HeaderText="ID CHOFER" />
                                            <asp:BoundField DataField="IIDASISTENTE" HeaderText="ID ASISTENTE" />
                                            <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID VEHICULO" />
                                            <asp:BoundField DataField="IIDHORARIO" HeaderText="ID HORARIO" />
                                            <asp:BoundField DataField="IIDANDEN" HeaderText="ID ANDEN" />
                                            <asp:BoundField DataField="IIDTIPOSERVICIO" HeaderText="ID TIPO SERVICIO" />
                                            <asp:BoundField DataField="IIDRUTA" HeaderText="ID RUTA" />
                                            <asp:BoundField DataField="IESTADOSALIDA" HeaderText="ESTADO SALIDA" />
                                            <asp:BoundField DataField="ICODIGO" HeaderText="CODIGO" />
                                            <asp:BoundField DataField="INOMBRECHOFER" HeaderText="NOMBRE CHOFER" />
                                            <asp:BoundField DataField="INOMBREASISTENTE" HeaderText="NOMBRE ASISTENTE" />
                                            <asp:BoundField DataField="ICODIGOITINERARIO" HeaderText="CODIGO ITINERARIO" />
                                            <asp:BoundField DataField="IIDITINERARIO" HeaderText="ID ITINERARIO" />
                                            <asp:BoundField DataField="IOCUPADOS" HeaderText="OCUPADOS" />
                                            <%--botones de acción sobre los registros...--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <%--Botones de eliminar y editar cliente...--%>
                                                    <asp:LinkButton ID="lnkEditar" runat="server" class="Editar" CommandName="Select" title="Editar" data-toggle="tooltip" OnClick="lnkEditar_Click"><i class="fa fa-pencil"></i></asp:LinkButton>&nbsp&nbsp
                                                    <asp:LinkButton ID="lnkEliminar" runat="server" class="Eliminar" CommandName="Select" title="Eliminar" data-toggle="tooltip" OnClick="lnkEliminar_Click"><i class="fa fa-trash"></i></asp:LinkButton>
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
                <%--FINAL DE PANEL DEL GRID--%>

                <%--INICIO DE PANEL DE REGISTRO--%>
                <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_VIAJES %></h3>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">

                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Código"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCodigo" runat="server" class="form-control input-sm" placeholder="CÓDIGO" Style="text-transform: uppercase" BackColor="White" ReadOnly="true"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="No. Viaje"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNumeroViaje" runat="server" class="form-control input-sm" placeholder="Número de Viaje" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Estado"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtEstadoViaje" runat="server" class="form-control input-sm" placeholder="Estado del viaje" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Ingrese la fecha de viaje"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="TxtFechaViaje" runat="server" class="form-control input-sm"  BackColor="White" ></asp:TextBox>                                                        
                                                        <ajaxToolkit:MaskedEditExtender ID="TxtFechaViaje_MaskedEditExtender" runat="server" BehaviorID="TxtFechaViaje_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="TxtFechaViaje" />
                                                        <ajaxToolkit:CalendarExtender ID="TxtFechaViaje_CalendarExtender" runat="server" BehaviorID="TxtFechaViaje_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="TxtFechaViaje" />
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="Andén"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbAnden" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN PRIMERA FILA--%>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Seleccione el vehículo"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtVehiculo" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Vehículo *" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalVehiculo" runat="server" Text="" OnClick="btnAbrirModalVehiculo_Click" tooltip="Seleccionar Vehículo"><i class="fa fa-car"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Seleccione el chofer"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtChofer" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Chofer *" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalChofer" runat="server" Text="" OnClick="btnAbrirModalChofer_Click" tooltip="Seleccionar Chofer"><i class="fa fa-male"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Seleccione el asistente"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtAsistente" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Asistente *" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalAsistente" runat="server" Text="" OnClick="btnAbrirModalAsistente_Click" tooltip="Seleccionar Asistente"><i class="fa fa-male"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN SEGUNDA FILA--%>
                                        
                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-8">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Seleccione el itinerario"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtItinerario" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="Itinerario *" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbrirModalItinerario" runat="server" Text="" OnClick="btnAbrirModalItinerario_Click" tooltip="Seleccionar el itinerario"><i class="fa fa-send"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="Ingreso de Hora de Salida"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtHoraSalida" runat="server" type="time" class="form-control timepicker input-sm"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-times"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>                                            
                                        </div>

                                        <%--FIN TERCERA FILA--%>

                                        <div class="row">
                                            <div class="col-md-6">
                                                <asp:RangeValidator ID ="rvFecha" runat ="server" ControlToValidate="TxtFechaViaje" ErrorMessage="Fecha inválida" Type="Date" MinimumValue="01/01/1900" MaximumValue="31/12/2500" Display="Dynamic" style="color: #FF0000"></asp:RangeValidator> 
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="btnEditaPrecios" runat="server" Text="Editar Precios" class="btn btn btn-warning" Visible="false" OnClick="btnEditaPrecios_Click" />
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" class="btn btn btn-success" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" class="btn btn btn-danger" OnClick="btnRegresar_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </asp:Panel>
                
                <%--FIN DE PANEL DE REGISTRO--%>

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label10" runat="server" Text="Información"></asp:Label>
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

                <%--FIN DE PANEL DE REGISTRO--%>

                <div class="modal fade" id="QuestionModal" data-backdrop="static" data-keyboard="true">
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
                                            <asp:Label ID="Label7" runat="server" Text="Desea eliminar el registro, puede que no sea recuperable"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnInicial" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial" 
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Registros de extras por terminal</h4>
                    </div>
                    <div class="modal-body">                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvExtras" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" >
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No."  />
                                            <asp:BoundField DataField="IIDPRODUCTO" HeaderText="ID PRODUCTO" />
                                            <asp:BoundField DataField="IDESCRIPCION" HeaderText="DESCRIPCIÓN" />
                                            <asp:BoundField DataField="IVALOR" HeaderText="PRECIO ACTUAL" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="NUEVO PRECIO">
                                                <ItemTemplate>
                                                    <asp:TextBox ID="txtValorExtra" runat="server" class="form-control input-sm" placeholder="0.00" Text='<%# Bind("IVALOR") %>'></asp:TextBox>
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

                        <div class="modal-footer">
                            <asp:Button ID="btnGuardarValor" runat="server" Text="Actualizar Precios" class="btn btn btn-success" OnClick="btnGuardarValor_Click" />                            
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>


    <%--MODAL DE VEHICULOS--%>

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
                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnFiltarChoferAsistente">
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
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvAsistentesChofer" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="10" OnSelectedIndexChanged="dgvAsistentesChofer_SelectedIndexChanged" OnPageIndexChanging="dgvAsistentesChofer_PageIndexChanging">
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

    <%--FIN DE MODAL DE VEHICULOS--%>

    <%--PROGRESS--%>

    <asp:Button ID="btnVehiculosI" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Vehiculo" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnVehiculosI" 
        PopupControlID="pnlVehiculos" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlVehiculos" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel4" runat="server">
        <ContentTemplate>
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalVehiculos" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalVehiculos_Click" />
                        <h4 class="modal-title" id="myModalLabel7">Registros de Vehículos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel2" runat="server" DefaultButton="btnFiltarChoferVehiculos">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarVehiculos" runat="server" class="form-control input-sm" placeholder="BÚSQUEDA DE VEHICULOS POR DISCO" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltarChoferVehiculos" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltarChoferVehiculos_Click" />
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvVehiculos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="10" OnSelectedIndexChanged="dgvVehiculos_SelectedIndexChanged" OnPageIndexChanging="dgvVehiculos_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="INUMERO" HeaderText="No."  />
                                            <asp:BoundField DataField="IIDREGISTRO" HeaderText="ID REGISTRO" />
                                            <asp:BoundField DataField="IPLACA" HeaderText="PLACA DEL VEHÍCULO" />
                                            <asp:BoundField DataField="IDISCO" HeaderText="DISCO" />
                                            <asp:BoundField DataField="ITIPOVEHICULO" HeaderText="TIPO VEHÍCULO" />
                                            <asp:BoundField DataField="IREGISTRO" HeaderText="REGISTRO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionVehiculos" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccionVehiculos_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
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

    <%--FIN DE MODAL DE VEHICULOS--%>

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
            <div class="modal-dialog">
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalItinerario" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalItinerario_Click" />
                        <h4 class="modal-title" id="myModalLabel17">Registros</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltarItinearios">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarItinerarios" runat="server" class="form-control input-sm" placeholder="BÚSQUEDA DE ITINERARIOS" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltarItinearios" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltarItinearios_Click" />
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvItinerarios" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="12" OnSelectedIndexChanged="dgvItinerarios_SelectedIndexChanged" OnPageIndexChanging="dgvItinerarios_PageIndexChanging" >
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DE MODAL DE VEHICULOS--%>

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
