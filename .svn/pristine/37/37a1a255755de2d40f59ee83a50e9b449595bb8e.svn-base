<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoRel_ExtratoCelularResult.aspx.vb"
    Inherits="GestaoRel_ExtratoCelularResult" MasterPageFile="~/Cadastros.master" %>

<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <head id="Head1">
        <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
        <title>Extrato de Usuário</title>

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
        <form id="form1">
                <br />
                <div class="">
                    <h1>
                        <asp:Label ID="lbUsuarioTop" runat="server" Text=""></asp:Label></h1>
                    <h4 style="text-transform: none; font-weight: 300">Extrato de <%= nome_mes %>/<%=ano%></h4>
                </div>
                <br />
                <div class="card">
                    <div class="card-header">
                        <h3>Extrato de Dispositivo Móvel</h3>
                    </div>
                    <div class="card-body card-padding">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Celular</label><br />
                                <asp:Label ID="lbLinha" runat="server" Text=""></asp:Label>
                            </div>
                            <div class="col-xs-2">
                                <div class="fg-line form-group">
                                    <label>Modelo:</label><br />
                                    <asp:Label ID="lbTipo" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                             <div class="col-xs-2">
                                <div class="fg-line form-group">
                                    <label>Plano:</label><br />
                                    <asp:Label ID="lbPlano"  runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <div class="col-xs-2">
                                <div class="fg-line form-group">
                                    <label>Usuário:</label><br />
                                    <asp:Label ID="lbUsuario" runat="server" Text=""></asp:Label>
                                </div>
                            </div>
                            <% If Request.QueryString("dataini") <> "" Then %>
                            <div class="col-xs-2">
                                <div class="fg-line form-group">
                                    <label>Período:</label><br />
                                    <%= Request.QueryString("dataini")%>-
                            <%= Request.QueryString("datafim")%>
                                </div>
                            </div>
                            <% end If %>
                            <div class="col-xs-1">
                                <div class="fg-line form-group">
                                    <label>Vencimento:</label><br />
                                    <%= mes%>/<%= ano%>
                                </div>
                            </div>
                            <div class="col-xs-1">
                                <div class="fg-line form-group">
                                    <label>Fatura:</label><br />
                                    <%=_descFatura%>
                                </div>
                            </div>
                            <div class="col-xs-1">
                                <div class="fg-line form-group">
                                    <label>Operadora:</label><br />
                                    <asp:Label ID="lbOperadora" runat="server"></asp:Label>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
                <div class="card">
                    <div class="card-header">
                        <h3>Resumo</h3>
                    </div>
                    <div class="table-responsive">
                        <asp:GridView ID="GvResumo" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                            <HeaderStyle />
                            <AlternatingRowStyle />
                            <FooterStyle CssClass="active" />
                            <Columns>
                                <asp:BoundField HeaderText="TOTAL DE SERVIÇOS POR TIPO" DataField="categoria" />
                                <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N1}" />
                                <asp:BoundField HeaderText="QUANTIDADE" DataField="qtd" />
                                <asp:BoundField HeaderText="VALOR" DataField="total" DataFormatString="{0:c}"  />
                            </Columns>
                        </asp:GridView>
                        <br />
                        <br />
                        <asp:Label ID="lbParticulares" runat="server" Text="" CssClass="label label-warning" Height="40px" Width="500px" Font-Size="Larger"></asp:Label>
                    </div>
                </div>
                <div class="card">
                    <div class="table-responsive">
                        <div class="card-header">
                            <h3>Detalhamento</h3>
                        </div>
                        <asp:GridView ID="gvExtrato" BorderWidth="0" runat="server" ShowFooter="true"
                            CssClass="table table-striped"  AutoGenerateColumns="false" EnableModelValidation="True">
                            <FooterStyle CssClass="active" />
                            <Columns>
                                <asp:BoundField HeaderText="CELULAR" DataField="ramal" />
                                <asp:BoundField HeaderText="CATEGORIA" DataField="categoria" Visible="false" />
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
                                <asp:BoundField HeaderText="CONSUMO DA FRANQUIA" DataField="valor_rateio" DataFormatString="{0:c}" />
                                <asp:BoundField HeaderText="VALOR AUDITADO" DataField="valor_audit" DataFormatString="{0:c}" />
                                <asp:BoundField HeaderText="VALOR" DataField="valor_total" DataFormatString="{0:c}" />
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
                                 <asp:BoundField HeaderText="DADOS(MB)" DataField="VALOR TRAFEGADO(MB)" />
                            </Columns>
                        </asp:GridView>

                        <asp:PlaceHolder ID="phParticulares" runat="server">

                            <asp:Button ID="btParticular" runat="server" Text="SALVAR ALTERACOES" Visible="false"
                                OnClientClick="ConfirmParticular();" />
                            <%--<cc1:ConfirmButtonExtender ID="btParticular_ConfirmButtonExtender" 
                            runat="server" ConfirmText="Autorizo o débito, através de folha de pagamento, do valor de R$  referente as ligações telefônicas que eu apontei como particulares em meu relatório de ligações telefônicas decorrentes do aparelho celular e/ou ramal que me foi disponibilizado para o trabalho." Enabled="True" TargetControlID="btParticular">
                            </cc1:ConfirmButtonExtender>--%>
                            <asp:Button ID="btSavaParticularExec" runat="server" Text="SALVA" Enabled="False"
                                Style="display: none;" OnClick="btSavaParticularExec_Click" />
                            <br />
                            <br />
                        </asp:PlaceHolder>
                    </div>
                </div>
                <br />
                © CL Consultoria
                 <br />
                Relatorio impresso em
        <asp:Label ID="lbdatenow" runat="server"></asp:Label>
        </form>
    </body>
</asp:Content>
