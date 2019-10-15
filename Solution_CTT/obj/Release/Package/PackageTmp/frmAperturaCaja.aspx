<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmAperturaCaja.aspx.cs" Inherits="Solution_CTT.frmAperturaCaja" %>
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
                                    <h3 class="box-title">Apertura de Caja</h3>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Fecha de Apertura"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaApertura" runat="server" class="form-control input-sm" placeholder="Fecha de Apertura" Style="text-transform: uppercase" BackColor="White" ReadOnly="true"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calculator"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="Hora de Apertura"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtHoraApertura" runat="server" class="form-control input-sm" placeholder="Hora de Apertura" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-hourglass-o"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label13" runat="server" Text="Usuario"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtUsuario" runat="server" class="form-control input-sm" placeholder="Usuario" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Jornada"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtJornada" runat="server" class="form-control input-sm" placeholder="Jornada" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-cloud"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Localidad"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtLocalidad" runat="server" class="form-control input-sm" placeholder="Localidad" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-address-book-o"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="Saldo Inicial"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtSaldoInicial" runat="server" class="form-control input-sm" placeholder="Saldo Inicial" Text="0.00" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Observaciones"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control input-sm" placeholder="Observaciones*" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" class="btn btn btn-success" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" class="btn btn btn-danger" OnClick="btnRegresar_Click" />
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
