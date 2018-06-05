<%@ Page Language="VB" AutoEventWireup="false" CodeFile="uc_graficoHome.aspx.vb" Inherits="uc_graficoHome" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
      <meta charset="utf-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <title></title>

 <%--   <script src="vendors/bower_components/jquery/dist/jquery.min.js"></script>
        <!-- Highcharts -->
        <script src="Highcharts-4.1.9/js/highcharts.js"></script>
        <script src="Highcharts-4.1.9/js/highcharts-3d.js"></script>
        <script src="Highcharts-4.1.9/js/modules/exporting.js"></script>--%>


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


    <style>

        .rowStyle{

            letter-spacing:-1px;
        }

    </style>


</head>
<body  >
    <form id="form1" runat="server">

         

    <div class="card" id="card">

        <div class="card-header">
        </div>

        <div class="card-body card-padding">

            <div class="well">
            <div class="row">
                <div class="col-sm-6">
                <div style="float:left;vertical-align:middle; height:30px;padding-right:10px;"> <h5>Telefonia</h5></div>
               
                <asp:TextBox ID="txtTipoGrafico" runat="server" Visible="false"></asp:TextBox>                    
                     <asp:TextBox ID="txtTipoVisao" runat="server" Visible="false"></asp:TextBox>
                <div class="btn-group btn-group" role="group">
                                    
                <button type="button" id="btGraficoTotal" style="margin-right:5px; width:120px;" class="btn btn-primary waves-effect" onclick="ChamaGraficoTotal()">Consolidado</button>                   
                <button type="button" id="btGraficoMovel" style="margin-right:5px; width:120px;" class="btn btn-primary waves-effect" onclick="ChamaGraficoMovel()">Movel</button>
                <button type="button" id="btGraficoFixo"  style="margin-right:5px; width:120px;" class="btn btn-primary waves-effect" onclick="ChamaGraficoFixo()" ng-show="showtop10Fixo">Fixo</button>                                                    
                    <button type="button" id="btTarifacao"  style="margin-right:5px; width:120px;" class="btn btn-primary waves-effect" onclick="ChamaGraficoRamal()" ng-show="showtop10Ramal">Ramais</button>                                                    
                <%--<asp:LinkButton type="button" id="btTarifacao"  style="width:120px;" class="btn bgm-orange waves-effect" runat="server" Visible="false" PostBackUrl="~/GastoUsuarioRamal.aspx?mostraArea=S">Tarifação</asp:LinkButton>                                    --%>
                <%--<asp:LinkButton type="button" id="btTarifacao"  style="width:120px;" class="btn bgm-orange waves-effect" runat="server" Visible="false" OnClientClick="ChamaGraficoRamal()">Tarifação</asp:LinkButton>                                    --%>

              </div>
            </div>
            <asp:PlaceHolder id="spanExibePor" runat="server">
                <div class="col-sm-1" > <h5 style="margin-top:7px">Exibir por</h5></div>
                <div class="col-sm-5" style="margin-top: 5px;">
            
          
               <label class="radio radio-inline m-r-20">
                                <input type="radio" id="rbTipo" ng-model="tipoVisao"  name="inlineRadioOptions" value="Tipo" checked="checked" onclick="ChamaGraficoTipo()">
                                <i class="input-helper"></i>  
                                Tipo de serviço
               </label>
                            
                <label class="radio radio-inline m-r-20">
                                <input type="radio" id="rbOper"  ng-model="tipoVisao" name="inlineRadioOptions" value="Oper" onclick="ChamaGraficoOperadora()"  >
                                <i class="input-helper"></i>  
                                Operadora
                </label>
            
        </div>
      </asp:PlaceHolder>  
            </div>
            </div>
               <div class="row">
                            <div class="col-sm-6 col-md-4">
                                <div class="mini-charts-item bgm-cyan">
                                    <div class="clearfix">
                                        <div class="chart stats-bar"></div>
                                        <div class="count">
                                            <small>Consolidado 12 meses</small>
                                            <h2><%=FormatCurrency(total12meses) %></h2>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-sm-6 col-md-4">
                                <div class="mini-charts-item bgm-lightgreen">
                                    <div class="clearfix">
                                        <div class="chart stats-bar-2"></div>
                                        <div class="count">
                                            <small>Consolidado no último mês (<%=MesAtual %>)</small>
                                            <h2><%=FormatCurrency(totalMesAtual) %></h2>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            <div class="col-sm-6 col-md-4">
                                <div class="mini-charts-item bgm-deeppurple">
                                    <div class="clearfix">
                                        <div class="chart stats-line"></div>
                                        <div class="count">
                                            <small>Variação do mês anterior</small>
                                            <h2><%=FormatCurrency(VariacaoMesAnterior) %></h2>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            
                            
                        </div>
              
            <div role="tabpanel" id="divPainel">
                                <ul class="tab-nav" role="tablist">
                                    <li class="active"><a href="#TABGRAFICO" aria-controls="home11" role="tab" data-toggle="tab">GRÁFICO</a></li>
                                    <li><a href="#TABDADOS" aria-controls="profile11" role="tab" data-toggle="tab">TABELA</a></li>
                                  
                                </ul>
                              
                                <div class="tab-content">
                                    <div role="tabpanel" class="tab-pane active" id="TABGRAFICO">
                                      <div class="row">
                                         <div class="table-responsive">
                                        
                                                  <div id="graficoMensal" ></div>
                                                  <p class="text-center" >
                                                      <span runat="server" id="divDetalhar" visible="false" >Selecione um mês para detalhar</span>
                                                      <span runat="server" id="divDetalharGeral" visible="false" >Selecione um mês para visualizar indicadores de consumo</span>
                                                      </p>
                                         </div>
                                      </div>



                                      
                                    </div>
                                    <div role="tabpanel" class="tab-pane" id="TABDADOS">
                                        
                                         <div class="row">                
                                            <div class="table-responsive">
                                                                            <asp:GridView ID="gvResumoMensal" runat="server" EnableSortingAndPagingCallbacks="True"
                                                                AllowSorting="True" AutoGenerateColumns="False" CellPadding="4" DataSourceID="SqlDataSourceResumoGeral" Font-Size="XX-Small"
                                                                Width="800px"
                                                                EnableModelValidation="True">
                                                                <FooterStyle BackColor="GRAY" Font-Bold="True" ForeColor="White" />
                                                                <RowStyle BackColor="#FFFBD6" ForeColor="#333333" />
                                                                <Columns>
                                                                    <asp:BoundField DataField="DATA" HeaderText="DATA" SortExpression="DATA">
                                                                        <ItemStyle HorizontalAlign="Center" />
                                                                    </asp:BoundField>
                                                                    <%--   <asp:HyperLinkField DataNavigateUrlFields="DATA" 
                                                                                DataNavigateUrlFormatString="rit.aspx?competencia={0}" HeaderText="DATA" />--%>
                                                                    <asp:BoundField DataField="GASTO" HeaderText="GASTO" SortExpression="GASTO" DataFormatString="{0:c}"   HtmlEncode="False">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                    <asp:BoundField DataField="codigo_tipo" HeaderText="codigo_tipo" SortExpression="codigo_tipo">
                                                                        <ItemStyle HorizontalAlign="Right" />
                                                                    </asp:BoundField>
                                                                </Columns>
                                                                <PagerStyle BackColor="GRAY" ForeColor="#333333" HorizontalAlign="Center" />
                                                                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="Navy" />
                                                                <HeaderStyle BackColor="GRAY" Font-Bold="True" ForeColor="White" />
                                                                <AlternatingRowStyle BackColor="White" />
                                                            </asp:GridView>
                                                            <asp:GridView ID="gvResumoGeral" runat="server" AllowPaging="True" EnableSortingAndPagingCallbacks="true"
                                                               Width="98%" AutoGenerateColumns="true" class="table table-striped" BorderWidth="0" >
                                                               <RowStyle CssClass="rowStyle" />
   
                                                            </asp:GridView>

                                                             <asp:GridView ID="gvServicos" runat="server" AllowPaging="True" EnableSortingAndPagingCallbacks="true" 
                                                               Width="98%" AutoGenerateColumns="true" class="table table-striped" BorderWidth="0"  Visible="false">
                                                               
   
                                                            </asp:GridView>
               
                                                </div>
                                    </div>                                 
                                </div>
                            </div>


           
            </div>
        </div>
    </div>

          <input type="text" id="txtVencimento" ng-model="vencimento" ng-init="vencimento='<%=ViewState("vencimento") %>'" style="display:none"/>    
      <input type="text" id="txtGrupo" ng-model="grupo" ng-init="grupo='<%=ViewState("grupo") %>'" style="display:none"/>
      <input type="text" id="txtArea" ng-model="area" ng-init="area='<%=ViewState("area") %>'" style="display:none"/>
      <input type="text" id="txtAreaInterna" ng-model="areaInterna" ng-init="areaInterna='<%=ViewState("areaInterna") %>'" style="display:none"/>
       <input type="text" id="txtCodigoUsuario" ng-model="codigousuario" ng-init="codigousuario='<%=Session("codigousuario") %>'" style="display:none"/>

         <asp:SqlDataSource ID="SqlDataSourceResumoGeral" runat="server" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"
    SelectCommand=""></asp:SqlDataSource>
    </form>


    <script>
                function CarregaGrafico()
                {

           //alert("passou");
                    //var width = $('#myApp').width();
                    //alert(width);

              var width = $('#myApp').width();
              var parentWidth = $('#divMainGrafico').offsetParent().width();
              var percent = 100*width/parentWidth;
              //alert(percent);
              $('#graficoMensal').width($('#myApp').width()-50);

              $('#graficoMensal').highcharts({

            chart: {
                type: 'line',
                options3d: {
                    enabled: true,
                    alpha: 0,
                    beta: 0,
                    viewDistance: 25,
                    depth: 40
                },
                marginTop: 80,
                marginRight: 40,
                //width: '1000px'
            },
              colors: ['#5F9EA0', '#FFA500', '#FFB6C1', '#00BFFF', '#4682B4', '#BDB76B', '#DCDCDC', '#FF6347', '#008B45', '#FFB90F', '#9F79EE'],
            title: {
                 text: 'Evolução de Gastos'
            },

            xAxis: {
                categories: [<%=GraficoLabel %>]
                 , labels: {
                     style: {
                         color: '#007EFF',
                         textDecoration: 'underline',
                         cursor: 'hand'
                     }
                     ,formatter: function () {
                         //return '<a href="javascript(0)">' + this.value + '</a>';
                         //return  getMes(this.value.substring(0, 2)) + this.value.substring(2, 7);
                         return  this.value;
                     }
                 }
            },

            yAxis: {
                allowDecimals: false,
                min: 0,
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
                  formatter: function() {
						return 'R$ ' + Highcharts.numberFormat(this.total, 2, ',', '.');
					}
            }
            },

            tooltip: {
                headerFormat: '<b>{point.key}</b><br>',
                pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: R$ {point.y:.2f}'
                ,
                formatter: function() {
						return '<b>'+ this.series.name +'</b><br/>'+
							this.x +': '+ 'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
					}
            },

            plotOptions: {
                column: {
                    stacking: 'normal',
                    depth: 40,
                     dataLabels: {
                    enabled: false
                }
                },

                series: {
                    cursor: 'normal',
                    point: {
                        events: {
                            click: function (e) {
                            GraficoCusto(this.category);
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

	
                 <% if ExibeMovel Then %>
                {
					name: 'Telefonia Móvel',
					data: [<%=GraficoData %>]
                    ,lineWidth:4
                    
				} 
                 <% end if %>
                  <% if ExibeFixo And exibeRamail = False Then %>
                <% if ExibeMovel Then %>
                  <%=virgulaGrafico%>
                  <% end If %>{
					name: 'Telefonia Fixa',
					data: [<%=GraficoData2 %>]
                    ,lineWidth:4
				}  
                   <% end if %>
                   <% if Exibe0800 Then %>
                  <%=virgulaGrafico%>{
					name: '0800',
					data: [<%=GraficoData3 %>]
                    ,lineWidth:4
				}  
                 <% end if %>
                <% if Exibe3003 Then %>
                  <%=virgulaGrafico%>{
					name: 'Num. Único',
					data: [<%=GraficoData4 %>]
                    ,lineWidth:4
				}
                <% end if %>
                <% if ExibeServico Then %>
                  <%=virgulaGrafico%>{
					name: 'Serviços',
					data: [<%=GraficoData5 %>]
                    ,lineWidth:4
				}
                 <% end if %>
                   <% if ExibeDados then %>
                  <%=virgulaGrafico%>{
					name: 'Link de Dados',
					data: [<%=GraficoData6 %>]
                    ,lineWidth:4
				}
                 <% end if %>
                   <% if exibeRamail Then %>
                     <% if ExibeMovel Then %>
                  <%=virgulaGrafico%>
                  <% end if %>
                {
					name: 'Ramais',
					data: [<%=GraficoData7 %>]
                    ,lineWidth:4
				}
                 <% end if %>
                 ]


              });
              //alert(String(<%=GraficoData %>));
            $('#graficoMensal').highcharts().redraw();
            $('#graficoMensal').show();

                }



        function CarregaGraficoMovel() {

            var width = $('#myApp').width();
            var parentWidth = $('#divMainGrafico').offsetParent().width();
            var percent = 100*width/parentWidth;
            //alert(percent);
            $('#graficoMensal').width($('#myApp').width()-50);

            //alert('teste');
           $('#graficoMensal').highcharts({

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
                colors: ['#5F9EA0', '#FFA500', '#FFB6C1', '#00BFFF', '#4682B4', '#8085E9', '#DCDCDC', '#FF6347', '#008B45', '#FFB90F', '#9F79EE'],
                title: {
                    text: 'Evolução dos Custos <br>  <%=viewstate("nome_usuario") %>  <%= ViewState("grupo") %>  <%= ViewState("nome_grupo") %>'
                },

                xAxis: {
                    categories: [<%=GraficoLabel %>],                            
                    labels: {
                        style: {
                            color: '#007EFF',
                            textDecoration: 'underline',
                            cursor: 'hand'
                        },
                        formatter: function () {

                            //return  this.value;
                            return  getMes(this.value.substring(0, 2)) + this.value.substring(2, 7);
                        }
                    }

                },

                yAxis: {
                    allowDecimals: true,
                    min: <%=negativeValue%>,
                    //type: 'logarithmic',
                    title: {
                        text: 'Gasto'
                    },
                    stackLabels: {
                        enabled: true,
                        style: {
                            fontWeight: 'bold',
                            fontSize: '9px',
                            color: (Highcharts.theme && Highcharts.theme.textColor) || 'gray'
                        }
                    ,formatter: function() {
                        if(this.total>0)
                        {
                            //return 'R$ ' + Highcharts.numberFormat(this.total, 2, ',', '.');
                            //return 'R$ ' + Highcharts.numberFormat(this.stackTotal, 2, ',', '.');
                            //return this.stack;
                            return '<span style=font-size:8px!important>R$' + Highcharts.numberFormat(this.total, 2, ',', '.') +'</span>';
                        }
                        else
                        {
                            return '' + Highcharts.numberFormat(this.total, 2, ',', '.');
                        }
                        
                    }
                    }
                },

                tooltip: {
                    headerFormat: '<b>{point.key}</b><br>',
                    pointFormat: '<span style="color:{series.color}">\u25CF</span> {series.name}: R$ {point.y:.2f}',
                    formatter: function() {
                        return '<b>'+ this.series.name +'</b><br/>'+
                            this.x +': '+ 'R$ ' + Highcharts.numberFormat(this.y, 2, ',', '.');
                    }
                },

                plotOptions: {
                    column: {
                        stacking: 'normal',
                        depth: 40,
                        dataLabels: {
                            enabled: false
                        }
                    },

                    series: {
                        cursor: 'pointer',
                        point: {
                            events: {
                                click: function (e) {
                                    GraficoCusto(this.category,  this.series.name);
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

           $('#graficoMensal').highcharts().redraw();
           $('#graficoMensal').show();
        }


        $(function() {

            

            //alert('<%=ViewState("vencimento") %>');

            if (<%=tipoGrafico%>==1)
            {
                //grafico geral
                CarregaGrafico();
                $('#btGraficoTotal').removeAttr( "class" )
                $('#btGraficoTotal').attr("class", "btn btn-primary waves-effect active");
            }
             if (<%=tipoGrafico%>==2)
            {
                 //grafico movel
                 //alert("movel");
                 CarregaGraficoMovel();
                 if (<%=tipoFatura%>==2)
                 {
                     //fixo
                     $('#btGraficoFixo').removeAttr( "class" )
                     $('#btGraficoFixo').attr("class", "btn btn-primary waves-effect active");
                 }
                 else
                 {
                     //movel
                     $('#btGraficoMovel').removeAttr( "class" )
                     $('#btGraficoMovel').attr("class", "btn btn-primary waves-effect active");
                 }
                
             }
             if (<%=tipoGrafico%>==3)
            {
                 //grafico movel
                 //alert("movel");
                 $('#btGraficoFixo').removeAttr( "class" );
                 $('#btGraficoFixo').attr("class", "btn btn-primary waves-effect active");
                 $('#btGraficoMovel').removeAttr( "class" );
                 $('#btGraficoMovel').attr("class", "btn btn-primary waves-effect active");
                 CarregaGraficoMovel();   
                 //alert('teste');
                
             }

             if ('<%=TipoVisao%>'=='Oper')
            {
                
                 $('#rbOper').attr('checked', true);
                 $('#rbTipo').attr('checked', false);              

                
                 
             }
             else {
                
                 $('#rbOper').attr('checked', false);
                 $('#rbTipo').attr('checked', true);
                 
             }


           
            
            //alert('<%=ViewState("vencimento") %>');
            CarregaVencimento('<%=ViewState("vencimento") %>');
           

            $('.highcharts-axis-labels text, .highcharts-axis-labels span').click(function () {
                //alert(this.textContent);
                //alert(getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7));
                CarregaVencimento(getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7));
                var mesAno = getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7);
                //alert(mesAno);
                //vencimento = getInvertMes(this.textContent.replace("/","").substring(0, 3))+ this.textContent.replace("/","").substring(3, 7)
               
                var area = $("#cmbCentral").val();
                var areaInterna = $("#cmbAreaInternas").val();
                var grupo = $("#cmbGrupos").val();

               
               // alert(<%=ViewState("tipoGrafico")%>);

                <% If tipoGrafico = "2" Or tipoGrafico = "3" Then %>
                $("#divLoader").show();
                //$("#divMainGrafico").hide();

               // alert('<%=TipoVisao%>');
                var tipoVisao = '<%=TipoVisao%>' ;

                var url ="uc_grafico_areas.aspx?codigoTipo=<%=Request.QueryString("tipoFatura")%>&tipoGrafico="+<%=tipoGrafico %>+ "&tipoVisao=" + tipoVisao +"&mesAno=" + mesAno+ "&area="+encodeURIComponent(area)+"&area_interna="+encodeURIComponent(areaInterna)+"&ccusto="+encodeURIComponent(grupo);
                //history.pushState({}, '', url);            
               
                //graficoMensal
                //divMainGrafico
                $("#graficoMensal").load(url, function () {
                    //alert("Load was performed.");
                    $("#divLoader").hide();
                    $("#divMainGrafico").show();
                
                });
                <% End If%>

            });


           
    
           
        });


        
        function ChamaGraficoMovel()
        {

            //angular.element(document.getElementById('myApp')).scope().area = '';            
            //angular.element(document.getElementById('myApp')).scope().areaInterna = '';            
            //angular.element(document.getElementById('myApp')).scope().grupo = ''; 

            var area = $("#cmbCentral").val();
            var areaInterna = $("#cmbAreaInternas").val();
            var grupo = $("#cmbGrupos").val();
            //alert(areaInterna);
            CarregaMain("2","1",'<%=TipoVisao%>',escape(area),escape(areaInterna),escape(grupo));
            //alert("teste");
            //$("#divMainGrafico").load("uc_graficoHome.aspx?tipoGrafico=2&grupo=" + escape(grupo) + "&area=" + escape(area) + "&area_interna=" + escape(areaInterna), function () {
            //    //alert("Load was performed.");
            //    alert("teste");
            //});
            
        }
        
      

        function ChamaGraficoTotal()
        {
            var area = $("#cmbCentral").val();
            var areaInterna = $("#cmbAreaInternas").val();
            var grupo = $("#cmbGrupos").val();
            //alert(area)
            
            CarregaMain("1","1",'<%=TipoVisao%>',escape(area),escape(areaInterna),escape(grupo));
            
            
        }

        function ChamaGraficoFixo()
        {
            var area = $("#cmbCentral").val();
            var areaInterna = $("#cmbAreaInternas").val();
            var grupo = $("#cmbGrupos").val();
            var tipofatura = $("#txtTipoGrafico").val();
            //alert(area)
            
            CarregaMain("2","2",'<%=TipoVisao%>',escape(area),escape(areaInterna),escape(grupo));
            //alert("teste");
            //$("#divMainGrafico").load("uc_graficoHome.aspx?tipoGrafico=2&grupo=" + escape(grupo) + "&area=" + escape(area) + "&area_interna=" + escape(areaInterna), function () {
            //    //alert("Load was performed.");
            //    alert("teste");
            //});
         
        }
         function ChamaGraficoRamal()
        {
            var area = $("#cmbCentral").val();
            var areaInterna = $("#cmbAreaInternas").val();
            var grupo = $("#cmbGrupos").val();
            var tipofatura = $("#txtTipoGrafico").val();
            //alert(area)
            
            CarregaMain("3","2",'<%=TipoVisao%>',escape(area),escape(areaInterna),escape(grupo));
            //alert("teste");
            //$("#divMainGrafico").load("uc_graficoHome.aspx?tipoGrafico=2&grupo=" + escape(grupo) + "&area=" + escape(area) + "&area_interna=" + escape(areaInterna), function () {
            //    //alert("Load was performed.");
            //    alert("teste");
            //});
         
        }

         function ChamaGraficoOperadora()
        {
            var area = $("#cmbCentral").val();
            var areaInterna = $("#cmbAreaInternas").val();
            var grupo = $("#cmbGrupos").val();
            
            //alert("Oper")
            CarregaMain("2",'<%=tipoFatura%>','Oper',escape(area),escape(areaInterna),escape(grupo));
            //alert("teste");
            //$("#divMainGrafico").load("uc_graficoHome.aspx?tipoGrafico=2&grupo=" + escape(grupo) + "&area=" + escape(area) + "&area_interna=" + escape(areaInterna), function () {
            //    //alert("Load was performed.");
            //    alert("teste");
            //});
         }

         function ChamaGraficoTipo()
         {
             var area = $("#cmbCentral").val();
             var areaInterna = $("#cmbAreaInternas").val();
             var grupo = $("#cmbGrupos").val();
             
             //alert(area)
             CarregaMain("2",'<%=tipoFatura%>','Tipo',escape(area),escape(areaInterna),escape(grupo));
             //alert("teste");
             //$("#divMainGrafico").load("uc_graficoHome.aspx?tipoGrafico=2&grupo=" + escape(grupo) + "&area=" + escape(area) + "&area_interna=" + escape(areaInterna), function () {
             //    //alert("Load was performed.");
             //    alert("teste");
             //});
         }

        function EscondeGrid()
        {
            //alert("teste");
            $("#divPainel").hide(1000);
            swal({   
                title: "",   
                text: "Sem informações no período",   
                timer: 2000,   
                showConfirmButton: false 
            });

       
        }


        function getMes(mes) {
            //alert(mes);
            var mesext = mes;
            switch (mes) {
                case "01":
                    mesext = "Jan";
                    break;
                case "02":
                    mesext = "Fev";
                    break;
                case "03":
                    mesext = "Mar";
                    break;
                case "04":
                    mesext = "Abr";
                    break;
                case "05":
                    mesext = "Mai";
                    break;
                case "06":
                    mesext = "Jun";
                    break;
                case "07":
                    mesext = "Jul";
                    break;
                case "08":
                    mesext = "Ago";
                    break;
                case "09":
                    mesext = "Set";
                    break;
                case "10":
                    mesext = "Out";
                    break;
                case "11":
                    mesext = "Nov";
                    break;
                case "12":
                    mesext = "Dez";
                    break;               
            }
            return mesext;

        }

        function getInvertMes(mes) {
            //alert(mes);
            mes=mes.toUpperCase()
            var mesext = mes;
            //alert(mes);
            switch (mes) {
                case "JAN":
                    mesext = "01";
                    break;
                case "FEV":
                    mesext = "02";
                    break;
                case "MAR":
                    mesext = "03";
                    break;
                case "ABR":
                    mesext = "04";
                    break;
                case "MAI":
                    mesext = "05";
                    break;
                case "JUN":
                    mesext = "06";
                    break;
                case "JUL":
                    mesext = "07";
                    break;
                case "AGO":
                    mesext = "08";
                    break;
                case "SET":
                    mesext = "09";
                    break;
                case "OUT":
                    mesext = "10";
                    break;
                case "NOV":
                    mesext = "11";
                    break;
                case "DEZ":
                    mesext = "12";
                    break;               
            }
            return mesext;

        }

          </script>

    

   
        <script src="vendors/farbtastic/farbtastic.min.js"></script>                   
        <script src="js/charts.js"></script>

   <%--     <script src="http://ajax.googleapis.com/ajax/libs/angularjs/1.4.8/angular.min.js"></script>
        <script src="Scripts/angular/app.js"></script>
        <script src="Scripts/angular/SGPCctrljs.js"></script>--%>
        
        <%--<script src="js/demo.js"></script>--%>

       
</body>
</html>
