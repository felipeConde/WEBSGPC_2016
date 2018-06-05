﻿(function () {
    'use strict';

    angular
       .module('SGPCAPP')   


    //Service
    //service    
    .service('UserService', function () {
        this.loginUsuario = '';

        this.login = {
            get value() {
                return this.loginUsuario;
            },
            set value(login) {
                this.loginUsuario = login;
            }
        };
        
    })
   
    .value('user', {
        id: 'testeid',
        login:'testeLogin'
    })

    //=================================================
    // LOGIN
    //=================================================
    
    .controller('loginCtrl', function ($scope, $http, $rootScope, $location, $window, UserService, user) {
        $scope.AD = false;
        //$scope.teste = "teste";

        $scope.VerificaLogin = function (username, password) {
            //alert($scope.username);
            //this.showErro = false;
            $scope.showConect = true;
            if (this.username == null || this.password == null) {
                $scope.showErro = true;
                $scope.showConect=false;
                return;
            }
            

            $http.post('api/Login/PostValue', { Login_Usuario: this.username, Senha_Usuario: this.password, AD: $scope.AD })
            .then(function successCallback(response) {
                // this callback will be called asynchronously
                // when the response is available
                //alert(response.data.Email);
                //this.showSuccess = true;
                $scope.showSuccess = true;
                $scope.showErro = false;
                $scope.showConect = false;
                SetCredentials(response.data.Login_Usuario, response.data.Senha_Usuario, response.data.codigo, response.data.nome_usuario)
                var teste;
                //alert("passou");
               // $rootScope.globals = $cookieStore.get('globals') || {};
                if ($rootScope.globals.currentUser) {
                    //alert($rootScope.globals.currentUser.codigo);
                    $http.defaults.headers.common['Authorization'] = 'Basic ' + $rootScope.globals.currentUser.authdata; // jshint ignore:line
                    //teste = $rootScope.globals.currentUser.username;
                    //$scope.Email = response.data.Email;
                    //$scope.Email = teste;

                    //SESSIONS DO USUÁRIO
                    $window.sessionStorage.setItem("codigo_usuario", $rootScope.globals.currentUser.codigo)
                    $window.sessionStorage.setItem("nome_usuario", $rootScope.globals.currentUser.nome_usuario)

                    //$scope.Email = $window.sessionStorage.getItem("codigo_usuario")
                    //redirect
                   
                    //$rootScope.name = 'teste';
                    //alert($rootScope.name);
                   
                    $http.post('user.ashx', $rootScope.globals.currentUser.codigo.toString())
                    .then(function successCallback(response) {
                        //alert(response);
                        //user.id = '123456';
                        //user.login = '99999';
                        $window.location.href = 'main.aspx';
                    }, function errorCallback(response) {
                        //alert(response.data);
                    });

                }

            }, function errorCallback(response) {
                // called asynchronously if an error occurs
                // or server returns response with an error status.
                //alert(this.showErro);
                //this.showErro = true;
                //alert(response);
                $scope.showErro = true;
                $scope.showSuccess = false;
                $scope.showConect = false;
                // alert("erro");
            });
            //.success(function (response) {
            //    //alert(response.username)
            //    callback(response);
            //});



        };

        function SetCredentials(username, password, codigo, nome_usuario) {
            var authdata = Base64.encode(username + ':' + password);

            $rootScope.globals = {
                currentUser: {
                    username: username,
                    authdata: authdata,
                    codigo: codigo,
                    nome_usuario: nome_usuario
                }
            };

            //$sessionStorage.globals = $rootScope.globals;

            $http.defaults.headers.common['Authorization'] = 'Basic ' + authdata; // jshint ignore:line
            //$cookie.put('globals', $rootScope.globals);

            //alert($rootScope.globals.currentUser.codigo);
        }

        function ClearCredentials() {
            $rootScope.globals = {};
            //$cookie.remove('globals');
            $http.defaults.headers.common.Authorization = 'Basic';
        }

        // Base64 encoding service used by AuthenticationService
        var Base64 = {

            keyStr: 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=',

            encode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                do {
                    chr1 = input.charCodeAt(i++);
                    chr2 = input.charCodeAt(i++);
                    chr3 = input.charCodeAt(i++);

                    enc1 = chr1 >> 2;
                    enc2 = ((chr1 & 3) << 4) | (chr2 >> 4);
                    enc3 = ((chr2 & 15) << 2) | (chr3 >> 6);
                    enc4 = chr3 & 63;

                    if (isNaN(chr2)) {
                        enc3 = enc4 = 64;
                    } else if (isNaN(chr3)) {
                        enc4 = 64;
                    }

                    output = output +
                        this.keyStr.charAt(enc1) +
                        this.keyStr.charAt(enc2) +
                        this.keyStr.charAt(enc3) +
                        this.keyStr.charAt(enc4);
                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";
                } while (i < input.length);

                return output;
            },

            decode: function (input) {
                var output = "";
                var chr1, chr2, chr3 = "";
                var enc1, enc2, enc3, enc4 = "";
                var i = 0;

                // remove all characters that are not A-Z, a-z, 0-9, +, /, or =
                var base64test = /[^A-Za-z0-9\+\/\=]/g;
                if (base64test.exec(input)) {
                    window.alert("There were invalid base64 characters in the input text.\n" +
                        "Valid base64 characters are A-Z, a-z, 0-9, '+', '/',and '='\n" +
                        "Expect errors in decoding.");
                }
                input = input.replace(/[^A-Za-z0-9\+\/\=]/g, "");

                do {
                    enc1 = this.keyStr.indexOf(input.charAt(i++));
                    enc2 = this.keyStr.indexOf(input.charAt(i++));
                    enc3 = this.keyStr.indexOf(input.charAt(i++));
                    enc4 = this.keyStr.indexOf(input.charAt(i++));

                    chr1 = (enc1 << 2) | (enc2 >> 4);
                    chr2 = ((enc2 & 15) << 4) | (enc3 >> 2);
                    chr3 = ((enc3 & 3) << 6) | enc4;

                    output = output + String.fromCharCode(chr1);

                    if (enc3 != 64) {
                        output = output + String.fromCharCode(chr2);
                    }
                    if (enc4 != 64) {
                        output = output + String.fromCharCode(chr3);
                    }

                    chr1 = chr2 = chr3 = "";
                    enc1 = enc2 = enc3 = enc4 = "";

                } while (i < input.length);

                return output;
            }
        };
    
    })

    //=================================================
    // HOME DASHBOARD
    //=================================================
    .controller('homeCtrl', function ($scope, $http, $rootScope, $location, $window) {

        //$scope
        //$scope.top10Movel = [
        //{ Descricao: 'Jani', Valor: 500 },
        //{ Descricao: 'Hege', Valor: 800 },
        //{ Descricao: 'Kai', Valor: 200 }
        //];
        
        $scope.totalBox = 0;
        $scope.boxClass = "col-sm-3";
        

        

        function getLabelArea(campo) {
            //alert($scope.vencimento);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/getLabel?campo=NOME_CENTRAL")
                .then(function (response) {
                   //alert(response.data);
                   $scope.nomeArea= response.data;   
                });

            $http.get("api/Home/getLabel?campo=NOME_AREA_INTERNA")
               .then(function (response) {
                   //alert(response.data);
                   $scope.nomeAreaInterna = response.data;
               });

            $http.get("api/Home/getLabel?campo=NOME_CCUSTO")
              .then(function (response) {
                  //alert(response.data);
                  $scope.nomeCcusto = response.data;
              });
        }

        function getLabelAreaInterna(campo) {
            //alert($scope.vencimento);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/getLabel?campo=" + campo)
                .then(function (response) {
                    //alert(response.data);
                    $scope.nomeAreaInterna = response.data;
                });
        }

        function ExibeRamal() {
            //alert($scope.vencimento);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/ExibeTarifacao")
                .then(function (response) {
                    
                    if(response.data)
                    {
                        //alert(response.data);
                        $scope.showtop10Fixo = false;
                        $scope.showtop10Ramal = true;
                        $scope.showServicosMes = false;
                        $("#btGraficoFixo").hide();
                        //alert($scope.totalBox);
                        if ($scope.totalBox == 4)
                        {
                            $scope.totalBox = '3';
                        }
                        
                        AjustaClasseBox();
                    }
                    else
                    {
                        //alert(response.data);
                        $("#btTarifacao").hide();
                        $scope.showtop10Ramal = false;
                        $scope.showServicosMes = true;
                        AjustaClasseBox();
                       

                    }
                });
        }
        

        $scope.getTopMovel = function () {
            //alert($scope.vencimento);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/GetTop10Movel?vencimento=" + $scope.vencimento + "&grupo=" + $scope.grupo + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna + "&codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    $scope.top10Movel = response.data;
                    //alert($scope.top10Movel);
                    if ($scope.top10Movel.length < 1) {
                        $scope.showtop10Movel = false;
                        $("#btGraficoMovel").hide();                       
                        //$("#btGraficoFixo").text("DETALHADO");
                        //AjustaClasseBox();
                    }
                    else {
                        $scope.showtop10Movel = true;
                        $scope.totalBox += 1;
                        $("#btGraficoMovel").show();
                        AjustaClasseBox();
                    }
                });

        }

        $scope.getTopFixo = function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
            $scope.showtop10Fixo = false;
            $http.get("api/Home/GetTop10Fixo?vencimento=" + $scope.vencimento + "&grupo=" + $scope.grupo + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna + "&codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    $scope.top10Fixo = response.data;
                    //alert($scope.top10Fixo.length);
                    if ($scope.top10Fixo.length < 1)
                    {
                        $scope.showtop10Fixo = false;
                        $("#btGraficoFixo").hide();
                        //$("#btGraficoMovel").text("DETALHADO");
                        
                        //AjustaClasseBox();
                       
                    }
                    else
                    {
                        $scope.showtop10Fixo = true;
                        $scope.totalBox += 1;
                        AjustaClasseBox();
                        $("#btGraficoFixo").show();
                        
                    }
                    //alert(angular.element(document.getElementById('myApp')).scope().showtop10Fixo);
                });

        }

        //top 10ramais
        $scope.getTopRamal = function () {
            //alert($scope.vencimento);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/GetTop10Ramais?vencimento=" + $scope.vencimento + "&grupo=" + $scope.grupo + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna + "&codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    $scope.TopRamal = response.data;
                    //alert($scope.top10Movel);
                    if ($scope.TopRamal.length < 1) {
                        //$scope.showtop10Ramal = false;
                        $("#btTarifacao").hide();                       
                        //AjustaClasseBox();
                    }
                    else {
                        //$scope.showtop10Ramal = true;
                        $("#btTarifacao").show();
                        //$scope.totalBox += 1;
                        //$("#btTarifacao").show();
                        AjustaClasseBox();
                        ExibeRamal();
                    }
                });

        }

        //GetServicosMes
        $scope.GetServicosMes = function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/GetServicosMes?vencimento=" + $scope.vencimento + "&grupo=" + $scope.grupo + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna + "&codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    $scope.ServicosMes = response.data;
                    //alert($scope.top10Fixo.length);
                    if ($scope.ServicosMes.length < 1) {
                        $scope.showServicosMes = false;
                        AjustaClasseBox();
                    }
                    else {
                        $scope.showServicosMes = true;
                        $scope.totalBox += 1;
                        AjustaClasseBox();
                        //alert('passou');
                        ExibeRamal();
                    }
                });

        }

        //Perfil
        $scope.GetLinhasPerfil = function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/getUsuariosPerfil?vencimento=" + $scope.vencimento + "&grupo=" + $scope.grupo + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna + "&codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    $scope.LinhasPerfil = response.data;
                    //alert($scope.LinhasPerfil.length);
                    if ($scope.LinhasPerfil.length < 1) {
                        $scope.showLinhasPerfil = false;
                         AjustaClasseBox();
                    }
                    else {
                        $scope.showLinhasPerfil = true;
                        $scope.totalBox += 1;
                        AjustaClasseBox();
                    }

                });

        }

       

        $scope.getAreas = function () {
            //alert($scope.codigousuario);
            //$http.get("api/Home/GetTop10Movel")
            $http.get("api/Home/GetAreas?codigousuario=" + $scope.codigousuario)
                .then(function (response) {
                    //alert(response.data);
                    $scope.areas = response.data;
                    
                    //$scope.class = "chosen";

                    setTimeout(function () {
                        //$("#cmbAreaInterna")[0].selectedIndex = 1;
                        //alert($scope.areasInternas[0].Codigo);
                        //$scope.areaInterna = $scope.areasInternas[0].Codigo;                        
                        aplicaChosen('cmbCentral');
                        
                    }, 500);
                });

        }

        $scope.GetAreasInternas = function () {
            //alert($scope.area);
            //$http.get("api/Home/GetTop10Movel")
            $scope.strArea = $scope.area;
            $http.get("api/Home/GetAreasInternas?codigousuario=" + $scope.codigousuario + "&area=" + $scope.area)
                .then(function (response) {
                    //alert(response.data);
                    //removeChosen();
                    $scope.areasInternas = response.data;
                    setTimeout(function () {
                        //$("#cmbAreaInterna")[0].selectedIndex = 1;
                        //alert($scope.areasInternas[0].Codigo);
                        //$scope.areaInterna = $scope.areasInternas[0].Codigo;
                        if ($scope.areasInternas.length == 1) {
                            selecionaPrimeiroItem('cmbAreaInternas');
                        }
                        aplicaChosen('cmbAreaInternas');
                        $scope.GetGrupos();
                    }, 500);
                    
                    //$scope.class = "chosen";
                });

        }

        $scope.GetGrupos = function () {
            //alert($scope.areaInterna);
            //$http.get("api/Home/GetTop10Movel")
            $scope.grupo = '';
            $scope.strareaInterna = $scope.areaInterna;
            $http.get("api/Home/GetGrupos?codigousuario=" + $scope.codigousuario + "&area=" + $scope.area + "&areaInterna=" + $scope.areaInterna)
                .then(function (response) {
                    //alert(response.data);
                    //removeChosen();
                    $scope.grupos = response.data;
                    setTimeout(function () {
                        if ($scope.grupos.length == 1) {
                            selecionaPrimeiroItem('cmbGrupos');
                        }
                        aplicaChosen('cmbGrupos');
                        $scope.carregaGraficos();   
                        //$scope.atualizaInfos();
                    }, 10);

                    //$scope.class = "chosen";
                });

        }

        $scope.carregaGraficos = function () {           
            
            CarregaMain(1,1,'Tipo',$scope.area, $scope.areaInterna, $scope.grupo);
            $scope.tipoVisao = 'Tipo';
            //$scope.atualizaInfos();

        }

        $scope.atualizaInfos = function () {
            //alert($scope.grupo);
            $scope.totalBox = 0;           
            $scope.getTopMovel();
            $scope.getTopFixo();
            $scope.GetServicosMes();            
            $scope.GetLinhasPerfil();
            $scope.getTopRamal();
            ExibeRamal();


            //PEGAMOS OS LABELS
             
            getLabelArea("NOME_CENTRAL");
            //NOME_AREA_INTERNA
            //getLabelAreaInterna("NOME_AREA_INTERNA");

            //AjustaClasseBox();
            $scope.mesStr = getMes($scope.vencimento.substring(0, 2));

              setTimeout(function () {
                        //$("#cmbAreaInterna")[0].selectedIndex = 1;
                        //alert($scope.areasInternas[0].Codigo);
                  //$scope.areaInterna = $scope.areasInternas[0].Codigo;
                  //alert('teste');
                  AjustaClasseBox();
                  //alert('teste');
             }, 3000);

            //alert($scope.mesStr);
            //CarregaMain($scope.area, $scope.areaInterna, $scope.grupo);   
             //AjustaClasseBox();
             
        }

       
       


        $scope.GetVencimentoFaturas = function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
            $scope.listVencimentosIndex = 1;
            $http.get("api/GastoUsuario/getvencimentosfaturas?codigousuario=" + $scope.codigousuario)
               .then(function (response) {
                   //alert(response.data);
                   $scope.listVencimentos = response.data;
                   setTimeout(function () {
                       aplicaChosen('cmbVencimento');
                       setTimeout(function () {
                           //setIndexChosen('cmbVencimento', 2);
                           //alert($scope.listVencimentos[0].Descricao);
                           //alert($scope.vencimento);
                           $scope.dtvencimento = $scope.vencimento;
                           //$scope.getResumoGastoUsuario();
                       }, 500);
                   }, 500);

                   //alert($scope.top10Fixo.length);                
               });

        }

        $scope.AvancaVencimento = function () {

            var yearStr= $scope.vencimento.substring(2, 6);
            var monthStr = $scope.vencimento.substring(0, 2);
            var d = new Date(yearStr, monthStr, "01");
            d.setMonth(d.getMonth() + 1);
            var proximaData = (d.getMonth().toString() >= 10 ? d.getMonth().toString() : "0" + d.getMonth().toString()) + d.getFullYear().toString();
            //alert(proximaData);
            $scope.vencimento = proximaData;
            $scope.atualizaInfos();
            $scope.carregaGraficos();

            //alert(proximaData);
            //d.setMonth(d.getMonth() + 8);

            //alert($scope.vencimento);
            //CarregaVencimento($scope.dtvencimento.Descricao.replace('/', ''));
        }

        $scope.MudaVencimento = function () {

             CarregaVencimento($scope.dtvencimento.Descricao.replace('/', ''));
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
        
    })

    //=================================================
    // MINHA CONTA CONTROLLER
    //=================================================
    .controller('usuarioCtrl', function ($scope, $http, $rootScope, $location, $window, UserService, user) {
        //PEGA AS INFOS DO PERFIL DO USUARIO
        
        $scope.GetUsuario = function () {
            if ($scope.mostraArea == "S")
            {
                $scope.mostraGastos = false;
                $scope.getUsuarios();
                $scope.mostraBusca = true;
            }
            else
            {
                $scope.mostraGastos = true;
                $scope.mostraBusca = false;
                
            }
           
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
         

        }

         $scope.GetVencimentoFaturas = function () {
            //alert($scope.username);
             //$http.get("api/Home/GetTop10Movel")
             $scope.listVencimentosIndex = 0;
             $http.get(encodeURI("api/GastoUsuario/getvencimentosfaturas?codigousuario=" + $scope.codigousuario))
                .then(function (response) {
                    //alert(response.data);
                    $scope.listVencimentos = response.data;
                    setTimeout(function () {                       
                        aplicaChosen('cmbVencimento');
                        setTimeout(function () {
                            //setIndexChosen('cmbVencimento', 2);
                            //alert($scope.listVencimentos[0].Descricao);
                            $scope.dtvencimento = $scope.listVencimentos[$scope.listVencimentosIndex];
                            $scope.getResumoGastoUsuario();
                        }, 500);
                    }, 500);
                    
                    //alert($scope.top10Fixo.length);                
                });

         }

         $scope.getResumoGastoUsuario = function () {

             
             //alert($scope.SessionUsuario);
             
             $http.get(encodeURI("api/GastoUsuario/GetInfoUsuario?codigousuario=" + $scope.codigousuario), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
             .then(function (response) {
                 $scope.usuario = response.data;
                 //alert($scope.top10Fixo.length);                
             });


            removeChosen('cmbVencimento');
           //alert($scope.dtvencimento.Descricao);
             //$http.get("api/Home/GetTop10Movel")
            $scope.mostraGastos = true;
             $http.get(encodeURI("api/GastoUsuario/getResumoGastoUsuario?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                 .then(function (response) {
                     //alert(response.data);
                     $scope.resumo = response.data;
                     aplicaChosen('cmbVencimento');
                     
                     //$scope.usuario = response.data;
                     //if ($scope.resumo.length == 0 && $scope.listVencimentosIndex < $scope.listVencimentos.length-1)
                     //{
                     //    // se não tem informações tenta o mês anterior
                     //    $scope.listVencimentosIndex += 1;
                     //    $scope.dtvencimento = $scope.listVencimentos[$scope.listVencimentosIndex]
                     //    //alert($scope.dtvencimento.Descricao);
                     //    $scope.getResumoGastoUsuario();

                     //    //se chegou no ultimo volta para o primeiro
                     //    if($scope.listVencimentosIndex== $scope.listVencimentos.length-1)
                     //    {
                     //        $scope.dtvencimento = $scope.listVencimentos[0];
                             
                     //    }
                     //}

                     if ($scope.resumo.length == 0)
                     {
                         // EscondeGrid();
                         $scope.SemGastos = true;
                     }
                     else
                     {
                         $scope.SemGastos = false;
                     }

                     //$scope.codigousuario
                   
                    
                     $scope.totalParcela = 0;
                     $scope.totalGasto = 0;
                     angular.forEach($scope.resumo, function (value, key) {
                         //alert(value.PARCELAMENTO);
                         $scope.totalParcela = parseFloat($scope.totalParcela) + parseFloat(value.PARCELAMENTO);
                         //alert($scope.totalParcela);
                         $scope.totalGasto = parseFloat($scope.totalGasto) + parseFloat(value.PARCELAMENTO) + parseFloat(value.GASTO);
                         //alert(value.totalParcela);
                     });
                    

                     //pega o valor do mes anterior
                     $http.get(encodeURI("api/GastoUsuario/getResumoGastoUsuario?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&intervaloMes=-1"), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                        .then(function (response) {
                            //alert(response.data);
                            $scope.mesanterior = response.data;
                            $scope.totalGastoAnterior = 0;
                            $scope.totalVariacao = 0;
                            angular.forEach($scope.mesanterior, function (value, key) {
                                $scope.totalGastoAnterior = parseFloat($scope.totalGastoAnterior) + parseFloat(value.GASTO) + parseFloat(value.PARCELAMENTO);
                            });
                            //alert($scope.totalGasto);
                            //alert($scope.totalGastoAnterior);
                            $scope.totalVariacao = parseFloat($scope.totalGasto) - parseFloat($scope.totalGastoAnterior);
                            //alert( $scope.totalVariacao);
                        });


                 },
                 function (data) {
                     // Handle error here
                     //alert("erro");
                 }
                 );

             //sera as variaveis
            $scope.totalVariacaoVoz = 0;
            $scope.totalVariacaoDados = 0;
            $scope.totalVariacaoServicos = 0;

             //pega o SERVIÇOS DE voz
             $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=VOZ&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
               .then(function (response) {
                   //alert(response.data.length);
                   //alert($scope.codigousuario);
                   if (response.data.length > 0)
                   {
                       $scope.totalVoz = response.data[0].GASTO;
                   }
                   //alert(response.data[0].GASTO);
                   // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                   //pega o SERVIÇOS DE voz
                   $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=VOZ&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                      .then(function (response) {
                          // alert(response.data[0].GASTO);
                          if (response.data.length > 0) {
                              $scope.totalVozAnterior = response.data[0].GASTO;
                              $scope.totalVariacaoVoz = $scope.totalVoz - $scope.totalVozAnterior;
                          }
                      });

              
               });

             //pega o SERVIÇOS DE DADOS
             $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=DADOS&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
               .then(function (response) {
                   //alert(response.data);
                   if (response.data.length > 0) {
                       $scope.totalDados = response.data[0].GASTO;
                   }
                   //alert(response.data[0].GASTO);
                   // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                   //pega o SERVIÇOS DE voz
                   $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=DADOS&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                      .then(function (response) {
                          // alert(response.data[0].GASTO);
                          if (response.data.length > 0) {
                              $scope.totalDadosAnterior = response.data[0].GASTO;
                              $scope.totalVariacaoDados = $scope.totalDados - $scope.totalDadosAnterior;
                          }

                      });


               });

             //pega o SERVIÇOS DE SERVIÇOS
             $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=SERVIÇOS&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
               .then(function (response) {
                   //alert(response.data);
                   if (response.data.length > 0) {
                       $scope.totalServicos = response.data[0].GASTO;
                   }
                   //alert(response.data[0].GASTO);
                   // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                   //pega o SERVIÇOS DE voz
                   $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=SERVIÇOS&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                      .then(function (response) {
                          // alert(response.data[0].GASTO);
                          if (response.data.length > 0) {
                              $scope.totalServicosAnterior = response.data[0].GASTO;
                              $scope.totalVariacaoServicos = $scope.totalServicos - $scope.totalServicosAnterior;
                          }

                      });


               });

             //getMediaMes
             //pega o A MEDIA DO MES DA AREA
             $http.get(encodeURI("api/GastoUsuario/getMediaMes?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
            .then(function (response) {
                //alert(response.data);
                if (response.data.length > 0) {
                    $scope.mediaMes = response.data[0].GASTO;
                }
            });

             setTimeout(function () {              
                    CarregaGraficos();             
             }, 500);

           

         }

              

    


        //busca de usuarios
         $scope.getUsuarios = function () {
             //alert($scope.username);
             //$http.get("api/Home/GetTop10Movel")             
             $http.get("api/GastoUsuario/getUsuarios?codigousuario=" + $scope.codigousuarioLogado + "&nome=" , { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                .then(function (response) {
                    //alert(response.data);
                    $scope.listUsuarios = response.data;
                    setTimeout(function () {
                        aplicaChosen('cmbUsuarios');
                        setTimeout(function () {
                            //setIndexChosen('cmbVencimento', 2);
                            //alert($scope.listVencimentos[0].Descricao);
                            //$scope.dtvencimento = $scope.listVencimentos[$scope.listVencimentosIndex];
                            //$scope.getResumoGastoUsuario();
                        }, 500);
                    }, 500);

                    //alert($scope.top10Fixo.length);                
                });

         }

         $scope.buscaUsuarios = function(data)
         {
             removeChosen('cmbUsuarios');
             //alert(data);
             //removeChosen('cmbVencimento');
             $scope.codigousuario = $scope.ddlFuncionarios.Valor;
             //removeChosen('cmbVencimento');
             $scope.getResumoGastoUsuario();
             //CarregaGraficos();
             aplicaChosen('cmbUsuarios');
         }

         $scope.getIndexFromValue = function (value) {
             for (var i = 0; i < scope.list.length; i++) {
                 if (scope.list[i].listValue === value)
                     return i;
             }
         };

               

    })


     //=================================================
    // GASTOS RAMAIS CONTROLLER
    //=================================================
    .controller('usuarioRamaisCtrl', function ($scope, $http, $rootScope, $location, $window, UserService, user) {
        //PEGA AS INFOS DO PERFIL DO USUARIO

        $scope.GetUsuario = function () {
            if ($scope.mostraArea == "S") {
                $scope.mostraGastos = false;
                $scope.getUsuarios();
                $scope.mostraBusca = true;
            }
            else {
                $scope.mostraGastos = true;
                $scope.mostraBusca = false;

            }

            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")


        }

        $scope.GetDatas= function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")
            $scope.listVencimentosIndex = 0;
            $http.get(encodeURI("api/GastoUsuario/getDatasRamais?codigousuario=" + $scope.codigousuario))
               .then(function (response) {
                   //alert(response.data);
                   $scope.listVencimentos = response.data;
                   setTimeout(function () {
                       aplicaChosen('cmbVencimento');
                       setTimeout(function () {
                           //setIndexChosen('cmbVencimento', 2);
                           //alert($scope.listVencimentos[0].Descricao);
                           $scope.dtvencimento = $scope.listVencimentos[$scope.listVencimentosIndex];
                           $scope.getResumoGastoUsuario();
                       }, 500);
                   }, 500);

                   //alert($scope.top10Fixo.length);                
               });

        }

        $scope.getResumoGastoUsuario = function () {


            //alert($scope.SessionUsuario);

            $http.get(encodeURI("api/GastoUsuario/GetInfoUsuario?codigousuario=" + $scope.codigousuario), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
            .then(function (response) {
                $scope.usuario = response.data;
                //alert($scope.top10Fixo.length);                
            });


            removeChosen('cmbVencimento');
            //alert($scope.dtvencimento.Descricao);
            //$http.get("api/Home/GetTop10Movel")
            $scope.mostraGastos = true;
            $http.get(encodeURI("api/GastoUsuario/getResumoGastoUsuarioRamal?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                .then(function (response) {
                    //alert(response.data);
                    $scope.resumo = response.data;
                    aplicaChosen('cmbVencimento');

                 
                    if ($scope.resumo.length == 0) {
                        // EscondeGrid();
                        $scope.SemGastos = true;
                    }
                    else {
                        $scope.SemGastos = false;
                    }


                    $scope.totalParcela = 0;
                    $scope.totalGasto = 0;
                    angular.forEach($scope.resumo, function (value, key) {                       
                        
                        //alert($scope.totalParcela);
                        $scope.totalGasto = parseFloat($scope.totalGasto) + parseFloat(value.GASTO);
                        //alert(value.totalParcela);
                    });


                    //pega o valor do mes anterior
                    $http.get(encodeURI("api/GastoUsuario/getResumoGastoUsuarioRamal?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&intervaloMes=-1"), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                       .then(function (response) {
                           //alert(response.data);
                           $scope.mesanterior = response.data;
                           $scope.totalGastoAnterior = 0;
                           $scope.totalVariacao = 0;
                           angular.forEach($scope.mesanterior, function (value, key) {
                               $scope.totalGastoAnterior = parseFloat($scope.totalGastoAnterior) + parseFloat(value.GASTO);
                           });
                           //alert($scope.totalGasto);
                           //alert($scope.totalGastoAnterior);
                           $scope.totalVariacao = parseFloat($scope.totalGasto) - parseFloat($scope.totalGastoAnterior);
                       });


                },
                function (data) {
                    // Handle error here
                    //alert("erro");
                }
                );

            //sera as variaveis
            $scope.totalVariacaoVoz = 0;
            $scope.totalVariacaoDados = 0;
            $scope.totalVariacaoServicos = 0;

            //pega o SERVIÇOS DE voz
            $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=VOZ&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
              .then(function (response) {
                  //alert(response.data.length);
                  if (response.data.length > 0) {
                      $scope.totalVoz = response.data[0].GASTO;
                  }
                  //alert(response.data[0].GASTO);
                  // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                  //pega o SERVIÇOS DE voz
                  $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=VOZ&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                     .then(function (response) {
                         // alert(response.data[0].GASTO);
                         if (response.data.length > 0) {
                             $scope.totalVozAnterior = response.data[0].GASTO;
                             $scope.totalVariacaoVoz = $scope.totalVoz - $scope.totalVozAnterior;
                         }
                     });


              });

            //pega o SERVIÇOS DE DADOS
            $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=DADOS&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
              .then(function (response) {
                  //alert(response.data);
                  if (response.data.length > 0) {
                      $scope.totalDados = response.data[0].GASTO;
                  }
                  //alert(response.data[0].GASTO);
                  // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                  //pega o SERVIÇOS DE voz
                  $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=DADOS&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                     .then(function (response) {
                         // alert(response.data[0].GASTO);
                         if (response.data.length > 0) {
                             $scope.totalDadosAnterior = response.data[0].GASTO;
                             $scope.totalVariacaoDados = $scope.totalDados - $scope.totalDadosAnterior;
                         }

                     });


              });

            //pega o SERVIÇOS DE SERVIÇOS
            $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=SERVIÇOS&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
              .then(function (response) {
                  //alert(response.data);
                  if (response.data.length > 0) {
                      $scope.totalServicos = response.data[0].GASTO;
                  }
                  //alert(response.data[0].GASTO);
                  // VOZ DO MES ANTERIOR P/ PEGAR VARIAÇÃO
                  //pega o SERVIÇOS DE voz
                  $http.get(encodeURI("api/GastoUsuario/getGastoServico?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=SERVIÇOS&intervaloMes=-1&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
                     .then(function (response) {
                         // alert(response.data[0].GASTO);
                         if (response.data.length > 0) {
                             $scope.totalServicosAnterior = response.data[0].GASTO;
                             $scope.totalVariacaoServicos = $scope.totalServicos - $scope.totalServicosAnterior;
                         }

                     });


              });

            //getMediaMes
            //pega o A MEDIA DO MES DA AREA
            $http.get(encodeURI("api/GastoUsuario/getMediaMes?codigousuario=" + $scope.codigousuario + "&vencimento=" + $scope.dtvencimento.Valor + "&tarifa=&intervaloMes=0&usuariocomum=" + $scope.usuariocomum), { headers: { 'SessionUsuario': $scope.SessionUsuario } })
           .then(function (response) {
               //alert(response.data);
               if (response.data.length > 0) {
                   $scope.mediaMes = response.data[0].GASTO;
               }
           });

            setTimeout(function () {
                CarregaGraficos();
            }, 500);



        }

        
        //busca de usuarios
        $scope.getUsuarios = function () {
            //alert($scope.username);
            //$http.get("api/Home/GetTop10Movel")             
            $http.get("api/GastoUsuario/getUsuarios?codigousuario=" + $scope.codigousuarioLogado, { headers: { 'SessionUsuario': $scope.SessionUsuario } })
               .then(function (response) {
                   //alert(response.data);
                   $scope.listUsuarios = response.data;
                   setTimeout(function () {
                       aplicaChosen('cmbUsuarios');
                       setTimeout(function () {
                           //setIndexChosen('cmbVencimento', 2);
                           //alert($scope.listVencimentos[0].Descricao);
                           //$scope.dtvencimento = $scope.listVencimentos[$scope.listVencimentosIndex];
                           //$scope.getResumoGastoUsuario();
                       }, 500);
                   }, 500);

                   //alert($scope.top10Fixo.length);                
               });

        }

        $scope.buscaUsuarios = function () {
            removeChosen('cmbUsuarios');
            //             alert($scope.ddlFuncionarios.Valor);
            //removeChosen('cmbVencimento');
            $scope.codigousuario = $scope.ddlFuncionarios.Valor;
            //removeChosen('cmbVencimento');
            $scope.getResumoGastoUsuario();
            //CarregaGraficos();
            aplicaChosen('cmbUsuarios');
        }

        $scope.getIndexFromValue = function (value) {
            for (var i = 0; i < scope.list.length; i++) {
                if (scope.list[i].listValue === value)
                    return i;
            }
        };



    })

})();
