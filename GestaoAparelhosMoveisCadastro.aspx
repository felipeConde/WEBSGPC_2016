<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoAparelhosMoveisCadastro.aspx.vb"
    EnableEventValidation="false" Inherits="GestaoAparelhosMoveisCadastro" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Cadastro Móveis</title>
    <link href="../CSS/CL.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript" src="../js/CL.js"></script>
    <link href="../CSS/Tabs.css" type="text/css" rel="Stylesheet" />
    <link href="../js/bootstrap-3.3.5-dist/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
    <link href="../css/jquery.autocomplete.css" type="text/css" rel="stylesheet" />
    <link type="text/css" href="../css/smoothness/jquery-ui-1.8.21.custom.css" rel="Stylesheet" />
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/jquery-ui-1.8.1.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/ui.jqgrid.css" />
    <link rel="stylesheet" href="../js/intl-tel-input/build/css/intlTelInput.css" />
    <link rel="stylesheet" href="../js/Lobibox/lobibox.css" />
    <style type="text/css">
        #icheckForm .radio label, #icheckForm .checkbox label {
            padding-left: 0;
        }

        .style1 {
            height: 230px;
        }

        * {
            box-sizing: border-box;
            border: 0px none;
        }

        .file-upload {
            display: inline-block;
            margin-top: 5px;
            font-size: 12px;
            font-weight: 400;
            line-height: 1.42857143;
            text-align: center;
            white-space: nowrap;
            vertical-align: middle;
            -ms-touch-action: manipulation;
            touch-action: manipulation;
            cursor: pointer;
            -webkit-user-select: none;
            -moz-user-select: none;
            -ms-user-select: none;
            user-select: none;
            background-image: none;
            border: 1px solid transparent;
            border-radius: 4px;
            border: 1px solid #2E6DA4;
            background: #3498DB;
            color: #fff;
            margin-left: -32px;
        }

            .file-upload:hover {
                background: -webkit-gradient(linear, left top, left bottom, color-stop(0.05, #0061a7), color-stop(1, #007dc1));
                background: -moz-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -webkit-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -o-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: -ms-linear-gradient(top, #0061a7 5%, #007dc1 100%);
                background: linear-gradient(to bottom, #0061a7 5%, #007dc1 100%);
                filter: progid:DXImageTransform.Microsoft.gradient(startColorstr='#0061a7', endColorstr='#007dc1',GradientType=0);
                background-color: #0061a7;
            }

        /* The button size */
        .file-upload {
            height: 30px;
        }

            .file-upload, .file-upload span {
                width: 120px;
            }

                .file-upload input {
                    font-weight: bold;
                    /* Loses tab index in webkit if width is set to 0 */
                    opacity: 0;
                    filter: alpha(opacity=0);
                }

                .file-upload strong {
                    font: normal 12px Tahoma,sans-serif;
                    text-align: center;
                    vertical-align: middle;
                }

                .file-upload span {
                    top: 0;
                    left: 0;
                    display: inline-block;
                    /* Adjust button text vertical alignment */
                    padding-top: 5px;
                }
    </style>
    <script type="text/javascript" src="../js/jquery-1.11.2.min.js"></script>
    <script type="text/javascript" src="../js/lightbox/jquery.lightbox_me.js"></script>
    <script type="text/javascript" language="JavaScript1.1" src="MaskedInput.js"></script>
    <script language="javascript" type="text/javascript" src="../js/jquery.maskedinput.js"></script>
    <script type="text/javascript" src="../js/bootstrap-3.3.5-dist/js/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/formvalidation/dist/js/formValidation.min.js"></script>
    <script type="text/javascript" src="../js/formvalidation/dist/js/framework/bootstrap.min.js"></script>
    <script type="text/javascript" src="../js/intl-tel-input/build/js/intlTelInput.min.js"></script>
    <link href="../CSS/blue.css" rel="stylesheet" type="text/css" />
    <script type="text/javascript" src="../js/icheck/icheck.js"></script>
    <script type="text/javascript" src="../js/Lobibox/Lobibox.js"></script>
    <script type="text/javascript" language="javascript">

        $(document).ready(function () {

            $('input').iCheck({
                checkboxClass: 'icheckbox_square-blue',
                radioClass: 'iradio_square-blue',
                increaseArea: '20%' // optional
            });

            $('#btnSucata').on('ifChecked', function (event) {
                $('#PnSucata').show();
            });

            $('#btnSucata').on('ifUnchecked', function (event) {
                $('#PnSucata').hide();
            });

            $('#btnBackup').on('ifChecked', function (event) {
                $('#PnBackup').show();
            });

            $('#btnBackup').on('ifUnchecked', function (event) {
                $('#PnBackup').hide();
            });

            $('ul.tabs li').click(function () {
                var tab_id = $(this).attr('data-tab');

                $('ul.tabs li').removeClass('current');
                $('.tab-content').removeClass('current');

                $(this).addClass('current');
                $("#" + tab_id).addClass('current');
            });
        });

        function incluirCentroDeCusto(codigo, nome) {

            if (document.getElementById('cmbCCusto_mirror').value == "") {
                document.getElementById('cmbCCusto').value = codigo;
                document.getElementById('cmbCCusto_mirror').value = codigo;
            }
            else {
                document.getElementById('cmbCCusto').value = document.getElementById('cmbCCusto_mirror').value + " " + codigo;
                document.getElementById('cmbCCusto_mirror').value = document.getElementById('cmbCCusto_mirror').value + " " + codigo;
            }

            for (i = 0; i < objComboCC.options.length; i++) {
                if (objComboCC.options[i].value == codigo) {
                    return;
                }
            }
            var objNewOption = document.createElement('option');
            objNewOption.value = codigo;
            objNewOption.text = nome;
            objNewOption.alt = nome;
            objComboCC.options.add(objNewOption);
        }

        function atualizaGrupos(boolIncluir, strCodigo, strNome) {

            if (boolIncluir) {
                incluirCentroDeCusto(strCodigo, strNome)
            } else {
                removeCentroDeCusto(strCodigo);
            }

        }

        function removeTodosCentroDeCusto() {
            var objComboCC = document.getElementById('centroCusto');
            for (i = 0; i < objComboCC.options.length; i++) {
                objComboCC.options[i] = null;
            }
        }

        function incluirBusca(nome, codigo, tabela) {

            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            tabela = tabela.replace("?", " ");

            if (tabela == "PROJETOS") {
                document.getElementById('tbProjetos').value = document.getElementById('tbProjetos').value + nome + '\n';
                document.getElementById('tbProjetos_codes').value = document.getElementById('tbProjetos_codes').value + codigo + ' '
            }
            if (tabela == "USUARIOS") {
                document.getElementById('tbUsuario').value = nome;
                document.getElementById('tbUsuario_mirror').value = nome;
                document.getElementById('tb_user_code').value = codigo;
                __doPostBack('btnAddUser', '');
            }
            if (tabela == "VAS") {
                document.getElementById('tbFacilidades').value = document.getElementById('tbFacilidades').value + nome + '\n';
                document.getElementById('tbFacilidades_codes').value = document.getElementById('tbFacilidades_codes').value + codigo + ' '
            }
            if (tabela == "CODIGOS_CLIENTE") {
                document.getElementById('tbCodigo_cliente').value = codigo;
                document.getElementById('tbCodigo_cliente_name').value = nome;
                document.getElementById('tbCodigo_cliente_name_mirror').value = nome;

            }
            if (tabela == "TERMOS_RESPONSABILIDADE") {
                document.getElementById('tbTermResp_code').value = codigo;
                document.getElementById('tbTermResp').value = nome;
                document.getElementById('tbTermResp_mirror').value = nome;
            }
            if (tabela == "CHAMADOS") {
                document.getElementById('tbChamado').value = nome;
                document.getElementById('tbChamado_Mirror').value = codigo;
            }
            if (tabela == "CHAMADOS p2") {
                document.getElementById('tbChamadoSelected').value = nome;
                __doPostBack('btnChamadoSelected', '');
            }
            if (tabela == "GRUPOS") {
                document.getElementById('tbCCusto').value = nome;
                document.getElementById('tbCCusto_codes').value = codigo;
                __doPostBack('btnAddCCusto', '');
            }
            if (tabela == "LOCALIDADES p1") {
                document.getElementById('txtSucursal').value = nome.replace("?", " ");
                document.getElementById('txtSucursal_code').value = codigo;
            }

            document.getElementById("AlterLog").checked = true;
        }

        function BuscarFacilidade() {
            __doPostBack('btnPesquisarFacilidade', '');
        }

        function RemoveLastProject() {
            var str = document.getElementById('tbProjetos').value;
            var str_codes = document.getElementById('tbProjetos_codes').value;
            str = str.substr(0, str.lastIndexOf("\n"));
            str_codes = str_codes.substr(0, str_codes.lastIndexOf(" "));

            document.getElementById('tbProjetos').value = str;
            document.getElementById('tbProjetos_codes').value = str_codes;
        }

        function RemoveLastFacility() {
            var str = document.getElementById('tbFacilidades').value;
            var str_codes = document.getElementById('tbFacilidades_codes').value;
            str = str.substr(0, str.lastIndexOf("\n"));
            str_codes = str_codes.substr(0, str_codes.lastIndexOf(" "));

            document.getElementById('tbFacilidades').value = str;
            document.getElementById('tbFacilidades_codes').value = str_codes;
        }

        function RemoveLastGroup() {
            var str = document.getElementById('tbCCusto').value;
            var str_codes = document.getElementById('tbCCusto_codes').value;
            str = str.substr(0, str.lastIndexOf("\n"));
            str_codes = str_codes.substr(0, str_codes.lastIndexOf(" "));

            document.getElementById('tbCCusto').value = str;
            document.getElementById('tbCCusto_codes').value = str_codes;
        }

        function ExecutarPostBack() {
            __doPostBack('btnPostBack', '');
        }

        function RemoveLastCCusto() {

            var text = document.getElementById('cmbCCusto_mirror').value.split(' ');

            document.getElementById('cmbCCusto_mirror').value = "";
            document.getElementById('cmbCCusto').value = "";

            for (var i = 0, l = text.length - 1; i < l; ++i) {

                if (document.getElementById('cmbCCusto_mirror').value == "") {
                    document.getElementById('cmbCCusto_mirror').value = text[i];
                    document.getElementById('cmbCCusto').value = text[i];
                }
                else {
                    document.getElementById('cmbCCusto_mirror').value = document.getElementById('cmbCCusto_mirror').value + " " + text[i];
                    document.getElementById('cmbCCusto').value = document.getElementById('cmbCCusto').value + " " + text[i];
                }
            }
        }

        function RemoveCodCli() {
            document.getElementById('tbCodigo_cliente').value = "";
            document.getElementById('tbCodigo_cliente_name').value = "";
        }

        function RemoveTerm() {
            document.getElementById('tbTermResp').value = "";
            document.getElementById('tbTermResp_code').value = "";
            document.getElementById('tbTermResp_mirror').value = "";
        }
        function RemoveUser() {
            //alert('teste');
            document.getElementById('tbUsuario').value = "";
            document.getElementById('tb_user_code').value = "";
            incluirBusca('','','USUARIOS');
        }
        function RemoveChamado() {
            document.getElementById('tbChamado').value = "";
            document.getElementById('tbChamado_Mirror').value = "";
        }

        function AbreRateios() {

            var page = "GestaoRateiosEdit.aspx?codigo=" + document.getElementById('tbCodigo').value
            window.open(page, "", "top=0,left=20,width=330,height=400,scrollbars=true,resizable=false, scrollbars=1");

        }

        function AbrePesquisaCCustos() {

            var page = "GestaoPesquisarCampo.aspx?table=GRUPOS&name=CODIGO  ||' - '|| NOME_GRUPO&code_field=CODIGO&titulo=Grupos"
            window.open(page, "", "top=0,left=20,width=550,height=400,scrollbars=true,resizable=false, scrollbars=1");

        }

        function ExibeGrupos() {
            //$("#divCadastroCCusto").css('display', 'inline');

            $("#pnlBucaGrupo").niceScroll({ cursorborder: "", cursorcolor: "#00F", boxzoom: true }); // First scrollable DIV

            var dlg = $("#divCadastroCCusto").dialog({
                resizable: true,
                height: 400,
                width: 600,
                modal: true,
                show: 'Transfer',
                hide: 'Transfer',
                closeText: 'hide',
                buttons: {
                    "Fechar": function () {
                        __doPostBack('btLimpaCCustos', '');
                        $(this).dialog("close");

                    }
                }
            });

            $("#divCadastroCCusto").dialog({
                close: function (event, ui) {
                    __doPostBack('btLimpaCCustos', '');
                }
            });

            dlg.parent().appendTo(jQuery("form:first"));
        }

        function abreHistorico() {
            var codigo = document.getElementById("tbCodigo").value;
            window.open('aparelhosaux.asp?operacao=12&codigo=' + codigo, 'Busca', 'width=600,height=400,scrollbars=1,resizable=yes');
        }

        function abreManutencao() {
            var codigo = document.getElementById("tbCodigo_aparelho").value;
            window.open('manutencao.asp?operacao=4&aparelho=' + codigo, 'Busca', 'width=500,height=400,scrollbars=1,resizable=yes');
        }

        function abreTroca() {
            var codigo = document.getElementById("tbCodigo_aparelho").value;
            var linha = document.getElementById("tbCodigo").value;
            window.open('GestaoTrocaAparelho.aspx?codigo_aparelho=' + codigo + '&codigo_linha=' + linha, 'Busca', 'width=1070,height=485,scrollbars=0,resizable=no');
        }


        function abreDesvinculo() {
            __doPostBack('btndesvincular', '');
        }

        function abreVinculo() {
            var codigo = document.getElementById("tbCodigo_aparelho").value;
            var linha = document.getElementById("tbCodigo").value;
            
            window.open('GestaoVincularAparelho.aspx?codigo_linha=' + linha, 'Busca', 'width=1070,height=485,scrollbars=0,resizable=no');

        }

        function Mascara(objeto) {
            if (objeto.value.length == 0)
                objeto.value = '(' + objeto.value;

            if (objeto.value.length == 3)
                objeto.value = objeto.value + ')';

            if (objeto.value.length == 8)
                objeto.value = objeto.value + '-';
            if (objeto.value.length >= 14)

                objeto.value = objeto.value.substr(0, 13);


        }

        //****************************** Form Change Validator functions *************************************************

        var formChanged = false;

        $(document).ready(function () {
            $('#form1 input[type=text], #form1 textarea').each(function (i) {
                $(this).data('initial_value', $(this).val());
            });

            $('#form1 input[type=text], #form1 textarea').keyup(function () {
                if ($(this).val() != $(this).data('initial_value')) {
                    handleFormChanged();
                }
            });

            $('#form1').bind('change paste', function () {
                handleFormChanged();
            });


        });

        function handleFormChanged() {
            $('#save_or_update').attr("disabled", false);
            formChanged = true;
            document.getElementById("AlterLog").checked = true;
        }

        window.addEventListener("beforeunload", function (e) {
            var confirmationMessage = "É possivel que as alterações feitas não sejam salvas";
            /* Do you small action code here */
            if(formChanged == true){
                (e || window.event).returnValue = confirmationMessage; //Gecko + IE
                return confirmationMessage;    
            }
            //Webkit, Safari, Chrome
        });

        function janelalog(tipo) {
            if (tipo == 1) {
                document.getElementById('hidden_tipo').value = '1'
            }
            if (tipo == 2) {
                document.getElementById('hidden_tipo').value = '2'
            }
            if (tipo == 3) {
                document.getElementById('hidden_tipo').value = '3'
            }
        }

        function RemovetbTelefone() {
            document.getElementById('tbTelefone').value = "(__)____-_____";
        }

        function CarregaFacilidades(valor) {
            //alert(valor);
            if (confirm("Deseja sobreescrever as facilidades atuais com as associadas ao plano?")) {
                //delete here
                formChanged = false;
                __doPostBack('btPlanosFacilidades', '');

            }

        }

        function PesquisaCampoSuc() {
            var table = "LOCALIDADES p1"
            var nome = "localidade"
            var codigo = "CODIGO"

            window.open('GestaoPesquisarCampo.aspx?table=' + table + '&name=' + nome + '&code_field=' + codigo + '', 'Busca', 'width=345,height=200,scrollbars=1');
        }

        

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hdnSelectedTab" runat="server" Value="0" />
        <div class="divSuperior">
          
            <script>
                var prm = Sys.WebForms.PageRequestManager.getInstance();

                prm.add_endRequest(function () {

                    $('input').iCheck({
                        checkboxClass: 'icheckbox_square-blue',
                        radioClass: 'iradio_square-blue',
                        increaseArea: '20%' // optional
                    });

                    $('#btnSucata').on('ifChecked', function (event) {
                        $('#PnSucata').show();
                    });

                    $('#btnSucata').on('ifUnchecked', function (event) {
                        $('#PnSucata').hide();
                    });

                    $('#btnBackup').on('ifChecked', function (event) {
                        $('#PnBackup').show();
                    });

                    $('#btnBackup').on('ifUnchecked', function (event) {
                        $('#PnBackup').hide();
                    });

                    $('ul.tabs li').click(function () {
                        var tab_id = $(this).attr('data-tab');

                        $('ul.tabs li').removeClass('current');
                        $('.tab-content').removeClass('current');

                        $(this).addClass('current');
                        $("#" + tab_id).addClass('current');
                    });
                });
        

                //********************************** VALIDATORS ************************************************************************

                $("#tbDt_ativ").mask("99/99/9999");
                $("#tbDt_des").mask("99/99/9999");
        
                //********************************** Importante validar caso use updatePanel ********************************************* 

                function isFormValid() {
                    var formValidation = $('#form1').data('formValidation');
                    formValidation.validate();

                    if (formValidation.isValid() != true) {
                        return false;
                    }
                    return true;
                }

                function fn_init() { 
            
                    $('#form1').data('formValidation',null);

                    $('#form1').find('[name="tbTelefone"]').intlTelInput({
                        utilsScript: '../js/intl-tel-input/lib/libphonenumber/build/utils.js',
                        autoPlaceholder: false,
                        nationalMode:true,
                        defaultCountry:  'br',
                        preferredCountries: ['br','us']
                    });

                    // $('#form1').find('[name="tbTelefone"]').intlTelInput("selectCountry", "br");

                    $('#form1').formValidation({
                        excluded: [':disabled'],
                        framework: 'bootstrap',
                        icon: {
                            valid: 'glyphicon glyphicon-ok',
                            invalid: 'glyphicon glyphicon-remove',
                            validating: 'glyphicon glyphicon-refresh'
                        },
                        fields: {
                            //     <%=tbTelefone.UniqueID%>: {
                            //        validators: {
                            //             callback: {
                            //                  message: 'Número de telefone inválido',
                            //                  callback: function (value, validator, $field) {
                            //                      return value === '' || $field.intlTelInput('isValidNumber');
                            //                  }
                            //             }
                            //          }
                            //      }
                            //   ,
                            <%=tbSIMCARD.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 30 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 30 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        ,
                            <%=tbFleet.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 20 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 20 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbContrato.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 20 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 20 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        , <%=tbIp.UniqueID%>: {
                            validators: {
                                ip: {
                                    message: 'Endereço de e-mail inválido'
                                }
                            }
                        },
                            <%=tbSIMCARD_value.UniqueID%>: {
                                validators: {
                                    numeric: {
                                        message: 'Número digitado inválido',
                                        // The default separators
                                        thousandsSeparator: '.',
                                        decimalSeparator: ','
                                    }
                                }   
                            },
                            <%=tbProtocolo_cancel.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 40 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 40 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbObs.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 2000 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 2000 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbPin_Aparelho.UniqueID%>: {
                                validators: {
                                    numeric: {
                                        message: 'Número digitado inválido',
                                    },
                                    stringLength: {
                                        message: 'Máximo de 20 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 20 - (value.match(/\r/g) || []).length;
                                           
                                        }
                                    }
                                }
                            },
                            <%=tbSerialNumber.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 50 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 50 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbNotaFiscal.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 20 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 20 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbIMEI.UniqueID%>: {
                                validators: {
                                    numeric: {
                                        message: 'Número digitado inválido',
                                    },
                                    stringLength: {
                                        message: 'Máximo de 30 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 30 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        ,
                            <%=tbProp_Estoque.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 100 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 100 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        ,
                            <%=tbOrdem_Serviço.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 50 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 50 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        ,
                            <%=tbChamado_Retirada.UniqueID%>: {
                                validators: {
                                    stringLength: {
                                        message: 'Máximo de 50 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 50 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            },
                            <%=tbLimite_Uso.UniqueID%>: {
                                validators: {
                                    numeric: {
                                        message: 'Número digitado inválido',
                                        // The default separators
                                        thousandsSeparator: '',
                                        decimalSeparator: ','
                                    },
                                    stringLength: {
                                        message: 'Máximo de 12 caracteres.',
                                        max: function (value, validator, $field) {
                                            return 12 - (value.match(/\r/g) || []).length;
                                        }
                                    }
                                }
                            }
                        
                        }
                    })
                .on('err.field.fv', '[name="<%=tbPin_Aparelho.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbSerialNumber.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbNotaFiscal.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbIMEI.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbProp_Estoque.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbOrdem_Serviço.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbChamado_Retirada.UniqueID%>"]', function(e, data) {
                    open_tab(2);
                })
                .on('err.field.fv', '[name="<%=tbLimite_Uso.UniqueID%>"]', function(e, data) {
                    open_tab(3);
                })

                .on('err.field.fv', function(e, data) {
                    // data.element --> The field element

                    var $tabPane = data.element.parents('.tab-pane'),
                        tabId    = $tabPane.attr('id');

                    $('a[href="#' + tabId + '"][data-toggle="tab"]')
                        .parent()
                        .find('i')
                        .removeClass('fa-check')
                        .addClass('fa-times');
                })
            // Called when a field is valid
            .on('success.field.fv', function(e, data) {
                // data.fv      --> The FormValidation instance
                // data.element --> The field element

                var $tabPane = data.element.parents('.tab-pane'),
                    tabId    = $tabPane.attr('id'),
                    $icon    = $('a[href="#' + tabId + '"][data-toggle="tab"]')
                                .parent()
                                .find('i')
                                .removeClass('fa-check fa-times');

                // Check if all fields in tab are valid
                var isValidTab = data.fv.isValidContainer($tabPane);
                if (isValidTab !== null) {
                    $icon.addClass(isValidTab ? 'fa-check' : 'fa-times');
                }
            })

                .on('click', '.country-list', function () {
                    $('#form1').formValidation('revalidateField', 'tbTelefone');
                })

                .on('err.field.fv', function(e, data) {
                    // $(e.target)  --> The field element
                    // data.fv      --> The FormValidation instance
                    // data.field   --> The field name
                    // data.element --> The field element

                    data.fv.disableSubmitButtons(false);
                });


                    var prm = Sys.WebForms.PageRequestManager.getInstance();
                }

                function PostaFoto() {
                    __doPostBack('btnPostFoto', '');

                }


            </script>
            <div class="tableTitulo" style="margin-bottom: -16px;">
                <ul>
                    <br />
                    <li class="titulopagina" coldiv="2">Cadastro de Móveis</li>
                </ul>
            </div>
        </div>
        <div class="divEsquerda_2columns" style="width: 140px; padding-left: 0px; margin-left: 0px;">
            <br />
            <br />
            <br />
            <ul>
                <li>
                    <asp:Button ID="btGravar" runat="server" Text="Gravar" Width="110px" class="btn btn-primary" OnClientClick="formChanged = false;" />
                    <asp:Button ID="btGravar_novo" runat="server" Text="Gravar Novo" Width="110px" class="btn btn-primary"
                        Visible="false" Style="margin-top: 4px;" OnClientClick="formChanged = false;" />
                    <asp:CheckBox ID="AlterLog" runat="server" Text="" Style="display: none;" Checked="False" />
                    <asp:Button ID="btExcluir" runat="server" Text="Excluir" Enabled="False" Width="110px"
                        class="btn btn-primary" Style="margin-top: 4px;" OnClientClick="formChanged = false;" />
                    
                    <br />
                    <br />
                </li>
            </ul>
            <ul>
                <li>
                    <asp:Button ID="btNovo" runat="server" Text="Novo" Enabled="False" Width="80px" class="btn btn-primary"
                        Style="display: none;" OnClientClick="formChanged = false;" />
                </li>
                <asp:Panel ID="pnAGeradora" Visible="false" runat="server">
                    <li>
                        <asp:Button ID="btTermo" runat="server" Text="Ter. Aparelho" Width="110px" class="btn btn-primary" OnClientClick="formChanged = false;" />
                        <br />
                        <br />
                        <asp:Button ID="btTermo_2" runat="server" Text="Ter. Linha" Width="110px" class="btn btn-primary" OnClientClick="formChanged = false;" />
                        <br />
                        <br />
                        <asp:Button ID="btTermo_3" runat="server" Text="Ter. Devolução" Width="110px" class="btn btn-primary" OnClientClick="formChanged = false;" />
                        <br />
                        <br />
                        <asp:Button ID="btTermo_4" runat="server" Text="Ter. Doação" Width="110px" class="btn btn-primary" OnClientClick="formChanged = false;" />
                    </li>
                </asp:Panel>
                <asp:Panel ID="pnVonpar" Visible="false" runat="server">
                    <li>
                        <br />
                        <asp:Button ID="btTermoVonpar" runat="server" Text="T. Responsabilidade" Width="110px"
                            class="btn btn-primary" OnClientClick="formChanged = false;" />
                    </li>
                </asp:Panel>
                <asp:Panel ID="pnGlobo" Visible="false" runat="server">
                    <li>
                        <br />
                        <asp:Button ID="btnTermoGLOBO" runat="server" Text="Termo" Width="110px"
                            class="btn btn-primary" OnClientClick="formChanged = false;" />
                    </li>
                </asp:Panel>
            </ul>
            <br />
            <ul>
                <li>
                    <asp:Button ID="btHistorico" runat="server" Text="Histórico Linha" OnClientClick="formChanged = false;return janelalog(1)"
                        Width="110px" class="btn btn-primary" Style="margin-top: 4px;" />
                    <asp:Button ID="btHistorico_SIM" runat="server" Text="Histórico SIM  " OnClientClick="formChanged = false;return janelalog(2)"
                        Width="110px" class="btn btn-primary" Style="margin-top: 4px;" />
                    <asp:Button ID="btHistorico_IMEI" runat="server" Text="Histórico IMEI " OnClientClick="formChanged = false;return janelalog(3)"
                        Width="110px" class="btn btn-primary" Style="margin-top: 4px;" />
                    <asp:TextBox ID="hidden_tipo" runat="server" Style="display: none;"></asp:TextBox>
                </li>
            </ul>
            <br />
            <ul>
                <li>
                    <div class="btn-group" runat="server" id="div_chamados">
                        <button type="button" class="btn btn-primary" data-toggle="dropdown">Chamados</button>
                        <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" style="height: 31px;">
                            <span class="caret"></span>
                        </button>
                        <ul class="dropdown-menu" role="menu">
                            <li>
                                <asp:LinkButton ID="btNovoChamado" Style="width: 100%" runat="server" Text="Criar novo chamado" Visible="true" OnClientClick="formChanged = false;" /></li>
                            <li>
                                <asp:LinkButton ID="btVincularChamado" Style="width: 100%" runat="server" Text="Vincular um chamado" Visible="true" OnClientClick="formChanged = false;" /></li>
                            <li>
                                <asp:LinkButton ID="btEditarChamado" Style="width: 100%" runat="server" Text="Editar chamados vinculados" Visible="true" OnClientClick="formChanged = false;" /></li>
                        </ul>
                    </div>
                </li>
            </ul>
            <div class="form-group" style="display: none">
                <div class="div_largura_90" style="background-color: white; border-radius: 6px; width: 172px; height: 56px; border: 1px solid silver;">
                    <center>
                        <br />
                        <div id="divChamadoNew" runat="server" style="margin-top: -10px;">
                            <a href="javascript:window.open('GestaoCadastroChamado.aspx?<%="item=" & tbcodigo.text & "&item_nome=" & tbtelefone.text & "&tipo=1" & "&page=new"& "&oem=" & lbUltimoOEM.Text %>','Busca','width=940,height=600,scrollbars=1'); void(0)">
                                <img alt="mag" src="..\Icons\95.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                    title="Criar um novo chamado" /></a> Chamado <a href="javascript:window.open('GestaoPesquisarCampo.aspx?table=CHAMADOS&name=OEM&code_field=OEM&titulo=Chamados','Busca','width=310,height=200,scrollbars=1'); void(0)">
                                        <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px;" title="Procurar" /></a>
                            <a href="javascript:RemoveChamado(); void(0)">
                                <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                    title="Remover" /></a>
                            <br />
                            <asp:TextBox ID="tbChamado" runat="server" Width="50px" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="tbChamado_Mirror" runat="server" Width="120px" Enabled="false"></asp:TextBox>
                        </div>
                        <div id="divChamadoedit" runat="server" style="margin-top: -10px;">
                            <asp:Label ID="lbChamado" runat="server" Text="Último chamado" />
                            <a href="javascript:window.open('GestaoCadastroChamado.aspx?<%="item=" & tbcodigo.text & "&item_nome=" & tbtelefone.text & "&tipo=1" & "&page=grid"%>','Busca','width=940,height=600,scrollbars=1'); void(0)">
                                <img alt="mag" src="..\Icons\43.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                    title="Novo Chamado" /></a> <a href="javascript:window.open('GestaoCadastroChamado.aspx?<%="item=" & tbcodigo.text & "&item_nome=" & tbtelefone.text & "&tipo=1" & "&page=edit"& "&oem=" & lbUltimoOEM.Text.Replace("Sem chamado","") %>','Busca','width=940,height=560,scrollbars=1'); void(0)">
                                        <img alt="mag" runat="server" id="imgChamEdit" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                            title="Editar último chamado" /></a>
                            <a href="javascript:window.open('GestaoPesquisarCampo.aspx?table=CHAMADOS p2&name=OEM&code_field=OEM&titulo=Chamados','Busca','width=310,height=200,scrollbars=1'); void(0)">
                                <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                    title="Procurar" /></a>
                            <br />
                            <asp:Button ID="btnChamadoSelected" runat="server" Text="Gravar" Width="100px" class="btn btn-primary"
                                Style="display: none" />
                            <asp:TextBox ID="tbChamadoSelected" runat="server" Width="50px" Style="display: none"></asp:TextBox>
                            <asp:TextBox ID="lbUltimoOEM" runat="server" Style="margin-top: 5px;" />
                        </div>
                    </center>
                </div>
            </div>
        </div>
        <div class="container" style="font-size: 14px; margin-left: 145px;">
            <ul class="tabs">
                <li class="tab-link current" data-tab="tab-1" id="tab_link1">Linha</li>
                <li class="tab-link" data-tab="tab-2" id="tab_link2">Aparelho</li>
                <li class="tab-link" data-tab="tab-3" id="tab_link3">Usuário</li>
            </ul>
            <div id="tab-1" class="tab-content current">
                <ul>
                    <center>
                        <asp:TextBox ID="tbCodigo" runat="server" Style="display: none"></asp:TextBox>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_90">
                                    <label class="control-label">
                                        Telefone</label>
                                    <asp:TextBox class="form-control" ID="tbTelefone" placeholder="" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Classificação <a href="javascript:window.open('GestaoClassificacao.aspx','Busca','width=380,height=450,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px;" title="Novo" /></a>
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbClassificacao" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Fornecedor <a href="javascript:window.open('GestaoFornecedores.aspx','Busca','width=1050,height=500,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px;" title="Novo" /></a>
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbOperadora" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO" AutoPostBack="true" onclick="formChanged = false;">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Plano <a href="javascript:window.open('GestaoCadastroPlanos.aspx','Busca','width=800,height=300,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\95.png" style="border: 0; width: 18px;" title="Novo" /></a>
                                        <a href="javascript:window.open('GestaoPlanos.aspx','Busca','width=1050,height=500,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px;" title="Novo" /></a>
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbPlanos" runat="server" Width="145px"
                                        DataTextField="DESCRICAO" onclick="formChanged = false;" onchange="CarregaFacilidades(this.value)" DataValueField="CODIGO">
                                    </asp:DropDownList>
                                    <asp:Button ID="btPlanosFacilidades" runat="server" Visible="false" />

                                </div>
                            </div>
                        </li>
                    </center>
                </ul>
                <ul>
                    <center>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        SIMCARD</label>
                                    <asp:TextBox ID="tbSIMCARD" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Valor SIM Card</label>
                                    <asp:TextBox class="form-control" ID="tbSIMCARD_value" runat="server" onKeyPress="return(MascaraMoeda(this,'.',',',event))"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Fleet</label>
                                    <asp:TextBox ID="tbFleet" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        IP</label>
                                    <asp:TextBox ID="tbIp" runat="server" class="form-control"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                    </center>
                </ul>
                <ul>
                    <center>

                        <li>
                            
                            <div class="form-group">
                                <div class="div_largura_40">
                                    <b>Ativ.</b>
                                    <asp:ImageButton ID="image1" runat="server" ImageUrl="~/images/Calendar.png" />
                                    <asp:TextBox class="form-control" ID="tbDt_ativ" runat="server" Columns="10" Enabled="true"></asp:TextBox>
                                </div>
                                <div class="div_largura_40">
                                    <b>Desativ.</b>
                                    <asp:ImageButton ID="image2" runat="server" ImageUrl="~/images/Calendar.png" />
                                    <asp:TextBox class="form-control" ID="tbDt_des" runat="server" Columns="10" Enabled="true"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Contrato</label>
                                    <asp:TextBox class="form-control" ID="tbContrato" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <b>
                                    <div class="div_largura_90" style="margin-top: 6px;">
                                        PIN1<asp:TextBox ID="tbPIN1" runat="server" Width="48px" class="form-control" MaxLength="20"></asp:TextBox>
                                        PUK1<asp:TextBox ID="tbPUK1" runat="server" Width="48px" class="form-control" MaxLength="20"></asp:TextBox>
                                        <br />
                                        PIN2<asp:TextBox ID="tbPIN2" runat="server" Width="48px" class="form-control" MaxLength="20"></asp:TextBox>
                                        PUK2<asp:TextBox ID="tbPUK2" runat="server" Width="48px" class="form-control" MaxLength="20"></asp:TextBox>
                                    </div>
                                </b>
                            </div>
                        </li>
                    </center>
                </ul>
                <ul>
                    <center>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Sucursal <a href="javascript:PesquisaCampoSuc(); void(0)">
                                            <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                title="Procurar" /></a>
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbSucursal" runat="server" DataTextField="Localidade"
                                        DataValueField="codigo" Visible="false">
                                    </asp:DropDownList>
                                    <asp:TextBox ID="txtSucursal" class="form-control" runat="server" ReadOnly="true"></asp:TextBox>
                                    <asp:TextBox ID="txtSucursal_code" class="form-control" runat="server" AutoPostBack="true"
                                        Style="display: none" />
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80">
                                    <label class="control-label">
                                        Status
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbStatus" DataValueField="CODIGO" DataTextField="DESCRICAO" runat="server">
                                    </asp:DropDownList>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_90">
                                    <label class="control-label">
                                        Código Cliente <a href="javascript:window.open('GestaoCodClienteCelular.aspx','Busca','width=500,height=480,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                title="Novo" /></a> <a href="javascript:window.open('GestaoPesquisarCampo.aspx?table=CODIGOS_CLIENTE&name=CLIENTE&code_field=CODIGO_CLIENTE&titulo=Codigo-Cliente','Busca','width=310,height=200,scrollbars=1'); void(0)">
                                                    <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                        title="Procurar" /></a> <a href="javascript:RemoveCodCli(); void(0)">
                                                            <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                                title="Remover" /></a></label>
                                    <asp:TextBox class="form-control" ID="tbCodigo_cliente" runat="server" Width="50px"
                                        Style="display: none"></asp:TextBox>
                                    <asp:TextBox class="form-control" ID="tbCodigo_cliente_name_mirror" runat="server"
                                        Style="display: none"></asp:TextBox>
                                    <asp:TextBox class="form-control" ID="tbCodigo_cliente_name" runat="server" Enabled="false"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_90">
                                    <label class="control-label">
                                        Protocolo Cancelamento
                                    </label>
                                    <asp:TextBox class="form-control" ID="tbProtocolo_cancel" runat="server" Width="160px"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                    </center>
                </ul>
                <ul>
                    <center>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_90">
                                    <label class="control-label">
                                        Conta Contábil <a href="javascript:window.open('GestaoContaContabil.aspx','Busca','width=368,height=480,scrollbars=1'); void(0)">
                                            <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                title="Novo" /></a>
                                    </label>
                                    <asp:DropDownList class="form-control" ID="cmbContaContabil" runat="server" DataTextField="DESCRICAO"
                                        DataValueField="CODIGO">
                                    </asp:DropDownList>
                                </div>
                            </div>
                            <div class="form-group">
                                <br />
                                <br />
                                <br />
                                <br />
                                <center>
                                    <div class="div_largura_40_nofloat">
                                        <label class="control-label">
                                            Intragrupo
                                        </label>
                                        <br />
                                        <br />
                                        <asp:CheckBox ID="tbIntragrupo" name="iCheck" runat="server" />
                                    </div>
                                </center>
                            </div>
                        </li>
                        <li>
                            <asp:Panel ID="pnlGridView" runat="server" ScrollBars="Vertical" Height="180px" Style="text-align: center; border: 1px solid silver; border-radius: 6px;">
                                Facilidades <a href="javascript:window.open('GestaoFacilidades.aspx','Busca','width=1050,height=500,scrollbars=1'); void(0)">
                                    <img alt="mag" src="..\Icons\43.png" style="border: 0; width: 18px; vertical-align: bottom; margin-top: 5px;"
                                        title="Visualizar Facilidades" /></a>
                                <center style="border: 0px none;">
                                    <asp:GridView ID="GvFacilidades" runat="server" BackColor="White" BorderColor="white"
                                        BorderStyle="None" CellPadding="4" EnableModelValidation="True"
                                        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="160px"
                                        Height="200px" RowStyle-Height="40px" HeaderStyle-Height="20px" ShowHeader="False"
                                        Style="margin-top: 5px; font-size: smaller;">
                                        <Columns>
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                                                    <input type="checkbox" id="chkFacilidade" runat="server" />
                                                    <input type="hidden" id="chkhidden" runat="server" value='<%# Eval("Codigo") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Facilidades" />
                                            <asp:BoundField DataField="VALOR" HeaderText="valor" DataFormatString="{0:C}" />
                                            <asp:ButtonField Text="<img src='../Icons/pencil_48.png' style='border:0;width:20px;height:20px' />"
                                                ImageUrl="~/Icons/pencil_48.png" CommandName="Editar" Visible="false">
                                                <ControlStyle Height="20px" Width="20px" />
                                                <ItemStyle Width="25px" />
                                            </asp:ButtonField>
                                        </Columns>
                                        <AlternatingRowStyle BackColor="White" />
                                        <FooterStyle BackColor="#CCCC99" />
                                    </asp:GridView>
                                </center>
                                <br />
                            </asp:Panel>
                            <asp:Label ID="Label1" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </li>
                        <li>
                            <asp:Panel ID="Panel1" runat="server" ScrollBars="Vertical" Height="180px" Width="160px"
                                Style="text-align: center; border: 1px solid silver; border-radius: 6px;">
                                Projetos
                                    <a href="javascript:window.open('GestaoProjetos.aspx','Busca','width=1050,height=500,scrollbars=1'); void(0)">
                                        <img alt="mag" src="..\Icons\43.png" style="border: 0; width: 18px; vertical-align: bottom; margin-top: 5px;"
                                            title="Visualizar Projetos" /></a>
                                <center>
                                    <asp:GridView ID="GvProjetos" runat="server" BackColor="White" BorderColor="white"
                                        BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                                        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="120px"
                                        Height="200px" RowStyle-Height="40px" HeaderStyle-Height="20px" ShowHeader="False"
                                        Style="margin-top: 5px; margin-left: 12px; font-size: smaller;">
                                        <Columns>
                                            <asp:TemplateField>
                                                <%-- <HeaderTemplate>
                                                <input type="checkbox" id="chk_SelectAll" runat="server" />
                                            </HeaderTemplate>--%>
                                                <ItemTemplate>
                                                    <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                                                    <input type="checkbox" id="chkProjeto" runat="server" />
                                                    <input type="hidden" id="chkhidden" runat="server" value='<%# Eval("CODIGO") %>' />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="DESCRICAO" HeaderText="Projetos" />
                                        </Columns>
                                    </asp:GridView>
                                </center>
                                <br />
                            </asp:Panel>
                            <asp:Label ID="Label2" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                        </li>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_80_nofloat">
                                    <label class="control-label">
                                        OBS
                                    </label>
                                    <asp:TextBox ID="tbOBS" class="form-control" runat="server" Width="140px" Height="164px"
                                        TextMode="MultiLine"></asp:TextBox>
                                    <%--                        <asp:Button ID="btnExcluiProjeto" runat="server" Text="Excluir" Enabled="False" Width="100px"
                            class="btn btn-primary" />
                        <asp:Button ID="btnExcluiCCusto" runat="server" Text="Excluir" Enabled="False" Width="100px"
                            class="btn btn-primary" />--%>
                                    <asp:Button ID="btnPesquisarFacilidade" runat="server" Text="Excluir" Enabled="False"
                                        Style="display: none;" Width="100px" class="btn btn-primary" />
                                </div>
                            </div>
                        </li>
                    </center>
                </ul>
            </div>
            <div id="tab-2" class="tab-content">
                <div id="div_troca" runat="server" style="float: left;">
                    <a href="javascript:abreTroca(); void(0)">Troca de Aparelho</a>
                    &nbsp; - &nbsp;
                </div>

                <div id="div_vincula" runat="server" style="float: left;">
                    <a href="javascript:abreVinculo(); void(0)" title="Salva o aparelho móvel atual em uma linha vazia selecionada deixando essa livre">Vincular Aparelho</a>
                    &nbsp; - &nbsp;
                </div>
                <div id="div_desvincula" runat="server" style="float: left;">

                    <a href="javascript:abreDesvinculo(); void(0)" title="Vincula o aparelho a uma linha vazia">Desvincular Aparelho</a>
                    <asp:Button ID="btnVincular" runat="server" Visible="false" />
                    <asp:Button ID="btndesvincular" runat="server" Visible="false" />
                </div>
                <br />
                <br />
                <ul style="width: 220px; float: left;">
                    <li>
                        <div class="form-group">
                            <div class="div_largura_80">
                                <label class="control-label">
                                    Marca <a href="javascript:window.open('GestaoMarcaCelular.aspx','Busca','width=380,height=450,scrollbars=1'); void(0)">
                                        <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                            title="Novo" /></a>
                                </label>
                                <asp:DropDownList class="form-control" ID="cmbMarca" runat="server" DataTextField="NOME"
                                    DataValueField="CODIGO" AutoPostBack="true" OnSelectedIndexChanged="cmbMarca_SelectedIndexChanged">
                                </asp:DropDownList>
                                <asp:TextBox ID="tbCodigo_aparelho" class="form-control" runat="server" Style="display: none"
                                    Width="90px"></asp:TextBox>
                            </div>
                        </div>
                    </li>
                </ul>
                <asp:UpdatePanel ID="PntabLinha" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <asp:Panel ID="PnSemAparelho" runat="server" Style="height: 110px;">
                            <ul style="float: left;">
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_80">
                                            <label class="control-label">
                                                Modelo <a href="javascript:window.open('GestaoModeloCelular.aspx','Busca','width=380,height=450,scrollbars=1'); void(0)">
                                                    <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                        title="Novo" />
                                                </a>
                                            </label>
                                            <asp:DropDownList class="form-control" ID="cmbModelo" runat="server" AutoPostBack="true"
                                                DataTextField="DESCRICAO" DataValueField="CODIGO" Width="155px">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_90">
                                            <label class="control-label">
                                                Tecnologia
                                            </label>
                                            <asp:DropDownList class="form-control" ID="cmbTecnologia" runat="server">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <label class="control-label">
                                            Natureza
                                        </label>
                                        <div class="div_largura_90">
                                            <asp:DropDownList class="form-control" ID="cmbNatureza" runat="server" AutoPostBack="true" OnSelectedIndexChanged="cmbNatureza_SelectedIndexChanged">
                                            </asp:DropDownList>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                            <ul>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_40">
                                            <label class="control-label">
                                                Ap.(PIN)
                                            </label>
                                            <asp:TextBox ID="tbPin_Aparelho" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                        <div class="div_largura_40">
                                            <label class="control-label">
                                                Valor Ap.
                                            </label>
                                            <asp:TextBox ID="tbValor_aparelho" class="form-control" runat="server" onKeyPress="return(MascaraMoeda(this,'.',',',event))"></asp:TextBox>
                                        </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_20" style="width: 50px;">
                                            <label class="control-label">
                                                QTD.
                                            </label>
                                            <asp:DropDownList ID="cmbQtdParcel" class="form-control" runat="server" DataTextField="CODIGO" DataValueField="CODIGO"></asp:DropDownList>
                                        </div>
                                        <div class="div_largura_60">
                                            
                                            <label class="control-label">
                                                Inicio Parcelas
                                            </label>
                                            <asp:TextBox ID="tbInicioParcl" class="form-control" runat="server" Columns="10"
                                                Enabled="true" Width="80px"></asp:TextBox>
                                            
                                            <asp:ImageButton ID="calendarParcl" runat="server" ImageUrl="~/images/Calendar.png" />
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="btn-group" data-toggle="buttons">
                                        <br />
                                        <asp:CheckBox ID="chkMostraParcela" runat="server" GroupName="01" />
                                        Exibe Parcela Relatório
                                    </div>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_80">
                                            <label class="control-label">
                                                Identificação(Hexa/IMEI)
                                            </label>
                                            <asp:TextBox ID="tbIMEI" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </li>
                            </ul>
                            <br />
                            <br />
                            <br />
                            <br />
                            <ul>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_80">
                                            <label class="control-label">
                                                Nota Fiscal
                                            </label>
                                            <asp:TextBox ID="tbNotaFiscal" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                    <div class="form-group">
                                        <div class="div_largura_80">
                                            <label class="control-label">
                                                Serial Number
                                            </label>
                                            <asp:TextBox ID="tbSerialNumber" class="form-control" runat="server"></asp:TextBox>
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <asp:Panel ID="PnFotos" runat="server" ScrollBars="Vertical" Height="100px" Width="170px" Visible="false" Style="text-align: center; border: 1px solid silver; border-radius: 6px;">
                                        <p style="margin-top: 5px;">Fotos</p>
                                        <center style="border: 0px none;">
                                            <asp:GridView ID="GvFotos" runat="server" BackColor="White" BorderColor="white"
                                                BorderStyle="None" CellPadding="4" EnableModelValidation="True"
                                                ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="120px"
                                                HeaderStyle-Height="20px" ShowHeader="False"
                                                Style="margin-top: 5px; margin-left: 12px;">
                                                <RowStyle Height="5px" />
                                                <Columns>
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <input type="hidden" id="chkhidden" runat="server" value='<%# Eval("CODIGO") %>' />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="CODIGO" HeaderText="Codigo" Visible="false" />
                                                    <asp:BoundField DataField="DESCRICAO" HeaderText="Arquivo" />
                                                    <asp:ButtonField HeaderText="Remover" Text="<img src='../Icons/cancel_48.png' style='border-width:0;width:20px;height:20px' />"
                                                        ImageUrl="~/Icons/cancel_48.png" CommandName="Excluir">
                                                        <ControlStyle Width="20px" />
                                                        <ItemStyle Width="25px" HorizontalAlign="Center" />
                                                    </asp:ButtonField>
                                                </Columns>
                                                <AlternatingRowStyle BackColor="White" />
                                                <FooterStyle BackColor="#CCCC99" />
                                            </asp:GridView>
                                        </center>
                                        <br />
                                        <asp:Label ID="Label4" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                        <center>
                                            <label class="file-upload">
                                                <span><strong>Selecionar Imagem</strong></span>
                                                <asp:FileUpload ID="UploadArquivo" runat="server" onchange="PostaFoto();"></asp:FileUpload>
                                                <asp:Button ID="btnPostFoto" runat="server" Text="postFoto" Style="display: none;" />
                                            </label>
                                        </center>
                                    </asp:Panel>
                                </li>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_60">
                                            
                                            <label class="control-label">
                                                Venc. Garantia
                                            </label>
                                            <asp:TextBox ID="tbVencimento_Garantia" class="form-control" runat="server" Columns="10"
                                                Enabled="true" Width="80px"></asp:TextBox>
                                            
                                            <asp:ImageButton ID="image3" runat="server" ImageUrl="~/images/Calendar.png" />
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div id="div_comodato" class="form-group" runat="server">
                                        <div class="div_largura_60">
                                            
                                            <label class="control-label">
                                                Venc. Comodato
                                            </label>
                                            <asp:TextBox ID="tbVencimento_Comodato" class="form-control" runat="server" Columns="10"
                                                Enabled="true" Width="80px"></asp:TextBox>
                                           
                                            <asp:ImageButton ID="image4" runat="server" ImageUrl="~/images/Calendar.png" />
                                        </div>
                                    </div>
                                </li>
                                <li>
                                    <div class="btn-group" data-toggle="buttons">
                                        <asp:CheckBox ID="tbEstoque" runat="server" GroupName="01" />
                                        Estoque
                                <asp:CheckBox ID="btnSucata" runat="server" GroupName="01" />
                                        Sucata
                                <br />
                                        <br />
                                        <asp:CheckBox ID="btnPerdido" runat="server" GroupName="01" />
                                        Perdido/Roubado
                                <br />
                                        <br />
                                        <asp:CheckBox ID="btnBackup" runat="server" GroupName="01" />
                                        Backup
                                <br />
                                    </div>
                                </li>
                            </ul>
                            <ul style="width: 220px;">
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_90_nofloat">
                                            <label class="control-label">
                                                Observação Aparelho
                                            </label>
                                            <asp:TextBox ID="tbObs_aparelho" class="form-control" runat="server" Width="160px" Height="164px"
                                                TextMode="MultiLine"></asp:TextBox>
                                        </div>
                                    </div>
                                </li>
                            </ul>

                            <asp:Panel ID="PnBackup" runat="server">
                                <p>
                                    Informações de Backup
                                </p>
                                <ul>
                                    <center>
                                        <li>
                                            <div class="form-group">
                                                <div class="div_largura_80">
                                                    <label class="control-label">
                                                        Prop. do Estoque
                                                    </label>
                                                    <asp:TextBox ID="tbProp_Estoque" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="form-group">
                                                <div class="div_largura_80">
                                                    <label class="control-label">
                                                        Ordem de Serviço
                                                    </label>
                                                    <asp:TextBox ID="tbOrdem_Serviço" class="form-control" runat="server"></asp:TextBox>
                                                </div>
                                            </div>
                                        </li>
                                        <li>
                                            <div class="form-group">
                                                <div class="div_largura_60">
                                                    
                                                    <label class="control-label">
                                                        Emissão
                                                    </label>
                                                    <asp:TextBox ID="tbEmissão" class="form-control" runat="server" Columns="10" Width="80px"
                                                        Enabled="true"></asp:TextBox>
                                                    
                                                    <asp:ImageButton ID="image6" runat="server" ImageUrl="~/images/Calendar.png" />
                                                </div>
                                            </div>
                                        </li>
                                    </center>
                                </ul>
                            </asp:Panel>
                            <asp:Panel ID="PnSucata" runat="server">
                                <div id="Div2" class="divinterna_link" style="height: 90px;">
                                    <p>
                                        Informações de Sucata
                                    </p>
                                    <ul>
                                        <center>
                                            <li>
                                                <div class="form-group">
                                                    <div class="div_largura_80">
                                                        <label class="control-label">
                                                            Chamado Retirada
                                                        </label>
                                                        <asp:TextBox ID="tbChamado_Retirada" class="form-control" runat="server"></asp:TextBox>
                                                    </div>
                                                </div>
                                            </li>
                                            <li>
                                                <div class="form-group">
                                                    <div class="div_largura_60">
                                                        
                                                        <label class="control-label">
                                                            Data Retirada</label>
                                                        <asp:TextBox ID="tbData_Retirada" class="form-control" runat="server" Columns="10"
                                                            Width="80px" Enabled="true"></asp:TextBox>
                                                        
                                                        <asp:ImageButton ID="image5" runat="server" ImageUrl="~/images/Calendar.png" />
                                                    </div>
                                                </div>
                                            </li>
                                        </center>
                                    </ul>
                                </div>
                                <br />
                            </asp:Panel>
                        </asp:Panel>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnPostFoto" EventName="Click" />
                        <asp:AsyncPostBackTrigger ControlID="cmbMarca" EventName="SelectedIndexChanged" />
                        <asp:AsyncPostBackTrigger ControlID="cmbNatureza" EventName="SelectedIndexChanged" />
                    </Triggers>
                </asp:UpdatePanel>
            </div>
            <div id="tab-3" class="tab-content">
                <br />
                <asp:UpdatePanel ID="UpnUsuario" runat="server">
                    <ContentTemplate>
                        <ul>
                            <center>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_90">
                                            <br />
                                            <label class="control-label">
                                                Usuário 
                                                <br />
                                                <a href="javascript:window.open('GestaoCadastroUsuario.aspx','Busca','width=692,height=900,scrollbars=1'); void(0)">
                                                    <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                        title="Novo" /></a>
                                                <a href="javascript:window.open('GestaoCadastroUsuario.aspx?codigo=' + <%=tb_user_code.Text %>,'Busca','width=692,height=900,scrollbars=1'); void(0)">
                                                    <img alt="mag" src="..\Icons\43.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                        title="Editar usuário" /></a>
                                                <asp:TextBox ID="tb_user_code" runat="server" Width="10px" Style="display: none"></asp:TextBox>
                                                <a href="javascript:window.open('GestaoPesquisarComHierarquia.aspx?table=USUARIOS&name=NOME_USUARIO&code=CODIGO&titulo=Usuário','Busca','width=530,height=400,scrollbars=1'); void(0)">
                                                    <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                        title="Procurar" /></a> <a href="javascript:RemoveUser(); void(0)">
                                                            <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                                title="Remover" /></a>
                                            </label>
                                            <br />
                                            <br />
                                            <asp:Label ID="tbUsuario" runat="server"></asp:Label>
                                            <asp:TextBox ID="tbUsuario_mirror" class="form-control" runat="server" Style="display: none"></asp:TextBox>
                                </li>
                                <li>
                                    <div id="div_termos" runat="server">
                                        <label class="control-label">
                                            Termos de<br />
                                            Responsabilidade
                                            <br />
                                            <a href="javascript:window.open('GestaoCadastroTermoResp.aspx?codigo_linha=<%= Request.QueryString("codigo")  %>&codigo_usuario=<%= tb_user_code.Text  %>','Termo','width=540,height=560,scrollbars=1'); void(0);">
                                                <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                    title="Novo" /></a><a href="javascript:window.open('GestaoTermoResp.aspx?codigo_linha=<%= Request.QueryString("codigo")  %>&codigo_usuario=<%= tb_user_code.Text  %>','Busca','width=840,height=580,scrollbars=1'); void(0);">
                                                        <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                            title="Procurar" /></a>
                                    </div>
                                    </label>
                                </li>

                                <asp:Panel ID="pnCCusto_Not_Editable" runat="server" Style="font-size: 12px;">
                                    <li>

                                        <asp:Button ID="btnAddUser" runat="server" Text="AddCCustoList" Style="display: none" />
                                        <p style="margin-top: 10px;">
                                            <b>Centro de Custo</b>
                                            <br />
                                            <asp:Label ID="lbCCusto_code" runat="server" ForeColor="red"></asp:Label>
                                            &nbsp
                                        <asp:Label ID="lbCCusto" runat="server" ForeColor="red"></asp:Label>
                                        </p>
                                    </li>

                                </asp:Panel>
                                <li>
                                    <div class="form-group">
                                        <div class="div_largura_60" style="width: 102%;">
                                            <label class="control-label">
                                                Limite de Uso (0=sem limite)
                                            </label>
                                            <asp:TextBox ID="tbLimite_Uso" class="form-control" runat="server" Width="80px"></asp:TextBox>
                                        </div>
                                    </div>
                                </li>
                            </center>
                        </ul>
                        <br />
                        <br />
                        <br />
                        <ul>
                            <li>
                                <center>
                                    <asp:Image runat="server" ID="noImage" ImageUrl="..\images\noPhotoAvailable.jpg"
                                        Style="border: 1px solid #CCCCCC; border-radius: 6px; width: 140px; vertical-align: bottom; margin-left: -30px;" />
                                    <asp:Image runat="server" ID="foto" Style="border: 1px solid #CCCCCC; border-radius: 6px; width: 140px; vertical-align: bottom; margin-left: -30px;"
                                        Visible="false" />
                                </center>
                                <br />

                                <asp:Panel ID="pnCCusto_Editable" runat="server" Visible="false" Width="660px" Style="text-align: center; border: 1px solid silver; border-radius: 6px;">
                                    <a href="javascript:window.open('GestaoCadastroGrupos.aspx','Busca','width=800,height=300,scrollbars=1'); void(0)">
                                        <img alt="mag" src="..\Icons\pencil_48.png" style="border: 0; width: 18px; margin-top: 5px; vertical-align: bottom;"
                                            title="Novo"></img></a> C.Custo <a href="javascript:AbrePesquisaCCustos()">
                                                <img alt="mag" src="..\Icons\Add_48.png" style="border: 0; width: 18px; vertical-align: bottom;"
                                                    title="Incluir" /></a>
                                    <center style="border: 0px none;">
                                        <asp:GridView ID="GvCCustos" runat="server" BackColor="White" BorderColor="white"
                                            BorderStyle="None" CellPadding="4" EnableModelValidation="True"
                                            ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="640px"
                                            RowStyle-Height="35px" HeaderStyle-Height="20px" ShowHeader="False"
                                            Style="margin-top: 5px;">
                                            <Columns>
                                                <asp:TemplateField Visible="false">
                                                    <ItemTemplate>
                                                        <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                                                        <input type="hidden" id="chkhidden" runat="server" value='<%# Eval("CODIGO") %>' />
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="CODIGO" HeaderText="Código" />
                                                <asp:BoundField DataField="DESCRICAO" HeaderText="Centro de Custo" />
                                                <asp:ButtonField Text="<img src='../Icons/cancel_48.png' style='border-width:0;width:20px;height:20px' />"
                                                    ImageUrl="~/Icons/cancel_48.png" CommandName="Excluir">
                                                    <ControlStyle Height="10px" Width="20px" />
                                                    <ItemStyle Width="15px" />
                                                </asp:ButtonField>
                                            </Columns>
                                            <AlternatingRowStyle BackColor="White" />
                                            <FooterStyle BackColor="#CCCC99" />
                                        </asp:GridView>
                                    </center>

                                </asp:Panel>
                                <asp:TextBox ID="tbCCusto_codes" runat="server" Width="60px" Style="display: none"></asp:TextBox>
                                <asp:TextBox ID="tbCCusto" runat="server" Width="120px" Style="display: none"></asp:TextBox>
                                <asp:Label ID="Label3" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                                <asp:Button ID="btnAddCCusto" runat="server" Text="AddCCustoList" Style="display: none" />
                                </asp:Panel>
                            </li>

                        </ul>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </div>
        <!-- Inicio do popup de Cadastro de C.Custo -->
        <div id="divCadastroCCusto" style="display: none;">
            <div title="Cadastro de C.Custo" class="divCadastroCCusto">
                <asp:UpdatePanel ID="upGrupos" runat="server">
                    <ContentTemplate>
                        <ul>
                            <li style="width: 240px">Grupo<br />
                                <asp:TextBox ID="tbGrupo" runat="server" Width="100px"></asp:TextBox>
                                <asp:Button ID="btBuscaGrupos" runat="server" Text="Pesquisar" />
                            </li>
                            <li style="width: 220px; padding-left: 10px;">
                                <asp:PlaceHolder ID="phGrupos" runat="server" Visible="false">Percentual Rateio %
                                <br />
                                    <asp:TextBox ID="tbPercent" Width="140px" runat="server" onKeyPress="return(MascaraMoeda(this,'.',',',event))"
                                        Text="0"></asp:TextBox>
                                    <asp:Button ID="btCCusto" runat="server" Text="Inserir" />
                                </asp:PlaceHolder>
                            </li>
                        </ul>
                        <ul>
                            <li style="width: 240px">
                                <asp:Panel ID="pnlBucaGrupo" class="divBucaGrupo" runat="server" Visible="false">
                                    <asp:RadioButtonList ID="rbGrupos" runat="server" AutoPostBack="true">
                                    </asp:RadioButtonList>
                                </asp:Panel>
                            </li>
                            <li>
                                <asp:GridView ID="gvGrupos" runat="server" BackColor="White" BorderColor="#DEDFDE"
                                    BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                                    ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="210px">
                                    <Columns>
                                        <asp:BoundField DataField="Grupo" HeaderText="Grupo" />
                                        <asp:BoundField DataField="Percentual" HeaderText="%" />
                                        <asp:ButtonField ButtonType="Link" CommandName="Excluir" HeaderText="" Text="Excluir"
                                            ItemStyle-HorizontalAlign="Center" />
                                    </Columns>
                                    <AlternatingRowStyle BackColor="White" />
                                    <FooterStyle BackColor="#CCCC99" />
                                    <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                                    <RowStyle BackColor="#F7F7DE" />
                                    <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                                <br />
                                <asp:Label ID="MSGGrupos" runat="server" ForeColor="Red" Visible="false"></asp:Label>
                            </li>
                        </ul>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <input type="hidden" name="formIsValid" id="formIsValid" value="false" />
        </div>
    </form>
    <script type="text/javascript" language="javascript">

        function pageLoad() {
            fn_init();
        }


        function open_tab(tab) {
            if(tab == 1){
                $("#tab-2").removeClass("current");
                $("#tab_link2").removeClass("current");
                $("#tab-3").removeClass("current");
                $("#tab_link3").removeClass("current");

                $("#tab-1").addClass("current");
                $("#tab_link1").addClass("current");
            }
            if(tab == 2){
                $("#tab-1").removeClass("current");
                $("#tab_link1").removeClass("current");
                $("#tab-3").removeClass("current");
                $("#tab_link3").removeClass("current");

                $("#tab-2").addClass("current");
                $("#tab_link2").addClass("current");

            }
            if(tab == 3){
                $("#tab-1").removeClass("current");
                $("#tab_link1").removeClass("current");
                $("#tab-2").removeClass("current");
                $("#tab_link2").removeClass("current");

                $("#tab-3").addClass("current");
                $("#tab_link3").addClass("current");

            }
        }

        
    </script>
</body>
</html>
<%  AppIni.inicialize()%>