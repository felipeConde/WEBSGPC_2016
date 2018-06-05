<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoRel_ExtratoFixo.aspx.vb"
    Inherits="GestaoRel_ExtratoFixo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Extrato de Linha Fixa</title>
    <!-- Add the Kendo styles to the in the head of the page... -->
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/jquery-ui-1.8.1.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/ui.jqgrid.css" />
    <script src="../jqGrid/js/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../jqGrid/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <link href="../CSS/CL.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="../js/CL.js"></script>

    <script language="javascript" src="../JqGrid_new/js/jquery-1.11.0.min.js" type="text/javascript"></script>
    <link href="../js/bootstrap-3.3.5-dist/css/bootstrap.min.css" rel="stylesheet" type="text/css" />
     <script src="../js/bootstrap-3.3.5-dist/js/bootstrap.min.js" type="text/javascript"></script>

    <script type="text/javascript">


        function incluirBusca(nome, codigo, tabela) {

            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");
            nome = nome.replace("?", " ");

            if (tabela == "USUARIOS") {
                document.getElementById('tbUsuario').value = nome;
                document.getElementById('tb_user_code').value = codigo;
                document.getElementById('tbLinha').value = "";
                __doPostBack('tb_user_code', '');
            }
            if (tabela == "LINHAS") {
                document.getElementById('tbLinha').value = codigo;
            }

        }

        function GestaoPesquisarComHierarquia() {

            var user_code = document.getElementById('tb_user_code').value
            window.open("GestaoPesquisarComHierarquia.aspx?table=LINHAS&name=NUM_LINHA&code=CODIGO_LINHA&celular=N&titulo=Linhas Fixas&value=" + user_code + "", 'Busca', 'width=330,height=400,scrollbars=1');
            void (0);
        }

        function LimparUsuario() {
            document.getElementById('tbUsuario').value = "";
            document.getElementById('tbUsuario_mirror').value = "";
            document.getElementById('tb_user_code').value = "";
        }

        function LimparCelular() {
            document.getElementById('tbLinha').value = "";
        }

    </script>
</head>
<body class="pagina_interna">
    <form id="form2" runat="server">
    <div class="pagina_interna_div">
        <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
            EnableScriptLocalization="true">
        </asp:ScriptManager>
        <h1>
            Extrato de Linha Fixa
        </h1>
        <center>
            <ul>
                <asp:Panel ID="PnGerencial" runat="server">
                <span style="display:none" >
                    <h2>
                        Filtro de Usuário</h2>
                    <li>
                        <asp:TextBox ID="tb_user_code" runat="server" Width="100px" Style="display: none;
                            width: 50px;" AutoPostBack="true"></asp:TextBox>
                        <asp:TextBox ID="tbUsuario" runat="server" Width="100px" Style="display: none;"></asp:TextBox>
                        <asp:TextBox ID="tbUsuario_mirror" runat="server" Width="100px" Style="width: 120px;"
                            Enabled="false"></asp:TextBox>
                        <a href="javascript:window.open('GestaoPesquisarComHierarquia.aspx?table=USUARIOS&name=NOME_USUARIO&code=CODIGO&titulo=Usuário&strquery=and exists (select 0 from linhas_moveis lm where lm.codigo_usuario=p1.codigo ) order by p1.nome_usuario','Busca','width=330,height=400,scrollbars=1'); void(0)">
                            <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                title="Procurar" /></a> <a href="javascript:LimparUsuario();">
                                    <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                        title="Limpar" /></a> </li>
                    <li>
                        <br />
                        Ao selecionar um usuário, a busca é feita somente em suas respectivas
                        linhas. </li>
                    <br />
                    </span>
                    <h2>
                        Linha</h2>
                    <li>
                        <asp:TextBox ID="tbLinha" runat="server" Width="120px" Style="width: 100px;"></asp:TextBox>
                        <a href="javascript:GestaoPesquisarComHierarquia();">
                            <img alt="mag" src="..\Icons\search_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                title="Procurar" /></a> <a href="javascript:LimparCelular();">
                                    <img alt="mag" src="..\Icons\cancel_48.png" style="border: 0; width: 21px; vertical-align: bottom;"
                                        title="Limpar" /></a> </li>
                </asp:Panel>
                <asp:Panel ID="PnUser_commom" runat="server" Visible="false">
                    <h2>
                        Minhas Linhas</h2>
                    <li>
                        <asp:RadioButtonList ID="rbUser_common_lines" runat="server">
                        </asp:RadioButtonList>
                    </li>
                </asp:Panel>
            </ul>
            <ul>
                <h2>
                    Vencimento da Fatura<br />
                </h2>
                <li>
                    <asp:DropDownList ID="cmbMes" runat="server" Width="90px" DataTextField="DESCRICAO"
                        DataValueField="CODIGO">
                    </asp:DropDownList>
                    <asp:DropDownList ID="cmbAno" runat="server" Width="60px">
                    </asp:DropDownList>
                </li>
                <br />
                <li>
                <span style="display:none">
                    <asp:CheckBox ID="chklinhas" runat="server" />
                    Exibir coluna com número da linha </li>
                </span>
               
            </ul>
            
        </center>
        <center>
               <div style="text-align:left;display:inline-block">
        <div class="btn-group" runat="server" id="divTipoRel" s>
                              <button type="button" class="btn btn-primary" data-toggle="dropdown" style="width:100px">Opções</button>
                              <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" style="height:31px;">
                                <span class="caret"></span>
                              </button>
                              <ul class="dropdown-menu" role="menu" style="padding:10px;width:250px">                               
                             
                <br />
               
                <li style="margin-top:12px;">
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
        <br /><br />
        <h2>
                  Gerar Relatório
                </h2>
         <asp:Button ID="btnExcel" runat="server" Text="Excel" Style="width: 70px; height: 39px;" />
                    <asp:Button ID="btnHtml" runat="server" Text="HTML" Style="width: 70px; height: 39px;" />
        </center>
    </div>
    </form>
</body>
</html>
