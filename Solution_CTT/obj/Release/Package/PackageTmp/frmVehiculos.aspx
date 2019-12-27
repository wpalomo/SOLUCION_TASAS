<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmVehiculos.aspx.cs" Inherits="Solution_CTT.frmVehiculos" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
            <!-- Main content -->
            <section class="content">

                <%--INICIO DEL PANEL DE GRID--%>
                <asp:Panel ID="pnlGrid" runat="server">
                    <div class="row">
                        <%--SECCION NEW--%>
                        <div class="col-md-2">
                            <div class="box box-danger">
                                <asp:LinkButton ID="lbtnNuevo" runat="server" class="btn btn-block btn-danger btn-lg" OnClick="lbtnNuevo_Click"><span class="fa fa-plus"></span> Nuevo</asp:LinkButton>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title"><%= Resources.MESSAGES.TXT_DATA %></h3>

                                    <div class="box-tools pull-right">
                                        <div class="input-group input-group-sm" style="width: 150px;">
                                            <asp:TextBox ID="txtFiltrar" runat="server" class="form-control pull-right" placeholder="Search"></asp:TextBox>
                                            <div class="input-group-btn">
                                                <asp:LinkButton ID="btnFiltrar" runat="server" class="btn btn-default" OnClick="btnFiltrar_Click" ><i class="fa fa-search"></i></asp:LinkButton>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-body">
                                    <asp:GridView ID="dgvDatos" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="true" PageSize="8" OnPageIndexChanging="dgvDatos_PageIndexChanging" OnRowDataBound="dgvDatos_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="IIDVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDTIPOVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="ITIPOVEHICULO" HeaderText="TIPO" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDMARCAVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IMARCAVEHICULO" HeaderText="MARCA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDMODELOVEHICULO" HeaderText="ID" />
                                            <asp:BoundField DataField="IMODELOVEHICULO" HeaderText="MODELO" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDTIPOBUS" HeaderText="ID" />
                                            <asp:BoundField DataField="ITIPOBUS" HeaderText="TIPO DE BUS" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDDISCO" HeaderText="ID" />
                                            <asp:BoundField DataField="IDISCO" HeaderText="DISCO" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IPLACA" HeaderText="PLACA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="ICHASIS" HeaderText="CHASIS" />
                                            <asp:BoundField DataField="IMOTOR" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IANIOPRODUCCION" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IPAISORIGEN" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="ICILINDRAJE" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IPESO" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="INUMEROPASAJEROS" HeaderText="MOTOR" />
                                            <asp:BoundField DataField="IESTADO" HeaderText="ESTADO" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="IIDFORMATOASIENTO" HeaderText="ID" />
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
                    </div>
                </asp:Panel>

                <%--INICIO DE PANEL DE REGISTRO--%>
                <asp:Panel ID="pnlRegistro" runat="server" Visible="false">
                    <div class="row">
                        <div class="col-xs-12">
                            <div class="box box-primary">
                                <div class="box-header with-border">
                                    <i class="fa fa-table"></i>
                                    <h3 class="box-title">Información del vehículo</h3>
                                </div>

                                <div class="box-body">
                                    <div class="form-group">

                                        <%--PRIMERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label18" runat="server" Text="Seleccione el tipo de vehículo"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTipoVehiculo" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label19" runat="server" Text="Seleccione la marca del vehículo"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbMarca" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>--%>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label20" runat="server" Text="Seleccione el modelo del vehículo"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbModelo" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>--%>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN PRIMERA FILA--%>

                                        <%--SEGUNDA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label23" runat="server" Text="Seleccione el tipo de asientos"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbTipoAsiento" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label24" runat="server" Text="Seleccione el disco para asignar"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbDisco" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label25" runat="server" Text="Seleccione el formato de asientos"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbFormatoAsiento" runat="server" class="form-control"></asp:DropDownList>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN SEGUNDA FILA--%>
                                        
                                        <%--TERCERA FILA--%>
                                        <div class="row">
                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label26" runat="server" Text="Ingrese el chasis del vehículo *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtChasis" runat="server" class="form-control" placeholder="Chasis del Vehículo" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-send"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-6">
                                                <div class="form-group">
                                                    <asp:Label ID="Label27" runat="server" Text="Ingrese el motor del vehículo *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtMotor" runat="server" class="form-control" placeholder="Motor del Vehículo" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-send"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN TERCERA FILA--%>

                                        <%--CUARTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label1" runat="server" Text="Ingrese el año de producción *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtAnioProduccion" runat="server" class="form-control" placeholder="Año de Producción" Style="text-transform: uppercase" onkeypress="return validar_numeros(event)" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label6" runat="server" Text="Ingrese el país de origen *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPaisOrigen" runat="server" class="form-control" placeholder="País de Origen" Style="text-transform: uppercase" onkeypress="return validar_letras(event)" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-4">
                                                <div class="form-group">
                                                    <asp:Label ID="Label7" runat="server" Text="Ingrese el número de pasajeros *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtNumeroPasajeros" runat="server" class="form-control" placeholder="Número de Pasajeros" onkeypress="return validar_numeros(event)" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN CUARTA FILA--%>

                                        <%--QUINTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label21" runat="server" Text="Ingrese el cilindraje del vehículo *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtCilindraje" runat="server" class="form-control" placeholder="Cilindraje" Onkeypress="return ValidaDecimal(this.value);" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label22" runat="server" Text="Ingrese el peso del vehículo *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPeso" runat="server" class="form-control" placeholder="Peso" Onkeypress="return ValidaDecimal(this.value);" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="Label28" runat="server" Text="Ingrese la placa del vehículo *"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:TextBox ID="txtPlaca" runat="server" class="form-control" placeholder="Placa" Style="text-transform: uppercase" autocomplete="off" ></asp:TextBox>
                                                        <%--<span class="input-group-addon input-sm"><i class="fa fa-male"></i></span>--%>
                                                    </div>                                                
                                                </div>
                                            </div>

                                            <div class="col-md-3">
                                                <div class="form-group">
                                                    <asp:Label ID="lblEtiquetaEstado" runat="server" Text="Estado"></asp:Label>
                                                    <div class="input-group col-sm-12">
                                                        <asp:DropDownList ID="cmbEstado" runat="server" class="form-control input-sm">
                                                            <asp:ListItem Value="A">ACTIVO</asp:ListItem>
                                                            <asp:ListItem Value="N">INACTIVO</asp:ListItem>
                                                        </asp:DropDownList>
                                                        <span class="input-group-addon input-sm"><i class="fa fa-search"></i></span>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                        <%--FIN QUINTA FILA--%>

                                         <%--SEXTA FILA--%>
                                        <div class="row">
                                            <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label10" runat="server" Text="Fecha Emisión Matrícula"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtFechaEmisionMatricula" runat="server" class="form-control" BackColor="White"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaEmisionMatricula_MaskedEditExtender" runat="server" BehaviorID="txtFechaEmisionMatricula_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaEmisionMatricula" />
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-4">
                                            <div class="form-group">
                                                <asp:Label ID="Label2" runat="server" Text="Fecha Caducidad Matrícula"></asp:Label>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtFechaCaducidadMatricula" runat="server" class="form-control" BackColor="White"></asp:TextBox>
                                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaCaducidadMatricula_MaskedEditExtender" runat="server" BehaviorID="txtFechaCaducidadMatricula_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaCaducidadMatricula" />
                                                </div>
                                            </div>
                                        </div>
                                        </div>
                                        <%--FIN SEXTA FILA--%>
                                    </div>
                                </div>

                                <div class="box-footer">
                                    <asp:Button ID="btnGuardar" runat="server" Text="Guardar Registro" class="btn btn btn-success" OnClick="btnGuardar_Click" />
                                    <asp:Button ID="btnLimpiar" runat="server" Text="Limpiar" class="btn btn btn-warning" OnClick="btnLimpiar_Click" />
                                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" class="btn btn btn-danger" OnClick="btnRegresar_Click" />
                                </div>

                            </div>
                        </div>
                    </div>
                </asp:Panel>                

                <div class="modal fade" id="QuestionModal" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label3" runat="server" Text="¿ Está seguro ?"></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="Label16" runat="server" Text="Desea eliminar el registro, puede que no sea recuperable"></asp:Label>
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
                                    <asp:Label ID="Label17" runat="server" Text="Información"></asp:Label>
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
