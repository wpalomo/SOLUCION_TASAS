<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmFrecuenciasContifico.aspx.cs" Inherits="Solution_CTT.frmFrecuenciasContifico" %>
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
                      <%--REGISTER--%>
                  <div class="col-md-12">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">Frecuencias del Sistema SMARTT</h3>
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
                                                <asp:Label ID="Label18" runat="server" Text="Seleccione la Frecuencia"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:DropDownList ID="cmbFrecuencias" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbFrecuencias_SelectedIndexChanged" class="form-control input-sm"></asp:DropDownList>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label9" runat="server" Text="Nombre del Destino"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtNombreDestino" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="NOMBRE DEL DESTINO" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Vía"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtVia" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="VIA" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label11" runat="server" Text="Tipo"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTipo" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="TIPO" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label4" runat="server" Text="Tipo Nombre"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTipoNombre" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="TIPO NOMBRE" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label12" runat="server" Text="Fecha de Validez"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtFechaValidez" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="FECHA DE VALIDEZ" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                    <%--TERCERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkLunes" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspLunes" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkMartes" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspMartes" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkMiercoles" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspMiércoles" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkJueves" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspJueves" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkViernes" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspViernes" />
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkSabado" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspSábado" />
                                            </div>
                                        </div>
                                    </div>

                                    <%--CUARTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <asp:CheckBox ID="chkDomingo" Enabled="false" runat="server" class="form-control input-sm" Text="&nbsp&nbspDomingo" />
                                            </div>
                                        </div>
                                    </div>                                                                 
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="form-group">
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
                                    <asp:Label ID="lbAccion" runat="server" Text="Información"></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label1" runat="server" Text="¿Está seguro que desea actualizar el registro?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal" />
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, confirmar" class="btn btn-success" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click" />
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
