<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmAutorizarDocumentosPorLote.aspx.cs" Inherits="Solution_CTT.frmAutorizarDocumentosPorLote" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">
                <section class="box-default">
                    <div class="row">
                        <div class="col-md-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-file-code-o"></i>
                                    <h3 class="box-title">Enviar y Autorizar Documentos por Lotes</h3>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-2 form-group">
                                            <label>Localidad</label>
                                            <div class="input-group col-sm-12">
                                                <asp:DropDownList ID="cmbLocalidad" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                <%--<span class="input-group-addon input-sm"><i class="fa fa-home"></i></span>--%>
                                            </div>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <label>Fecha de Inicio</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtFechaInicial" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaInicial_MaskedEditExtender" runat="server" BehaviorID="txtFechaInicial_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaInicial" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaInicial_CalendarExtender" runat="server" BehaviorID="txtFechaInicial_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaInicial" />
                                                <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>
                                        <div class="col-md-2 form-group">
                                            <label>Fecha de Final</label>
                                            <div class="input-group col-sm-12">
                                                <asp:TextBox ID="txtFechaFinal" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip=""></asp:TextBox>
                                                <ajaxToolkit:MaskedEditExtender ID="txtFechaFinal_MaskedEditExtender" runat="server" BehaviorID="txtFechaFinal_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaFinal" />
                                                <ajaxToolkit:CalendarExtender ID="txtFechaFinal_CalendarExtender" runat="server" BehaviorID="txtFechaFinal_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaFinal" />
                                                <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                            </div>
                                        </div>

                                        <%--BOTON DE EXTRAER DOCUMENTOS--%>
                                        <div class="col-md-2 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnExtraerDocumentosXML" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para extraer las facturas a enviar" OnClick="lbtnExtraerDocumentosXML_Click"><i class="fa fa-pencil-square-o"></i>  Extraer Documentos</asp:LinkButton>
                                            </div>
                                        </div>

                                        <%--BOTON DE AUTORIZAR--%>
                                        <div class="col-md-2 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnAutorizarXML" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para autorizar XML seleccionados" OnClick="lbtnAutorizarXML_Click"><i class="fa fa-pencil-square-o"></i>  Autorizar Facturas</asp:LinkButton>
                                            </div>
                                        </div>

                                        <%--BOTON DE CANCELAR--%>
                                        <div class="col-md-2 form-group">
                                            <label></label>
                                            <div class="input-group col-sm-12">
                                                <asp:LinkButton ID="lbtnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="lbtnCancelar_Click"><i class="fa fa-circle-o-notch"></i>   Cancelar</asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <div class="box">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Registros Encontrados
                                        <asp:Label ID="lbRegistrosEncontrados" runat="server" Text=""></asp:Label></h3>
                                </div>
                                <div class="box-body">
                                    <div class="scrolling-table-container" runat="server" id="Scroll" visible="false">
                                        <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                            <Columns>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:CheckBox ID="chkTodos" runat="server" Text="Todos" AutoPostBack="True" OnCheckedChanged="chkTodos_CheckedChanged" ItemStyle-HorizontalAlign="Center" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkSeleccionar" runat="server" ItemStyle-HorizontalAlign="Center"  />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="IDFactura" HeaderText="IDFactura" />
                                                <asp:BoundField DataField="FechaFactura" HeaderText="Fecha Emisión" />
                                                <asp:BoundField DataField="ClaveAcceso" HeaderText="Clave Acceso" />
                                                <asp:BoundField DataField="NumeroFactura" HeaderText="Número Factura" />
                                                <asp:BoundField DataField="Cliente" HeaderText="Nombre Cliente" />
                                                <asp:BoundField DataField="CorreoCliente" HeaderText="Correo Cliente" />
                                                <asp:BoundField DataField="Mensaje" HeaderText="Mensaje" />

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblGeneradoGrid" runat="server" Text="G." ToolTip="XML Generado" ItemStyle-HorizontalAlign="Center"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkGenerado" runat="server" ItemStyle-HorizontalAlign="Center" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblFirmadoGrid" runat="server" Text="F." ToolTip="XML Firmado" ItemStyle-HorizontalAlign="Center"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkFirmado" runat="server" ItemStyle-HorizontalAlign="Center" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblEnviadoGrid" runat="server" Text="E." ToolTip="Enviado al SRI" ItemStyle-HorizontalAlign="Center"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkEnviado" runat="server" ItemStyle-HorizontalAlign="Center" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <asp:Label ID="lblAutorizadoGrid" runat="server" Text="A." ToolTip="Autorizado" ItemStyle-HorizontalAlign="Center"></asp:Label>
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <asp:CheckBox ID="chkAutorizado" runat="server" ItemStyle-HorizontalAlign="Center" Enabled="false" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>

                                            </Columns>
                                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                        </asp:GridView>
                                    </div>
                                </div>
                                <div class="box-footer">
                                    <%--<div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnAutorizarXML" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para autorizar XML seleccionados" OnClick="lbtnAutorizarXML_Click"><i class="fa fa-pencil-square-o"></i>  Autorizar XML's</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-md-2 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="lbtnCancelar_Click"><i class="fa fa-circle-o-notch"></i>   Cancelar</asp:LinkButton>
                                        </div>
                                    </div>--%>
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
<%--    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="modal-backdrop">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>--%>
</asp:Content>
