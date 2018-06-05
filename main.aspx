﻿<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="main.aspx.vb" Inherits="main" EnableSessionState="true" %>



<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server"  >
  


    <div id="myApp" ng-app="SGPCAPP" ng-controller="homeCtrl">

    <div class="block-header">
        <h2>CUSTOS COM TELEFONIA</h2>



    </div>


            <div class="card">

        <div class="card-header">
          
        </div>

        <div class="card-body card-padding">
            <div class="row">              


                <div class="col-sm-3 m-b-15">
                    <p class="f-500 c-black m-b-15">{{ nomeArea }}</p>
                    <%--<asp:DropDownList ID="cmbCentral" runat="server" CssClass="chosen" ClientIDMode="Static" AutoPostBack="false"  data-placeholder="Selecione uma central..." DataValueField="CODIGO" DataTextField="DESCRICAO" />--%>

                    <select  ng-model="area" id="cmbCentral" data-placeholder="Selecione uma área..." style="width:100%" ng-change="GetAreasInternas()"  >
                        <option   ng-repeat="x in areas" value="{{x.Codigo}}">{{x.Descricao}}</option>

                    </select>
                </div>

               
                <div class="col-sm-3 m-b-15">
                    <p class="f-500 c-black m-b-15">{{ nomeAreaInterna }}</p>
                    <asp:DropDownList ID="cmbAreaInterna" runat="server"   AutoPostBack="True" ClientIDMode="Static" CssClass="chosen" DataValueField="CODIGO" DataTextField="DESCRICAO" Visible="false" />

                     <select ng-model="areaInterna" id="cmbAreaInternas" data-placeholder="Selecione uma área..." style="width:100%" ng-change="GetGrupos()" >
                        <option   ng-repeat="x in areasInternas" value="{{x.Codigo}}">{{x.Descricao}}</option>

                    </select>
                </div>
                  <div class="col-sm-3 m-b-15" id="div_grupo" runat="server" >
                    <p class="f-500 c-black m-b-15"  >{{ nomeCcusto }}</p>
                     <asp:DropDownList ID="ddlGrupos" runat="server"   AutoPostBack="True"  CssClass="chosen" ClientIDMode="Static" Visible="false"  >
                                                <asp:ListItem Value="" Selected="True">Selecione uma Visão</asp:ListItem>
                                            </asp:DropDownList>

                      <select ng-model="grupo" id="cmbGrupos" data-placeholder="Selecione..." style="width:100%;display:none;"  ng-change="carregaGraficos()"  >
                        <option   ng-repeat="x in grupos" value="{{x.Codigo}}">{{x.NomeGrupo}}</option>

                    </select>
                </div>


               
                
            </div>
        </div>
    </div>
        
            
              <!-- Page Loader -->
        <%--<div id="divLoader" style="text-align:center">
            <div class="card" style="height:500px;vertical-align:middle">
                <div class="card-body card-padding">
                    <div class="row">
                        <div class="preloader pls-blue">
                            <svg class="pl-circular" viewBox="25 25 50 50">
                                <circle class="plc-path" cx="50" cy="50" r="20" />
                            </svg>
                        </div>
                    </div>
                </div>
            </div>
        </div>--%>

        <div id="divLoader2" style="text-align: center">
            
            <%-- <div class="alert alert-info" role="alert">
                 <div class="preloader pl-xs pls-amber">
                                <svg class="pl-circular" viewBox="25 25 50 50">
                                    <circle class="plc-path" cx="50" cy="50" r="20"></circle>
                                </svg>
                            </div>
                 <br />
                 Carregando...</div>--%>

            <div class="page-loader">
            <div class="preloader pls-blue">
                <svg class="pl-circular" viewBox="25 25 50 50">
                    <circle class="plc-path" cx="50" cy="50" r="20" />
                </svg>

                <p>Please wait...</p>
            </div>
        </div>
        </div>
      
        <div id="divMainGrafico">


        </div>

    
    
              <asp:Button ID="btnPostBack" Visible="false" runat="server" />
             <asp:TextBox ID="hidden_area" runat="server" ClientIDMode="Static" Visible="false"></asp:TextBox>
      
        <input type="text" id="txtVencimento" ng-model="vencimento" style="display:none"/>    
      <input type="text" id="txtGrupo" ng-model="grupo" ng-init="grupo='<%=ViewState("grupo") %>'" style="display:none"/>
      <input type="text" id="txtArea" ng-model="area" ng-init="area='<%=ViewState("area") %>'" style="display:none"/>
      <input type="text" id="txtAreaInterna" ng-model="areaInterna" ng-init="areaInterna='<%=ViewState("areaInterna") %>'" style="display:none"/>
       <input type="text" id="txtCodigoUsuario" ng-model="codigousuario" ng-init="codigousuario='<%=Session("codigousuario") %>'" style="display:none"/>

        
       

    <div class="text-center" style="padding-bottom:20px; width:100%;height: 50px;">
        <center>
            <h4>
         <span class="text-center">{{ mesStr }}/{{ vencimento | limitTo : 4 : 2}} <span class="btn btn-warning waves-effect" ng-click="AvancaVencimento()" style="display:none;" ><i class="zmdi zmdi-arrow-forward"></i></span></span> 
        <span class="text-center" ng-bind="strArea"></span>
        <span class="text-center" ng-bind="strareaInterna"></span>
                <br />
        <span class="text-center" ng-bind="grupo"></span>
                </h4>
      </center>  
    </div>

    <div class="dash-widgets" >
                        <div class="row">
                           

                            <div ng-class="boxClass" ng-show="showtop10Movel">
                                <div id="site-visits" class="dash-widget-item bgm-deeppurple" style="min-height:800px;">
                                    <div class="dash-widget-header">                                       
                                       
                                        <div class="dash-widget-title">Top 10 Telefone Móvel ({{ mesStr }}/{{ vencimento | limitTo : 4 : 2}})</div>
                                        
                                        <ul class="actions actions-alt" style="display:none;">
                                            <li class="dropdown">
                                                <a href="" data-toggle="dropdown">
                                                    <i class="zmdi zmdi-more-vert"></i>
                                                </a>
                                                
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a href="">Refresh</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Manage Widgets</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Widgets Settings</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <div class="p-20">
                                         <span ng-repeat="x in top10Movel">

                                            <small>{{x.Descricao}}</small>
                                            <h3 class="m-0 f-400">{{x.Valor}}</h3>
                                        
                                        <br/>
                                        </span>
                                        
                                    </div>
                                </div>
                            </div>
