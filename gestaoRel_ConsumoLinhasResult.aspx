<%@ Page Title="" Language="VB" MasterPageFile="~/Cadastros.master" AutoEventWireup="false" CodeFile="gestaoRel_ConsumoLinhasResult.aspx.vb" Inherits="gestaoRel_ConsumoLinhasResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">  
   <style>
       .container {
    width: 98%;
}

   </style> 
        
                <br />
                <div class="">
                    <h1>
                        <asp:Label ID="lbUsuarioTop" runat="server" Text=""></asp:Label></h1>
                    <h4 style="text-transform: none; font-weight: 300">Relatório de Consumo de Linhas: <%= nome_mes %>/<%= ano.Trim%></h4>
                </div>
                <br />
                
               
                <div class="card">
                    <div class="card-body card-padding">
                    <div class="table-responsive">
                      
                        <asp:GridView ID="gvRel"  BorderWidth="0"  runat="server"  ClientIDMode="Static"  ShowFooter="true"
                            CssClass="table table-striped"  AutoGenerateColumns="true" EnableModelValidation="True">
                            
                            <FooterStyle CssClass="active" />
                        </asp:GridView>

                 
                    </div>
                        </div>
                </div>
                <br />
                © CL Consultoria
                 <br />
                Relatorio impresso em
        <asp:Label ID="lbdatenow" runat="server"></asp:Label>
 
    
    <script>
        $(function () {

            $('tbody').after("<tfoot>");
            // $('<tfoot>').insertBefore("tr.GridRelatorioFooter:first");
            // $('</tfoot>').insertAfter("tr.GridRelatorioFooter:last");
            // $('tr.GridRelatorioFooter:first').before('<tbody class="tablesorter-no-sort">');
            //$('tr.GridRelatorioFooter').insertAfter('<tbody class="tablesorter-no-sort">');

            $("#gvRel tr.active").appendTo($("#gvRel tfoot"));

            $("#gvRel").tablesorter(
           {
               widgets: ['reorder', 'stickyHeaders'],
               widgetOptions: {
                   reorder_axis: 'x', // 'x' or 'xy'
                   reorder_delay: 300,
                   reorder_helperClass: 'tablesorter-reorder-helper',
                   reorder_helperBar: 'tablesorter-reorder-helper-bar',
                   reorder_noReorder: 'reorder-false',
                   reorder_blocked: 'reorder-block-left reorder-block-end',
                   reorder_complete: null // callback
               }
                , headers: {
                    6: { sorter: 'digit' } // column number, type                
                }
                , textExtraction: function (node) {
                    // for numbers formattted like €1.000,50 e.g. Italian
                    // return $(node).text().replace(/[.$£€]/g,'').replace(/,/g,'.');

                    // for numbers formattted like $1,000.50 e.g. English
                    return $(node).text().replace(/[.,R$£€]/g, '').replace(/[-R$]/g, '-').replace(/[ ]/g, '');
                }
           }

           );

        });


    </script>     
  
</asp:Content>


