<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmTasaUsuarioPrueba.aspx.cs" Inherits="Solution_CTT.frmTasaUsuarioPrueba" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">
                <%--INICIO DE PANEL DE REGISTRO--%>
                <asp:Panel ID="pnlRegistro" runat="server">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_VIAJES %></h3>

                                   <%-- <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            
                                        </div>
                                    </div>--%>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">

                                        <%--SECCION: OFICINA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="OFICINA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtOficina" runat="server" class="form-control input-sm" placeholder="OFICINA" Style="text-transform: uppercase" BackColor="White" ReadOnly="true"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="COOPERATIVA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCooperativa" runat="server" class="form-control input-sm" placeholder="COOPERATIVA" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="TERMINAL"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTerminal" runat="server" class="form-control input-sm" placeholder="TERMINAL" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--SECCION: CLIENTE--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="IDENTIFICACION"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtIdentificacion" runat="server" class="form-control input-sm" placeholder="IDENTIFICACION" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="NOMBRE DEL CLIENTE"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNombreCliente" runat="server" class="form-control input-sm" placeholder="CLIENTE" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="DIRECCIÓN:"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtDireccion" runat="server" class="form-control input-sm" placeholder="DIRECCION" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="CORREO ELECTRÓNICO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMail" runat="server" class="form-control input-sm" placeholder="CORREO ELECTRÓNICO" BackColor="White" ></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>                                               
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="TELÉFONO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" placeholder="TELÉFONO" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>                                           
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="TIPO DE CLIENTE"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTipoCliente" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-amazon"></i></span>
                                                    </div>                                              
                                                </div>
                                            </div>
                                        </div>

                                        <%--SECCION: TASA--%>
                                        <div class="row">
                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label8" runat="server" Text="CANTIDAD"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCantidad" runat="server" class="form-control input-sm" MaxLength="2" placeholder="CANTIDAD" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="SECUENCIAL"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtSecuencial" runat="server" class="form-control input-sm" placeholder="SECUENCIAL" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="TOKEN"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtToken" runat="server" class="form-control input-sm" placeholder="TOKEN" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label14" runat="server" Text="TIPO TASA"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTipoTasa" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-amazon"></i></span>
                                                    </div> 
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label15" runat="server" Text="TASA DE USUARIO"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtTasaUsuario" runat="server" class="form-control input-sm" placeholder="TASA USUARIO" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>                                            
                                        </div>

                                        <%--JSON ENVIO--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label16" runat="server" Text="JSON"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtJson" runat="server" class="form-control input-sm" placeholder="JSON" BackColor="White" TextMode="MultiLine"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label17" runat="server" Text="MENSAJE JSON"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMensaje" runat="server" class="form-control input-sm" placeholder="MENSAJE JSON" BackColor="White" TextMode="MultiLine"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>

                                            <div class="col-md-2">
                                                <div class="form-group">
                                                    <asp:Label ID="Label18" runat="server" Text="id tasa"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtIdTasa" runat="server" class="form-control input-sm" placeholder="ID TASA" BackColor="White" TextMode="MultiLine"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>
                                                    </div>  
                                                </div>
                                            </div>
                                        </div>

                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="btnGenerar" runat="server" Text="Generar Tasa" class="btn btn btn-warning" OnClick="btnGenerar_Click" />
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" class="btn btn btn-success" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn btn-danger" OnClick="btnLimpiar_Click" />
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
