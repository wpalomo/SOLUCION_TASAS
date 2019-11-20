<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmDefinicionLocalidades.aspx.cs" Inherits="Solution_CTT.frmDefinicionLocalidades" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-md-5">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title">Definición de Localidades</h3>

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
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging" >
                                    <Columns>
                                        <asp:BoundField DataField="id_localidad" HeaderText="ID LOCALIDAD"/>
                                        <asp:BoundField DataField="nombrecomercial" HeaderText="NOM. COMERCIAL" />
                                        <asp:BoundField DataField="valor_texto" HeaderText="LOCALIDAD" />
                                        <asp:BoundField DataField="apellidos" HeaderText="RESPONSABLE" />
                                        <asp:BoundField DataField="establecimiento" HeaderText="ESTABLECIMIENTO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="punto_emision" HeaderText="PUNTO EMISION" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="emite_comprobante_electronico" HeaderText="EMITE COMP. ELEC." />
                                        <asp:BoundField DataField="direccion" HeaderText="DIRECCIÓN" />
                                        <asp:BoundField DataField="estado" HeaderText="ESTADO" />
                                        <asp:BoundField DataField="id_responsable" HeaderText="ID RESPONSABLE" />
                                        <asp:BoundField DataField="identificacion" HeaderText="IDENTIFICACION" />
                                        <asp:BoundField DataField="telefono1" HeaderText="TELEFONO 1" />
                                        <asp:BoundField DataField="telefono2" HeaderText="TELEFONO 2" />
                                        <asp:BoundField DataField="id_bodega" HeaderText="TELEFONO 2" />
                                        <asp:BoundField DataField="id_lista_defecto" HeaderText="ID AUXILIAR" />
                                        <asp:BoundField DataField="id_servidor" HeaderText="ID SERVIDOR" />
                                        <asp:BoundField DataField="idempresa" HeaderText="ID EMPRESA" />
                                        <asp:BoundField DataField="cg_localidad" HeaderText="CG LOCALIDAD" />
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
                    <div class="col-md-7">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">Datos del Registro</h3>
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
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label18" runat="server" Text="Empresa *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbEmpresa" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label3" runat="server" Text="Localidad *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbLocalidadTerminal" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEGUNDA FILA--%>

                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label4" runat="server" Text="Establecimiento *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtEstablecimiento" runat="server" class="form-control input-sm" placeholder="Estab." MaxLength="3" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Label ID="Label5" runat="server" Text="Punto Emisión *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPuntoEmision" runat="server" class="form-control input-sm" placeholder="Pto. Emi." MaxLength="3" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <asp:Label ID="Label6" runat="server" Text="Comprobantes Electrónicos"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:CheckBox ID="chkEmiteElectronico" runat="server" class="form-control input-sm" Text="&nbsp;&nbsp;Emite Comprobantes Electrónicos" />
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--TERCERA FILA--%>

                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label7" runat="server" Text="Identificación Responsable *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtIdentificacion" runat="server" class="form-control input-sm" BackColor="White" placeholder="Identificación" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-1">
                                            <div class="form-group">
                                                <asp:Label ID="Label8" runat="server" Text="Ver"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:Button ID="btnVerModal" runat="server" Text="?" class="btn btn-sm btn-warning btn-block pull-right" OnClick="btnVerModal_Click" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-7">
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="Nombre Responsable *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNombreResponsable" runat="server" class="form-control input-sm" BackColor="White" placeholder="Responsable" ReadOnly="true"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--CUARTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Bodega *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbBodegas" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="Lista de Precio por Defecto *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbListasPrecios" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="Servidor por Defecto *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbServidores" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--QUINTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <asp:Label ID="Label13" runat="server" Text="Dirección *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtDireccion" runat="server" class="form-control input-sm" placeholder="Dirección"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEXTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label14" runat="server" Text="Teléfono # 1 *"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTelefono_1" runat="server" class="form-control input-sm" placeholder="Teléfono # 1"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label15" runat="server" Text="Teléfono # 2"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTelefono_2" runat="server" class="form-control input-sm" placeholder="Teléfono # 2"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEPTIMA FILA--%>
                                    <div class="row">
                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="btn btn-sm btn-default btn-block pull-right" OnClick="btnCancel_Click" />
                                            </div>
                                        </div>

                                        <div class="col-md-3">
                                            <div class="form-group">
                                                <asp:Button ID="btnSave" runat="server" Text="Crear" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSave_Click" />
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

    <%--MODAL DE VEHICULOS--%>

    <asp:Button ID="btnResponsables" runat="server" Text="Button" style="display:none"/>
    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Personas" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnResponsables" 
        PopupControlID="pnlPersonas" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlPersonas" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel3" runat="server">
        <ContentTemplate>
            <%--<div class="modal-dialog">--%>
                <div class="modal-content modal-lg">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalPersonas" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalPersonas_Click" />
                        <h4 class="modal-title" id="myModalLabel6">Registros</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltarResponsable">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarResponsables" runat="server" class="form-control input-sm" placeholder="BÚSQUEDA" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                    </div>

                                    <div class="col-md-4">
                                        <asp:Button ID="btnFiltarResponsable" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltarResponsable_Click" />
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvResponsables" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="10" OnSelectedIndexChanged="dgvResponsables_SelectedIndexChanged" OnPageIndexChanging="dgvResponsables_PageIndexChanging">
                                        <Columns>
                                            <asp:BoundField DataField="id_persona" HeaderText="ID PERSONA"  />
                                            <asp:BoundField DataField="identificacion" HeaderText="IDENTIFICACION" />
                                            <asp:BoundField DataField="responsable" HeaderText="RESPONSABLE" />
                                            <asp:BoundField DataField="correo_electronico" HeaderText="CORREO ELECTRÓNICO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccion_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
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
