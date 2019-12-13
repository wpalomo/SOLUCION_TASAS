
<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmTransacciones.aspx.cs" Inherits="Solution_CTT.frmTransacciones" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <script type="text/javascript">
        function doClick(buttonName, e) {
            var key; lblMensajeError
            if (window.event)
                key = window.event.keyCode;
            else
                key = e.which;
            if (key == 13) {
                var btn = document.getElementById(buttonName);
                if (btn != null) {
                    btn.click();
                    event.keyCode = 0
                }
            }
        }
    </script>
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

                                            <div class="input-group-btn">
                                                <%--<asp:Button ID="btnFiltrarGrid" runat="server" Text="FILTRAR" class="btn btn-default" OnClick="btnFiltrarGrid_Click" />--%>
                                            </div>
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
                                        OnSelectedIndexChanged="dgvDatos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatos_PageIndexChanging" PageSize="10" OnRowDataBound="dgvDatos_RowDataBound">
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
                                            <asp:BoundField DataField="IIDPUEBLOORIGEN" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDPUEBLODESTINO" HeaderText="ID" />
                                            <asp:BoundField DataField="ICOBROADMINISTRATIVO" HeaderText="COBRO ADMINISTATIVO" />
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
                                        OnSelectedIndexChanged="dgvDatosExtras_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvDatosExtras_PageIndexChanging" PageSize="10" OnRowDataBound="dgvDatosExtras_RowDataBound">
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
                                            <asp:BoundField DataField="IIDPUEBLOORIGEN" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDPUEBLODESTINO" HeaderText="ID" />
                                            <asp:BoundField DataField="ICOBROADMINISTRATIVO" HeaderText="COBRO ADMINISTATIVO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditarExtra" runat="server" CommandName="Select" class="btn btn-xs btn-success"><i class="fa fa-bus"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                </asp:Panel>

                <div class="row">
                <asp:Panel ID="pnlBus" runat="server" Visible="false">
                    <section class="col-md-9">
                        <%--PANEL DE VENTA--%>
                        <div class="form-horizontal">
                            <div class="box box-success">
                                <div class="box-header with-border text-center">
                                    <h3 class="box-title">
                                        <%--<asp:LinkButton ID="LinkButton1" runat="server"><i class="fa fa-arrow-circle-left"></i></asp:LinkButton>--%>
                                        <asp:Label ID="lblDetalleBus2" runat="server" Text=""></asp:Label></h3>
                                        <%--<asp:LinkButton ID="LinkButton2" runat="server"><i class="fa fa-arrow-circle-right"></i></asp:LinkButton>--%>
                                    <%--<asp:LinkButton ID="btnAbrirModalViajes" runat="server"><i class="fa fa-plane"></i></asp:LinkButton>--%>
                                </div>
                                <div class="box-body">
                                    <div class="row">
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">Origen:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:DropDownList ID="cmbOrigen" runat="server" class="form-control"></asp:DropDownList>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Tipo ID:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:DropDownList ID="cmbTipoIdentificacion" runat="server" class="form-control input-sm"></asp:DropDownList>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-file"></i></span>
                                                </div>
                                            </div>
                                            <asp:Panel ID="Panel10" runat="server" DefaultButton="btnBuscarCliente">
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label">CI / RUC:</label>
                                                    <div class="input-group col-sm-8">                                                    
                                                        <asp:TextBox ID="txtIdentificacion" runat="server" class="form-control input-sm" autocomplete="off" placeholder="IDENTIFICACIÓN" tooltip="Para buscar presione la lupa" OnTextChanged="txtIdentificacionRegistro_TextChanged" onkeypress="return validar_numeros(event)"></asp:TextBox>                                                    
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnBuscarCliente" runat="server" Text="" OnClick="btnBuscarCliente_Click" tooltip="Buscar Cliente"><i class="fa fa-search"></i></asp:LinkButton></span>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnEditarPasajero" runat="server" Text="" tooltip="Agregar / Editar Cliente" OnClick="btnEditarPasajero_Click"><i class="fa fa-user-plus"></i></asp:LinkButton></span>
                                                        <span class="input-group-addon input-sm"><asp:LinkButton ID="btnConsumimdorFinal" runat="server" Text="" tooltip="Consumidor Final" OnClick="btnConsumimdorFinal_Click">C.F.</asp:LinkButton></span>
                                                    </div>                                                
                                                </div>
                                            </asp:Panel>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">Pasajero:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:TextBox ID="txtNombrePasajero" runat="server" class="form-control input-sm" autocomplete="off" placeholder="NOMBRE DEL PASAJERO" Style="text-transform: uppercase" onkeypress="return validar_letras(event)"></asp:TextBox>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-user"></i></span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">Edad:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:Label ID="lblEdad" runat="server" Text="SIN ASIGNAR" class="form-control input-sm" BackColor="White" Font-Bold="true"></asp:Label>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-calendar"></i></span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">Pasajeros:</label>
                                                <div class="input-group col-sm-8">                                                    
                                                    <asp:LinkButton ID="lbtnListaPasajeros" runat="server" class="form-control input-sm" tooltip="Clic para ver lista de pasajeros vendidos" OnClick="lbtnListaPasajeros_Click">VER LISTA PASAJEROS VENDIDOS</asp:LinkButton>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-pie-chart"></i></span>--%>
                                                    <span class="input-group-addon input-sm">
                                                        <asp:Label ID="lblCantidadVendida" runat="server" Text=""></asp:Label></i></span>
                                                </div>
                                            </div>                                            
                                        </div>                                    
                                        <%--RIGHT--%>
                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Destino:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:DropDownList ID="cmbDestino" runat="server" class="form-control" AutoPostBack="true" OnSelectedIndexChanged="cmbDestino_SelectedIndexChanged"></asp:DropDownList>
                                                    <span class="input-group-addon input-sm"><i class="fa fa-car"></i></span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">T. Cliente:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:DropDownList ID="cmbTipoCliente" runat="server" class="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoCliente_SelectedIndexChanged" ></asp:DropDownList>                                                    
                                                    <span class="input-group-addon input-sm"><i class="fa fa-users"></i></span>
                                                </div>                                                
                                            </div>                                            
                                             <div class="form-group">
                                                <label class="col-sm-3 control-label">P. Oficial:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:TextBox ID="txtPrecio" runat="server" BackColor="White" ReadOnly="true" class="form-control input-sm" Text="0.00" placeholder="Precio" ></asp:TextBox>
                                                    <span class="input-group-addon input-sm">$</span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">Descuento:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:TextBox ID="txtDescuento" runat="server" BackColor="White" ReadOnly="true" class="form-control input-sm" Text="0.00" placeholder="Descuento" ></asp:TextBox>
                                                    <span class="input-group-addon input-sm">
                                                        <asp:CheckBox ID="chkCortesia" runat="server" Text="&nbsp&nbspCortesía" AutoPostBack="true" />    
                                                    </span>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label class="col-sm-3 control-label">P. Final:</label>
                                                <div class="input-group col-sm-8">
                                                    <asp:TextBox ID="txtPrecioFinal" runat="server" BackColor="White" ReadOnly="true" class="form-control input-sm" Text="0.00" placeholder="PVP" Style="text-transform: uppercase" Font-Bold="True"></asp:TextBox>
                                                    <span class="input-group-addon input-sm">$</span>
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnlVerTasas" runat="server" Visible="false">
                                                <div class="form-group">
                                                    <label class="col-sm-3 control-label">Info Tasas</label>
                                                    <div class="input-group col-sm-8">                                                        
                                                        <asp:TextBox ID="txtCantidadTasasDisponibles" runat="server" ForeColor="White" class="form-control input-sm" Text="" ToolTip="Tasas de Usuario Disponible para el oficinista logueado." ReadOnly="true" Font-Bold="True" style="text-align:center;"></asp:TextBox>                                                        
                                                        <span class="input-group-addon input-sm" style="background-color:#5df127"><asp:LinkButton ID="btnValidacionToken" runat="server" Text="" tooltip="Ingreso y validación de Token" OnClick="btnValidacionToken_Click"><i class="fa fa-cloud" style="color:white;"></i></asp:LinkButton></span>
                                                        <%--<span class="input-group-addon input-sm"><asp:LinkButton ID="btnNotificacionToken" runat="server" Text="" tooltip="Notificación de Tasas de Usuario" OnClick="btnNotificacionToken_Click"><i class="fa fa-plus"></i></asp:LinkButton></span>--%>
                                                        <asp:TextBox ID="txtPorcentajeDisponibles" runat="server" ForeColor="White" class="form-control input-sm" Text="" ToolTip="Porcentaje de Tasas de Usuario Disponible para el oficinista logueado." ReadOnly="true" Font-Bold="True" style="text-align:center;"></asp:TextBox>
                                                        <span class="input-group-addon input-sm" style="background-color:#f237d8"><asp:LinkButton ID="btnReporteToken" runat="server" Text="" tooltip="Reporte de Token" onclick="btnReporteToken_Click"><i class="fa fa-print" style="color:white;"></i></asp:LinkButton></span>
                                                        <asp:TextBox ID="txtTasaUsuario" runat="server" class="form-control input-sm" Text="0" ReadOnly="true" placeholder="Tasas de Usuario que se emitirán en el boleto." Font-Bold="True" style="text-align:center; background-color:#fcded5"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </div>                                    
                                </div>

                                <%--BOTONES--%>
                                <div class="box-footer">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="col-md-3 col-sm-12 col-xs-12">
                                                <asp:Label ID="lblTotalCobradoBus" runat="server" Text="COBRADO: $ 0.00" class="form-control input-sm" BackColor="Blue" Font-Bold="true" ForeColor="White" ToolTip="Total cobrado en el viaje" style="text-align:center;"></asp:Label>                                                
                                            </div>
                                            <div class="col-md-2 col-sm-12 col-xs-12">
                                                <asp:Button ID="btnLimpiarAsignacion" runat="server" Text="Limpiar" data-backdrop="false" class="btn btn-block btn-danger" OnClick="btnLimpiarAsignacion_Click" ToolTip="Clic aquí para limpiar el formulario" />
                                            </div>
                                            <div class="col-md-2 col-sm-12 col-xs-12">
                                                <asp:Button ID="btnAbrirModalViajes" runat="server" Text="Otro Viaje" data-backdrop="false" class="btn btn-block btn-info" OnClick="btnAbrirModalViajes_Click" ToolTip="Clic aquí para seleccionar o ver los viajes activos" />
                                            </div>
                                            <div class="col-md-3 col-sm-12 col-xs-12">
                                                <asp:Button ID="btnFacturaDatos" runat="server" Text="Factura con Datos" data-backdrop="false" class="btn btn-block btn-warning" ToolTip="Clic aquí para facturar con datos" OnClick="btnFacturaDatos_Click" />                                                
                                            </div>

                                            <div class="col-md-2 col-sm-12 col-xs-12">
                                                <asp:Button ID="btnFacturaRapida" runat="server" Text="Facturar" data-backdrop="false" class="btn btn-block btn-success " OnClick="btnFacturaRapida_Click" />
                                            </div>
                                        </div>
                                   </div>
                                </div>
                                <%--LEYENDA--%>
                                <div class="box-footer">
                                    <div class="box-header text-center">
                                        <h3 class="box-title">Leyenda Tarifas</h3>
                                    </div>
                                        <div class="row center-block">
                                            <div class="col-md-12 ">
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-lime">CONVENCIONAL</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-red">MENOR DE EDAD</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-fuchsia">TERCERA EDAD</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-blue">DISCAPACIDAD</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-black-gradient">TERMINALES</span>
                                                </div>
                                                <div class="col-md-2">
                                                    <span class="badge btn-block bg-purple-gradient">OTROS</span>
                                                </div>
                                            </div>
                                        </div>                                   
                                </div>
                            </div>                            
                        </div>

                        <%--CIERRE PANEL VENTA--%>


                        <div class="form-horizontal">
                            <div class="box box-success">
                                <div class="box-header">
                                    <i class="fa fa-car"></i>
                                    <h3 class="box-title">Detalle del Viaje</h3>
                                </div>
                                <div class="box-body">
                                    <div class="form-group">

                                        <%--IZQUIERDA--%>
                                        <div class="col-md-6 pull-left">
                                            <div class="form-group">
                                                <label for="inputEmail3" class="col-sm-3 control-label">No. Viaje:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtNumeroViaje" runat="server" class="form-control input-sm" placeholder="No. de Viaje" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Fecha:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtFechaViaje" runat="server" class="form-control input-sm" placeholder="Fecha del Viaje" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Transporte:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtTransporteViaje" runat="server" class="form-control input-sm" placeholder="Transporte" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    
                                        <%--RIGHT--%>
                                        <div class="col-md-6 pull-right">
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Viaje Día:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtViajeDia" runat="server" class="form-control input-sm" placeholder="No. Viaje del día" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Hora:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtHoraViaje" runat="server" class="form-control input-sm" placeholder="Hora de Salida" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                            <div class="form-group">
                                                <label for="inputPassword3" class="col-sm-3 control-label">Ruta:</label>
                                                <div class="col-sm-9">
                                                    <asp:TextBox ID="txtRutaViaje" runat="server" class="form-control input-sm" placeholder="Ruta" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                
                                <%--BOTONES--%>
                                    <div class="box-footer">
                                        <div class="row">
                                            <div class="col-md-12">                                                
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnCerrarViaje" runat="server" Text="Cerrar Viaje" data-backdrop="false" class="btn btn-block btn-success" OnClick="btnCerrarViaje_Click" />
                                                </div>
                                                <div class="col-md-2">
                                                    <asp:Button ID="btnRegresar" runat="server" Text="Regresar" class="btn btn-block btn-danger" OnClick="btnRegresar_Click" />
                                                </div>
                                                <div class="col-md-3">
                                                    <asp:Button ID="btnReimprimirFactura" runat="server" Text="Reimprimir Factura" data-backdrop="false" class="btn btn-block btn-warning" OnClick="btnReimprimirFactura_Click" />
                                                </div>
                                                <div class="col-md-4">
                                                    <asp:Button ID="btnGenerarTasaAcompanante" runat="server" Text="Generar Tasa Acompañante" class="btn btn-block btn-info" Visible="false" ToolTip="Se generará una tasa de usuario dependieno la cantidad ingresada en la caja de texto." OnClick="btnGenerarTasaAcompanante_Click" />
                                                </div>
                                                <div class="col-md-1">
                                                    <asp:TextBox ID="txtCantidadTasasAcompanate" class="form-control form-control-sm" runat="server" Visible="false" Text="1"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--FIN BOTONES--%>

                            </div>                            
                        </div>

                    </section>

                    <div class="col-md-3">
                        <div class="box box-danger">
                            <div class="box-header with-border text-center">
                                <h3 class="box-title">
                                    <asp:LinkButton ID="lbtnNuevaHora" runat="server" OnClick="lbtnNuevaHora_Click"><asp:Label ID="lblDetalleBus1" runat="server" Text=""></asp:Label></asp:LinkButton>                                    
                                </h3>
                            </div>
                            <%--BUS FONDO style="background: url(assets/img/Backbus1.png) no-repeat;"--%>
                            <div class="box-body text-center" style="background: url(assets/img/Backbus1.png) no-repeat; background-position:top; ">
                                <div class="box-body text-center">
                                    <p></p>
                                    <br />
                                    <br />
                                    <br />
                                    <br />
                                    <asp:Panel ID="pnlAsientos" runat="server" Height="520px">
                                    </asp:Panel>
                                </div>
                            </div>
                        </div>
                    </div>
                </asp:Panel>
                </div>

                <div class="col-xs-12">
                    <asp:Panel ID="pnlCierreViaje" runat="server" Visible="false">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">CIERRE DE VIAJE  -  <asp:Label ID="lblEtiquetaCierre" runat="server" Text=""></asp:Label></h3>
                            </div>

                            <asp:Panel ID="pnlMostrarPagosPendientes" runat="server">

                                <div class="box-body">
                                    <div class="form-group">
                                        <%--FILA DEL GRID--%>
                                        <div class="row">
                                            <div class="col-md-12">
                                                <div class="form-group">
                                                    <div class="col-sm-12">

                                                        <asp:GridView ID="dgvDetalle" runat="server" class="mGrid" AllowPaging="True" AutoGenerateColumns="False" 
                                                            EmptyDataText="No hay Registros o Coindicencias..!!" OnPageIndexChanging="dgvDetalle_PageIndexChanging"
                                                            OnSelectedIndexChanged="dgvDetalle_SelectedIndexChanged">
                                                            <AlternatingRowStyle BackColor="White" />
                                                            <Columns>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                                                                    <ItemTemplate>
                                                                        <asp:CheckBox ID="chkSeleccionar" runat="server" CommandName="Select" Enabled="false" />
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:BoundField HeaderText="IDPedido" DataField="id_pedido" />
                                                                <asp:BoundField DataField="estado_pago" HeaderText="Tipo de Pago" />
                                                                <asp:BoundField HeaderText="No. Viaje" DataField="numero_viaje" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField HeaderText="Fecha Viaje" DataField="fecha_viaje" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="hora_salida" HeaderText="Hora Viaje" ItemStyle-HorizontalAlign="Center" />
                                                                <asp:BoundField DataField="abono" HeaderText="Valor Abonado" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:BoundField DataField="precio" HeaderText="Valor Debido" ItemStyle-HorizontalAlign="Right" />
                                                                <asp:BoundField DataField="cg_estado_dcto" HeaderText="Estado Dcto" />
                                                                <asp:TemplateField HeaderText="Valor Abono" >
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtValorAbonoGrid" runat="server" class="form-control input-sm" placeholder="Valor Abono" Onkeypress="return ValidaDecimal(this.value);" Text="" ></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="ABONAR">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnAbonarPago" runat="server" CommandName="Select" class="btn btn-xs btn-info" OnClick="lbtnAbonarPago_Click"><i class="fa fa-plus"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="REMOVER   ">
                                                                    <ItemTemplate>
                                                                        <asp:LinkButton ID="lbtnRemoverPago" runat="server" CommandName="Select" class="btn btn-xs btn-warning" OnClick="lbtnRemoverPago_Click"><i class="fa fa-minus"></i></asp:LinkButton>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Abono" ItemStyle-HorizontalAlign="Right" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblAbonoGrid" runat="server" Text="0.00"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Saldo" ItemStyle-HorizontalAlign="Right" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblSaldoGrid" runat="server" Text="0.00"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                                <asp:TemplateField HeaderText="Ingreso" ItemStyle-HorizontalAlign="Right" >
                                                                    <ItemTemplate>
                                                                        <asp:Label ID="lblIngresoEfectivoFaltante" runat="server" Text="0.00"></asp:Label>
                                                                    </ItemTemplate>
                                                                </asp:TemplateField>
                                                            </Columns>
                                                        </asp:GridView>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>

                                        <%--FIN FILA DEL GRID--%>

                                    </div>
                                </div>

                                <div class="box-footer">
                                    <div class="row">
                                        <div class="col-md-12">
                                            <div class="form-group">
                                                <%--<div class="col-sm-6">
                                                    <span class="pull-left">
                                                        <asp:Label ID="lblSumaCobrar" runat="server" Text="Total a Cobrar: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                                    </span>
                                                </div>--%>
                                                <div class="col-sm-6">
                                                    <span class="pull-left">
                                                        <asp:Label ID="lblSumaRecuperado" runat="server" Text="Total Recuperado: 0.00 $" class="badge bg-light-blue"></asp:Label>
                                                    </span>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                </div>

                            </asp:Panel>

                        </div>

                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-table"></i>
                                <h3 class="box-title">Recálculos</h3>
                            </div>

                            <div class="box-body">
                                <div class="form-group">
                                    <%--PRIMERA FILA--%>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label5" runat="server" Text="TOTAL RECAUDADO"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTotalCobradoModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label4" runat="server" Text="RETENCIÓN"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPagoRetencionModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label13" runat="server" Text="1° TOTAL"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPrimerTotalModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label3" runat="server" Text="ADMINISTRACIÓN"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPagoModal" runat="server" class="form-control" ReadOnly="true" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><asp:LinkButton ID="btnAbonarAdministracion" runat="server" Text="" OnClick="btnAbonarAdministracion_Click" tooltip="Clic aquí para abonar el pago de administración."><i class="fa fa-dollar"></i></asp:LinkButton></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label9" runat="server" Text="INGRESO"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtEfectivoModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);"></asp:TextBox>
                                                    <%--<asp:Panel ID="pnlAgregarPagos" runat="server">--%>
                                                        <span id="agregarFaltante" runat="server" class="input-group-addon input-sm"><asp:LinkButton ID="btnIngresarFaltante" runat="server" Text="" OnClick="btnIngresarFaltante_Click" tooltip="Clic aquí para ingresar el efectivo faltante y cobrar los pagos"><i class="fa fa-dollar"></i></asp:LinkButton></span>
                                                    <%--</asp:Panel>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label10" runat="server" Text="2° TOTAL"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtSegundoTotalModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-user-plus"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
                                    <%--FIN PRIMERA FILA--%>

                                    <br />


                                    <%--SEGUNDA FILA--%>
                                    <div class="row">
                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label8" runat="server" Text="FALTANTE"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtFaltanteModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-save"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label6" runat="server" Text="PAGOS PENDIENTES"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtPagosPendientesModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                    <%--<span class="input-group-addon input-sm"><i class="fa fa-pencil"></i></span>--%>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-2">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label7" runat="server" Text="TOTAL NETO"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtTotalNetoModal" runat="server" class="form-control" Text="0.00" Onkeypress="return ValidaDecimal(this.value);" ReadOnly="true" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>

                                        <div class="col-md-6">
                                            <div class="form-group">
                                                <b><asp:Label ID="Label111" runat="server" Text="OBSERVACIONES"></asp:Label></b>
                                                <div class="input-group col-sm-12">
                                                    <asp:TextBox ID="txtObservacionProgramacion" runat="server" class="form-control" placeholder="Ingrese una observación en caso de que exista novedades." Autocomplete="off" BackColor="White"></asp:TextBox>
                                                </div>
                                            </div>
                                        </div>   
                                    </div>
                                    <%--FIN SEGUNDA FILA--%>
                                </div>
                            </div>    
                            
                            <%--CUARTA FILA--%>
                            <div class="box-footer">
                                <div class="row">
                                    <div class="col-md-6">
                                        <div class="form-group">
                                            <div class="col-sm-8">
                                                <asp:Button ID="btnRecalcular" runat="server" Text="Recalcular" class="btn btn-warning" OnClick="btnRecalcular_Click" Visible="false" />
                                                <asp:Button ID="btnCierreViajeModal" runat="server" Text="Procesar Cierre" class="btn btn-success" OnClick="btnCierreViajeModal_Click" />
                                                <%--<asp:Button ID="btnCierreViajeModal" onclick="Aceptar(); return false;" runat="server" Text="Procesar Cierre" class="btn btn-success" />--%>
                                                <asp:Button ID="btnCancelarModal" runat="server" Text="Cancelar" class="btn btn-danger" UseSubmitBehavior="false" OnClick="btnCancelarModal_Click" />
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </asp:Panel>
                </div>

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

                <%--MODAL PARA CERRAR EL VIAJE--%>
                <div class="modal fade" id="QuestionModalCierre" data-backdrop="static" data-keyboard="true">
                    <div class="modal-dialog">
                        <div class="modal-content">
                            <div class="modal-header">
                                <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                                <h4 class="modal-title">
                                    <asp:Label ID="Label11" runat="server" Text="Información."></asp:Label>
                                </h4>
                            </div>
                            <div class="modal-body">
                                <div class="form-group">
                                    <div class="row">
                                        <div class="col-md-10">
                                            <asp:Label ID="lblAlertaMensajeCierre" runat="server" Text="¿Está seguro que desea cerrar el viaje?"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            <div class="modal-footer">
                                <asp:Button ID="btnNoCerrar" runat="server" Text="No, cancelar" class="btn btn-default" data-dismiss="modal"/>                                
                                <asp:Button ID="btnAceptarCerrar" runat="server" Text="Sí, confirmar" class="btn btn-info" data-dismiss="modal" UseSubmitBehavior="false" OnClick="btnAceptarCerrar_Click"/>
                            </div>
                        </div>
                    </div>
                </div>

                <%--FIN MODAL PARA CERRAR EL VIAJE--%>

            </section>
        </ContentTemplate>
    </asp:UpdatePanel>

    <asp:Button ID="btnInicial" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="btnPopUp_ModalPopupExtender" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial"
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server">
        <asp:UpdatePanel ID="UpdatePanel2" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModal" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel5">Registro de Pasajeros</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <%--<asp:Label ID="Label3" runat="server" Text="Tipo de Identificacion"></asp:Label>--%>
                                    <asp:TextBox ID="txtFiltrarCliente" runat="server" class="form-control" placeholder="BÚSQUEDA DE PERSONAS" Style="text-transform: uppercase"></asp:TextBox>
                                </div>

                                <div class="col-md-4">
                                    <asp:Button ID="btnFiltarCliente" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarCliente_Click" />
                                    <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>
                        <div class="form-group"></div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvFiltrarClientes" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvFiltrarClientes_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvFiltrarClientes_PageIndexChanging" OnRowDataBound="dgvFiltrarClientes_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="IIDCLIENTEFILTRO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDENTIFICACIONFILTRO" HeaderText="IDENTIFICACIÓN" />
                                            <asp:BoundField DataField="ICLIENTEFILTRO" HeaderText="NOMBRES" />
                                            <asp:BoundField DataField="IFECHAFILTRO" HeaderText="FECHA DE NACIMIENTO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-danger" OnClick="lbtnSeleccion_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="EDITAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEdit" runat="server" CommandName="cmdEditar" class="btn btn-xs btn-warning"><i class="fa fa-pencil"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--MODAL PARA MOVER LOS VIAJES--%>

    <asp:Button ID="btnSeleccionViaje" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopUpSeleccionViajes" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnSeleccionViaje"
        PopupControlID="pnlGridFiltroViajes" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltroViajes" runat="server">
        <asp:UpdatePanel ID="UpdatePanel13" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalViajesActivos" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalViajesActivos_Click" />
                        <h4 class="modal-title" id="myModalLabel8">Selección de Viajes Activos</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-4">
                                    <asp:TextBox ID="txtFechaViajesActivos" runat="server" class="form-control pull-right" autocomplete="off" placeholder="Fecha"></asp:TextBox>
                                    <ajaxToolkit:MaskedEditExtender ID="txtFechaViajesActivos_MaskedEditExtender" runat="server" BehaviorID="txtFechaViajesActivos_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaViajesActivos" />
                                    <ajaxToolkit:CalendarExtender ID="txtFechaViajesActivos_CalendarExtender" runat="server" BehaviorID="txtFechaViajesActivos_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaViajesActivos" />
                                </div>

                                <div class="col-md-4">
                                    <asp:Button ID="btnBuscarViajesActivos" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnBuscarViajesActivos_Click" />
                                    <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>
                        <div class="form-group"></div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvViajesActivos" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        OnSelectedIndexChanged="dgvViajesActivos_SelectedIndexChanged" AllowPaging="True" 
                                        OnPageIndexChanging="dgvViajesActivos_PageIndexChanging" PageSize="10"
                                        OnRowDataBound="dgvViajesActivos_RowDataBound">
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
                                            <asp:BoundField DataField="IIDPUEBLOORIGEN" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDPUEBLODESTINO" HeaderText="ID" />
                                            <asp:BoundField DataField="ICOBROADMINISTRATIVO" HeaderText="COBRO ADMINISTATIVO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditarExtra" runat="server" CommandName="Select" class="btn btn-xs btn-success"><i class="fa fa-bus"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DEL MODAL PARA MOVER LOS VIAJES--%>

    <%--MODAL PARA REGISTRAR O EDITAR LOS DATOS DEL CLIENTE--%>

    <asp:Button ID="btnInicialCrear" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtenderCrearEditar" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicialCrear"
        PopupControlID="pnlCrearEditar" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlCrearEditar" runat="server">
        <asp:UpdatePanel ID="UpdatePanel3" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModalCrearEditar" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalCrearEditar_Click" />
                        <h4 class="modal-title" id="myModalLabel6">Registro de Pasajeros</h4>
                    </div>
                    <asp:Panel ID="Panel100" runat="server" DefaultButton="btnGuardarPasajero">
                        <div class="modal-body">                        
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label17" runat="server" Text="Tipo de Identificacion"></asp:Label>
                                        <asp:DropDownList ID="cmbIdentificacion" runat="server" class="form-control"></asp:DropDownList>
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label18" runat="server" Text="Número de Identificación"></asp:Label>
                                        <asp:TextBox ID="txtIdentificacionRegistro" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase" OnTextChanged="txtIdentificacionRegistro_TextChanged" AutoPostBack="true" onkeypress="return validar_numeros(event)"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        
                            <div class="form-group"></div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <asp:Label ID="Label20" runat="server" Text="Razón Social / Apellidos"></asp:Label>
                                        <asp:TextBox ID="txtRazonSocial" runat="server" class="form-control" placeholder="RAZÓN SOCIAL / APELLIDOS" Style="text-transform: uppercase" autocomplete="off"></asp:TextBox>
                                    </div>
                                </div>
                            </div>
                        
                            <div class="form-group"></div>
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label ID="Label23" runat="server" Text="Ingrese el nombre"></asp:Label>
                                        <asp:TextBox ID="txtNombreRegistro" runat="server" class="form-control" placeholder="NOMBRES" Style="text-transform: uppercase" autocomplete="off"></asp:TextBox>
                                    </div>

                                    <div class="col-md-6">
                                        <asp:Label ID="Label24" runat="server" Text="Ingrese la fecha de nacimiento"></asp:Label>
                                        <asp:TextBox ID="txtFechaNacimiento" runat="server" class="form-control" BackColor="White"></asp:TextBox>
                                        <ajaxToolkit:MaskedEditExtender ID="txtFechaNacimiento_MaskedEditExtender" runat="server" BehaviorID="txtFechaNacimiento_MaskedEditExtender" Century="2000" CultureAMPMPlaceholder="" CultureCurrencySymbolPlaceholder="" CultureDateFormat="" CultureDatePlaceholder="" CultureDecimalPlaceholder="" CultureThousandsPlaceholder="" CultureTimePlaceholder="" Mask="99/99/9999" MaskType="Date" TargetControlID="txtFechaNacimiento" />
                                        <%--<ajaxToolkit:CalendarExtender ID="txtFechaNacimiento_CalendarExtender" runat="server" BehaviorID="txtFechaNacimiento_CalendarExtender" Format="dd/MM/yyyy" TargetControlID="txtFechaNacimiento" />--%>
                                    </div>
                                </div>
                            </div>
                        
                            <%--<div class="form-group">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:CheckBox ID="chkDiscapacidad" class="form-control" runat="server" Text="&nbsp&nbspDiscapacidad" />
                                    </div>
                                </div>
                            </div>--%>

                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-6">
                                        <asp:Label ID="lblAlerta" runat="server" Text="" ForeColor="Red"></asp:Label>
                                    </div>
                                </div>
                            </div>                        
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="btnGuardarPasajero" runat="server" Text="Guardar" class="btn btn btn-info" OnClick="btnGuardarPasajero_Click" />
                            <asp:Button ID="btnLimpiarPasajero" runat="server" Text="Limpiar" data-backdrop="false" data-dismiss="modal" class="btn btn btn-warning" />
                            <asp:Button ID="btnCerrarPasajero" runat="server" Text="Salir" class="btn btn-danger" OnClick="btnCerrarPasajero_Click" />
                        </div>
                    </asp:Panel>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--CIERRE DE MODAL--%>

    <asp:Button ID="btnInicial2" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Factura" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial2"
        PopupControlID="pnlGridFactura" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFactura" runat="server">
        <asp:UpdatePanel ID="UpdatePanel4" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="Button2" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel1">Búsqueda de datos para facturar</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <%--<asp:Label ID="Label3" runat="server" Text="Tipo de Identificacion"></asp:Label>--%>
                                    <asp:TextBox ID="txtBuscarClienteFactura" runat="server" class="form-control" placeholder="BÚSQUEDA DE PERSONAS" Style="text-transform: uppercase"></asp:TextBox>
                                </div>

                                <div class="col-md-4">
                                    <asp:Button ID="btnFiltrarFactura" runat="server" Text="Buscar" class="btn btn btn-info" UseSubmitBehavior="false" OnClick="btnFiltrarFactura_Click" />
                                    <%--<asp:TextBox ID="TextBox1" runat="server" class="form-control" placeholder="CÉDULA" Style="text-transform: uppercase"></asp:TextBox>--%>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:Label ID="Label2" runat="server" Text="Razón Social:"></asp:Label>
                                    <asp:Label ID="lblRazonSocial" runat="server" Text=""></asp:Label>
                                </div>
                            </div>   
                        </div>
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-8">
                                    <asp:Label ID="lblMensajeFactura" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </div>
                            </div>   
                        </div>

                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-15">
                                    <asp:GridView ID="dgvGridFacturar" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" OnSelectedIndexChanged="dgvGridFacturar_SelectedIndexChanged" AllowPaging="true" PageSize="5" OnPageIndexChanging="dgvGridFacturar_PageIndexChanging" OnRowDataBound="dgvGridFacturar_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="IIDCLIENTEFILTRO" HeaderText="ID" />
                                            <asp:BoundField DataField="IIDENTIFICACIONFILTRO" HeaderText="IDENTIFICACIÓN" />
                                            <asp:BoundField DataField="ICLIENTEFILTRO" HeaderText="NOMBRES" />
                                            <asp:BoundField DataField="IFECHAFILTRO" HeaderText="FECHA DE NACIMIENTO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="SELECCIONAR">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccionFactura" CommandName="Select" runat="server" class="btn btn-xs btn-danger" OnClick="lbtnSeleccionFactura_Click"><i class="fa fa-search"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>
                            </div>
                        </div>

                        <div class="modal-footer">
                            <asp:Button ID="btnProcesarFactura" runat="server" Text="Procesar Factura" class="btn btn-success" OnClick="btnProcesarFactura_Click" />
                            <asp:Button ID="btnCancelarModalFactura" runat="server" Text="Cancelar" class="btn btn-danger" UseSubmitBehavior="false" OnClick="btnCancelarModalFactura_Click" />
                        </div>

                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--MODAL VER PASAJEROS--%>
    <asp:Button ID="btnInicial5" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_ListaPasajeros" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial5"
        PopupControlID="pnlListaPasajeros" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlListaPasajeros" runat="server">
        <asp:UpdatePanel ID="UpdatePanel6" runat="server">
            <ContentTemplate>
                <div class="modal-content modal-lg">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="Button4" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModal_Click" />
                        <h4 class="modal-title" id="myModalLabel2">Lista de Pasajeros Vendidos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:GridView ID="dgvListaPasajeros" runat="server" class="mGrid"
                            AutoGenerateColumns="False"
                            EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" OnPageIndexChanging="dgvListaPasajeros_PageIndexChanging" PageSize="10" OnRowDataBound="dgvListaPasajeros_RowDataBound">
                            <Columns>
                                <asp:BoundField DataField="numero_asiento" HeaderText="N° Asiento" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="identificacion" HeaderText="Documento" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="pasajero" HeaderText="Pasajero" />
                                <asp:BoundField DataField="tipo_cliente" HeaderText="Tipo" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="descripcion" HeaderText="Destino" ItemStyle-HorizontalAlign="Center">
                                <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="valor" HeaderText="Valor" ItemStyle-HorizontalAlign="Right">
                                <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                            </Columns>
                            <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                        </asp:GridView>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnCloseModalListaPasajeros" runat="server" Text="Cancelar" class="btn btn-default pull-right" OnClick="btnCerrarModal_Click"/>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--INICIO DEL MODAL PARA REPORTE--%>

    <asp:Button ID="btnInicial1" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_Reporte" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicial1"
        PopupControlID="pnlReporte" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlReporte" runat="server">
        <asp:UpdatePanel ID="UpdatePanel7" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalReport" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" />
                        <h4 class="modal-title" id="myModalLabel15">Factura</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-10">
                                    <rsweb:ReportViewer ID="rptFactura" runat="server" Font-Names="Verdana" Font-Size="8pt" WaitMessageFont-Names="Verdana" WaitMessageFont-Size="14pt" Height="400px" Width="800px">
                                    <LocalReport ReportEmbeddedResource="Solution_CTT.Reportes.rptFactura.rdlc">
                                    </LocalReport>
                                    </rsweb:ReportViewer>
                                    <br />
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="modal-footer">
                        <asp:Button ID="btnImprimirReporte" runat="server" Text="Imprimir" class="btn btn btn-success" />
                        <asp:Button ID="btnCerrarModalReporte" runat="server" Text="Salir" class="btn btn btn-warning" />                        
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DEL MODAL REPORTE--%>

    <%--MODAL DE FACTURAS EMITIDAS--%>

    <asp:Button ID="btnReimprimirFAC" runat="server" Text="Button" style="display:none"/>

    <%--<ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender1" runat="server"></ajaxToolkit:ModalPopupExtender>--%>
    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_ReimprimirFacturas" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnReimprimirFAC" 
        PopupControlID="pnlReimprimirFacturas" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlReimprimirFacturas" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel8" runat="server">
        <ContentTemplate>
            
                <div class="modal-content">
                    <div class="modal-header bg-teal-active color-palette">
                        <asp:Button ID="btnCerrarModalFacturasEmitidas" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalFacturasEmitidas_Click" />
                        <h4 class="modal-title" id="myModalLabel7">Facturas emitidas en el viaje</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">

                            </div>   
                        </div>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvVendidos" runat="server" class="mGrid"
                                        AutoGenerateColumns="False"
                                        EmptyDataText="No hay Registros o Coindicencias..!!"
                                        OnSelectedIndexChanged="dgvVendidos_SelectedIndexChanged" AllowPaging="True" OnPageIndexChanging="dgvVendidos_PageIndexChanging" PageSize="7" OnRowDataBound="dgvVendidos_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="IIDPEDIDO" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IIDPROGRAMACION" HeaderText="ID PROGRAMACION" />
                                            <asp:BoundField DataField="IIDFACTURA" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IIDPERSONA" HeaderText="ID PERSONA" />
                                            <asp:BoundField DataField="IIDENTIFICACION" HeaderText="IDENTIFICACION" />
                                            <asp:BoundField DataField="ICLIENTE" HeaderText="CLIENTE" />
                                            <asp:BoundField DataField="IFACTURA" HeaderText="No. FACTURA" />
                                            <asp:BoundField DataField="IFECHAVIAJE" HeaderText="FECHA DE VIAJE" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="VER">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnEditarFactura" runat="server" CommandName="Select" class="btn btn-xs btn-warning" ToolTip="Clic aquí para reimprimir la factura."><i class="fa fa-print"></i></asp:LinkButton>
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle HorizontalAlign="Center" CssClass="pagination-ys" />
                                    </asp:GridView>
                                </div>                                 
                            </div>                                           
                        </div>                  
                                     
                        </div>
                    </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DE MODAL DE FACTURAS EMITIDAS--%>

    <%--MODAL PARA CAMBIAR LA HORA--%>

    <asp:Button ID="btnInicialHora" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_NuevaHora" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnInicialHora"
        PopupControlID="pnlNuevaHora" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlNuevaHora" runat="server">
        <asp:UpdatePanel ID="UpdatePanel5" runat="server">
            <ContentTemplate>
                <!-- Modal GRIDS-->
                <%--<div id="modalGrid" class="modal">
            <div class="modal-dialog modal-lg" role="document">--%>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:Button ID="btnCerrarModalNuevaHora" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalNuevaHora_Click" />
                        <h4 class="modal-title" id="myModalLabel17">Asignar adelanto de Viaje</h4>
                    </div>
                    <div class="modal-body">
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-6">
                                    <asp:Label ID="Label14" runat="server" Text="Hora Actual"></asp:Label>
                                    <asp:TextBox ID="txtHoraActual" runat="server" class="form-control" placeholder="HORA ACTUAL" ReadOnly="true" BackColor="White"></asp:TextBox>
                                </div>

                                <div class="col-md-6">
                                    <asp:Label ID="Label15" runat="server" Text="Nueva Hora"></asp:Label>
                                    <asp:TextBox ID="txtNuevaHoraSalida" runat="server" type="time" class="form-control timepicker"></asp:TextBox>
                                </div>
                            </div>
                        </div>                       

                        <div class="modal-footer">
                            <asp:Button ID="btnAsignarNuevaHora" runat="server" Text="Guardar" class="btn btn btn-info" OnClick="btnAsignarNuevaHora_Click" />
                            <asp:Button ID="btnRemoverNuevaHora" runat="server" Text="Remover" class="btn btn btn-warning" OnClick="btnRemoverNuevaHora_Click" />
                            <asp:Button ID="btnCancelarNuevaHora" runat="server" Text="Cancelar" class="btn btn-danger" OnClick="btnCancelarNuevaHora_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--CIERRE DE MODAL--%>


    <%--MODAL PARA INGRESAR UN NUEVO TOKEN--%>

    <asp:Button ID="btnModalValidaToken" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_ValidaToken" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnModalValidaToken"
        PopupControlID="pnlValidaToken" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlValidaToken" runat="server">
        <asp:UpdatePanel ID="UpdatePanel9" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-teal-active color-palette">
                            <asp:Button ID="btnAbrirModalInfoToken" runat="server" Text="?" class="close" aria-label="Close" OnClick="btnAbrirModalInfoToken_Click" />
                            <asp:Button ID="btnCerrarModalValidarToken" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalValidarToken_Click" /></span></button>
                            <h4 class="modal-title">
                                <asp:Label ID="Label26" runat="server" Text="Ingreso de un nuevo TOKEN"></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-success alert-dismissible text-center">
                                            <label for="inputEmail3" class="col-sm-12 control-label">Ingrese el código TOKEN de 5 dígitos (00000)</label><br></br>
                                            <label for="inputEmail3" class="col-sm-12 control-label">emitido por los operadores de la EPMMOP</label><br></br>
                                        </div>                                        
                                    </div>
                                </div>

                                <div class="row">
                                    <asp:Panel ID="pnlEnterValidar" runat="server" DefaultButton="btnValidarTokenModal">
                                        <div class="col-md-12">
                                            <div class="col-md-4 text-center">
                                                <asp:TextBox ID="txtNumeroTokenModal" runat="server" MaxLength="5" class="form-control" style="text-align:center;" onkeypress="return ValidarSoloNumeros(event)"></asp:TextBox>
                                            </div>
                                            <div class="col-md-4 text-center">
                                                <asp:Button ID="btnValidarTokenModal" runat="server" Text="Validar Nuevo Token" class="btn btn btn-danger" OnClick="btnValidarTokenModal_Click" />                                            
                                            </div>
                                            <div class="col-md-4 text-center">
                                                <asp:Button ID="btnContinuarToken" runat="server" Text="CONTINUAR" class="btn btn btn-warning" OnClick="btnContinuarToken_Click" />
                                            </div>
                                        </div>
                                    </asp:Panel>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-dismissible text-center">
                                            <h2><asp:Label ID="lblTituloValidacionModal" Font-Bold="true" runat="server" Text="RESPUESTA"></asp:Label></h2>
                                            <asp:Label ID="lblMensajeValidacionModal" runat="server" Text="Mensaje a Recibir"></asp:Label>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--CIERRE DE MODAL PARA INGRESAR UN NUEVO TOKEN--%>

    <%--MODAL PARA INFORMACION DEL TOKEN--%>
    
    <asp:Button ID="btnModalInfoToken" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_InfoToken" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnModalInfoToken"
        PopupControlID="pnlInfoToken" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlInfoToken" runat="server">
        <asp:UpdatePanel ID="UpdatePanel10" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-teal-active color-palette">
                            <asp:Button ID="btnCerrarModalInfoToken" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalInfoToken_Click" />
                            <h4 class="modal-title">
                                <asp:Label ID="Label27" runat="server" Text="Procedimiento de Activación de Token"></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-success alert-dismissible text-center">
                                            <label for="inputEmail3" class="col-sm-12 control-label"><b>1. </b>Anote bien su código de oficina que es este</label>
                                            <asp:TextBox ID="txtCodigoOficina" runat="server" MaxLength="5" Font-Size="XX-Large" style="text-align:center;" ReadOnly="true" BackColor="White" ForeColor="Red" Font-Bold="true"></asp:TextBox>
                                            <br>
                                            </br>
                                        </div>                                        
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-warning alert-dismissible text-center">
                                            <label for="inputEmail3" class="col-sm-12 control-label"><b>2. </b>Acérquese a la taquilla de EPMMOP y solicite un nuevo<b>TOKEN</b></label><br></br>
                                        </div>                                        
                                    </div>
                                </div>

                                <div class="row">
                                    <div class="col-md-12">
                                        <div class="alert alert-info alert-dismissible text-center">
                                            <label for="inputEmail3" class="col-sm-12 control-label"><b>3. </b>Ingrese el código del TOKEN de 5 dígitos entregado.</label><br></br>
                                        </div>                                        
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--CIERRE DE MODAL--%>

    <%--MODAL DE TOKENS GENERADOS--%>

    <asp:Button ID="btnReporteTokenInfo" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_ReporteTokenInfo" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnReporteTokenInfo"
        PopupControlID="pnlReporteTokenInfo" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlReporteTokenInfo" runat="server">
        <asp:UpdatePanel ID="UpdatePanel11" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-teal-active color-palette">
                            <asp:Button ID="btnCerrarModalReporteTokenInfo" runat="server" Text="x" class="close" data-dismiss="modal" aria-label="Close" OnClick="btnCerrarModalReporteTokenInfo_Click" />
                            <h4 class="modal-title">
                                <asp:Label ID="Label28" runat="server" Text="Reporte de Token"></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                         <asp:GridView ID="dgvReporteTokenInfo" runat="server" class="mGrid" AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" AllowPaging="True" PageSize="10" OnPageIndexChanging="dgvReporteTokenInfo_PageIndexChanging" OnRowDataBound="dgvReporteTokenInfo_RowDataBound">
                                        <Columns>
                                            <asp:BoundField DataField="token" HeaderText="TOKEN" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="fecha_compra" HeaderText="FECHA DE COMPRA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="hora" HeaderText="HORA" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="maximo_secuencial" HeaderText="CANTIDAD" ItemStyle-HorizontalAlign="Center" />
                                            <asp:BoundField DataField="utilizados" HeaderText="UTILIZADOS" ItemStyle-HorizontalAlign="Center" />
                                            <%--<asp:BoundField DataField="disponibles" HeaderText="DISPONIBLES" ItemStyle-HorizontalAlign="Center" />--%>
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="DISPONIBLES">
                                                <ItemTemplate>
                                                    <asp:Label ID="lblDisponibleTasasModal" runat="server" class="form-control" Text='<%# Bind("disponibles") %>'></asp:Label>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <PagerStyle CssClass="pagination-ys" />
                                    </asp:GridView>                                     
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--FIN DE MODAL DE TOKENS GENERADOS--%>

    <%--MODAL PARA NOTIFICACIONES AUTOMATICAS--%>

    <asp:Button ID="btnNotificacionAutomatica" runat="server" Text="Button" Style="display: none" />

    <ajaxToolkit:ModalPopupExtender ID="ModalPopupExtender_NotificacionAutomatica" runat="server"
        DynamicServicePath="" Enabled="True" TargetControlID="btnNotificacionAutomatica"
        PopupControlID="pnlNotificacionAutomatica" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlNotificacionAutomatica" runat="server">
        <asp:UpdatePanel ID="UpdatePanel12" runat="server">
            <ContentTemplate>
                <div class="modal-dialog">
                    <div class="modal-content">
                        <div class="modal-header bg-teal-active color-palette">
                            <%--<button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>--%>
                            <h4 class="modal-title">
                                <asp:Label ID="Label25" runat="server" Text="Notificación Automática"></asp:Label>
                            </h4>
                        </div>
                        <div class="modal-body">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-12">
                                        <div id="divTitulo" class="alert alert-dismissible text-center" runat="server">
                                            <%--<asp:Panel ID="pnlMensajeNotificacion" runat="server">--%>
                                            <h4 class="modal-title"><asp:Label ID="lblMensajeNotificacion" runat="server" Text=""></asp:Label></h4>
                                            <%--</asp:Panel>--%>
                                        </div>
                                        <div class="text-center">
                                            <h3 class="modal-title"><asp:Label ID="lblDatosMensajeNotificacion" runat="server" Text=""></asp:Label></h3>
                                        </div>
                                        <div class="text-center">
                                            <h1 class="modal-title"><label><asp:Label ID="lblCantidadMensajeNotificacion" runat="server" Text="" ForeColor="Blue"></asp:Label></label></h1>
                                            <h3 class="modal-title"><label>Tasas de Usuario</label></h3>
                                        </div>
                                        <div class="text-center">
                                            Se recomienda que prepare con anticipación la compra de un nuevo TOKEN
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="modal-footer">
                            <div class="text-center">
                                <asp:CheckBox ID="chkConfirmacionVisualizacion" runat="server" Text="&nbsp&nbspConfirmo que he visto esta notificación" />
                            </div>
                            <asp:Button ID="btnOkNotificacionAutomatica" runat="server" Text="OK" class="btn btn btn-success" OnClick="btnOkNotificacionAutomatica_Click" />
                        </div>
                    </div>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>

    <%--CIERRE DE MODAL PARA NOTIFICACIONES AUTOMATICAS--%>

    <%--CIERRE DE MODAL PARA NOTIFICACIONES AUTOMATICAS 2--%>
    <div class="modal fade" id="modalNotify" data-keyboard="true">
        <div class="modal-dialog">
            <div class="modal-content">
                <div class="modal-header bg-teal-active color-palette">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title">
                        <asp:Label ID="Label16" runat="server" Text="Notificación Automática"></asp:Label>
                    </h4>
                </div>
                <div class="modal-body">
                    <div class="form-group">
                        <div class="row">
                            <div class="col-md-12">
                                <div class="alert alert-dismissible text-center">
                                    <h4 class="modal-title"><asp:Label ID="Label19" runat="server" Text="Has consumido el 40% de la cantidad de tasas de usuario"></asp:Label></h4>
                                </div>
                                <div class="text-center">
                                    <asp:Label ID="Label21" runat="server" Text="Hoy 24-07-2019 a las 10:08"></asp:Label>
                                </div>
                                <div class="text-center">
                                    Se le notifica que solo dispone de:
                                </div>
                                <div class="text-center">
                                    <h1 class="modal-title"><label><asp:Label ID="Label22" runat="server" Text="68" ForeColor="Blue"></asp:Label></label></h1>
                                    <h3 class="modal-title"><label>Tasas de Usuario</label></h3>
                                </div>
                                <div class="text-center">
                                    Se recomienda que prepare con anticipación la compra de un nuevo TOKEN
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="modal-footer">
                    <div class="text-center">
                        <asp:CheckBox ID="CheckBox1" runat="server" Text="&nbsp&nbsp Confirmo que he visto esta notificación" />
                    </div>
                    <button type="button" class="btn btn-default pull-left" data-dismiss="modal">Cerrar</button>                                
                </div>
            </div>
        </div>
    </div>
    <%--FIN MODAL--%>


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
