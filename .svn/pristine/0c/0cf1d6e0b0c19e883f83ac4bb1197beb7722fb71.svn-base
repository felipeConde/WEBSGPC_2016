<%@ Page Title="" Language="VB" MasterPageFile="~/Cadastros.master" AutoEventWireup="false" CodeFile="GestaoRel_ExtratoRamalResult.aspx.vb" Inherits="GestaoRel_ExtratoRamalResult" %>


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
                        <h3>Extrato de Ramal</h3>
                    </div>
                    <div class="card-body card-padding">
                        <div class="row">
                            <div class="col-xs-2">
                                <label>Ramal</label><br />
                                <%=ramal %>
                            </div> 
                             <div class="col-xs-4">
                                <label>C.Custo</label><br />
                                Centro de Custo: <asp:Label ID="lbGrupo" runat="server" Text=""></asp:Label>
                            </div>        
                                                
                            <div class="col-xs-3">
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
                            <div class="col-xs-3">
                                <div class="fg-line form-group">
                                    <label>Período:</label><br />
                                    <%= mes%>/<%= ano%>
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
                                <asp:BoundField HeaderText="VALOR" DataField="valor" DataFormatString="{0:c}"  />
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
            <asp:BoundField HeaderText="RAMAL" DataField="ramal" />
            <asp:BoundField HeaderText="ORIGEM" DataField="fisico" />            
            <asp:BoundField HeaderText="NÚMERO CHAMADO" DataField="numero" />
            <asp:BoundField HeaderText="TIPO" DataField="categoria" />
            <asp:BoundField HeaderText="DATA" DataField="dataini" />
            <asp:BoundField HeaderText="MINUTAGEM" DataField="duracao" />
            <asp:BoundField HeaderText="ROTA" DataField="rota" Visible="false" />
            <asp:BoundField HeaderText="VALOR" DataField="valor" DataFormatString="{0:c}" ItemStyle-HorizontalAlign="right" HeaderStyle-HorizontalAlign="right"   footerstyle-HorizontalAlign="right"   />       
              
            <%--<asp:CheckBoxField DataField="particular" HeaderText="PARTICULAR" ReadOnly="false"
                SortExpression="particular" />--%>

            <asp:TemplateField>
            <HeaderTemplate>
            PARTICULAR
            </HeaderTemplate>
                <ItemTemplate>
                    <%--<input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  onclick='SomaParticular(this.checked);' />--%>
                    <input type="checkbox" ID="chkParticular" runat="server" checked='<%# Container.DataItem("PARTICULAR") %>'  />
                    <input type="hidden" ID="tbCodigo" runat="server" value='<%# Container.DataItem("CODIGO") %>' />
                </ItemTemplate>
            </asp:TemplateField>  
           
        </Columns>
                        </asp:GridView>
<br /><br />
                        

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
            <div class="card" style="display:none;">
                <div class="card-body">
                     <div class="table-responsive">
                    <asp:GridView ID="GvTotais" runat="server" ShowFooter="FALSE"          CssClass="table table-striped bootgrid-table" AutoGenerateColumns="false" BorderWidth="0"
            EnableModelValidation="True">
                           
            <Columns>
            <asp:BoundField HeaderText="Meta do Ramal" DataField="meta" />
            <asp:BoundField HeaderText="Gasto Particular" DataField="particular" />
            <asp:BoundField HeaderText="Gasto Serviço" DataField="servico" />
            <asp:BoundField HeaderText="Saldo Restante" DataField="saldo_rest" />
              
            <%--<asp:CheckBoxField DataField="particular" HeaderText="PARTICULAR" ReadOnly="false"
                SortExpression="particular" />--%>
           
        </Columns>
       </asp:GridView>
</div>
                </div>

            </div>
                © CL Consultoria
                 <br />
                Relatorio impresso em
        <asp:Label ID="lbdatenow" runat="server"></asp:Label>
        </form>
    </body>
</asp:Content>

