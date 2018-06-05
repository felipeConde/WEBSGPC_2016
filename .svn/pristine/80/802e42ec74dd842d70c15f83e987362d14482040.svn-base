<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uc_GraficoUsuarioServicos.aspx.vb" Inherits="uc_GraficoUsuarioServicos" %>


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server"><!DOCTYPE html>

    <title></title>

        <%--<script src="vendors/bower_components/jquery/dist/jquery.min.js"></script>
        <!-- Highcharts -->
        <script src="Highcharts-4.1.9/js/highcharts.js"></script>
        <script src="Highcharts-4.1.9/js/highcharts-3d.js"></script>
        <script src="Highcharts-4.1.9/js/modules/exporting.js"></script>--%>


    <!-- Vendor CSS -->
  <%--      <link href="vendors/bower_components/animate.css/animate.min.css" rel="stylesheet">
        <link href="vendors/bower_components/bootstrap-sweetalert/lib/sweet-alert.css" rel="stylesheet">
        <link href="vendors/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.min.css" rel="stylesheet">
        <link href="vendors/bower_components/malihu-custom-scrollbar-plugin/jquery.mCustomScrollbar.min.css" rel="stylesheet">        
        <link href="vendors/bower_components/bootstrap-select/dist/css/bootstrap-select.css" rel="stylesheet">
        <link href="vendors/bower_components/nouislider/distribute/jquery.nouislider.min.css" rel="stylesheet">
        <link href="vendors/bower_components/eonasdan-bootstrap-datetimepicker/build/css/bootstrap-datetimepicker.min.css" rel="stylesheet">
        <link href="vendors/farbtastic/farbtastic.css" rel="stylesheet">
        <link href="vendors/bower_components/chosen/chosen.min.css" rel="stylesheet">
        <link href="vendors/summernote/dist/summernote.css" rel="stylesheet">
            
        <!-- CSS -->
        <link href="css/app.min.1.css" rel="stylesheet">
        <link href="css/app.min.2.css" rel="stylesheet">--%>
    <script src="js/functions.js"></script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
       <div id="containerServs" style="width:98%" ></div>
    </div>
    </form>


    <script>
        var tam = $(window).width() * 0.50;

          $(function () {
              //alert('teste');
              $('#containerServs').width($('#myApp').width()-50);
              $('#containerServs').highcharts({

                    chart: {
                        type: 'column',
                        options3d: {
                            enabled: true,
                            alpha: 0,
                            beta: 0,
                            viewDistance: 0,
                            depth: 0
                        },                      
                        marginTop: 80,
                        marginRight: 40
                    },
                    colors: ['#5F9EA0', '#FFA500', '#FFB6C1', '#00BFFF', '#4682B4', '#BDB76B', '#DCDCDC', '#FF6347', '#008B45', '#FFB90F', '#9F79EE'],
                    title: {
                        text: 'Evolução dos Custos por Serviço'
                    },

                    xAxis: {
                        categories: [<%=GraficoLabel %>]
                              , labels: {
                                  formatter: function () {
                                      //return '<a href="javascript(0)">' + this.value + '</a>';
                                      return  getMes(this.value.substring(0, 2)) + this.value.substring(2, 7);
                                  }
                              }
                    },

                    yAxis: {
                        allowDecimals: false,
                        min: <%= negativeValue %>,
                        //type: 'logarithmic',
                        title: {
                            text: 'Gasto'
                        }
                        ,
                        stackLabels: {
                            enabled: true,
                            style: {
                                fontWeight: 'bold',
                                color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                            },
                            formatter: function () {
                                return 'R$ ' + Highcharts.numberFormat(this.total, 2, ',', '.');
                            }
                        }
                    },

                    tooltip: {
                        headerFormat: '<b>{point.key}</b><br>',
                        pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: R$ {point.y:.2f}',
                        formatter: function () {
                            return '<b>' + this.series.name + '</b><br/>' +
                                this.x + ': ' + 'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
                        }
                    },

                    plotOptions: {
                        column: {
                            stacking: 'normal',
                            depth: 40
                        },

                        series: {
                            cursor: 'pointer',
                            point: {
                                events: {
                                    click: function (e) {
                                        //GraficoCusto(this.category);
                                    }
                                }
                            },
                            marker: {
                                lineWidth: 1
                            }
                        }
                    },

                    series: [
        //            {
        //                name: 'John',
        //                data: [5, 3, 4, 7, 2]
        //            }, {
        //                name: 'Joe',
        //                data: [3, 4, 4, 2, 5]
        //            }, {
        //                name: 'Jane',
        //                data: [2, 5, 6, 2, 1]
        //            }, {
        //                name: 'Janet',
        //                data: [3, 0, 4, 4, 3]
        //            }

<%=GraficoData %>
                    ]
                });
            });

    </script>
</body>
</html>
