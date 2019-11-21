<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmEnvioComprobantes.aspx.cs" Inherits="Solution_CTT.frmEnvioComprobantes" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <section class="box-default">
                <div class="row">
                    <div class="col-md-12">
                        <div class="box box-primary">
                            <div class="box-header with-border">
                                <i class="fa fa-send-o"></i>
                                <h3 class="box-title">Envío de Comprobantes</h3>
                            </div>
                            <div class="box-body">
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Tipo de Comprobante</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoComprobante" runat="server" disabled="" class="form-control input-sm" AutoPostBack="True" OnSelectedIndexChanged="cmbTipoComprobante_SelectedIndexChanged" ></asp:DropDownList>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Tipo de Ambiente</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoAmbiente" runat="server" disabled="" class="form-control input-sm"></asp:DropDownList>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>                                    
                                    <div class="col-md-3 form-group">
                                        <label>Tipo de Emisión</label>
                                        <div class="input-group col-sm-12">
                                            <asp:DropDownList ID="cmbTipoEmision" runat="server" disabled="" class="form-control input-sm"></asp:DropDownList>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="box-header with-border">
                                    <h4 class="box-title">Datos del Documento</h4>
                                </div>
                                <p></p>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Buscar...</label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnBuscarXML" runat="server" class="form-control btn btn-flat btn-success" ToolTip="Clic aquí, para buscar archivos a firmar" OnClick="lbtnBuscarXML_Click" ><i class="fa fa-search"></i>  BUSCAR XML</asp:LinkButton>
                                        </div>
                                    </div>                                    
                                    <div class="col-md-6 form-group">
                                        <label>Clave de Acceso</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtClaveAcceso" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Numero de Documento</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtNumeroDocumento" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Ruta de archivos firmados</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtRutaFirmados" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Archivo a firmar</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtDocumentoEnviar" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>                                 
                                </div>
                                <div class="box-header with-border">
                                    <h4 class="box-title">Respuesta de Envío del SRI</h4>
                                </div>
                                <p></p>
                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label>Mensaje</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtMensaje" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label>Código</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtCodigoEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>     
                                    <div class="col-md-3 form-group">
                                        <label>Tipo</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtTipoEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div> 
                                    <div class="col-md-3 form-group">
                                        <label>Estado</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtEstadoEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" disabled="" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 form-group">
                                        <label>Mensaje de Error</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtMensajeErrorEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div> 
                                    <div class="col-md-6 form-group">
                                        <label>Información Adicional</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtInformacionAdicionalEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off"  ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div> 
                                    <div class="col-md-12 form-group">
                                        <label>Detalles</label>
                                        <div class="input-group col-sm-12">
                                            <asp:TextBox ID="txtDetallesEnvio" runat="server" class="form-control input-sm" placeholder="" AutoComplete="off" ToolTip="Este campo es generado automático"></asp:TextBox>
                                            <span class="input-group-addon input-sm"><i class="fa fa-dollar"></i></span>
                                        </div>
                                    </div>                             
                                </div>

                                <div class="row">
                                    <div class="col-md-3 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnEnviarXML" runat="server" class="form-control btn btn-flat btn-primary" ToolTip="Clic aquí, para enviar XML" OnClick="lbtnEnviarXML_Click" ><i class="fa fa-pencil-square-o"></i>  Enviar XML</asp:LinkButton>
                                        </div>
                                    </div>
                                    <div class="col-md-3 form-group">
                                        <label></label>
                                        <div class="input-group col-sm-12">
                                            <asp:LinkButton ID="lbtnCancelar" runat="server" class="form-control btn btn-flat btn-default" ToolTip="Clic aquí, para cancelar" OnClick="lbtnCancelar_Click" ><i class="fa fa-circle-o-notch"></i>  Cancelar</asp:LinkButton>
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

    <%--MODAL FACTURAS--%>
    <asp:Button ID="btnInicial" runat="server" Text="Button" style="display:none"/>
    <ajaxToolkit:ModalPopupExtender ID="ModalBuscarXML" runat="server"
        Enabled="True" TargetControlID="btnInicial" 
        PopupControlID="pnlGridFiltro" BackgroundCssClass="modalBackground">
    </ajaxToolkit:ModalPopupExtender>

    <asp:Panel ID="pnlGridFiltro" runat="server" >
    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
        <ContentTemplate>
                <div class="modal-content">
                    <div class="modal-header">
                        <asp:LinkButton ID="lbtnCerrarModalBuscarXML" runat="server" CssClass="close" OnClick="lbtnCerrarModalBuscarXML_Click"><span aria-hidden="true">&times;</span></asp:LinkButton>
                        <h4 class="modal-title" id="myModalLabel5">Documentos Electrónicos</h4>
                    </div>
                    <div class="modal-body">
                        <asp:Panel ID="Panel1" runat="server" DefaultButton="btnFiltarModalBuscarXML">
                            <div class="form-group">
                                <div class="row">
                                    <div class="col-md-8">
                                        <asp:TextBox ID="txtFiltrarModalBuscarXML" runat="server" class="form-control" placeholder="Número de Factura o Nombre de Cliente" Style="text-transform: uppercase"></asp:TextBox>
                                    </div>
                                    <div class="col-md-3">
                                        <asp:Button ID="btnFiltarModalBuscarXML" runat="server" Text="Buscar" class="btn btn-block btn-primary" UseSubmitBehavior="false" OnClick="btnFiltarModalBuscarXML_Click" />
                                    </div>
                                </div>   
                            </div>
                        </asp:Panel>
                        <div class="form-group"></div>
                                           
                        <div class="form-group">
                            <div class="row">
                                <div class="col-md-12">
                                    <asp:GridView ID="dgvFiltrarModalBuscarXML" runat="server" class="mGrid" 
                                        AutoGenerateColumns="False" EmptyDataText="No hay Registros o Coindicencias..!!" 
                                        AllowPaging="true" PageSize="10" OnSelectedIndexChanged="dgvFiltrarModalBuscarXML_SelectedIndexChanged" 
                                        OnPageIndexChanging="dgvFiltrarModalBuscarXML_PageIndexChanging" OnRowDataBound="dgvFiltrarModalBuscarXML_RowDataBound">
                                        <Columns>
                                            <%--<asp:BoundField DataField="Numero_factura" HeaderText="Numero_FACTURA"  />
                                            <asp:BoundField DataField="id_factura" HeaderText="N° FACTURA"  />                                            
                                            <asp:BoundField DataField="Cliente" HeaderText="NOMBRE DEL CLIENTE"  />
                                            <asp:BoundField DataField="Localidad" HeaderText="LOCALIDAD"  />
                                            <asp:BoundField DataField="fecha_factura" HeaderText="FECHA FACTURACIÓN"  />                                            
                                            <asp:BoundField DataField="clave_acceso" HeaderText="CLAVE ACCESO"  />
                                            <asp:BoundField DataField="estab" HeaderText="ESTABLECIMIENTO"  />
                                            <asp:BoundField DataField="ptoEmi" HeaderText="PTO EMISION"  />
                                            <asp:BoundField DataField="autorizacion" HeaderText="AUTORIZACIÓN"  />     
                                            <asp:BoundField DataField="fecha_autorizacion" HeaderText="FECHA AUTO."  />
                                            <asp:BoundField DataField="id_tipo_emision" HeaderText="TIPO EMISION"  />
                                            <asp:BoundField DataField="id_tipo_ambiente" HeaderText="TIPO AMBIENTE"  />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccion_Click"  ><i class="fa fa-check-square-o"></i></asp:LinkButton>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>--%>                                                                          
                                            <asp:BoundField DataField="INUMERO" HeaderText="N°" />
                                            <asp:BoundField DataField="IIDFACTURA" HeaderText="ID FACTURA" />
                                            <asp:BoundField DataField="IFECHAFACTURA" HeaderText="FECHA DE FACTURA" />
                                            <asp:BoundField DataField="ICLIENTE" HeaderText="CLIENTE" />
                                            <asp:BoundField DataField="IFACTURAEMITIDA" HeaderText="FACTURA" />
                                            <asp:BoundField DataField="ICLAVEACCESO" HeaderText="CLAVE DE ACCESO" />
                                            <asp:TemplateField ItemStyle-HorizontalAlign="Center" HeaderText="">
                                                <ItemTemplate>
                                                    <asp:LinkButton ID="lbtnSeleccion" runat="server" CommandName="Select" class="btn btn-xs btn-success" OnClick="lbtnSeleccion_Click"><i class="fa fa-check-square-o"></i></asp:LinkButton>
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
    <%--FIN MODAL--%>
    <%--PROGRESS--%>
    <asp:UpdateProgress ID="UpdateProgress1" runat="server" AssociatedUpdatePanelID="UpdatePanel1">
        <ProgressTemplate>
            <div class="Sending">
                <div id="ParentDiv" align="center" valign="middle" runat="server" style="position: absolute; left: 50%; top: 25%; visibility: visible; vertical-align: middle; z-index: 40;">
                    <img src="assets/img/loading4.gif" /><br />
                    <%--<img src="assets/images/icons/send_document_1.gif" width="100" height="100"/><br />
                    <br />
                    <p style="color: #dddddd">Enviando Documento...</p>
                    <br />
                    <input type="button" onclick="CancelPostBack()" value="Cancelar" class="btn btn-sm btn-default" />--%>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>
</asp:Content>
