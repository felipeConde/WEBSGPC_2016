<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoCadastroUsuario.aspx.vb"
    Inherits="GestaoCadastroUsuario" MasterPageFile="~/Cadastros.master" %>

<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <head id="Head1">
        <title>Cadastro do Gestão de Usuários</title>
    </head>
    <style>
        .file-upload {
            text-rendering: auto;
            letter-spacing: normal;
            word-spacing: normal;
            text-indent: 0px;
            text-shadow: none;
            align-items: flex-start;
            box-sizing: border-box;
            -webkit-font-smoothing: antialiased;
            margin: 0;
            font: inherit;
            box-sizing: border-box;
            font-family: inherit;
            font-weight: 400;
            text-align: center;
            vertical-align: middle;
            touch-action: manipulation;
            background-image: none;
            white-space: nowrap;
            padding: 10px;
            font-size: 13px;
            line-height: 1.42857143;
            border-radius: 2px;
            color: #ffffff;
            background-color: #2196f3;
            position: relative;
            cursor: pointer;
            display: inline-block;
            overflow: hidden;
            -webkit-user-select: none;
            border: 0;
            text-transform: uppercase;
        }

        .file-upload {
            -webkit-writing-mode: horizontal-tb;
        }

            .file-upload:after {
                box-sizing: border-box;
            }

            .file-upload:not(.file-upload-link) {
                box-shadow: 0 2px 5px rgba(0, 0, 0, 0.16), 0 2px 10px rgba(0, 0, 0, 0.12);
            }

            .file-upload, .file-upload:active, .file-upload:hover {
                outline: none !important;
                -webkit-tap-highlight-color: rgba(0, 0, 0, 0) !important;
            }

                .file-upload:hover:hover,
                .file-upload:focus:hover,
                .file-upload:focus:hover,
                .file-upload:active:hover,
                .open > .file-upload:hover {
                    color: #ffffff;
                    background-color: #2196f3;
                    border-color: transparent;
                }

                /* The button size */

                .file-upload:before, .file-upload:after {
                    -webkit-box-sizing: border-box;
                    -moz-box-sizing: border-box;
                    box-sizing: border-box;
                }

                .file-upload input {
                    /* Loses tab index in webkit if width is set to 0 */
                    width: 2px;
                    height: 2px;
                    opacity: 0;
                    filter: alpha(opacity=0);
                }
    </style>
    <script type="text/javascript" language="javascript">


        function ConfirmaInsert() {

            var nome = $("#tbNome").val();

            var txt;
            var r = confirm("Já existe um usuário com este nome! Deseja realmente incluir novo?");
            if (r == true) {
                //alert('OK!');
                __doPostBack('btConfimaNovo', '');

            } else {

            }

        }

        jQuery(function ($) {

            $("#tbCEP").mask("99999-999");
            $("#tbCPF").mask("999.999.999-99");
            $("#tbDiasExpiraSenha").mask("99999");
            $(":input").attr("disabled", true);
            $(":select").attr("disabled", true);
        });

        $("#busca").lightbox_me();

        function incluirBusca(nome, codigo, tabela) {

            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");

            if (tabela == "GRUPOS") {
                document.getElementById('tbCCUsuario').value = codigo;
                document.getElementById('tbCCUsuario_mirror').value = codigo;
            }
            if (tabela == "USUARIOS") {
                document.getElementById('tbUsuario').value = nome;
                document.getElementById('tbUsuario_mirror').value = nome;
                document.getElementById('tb_user_code').value = codigo;
            }
            if (tabela == "RAMAIS") {
                document.getElementById('tbRamal').value = codigo;
                document.getElementById('tbRamal_mirror').value = codigo;
                __doPostBack('BtnChangeRamal', '');
            }
            //ExecutarPostBack();
        }

        function ExecutarPostBack() {
            __doPostBack('btnPostBack', '');
        }

        function btngerasenha_onclick() {
            var sql = prompt("Entre com tamanho da senha", "6", "");

            if (sql == "") {
                return;
            }

            document.getElementById('txtsenha').focus();
            document.getElementById('txtsenha').value = "aguarde...";

            var myConn = new XHConn();
            if (!myConn) alert("XMLHTTP not available. Try a newer/better browser.");
            var fnWhenDone = function (oXML) {
                //sql=jMid(oXML.responseText,11,oXML.responseText.length);
                sql = oXML.responseText;
                var s = "";
                for (i = 0; i < sql.length; i++) {
                    if (sql.charAt(i) != 'x') {
                        s += sql.charAt(i);
                    }
                }
                document.getElementById('txtsenha').value = s;
            };
            myConn.connect("gerasenhav2.asp", "GET", "tamanhosenha=" + sql, fnWhenDone);
        }

        function CCUSTO_RAMAL() {
            $("#dialog-confirm").dialog({
                resizable: false,
                //height: auto,
                modal: true,
                buttons: {
                }
            });
        }

        function RM_CEL() {
            $("#dialog-confirm2").dialog({
                resizable: false,
                height: 180,
                modal: true,
                buttons: {
                }
            });
        }

        function YES_CCUSTORAMAL() {
            __doPostBack('btnCCUSTO_RAMAL', '');
        }

        function NO_CCUSTORAMAL() {
            __doPostBack('btnCCUSTO_RAMAL_NO', '');
        }

        function YES_RM_CEL() {
            __doPostBack('btnRM_YES_CEL', '');
        }

        function NO_RM_CEL() {
            __doPostBack('btnRM_NO_CEL', '');
        }

        function AbrePesquisaCCustos() {

            var page = "GestaoPesquisarComHierarquia.aspx?table=GRUPOS&titulo=Centro de Custo&name=CODIGO || ' - ' || p1.NOME_GRUPO&code=CODIGO"
            window.open(page, "_blank", "top=0,left=20,width=330,height=400,scrollbars=true,resizable=false, scrollbars=1");

        }

        function RemovetbRamais() {
            document.getElementById('tbRamal').value = "SEM RAMAL";
            document.getElementById('tbRamal_mirror').value = "SEM RAMAL";
        }

        function RemoveUser() {
            document.getElementById('tbUsuario').value = "";
            document.getElementById('tbUsuario_mirror').value = "";
            document.getElementById('tb_user_code').value = "";

        }

        function CheckAllEmp(Checkbox) {
            var GvRelatorios = document.getElementById("<%=GvRelatorios.ClientID %>");
            for (i = 1; i < GvRelatorios.rows.length; i++) {
                GvRelatorios.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }

        function PostaFoto() {
            __doPostBack('btnPostFoto', '');

        }

    </script>
    <div class="container">
        <div class="card">
            <div class="block-header">
            </div>
            <div class="card-header">
                <h2>Cadastro de Usuários</h2>
            </div>
            <div class="card-body card-padding">
                <div class="row">
                    <div class="col-xs-4">
                        <center>
                        <asp:Image runat="server" ID="noImage" ImageUrl="img\noPhotoAvailable.jpg"
                            Style="border: 1px solid #CCCCCC; border-radius: 6px; width: 140px;" />
                        <asp:Image runat="server" ID="foto" Style="border: 1px solid #CCCCCC; border-radius: 6px; width: 140px; vertical-align: bottom; margin-left: -30px;"
                            Visible="false" />
                    <br />
                    <br />
                        <label class="file-upload">
                            <span><strong>Selecionar Imagem</strong></span>
                            <asp:FileUpload ID="UploadArquivo" runat="server" onchange="PostaFoto();"></asp:FileUpload>
                            <asp:Button ID="btnPostFoto" runat="server" Text="postFoto" Style="display: none;" />
                        </label>
                        <br />
                        <asp:ImageButton ID="btnRemove_foto" runat="server" src="..\Icons\cancel_48.png" ToolTip="Remover foto"  />
                        </center>
                    </div>
                    <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbCodigo" runat="server" Width="100px" Enabled="false"
                        Style="display: none;"></asp:TextBox>

                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="tbNome">Nome</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Nome" ID="tbNome" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="tbCPF">CPF</label>
                            <asp:TextBox class="form-control input-sm" placeholder="CPF" ID="tbCPF" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-8">
                        <div class="fg-line form-group">
                            <asp:PlaceHolder runat="server" ID="phCelulares" Visible="false">
                                <label for="lbCelular">Celulares</label><br />
                                <asp:Label runat="server" Text="" ID="lbCelular"></asp:Label>
                            </asp:PlaceHolder>
                        </div>
                    </div>
                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="tbEmail">E-mail</label>
                            <asp:TextBox class="form-control input-sm" placeholder="E-mail" ID="tbEmail" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="tbEmailSup">E-mail Supervisor</label>
                            <asp:TextBox class="form-control input-sm" placeholder="E-mail Supervisor" ID="tbEmailSup" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="tbCargo">Cargo</label>
                            <asp:TextBox class="form-control input-sm" placeholder=".Cargo" ID="tbCargo" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-4">
                        <div class="fg-line form-group">
                            <label for="cmbStatus">Status</label>
                            <a href="javascript:window.open('gestaostatususuarios.aspx','Busca','width=380,height=440,scrollbars=1'); void(0)">
                                <i class="zmdi zmdi-assignment zmdi-hc-lg"></i></a>
                            <asp:DropDownList  CssClass="chosen"  placeholder=".col-xs-3" ID="cmbStatus" runat="server" DataTextField="DESCRICAO"
                                DataValueField="CODIGO" />
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbEndereco">Logradouro</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Logradouro" ID="tbEndereco" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-2">
                        <div class="fg-line form-group">
                            <label for="cmbUF">UF</label>
                            <asp:DropDownList CssClass="chosen" placeholder="uf" ID="cmbUF" runat="server" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="cmbCidade">Cidade</label>
                            <asp:DropDownList CssClass="chosen" placeholder="Cidade" ID="cmbCidade" runat="server" DataTextField="DESCRICAO"
                                DataValueField="CODIGO">
                            </asp:DropDownList>
                        </div>
                    </div>

                    <div class="col-xs-2">
                        <div class="fg-line form-group">
                            <label for="tbNumero">Número</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Número" ID="tbNumero" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-2">
                        <div class="fg-line form-group">
                            <label for="tbComplemento">Complemento</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Complemento" ID="tbComplemento" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbCEP">CEP</label>
                            <asp:TextBox class="form-control input-sm" placeholder="CEP" ID="tbCEP" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbBairro">Bairro</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Bairro" ID="tbBairro" runat="server" Style="padding-right: 0px"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="cmbLocalidades">Localidade</label><a href="javascript:window.open('GestaoLocalidades.aspx','Busca','width=1020,height=480,scrollbars=1'); void(0)">
                                <i class="zmdi zmdi-assignment zmdi-hc-lg"></i></a>
                            <asp:DropDownList CssClass="chosen" placeholder="Localidade" ID="cmbLocalidades" runat="server"
                                DataTextField="DESCRICAO" DataValueField="CODIGO" AutoPostBack="true">
                            </asp:DropDownList>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbLogin">Login</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Login" ID="tbLogin" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <center>
                        <div class="col-xs-3">
                            <div class="fg-line form-group">
                                <label for="tbLogin">Senha WEB</label>
                                <label for="tbSenhaWEB"></label>
                                <br />
                                <asp:Label ID="LbPassword" runat="server" Font-Size="Smaller" ForeColor="#FF3300" />
                                <br />
                                <asp:LinkButton ID="btSenhaWEB" runat="server" Text="gerar senha" />
                            </div>
                        </div>
                        <div class="col-xs-3">
                            <br />
                            <div class="fg-line form-group">
                                <label class="checkbox checkbox-inline m-r-20">
                                  Acessa WEB &nbsp&nbsp <asp:CheckBox ID="cbAcessaWEB" runat="server"   ></asp:CheckBox>
                                    <i class="input-helper"></i>    
                                </label>                        
                                <asp:TextBox class="form-control input-sm" placeholder="Senha WEB" ID="tbSenhaWEB" runat="server" Style="display: none;"></asp:TextBox>
                            </div>
                        </div>
                            </center>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbDiasExpiraSenha">Dias p/Expirar Senha</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Dias p/Expirar Senha" ID="tbDiasExpiraSenha" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <div class="fg-line disabled">
                            <label for="tbBloqWEB">Bloqueio WEB</label>
                            <asp:TextBox class="form-control" ID="tbBloqWEB" runat="server" disabled></asp:TextBox>
                        </div>

                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line disabled">
                            <label for="tbExpiraSenha">Expiração Senha</label>
                            <asp:TextBox class="form-control" placeholder="Expiração Senha" ID="tbExpiraSenha" runat="server" disabled></asp:TextBox>
                            <input id="btnPostBack" style="display: none" />
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line disabled">
                            <label for="tbCCRamal">C.C Ramal</label>
                            <asp:TextBox class="form-control" placeholder="C.C Ramal" ID="tbCCRamal" runat="server" disabled></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line disabled">
                            <label class="checkbox checkbox-inline m-r-20">
                                <asp:CheckBox ID="chk_recursos_btn" runat="server" ToolTip="Quando gravar, atribuir o centro de custo desse usuário aos recursos vinculados ramal, celular e linha fixa." />
                                <i class="input-helper"></i>
                            </label>
                            <label for="tbCCUsuario">C.Custo Usuário</label><a id="busca" href="javascript:AbrePesquisaCCustos()">
                                <i class="zmdi zmdi-search zmdi-hc-lg"></i></a>
                            <asp:TextBox class="form-control" disabled placeholder="C.Custo Usuário" ID="tbCCUsuario" runat="server" Style="display: none"></asp:TextBox>
                            <asp:TextBox class="form-control" disabled placeholder="C.Custo Usuário" ID="tbCCUsuario_mirror" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-3">
                        <div class="fg-line disabled">
                            <label for="tbRamal">Ramal</label>
                            <a href="javascript:window.open('GestaoPesquisarComHierarquia.aspx?table=RAMAIS&name=NUMERO_A&code=NUMERO_A&titulo=Ramais Livres&strquery=and not exists (select 0 from usuarios where rml_numero_a=p1.NUMERO_A) ','Busca','width=330,height=400,scrollbars=1'); void(0)">
                                <i class="zmdi zmdi-search zmdi-hc-lg"></i></a><a href="javascript:RemovetbRamais(); void(0)">
                                    <i class="zmdi zmdi-delete zmdi-hc-lg"></i></a>

                            <br />
                            <asp:TextBox ID="tbRamal" class="form-control" disabled placeholder="Ramal" runat="server"></asp:TextBox>
                            <asp:TextBox class="form-control" disabled placeholder=".col-xs-3" ID="tbRamal_mirror" runat="server" Style="display: none"></asp:TextBox>
                            <%--  <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3"  ID="tbRamal" runat="server" Width="10px" Style="display: none"></asp:TextBox>--%>
                            &nbsp
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">

                            <label for="txtsenha">Senha Ramal</label>
                            <a href="#" onclick="btngerasenha_onclick()">gerar senha</a></span>
                            <asp:TextBox class="form-control input-sm" placeholder="Senha Ramal" ID="txtsenha" name="txtsenha" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbTelefone">Telefone</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Telefone" ID="tbTelefone" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbMatricula">Matrícula</label>
                            <asp:TextBox class="form-control input-sm" placeholder="Matrícula" ID="tbMatricula" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <div class="row">
                    <div class="col-xs-9">
                        <div class="fg-line form-group">
                            Recebe: Extrato de Celular &nbsp&nbsp
                            <label class="checkbox checkbox-inline m-r-20">
                                <asp:CheckBox ID="cbExtratoCelular" runat="server"></asp:CheckBox>
                                <i class="input-helper"></i>
                            </label>
                            Relatório Gerencial
                            <label class="checkbox checkbox-inline m-r-20">
                                <asp:CheckBox ID="cbRecebRelatorio" runat="server"></asp:CheckBox>
                                <i class="input-helper"></i>
                            </label>
                            Extrato de Ramal 
                            <label class="checkbox checkbox-inline m-r-20">
                                <asp:CheckBox ID="cbRecebeEmail" runat="server"></asp:CheckBox>
                                <i class="input-helper"></i>
                            </label>
                        </div>
                    </div>
                    <div class="col-xs-3">
                        <div class="fg-line form-group">
                            <label for="tbUsuario">Responsável</label>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tb_user_code" runat="server" Width="10px" Style="display: none"></asp:TextBox>
                            <a href="javascript:window.open('GestaoPesquisarComHierarquia.aspx?table=USUARIOS&name=NOME_USUARIO&code=CODIGO&titulo=Responsável','Busca','width=330,height=400,scrollbars=1'); void(0)">
                                <i class="zmdi zmdi-search zmdi-hc-lg"></i></a><a href="javascript:RemoveUser(); void(0)">
                                    <i class="zmdi zmdi-delete zmdi-hc-lg"></i></a>
                            <br />
                            <asp:TextBox class="form-control input-sm" placeholder="Responsável" ID="tbUsuario" runat="server" disabled></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder="Responsável" ID="tbUsuario_mirror" runat="server" disabled
                                Style="display: none"></asp:TextBox>
                        </div>
                    </div>
                </div>
                <asp:Panel ID="PnRelatorios" runat="server" ScrollBars="Vertical" Width="220px" Height="150px"
                    BorderColor="Silver" BorderStyle="Groove" BorderWidth="1px">
                    <asp:GridView ID="GvRelatorios" runat="server" BackColor="White" BorderColor="#DEDFDE"
                        BorderStyle="None" BorderWidth="1px" CellPadding="4" EnableModelValidation="True"
                        ForeColor="Black" GridLines="Vertical" AutoGenerateColumns="False" Width="202px"
                        Height="160px" RowStyle-Height="20px" HeaderStyle-Height="10px">
                        <Columns>
                            <asp:TemplateField ItemStyle-Width="10px">
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                                    <asp:CheckBox ID="chkRelatorios" runat="server"></asp:CheckBox>
                                    <input type="hidden" id="chkhidden" runat="server" value='<%# Eval("CODIGO") %>' />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="DESCRICAO" HeaderText="Menu do Usuário" />
                        </Columns>
                        <AlternatingRowStyle BackColor="White" />
                        <FooterStyle BackColor="#CCCC99" />
                        <HeaderStyle BackColor="#6B696B" Font-Bold="True" ForeColor="White" />
                        <PagerStyle BackColor="#F7F7DE" ForeColor="Black" HorizontalAlign="Right" />
                        <RowStyle BackColor="#F7F7DE" />
                        <SelectedRowStyle BackColor="#CE5D5A" Font-Bold="True" ForeColor="White" />
                    </asp:GridView>
                    <br />
                </asp:Panel>

                <asp:Button ID="btnCCUSTO_RAMAL" runat="server" Text="SIM" Style="display: none"
                    CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
                <asp:Button ID="btnCCUSTO_RAMAL_NO" runat="server" Text="NÃO" Style="display: none"
                    CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
                <asp:Button ID="btnRM_YES_CEL" runat="server" Text="SIM" Style="display: none" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
                <asp:Button ID="btnRM_NO_CEL" runat="server" Text="NÃO" Style="display: none" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />
                <asp:Button ID="BtnChangeRamal" runat="server" Text="SIM" Style="display: none" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only" />

                <asp:Panel runat="server" ID="PnSulamerica" Visible="False">
                    &nbsp&nbsp&nbsp&nbsp Matricula Supervisor
                    <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbMatriculaSuperv" runat="server" Width="120px"></asp:TextBox>
                    <div style="width: 200px; float: left;">
                        Data Admissão
                        <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbDtAdmissao" runat="server" Columns="10" Width="80px"
                            Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="image5" runat="server" ImageUrl="~/images/Calendar.png" />
                        <br />
                        Data Desliga . &nbsp;
                        <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbDtDesligamento" runat="server" Columns="10"
                            Width="80px" Enabled="true"></asp:TextBox>
                        <asp:ImageButton ID="image6" runat="server" ImageUrl="~/images/Calendar.png" />

                        <center>
                        <li style="width: 460px;">&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp &nbsp&nbsp&nbsp&nbsp&nbsp&nbsp&nbsp
                        VICE &nbsp&nbsp&nbsp DIR &nbsp&nbsp&nbsp SUPTE &nbsp&nbsp GER &nbsp&nbsp&nbsp&nbsp
                        SEC &nbsp&nbsp&nbsp NUC
                        <br />
                            Hierarquia
                        <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbVICE" runat="server" Width="40px"></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbDIR" runat="server" Width="40px"></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="TbSUPTE" runat="server" Width="40px"></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbGER" runat="server" Width="40px"></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbSEC" runat="server" Width="40px"></asp:TextBox>
                            <asp:TextBox class="form-control input-sm" placeholder=".col-xs-3" ID="tbNUC" runat="server" Width="40px"></asp:TextBox>
                        
                    </center>
                </asp:Panel>
            </div>
            <div class="row" style="display:none">
                <center>
                    <asp:LinkButton ID="btGravar" runat="server" Text="Gravar" class="btn btn-primary" />
                    <asp:LinkButton ID="btExcluir" runat="server" Text="Excluir" Enabled="False"
                        class="btn btn-primary" />
                    <asp:LinkButton ID="btCategoria" runat="server" Text="Categoria" Enabled="False"
                        class="btn btn-primary" Style="display: none" />
                    <asp:LinkButton ID="btCategoriaNova" runat="server" Text="Categoria" Enabled="False"
                        class="btn btn-primary" />
                    <asp:LinkButton ID="btRelatorio" runat="server" Text="Relatórios" class="btn btn-primary" />
                    <asp:LinkButton ID="btMenus" runat="server" Text="Menus" class="btn btn-primary" />
                    <asp:LinkButton ID="btHistorico" runat="server" Text="Histórico" Enabled="False"
                        class="btn btn-primary" />
                    </center>
                <br />
                <br />
            </div>
            <div id="dialog-confirm" title="Atualização de Recursos" style="display: none">
                <p>
                    <center>
                    <%
                Dim ramal_list As New List(Of AppGeneric)
                Dim celphoneList As New List(Of AppGeneric)
                Dim line_list As New List(Of AppGeneric)

                ramal_list = _dao_commons.GetGenericList("", "numero_a", "numero_a", "ramais", "", "and numero_a=(select rml_numero_a from usuarios where codigo='" & Request.QueryString("codigo") & "')")
                celphoneList = _dao_commons.GetGenericList("", "num_linha", "num_linha", "linhas", "", "and codigo_linha in (select codigo_linha from linhas_moveis where codigo_usuario='" & Request.QueryString("codigo") & "')")
                line_list = _dao_commons.GetGenericList("", "num_linha", "num_linha", "linhas", "", "and codigo_usuario='" & Request.QueryString("codigo") & "'")

                    %>
                Deseja realmente atribuir o centro de custo <b>
                    <%= tbCCUsuario.Text%></b> aos recursos abaixo ?
                <br />
                    <br />
                    <%                
                For Each item As AppGeneric In ramal_list
                    Response.Write("Ramal do usuário: <b>" & item.Codigo.ToString & "</b><br />")
                Next

                For Each item As AppGeneric In celphoneList
                    Response.Write("Celular do usuário: <b>" & item.Codigo.ToString & "</b><br />")
                Next

                For Each item As AppGeneric In line_list
                    Response.Write("Linha do usuário: <b>" & item.Codigo.ToString & "</b><br />")
                Next
                    %>
                    <br />
                    <br />
                    <asp:Button ID="btn_YES" runat="server" Text="SIM" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                        OnClientClick="YES_CCUSTORAMAL();" />
                    <asp:Button ID="btn_NO" runat="server" Text="NÃO" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                        OnClientClick="NO_CCUSTORAMAL();" />
                </center>
            </div>
            <div id="dialog-confirm2" title="Usuários Excluídos" style="display: none">
                <p>
                    <center>
                    Deseja desvincular esse usuário ao celular antes de remove-lo ?
                <br />
                    <br />
                    <br />
                    <asp:Button ID="btn_YES_rmlCel" runat="server" Text="SIM" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                        OnClientClick="YES_RM_CEL();" />
                    <asp:Button ID="btn_NO_rmlCel" runat="server" Text="NÃO" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
                        OnClientClick="NO_RM_CEL();" />
                </center>
            </div>
            <asp:Button ID="btConfimaNovo" runat="server" Text="Gravar" Width="80px"
                Style="display: none;" />
        </div>
</asp:Content>
