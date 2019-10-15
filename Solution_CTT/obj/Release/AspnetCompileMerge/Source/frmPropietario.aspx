<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPropietario.aspx.cs" Inherits="Solution_CTT.frmPropietario" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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

                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" autocomplete="off" placeholder="Search"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="IIDVEHICULOPROPIETARIO" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDPERSONA" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                        <asp:BoundField DataField="ICODIGO" HeaderText="CÓDIGO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="INOMBRE" HeaderText="PROPIETARIO" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IVEHICULO" HeaderText="VEHÍCULO" HeaderStyle-HorizontalAlign="Center" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IDESCRIPCION" HeaderText="DESCRIPCIÓN" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" HeaderStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEdit_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="BORRAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnDelete_Click"><i class="fa fa-trash"></i></asp:LinkButton>
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
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_PROPIETARIO_VEHICULO %></h3>
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
                                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Código *" autocomplete="off" ></asp:TextBox>
                                            </div>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Descripción *" autocomplete="off" Style="text-transform: uppercase" ></asp:TextBox>
                                            </div> 
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="TxtPersona" ReadOnly="true" runat="server" CssClass="form-control" placeholder="Propietario *"></asp:TextBox>
                                            </div>
                                            <div class="form-group has-feedback">
                                                <asp:LinkButton ID="btnAbrirModalPersonas" runat="server" Text="" class="btn btn-block btn-success" OnClick="btnAbrirModalPersonas_Click"><i class="fa fa-search"> BUSCAR PERSONA</i></asp:LinkButton>
                                            </div> 
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtVehiculo" ReadOnly="true" runat="server" CssClass="form-control" placeholder="Vehículo *"></asp:TextBox>
                                            </div>
                                            <div class="form-group has-feedback">                                                       
                                                 <asp:LinkButton ID="btnAbrirModalVehiculos" runat="server" Text="" class="btn btn-block btn-success" OnClick="btnAbrirModalVehiculos_Click"><i class="fa fa-search"> BUSCAR VEHÍCULO</i></asp:LinkButton>
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
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click"/>
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
                        <h4 class="modal-title" id="myModalLabel5">Registro de Personas</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel3" runat="server" DefaultButton="btnFiltarPersonas">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <%--<asp:Label ID="Label3" runat="server" Text="Tipo de Identificacion"></asp:Label>--%>
                                        <asp:TextBox ID="txtFiltrarPersonas" runat="server" class="form-control" placeholder="BÚSQUEDA DE PERSONAS" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltarPersonas" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarPersonas_Click" />
                                        <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase"></asp:TextBox>--%>
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvFiltrarPersonas" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarPersonas_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvFiltrarPersonas_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="IIDCLIENTEFILTRO" HeaderText="ID"  />
                                            <asp:BoundField DataField="IIDENTIFICACIONFILTRO" HeaderText="IDENTIFICACIÓN" />
                                            <asp:BoundField DataField="ICLIENTEFILTRO" HeaderText="NOMBRES" />
                                            <asp:BoundField DataField="IFECHAFILTRO" HeaderText="FECHA DE NACIMIENTO" />        
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnSeleccion_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                    
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEditarPersona_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
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
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>



    <asp:Button ID="btnInicialVehiculo" runat="server" Text="Button" style="display:none"/>

    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender2" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicialVehiculo" 
        PopupControlID="pnlGridVehiculo" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridVehiculo" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModal2" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal2_Click" />
                        <h4 class="modal-title" id="myModalLabel6">Registro de Vehículos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltrarVehiculos">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-4">
                                        <%--<asp:Label ID="Label3" runat="server" Text="Tipo de Identificacion"></asp:Label>--%>
                                        <asp:TextBox ID="txtFiltrarVehiculos" runat="server" class="form-control" placeholder="BÚSQUEDA DE VEHÍCULOS" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltrarVehiculos" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarVehiculos_Click" />
                                        <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase"></asp:TextBox>--%>
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvFiltrarVehiculos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarVehiculos_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvFiltrarVehiculos_PageIndexChanging" >
                                        <Columns>
                                            <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDTIPOVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="ITIPOVEHICULO" HeaderText="TIPO" />
                                            <asp:BoundField DataField="IIDMARCAVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IMARCAVEHICULO" HeaderText="MARCA" />
                                            <asp:BoundField DataField="IIDMODELOVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IMODELOVEHICULO" HeaderText="MODELO" />
                                            <asp:BoundField DataField="IIDTIPOBUS" HeaderText="ID" />
                                            <asp:BoundField DataField="ITIPOBUS" HeaderText="TIPO DE BUS" />
                                            <asp:BoundField DataField="IIDDISCO" HeaderText="ID" />
                                            <asp:BoundField DataField="IDISCO" HeaderText="DISCO" />
                                            <asp:BoundField DataField="IPLACA" HeaderText="PLACA" />
                                            <asp:BoundField DataField="ICHASIS" HeaderText="CHASIS" />
                                            <asp:BoundField DataField="IMOTOR" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IANIOPRODUCCION" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IPAISORIGEN" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="ICILINDRAJE" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IPESO" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="INUMEROPASAJEROS" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" />
                                            <asp:BoundField DataField="IIDFORMATOASIENTO" HeaderText="ID" />   
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionVehiculo" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnSeleccionVehiculo_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>                                    
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditVehiculo" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEditarVehiculo_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
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
