<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmReabrirViaje.aspx.cs" Inherits="Solution_CTT.frmReabrirViaje" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server" EnableScriptGlobalization="True"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="content">                    
                <asp:Panel ID="pnlGrid" runat="server">                    
                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Viajes Normales</h3>
                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 300px;">
                                        <asp:DropDownList ID="cmbFiltrarGrid" runat="server" class="form-control pull-right" AutoPostBack="true" OnSelectedIndexChanged="cmbFiltrarGrid_SelectedIndexChanged"></asp:DropDownList>
                                        <div class="input-group-btn"></div>
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <asp:TextBox ID="txtDate" runat="server" class="form-control pull-right" autocomplete="off" placeholder="Buscar"></asp:TextBox>
                                            <ajaxToolkit:MaskedEditExtender ID="txtDate_MaskedEditExtender" runat="server" BehaviorID="txtDate_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtDate" />
                                            <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" BehaviorID="txtDate_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtDate" />
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid"
                                    AutoGenerateColumns="False"
                                    EmptyDataText="No hay Registros o Coindicencias..!!"
                                    OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="10">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                        <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID" />
                                        <asp:BoundField DataField="INUMEROVIAJE" HeaderText="No. VIAJE" />
                                        <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" />
                                        <asp:BoundField DataField="IVEHICULO" HeaderText="TRANSPORTE" />
                                        <asp:BoundField DataField="IRUTA" HeaderText="RUTA" />
                                        <asp:BoundField DataField="IHORASALIDA" HeaderText="SALIDA" />
                                        <asp:BoundField DataField="IASIENTOSOCUPADOS" HeaderText="ASIENTOS OCUP." />
                                        <asp:BoundField DataField="ITIPOVIAJE" HeaderText="TIPO DE VIAJE" />
                                        <asp:BoundField DataField="IESTADOVIAJE" HeaderText="ESTADO" />
                                        <asp:BoundField DataField="ICHOFER" HeaderText="CHOFER" />
                                        <asp:BoundField DataField="IASISTENTE" HeaderText="ASISTENTE" />
                                        <asp:BoundField DataField="IANDEN" HeaderText="ANDEN" />
                                        <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDREEMPLAZO" HeaderText="ID" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnEdit_Click"><i class="fa fa-bus"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>

                    <div class="col-xs-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Viajes Extras</h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatosExtras" runat="server" class="mGrid"
                                    AutoGenerateColumns="False"
                                    EmptyDataText="No hay Registros o Coindicencias..!!"
                                    OnSelectedIndexChanged="dgvDatosExtras_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatosExtras_PageIndexChanging" PageSize="10">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." />
                                        <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID" />
                                        <asp:BoundField DataField="INUMEROVIAJE" HeaderText="No. VIAJE" />
                                        <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA SALIDA" />
                                        <asp:BoundField DataField="IVEHICULO" HeaderText="TRANSPORTE" />
                                        <asp:BoundField DataField="IRUTA" HeaderText="RUTA" />
                                        <asp:BoundField DataField="IHORASALIDA" HeaderText="SALIDA" />
                                        <asp:BoundField DataField="IASIENTOSOCUPADOS" HeaderText="ASIENTOS OCUP." />
                                        <asp:BoundField DataField="ITIPOVIAJE" HeaderText="TIPO DE VIAJE" />
                                        <asp:BoundField DataField="IESTADOVIAJE" HeaderText="ESTADO" />
                                        <asp:BoundField DataField="ICHOFER" HeaderText="CHOFER" />
                                        <asp:BoundField DataField="IASISTENTE" HeaderText="ASISTENTE" />
                                        <asp:BoundField DataField="IANDEN" HeaderText="ANDEN" />
                                        <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID" />
                                        <asp:BoundField DataField="IIDREEMPLAZO" HeaderText="ID" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEditarExtra" runat="server" CommandName="Select" class="btn btn-xs btn-warning"><i class="fa fa-bus"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                </asp:Panel>

                <!-- Modal MENSAJE-->
                <div class="modal fade" id="myModal1" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <a href="#" data-dismiss="modal" aria-hidden="true" class="close">X</a>
                                <h3 class="alert alert-info"><span class="glyphicon glyphicon-warning-sign"></span>Información</h3>
                            </div>
                            <div class="modal-body">
                                <p>
                                    <asp:Label ID="lblMensaje" runat="server" Text=""></asp:Label>
                                </p>
                            </div>

                            <div class="col-md-offset-8">
                                <%--<button type="button" class="btn btn-success" id="btnYesCancelNotification"><span class="glyphicon glyphicon-ok"></span>Sí</button>--%>
                                <button type="submit" class="btn btn-success" data-dismiss="modal" id="btnNoCancelNotification"><span class="glyphicon glyphicon-remove"></span>OK</button>
                            </div>
                            <br />
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
                                    <asp:Label ID="Label1" runat="server" Text="Información"></asp:Label>
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
                                            <asp:Label ID="Label2" runat="server" Text="Desea reaperturar el viaje"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, reaperturar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

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
<%--FIN MODAL VER PASAJEROS--%>
</asp:Content>
