<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmParametrosFacturacionElectronica.aspx.cs" Inherits="Solution_CTT.frmParametrosFacturacionElectronica" %>
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
                                <i class="fa  fa-cog"></i>
                                <h3 class="box-title">Parámetros Generales</h3>
                            </div>
                            <div class="box-body">
                                <div class="box-header">
                                    <h4 class="box-title">Correo Electrónico</h4>
                                </div>
                                <p></p>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Cuenta de Envío *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCuenta" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Contraseña de Envío *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPasswordCuenta" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Smtp *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtSmtp" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>SSL</label>
                                        <div class="input-group col-sm-12 text-center">
                                            <asp:CheckBox ID="chkSSL" runat="server" CssClass="form-control input-sm" />
                                        </div>
                                    </div>                                    
                                </div>
                                <div class="row">   
                                    <div class="col-md-3 form-group">
                                        <label>Puerto *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPuerto" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                        </div>
                                    </div>                                 
                                    <div class="col-md-3 form-group">
                                        <label>Correo con copia *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoCopia" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Correo Consumindor Final *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoConsumidorFinal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Correo Ambiente Pruebas *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoAmbientePruebas" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-header with-border">
                                    <h4 class="box-title">URL Consumo Web Service</h4>
                                </div>
                                <p></p>
                                <h5 class="box-title">Ambiente de Prueba</h5>
                                <p></p>
                                <div class="row">
                                    <div class="col-md-6 form-group">
                                        <label>URL Envío *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtEnvioPruebas" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 form-group">
                                        <label>URL Consulta *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtConsultaPruebas" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <h5 class="box-title">Ambiente de Producción</h5>
                                    <p></p>
                                <div class="row">                                    
                                    <div class="col-md-6 form-group">
                                        <label>URL Envío *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtEnvioProduccion" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 form-group">
                                        <label>URL Consulta *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtConsultaProduccion" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-header with-border">
                                    <h3 class="box-title">Certificado Digital</h3>
                                </div>
                                <p></p>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Ruta de archivo (*.p12)</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtRuta" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Contraseña del Certificado *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPasswordCertificado" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>              
                            </div>
                            <div class="box-footer">
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnGuardar" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para guardar los cambios realizados" OnClick="lbtnGuardar_Click"><i class="fa fa-pencil-square-o"></i>  Guardar</asp:LinkButton>
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

    <%--MODAL--%>

    <%--FIN MODAL--%>
    <%--PROGRESS--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal-backdrop">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <%--<br />
                    <input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />--%>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