<asp:placeholder runat="server" id="phFixo" Visible="true">
                            <div  ng-class="boxClass" ng-show="showtop10Fixo">
                                <div id="site-visits" class="dash-widget-item bgm-teal" style="min-height:800px;">
                                    <div class="dash-widget-header" >                                       
                                       
                                        <div class="dash-widget-title">Top 10 Telefone Fixo ({{ mesStr }}/{{ vencimento | limitTo : 4 : 2}})</div>
                                        
                                        <ul class="actions actions-alt" style="display:none;">
                                            <li class="dropdown">
                                                <a href="" data-toggle="dropdown">
                                                    <i class="zmdi zmdi-more-vert"></i>
                                                </a>
                                                
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a href="">Refresh</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Manage Widgets</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Widgets Settings</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <div class="p-20">
                                         <span ng-repeat="x in top10Fixo ">

                                            <small>{{x.Descricao}}</small>
                                            <h3 class="m-0 f-400">{{x.Valor}}</h3>
                                        
                                        <br/>
                                        </span>
                                        
                                    </div>
                                </div>
                            </div>
</asp:placeholder>
                            <div  ng-class="boxClass" ng-show="showtop10Ramal">
                                <div id="site-visits" class="dash-widget-item bgm-blue" style="min-height:800px;">
                                    <div class="dash-widget-header" >                                       
                                       
                                        <div class="dash-widget-title">Top 10 Ramais ({{ mesStr }}/{{ vencimento | limitTo : 4 : 2}})</div>
                                        
                                        <ul class="actions actions-alt" style="display:none;">
                                            <li class="dropdown">
                                                <a href="" data-toggle="dropdown">
                                                    <i class="zmdi zmdi-more-vert"></i>
                                                </a>
                                                
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a href="">Refresh</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Manage Widgets</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Widgets Settings</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <div class="p-20">
                                         <span ng-repeat="x in TopRamal ">

                                            <small>{{x.Descricao}}</small>
                                            <h3 class="m-0 f-400">{{x.Valor}}</h3>
                                        
                                        <br/>
                                        </span>
                                        
                                    </div>
                                </div>
                            </div>

                            <div ng-class="boxClass" ng-show="showServicosMes">
                                <div id="site-visits" class="dash-widget-item bgm-blue" style="min-height:800px;">
                                    <div class="dash-widget-header">                                       
                                   
                                        <div class="dash-widget-title">Tipo de Serviço ({{ mesStr }}/{{ vencimento | limitTo : 4 : 2}})</div>
                                        
                                        <ul class="actions actions-alt" style="display:none;">
                                            <li class="dropdown">
                                                <a href="" data-toggle="dropdown">
                                                    <i class="zmdi zmdi-more-vert"></i>
                                                </a>
                                                
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a href="">Refresh</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Manage Widgets</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Widgets Settings</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <div class="p-20">
                                         <span ng-repeat="x in ServicosMes">

                                            <small>{{x.Descricao}}</small>
                                            <h3 class="m-0 f-400">{{x.Valor}}</h3>
                                        
                                        <br/>
                                        </span>
                                        
                                    </div>
                                </div>
                            </div>

                             
                            <div ng-class="boxClass" ng-show="showLinhasPerfil" >
                           <%-- <div class="col-sm-3" ng-show="false" >--%>
                                <div id="site-visits" class="dash-widget-item bgm-orange" style="min-height:800px;">
                                    <div class="dash-widget-header">                                       
                                   
                                        <div class="dash-widget-title">Quantidade de linhas por perfil</div>
                                        
                                        <ul class="actions actions-alt" style="display:none;">
                                            <li class="dropdown">
                                                <a href="" data-toggle="dropdown">
                                                    <i class="zmdi zmdi-more-vert"></i>
                                                </a>
                                                
                                                <ul class="dropdown-menu dropdown-menu-right">
                                                    <li>
                                                        <a href="">Refresh</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Manage Widgets</a>
                                                    </li>
                                                    <li>
                                                        <a href="">Widgets Settings</a>
                                                    </li>
                                                </ul>
                                            </li>
                                        </ul>
                                    </div>
                                    
                                    <div class="p-20">
                                         <span ng-repeat="x in LinhasPerfil">

                                            <small>{{x.Descricao}}</small>
                                            <h3 class="m-0 f-400">{{x.Valor}} <small style="color:#fff"> LINHAS</small> </h3> 
                                        
                                        <br/>
                                        </span>
                                        
                                    </div>
                                </div>
                            </div>
                        </center>    
                            
                        </div>
                    </div>

        
    </div>    
  



    <script>
        var dados1;

        $(window).load(function () {
            //Welcome Message (not for login page)
            //alert("passou");
            function notify(message, type) {
                $.growl({
                    message: message
                }, {
                    type: type,
                    allow_dismiss: false,
                    label: 'Cancel',
                    className: 'btn-xs btn-inverse',
                    placement: {
                        from: 'top',
                        align: 'right'
                    },
                    delay: 2500,
                    animate: {
                        enter: 'animated fadeIn',
                        exit: 'animated fadeOut'
                    },
                    offset: {
                        x: 20,
                        y: 85
                    }
                });
            };

            



            //valores das centrais
            //var centraisvalues = "";
            //$("#cmbCentral").chosen().change(function (e, params) {
            //    centraisvalues = $("#cmbCentral").chosen().val();
            //    //values = $('.result-selected').html();
                
            //    //values is an array containing all the results.
            //    //alert(values);
            //    $("#hidden_area").val(centraisvalues);
            //    //ExecutarPostBack();
            //    //CarregaGrafico();

            //    ExecutarPostBack('', function () {
            //        //CarregaGrafico();
            //        //alert('teste');
            //    });
            //});



        });

     <%--   function ExecutarPostBack(value, callback) {
            // __doPostBack('btnPostBack', '');
            __doPostBack('<%= upMain.ClientID %>', '');
            callback();
            
        }--%>


        //grafico
         $(function () {
             //alert('teste');
             //CarregaGrafico();
 
             //$("#MainContent_cmbCentral").chosen();

             
      
         });

        
         function Bemvindo() {
             //alert("teste");
           if (!$('.login-content')[0]) {
                notify('Bem vindo <%=nomeusuario%>', 'inverse');
            }
         }

        function CarregaVencimento(vencimento) {
            //alert('teste');
            //alert(vencimento);
            //$("#txtVencimento").val(vencimento);
            angular.element(document.getElementById('myApp')).scope().vencimento = vencimento;

            //alert(angular.element(document.getElementById('myApp')).scope().vencimento);
            angular.element(document.getElementById('myApp')).scope().atualizaInfos();

            setTimeout(function () {

                //alert(angular.element(document.getElementById('myApp')).scope().totalBox);

            }, 500);
        }


        function AjustaClasseBox()
        {
            //angular.element(document.getElementById('myApp')).scope().boxClass = 'col-sm-3';

            
            if (angular.element(document.getElementById('myApp')).scope().totalBox == 4) {
                angular.element(document.getElementById('myApp')).scope().boxClass = 'col-sm-3';
            }


            if(angular.element(document.getElementById('myApp')).scope().totalBox==3)
            {
                //alert(angular.element(document.getElementById('myApp')).scope().totalBox);
                angular.element(document.getElementById('myApp')).scope().boxClass = 'col-sm-4';
            }
            if (angular.element(document.getElementById('myApp')).scope().totalBox == 2) {
                angular.element(document.getElementById('myApp')).scope().boxClass = 'col-sm-6';
            }

            //setTimeout(function () {

            //    alert(angular.element(document.getElementById('myApp')).scope().totalBox);

            //}, 500);
        }


    </script>

    <asp:SqlDataSource ID="SqlDataSourceResumoGeral" runat="server" ProviderName="<%$ ConnectionStrings:ConnectionString.ProviderName %>"
    SelectCommand=""></asp:SqlDataSource>

   

