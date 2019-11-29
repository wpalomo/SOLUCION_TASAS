<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReabrirCaja.aspx.cs" Inherits="Solution_CTT.frmReabrirCaja" %>
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
                                    <h3 class="box-title">Reaperturar última Caja</h3>
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
                                                    <asp:Label ID="Label13" runat="server" Text="Oficinista Apertura"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtOficinistaApertura" runat="server" class="form-control input-sm" placeholder="Oficinista Apertura" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Fecha de Cierre"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaCierre" runat="server" class="form-control input-sm" placeholder="Fecha de Cierre" Style="text-transform: uppercase" BackColor="White" ReadOnly="true"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-calculator"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Hora de Cierre"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtHoraCierre" runat="server" class="form-control input-sm" placeholder="Hora de Cierre" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-hourglass-o"></i></span>
                                                    </div>
                                                </div>
                                            </div>


                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label2" runat="server" Text="Oficinista Cierre"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtOficinistaCierre" runat="server" class="form-control input-sm" placeholder="Oficinista Cierre" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-cloud"></i></span>
                                                    </div>
                                                </div>
                                            </div>                                            
                                        </div>

                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label12" runat="server" Text="Estado de la Caja"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtEstado" runat="server" class="form-control input-sm" placeholder="Estado" ReadOnly="true" BackColor="White" Style="text-transform: uppercase"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-address-book-o"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="Jornada"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtJornada" runat="server" class="form-control input-sm" ReadOnly="true" placeholder="Jornada" Text="" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Oficina"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtOficina" runat="server" class="form-control input-sm" ReadOnly="true" placeholder="Oficina" Text="" BackColor="White"></asp:TextBox>
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
                                                        <asp:TextBox ID="txtObservaciones" runat="server" CssClass="form-control input-sm" placeholder="Observaciones*" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <asp:Panel ID="pnlMotivo" runat="server">
                                            <div class="row">
                                                <div class="col-md-12">
                                                    <div class="form-group">
                                                        <asp:Label ID="Label14" runat="server" Text="Motivo de reapertura"></asp:Label>
                                                        <div class="input-group col-sm-12">
                                                            <asp:TextBox ID="txtMotivo" runat="server" CssClass="form-control input-sm" placeholder="Motivo de Reapertura*" BackColor="White"></asp:TextBox>
                                                            <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                        </div>                                                
                                                    </div>
                                                </div>
                                            </div>
                                        </asp:Panel>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="BtnReabrir" runat="server" Text="Reabrir última caja" class="btn btn btn-success" OnClick="BtnReabrir_Click" />
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

                <%--MODAL PARA CERRAR EL VIAJE--%>
                <div class="modal fade" id="QuestionModalAbrir" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label8" runat="server" Text="Información."></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label9" runat="server" Text="¿Está seguro que desea realizar la reapertura de caja?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNoCerrar" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAceptar" runat="server" Text="Sí, confirmar" class="btn btn-info" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptar_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL PARA CERRAR EL VIAJE--%>

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
