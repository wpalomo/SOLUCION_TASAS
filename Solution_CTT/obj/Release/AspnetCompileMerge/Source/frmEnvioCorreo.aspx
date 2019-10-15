<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmEnvioCorreo.aspx.cs" Inherits="Solution_CTT.frmCorreo" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="box-default">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-info">
                            <div class="box-header with-border">
                                <i class="fa fa-envelope"></i>
                                <h3 class="box-title ">Enviar Correo</h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-6 form-group">
                                        <label>Cuenta de Envío</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoEnvia" runat="server" class="form-control input-sm" placeholder="" ReadOnly="true" BackColor="White" disabled="" ToolTip="Para modificar ingresar a parámetros"></asp:TextBox>
                                            <span class="input-group-addon input-sm">@</span>
                                        </div>
                                    </div>
                                    <div class="col-md-6 form-group">
                                        <label>Contraseña de Envío</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPasswordCorreoEnvia" runat="server" class="form-control input-sm" placeholder="" ReadOnly="true" BackColor="White" disabled="" ToolTip="Para modificar ingresar a parámetros"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-eye"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Smtp</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtSmtp" runat="server" class="form-control input-sm" placeholder="" ReadOnly="true" BackColor="White" disabled="" ToolTip="Para modificar ingresar a parámetros"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-genderless"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Puerto</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtPuerto" runat="server" class="form-control input-sm" placeholder="" ReadOnly="true" BackColor="White" disabled="" ToolTip="Para modificar ingresar a parámetros"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-send-o"></i></span>
                                        </div>
                                    </div>
                                    <%--<div class="col-md-3 form-group">
                                        <label>SSL</label>
                                        <div class="input-group col-sm-12 text-center">
                                            <asp:CheckBox ID="chkSSL" runat="server" CssClass="form-control input-sm" />
                                        </div>
                                    </div>--%>
                                </div>
                                <div class="row">
                                    <div class="col-md-12 form-group">
                                        <label>Para *</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoRecibe" runat="server" class="form-control input-sm" placeholder="xxxxxx@gmail.com" AutoComplete="off" ToolTip="Aquí, correo a quién se envía"></asp:TextBox>
                                            <span class="input-group-addon input-sm">@</span>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label>Correo Copia </label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoCopia" runat="server" class="form-control input-sm" placeholder="xxxxxx@gmail.com" AutoComplete="off" ToolTip="Aquí, correo a quién se envía"></asp:TextBox>
                                            <span class="input-group-addon input-sm">@</span>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label>Correo Copia Oculta</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCorreoCopiaOculta" runat="server" class="form-control input-sm" placeholder="xxxxxx@gmail.com" AutoComplete="off" ToolTip="Aquí, correo a quién se envía"></asp:TextBox>
                                            <span class="input-group-addon input-sm">@</span>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label>Asunto</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtAsunto" runat="server" class="form-control input-sm" placeholder="Asunto" AutoComplete="off" ToolTip="Aquí, asunto del correo"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-bookmark-o"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-12 form-group">
                                        <label>Mensaje</label>
                                        <div class="input-group col-sm-12">                
                                            <asp:TextBox ID="txtMensaje" runat="server" class="form-control" placeholder="Mensaje" style="width: 100%; height: 125px; font-size: 14px; border: 1px solid #dddddd; padding: 10px;" AutoComplete="off" ToolTip="Aquí, su correo" TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-6 form-group">
                                        <label>Adjuntar Archivos</label>
                                        <div class="input-group col-sm-12">
                                            <asp:FileUpload ID="fileAdjunto1" runat="server" accept=".pdf, .xml" class="form-control input-sm" ToolTip="Clic aquí, para adjuntar el primer archivo" />
                                            <span class="input-group-addon input-sm"><asp:Button ID="cmdAddFile" runat="server"  Text="+" CssClass="btn-success"  ToolTip="Añade el fichero a la lista" OnClick="cmdAddFile_Click" /></span>
                                            
                                        </div>
                                        
                                    </div>

                                    <div class="col-md-6 form-group">
                                        <label>Lista de Archivos</label>
                                        <div class="input-group col-sm-12">
                                            <asp:ListBox ID="lstFiles" runat="server" class="form-control input-sm"></asp:ListBox>
                                            <span class="input-group-addon input-sm"><asp:Button ID="cmdDelFile" runat="server" Text="-" CssClass="btn-warning" ToolTip="Elimina el fichero seleccionado de la lista" OnClick="cmdDelFile_Click" /></span>
                                        </div>
                                    </div>
                                    

                                </div>
                            </div>
                            <div class="box-footer">
                                <div class="col-md-3">
                                    <asp:LinkButton ID="btnEnviarMail" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para enviar el correo" OnClick="btnEnviarMail_Click"><i class="fa fa-envelope-o"></i>  Enviar</asp:LinkButton>
                                </div>
                                <div class="col-md-3">
                                    <asp:LinkButton ID="btnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="btnCancelar_Click"><i class="fa fa-circle-o-notch"></i>  Cancelar</asp:LinkButton>
                                </div>
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
                                    <label>Información</label>
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
        <Triggers><%--CON ESO HACE POST BACK LA PAGINA--%>
            <asp:PostBackTrigger ControlID="cmdAddFile" />
        </Triggers>
    </asp:UpdatePanel>
    <%--PROGRESS--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="Sending">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <%--<img src="assets/images/icons/Preloader50x50.gif" width="50" height="50"/><br />
                    <br />
                    <p style="color: #dddddd">Espere por favor...</p>
                    <br />
                    <input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />--%>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
