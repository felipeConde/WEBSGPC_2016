<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoRel_ExtratoCelular.aspx.vb"
    Inherits="GestaoRel_ExtratoCelular" MasterPageFile="~/Cadastros.master" %>

<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <head id="Head1">
        <title>Extrato de Linha Móvel</title>

        <script type="text/javascript">


            function incluirBusca(nome, codigo, tabela) {

                nome = nome.replace("?", " ");
                nome = nome.replace("?", " ");
                nome = nome.replace("?", " ");

                if (tabela == "USUARIOS") {
                    document.getElementById('tbUsuario').value = nome;
                    document.getElementById('tb_user_code').value = codigo;
                    document.getElementById('tbCelular').value = "";
                    __doPostBack('tb_user_code', '');
                }
                if (tabela == "LINHAS") {
                    document.getElementById('tbCelular').value = codigo;
                }

            }

            function GestaoPesquisarComHierarquia() {

                window.open("GestaoPesquisarComHierarquia.aspx?table=LINHAS&name=NUM_LINHA&code=CODIGO_LINHA&celular=S&titulo=Celulares Cadastrados", 'Busca', 'width=550,height=400,scrollbars=1');
                void (0);
            }

            function LimparUsuario() {
                document.getElementById('tbUsuario').value = "";
                document.getElementById('tbUsuario_mirror').value = "";
                document.getElementById('tb_user_code').value = "";
            }

            function LimparCelular() {
                document.getElementById('tbCelular').value = "";
            }



        </script>
    </head>
    <body class="pagina_interna">
        <form id="form1" runat="server">
            <div class="pagina_interna_div">
                <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
                    EnableScriptLocalization="true">
                </asp:ScriptManager>
                <h1>Extrato de Dispositivo Móvel
                </h1>
                <center>
                <ul style="height: 120px">
                    <asp:Panel ID="PnGerencial" runat="server">
                        <h2>Celular
                            <a href="javascript:GestaoPesquisarComHierarquia();">
                                <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                    title="Procurar" /></a> <a href="javascript:LimparCelular();">
                                        <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                            title="Limpar" /></a>

                        </h2>
                        <li>
                            <div class="form-group">
                                <div class="div_largura_90">
                                    <label class="control-label">
                                        </label>
                                    <asp:TextBox ID="tbCelular" runat="server" Style="width: 180px;"></asp:TextBox>
                                </div>
                            </div>
                        </li>
                    </asp:Panel>
                    <asp:Panel ID="PnUser_commom" runat="server" Visible="false">
                        <h2>Meus Celulares</h2>
                        <li>
                            <asp:RadioButtonList ID="rbUser_common_lines" runat="server" AutoPostBack="true">
                            </asp:RadioButtonList>
                        </li>
                    </asp:Panel>
                </ul>
                <ul style="height: 100px">
                    <h2>Vencimento da Fatura<br />
                    </h2>
                    <li>
                        <asp:DropDownList ID="cmbMes" runat="server" Width="100px" DataTextField="DESCRICAO"
                            DataValueField="CODIGO">
                        </asp:DropDownList>
                        <asp:DropDownList ID="cmbAno" runat="server" Width="70px">
                        </asp:DropDownList>
                    </li>
                    <br />
                    <li>
                        <span style="display: none;">
                            <asp:CheckBox ID="chklinhas" runat="server" />
                    Exibir coluna com número celular </li>
                    <br />

                    </span>
                
                </ul>



                <center>
                    <div style="text-align: left; display: inline-block">
                        <div class="btn-group" runat="server" id="divTipoRel" s>
                            <button type="button" class="btn btn-primary" data-toggle="dropdown" style="width: 100px">Opções</button>
                            <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" style="height: 31px;">
                                <span class="caret"></span>
                            </button>
                            <ul class="dropdown-menu" role="menu" style="padding: 10px; width: 250px">

                                <br />

                                <li style="margin-top: 12px;">
                                    <cc1:CalendarExtender ID="CalendarExtender1" runat="server" TargetControlID="tbDt_ativ"
                                        Format="dd/MM/yyyy" PopupButtonID="Image1" Animated="true" BehaviorID="calendar1">
                                    </cc1:CalendarExtender>
                                    &nbsp; Data Início
                    <asp:ImageButton ID="image1" runat="server" ImageUrl="~/images/Calendar.png" />
                                    <cc1:CalendarExtender ID="CalendarExtender2" runat="server" TargetControlID="tbDt_des"
                                        Format="dd/MM/yyyy" PopupButtonID="Image2" Animated="true" BehaviorID="calendar2">
                                    </cc1:CalendarExtender>
                                    &nbsp;&nbsp; Data Fim
                    <asp:ImageButton ID="image2" runat="server" ImageUrl="~/images/Calendar.png" />
                                    <br />
                                    <asp:TextBox ID="tbDt_ativ" runat="server" Columns="10" MaxLength="10" Width="85px"
                                        Enabled="true"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="tbDt_ativ_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="tbDt_ativ" UserDateFormat="DayMonthYear">
                                    </cc1:MaskedEditExtender>
                                    <asp:TextBox ID="tbDt_des" runat="server" Columns="10" MaxLength="10" Width="85px"
                                        Enabled="true"></asp:TextBox>
                                    <cc1:MaskedEditExtender ID="tbDt_des_MaskedEditExtender" runat="server" Mask="99/99/9999"
                                        MaskType="Date" TargetControlID="tbDt_des" UserDateFormat="DayMonthYear">
                                    </cc1:MaskedEditExtender>
                                </li>
                                <h2>Filtrar somente cobranças no intervalo acima</h2>


                            </ul>
                        </div>
                    </div>
                    <div class="row"></div>
                    <br />
                    <br />
                    <h2>Gerar Relatório
                    </h2>
                    <asp:Button ID="btnExcel" runat="server" Text="Excel" Style="width: 70px; height: 39px;" />
                    <asp:Button ID="btnHtml" runat="server" Text="HTML" Style="width: 70px; height: 39px;" />
                </center>
            </div>
            <script>

            
            
                $('#form1').data('formValidation',null);

                $('#form1').find('[name="tbCelular"]').intlTelInput({
                    utilsScript: '../js/intl-tel-input/lib/libphonenumber/build/utils.js',
                    autoPlaceholder: false,
                    nationalMode:true,
                    defaultCountry:  'br',
                    preferredCountries: ['br','us']
                });

                $('#form1').find('[name="tbCelular"]').intlTelInput("selectCountry", "br");


                $('#form1').formValidation({
                    excluded: [':disabled'],
                    framework: 'bootstrap',
                    icon: {
                        valid: 'glyphicon glyphicon-ok',
                        invalid: 'glyphicon glyphicon-remove',
                        validating: 'glyphicon glyphicon-refresh'
                    },
                    fields: {
                        <%=tbCelular.UniqueID%>: {
                            validators: {
                                callback: {
                                    message: 'Número de telefone inválido',
                                    callback: function (value, validator, $field) {
                                        return value === '' || $field.intlTelInput('isValidNumber');
                                    }
                                }
                            }
                        }
                        
                    }
                })
                .on('err.validator.fv', function(e, data) {

                    

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
            


            </script>
        </form>
    </body>
</div>
</asp:Content>
