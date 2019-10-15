﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametrosLocalidad.aspx.cs" Inherits="Solution_CTT.frmParametrosLocalidad" %>
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
                    <div class="col-md-8">
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
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging">
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
                  <div class="col-md-4">
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
                                <div class="register-box-body">
                                    <div class="row">
                                        <div class="col-md-offset-1 col-md-10">
                                            <div class="row">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="cmbTerminales" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                            </div>
                                            <div class="row">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="cmbCiudad" runat="server" class="form-control input-sm"></asp:DropDownList>                                                        
                                                    </div>
                                            </div>
                                            <div class="row">
                                                    <div class="form-group has-feedback">
                                                        <asp:DropDownList ID="cmbVendedor" runat="server" class="form-control input-sm"></asp:DropDownList>                                                        
                                                    </div>
                                            </div>
                                            <div class="row">
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtModalPago" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="DESCRIPCION *" ToolTip="Debe seleccionar un ítem, haciendo click en el boton verde Buscar Ítem" BackColor="White"></asp:TextBox>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtPago" runat="server" CssClass="form-control input-sm" placeholder="Pago administración *" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:LinkButton ID="btnAbrirModalPago" runat="server" Text="" class="btn btn-block btn-success" OnClick="btnAbrirModalPago_Click"><i class="fa fa-search"> BUSCAR ÍTEM DE PAGO</i></asp:LinkButton>
                                                </div>
                                            </div>
                                            <div class="row">
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtModalRetencion" ReadOnly="true" runat="server" CssClass="form-control input-sm" placeholder="DESCRIPCION *" ToolTip="Debe seleccionar un ítem, haciendo click en el boton verde Buscar Ítem" BackColor="White"></asp:TextBox>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtPorcentaje" runat="server" CssClass="form-control input-sm" placeholder="% Retención *" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:LinkButton ID="btnAbrirModalRetencion" runat="server" Text="" class="btn btn-block btn-success" OnClick="btnAbrirModalRetencion_Click"><i class="fa fa-search"> BUSCAR ÍTEM DE RETENCIÓN</i></asp:LinkButton>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:TextBox ID="txtCantidadManifiesto" runat="server" CssClass="form-control input-sm" placeholder="Cantidad Manifiesto Imprimir" BackColor="White" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                                <div class="form-group has-feedback">
                                                    <asp:CheckBox ID="chkManejaTasaUsuario" runat="server" Text="&nbsp;&nbsp;Aplica Tasa Usuario" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-md-offset-1 col-md-5">
                                            <div class="form-group">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="btn btn-sm btn-default btn-block pull-right" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>
                                        <div class=" col-md-5">
                                            <div class="form-group">
                                                <asp:Button ID="btnSave" runat="server" Text="Crear" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSave_Click" />
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
                    <div class="modal-header">
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
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvFiltrarItems" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarItems_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvFiltrarItems_PageIndexChanging" >
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