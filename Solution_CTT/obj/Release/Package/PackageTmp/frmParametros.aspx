<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametros.aspx.cs" Inherits="Solution_CTT.frmParametros" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">

                <%--INICIO DE PANEL DE REGISTRO--%>                
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Parámetros Generales</h3>
                                </div>

                                <asp:Panel ID="pnlRegistro" runat="server">

                                <div class="box-body">
                                    <div class="form-group">

                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Porcentaje IVA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPorcentajeIva" runat="server" class="form-control input-sm" placeholder="Porcentaje IVA" onkeypress="return validar_numeros(event)"></asp:TextBox>
                                                       <span class="input-group-addon input-sm">$</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="Porcentaje ICE"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPorcentajeIce" runat="server" class="form-control input-sm" placeholder="Porcentaje ICE"></asp:TextBox>
                                                        <span class="input-group-addon input-sm">$</span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Tipo de Comprobante"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTipoComprobante" runat="server" class="form-control input-sm"></asp:DropDownList>                                                    
                                                        <span class="input-group-addon input-sm"><i class="fa fa-paper-plane"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Número de decimales"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNumeroDecimales" runat="server" class="form-control input-sm" placeholder="Cantidad Decimales" onkeypress="return validar_numeros(event)"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-question"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN PRIMERA FILA--%>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Registro sin datos"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPersonaSinDatos" runat="server" class="form-control input-sm" autocomplete="off" placeholder="PERSONA SIN DATOS" tooltip="Para buscar presione la lupa" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnBuscarPersonaSinDatos" runat="server" Text="" OnClick="btnBuscarPersonaSinDatos_Click" tooltip="Buscar Registro"><i class="fa fa-search"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Registro Menor de Edad"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPersonaMenorEdad" runat="server" class="form-control input-sm" autocomplete="off" placeholder="PERSONA MENOR DE EDAD" tooltip="Para buscar presione la lupa" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnBuscarPersonaMenorEdad" runat="server" Text="" OnClick="btnBuscarPersonaMenorEdad_Click" tooltip="Buscar Registro"><i class="fa fa-search"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Consumidor Final"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPersonaConsumidorFinal" runat="server" class="form-control input-sm" autocomplete="off" placeholder="REGISTRO CONSUMIDOR FINAL" tooltip="Para buscar presione la lupa" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnPersonaConsumidorFinal" runat="server" Text="" OnClick="btnPersonaConsumidorFinal_Click" tooltip="Buscar Registro"><i class="fa fa-search"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="Registro sin Pago"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtProductoExtra" runat="server" class="form-control input-sm" autocomplete="off" placeholder="REGISTRO SIN COBRO ADMINISTRACION" tooltip="Para buscar presione la lupa" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnProductoExtra" runat="server" Text="" OnClick="btnProductoExtra_Click" tooltip="Buscar Registro"><i class="fa fa-search"></i></asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN SEGUNDA FILA--%>
                                        
                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Moneda"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbMoneda" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-money"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="Ciudad Default"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCiudad" runat="server" class="form-control input-sm" autocomplete="off" placeholder="Ciudad Default" MaxLength="100" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-opera"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="Telefóno Default"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" autocomplete="off" placeholder="Teléfono Default" MaxLength="15" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-phone"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                            
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Correo Electrónico Default"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCorreoElectronico" runat="server" class="form-control input-sm" autocomplete="off" placeholder="Correo Electrónico Default" MaxLength="200" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-mail-reply"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>                                            
                                        </div>

                                        <%--FIN TERCERA FILA--%>

                                        <%--CUARTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkVistaPrevia" runat="server" class="form-control input-sm" Text="&nbsp&nbspVista Previa de Impresión" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkNotaVenta" runat="server" class="form-control input-sm" Text="&nbsp&nbspManeja Nota de Venta" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkFacturacionElectronica" runat="server" class="form-control input-sm" Text="&nbsp&nbspHabilitar Facturación Electrónica" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkVersionDemo" runat="server" class="form-control input-sm" Text="&nbsp&nbspVersión Demo" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN CUARTA FILA--%>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkBaseClientes" runat="server" class="form-control input-sm" Text="&nbsp&nbspConsulta Base de Datos Clientes" />
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <label class="col-sm-5 control-label">&nbsp</label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:CheckBox ID="chkRegistroCivil" runat="server" class="form-control input-sm" Text="&nbsp&nbspConsultar Registro Civil" />
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN QUINTA FILA--%>

                                    </div>
                                </div>

                                </asp:Panel>

                                <div class="box-footer pull-right">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar" class="btn btn btn-info" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" data-backdrop="false" class="btn btn btn-danger" OnClick="btnCancelar_Click" />
                                </div>

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
            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnInicial" runat="server" Text="Button" style="display:none"/>

    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial" 
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Registro de Personas</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:TextBox ID="txtFiltrarPersonas" runat="server" class="form-control" placeholder="BÚSQUEDA DE PERSONAS" Style="text-transform: uppercase"></asp:TextBox>
                                </div>

                                <div class="col-md-4">
                                    <asp:Button ID="btnFiltarPersonas" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarPersonas_Click" />
                                </div>
                            </div>   
                        </div>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvFiltrarPersonas" runat="server" class="table table-bordered table-striped" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarPersonas_SelectedIndexChanged" AllowPaging="True" PageSize="5" OnPageIndexChanging="dgvFiltrarPersonas_PageIndexChanging" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" >
                                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        <Columns>
                                            <asp:BoundField DataField="IIDCLIENTEFILTRO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDENTIFICACIONFILTRO" HeaderText="IDENTIFICACIÓN" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="ICLIENTEFILTRO" HeaderText="NOMBRES" />
                                            <asp:BoundField DataField="IFECHAFILTRO" HeaderText="FECHA DE NACIMIENTO" ItemStyle-HorizontalAlign="Center">
                                            <ItemStyle HorizontalAlign="Center" />
                                            </asp:BoundField>
                                            <asp:TemplateField HeaderText="SELECCIONAR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" class="btn btn-xs btn-success" CommandName="Select" OnClick="lbtnSeleccion_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
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


    <asp:Button ID="btnInicialItems" runat="server" Text="Button" style="display:none"/>

    <ajaxToolkit:ModalPopupExtender ID="modalExtenderItems" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicialItems" 
        PopupControlID="pnlGridFiltroItems" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltroItems" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModalItems" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalItems_Click" />
                        <h4 class="modal-title" id="myModalLabel6">Registros Existentes</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <div class="form-group">
                                        <div class="input-group col-sm-8">
                                            <asp:TextBox ID="txtFiltrarItems" runat="server" class="form-control input-sm" autocomplete="off" placeholder="BÚSQUEDA DE ÍTEMS" tooltip="Para buscar presione la lupa"></asp:TextBox>
                                        </div>                                                
                                    </div>                                    
                                </div>
                                <div class="col-md-4">
                                    <asp:Button ID="btnFiltrarItems" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarItems_Click" />
                                </div>
                            </div>   
                        </div>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvFiltrarItems" runat="server" class="table table-bordered table-striped" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarItems_SelectedIndexChanged" AllowPaging="True" PageSize="5" OnPageIndexChanging="dgvFiltrarItems_PageIndexChanging" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None" >
                                        <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                        <Columns>
                                            <asp:BoundField DataField="id_producto" HeaderText="ID" />
                                            <asp:BoundField DataField="codigo" HeaderText="CODIGO" />
                                            <asp:BoundField DataField="nombre" HeaderText="ÍTEM" />
                                            <asp:BoundField DataField="porcentaje_retencion_ticket" HeaderText="% RETENCIÓN" />
                                            <asp:BoundField DataField="valor" HeaderText="VALOR" />
                                            <asp:TemplateField HeaderText="SELECCIONAR" ItemStyle-HorizontalAlign="Center">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionItem" runat="server" class="btn btn-xs btn-success" CommandName="Select" OnClick="lbtnSeleccionItem_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
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
