<%@ Page Title="" Language="VB" MasterPageFile="~/Site.master" AutoEventWireup="false" CodeFile="GestaoRamais.aspx.vb" Inherits="GestaoRamais" %>
<%@ Reference Control="~/Site.master" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
<head>

        <title>Ramais</title>
        <!-- Add the Kendo styles to the in the head of the page... -->
      
       <%-- <link href="js/JqGrid/css/jquery-ui-1.8.1.custom.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid-bootstrap.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/ui.jqgrid-bootstrap-ui.css" rel="stylesheet"></link>
        <link href="js/JqGrid/css/checkbox_googleStyle.css" rel="stylesheet"></link>--%>

        <!-- Vendor CSS -->        
        
        <link href="vendors/bootgrid/jquery.bootgrid.min.css" rel="stylesheet">



   <%-- <script type="text/javascript">
        $.jgrid.no_legacy_api = true;
        $.jgrid.useJSON = true;
    </script>--%>


    <script type="text/javascript">
        $(document).keypress(function (e) {
            if (e.which == 13) {
                //alert('You pressed enter!');
                e.preventDefault();
                //return;
            }
        });




        var rowIds = [];
        var rowNumbers = [];
        var rowSIMs = [];
        var rowIMEIs = [];

        //$("#carregando").show();

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
                rowCount: <%=_dao_commons.GetGridRowCount %>,
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
                url: '<%=myUrl%>gestaoRamais.aspx?operacao=1',

                formatters: {
                    "commands": function (column, row) {
                        return "<button type=\"button\" class=\"btn btn-icon command-edit waves-effect waves-circle\"  onclick=abreJanela(\"" + row.ID + "\")><span class=\"zmdi zmdi-zoom-in\"></span></button> "
                        //    + "<button type=\"button\" class=\"btn btn-icon command-delete waves-effect waves-circle\" data-row-id=\"" + row.id + "\" onclick='abreJanela(" + row.ID + ")'><span class=\"zmdi zmdi-delete\"></span></button>"
                        ;
                    }
                }
            }).on("selected.rs.jquery.bootgrid", function (e, rows) {

                document.getElementById('textbox_hidden').value = '';
                document.getElementById('textHiddenNumber').value = '';
                document.getElementById('textHiddenIMEI').value = '';
                document.getElementById('textHiddenSIM').value = '';
                for (var i = 0; i < rows.length; i++) {
                    rowIds.push(rows[i].ID);
                    rowSIMs.push(rows[i].SIMCARD);
                    rowNumbers.push(rows[i].NUM_LINHA);
                    rowIMEIs.push(rows[i].IMEI);
                }
                //alert("Select: " + rowIds.join(","));
                document.getElementById('textHiddenNumber').value = rowNumbers.join(" ");
                document.getElementById('textHiddenIMEI').value = rowIMEIs.join(" ");
                document.getElementById('textHiddenSIM').value = rowSIMs.join(" ");
                document.getElementById('textbox_hidden').value = rowIds.join(" ");

                //alert(document.getElementById('textbox_hidden').value);
            }).on("deselected.rs.jquery.bootgrid", function (e, rows) {
                //var rowIds = [];
                document.getElementById('textbox_hidden').value = '';
                document.getElementById('textHiddenNumber').value = '';
                document.getElementById('textHiddenIMEI').value = '';
                document.getElementById('textHiddenSIM').value = '';

                for (var i = 0; i < rows.length; i++) {
                    //rowIds.push(rows[i].ID);
                    rowIds = jQuery.grep(rowIds, function (value) {
                        return value != rows[i].ID;
                    });
                    rowSIMs = jQuery.grep(rowSIMs, function (value) {
                        return value != rows[i].SIMCARD;
                    });
                    rowNumbers = jQuery.grep(rowNumbers, function (value) {
                        return value != rows[i].NUM_LINHA;
                    });
                    rowIMEIs = jQuery.grep(rowIMEIs, function (value) {
                        return value != rows[i].IMEI;
                    });
                }

                document.getElementById('textHiddenNumber').value = rowNumbers.join(" ");
                document.getElementById('textHiddenIMEI').value = rowIMEIs.join(" ");
                document.getElementById('textHiddenSIM').value = rowSIMs.join(" ");
                document.getElementById('textbox_hidden').value = rowIds.join(" ");

                //alert("Deselect: " + rowIds.join(","));
            });

        }



        $(document).ready(function () {


            CarregaGrid();

        });

        //function cleanID(id) {
        //    var aux = id.replace('<span class="', '');
        //    aux = aux.replace('mywrapping">', '');
        //    aux = aux.replace('</span>', '');
        //    return aux
        //}

        function abreJanela(id) {
            //alert(id);
            var configuracao = "top=50,left=50,width=1060,height=620,location=no,scrollbars=yes,resizable=YES,toolbar=no,directories=no";
            if (id == "0" || id == "") {
                arquivo = "RamalDetalhe.aspx"
            }
            else {
                //id = id.replace('?id=', '');
                arquivo = "RamalDetalhe.aspx?codigo=" + id
            }
            //window.open(arquivo, "_blank", configuracao);
            window.location.assign(arquivo);
            
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

        function janelalog(tipo) {
      
                    if (tipo == 1) {
                        document.getElementById('hidden_tipo').value = '1';
                    }
                    if (tipo == 2) {
                        document.getElementById('hidden_tipo').value = '2';
                    }
                    if (tipo == 3) {
                        document.getElementById('hidden_tipo').value = '3';

                    }

        }

        //function teste() {
        //    alert('checked!');
        //}

        var aberto = false;

        $('.sub-menu li a').click(function (event) {          
                  
            //alert($('#' + this.id));
            if ($(this).attr("href") != '#')
            {
                //alert($(this).attr("href"));
                //redireciona
                var url = $(this).attr("href");
                window.location.assign(url);
            }
            else
            {
                //alert('Não tem link');
            }
           
            return false;
        });

        $('.sub-menu').click(function (event) {
            if ($(this).hasClass("toogle"))
            {
                //alert($(this).hasClass("toogle"));
                $(this).removeClass("toogle");
                aberto=true
            }
            else
            {
                $(this).addClass("toogle");
                aberto = false;
            }          
          
            $('#' + this.id + ' ul').toggle(!aberto);            
            return false;
        });

    </script>

</head>
<body>
    <div id="carregando" style="display: none;">
        Carregando informações...Aguarde...
    </div>
    <br />
    
        <div class="block-header">
            <h2>RAMAIS</h2>
        </div>
       
        </asp:ScriptManager>
        <div class="card" id="card_grid">
            <br />
            <div class="col-sm-4 m-b-20">

                <div class="btn-group" style="float: left; display:none;">
                    <button type="button" class="btn btn-primary" data-toggle="dropdown">Relatórios</button>
                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" style="height: 30px;">
                        <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu" role="menu" style="padding: 10px">
                        <asp:LinkButton ID="btHistorico" runat="server" Text="Extrair histórico de linha(s)"
                            OnClientClick="return janelalog(1)" />
                        <br />
                        <br />
                        <asp:LinkButton runat="server" ID="btHistorico_SIM" runat="server" Text="Extrair histórico de simcard"
                            OnClientClick="return janelalog(2)" />
                        <br />
                        <br />
                        <asp:LinkButton runat="server" ID="btHistorico_IMEI" runat="server" Text="Extrair histórico de IMEI"
                            OnClientClick="return janelalog(3)" />
                        <br />
                        <br />
                        <button type="button" id="Button1" class="btn btn-primary"
                            value="Relatório de Aparelhos" onclick="javascript: window.location = 'GestaoRel_AparelhosMoveis.aspx';" >Relatório de Aparelhos </button>
                        <br />
                        <br />
                        <button type="button" id="Button3" class="btn btn-primary"
                            value="Relatório de alterações" onclick="javascript: window.location = 'GestaoRel_HistoricoMoveis.aspx';" >Relatório de alterações</button>
                    </ul>
                </div>

                <div class="btn-group" style="margin-left: 5px; float: left; display:none">
                    <button type="button" class="btn btn-primary" data-toggle="dropdown">Opções</button>
                    <button type="button" class="btn btn-primary dropdown-toggle" data-toggle="dropdown" style="height: 30px;">
                        <span class="caret"></span>
                    </button>
                    <ul class="dropdown-menu" role="menu" style="padding: 10px; width: 200px;">
                        <asp:CheckBox ID="chkHistorico" runat="server" AutoPostBack="True" ToolTip="Exibe os registro do log de linhas móveis"
                            Text="Histórico de Portabilidade" Visible="true" />
                    </ul>
                </div>
            </div>
            <br />
            <asp:GridView ID="gvGrid" runat="server" AutoGenerateColumns="false" ClientIDMode="Static">
                <Columns>
                    <asp:BoundField HeaderText="DESCRICAO" DataField="DESCRICAO" />
                </Columns>
            </asp:GridView>
            <div class="table-responsive">
                <%--<table id="list1" class="table table-condensed table-hover table-striped" data-toggle="bootgrid" data-ajax="true" data-url="GestaoUsuarios.aspx?operacao=1" data-multi-select="true" data-row-select="true" data-keep-selection="true">--%>
                <table id="list1" class="table table-condensed table-hover table-striped">
                    <thead>
                        <tr>
                             <th data-column-id="ID" data-type="string" data-identifier="true">RAMAL</th>
                            <th data-column-id="NOME">USUÁRIO</th>
                            <th data-column-id="GRUPO">DESC. CCUSTO</th>
                            <th data-column-id="CCUSTO">CCUSTO</th>
                            <th data-column-id="MODELO">MODELO</th>                            
                            
                            <th data-column-id="commands" data-formatter="commands" data-sortable="false" data-searchfield="false">VISUALIZAR</th>
                       </tr>
                    </thead>
                </table>
            </div>
            <div id="pager" style="width: 100%;">
            </div>
            <br />
            <br />
            <span style="display:none">
            <center>
                <asp:TextBox ID="textHiddenNumber" ClientIDMode="Static" runat="server" Width="180px" Style="display: none;"  ></asp:TextBox>
                <asp:TextBox ID="textHiddenSIM" ClientIDMode="Static" runat="server" Width="180px" Style="display: none;" ></asp:TextBox>
                <asp:TextBox ID="textHiddenIMEI" ClientIDMode="Static" runat="server" Width="180px" Style="display: none;" ></asp:TextBox>
                <asp:TextBox ID="textbox_hidden" ClientIDMode="Static" runat="server" Width="180px" Style="display: none;"></asp:TextBox>
                <button id="btNovo" class="btn btn-primary" value="Novo" onclick="javascript: abreJanela('0');">Novo</button>
                <span id="excluir" class="btn btn-primary" onclick="Excluir();" />Excluir Selecionado(s)</span>            
            <asp:Button ID="btnExcluir" ClientIDMode="Static" runat="server" Text="Excluir Selecionado(s)" Style="display: none;" />
            
            <div>
            <asp:UpdatePanel ID="upButtons" runat="server">
                <ContentTemplate>
                    <center>
                        <asp:Panel ID="Panel_Admin" runat="server">

                            <asp:TextBox ID="textbox1" runat="server" Width="180px" Style="display: none;"></asp:TextBox>
                            <asp:TextBox ID="hidden_tipo" runat="server" Width="180px" Style="display: none;"></asp:TextBox>
                        </asp:Panel>
                    </center>
                </ContentTemplate>
            </asp:UpdatePanel>

        </div>
                <center>
                    <div id="dialog-confirm" title="Linhas Excluídas" style="display: none;">
                        <p>
                            <span class="ui-icon ui-icon-alert" style="float: left; margin: 0 7px 20px 0;"></span>
                            <asp:Label ID="lbMSG" runat="server"></asp:Label>
                    </div>
                </center>
                <br />
            </center>
            </span>
        </div>

    </asp:Content>
