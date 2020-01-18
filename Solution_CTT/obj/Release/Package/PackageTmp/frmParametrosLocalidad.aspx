<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametrosLocalidad.aspx.cs" Inherits="Solution_CTT.frmParametrosLocalidad" %>
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
                    <div class="col-md-6">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging" OnRowDataBound="dgvDatos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IIDPARAMETROLOCALIDAD" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID PUEBLO" />
                                        <asp:BoundField DataField="IIDCIUDAD" HeaderText="ID CIUDAD" />
                                        <asp:BoundField DataField="IIDVENDEDOR" HeaderText="ID VENDEDOR" />
                                        <asp:BoundField DataField="IPUEBLO" HeaderText="LOCALIDAD" />
                                        <asp:BoundField DataField="IPAGOADMINISTRACION" HeaderText="PAGO ADMIN." ItemStyle-HorizontalAlign="Center"  />
                                        <asp:BoundField DataField="IPORCENTAJERETENCION" HeaderText="% RETENCIÓN" ItemStyle-HorizontalAlign="Center"  />
                                        <asp:BoundField DataField="IIDPRODUCTORETENCION" HeaderText="IDPRODUCTORETENCION" />
                                        <asp:BoundField DataField="IIDPRODUCTOPAGO" HeaderText="IDPRODUCTOPAGO" />
                                        <asp:BoundField DataField="INOMBRERETENCION" HeaderText="NOMBRE RETENCION" />
                                        <asp:BoundField DataField="INOMBREPAGO" HeaderText="NOMBRE PAGO" />
                                        <asp:BoundField DataField="ITASAUSUARIO" HeaderText="TASA USUARIO" />
                                        <asp:BoundField DataField="ICANTIDADMANIFIESTO" HeaderText="MANIFIESTO" />
                                        <asp:BoundField DataField="IEJECUTACOBROADMINISTRACION" HeaderText="COBRO ADMINISTRADOR" />
                                        <asp:BoundField DataField="IIDPROVEEDORTASAS" HeaderText="ID PROVEEDOR" />
                                        <asp:BoundField DataField="ICODIGOPROVEEDOR" HeaderText="CODIGO PROVEEDOR" />
                                        <asp:BoundField DataField="IBOLETOCORTESIA" HeaderText="CORTESIA" />
                                        <asp:BoundField DataField="INOTAENTREGAEXTRA" HeaderText="NOTA ENTREGA EXTRA" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEdit_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="BORRAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" class="btn btn-xs btn-danger" OnClick="lbtnDelete_Click"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- /.box -->
                    </div>

                    <%--REGISTER--%>
                  <div class="col-md-6">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_PARAMETROS_LOCALIDAD %></h3>
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
                                    <%--PRIMERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label18" runat="server" Text="Seleccione Terminal *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbTerminales" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label3" runat="server" Text="Seleccione Ciudad *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbCiudad" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label100" runat="server" Text="Seleccione Vendedor *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbVendedor" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:Label ID="Label4" runat="server" Text="Pago Administración *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtModalPago" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="DESCRIPCION *" ToolTip="Debe seleccionar un ítem, haciendo click en el boton verde Buscar Ítem" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <asp:Label ID="Label6" runat="server" Text="Ver"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:Button ID="btnAbrirModalPago" runat="server" Text="?" class="btn btn-sm btn-warning btn-block pull-right" OnClick="btnAbrirModalPago_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label5" runat="server" Text="Valor *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPago" runat="server" CssClass="form-control input-sm" placeholder="Pago administración *" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--TERCERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-8">
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="Registro de Retención *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtModalRetencion" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="DESCRIPCION *" ToolTip="Debe seleccionar un ítem, haciendo click en el boton verde Buscar Ítem" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <asp:Label ID="Label8" runat="server" Text="Ver"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:Button ID="btnAbrirModalRetencion" runat="server" Text="?" class="btn btn-sm btn-info btn-block pull-right" OnClick="btnAbrirModalRetencion_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="Porcentaje *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="form-control input-sm" placeholder="% Retención *" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--CUARTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Tasa de Usuario"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkManejaTasaUsuario" CssClass="form-control input-sm" runat="server" Text="&nbsp;&nbsp;Aplica Tasa Usuario" AutoPostBack="true" OnCheckedChanged="chkManejaTasaUsuario_OnCheckedChanged" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label112" runat="server" Text="Proveedores de Tasa de Usuario"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbProveedoresTasas" runat="server" class="form-control" Enabled="false"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>  
                                    
                                    <%--QUINTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="Pago Admin."></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkEjecutaCobrosAdministrativos" CssClass="form-control input-sm" runat="server" Text="&nbsp;&nbsp;Ejecuta Cobros" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label15" runat="server" Text="Manifiesto *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtCantidadManifiesto" runat="server" CssClass="form-control input-sm" placeholder="Cantidad" autocomplete="off" BackColor="White" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="Boleto Cortesía"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkBoletoCortesia" CssClass="form-control input-sm" runat="server" Text="&nbsp;&nbsp;Habilitar Cortesía" />
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                    
                                    <%--SEXTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label13" runat="server" Text="Nota Entrega Extra"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkExtraNotaEntrega" CssClass="form-control input-sm" runat="server" Text="&nbsp;&nbsp;Aplica Nota Entrega Extra" />
                                                </div>
                                            </div>
                                        </div>
                                    </div> 
                                                                       
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="form-group">
                                    <%--PRIMERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnSave" runat="server" Text="Crear" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSave_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="btn btn-sm btn-default btn-block pull-right" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>
                                    </div> 

                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
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
                </div>
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
                                            <asp:Label ID="Label1" runat="server" Text="Desea eliminar el registro, puede que no sea recuperable"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal" />
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click" />
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

    <asp:Button ID="btnInicial" runat="server" Text="Button" style="display:none"/>

    <ajaxToolkit:ModalPopupExtender ID="modalExtenderItems" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial" 
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Registros Existentes</h4>
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
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvFiltrarItems" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarItems_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvFiltrarItems_PageIndexChanging" OnRowDataBound="dgvFiltrarItems_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="id_producto" HeaderText="ID"  />
                                            <asp:BoundField DataField="codigo" HeaderText="CODIGO" />
                                            <asp:BoundField DataField="nombre" HeaderText="ÍTEM" />
                                            <asp:BoundField DataField="porcentaje_retencion_ticket" HeaderText="% RETENCIÓN" />
                                            <asp:BoundField DataField="valor" HeaderText="VALOR" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionItem" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccionItem_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                    
                                            <%--<asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEditarPersona_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                    </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField> --%>                                           
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
