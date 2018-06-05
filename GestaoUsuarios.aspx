<%@ Page Language="VB" AutoEventWireup="false" CodeFile="GestaoUsuarios.aspx.vb"
    Inherits="GestaoUsuarios" MasterPageFile="~/Site.master" %>

<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <head>

        <title>Usuários</title>
        <!-- Add the Kendo styles to the in the head of the page... -->
      
       <%-- <link href="js/JqGrid/css/jquery-ui-1.8.1.custom.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid-bootstrap.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid-bootstrap-ui.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/checkbox_googleStyle.css" rel="stylesheet"></link>--%>

        <!-- Vendor CSS -->        
        
        <link href="vendors/bootgrid/jquery.bootgrid.min.css" rel="stylesheet">
  

     

        <script type="text/javascript">
            var rowIds = [];

            //$("#carregando").show();


            function CarregaGridOld() {
                //alert('teste');

                jQuery("#list1").jqGrid({
                    url: 'GestaoUsuarios.aspx?operacao=1',
                    datatype: "json",
                    height: $(window).height() * 0.54,
                    width: $("card_grid").width(),
                    colNames: ['Código', 'Usuário', 'Cargo', 'Senha Ramal', 'Ramal', 'Email', 'Login', 'Supervisor', 'CCusto Usuário', 'C.CCusto Usuário', 'C.CCusto Ramal', 'Matricula'],
                    colModel: [
              { name: 'ID', index: 'ID', sorttype: "int", key: true, hidden: true },
              { name: 'NOME', index: 'NOME', sortable: true, jsonmap: "NOME" },
              { name: 'CARGO_USUARIO', index: 'CARGO_USUARIO' },
              { name: 'SENHA', index: 'SENHA' },
              { name: 'RAMAL', index: 'RAMAL' },
              { name: 'EMAIL', index: 'EMAIL' },
              { name: 'LOGIN', index: 'LOGIN' },
              { name: 'SUPERVISOR', index: 'SUPERVISOR' },
              { name: 'GRUPO', index: 'GRUPO' },
              { name: 'CCUSTO', index: 'CCUSTO' },
              { name: 'CCUSTO_RAMAL', index: 'CCUSTO_RAMAL' },
              { name: 'MATRICULA', index: 'MATRICULA' }],

                    multiselect: false,
                    rowNum: 50,
                    rowList: [50, 100, 200],
                    pager: "#pager",
                    autoencode: true,
                    ignoreCase: true,
                    sortname: "ID",
                    viewrecords: true,
                    sortorder: "desc",
                    shrinkToFit: false,
                    reloadAfterSubmit: true,

                    loadBeforeSend: function (xhr, settings) {
                        this.p.loadBeforeSend = null; //remove event handler
                        return false; // dont send load data request
                    },

                    loadComplete: function () {
                        $("tr.jqgrow:odd").css("background", "#f4f4f4");
                    },

                    ondblClickRow: function (id) {
                        var ret = jQuery("#list1").jqGrid('getRowData', id);
                        //alert("codigo="+ret.CODIGO+" invdate="+ret.NOME_USUARIO+"...");;
                        abreJanela(cleanID(ret.ID));
                    },
                    gridComplete: function () {
                        $("tr.jqgrow:odd").addClass('GrdAlternateRow');
                        //esconde o carregando
                        $("#carregando").hide();
                    }


                });

                jQuery("#list1").jqGrid('filterToolbar', { stringResult: true, searchOnEnter: true }, { defaultSearch: "cn" });
                jQuery("#list1").jqGrid('navGrid', '#pager', { edit: false, add: false, del: false });

                //jQuery("#list1").jqGrid('setGridParam', { 'datatype' : 'local' }).trigger('reloadGrid');


                jQuery("#list1").bind("jqGridAfterLoadComplete", function () {


                    if (this.rows.length > 1) {

                        var $this = $(this), iCol, iRow, rows, row, cm, colWidth,
                        $cells = $this.find(">tbody>tr>td"),
                        $colHeaders = $(this.grid.hDiv).find(">.ui-jqgrid-hbox>.ui-jqgrid-htable>thead>.ui-jqgrid-labels>.ui-th-column>div"),
                        colModel = $this.jqGrid("getGridParam", "colModel"),
                        n = $.isArray(colModel) ? colModel.length : 0,
                        idColHeadPrexif = "jqgh_" + this.id + "_";

                        $cells.wrapInner("<span class='mywrapping'></span>");
                        $colHeaders.wrapInner("<span class='mywrapping'></span>");

                        for (iCol = 0; iCol < n; iCol++) {
                            cm = colModel[iCol];
                            colWidth = $("#" + idColHeadPrexif + $.jgrid.jqID(cm.name) + ">.mywrapping").outerWidth() + 25; // 25px for sorting icons
                            for (iRow = 0, rows = this.rows; iRow < rows.length; iRow++) {
                                row = rows[iRow];
                                if ($(row).hasClass("jqgrow")) {
                                    colWidth = Math.max(colWidth, $(row.cells[iCol]).find(".mywrapping").outerWidth());
                                }
                            }
                            $this.jqGrid("setColWidth", iCol, colWidth + 10);
                        }

                        jQuery("#list1").jqGrid('setGridWidth', 1140);
                    }
                });

            }

            function CarregaGrid() {

                $("#list1").bootgrid({
                    css: {
                        icon: 'zmdi icon',
                        iconColumns: 'zmdi-view-module',
                        iconDown: 'zmdi-expand-more',
                        iconRefresh: 'zmdi-refresh',
                        iconUp: 'zmdi-expand-less'
                    },
                    selection: false,
                    multiSelect: false,
                    //rowSelect: true,
                    keepSelection: true,
                    requestHandler: function (request) {
                        var model = {
                            param1: 1,
                            param2: 2

                        }
                        model.Current = request.current;
                        model.RowCount = request.rowCount;
                        model.Search = request.searchPhrase;
                        model.Colunas = request.colunas;

                        for (var key in request.sort) {
                            model.SortBy = key;
                            model.SortDirection = request.sort[key];
                        }

                        return JSON.stringify(model);
                    },
                    ajaxSettings: {
                        method: "GET",
                        contentType: "application/json",
                        cache: false
                    },
                    ajax: true,
                    url: '<%=myUrl%>GestaoUsuarios.aspx?operacao=1',

                    formatters: {
                        "commands": function(column, row) {
                            return "<button type=\"button\" class=\"btn btn-icon command-edit waves-effect waves-circle\"  onclick='abreJanela(" + row.ID + ")'><span class=\"zmdi zmdi-edit\"></span></button> "
                            //    + "<button type=\"button\" class=\"btn btn-icon command-delete waves-effect waves-circle\" data-row-id=\"" + row.id + "\" onclick='abreJanela(" + row.ID + ")'><span class=\"zmdi zmdi-delete\"></span></button>"
                            ;
                        }
                    }
                }).on("selected.rs.jquery.bootgrid", function (e, rows) {
                    document.getElementById('textbox_hidden').value = '';
                    for (var i = 0; i < rows.length; i++) {
                        rowIds.push(rows[i].ID);
                    }
                    //alert("Select: " + rowIds.join(","));
                    document.getElementById('textbox_hidden').value = rowIds.join(",");
                    //alert(document.getElementById('textbox_hidden').value);
                }).on("deselected.rs.jquery.bootgrid", function (e, rows) {
                    //var rowIds = [];
                    document.getElementById('textbox_hidden').value = '';
                    for (var i = 0; i < rows.length; i++) {
                        //rowIds.push(rows[i].ID);
                        rowIds = jQuery.grep(rowIds, function (value) {
                            return value != rows[i].ID;
                        });
                    }
                    document.getElementById('textbox_hidden').value = rowIds.join(",");
                   
                    //alert("Deselect: " + rowIds.join(","));
                });

            }



            $(document).ready(function () {
               
                
                CarregaGrid();
                //Selection
                //$("#gvGrid").bootgrid({
                //    css: {
                //        icon: 'zmdi icon',
                //        iconColumns: 'zmdi-view-module',
                //        iconDown: 'zmdi-expand-more',
                //        iconRefresh: 'zmdi-refresh',
                //        iconUp: 'zmdi-expand-less'
                //    },
                //    selection: true,
                //    multiSelect: true,
                //    rowSelect: true,
                //    keepSelection: true
                //});

            });

            function cleanID(id) {
                var aux = id.replace('<span class="', '');
                aux = aux.replace('mywrapping">', '');
                aux = aux.replace('</span>', '');
                return aux
            }

            function abreJanela(id) {
                //alert(id);
                var configuracao = "top=50,left=50,width=750,height=800";
                if (id == "0" || id == "") {
                    arquivo = "GestaoCadastroUsuario.aspx"
                }
                else {
                    //id = id.replace('?id=', '');
                    arquivo = "GestaoCadastroUsuario.aspx?codigo=" + id
                }
                window.open(arquivo, "_blank", configuracao);
            }

            function abreLink(id) {

                id = id.replace('?id=', '');
                var ret = jQuery("#list1").jqGrid('getRowData', id);
                //alert("codigo="+ret.CODIGO+" invdate="+ret.NOME_USUARIO+"...");;
                abreJanela(cleanID(ret.ID));
            }

            function janelalog() {
                
                var codigo = -1;
                document.getElementById('textbox_hidden').value = ""

                $.each(rowIds, function (index, value) {
                    codigo = value;
                    document.getElementById('textbox_hidden').value = document.getElementById('textbox_hidden').value + " " + codigo
                });
                if (codigo == -1) {
                    alert("Selecione ao menos um Usuário");
                    return false;
                }
                else
                {
                    //alert("passou");
                    __doPostBack('btHistorico', '');
                    
                }
            }

            function janelabatch() {
                window.open("atualizausuariosbatch.html", "", "top=0,left=20,width=450,height=450,scrollbars=0,resizable=0")
            }

            function janeladepara() {
                window.open("deparacdrs.html", "", "top=0,left=20,width=450,height=650,scrollbars=false,resizable=false")
            }

            function Excluir() {
                //alert(rowIds.length());
                
                var codigo = -1;
                //document.getElementById('textbox_hidden').value = ""
               

                //var answer = confirm('Tem certeza que deseja excluir os regitros selecionados?');

                //confirmação              
                    swal({
                        title: "Confirmar exclusão?",
                        text: "Os registros selecionados serão excluídos permanentemente do sistema!",
                        type: "warning",
                        showCancelButton: true,
                        confirmButtonColor: "#DD6B55",
                        confirmButtonText: "Sim, excluir registros!",
                        closeOnConfirm: false
                    }, function () {
                        //swal("Deleted!", "Your imaginary file has been deleted.", "success");
                        $.each(rowIds, function (index, value) {
                            codigo = value;
                            document.getElementById('textbox_hidden').value = document.getElementById('textbox_hidden').value + " " + codigo
                        });
                        //return;                    
                        if (codigo == -1) {
                            alert("Selecione ao menos um registro");
                            return false;
                        }
                        else {
                            //alert("Entrou");
                            __doPostBack('btnExcluir', '');
                        }
                    });
               


                
            }

            function ExibeExclusao() {
                //alert("teste");
                setTimeout(function () {                 
                    swal("Sucesso!", "Operação realizada:", "success");
                }, 500);
            }

             function ExibeErro() {
                //alert("teste");
                setTimeout(function () {
                    swal({ title: 'Error!', text: "<%=strResult%>", type: 'error', confirmButtonText: 'Fechar', html: true });
                    //swal("Sucesso!", "Operação realizada:", "success");
                }, 500);
            }


        </script>
    </head>
    <body>
        <div id="carregando" style="display: none;">
            Carregando informações...Aguarde...
        </div>
        <div class="block-header">
            <h2>Usuários</h2>
        </div>
        <div class="card" id="card_grid">
            <br />


           
            <asp:GridView ID="gvGrid" runat="server" AutoGenerateColumns="false" ClientIDMode="Static">

                <Columns>
                    <asp:BoundField HeaderText="Nome" DataField="NOME"  />
                </Columns>

            </asp:GridView>

                            
                     

    <div class="table-responsive">
            <%--<table id="list1" class="table table-condensed table-hover table-striped" data-toggle="bootgrid" data-ajax="true" data-url="GestaoUsuarios.aspx?operacao=1" data-multi-select="true" data-row-select="true" data-keep-selection="true">--%>
                <table id="list1" class="table table-condensed table-hover table-striped">
                 <thead>
                    <tr>
                       <th data-column-id="ID" data-type="numeric" data-identifier="true">ID</th>
                        <th data-column-id="NOME">NOME</th>                        
                        <th data-column-id="LOGIN" >LOGIN</th>
                        <th data-column-id="EMAIL" >EMAIL</th>
                         <th data-column-id="RAMAL" >RAMAL</th>
                         <th data-column-id="CCUSTO" >CCUSTO</th>
                         <th data-column-id="MATRICULA"  >MATRÍCULA</th>
                         <th data-column-id="commands" data-formatter="commands" data-sortable="false" data-searchField="false"></th>
                         
                    </tr>
                </thead>
            </table>
    </div>
            <div id="pager" style="width:100%;">
            </div>
            <br />
            <center>
            <asp:TextBox ID="textbox_hidden" ClientIDMode="Static" runat="server" Width="180px" Style="display: none"></asp:TextBox>
            <button id="btNovo" class="btn btn-primary"  value="Novo Usuário" onclick="javascript: abreJanela(0);" / Style="display: none">Novo Usuário</button>
            <span id="excluir" class="btn btn-primary" onclick="Excluir();" Style="display: none" />Excluir Selecionado(s)</span>            
            <asp:Button ID="btnExcluir" ClientIDMode="Static" runat="server" Text="Excluir Selecionado(s)" style="display:none;"  />
            <button id="historico" class="btn btn-primary" onclick="janelalog();" Style="display: none" />Histórico</button>            
            <asp:Button ID="btHistorico" runat="server"  ClientIDMode="Static" Text="Histórico" style="display:none;" />
            <%--
        <asp:Button ID="btHistoricoGeral" runat="server" Text="Histórico de Alterações" CssClass="ui-button ui-widget ui-state-default ui-corner-all ui-button-text-only"
            OnClientClick="window.open('velog.asp?operacao=3&dataini=01/01/1987&datafim=31/12/2100&usuario=','historico', 'width=500,height=450');" />
            --%>
            <asp:Button ID="btnSyncLines" runat="server" Text="Sincronizar Linhas" Style="display: none" />
            <center>
                <div id="dialog-confirm" title="Linhas Excluídas" style="display: none;">
                    <p>
                        <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span>
                        <asp:Label ID="lbMSG" runat="server"></asp:Label>
                </div>
            </center>
                    <br />
        </center>
        </div>
    </body>

            

</asp:Content>
