<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmObtenerTokens.aspx.cs" Inherits="Solution_CTT.frmObtenerTokens" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="true"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-md-12">
                        <div class="col-md-2">
                            <asp:Button ID="btnEjecutar" runat="server" Text="Ejecutar Consulta" class="btn btn btn-info" OnClick="btnEjecutar_Click" />
                        </div>
                        <div class="col-md-2">
                            <asp:Button ID="btnCancelar" runat="server" Text="Cancelar" data-backdrop="false" class="btn btn btn-danger" OnClick="btnCancelar_Click" />
                        </div>
                    </div>
                </div>

                <br />

                <div class="row">
                    <div class="col-md-6">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Registros del Web Service</h3>
                            </div>

                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="Id" HeaderText="ID" />
                                        <asp:BoundField DataField="Token" HeaderText="TOKEN" />
                                        <asp:BoundField DataField="OficinaId" HeaderText="ID OFICINA" />
                                        <asp:BoundField DataField="EstatusId" HeaderText="ESTATUS ID" />
                                        <asp:BoundField DataField="MaxSec" HeaderText="MAX SECUENCIAL" />
                                        <asp:BoundField DataField="CreatedAt" HeaderText="FECHA CREACIÓN" />
                                        <asp:BoundField DataField="UpdatedAt" HeaderText="FECHA ACTUALIZACIÓN" />
                                        <asp:BoundField DataField="MaxCant" HeaderText="CANT. MAX." />
                                        <asp:BoundField DataField="CantActual" HeaderText="CANT. USADOS" />
                                        <asp:BoundField DataField="Estado" HeaderText="ESTADO" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="col-md-6">
                        <div class="box box-warning">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Registros del Sistema</h3>
                            </div>

                            <div class="box-body">
                                <asp:GridView ID="dgvDatosSistema" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                    <Columns>
                                        <asp:BoundField DataField="id_ctt_tasa_token" HeaderText="ID" />
                                        <asp:BoundField DataField="id_ctt_oficinista" HeaderText="ID OFICINISTA" />
                                        <asp:BoundField DataField="token" HeaderText="TOKEN" />
                                        <asp:BoundField DataField="fecha_generacion" HeaderText="FECHA GENERACIÓN" />
                                        <asp:BoundField DataField="maximo_secuencial" HeaderText="CANT. MÁX." />
                                        <asp:BoundField DataField="emitidos" HeaderText="CANT. USADOS" />
                                        <asp:BoundField DataField="anulados" HeaderText="CANT. ANULADOS" />                                                                
                                        <asp:BoundField DataField="oficinista" HeaderText="OFICINISTA" />
                                        <asp:BoundField DataField="estado_token" HeaderText="ESTADO" />
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
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
