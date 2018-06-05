<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uc_grafico_areas.aspx.vb" Inherits="uc_grafico_areas" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>

        <!-- Vendor CSS -->
        <link href="vendors/bower_components/animate.css/animate.min.css" rel="stylesheet">
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
        <link href="css/app.min.2.css" rel="stylesheet">

<%--        <script src="https://ajax.googleapis.com/ajax/libs/jquery/2.2.2/jquery.min.js"></script>--%>
      <%--  <script src="https://code.highcharts.com/highcharts.js"></script>
        <script src="https://code.highcharts.com/modules/exporting.js"></script>--%>

</head>
<body>
    <form id="form1" runat="server">

        <div class="card">

    
    <div class="card-body card-padding">

        <div class="row">
            <button style="display:none;" class="btn btn-default btn-icon waves-effect waves-circle waves-float" onclick="window.history.go(-1); return false;"><i class="zmdi zmdi-arrow-back"></i></button>
            <div id="containerGrafico"  runat="server" clientidmode="Static" style="width:90%"></div>

            
            <div id="myGraf"></div>
        </div>

        <center>
            <% If ViewState("nivel") <> "0" Then%>
                    <span class="btn btn-default btn-icon-text waves-effect" onclick="TotalArea()"><i class="zmdi zmdi-refresh"></i> Total da Área</span>
            <% end If %>
         </center>

    </div>
    </form>
  
    <script>

        function TotalArea()
        {

                      
            

              <% If ViewState("nivel") = "0" Then%>
            'Centrais'
            angular.element(document.getElementById('myApp')).scope().area = '';
               <%ElseIf ViewState("nivel") = "1" Then%>
            angular.element(document.getElementById('myApp')).scope().areaInterna = '';
                                <%ElseIf ViewState("nivel") = "2" Then%>
            angular.element(document.getElementById('myApp')).scope().grupo = '';
                                <%ElseIf ViewState("nivel") = "3" Then%>
            angular.element(document.getElementById('myApp')).scope().grupo = '';

            <% End If%>
                   
           
            


            CarregaVencimento('<%=Replace(ViewState("vencimento"), "/", "") %>');
        }

        $(function () {

            $("#divLoader").hide();
            $("#divMainGrafico").show();

            $("#divDetalhar").text('Selecionar no gráfico acima uma área para detalhamento');


            //$('#container').highcharts().redraw();
                
                //alert('teste');
            // $('#containerGrafico').highcharts({
            var chart = new Highcharts.Chart({

                    chart: {
                        type: 'bar',
                        renderTo: 'containerGrafico',

                        height:  <%= IIf((RowsCount * 50) < 500, 500, (RowsCount * 50))%> ,
                       // height: 700,

                        //options3d: {
                        //    enabled: true,
                        //    alpha: 0,
                        //    beta: 0,
                        //    viewDistance: 0,
                        //    depth: 0
                        //},
                        marginTop: 80,
                        marginRight: 40
                    },
                    colors: ['#5F9EA0', '#FFA500', '#FFB6C1', '#00BFFF', '#4682B4', '#BDB76B', '#DCDCDC', '#FF6347', '#008B45', '#FFB90F', '#9F79EE'],
                  title: {
                        text: 
                            <% If AppIni.GloboRJ_Parm = True Then%>
                                <% If ViewState("nivel") = "0" Then%>
                                ' '
                                <%ElseIf ViewState("nivel") = "1" Then%>
                                ' ' + '<%= IIf(ViewState("area") = "", Session("area" & Session("codigousuario")), ViewState("area")) %>' +'<br />  Áreas Internas'
                                <%ElseIf ViewState("nivel") = "2" Then%>
                                ' ' + '<%= IIf(ViewState("area_interna") = "", Session("area_interna" & Session("codigousuario")), ViewState("area_interna"))  %>' +'<br />  <%=labelCCusto%>'
                                <%ElseIf ViewState("nivel") = "3" Then%>
                                ' ' + '<%= IIf(ViewState("ccusto") = "", (ViewState("nome_usuario") & ViewState("grupo") & ViewState("nome_grupo")).ToString.Replace("AR: ", ""), ViewState("ccusto")) %>' + '<br />  Usuários ' 
                <% End If%>
                            <% Else%>
                '<%= IIf(ViewState("ccusto") = "", (ViewState("nome_usuario") & ViewState("grupo") & ViewState("nome_grupo")).ToString.Replace("AR: ", ""), ViewState("ccusto")) %>' + '<br />  Top 10  <%=IIf(ViewState("nivel") = "3", "Usuários", "Centros de Custo") %> '
                            <% End If%>
                            
            },

                xAxis: {
                categories: [<%=GraficoLabel %>],
                    labels: {
                        style: {
                                color: '#007EFF',
                                textDecoration: 'underline',
                                cursor: 'hand'
                        }
                        <%--     formatter: function () {
                            //return '<a href=http://localhsot/' + GraficoUsuarioReturnUrl(this.value, '<%=ViewState("nivel")%>') + ' >' + this.value + '</a>';
                            //var url = GraficoUsuarioReturnUrl(this.value, '<%=ViewState("nivel")%>');

                            //return '<a href="<%=myURL%>' + GraficoUsuarioReturnUrl(this.value, '<%=ViewState("nivel")%>') + '" >' + this.value + '</a>';
                            return '<a href="javascript:void(0)" >' + this.value + '</a>';
                     
                        }--%>

                 
                    }
                    
                },

                yAxis: {
                        
                        allowDecimals: false,
                      //  min: <%= negativeValue %>,
                    //type: 'logarithmic',
                        title: {
                                text: 'Gasto'
                        },
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
                        //pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: R$ {point.y:.2f}',
                            pointFormat: '<span style="color:{series.color}">\u25CF</span> R$ {point.y:.2f}',
                            formatter: function () {
                                //return '<b>'+ this.series.name +'</b><br/>'+
                                return '<b>' + this.series.name + '</b><br/>' +
                                    //this.x +': '+ 'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
                                    'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
                            }
                    },
                    shadow: {
                            color: 'yellow',
                            width: 50,
                            offsetX: 10,
                            offsetY: 0
                    },

                    plotOptions: {
                            column: {
                                stacking: 'normal',
                                depth: 40
                            },

                        series: {
                                stacking: 'normal',
                                cursor: 'normal',
                                point: {
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

         <% if ViewState("nivel") <=1 Then %>
        $('.highcharts-axis-labels text, .highcharts-axis-labels span').click(function () {
            //alert(this.textContent);
           // alert(<%=ViewState("nivel")%>);
            var area = $("#cmbCentral").val();
            var area_interna = $("#cmbAreaInternas").val();
            var ccusto = $("#cmbGrupos").val();

                    <% If AppIni.GloboRJ_Parm = True Then%>
                                <% If ViewState("nivel") = "0" Then%>
                                 //Centrais
                                    area=this.textContent;
                                    removeChosen('cmbCentral');
                                    angular.element(document.getElementById('myApp')).scope().area=area;
                                    angular.element(document.getElementById('myApp')).scope().strArea='-'  + area;                                   
                                    
                                    $("#cmbCentral").val(area);
                                    aplicaChosen('cmbCentral');
                                    
                                    

                                <%ElseIf ViewState("nivel") = "1" Then%>
                                    area_interna=this.textContent;
                                    removeChosen('cmbAreaInternas');
                                    
                                    angular.element(document.getElementById('myApp')).scope().areaInterna=area_interna;
                                    angular.element(document.getElementById('myApp')).scope().strareaInterna='-'  + area_interna;
                                    
                                    $("#cmbAreaInternas").val(area_interna);
                                    aplicaChosen('cmbAreaInternas');
                                    

            <%ElseIf ViewState("nivel") = "2" Then%>
                                        //alert("teste");
                                        //alert(this.textContent);
                                        var cod = this.textContent;
                                        removeChosen('cmbGrupos');
                                        ccusto=cod.split('-')[0].replace(' ', '');
                                        angular.element(document.getElementById('myApp')).scope().grupo=ccusto;
                                        angular.element(document.getElementById('myApp')).scope().strgrupo=ccusto;
                                        
                                        $("#cmbGrupos").val(ccusto);
                                        aplicaChosen('cmbGrupos');
                                        
                                        //alert("teste");

                               
                <% End If%>
            <% Else%>
                                    
                                    //ccusto=this.textContent;
               <% End If%>
                
                
                
            //alert(getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7));
            //CarregaVencimento(getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7));
            //var mesAno = getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7);
            //alert(mesAno);
            //vencimento = getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7)
            var url = GraficoUsuarioReturnUrl(this.value, '<%=ViewState("nivel")%>')+'&area='+encodeURIComponent(area)+'&area_interna='+encodeURIComponent(area_interna)+'&ccusto='+encodeURIComponent(ccusto);
            //alert(url);

            angular.element(document.getElementById('myApp')).scope().vencimento='<%=ViewState("vencimento")%>';

            //alert(angular.element(document.getElementById('myApp')).scope().vencimento);

           //alterado para filtrar por area -antes so atualizava as infos de top10
            CarregaMain('<%=request.querystring("tipoGrafico")%>','<%=ViewState("codServico")%>','<%=request.querystring("TipoVisao")%>',escape(area),escape(area_interna),escape(ccusto));

            angular.element(document.getElementById('myApp')).scope().atualizaInfos();
           
            $("#divLoader").show();
            //$("#divMainGrafico").hide();
            //graficoMensal
            //divMainGrafico
            $("#graficoMensal").load(url, function () {
                //alert("Load was performed.");
                $("#divLoader").hide();
                $("#divMainGrafico").show();
                
            });
           

        });
        <%Else%>
        $('.highcharts-axis-labels text, .highcharts-axis-labels span').click(function () {
            //var ccusto=this.textContent;
            var cod = this.textContent;
            var ccusto=cod.split('-')[0].replace(' ', '');
            removeChosen('cmbGrupos');
            angular.element(document.getElementById('myApp')).scope().grupo=ccusto;
            angular.element(document.getElementById('myApp')).scope().atualizaInfos();
            $("#cmbGrupos").val(ccusto);
            aplicaChosen('cmbGrupos');
            //alterado para filtrar por area -antes so atualizava as infos de top10
            CarregaMain('<%=request.querystring("tipoGrafico")%>','<%=ViewState("codServico")%>','<%=request.querystring("TipoVisao")%>',escape(''),escape(''),escape(ccusto));
        });

        <%End If%>
        });

   

        function ResizeMe() {
            //alert(document.getElementById('faturasMain').offsetHeight + 300);
            //parent.document.getElementById('conteudo').height = document.getElementById('faturasMain').offsetHeight + 300;
            parent.document.getElementById('divgeral').style.height = document.getElementById('container').offsetHeight + 800 + 'px';
            $('#divgeral', window.parent.document).contents().find("body").height = document.getElementById('container').offsetHeight + 800;

        }

        
          function GraficoUsuarioReturnUrl(codigo, nivel) {
                //alert(ccusto);
              //alert(cod);
              var tipoVisao = '<%=ViewState("tipoVisao") %>' ;
              //alert(tipoVisao);
                <% If ViewState("nivel") <> "4" Then%>

                <% Session("exibir_todos") = False %>

                if (nivel == "0") {
                    var url = 'uc_grafico_areas.aspx?tipoRel=1&mesAno=<%=ViewState("vencimento")%>&tipoGrafico=<%=ViewState("tipoGrafico")%>&codigoTipo=<%=ViewState("codServico")%>&nomeOper=<%= ViewState("nomeOper")%>&nomeServico=<%= ViewState("nomeServico")%>&area=' + codigo + '&nivel=1&tipoVisao=' + tipoVisao;
                } else if (nivel == "1") {
                    var url = 'uc_grafico_areas.aspx?tipoRel=1&mesAno=<%=ViewState("vencimento")%>&tipoGrafico=<%=ViewState("tipoGrafico")%>&codigoTipo=<%=ViewState("codServico")%>&nomeOper=<%= ViewState("nomeOper")%>&nomeServico=<%= ViewState("nomeServico")%>&area_interna=' + codigo + '&nivel=2&tipoVisao=' + tipoVisao;
                } else if (nivel == "2") {
                    var url = 'uc_grafico_areas.aspx?tipoRel=1&mesAno=<%=ViewState("vencimento")%>&tipoGrafico=<%=ViewState("tipoGrafico")%>&codigoTipo=<%=ViewState("codServico")%>&nomeOper=<%= ViewState("nomeOper")%>&nomeServico=<%= ViewState("nomeServico")%>&ccusto=' + codigo + '&nivel=3&tipoVisao=' + tipoVisao;
                } else if (nivel == "3") {
                    var cod = codigo.split('>')[0];
                    cod = cod.replace('<', '');
                    var url = 'uc_grafico_areas.aspx?codigo_usuario=' + cod + '&vencimento=<%=ViewState("vencimento")%>';
                }

                <% End If%>

                //alert(nomeOper);
                //alert(mes);
                //alert(url);
                return url;
            }



        function CarregaGraficoMain(url)
        {
            $("#divLoader").show();
            $("#divMainGrafico").hide();
            $("#divMainGrafico").load(url, function () {
                //alert("Load was performed.");
                $("#divLoader").hide();
                $("#divMainGrafico").show();
                
            });
        }

    </script>

    
     

        <%--<script src="vendors/farbtastic/farbtastic.min.js"></script>                   
        <script src="js/charts.js"></script>--%>
</body>
</html>
