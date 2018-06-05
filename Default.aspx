﻿<%@ Page Title="Home Page" Language="VB"  AutoEventWireup="true" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>
<html >
    <!--[if IE 9 ]><html class="ie9"><![endif]-->
    <head>
        <meta charset="utf-8">
        <meta http-equiv="X-UA-Compatible" content="IE=edge">
        <meta name="viewport" content="width=device-width, initial-scale=1">
        <title>SGPC</title>
        
        <!-- Vendor CSS -->
        <link href="vendors/bower_components/animate.css/animate.min.css" rel="stylesheet">
        <link href="vendors/bower_components/material-design-iconic-font/dist/css/material-design-iconic-font.min.css" rel="stylesheet">
            
        <!-- CSS -->
        <link href="css/app.min.1.css" rel="stylesheet">
        <link href="css/app.min.2.css" rel="stylesheet">

        <script>


           

        </script>
    </head>
    
    <body  id="myApp"  ng-app="SGPCAPP" class="login-content" ng-controller="loginCtrl" >
     <%=_googleAnalytics%>

    <center>

        <img class="i-logo" src="img/logo-login.png" alt="" style="z-index: 99999999999;position: relative;margin-top: 2%; width:400px;">
    </center>
        <!-- Login -->

        <div class="lc-block toggled" id="l-login" style="margin-top:-30%">

            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-account"></i></span>
                <div class="fg-line">
                    <input type="text" class="form-control" placeholder="login" ng-model="username">
                   
                </div>
            </div>
            
            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-lock"></i></span>
                <div class="fg-line">
                    <input type="password" class="form-control" placeholder="Senha" ng-model="password">
                </div>
            </div>
             <p class="alert alert-danger"  ng-show="showErro" >Usuário inválido!</p>
             <p class="alert alert-info" ng-show="showSuccess">Carregando informações ....</p>
            <p class="alert alert-warning" ng-show="showConect">Conectando ....</p>
            
            <div class="clearfix"></div>
            
            <div class="checkbox" style="display:none">
                <label>
                    <input type="checkbox" value="">
                    <i class="input-helper"></i>
                    Mantenha-me conectado
                </label>
            </div>
            
            <a href="#" class="btn btn-login btn-warning btn-float"  ng-click="VerificaLogin()" ><i class="zmdi zmdi-arrow-forward"></i></a>
            
            <ul class="login-navigation" style="display:none;">
                <li data-block="#l-register" class="bgm-red">Register</li>
                <li data-block="#l-forget-password" class="bgm-orange">Forgot Password?</li>
            </ul>

        </div>
 
        
        <!-- Register -->
        <div class="lc-block" id="l-register">
            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-account"></i></span>
                <div class="fg-line">
                    <input type="text" class="form-control" placeholder="Username">
                </div>
            </div>
            
            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-email"></i></span>
                <div class="fg-line">
                    <input type="text" class="form-control" placeholder="Email Address">
                </div>
            </div>
            
            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-male"></i></span>
                <div class="fg-line">
                    <input type="password" class="form-control" placeholder="Password">
                </div>
            </div>
            
            <div class="clearfix"></div>
            
            <div class="checkbox">
                <label>
                    <input type="checkbox" value="">
                    <i class="input-helper"></i>
                    Accept the license agreement
                </label>
            </div>
            
            <a href="" class="btn btn-login btn-danger btn-float"><i class="zmdi zmdi-arrow-forward"></i></a>
            
            <ul class="login-navigation" >
                <li data-block="#l-login" class="bgm-green">Login</li>
                <li data-block="#l-forget-password" class="bgm-orange">Forgot Password?</li>
            </ul>
        </div>
        
        <!-- Forgot Password -->
        <div class="lc-block" id="l-forget-password">
            <p class="text-left">Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nulla eu risus. Curabitur commodo lorem fringilla enim feugiat commodo sed ac lacus.</p>
            
            <div class="input-group m-b-20">
                <span class="input-group-addon"><i class="zmdi zmdi-email"></i></span>
                <div class="fg-line">
                    <input type="text" class="form-control" placeholder="Email Address">
                </div>
            </div>
            
            <a href="" class="btn btn-login btn-danger btn-float"><i class="zmdi zmdi-arrow-forward"></i></a>
            
            <ul class="login-navigation">
                <li data-block="#l-login" class="bgm-green">Login</li>
                <li data-block="#l-register" class="bgm-red">Register</li>
            </ul>
        </div>
        
        <!-- Older IE warning message -->
        <!--[if lt IE 9]>
            <div class="ie-warning">
                <h1 class="c-white">Warning!!</h1>
                <p>You are using an outdated version of Internet Explorer, please upgrade <br/>to any of the following web browsers to access this website.</p>
                <div class="iew-container">
                    <ul class="iew-download">
                        <li>
                            <a href="http://www.google.com/chrome/">
                                <img src="img/browsers/chrome.png" alt="">
                                <div>Chrome</div>
                            </a>
                        </li>
                        <li>
                            <a href="https://www.mozilla.org/en-US/firefox/new/">
                                <img src="img/browsers/firefox.png" alt="">
                                <div>Firefox</div>
                            </a>
                        </li>
                        <li>
                            <a href="http://www.opera.com">
                                <img src="img/browsers/opera.png" alt="">
                                <div>Opera</div>
                            </a>
                        </li>
                        <li>
                            <a href="https://www.apple.com/safari/">
                                <img src="img/browsers/safari.png" alt="">
                                <div>Safari</div>
                            </a>
                        </li>
                        <li>
                            <a href="http://windows.microsoft.com/en-us/internet-explorer/download-ie">
                                <img src="img/browsers/ie.png" alt="">
                                <div>IE (New)</div>
                            </a>
                        </li>
                    </ul>
                </div>
                <p>Sorry for the inconvenience!</p>
            </div>   
        <![endif]-->
        
        <!-- Javascript Libraries -->
        <script src="vendors/bower_components/jquery/dist/jquery.min.js"></script>
        <script src="vendors/bower_components/bootstrap/dist/js/bootstrap.min.js"></script>
        
        <script src="vendors/bower_components/Waves/dist/waves.min.js"></script>
        
        <!-- Placeholder for IE9 -->
        <!--[if IE 9 ]>
            <script src="vendors/bower_components/jquery-placeholder/jquery.placeholder.min.js"></script>
        <![endif]-->
        
        
        <script src="js/functions.js"></script>

        <!-- ANGULAR Libraries -->
        <%--<script src="Scripts/angular.min.js"></script>--%>
        <script src="Scripts/angular.min.js"></script>
        <script src="Scripts/angular/app.js"></script>
        <script src="Scripts/angular/SGPCctrljs.js"></script>

      <%--  <script src="Scripts/angular.min.js"></script>
        <script src="Scripts/angular-resource.min.js"></script>
        
        --%>

        <script>

            function autoLogon() {
                //alert("teste");

                setTimeout(function () {
                    // alert('teste2');
                    // $("#cmbAreaInternas").chosen();
                    angular.element(document.getElementById('myApp')).scope().username = '<%=loginAD%>';
                    angular.element(document.getElementById('myApp')).scope().password = '<%=senhaAD%>';
                    angular.element(document.getElementById('myApp')).scope().AD = true;
                   // alert(angular.element(document.getElementById('myApp')).scope().username);
                    angular.element(document.getElementById('myApp')).scope().VerificaLogin();

                }, 500);

               
            }
            window.onload = function () {
                //angular.element(document.getElementById('myApp')).scope().username = 'teste';
                //alert(angular.element(document.getElementById('myApp')).scope().username);
            }

            $(document).keypress(function (e) {
                if (e.which == 13) {
                    //alert('You pressed enter!');
                    angular.element(document.getElementById('myApp')).scope().VerificaLogin();
                }
            });

        </script>

        <form id="form1" runat="server" style="display:none"></form>

    </body>
</html>