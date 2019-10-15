<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmEmisor.aspx.cs" Inherits="Solution_CTT.frmEmisor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="box-default">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-user"></i>
                                <h3 class="box-title">Datos del Emisor</h3>
                            </div>
                            <div class="box-body">
                                <div class="col-md-8">
                                    <div class="col-md-4 form-group">
                                        <label>Código de Empresa</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCodigoEmpresa" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>R.U.C.</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtRUC" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Razón Social</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>

                                    <div class="col-md-8 form-group">
                                        <label>Nombre Comercial</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNombreComercial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Teléfono</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtTelefono" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-8 form-group">
                                        <label>Dirección Matriz</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtDireccionMatriz" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Fax</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtFax" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Contrib. Esp. N° Resolución</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtContribuyenteEspecial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Estado</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtEstado" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-4 form-group">
                                        <label>Obligado a llevar Contabilidad</label>
                                        <div class="input-group col-sm-12 text-center">
                                            <asp:CheckBox ID="chkContabilidad" runat="server" CssClass="form-control input-sm" />
                                        </div>
                                    </div>
                                </div>

                                <div class="col-md-4">
                                    <div class="col-sm-10 text-center">
                                        <asp:Image ID="ImgLogo" runat="server" class="img-responsive" ImageUrl="assets/images/icons/logo_here.png" />
                                    </div>
                                    <%--<p>m</p>--%>
                                    <div class="col-sm-12">
                                        <asp:FileUpload ID="FuploadLogo" runat="server" accept=".png,.jpg,.jpeg" onchange="previewImagen(this)" class="form-control input-sm" />
                                    </div>
                                </div>

                                <div class="col-md-12">
                                    <div class="col-md-3 form-group">
                                        <label>Tipo de Emisión</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoEmision" runat="server" class="form-control input-sm"></asp:DropDownList>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Tipo Certificado Digital</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoCertificadoDigital" runat="server" class="form-control input-sm"></asp:DropDownList>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Tipo Ambiente</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoAmbiente" runat="server" class="form-control input-sm"></asp:DropDownList>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Tiempo Max. espera Rspta</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtTiempoMaxEspera" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>N° Patronal</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNumeroPatronal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 form-group">
                                        <label>Actividad Económica</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtActividadEconomica" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Sector Municiapl</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtSectorMunicipal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Ciudad</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCiudad" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Pais</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPais" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Gerente General</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtGerenteGeneral" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Contador General</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtContadorGeneral" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>R.U.C. Contador</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtRUCContador" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Matrícula Contador</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtMatriculaContador" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-footer">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="col-md-3 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnGuardar" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para guardar los cambios realizados" OnClick="lbtnGuardar_Click"><i class="fa fa-pencil-square-o"></i>  Guardar</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnEliminar" runat="server" class="form-control btn btn-flat btn-danger" ToolTip="Clic aquí, para eliminar el registro" OnClick="lbtnEliminar_Click"><i class="fa fa-trash"></i>  Eliminar</asp:LinkButton>
                                            </div>
                                        </div>
                                        <div class="col-md-3 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="lbtnCancelar_Click"><i class="fa fa-circle-o-notch"></i>  Cancelar</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <%--MODAL CONFIRMACION ELIMINAR--%>
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
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" />
                            </div>
                        </div>
                    </div>
                </div>
                <%--FIN--%>

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
                                <button type="button" class="btn btn-default" data-dismiss="modal">Aceptar</button>
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