</asp:Content>

<asp:Content ID="contentFoorter" ContentPlaceHolderID="FooterPlaceHolder1" runat="Server">


    <!-- Charts - Please read the read-me.txt inside the js folder-->        
      
        <script src="js/flot-charts/pie-chart.js"></script>

     <!-- ANGULAR Libraries -->
        
   <%-- <script src="vendors/bower_components/chosen_angular/chosen.jquery.js" async=""></script>       --%>
        <script src="Scripts/angular.min.js"></script>
   <%--  <script src="vendors/bower_components/angular-chosen-localytics/chosen.js" async=""></script>--%>
        
        <script src="Scripts/angular/app.js"></script>
        <script src="Scripts/angular/SGPCctrljs.js"></script>
        

    <script>

        window.onload = function () {
            //alert('teste');
            //CarregaGrafico();

            //$("#MainContent_cmbCentral").chosen();           

            setTimeout(function () {
                var teste = $("#txtTeste").val();
               //alert(teste);
                angular.element(document.getElementById('myApp')).scope().getAreas();              
                setTimeout(function () {
                    // alert('teste2');
                    //angular.element(document.getElementById('myApp')).scope().area = angular.element(document.getElementById('myApp')).scope().areas[2];
                    if (angular.element(document.getElementById('myApp')).scope().areas.length==1)
                    {
                        $("#cmbCentral")[0].selectedIndex = 1;
                    }
                   
                      
                        $("#cmbCentral").chosen();
                   
                    
                }, 500);               
                angular.element(document.getElementById('myApp')).scope().GetAreasInternas();
                setTimeout(function () {
                    // alert('teste2');
                    // $("#cmbAreaInternas").chosen();
                    if (angular.element(document.getElementById('myApp')).scope().areasInternas.length == 1) {
                       // $("#cmbAreaInterna")[0].selectedIndex = 1;
                    }
                    

                }, 500);
                //angular.element(document.getElementById('myApp')).scope().getTopMovel();
                //angular.element(document.getElementById('myApp')).scope().getTopFixo();

                

            }, 500);

            

           
            
        };

        function CarregaMain(tipoGrafico,tipoFatura,tipoVisao, area, areaInterna, grupo)
        {
            //divMainGrafico
            //alert(areaInterna);
            $("#divLoader").show();
            //            page - loader
            
           //$(".page-loader").show();
            //$("#divMainGrafico").hide();

            var url = "uc_graficoHome.aspx?tipoGrafico=" + tipoGrafico + "&tipoVisao=" + tipoVisao + "&tipoFatura=" + tipoFatura + "&grupo=" + encodeURIComponent(grupo) + "&area=" + encodeURIComponent(area) + "&area_interna=" + encodeURIComponent(areaInterna);
            //history.pushState({}, '', url);

            $("#divMainGrafico").load(url, function () {
                //alert("Load was performed.");
                $("#divLoader").hide();
                //$(".page-loader").hide();
                $("#divMainGrafico").show();
              
            });

        }

        function aplicaChosen(object)
        {
            //alert(object);
            //$("#cmbAreaInternas").removeClass("chosen");
            //$("#cmbCentral").chosen();

            $("#" + object).show();
            $("#" + object).chosen('destroy');
            $("#" + object).chosen({ search_contains: true });
            //alert(object.options[object.selectedIndex].value);
            
        }

        function selecionaPrimeiroItem(object)
        {
            $("#" + object)[0].selectedIndex = 1;
        }

        function removeChosen(object) {
            $("#" + object).show();
            $("#" + object).chosen('destroy');
                       
           // $("#cmbAreaInternas_chosen").html('');
           // alert("alert1");
            //$("#cmbAreaInternas").removeClass();
            //$("#cmbCentral").chosen();
            //$("#cmbAreaInternas").chosen();
        }

        
    </script>
      <!-- Page Loader -->
        <div class="page-loader" id="divLoader" style="opacity:0.7">
            <div class="preloader pls-blue">
                <svg class="pl-circular" viewBox="25 25 50 50">
                    <circle class="plc-path" cx="50" cy="50" r="20" />
                </svg>

                <p>Processando...</p>
            </div>
        </div>

</asp:Content>
