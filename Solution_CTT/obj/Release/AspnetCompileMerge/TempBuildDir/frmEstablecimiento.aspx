<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmEstablecimiento.aspx.cs" Inherits="Solution_Encomiendas.frmEstablecimiento" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="box-default">
                <div class="row">
                    <div class="col-md-8">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-user"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" OnRowDataBound="dgvDatos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="Item" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ICODIGO" HeaderText="CÓDIGO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="INOMBRES" HeaderText="DESCRIPCIÓN" />
                                        <asp:BoundField DataField="IEST" HeaderText="ESTABLECIMIENTO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IPOT_EMI" HeaderText="PUNTO EMISIÓN" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IDIRECCION" HeaderText="DIRECCIÓN" />
                                        <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IID_LOCALIDAD" HeaderText="IDLOCALIDAD" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnEdit_Click"><i class="fa fa-pencil"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="BORRAR">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnDelete" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnDelete_Click"><i class="fa fa-trash"></i></asp:LinkButton>
                                            </ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                    </div>
                    <div class="col-md-4">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_ESTABLECIMIENTOS %></h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-offset-1 col-md-10">
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" autocomplete="off" placeholder="Código *"></asp:TextBox>
                                        </div>
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtNombres" runat="server" CssClass="form-control" autocomplete="off" placeholder="Nombres *"></asp:TextBox>
                                        </div>
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtEstablecimiento" runat="server" CssClass="form-control" autocomplete="off" placeholder="Establecimeinto *"></asp:TextBox>
                                        </div>
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtPuntoEmision" runat="server" CssClass="form-control" autocomplete="off" placeholder="Punto de Emisión *"></asp:TextBox>
                                        </div>
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtDireccion" runat="server" CssClass="form-control" autocomplete="off" placeholder="Dirección *"></asp:TextBox>
                                        </div>
                                        <div class="form-group has-feedback">
                                            <asp:TextBox ID="txtEstado" runat="server" CssClass="form-control" autocomplete="off" placeholder="Estado" ReadOnly="True"></asp:TextBox>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-offset-1 col-md-5">
                                        <div class="form-group">
                                            <asp:Button ID="btnCancel" runat="server" Text="Cancelar" class="btn btn-sm btn-default btn-block pull-right" OnClick="btnCancel_Click" />
                                        </div>
                                    </div>
                                    <div class=" col-md-5">
                                        <div class="form-group">
                                            <asp:Button ID="btnSave" runat="server" Text="Crear" class="btn btn-sm btn-primary btn-block pull-right" OnClick="btnSave_Click" />
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
