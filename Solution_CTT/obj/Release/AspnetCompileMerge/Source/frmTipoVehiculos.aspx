<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmTipoVehiculos.aspx.cs" Inherits="Solution_CTT.frmTipoVehiculos" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <!-- Main content -->
            <section class="content">
                <div class="row">
                    <div class="col-md-8">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                <div class="box-tools pull-right">
                                    <div class="input-group input-group-sm" style="width: 150px;">
                                        <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search" autocomplete="off"></asp:TextBox>
                                        <div class="input-group-btn">
                                            <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="box-body">
                                <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging">
                                    <Columns>
                                        <asp:BoundField DataField="IIDTIPOVEHICULO" HeaderText="ID" />
                                        <asp:BoundField DataField="ICODIGO" HeaderText="CÓDIGO" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="IDESCRIPCION" HeaderText="DESCRIPCIÓN" />
                                        <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
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
                        <!-- /.box -->
                    </div>


                    <%--REGISTER--%>
                    <div class="col-md-4">
                        <div class="box box-success">
                            <div class="box-header with-border">
                                <h3 class="box-title"><%= Resources.MESSAGES.TXT_TIPO_VEHICULO %></h3>
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
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtCodigo" runat="server" CssClass="form-control" placeholder="Código *" BackColor="White" autocomplete="off"></asp:TextBox>
                                            </div>
                                            <div class="form-group has-feedback">
                                                <asp:TextBox ID="txtDescripcion" runat="server" CssClass="form-control" placeholder="Descripción *" autocomplete="off"></asp:TextBox>                                                
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
                                    <br />
                                    <div class="row">
                                        <div class="col-md-offset-1 col-md-10">
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
                                <asp:Button ID="btnNo" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAccept" runat="server" Text="Sí, eliminar" class="btn btn-danger" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAccept_Click"/>
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
