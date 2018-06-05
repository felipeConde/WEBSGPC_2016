<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoHistoricos.aspx.vb" MasterPageFile="~/Cadastros.master"
    Inherits="GestaoHistoricos" EnableEventValidation="false" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
<head >
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.4/jspdf.debug.js"></script>
    <script src="https://cdnjs.cloudflare.com/ajax/libs/jspdf/1.3.4/html2pdf.js"></script>
<%-- 
     <link href = "../js/bootstrap-3.3.5-dist/css/bootstrap.min.css" rel="stylesheet" type="text/css" />

      <script src="../js/jquery-1.11.2.min.js" type="text/javascript"></script>
   --%>
        <script src="Scripts/jquery-1.10.2.min.js"></script>
       <script type="text/javascript" src="../js/jquery.tablesorter.min.js"></script>     
      <script type="text/javascript" src="../js/jquery.tablesorter.widgets.js "></script>
      <script src="../js/widget-reorder.js"></script>
   
     

    <script type="text/javascript">

        function Chama_Extrato(parametro, mes, ano) {

            if (mes < 10) {
                mes = '0' + mes
            }

            window.open(('GestaoRel_ExtratoCelularResult.aspx?celular=' + parametro + '&mes=' + mes + '&ano=' + ano + '&tipo=HTML&dataini=&datafim='), '_blank ', '', ''); void (0);
        }

        function Chama_Extrato_Fixo(parametro, mes, ano) {

            if (mes < 10) {
                mes = '0' + mes
            }

            window.open(('GestaoRel_ExtratoFixoResult.aspx?celular=' + parametro + '&mes=' + mes + '&ano=' + ano + '&tipo=HTML&dataini=&datafim='), '_blank ', '', ''); void (0);
        }

        function Chama_Relatorio_Num_Mais(parametro) {

            window.open(('GestaoHistoricos.aspx?linhaschamadas=1&parametro=' + parametro), '_blank ', '', ''); void (0);
        }



        $(function () {
            //  alert('teste');
            // Handler for .ready() called.
            //$('tbody').after("<tfoot>");
            $("#TaskGridView tr.GridRelatorioFooter").appendTo($("#TaskGridView tfoot"));
            $("#TaskGridView").tablesorter(
            {               
                widgets: ['reorder','stickyHeaders'],
                widgetOptions: {
                    reorder_axis: 'x', // 'x' or 'xy'
reorder_delay: 300,
                    reorder_helperClass: 'tablesorter-reorder-helper',
reorder_helperBar: 'tablesorter-reorder-helper-bar',
reorder_noReorder: 'reorder-false',
reorder_blocked: 'reorder-block-left reorder-block-end',
reorder_complete: null // callback
                }
                 ,headers: {
                    6: { sorter: 'digit'} // column number, type                
                     }
                 , textExtraction: function (node) {
                     // for numbers formattted like €1.000,50 e.g. Italian
                     // return $(node).text().replace(/[.$£€]/g,'').replace(/,/g,'.');

                     // for numbers formattted like $1,000.50 e.g. English
                     return $(node).text().replace(/[.,R$£€]/g, '').replace(/[-R$]/g, '-').replace(/[ ]/g, '');
                 }
            }
           
            );
            //alert('teste');

             //$(".well").width($("#TaskGridView").width());

            
        });
            

        function printPDF()
        {
            alert('teste');
            var pdf = new jsPDF('p', 'pt', 'letter');
            var canvas = pdf.canvas;
            canvas.width = 8.5 * 72;
            html2canvas(document.body, {
                canvas: canvas,
                onrendered: function (canvas) {
                    var iframe = document.createElement('iframe');
                    iframe.setAttribute('style', 'position:absolute;right:0; top:0; bottom:0; height:100%; width:500px');
                    document.body.appendChild(iframe);
                    iframe.src = pdf.output('datauristring');
                    //var div = document.createElement('pre');
                    //div.innerText=pdf.output();
                    //document.body.appendChild(div);
                }
            });

        }

    </script>
    <style type="text/css">
        TABLE.GridView td
        {
            text-align: right;
            mso-number-format:   \@;
            white-space: nowrap;
        }
        
      /*  table.GridRelatorio thead tr .header { */
       #TaskGridView th { 
            background-image: url(img/sort_bg.gif);
            background-repeat: no-repeat;
            background-position: center right;
            cursor: pointer;
            padding: 4px;
            padding-right:  20px;
        }


       .container {
            width: 100%;
        }
    </style>


</head>
<body >
  
    <panel id="pagina" runat="server">  
        <br />
                <div class="text-center" id="divWell" runat="server">
                    <h1>
            <asp:Label ID="lbtitle" runat="server" Font-Size="Medium"></asp:Label></h1>
                    <h4 style="text-transform: none; font-weight: 300"><asp:PlaceHolder ID="Information" runat="server" ></asp:PlaceHolder></h4>
                </div>
      <br />
        <div class="card" style="width:100%">
           <div class="card-header">
                            
                            
                            <ul class="actions">
                                <li>
                                    <a href="">
                                        <i class="zmdi zmdi-refresh-alt"></i>
                                    </a>
                                </li>
                                <li>
                                    <asp:LinkButton ID="btExcel" runat="server" title="Baixar Excel">
                                    
                                        <i class="zmdi zmdi-download"></i>
                                    
                                    </asp:LinkButton>
                                </li>
                                
                            </ul>
                        </div>
            <div class="card-body card-padding">
                <div class="row">
                     <div class="table-responsive">
                      <asp:GridView ID="TaskGridView" ClientIDMode="Static" runat="server" AllowPaging="false" ShowFooter="false" 
                           EnableModelValidation="True"
                            AllowSorting="False"  CssClass="table table-striped bootgrid-table" BorderWidth="0" AutoGenerateColumns="true"  >
                          
                        </asp:GridView>
                  </div>
                </div>
            </div>
        </div>

    

    
        <center>

                                <hr>
                                                                           
    </center> © <%=Date.Now.Year %> CL Consultoria<br>
    Relatorio impresso em
    <asp:Label ID="lbdatenow" runat="server"></asp:Label>
     </panel>
   
</body>
</asp:Content>