<%@ Page Title="" Language="VB" MasterPageFile="~/Cadastros.master" AutoEventWireup="false" CodeFile="gestaoRel_ConsumoRamaisResult.aspx.vb" Inherits="gestaoRel_ConsumoRamaisResult" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">    
    
    <style>
       .container {
        width: 98%;
        }

   </style> 

  <span id="contentPDF" >
      
      <br />
                <div class="">
                    <h1>
                        <asp:Label ID="lbUsuarioTop" runat="server" Text=""></asp:Label></h1>
                    <h4 style="text-transform: none; font-weight: 300">Relatório de Consumo de Ramais: <%= nome_mes %>/<%= ano.Trim%></h4>
                </div>
                <br />
                
               
                <div class="card">
                    <div class="card-body card-padding">
                    <div class="table-responsive">
                      
                        <asp:GridView ID="gvRel"  BorderWidth="0"  runat="server" ShowFooter="true" ClientIDMode="Static"
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
 </span>
   <script src="http://mrrio.github.io/jsPDF/dist/jspdf.debug.js"></script>  
      <script>
    function demoFromHTML() {
        var pdf = new jsPDF('p', 'pt', 'letter');
        // source can be HTML-formatted string, or a reference
        // to an actual DOM element from which the text will be scraped.
        source = $('#contentPDF')[0];

        // we support special element handlers. Register them with jQuery-style 
        // ID selector for either ID or node name. ("#iAmID", "div", "span" etc.)
        // There is no support for any other type of selectors 
        // (class, of compound) at this time.
        specialElementHandlers = {
            // element with id of "bypass" - jQuery style selector
            '#bypassme': function (element, renderer) {
                // true = "handled elsewhere, bypass text extraction"
                return true
            }
        };
        margins = {
            top: 80,
            bottom: 60,
            left: 40,
            width: 522
        };
        // all coords and widths are in jsPDF instance's declared units
        // 'inches' in this case
        pdf.fromHTML(
        source, // HTML string or DOM elem ref.
        margins.left, // x coord
        margins.top, { // y coord
            'width': margins.width, // max width of content on PDF
            'elementHandlers': specialElementHandlers
        },

        function (dispose) {
            // dispose: object with X, Y of the last line add to the PDF 
            //          this allow the insertion of new lines after html
            pdf.save('Test.pdf');
        }, margins);
    }
 </script>
  <script>
      $(document).ready(function () {
          // console.log("ready!");
          //printPDF2();
          //demoFromHTML();
      });

  </script>

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

