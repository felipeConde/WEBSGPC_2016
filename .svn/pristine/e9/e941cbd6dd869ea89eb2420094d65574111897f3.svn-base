<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoAnaliseConsumoResult.aspx.vb"
    Inherits="GestaoAnaliseConsumoResult" MasterPageFile="~/Cadastros.master" %>

<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
    <head id="Head1">
        <meta http-equiv="Content-Type" content="text/html; charset=windows-1252">
        <title>Análise de Consumo de Linhas Móveis</title>

        <script>
            $(function () {
                // Handler for .ready() called.
                $(".pagina_interna").width($("#gvRel").width());
                //            $("#gvSemrateio").css('margin-left', '10%');

                $(".well").width($("#gvRel").width());

                //            var foot = $("#gvRel").find('GridRelatorioFooter');
                //            if (!foot.length) foot = $('<GridRelatorioFooter>').appendTo("#gvRel");
                //            foot.append($('<td><b>Blablabla</b></td><td>a</td><td>b</td>'));

                $('tbody').after("<tfoot>");
                // $('<tfoot>').insertBefore("tr.GridRelatorioFooter:first");
                // $('</tfoot>').insertAfter("tr.GridRelatorioFooter:last");
                // $('tr.GridRelatorioFooter:first').before('<tbody class="tablesorter-no-sort">');
                //$('tr.GridRelatorioFooter').insertAfter('<tbody class="tablesorter-no-sort">');

                $("#gvRel tr.GridRelatorioFooter").appendTo($("#gvRel tfoot"));

                $("#gvRel").tablesorter(
              {
                  //cssInfoBlock: "tablesorter-no-sort", 
                  widgets: ['reorder', 'stickyHeaders']
                  , widgetOptions: {
                      reorder_axis: 'x', // 'x' or 'xy'
                      reorder_delay: 300,
                      //reorder_helperClass: 'tablesorter-reorder-helper',
                      //reorder_helperBar: 'tablesorter-reorder-helper-bar',
                      //reorder_noReorder: 'reorder-false',
                      //reorder_blocked: 'reorder-block-left reorder-block-end',
                      //reorder_complete: null // callback
                  }
                  , columnDefs: [
                         { orderable: true, targets: -1 }
                  ]
                  //                ,headers: {
                  //                    11: { sorter: 'digit'} // column number, type
                  //                  , 12: { sorter: 'digit'} // column number, type
                  //                  , 13: { sorter: 'digit'} // column number, type
                  //                  , 14: { sorter: 'digit'} // column number, type
                  //                  , 15: { sorter: 'digit'} // column number, type
                  //                     }
                  , textExtraction: function (node) {
                      // for numbers formattted like €1.000,50 e.g. Italian
                      // return $(node).text().replace(/[.$£€]/g,'').replace(/,/g,'.');

                      // for numbers formattted like $1,000.50 e.g. English
                      return $(node).text().replace(/[.,R$£€]/g, '').replace(/[-R$]/g, '-').replace(/[ ]/g, '');
                  }
              }

              );


            });


            function MostragvCabecalho() {
                // alert('teste');
                $("#divCabecalho").toggle(1000);
                //$("#gvCabecalho").show();
            }

        </script>


    </head>
    <body class="reportBody">
        <panel id="pagina" runat="server">
    <div >
                <br />

        <h1 >
            Relatório de Análise de Consumo
        </h1>
        <h5> <%=ViewState("nome_area") %></h5>
    </div>
        <div class="card">
                    <div class="card-header">
        <asp:GridView ID="gvCabecalho" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="codigo" DataField="codigo_fatura" ItemStyle-HorizontalAlign="Center" Visible="false" FooterStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="FATURA" DataField="fatura" DataFormatString="{0:N0}"  />  
            <asp:BoundField HeaderText="OPERADORA" DataField="operadora"  />              
            <asp:BoundField HeaderText="VENCIMENTO" DataField="dt_vencimento" />                        
            <asp:BoundField HeaderText="CONTESTAÇÃO CONCLUÍDA" DataField="contestada" />      
        </Columns>      
        <EmptyDataTemplate>Nenhum registro encontrato</EmptyDataTemplate>
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
        </div>
        </div>
         <table border="0" cellspacing="1" cellpadding="1" class="reportBody">
                <tr>
                    <td style="font-size:13px;">
                        <b>
                            <asp:Label ID="lbtitle" runat="server" Font-Size="14px"></asp:Label>
            <center>
                           <asp:PlaceHolder ID="Information" runat="server"></asp:PlaceHolder>
            </center>
            </table>
        
        <div >
        <h4>
        <a href="#" onclick="MostragvCabecalho()">
            <asp:Label ID="txtMSG" runat="server" ForeColor="Red" ToolTip="Clique aqui para detalhar" Font-Size="Small"></asp:Label>
        </a>
        </h4>
        </div >
                <div class="card">
                    <div class="card-header">
       <asp:GridView ID="gvRel" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="CCUSTO" DataField="grupo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="DESC CCUSTO" DataField="nome_grupo"  />           
            <asp:BoundField HeaderText="USUÁRIO" DataField="usuario" />
            <asp:BoundField HeaderText="MATRÍCULA" DataField="matricula"  />
            <asp:BoundField HeaderText="FATURA" DataField="fatura"  />
            <asp:BoundField HeaderText="LINHA" DataField="LINHA"  />
            <asp:BoundField HeaderText="STATUS" DataField="status" />
            <asp:BoundField HeaderText="TIPO" DataField="tipo" />
            <asp:BoundField HeaderText="CLASSIFICAÇÃO" DataField="classificacao" />
            <asp:BoundField HeaderText="QTD" DataField="QTD" DataFormatString="{0:N0}" />
            <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N}" />
            <asp:BoundField HeaderText="VALOR COBRADO" DataField="GASTO" DataFormatString="{0:c}" />            
            <asp:BoundField HeaderText="VALOR APÓS CONTESTAÇÃO" DataField="AUDITADO" DataFormatString="{0:c}"  />
            <asp:BoundField HeaderText="FRANQUIA" DataField="CONSUMO(FRANQUIA)" DataFormatString="{0:c4}" />
            <asp:BoundField HeaderText="RATEIO" DataField="RATEIO" DataFormatString="{0:c4}" />
            <asp:BoundField HeaderText="TOTAL" DataField="GASTO+RATEIO" DataFormatString="{0:c4}" />
            <asp:BoundField HeaderText="VICE PRES." DataField="VP"  />
            <asp:BoundField HeaderText="DIRETORIA" DataField="DIRETORIA"  />
        </Columns>      
        <EmptyDataTemplate>
            <center>
                Nenhum Registro Encontrado.
            </center>
        </EmptyDataTemplate>
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />
       </asp:GridView>
