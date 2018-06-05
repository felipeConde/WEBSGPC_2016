<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoRel_ExtratoFixoResult.aspx.vb" Inherits="GestaoRel_ExtratoFixoResult" %>

<%@ Register Assembly="System.Web.Extensions, Version=1.0.61025.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35"
    Namespace="System.Web.UI" TagPrefix="asp" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
    <title>Extrato de Linha Fixa</title>
    <!-- Add the Kendo styles to the in the head of the page... -->
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/jquery-ui-1.8.1.custom.css" />
    <link rel="stylesheet" type="text/css" media="screen" href="../jqGrid/css/ui.jqgrid.css" />
    <script src="../jqGrid/js/jquery-1.5.2.min.js" type="text/javascript"></script>
    <script src="../jqGrid/js/jquery.jqGrid.min.js" type="text/javascript"></script>
    <link href="../CSS/CL.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="text/javascript" src="../js/CL.js"></script>
    <script language="javascript" type="text/javascript">
        var total = 0.0
        function ConfirmParticular() {

            var answer = confirm("Autorizo o débito, através de folha de pagamento, referente as ligações telefônicas que eu apontei como particulares em meu relatório de ligações telefônicas decorrentes do aparelho celular e/ou ramal que me foi disponibilizado para o trabalho.");

            if (!answer) {
                return;
            }
            else {

                __doPostBack('btSavaParticularExec', '');


            }
        }

        function SomaParticular(marcado) {

            alert(marcado);
            if (marcado == 1) {

                alert("passou");

            }

        }

    </script>
</head>
<body class="reportBody">
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager" runat="server" EnableScriptGlobalization="true"
        EnableScriptLocalization="true">
    </asp:ScriptManager>
    <center>
        <div>
            <center>
                <div style="font-size: 12px;">
                    <%--                    Centro de Custo:
                    <asp:Label ID="lbGrupo" runat="server" Text=""></asp:Label>
                    <br />--%>
                    Linha:
                    <%= Request.QueryString("celular")%>
                    <br />
                    Usuário:
                    <asp:Label ID="lbUsuario" runat="server" Text=""></asp:Label>
                    <br />
                    Período:
                    <%= Request.QueryString("dataini")%>-
                    <%= Request.QueryString("datafim")%>
                    <br />
                    Vencimento da fatura:
                    <%= Request.QueryString("mes")%>/<%= Request.QueryString("ano").Trim%>
                </div>
            </center>
            <h3>
                Resumo</h3>
            <asp:GridView ID="GvResumo" runat="server" Width="300px" ShowFooter="true" BorderColor="#333333"
                AutoGenerateColumns="False" BorderStyle="Solid" CellPadding="3" CellSpacing="3"
                ForeColor="Black" ShowHeaderWhenEmpty="True" CssClass="GridRelatorio" Style="text-align: right;
                mso-number-format: \@; white-space: nowrap;" EnableModelValidation="True">
                <HeaderStyle CssClass="GridRelatorioHeader" />
                <AlternatingRowStyle CssClass="GridRelatorioAltRow" />
                <FooterStyle CssClass="GridRelatorioFooter" />
                <Columns>
                    <asp:BoundField HeaderText="TOTAL DE SERVIÇOS POR TIPO" DataField="categoria" />
                    <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N1}" />
                    <asp:BoundField HeaderText="QUANTIDADE" DataField="qtd" />
                    <asp:BoundField HeaderText="VALOR" DataField="total" DataFormatString="{0:c}" />
                </Columns>
            </asp:GridView>
            <br />
            <h3>
                Chamadas</h3>
            <asp:Label ID="lbParticulares" runat="server" Text=""></asp:Label>
            <br />
            <asp:Button ID="btParticular" runat="server" Text="SALVAR ALTERACOES" Visible="false"
                OnClientClick="ConfirmParticular();" />
            <%--<cc1:ConfirmButtonExtender ID="btParticular_ConfirmButtonExtender" 
            runat="server" ConfirmText="Autorizo o débito, através de folha de pagamento, do valor de R$  referente as ligações telefônicas que eu apontei como particulares em meu relatório de ligações telefônicas decorrentes do aparelho celular e/ou ramal que me foi disponibilizado para o trabalho." Enabled="True" TargetControlID="btParticular">
        </cc1:ConfirmButtonExtender>--%>
            <asp:Button ID="btSavaParticularExec" runat="server" Text="SALVA" Enabled="False"
                Style="display: none;" OnClick="btSavaParticularExec_Click" />
            <br />
            <br />
            <br />
            <h3>
                Extrato de Fixo</h3>
            <asp:GridView ID="gvExtrato" runat="server" Width="600px" ShowFooter="true" BorderColor="#333333"
                AutoGenerateColumns="False" BorderStyle="Solid" CellPadding="3" CellSpacing="3"
                ForeColor="Black" ShowHeaderWhenEmpty="True" CssClass="GridRelatorio" Style="text-align: right;
                mso-number-format: \@; white-space: nowrap;" EnableModelValidation="True">
                <HeaderStyle CssClass="GridRelatorioHeader" />
                <AlternatingRowStyle CssClass="GridRelatorioAltRow" />
                <FooterStyle CssClass="GridRelatorioFooter" />
                <Columns>
                    <asp:BoundField HeaderText="LINHA" DataField="ramal" />
                    <asp:BoundField HeaderText="CATEGORIA" DataField="categoria" />
                    <asp:BoundField HeaderText="ORIGEM" DataField="origem" />
                    <asp:BoundField HeaderText="DESTINO" DataField="fisico" />
                    <asp:BoundField HeaderText="SERVIÇO" DataField="tipo_serv" />
                    <asp:BoundField HeaderText="DESCRIÇÃO DO SERVIÇO" DataField="tipo_serv2" />
                    <asp:BoundField HeaderText="TIPO" DataField="tipo" Visible="false" />
                    <asp:BoundField HeaderText="NÚMERO CHAMADO" DataField="numero" />
                    <asp:BoundField HeaderText="DATA" DataField="dataini" />
                    <asp:BoundField HeaderText="DATAFIM" DataField="datafim" Visible="false" />
                    <asp:BoundField HeaderText="ROTA" DataField="rota" Visible="false" />
                    <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N1}" />
                    <asp:BoundField HeaderText="PARTICULAR" DataField="particular" Visible="false" />
                    <asp:BoundField HeaderText="VALOR FATURADO" DataField="valor" DataFormatString="{0:c}" />
                    <asp:BoundField HeaderText="VALOR AUDITADO" DataField="valor_audit" DataFormatString="{0:c}" />
                    <asp:BoundField HeaderText="OBS" DataField="obs" />
                    <asp:BoundField HeaderText="FATURADO" DataField="faturado" Visible="false" />
                    <asp:BoundField HeaderText="CODIGO_CONTA" DataField="codigo_conta" Visible="false" />
                    <asp:BoundField HeaderText="VALOR_OK" DataField="valor_ok" Visible="false" />
                    <asp:BoundField HeaderText="TARIFA" DataField="tarif_codigo" />
                    <asp:TemplateField>
                        <HeaderTemplate>
                            PARTICULAR
                        </HeaderTemplate>
                        <ItemTemplate>
                            <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                            <input type="checkbox" id="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>' />
                            <input type="hidden" id="tbCodigo" runat="server" value='<%# Container.DataItem("CODIGO") %>' />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <br />
    </center>
    <center>
        © CL Consultoria
        <br />
        Relatorio impresso em
        <asp:Label ID="lbdatenow" runat="server"></asp:Label>
    </center>
    </form>
</body>
</html>

