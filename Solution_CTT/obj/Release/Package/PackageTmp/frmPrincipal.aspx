<%@ Page Title="" Language="C#" MasterPageFile="~/Master.Master" AutoEventWireup="true" CodeBehind="frmPrincipal.aspx.cs" Inherits="Solution_CTT.frmPrincipal" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="server">
    <%--<script src="assets/js/loader.js"></script>  --%> 
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
    <script type="text/javascript">
        google.charts.load('current', { 'packages': ['corechart'] });
        google.charts.setOnLoadCallback(drawChart);

        function drawChart() {

            var data = new google.visualization.arrayToDataTable(<%=ObtenerDatos()%>);

            var options = {
                'title': 'Ventas Atenas Express S.A'
            };

            var chart = new google.visualization.PieChart(document.getElementById('chart_div'));
            chart.draw(data, options);
        }
    </script>
    
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="server">
    <section class="content">
        <div class="row">
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-aqua">
                    <div class="inner">
                        <h3>150</h3>
                        <p>Transacciones</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-refresh"></i>
                    </div>
                    <a href="frmTransacciones.aspx" class="small-box-footer">Acceder <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-green">
                    <div class="inner">
                        <h3>53<sup style="font-size: 20px">%</sup></h3>
                        <p>Rutas</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-map"></i>
                    </div>
                    <a href="frmRutas.aspx" class="small-box-footer">Acceder <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-yellow">
                    <div class="inner">
                        <h3>44</h3>
                        <p>Asistentes</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-person-add"></i>
                    </div>
                    <a href="frmAsistentes.aspx" class="small-box-footer">Acceder <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
            <div class="col-lg-3 col-xs-6">
                <div class="small-box bg-red">
                    <div class="inner">
                        <h3>65</h3>
                        <p>Reportes</p>
                    </div>
                    <div class="icon">
                        <i class="ion ion-pie-graph"></i>
                    </div>
                    <a href="#" class="small-box-footer">Acceder <i class="fa fa-arrow-circle-right"></i></a>
                </div>
            </div>
        </div>
        <%--GRAFIC SALLES--%>
        <div class="row">
            <div class="col-md-12">
                <div class="box">
                    <div class="box-header with-border">
                        <h3 class="box-title">Reporte Mensual de Viajes</h3>
                        <div class="box-tools pull-right">
                            <button type="button" class="btn btn-box-tool" data-widget="collapse">
                                <i class="fa fa-minus"></i>
                            </button>
                            <button type="button" class="btn btn-box-tool" data-widget="remove"><i class="fa fa-times"></i></button>
                        </div>
                    </div>
                    <div class="box-body">
                        <div class="row">
                            <div class="col-md-12">
                                <p class="text-center">
                                    <strong><asp:Label ID="lblReportDate" runat="server" Text=""></asp:Label></strong>
                                </p>
                                <div id="chart_div"></div>
                            </div>
                        </div>
                    </div>
                    <div class="box-footer">
                        <div class="row">
                            <div class="col-sm-3 col-xs-6">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-green"><i class="fa fa-caret-up"></i>17%</span>
                                    <h5 class="description-header">$35,210.43</h5>
                                    <span class="description-text">TOTAL REVENUE</span>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-6">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-yellow"><i class="fa fa-caret-left"></i>0%</span>
                                    <h5 class="description-header">$10,390.90</h5>
                                    <span class="description-text">TOTAL COST</span>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-6">
                                <div class="description-block border-right">
                                    <span class="description-percentage text-green"><i class="fa fa-caret-up"></i>20%</span>
                                    <h5 class="description-header">$24,813.53</h5>
                                    <span class="description-text">TOTAL PROFIT</span>
                                </div>
                            </div>
                            <div class="col-sm-3 col-xs-6">
                                <div class="description-block">
                                    <span class="description-percentage text-red"><i class="fa fa-caret-down"></i>18%</span>
                                    <h5 class="description-header">1200</h5>
                                    <span class="description-text">GOAL COMPLETIONS</span>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>                
            </div>
        </div>
    </section>
</asp:Content>
