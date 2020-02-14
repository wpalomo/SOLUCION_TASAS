<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReimprimirCajas.aspx.cs" Inherits="Solution_CTT.frmReimprimirCajas" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <div class="row">
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                            </div>
                            <div class="box-body">
                                <div class="form-group">
                                    <div class="row">                                
                                        <div class="col-md-3">
                                            <asp:Label ID="Label1" runat="server" Text="Fecha Inicial:"></asp:Label>
                                            <asp:TextBox ID="txtFechaDesde" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaDesde_MaskedEditExtender" runat="server" BehaviorID="txtFechaDesde_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaDesde" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaDesde_CalendarExtender" runat="server" BehaviorID="txtFechaDesde_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaDesde" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label2" runat="server" Text="Fecha Final:"></asp:Label>
                                            <asp:TextBox ID="txtFechaHasta" runat="server" class="form-control"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtFechaHasta_MaskedEditExtender" runat="server" BehaviorID="txtFechaHasta_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaHasta" />
                                            <ajaxToolkit:CalendarExtender ID="txtFechaHasta_CalendarExtender" runat="server" BehaviorID="txtFechaHasta_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaHasta" />
                                        </div>
                                        <div class="col-md-3">
                                            <asp:Label ID="Label4" runat="server" Text="Usuarios"></asp:Label>
                                            <asp:DropDownList ID="cmbUsuarios" runat="server" class="form-control"></asp:DropDownList>
                                        </div>  
                                        <div class="col-md-3">
                                            <asp:Label ID="Label3" runat="server" Text="Boleterías"></asp:Label>
                                            <asp:DropDownList ID="cmbBoleteria" runat="server" class="form-control"></asp:DropDownList>
                                        </div>                                  
                                    </div>
                                </div>

                                <div class="form-group">
                                    <asp:LinkButton ID="btnConsultar" runat="server" Text="" class="btn btn-warning" OnClick="btnConsultar_Click" ><i class="fa fa-search"> CONSULTAR</i></asp:LinkButton>
                                    <asp:LinkButton ID="btnLimpiar" runat="server" Text="" class="btn btn-danger" OnClick="btnLimpiar_Click" ><i class="fa fa-eraser"> LIMPIAR</i></asp:LinkButton>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                         <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="Ingrese parámetros de búsqueda...!!" 
                             AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="7" 
                             OnRowDataBound="dgvDatos_RowDataBound" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged">
                             <Columns>
                                <asp:BoundField DataField="id_ctt_cierre_caja" HeaderText="ID" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fecha_apertura" HeaderText="FECHA CAJA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="hora_apertura" HeaderText="HORA APERTURA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="estado_cierre_caja" HeaderText="ESTADO CAJA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="oficinista" HeaderText="OFICINISTA APERTURA" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="jornada" HeaderText="JORNADA" ItemStyle-HorizontalAlign="Center" />
                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="IMPRIMIR">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnImprimir" runat="server" CommandName="Select" class="btn btn-xs btn-info" OnClick="lbtnImprimir_Click"><i class="fa fa-print"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                 <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="REENVIAR REPORTE">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="lbtnReimprimir" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnReimprimir_Click"><i class="fa fa-send"></i></asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                </div>

                <%--MODAL DE ERRORES--%>
                <div class="modal fade" id="modalError" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label6" runat="server" Text="Información"></asp:Label>
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
