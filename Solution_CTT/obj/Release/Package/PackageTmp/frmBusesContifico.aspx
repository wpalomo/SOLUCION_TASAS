<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmBusesContifico.aspx.cs" Inherits="Solution_CTT.frmBusesContifico" %>
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
                                <h3 class="box-title">Buses del Sistema SMARTT</h3>
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

                                    <asp:Panel ID="pnlRegistros" runat="server" Visible="false">
                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label18" runat="server" Text="Seleccione el Bus"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbBuses" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbBuses_SelectedIndexChanged" class="form-control input-sm"></asp:DropDownList>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label9" runat="server" Text="Disco"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtDisco" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="DISCO" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label10" runat="server" Text="Placa"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPlaca" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="PLACA" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label11" runat="server" Text="Conductor"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtConductor" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="CONDUCTOR" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label4" runat="server" Text="Capacidad"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCapacidad" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="CAPACIDAD" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label3" runat="server" Text="Año de Fabricación"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtAnioFabricacion" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="AÑO DE FABRICACIÓN" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label5" runat="server" Text="Marca del Bus"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMarca" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="MARCA DEL BUS" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Fecha Emisión de Matrícula"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaEmision" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="FECHA EMISIÓN MATRÍCULA" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Fecha Caducidad de Matrícula"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtFechaCaducidad" ReadOnly="true" runat="server" class="form-control input-sm" placeholder="FECHA CADUCIDAD MATRÍCULA" BackColor="White"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </asp:Panel>

                                    <%--CUARTA FILA--%>
                                    <div class="row">
                                        <div class="col-md-12">
                                            <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!">
                                            <Columns>
                                                <asp:BoundField DataField="disco" HeaderText="DISCO" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="conductor" HeaderText="CONDUCTOR" />
                                                <asp:BoundField DataField="capacidad" HeaderText="CAPACIDAD" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="anio_fabricacion" HeaderText="AÑO FABRICACION" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="placa" HeaderText="PLACA" />
                                                <asp:BoundField DataField="marca_nombre" HeaderText="MARCA" />
                                                <asp:BoundField DataField="fecha_emision_matricula" HeaderText="FECHA EMI. MATRÍCULA" ItemStyle-HorizontalAlign="Center" />
                                                <asp:BoundField DataField="fecha_vencimiento_matricula" HeaderText="FECHA CAD. MATRÍCULA" ItemStyle-HorizontalAlign="Center" />
                                            </Columns>
                                            </asp:GridView>
                                        </div>
                                    </div>                        
                                </div>
                            </div>

                            <div class="modal-footer">
                                <div class="form-group">
                                    <asp:Button ID="btnAnterior" runat="server" Text="Anterior" class="btn btn btn-warning" Visible="false" OnClick="btnAnterior_Click" />
                                    <asp:Button ID="btnSiguiente" runat="server" Text="Siguiente" class="btn btn btn-success" OnClick="btnSiguiente_Click" />
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
