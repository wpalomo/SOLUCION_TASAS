<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPueblosHorarios.aspx.cs" Inherits="Solution_CTT.frmPueblosHorarios" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager2" runat="server"></asp:ScriptManager>--%>
            <section class="content">
                <div class="row">
                    <div class="col-md-8">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>

                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Buscar"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid"
                                AutoGenerateColumns="False"
                                EmptyDataText="No hay Registros o Coindicencias..!!"
                                OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="10" OnRowDataBound="dgvDatos_RowDataBound">
                                    <Columns>
                                        <asp:BoundField DataField="INUMERO" HeaderText="No." ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IIDPUEBLO" HeaderText="ID" />
                                        <asp:BoundField DataField="IDESCRIPCION" HeaderText="TERMINAL" />
                                        <asp:BoundField DataField="IPROVINCIA" HeaderText="PROVINCIA" />
                                        <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="ICUENTA" HeaderText="No. REGISTROS" ItemStyle-HorizontalAlign="Center" />
                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                            <ItemTemplate>
                                                <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="Select" class="btn btn-xs btn-success"><i class="fa fa-bus"></i></asp:LinkButton>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                </asp:GridView>
                            </div>
                        </div>
                        <!-- /.box -->
                    </div>
                    <%--REGISTER--%>
                    <div class="col-md-4">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title">
                                    <asp:Label ID="lblHorarios" runat="server" Text="Seleccione Terminal"></asp:Label></h3>
                                <div class="box-tools pull-right">
                                    <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                        <i class="fa fa-minus"></i>
                                    </button>
                                    <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                                </div>
                            </div>
                            <%--FORM--%>
                            <div class="box-body">
                                <div class="register-box-body">
                                    <div class="row">
                                        <div class="col-md-offset-1 col-md-10">
                                            <asp:Panel ID="pnlHorarios" Enabled="false" runat="server">
                                                <asp:GridView ID="dgvHorarios" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" BackColor="LightGoldenrodYellow" BorderColor="Tan" BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None">
                                                    <AlternatingRowStyle BackColor="PaleGoldenrod" />
                                                    <Columns>
                                                        <asp:BoundField DataField="IIDHORARIO" HeaderText="ID" />
                                                        <asp:BoundField DataField="IHORA" HeaderText="HORA" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="IJORNADA" HeaderText="JORNADA" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="Sel.">
                                                            <ItemTemplate>
                                                                <asp:CheckBox ID="chkSeleccionar" runat="server" CommandName="Select" />
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="Tan" />
                                                    <HeaderStyle BackColor="Tan" Font-Bold="True" />
                                                    <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" />
                                                    <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
                                                    <SortedAscendingCellStyle BackColor="#FAFAE7" />
                                                    <SortedAscendingHeaderStyle BackColor="#DAC09E" />
                                                    <SortedDescendingCellStyle BackColor="#E1DB9C" />
                                                    <SortedDescendingHeaderStyle BackColor="#C2A47B" />
                                                </asp:GridView>
                                            </asp:Panel>
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
                                    <br />
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
                                    <asp:Label ID="Label5" runat="server" Text="Información"></asp:Label>
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