</div>
        </div>
                <div class="card">
                    <div class="card-header">
       <asp:GridView ID="gvRelConsolidado" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="CCUSTO" DataField="grupo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="DESC CCUSTO" DataField="nome_grupo"  /> 
            <asp:BoundField HeaderText="QTD DE LINHAS" DataField="qtd_celular" DataFormatString="{0:N0}" />           
            <asp:BoundField HeaderText="QTD DE CHAMADAS" DataField="QTD" DataFormatString="{0:N0}" />
            <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N}" />
            <asp:BoundField HeaderText="VALOR COBRADO" DataField="GASTO" DataFormatString="{0:c}" />            
            <asp:BoundField HeaderText="VALOR APÓS CONTESTAÇÃO" DataField="AUDITADO" DataFormatString="{0:c}" />
            <asp:BoundField HeaderText="CONSUMO DA FRANQUIA" DataField="CONSUMO(FRANQUIA)" DataFormatString="{0:c}" />
            <asp:BoundField HeaderText="RATEIO" DataField="RATEIO" DataFormatString="{0:c}" />
            <asp:BoundField HeaderText="TOTAL" DataField="GASTO+RATEIO" DataFormatString="{0:c}" />        
        </Columns>      
         <EmptyDataTemplate>
            <center>
                Nenhum Registro Encontrado.
            </center>
        </EmptyDataTemplate>


        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
       </div>
       </div>
       <asp:PlaceHolder ID="phServicos" runat="server">
           <div class="card">
                    <div class="card-header">
         <h4><asp:Label runat="server" ID="lbServicos" Text="Serviços de Franquias Compartilhadas" Visible=true></asp:Label></h4>
       <asp:GridView ID="gvRelServicos" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="CCUSTO" DataField="grupo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"  Visible="false" />
            <asp:BoundField HeaderText="DESC CCUSTO" DataField="nome_grupo" Visible="false"  />
            <asp:BoundField HeaderText="USUÁRIO" DataField="usuario"  Visible="false" />
            <asp:BoundField HeaderText="MATRÍCULA" DataField="matricula"  Visible="false" />
            <asp:BoundField HeaderText="FATURA" DataField="fatura"  Visible="false"  />
            <asp:BoundField HeaderText="SERVICO" DataField="LINHA"  />
            <asp:BoundField HeaderText="STATUS" DataField="status"  Visible="false" />
            <asp:BoundField HeaderText="TIPO" DataField="tipo"  Visible="false" />
            <asp:BoundField HeaderText="CLASSIFICAÇÃO" DataField="classificacao"  Visible="false" />
            <asp:BoundField HeaderText="QTD" DataField="QTD" DataFormatString="{0:N0}" />
            <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N}"  Visible="false" />
            <asp:BoundField HeaderText="VALOR COBRADO" DataField="GASTO" DataFormatString="{0:c}" />            
            <asp:BoundField HeaderText="VALOR APÓS CONTESTAÇÃO" DataField="AUDITADO" DataFormatString="{0:c}" />
            <asp:BoundField HeaderText="CONSUMO DA FRANQUIA" DataField="CONSUMO(FRANQUIA)" DataFormatString="{0:c4}"  Visible="false" />
            <asp:BoundField HeaderText="RATEIO" DataField="RATEIO" DataFormatString="{0:c4}"  Visible="false" />
            <asp:BoundField HeaderText="TOTAL" DataField="GASTO+RATEIO" DataFormatString="{0:c4}"  Visible="false" />       
                 
        </Columns>      
        
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
</div>
       </div>
           <div class="card">
                    <div class="card-header">
         <h4><asp:Label runat="server" ID="lbServicosRateios" Text="Serviços Compartilhados Rateados" Visible=true></asp:Label></h4>
       <asp:GridView ID="gvServicoRateio" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="CCUSTO" DataField="grupo" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center"  Visible="false" />
            <asp:BoundField HeaderText="DESC CCUSTO" DataField="nome_grupo" Visible="false"  />
            <asp:BoundField HeaderText="USUÁRIO" DataField="usuario"  Visible="false" />
            <asp:BoundField HeaderText="MATRÍCULA" DataField="matricula"  Visible="false" />
            <asp:BoundField HeaderText="FATURA" DataField="fatura"  Visible="false"  />
            <asp:BoundField HeaderText="SERVICO" DataField="LINHA"  />
            <asp:BoundField HeaderText="STATUS" DataField="status"  Visible="false" />
            <asp:BoundField HeaderText="TIPO" DataField="tipo"  Visible="false" />
            <asp:BoundField HeaderText="CLASSIFICAÇÃO" DataField="classificacao"  Visible="false" />
            <asp:BoundField HeaderText="QTD" DataField="QTD" DataFormatString="{0:N0}" />
            <asp:BoundField HeaderText="DURAÇÃO" DataField="duracao" DataFormatString="{0:N}"  Visible="false" />
            <asp:BoundField HeaderText="VALOR COBRADO" DataField="GASTO" DataFormatString="{0:c}" />            
            <asp:BoundField HeaderText="VALOR APÓS CONTESTAÇÃO" DataField="AUDITADO" DataFormatString="{0:c}" />
            <asp:BoundField HeaderText="CONSUMO DA FRANQUIA" DataField="CONSUMO(FRANQUIA)" DataFormatString="{0:c4}"  Visible="false" />
            <asp:BoundField HeaderText="RATEIO" DataField="RATEIO" DataFormatString="{0:c4}"  Visible="false" />
            <asp:BoundField HeaderText="TOTAL" DataField="GASTO+RATEIO" DataFormatString="{0:c4}"  Visible="false" />       
                 
        </Columns>      
        <EmptyDataTemplate>Nenhum registro encontrato</EmptyDataTemplate>
        
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
        </div>
       </div>
            <div class="card">
                    <div class="card-header">
       <h4><asp:Label runat="server" ID="lbFranquia" Text="Franquias da Fatura" Visible=true></asp:Label></h4>
       <asp:GridView ID="gvFranquia" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="FRANQUIA" DataField="franquia" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="CONSUMO CONTRATADO" DataField="consumo_contratado" DataFormatString="{0:N0}"  />  
            <asp:BoundField HeaderText="TIPO" DataField="tipo"  />              
            <asp:BoundField HeaderText="VALOR COBRADO" DataField="valor_pacote" DataFormatString="{0:c}" />                        
                     
                             
        </Columns>      
        <EmptyDataTemplate>Nenhum registro encontrato</EmptyDataTemplate>
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
        </div>
       </div>
     <div class="card">
                    <div class="card-header">
       <h4><asp:Label runat="server" ID="txtRateio" Text="Pacotes Rateados" Visible=false></asp:Label></h4>
       <asp:GridView ID="gvRateio" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="SERVIÇO" DataField="servico" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
            <asp:BoundField HeaderText="FATURA" DataField="fatura" Visible="false" />            
            <asp:BoundField HeaderText="VALOR" DataField="valor" DataFormatString="{0:c}" />                        
            <asp:BoundField HeaderText="TIPO" DataField="tipo"  />      
                 
        </Columns>      
        <EmptyDataTemplate>Nenhum registro encontrato</EmptyDataTemplate>
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
        </div>
       </div>
     <div class="card">
                    <div class="card-header">
       <br /><br />
       <h4><asp:Label runat="server" ID="lbSemRateio" Text="Serviços que não foram distribuídos como franquia ou rateados" Visible=true></asp:Label></h4>
       <asp:GridView ID="gvSemrateio" AutoGenerateColumns="false" BorderWidth="0" CssClass="table table-striped bootgrid-table" runat="server" ShowFooter="true" EnableModelValidation="True">
                             <HeaderStyle  />
                            <AlternatingRowStyle  />
        <Columns>
            <asp:BoundField HeaderText="SERVIÇO" DataField="servico" ItemStyle-HorizontalAlign="Center" FooterStyle-HorizontalAlign="Center" />
                 
            <asp:BoundField HeaderText="VALOR" DataField="valor" DataFormatString="{0:c}" /> 
            <asp:BoundField HeaderText="VALOR APÓS CONTESTAÇÃO" DataField="AUDITADO" DataFormatString="{0:c}" />                       
        </Columns>      
        <EmptyDataTemplate>Nenhum registro encontrato</EmptyDataTemplate>
        <FooterStyle Font-Bold="true" CssClass="GridRelatorioFooter" />   
       </asp:GridView>
            </div>
       </div>   
</asp:PlaceHolder>
       <br /><br />
   
     © <%=Date.Now.Year %> CL Consultoria<br>
    Relatorio impresso em
    <asp:Label ID="lbdatenow" runat="server"></asp:Label>
    </panel>
    </body>
</asp:Content>
