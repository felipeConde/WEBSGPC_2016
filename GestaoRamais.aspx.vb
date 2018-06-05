Imports System.Data
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System

Partial Class GestaoRamais
    Inherits System.Web.UI.Page
    Dim _jqGrid As New bootGrid
    Dim operacao As Integer = 0
    Dim numberOfRows As Integer = 3
    Dim pageIndex As Integer = 1
    Dim totalrecords As Integer = 0
    Dim search As String = ""
    Dim sortColumnName As String = ""
    Dim sortOrderBy As String = ""
    Dim filters As String = ""
    Dim arrayFilters As New List(Of JQGridFilterRules)
    Public result As String = ""
    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Public _dao_commons As New DAO_Commons
    Private _dao As New DAORamais
    Private current As String = "1"
    Private rowCount As String = "10"
    Private sort As String = ""
    Private searchPhrase As String = ""
    Public myUrl As String = ""
    Public strResult As String = ""
    Public rowCountDefault As String = 5


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("conexao") = "Provider=OraOLEDB.Oracle;Password=light;User ID=light;Data Source=server;"
        myUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) & "/"


        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        Else
            strConexao = Session("conexao")
        End If

        _dao_commons.strConn = Session("conexao").ToString
        _dao.strConn = Session("conexao").ToString

        rowCountDefault = _dao_commons.GetGridRowCount

        If Not Page.IsPostBack Then
            operacao = Request.QueryString("operacao")
            If operacao = 1 Then

                'pegamos os parametros
                Dim JSonString = Request.QueryString(1)
                Dim ThisToken As bootGrid.GridResult = Newtonsoft.Json.JsonConvert.DeserializeObject(Of bootGrid.GridResult)(JSonString)

                current = ThisToken.current
                rowCount = ThisToken.rowCount
                sort = ThisToken.sortBy
                'searchPhrase = Request.QueryString("searchPhrase")
                sortColumnName = sort
                sortOrderBy = ThisToken.sortDirection
                search = ThisToken.search
                'numberOfRows = IIf(String.IsNullOrEmpty(Request.QueryString("rows")), 10, Request.QueryString("rows"))
                'pageIndex = IIf(String.IsNullOrEmpty(Request.QueryString("page")), 1, Request.QueryString("page"))
                'search = IIf(String.IsNullOrEmpty(Request.QueryString("_search")), "", Request.QueryString("_search"))
                'filters = IIf(String.IsNullOrEmpty(Request.QueryString("filters")), "", Request.QueryString("filters"))
                numberOfRows = IIf(String.IsNullOrEmpty(rowCount), 5, rowCount)
                pageIndex = IIf(String.IsNullOrEmpty(current), 1, current)
                'search = IIf(String.IsNullOrEmpty(Request.QueryString("_search")), "", Request.QueryString("_search"))
                'filters = IIf(String.IsNullOrEmpty(Request.QueryString("filters")), "", Request.QueryString("filters"))


                _jqGrid.StrConn = strConexao

                'objeto para filtrar as buscas
                If search <> "" Then

                    For Each _item As String In _jqGrid.getColsNames(MontaQuery)
                        Dim _filter As New JQGridFilterRules
                        _filter.field = _item
                        _filter.data = search
                        _filter.op = "%"
                        arrayFilters.Add(_filter)
                    Next


                    'If Not String.IsNullOrEmpty(filters) Then
                    '    arrayFilters = _jqGrid.JsonToArray(filters.Substring(25, filters.Substring(25).Length - 1))
                    'Else
                    '    'busca pela janela
                    '    If Not String.IsNullOrEmpty(Request.QueryString("searchField")) Then
                    '        Dim _filter As New JQGridFilterRules
                    '        _filter.field = Request.QueryString("searchField").ToString
                    '        _filter.data = Request.QueryString("searchString").ToString
                    '        _filter.op = Request.QueryString("searchOper").ToString
                    '        arrayFilters.Add(_filter)
                    '    End If
                    'End If
                End If

                'sortColumnName = IIf(String.IsNullOrEmpty(Request.QueryString("sidx")), "", Request.QueryString("sidx"))
                'sortOrderBy = IIf(String.IsNullOrEmpty(Request.QueryString("sord")), 1, Request.QueryString("sord"))




                'monta a query do grid
                MontaQuery()
                CarregaTotal()
                CarregaData(numberOfRows, pageIndex)
                Response.End()
            End If
        Else

            If Request.Form("__EVENTTARGET") = "btnExcluir" Then
                ' btnExcluir_Click()
                Excluir()
            ElseIf Request.Form("__EVENTTARGET") = "btHistorico" Then

            End If

        End If

        'CarregaGridNovo()

    End Sub

    Private Sub CarregaGridNovo()
        Dim dt As DataTable = _dao_commons.myDataTable(MontaQuery)
        Me.gvGrid.DataSource = dt
        'Me.gvGrid.Visible = False
        If gvGrid.Rows.Count > 0 Then
            ' gvGrid.HeaderRow.TableSection = TableRowSection.TableHeader

        End If
        Me.gvGrid.DataBind()
        ' Me.gvGrid.Visible = False


    End Sub

    Private Function MontaQuery() As String
        '///////QUERY DO GRID/////////////////////////////////

        strSQL = ""
        strSQL = "SELECT nvl(r.numero_a,'') AS ID,  "
        strSQL = strSQL + " nvl(u.nome_usuario,'LIVRE') AS NOME,  "
        strSQL = strSQL + " nvl(g.nome_grupo, '') AS GRUPO,"
        strSQL = strSQL + " nvl(g.codigo, '') AS CCUSTO,  "
        'strSQL = strSQL + " nvl(u.senha_usuario, '') AS SENHA,  "
        strSQL = strSQL + " nvl(rm.modelo, 'SEM MODELO') AS MODELO  "
        'strSQL = strSQL + " Replace(Replace(Replace(Replace(Replace(Replace(to_char(r.credito_mensal, '$999,999,999.00'),'.','@'),',','.'),'@',','),' ',''), '$', 'R$ '),'R$ ,00','R$ 0,00') AS META,  "
        'strSQL = strSQL + " Replace(Replace(Replace(Replace(Replace(Replace(to_char(r.saldo_atual, '$999,999,999.00'),'.','@'),',','.'),'@',','),' ',''), '$', 'R$ '),'R$ ,00','R$ 0,00') AS SALDO  "
        strSQL = strSQL + "FROM USUARIOS u,  "
        strSQL = strSQL + "	GRUPOS g,  "
        strSQL = strSQL + "	RAMAIS r, RAMAIS_MODELOS RM,"
        strSQL = strSQL + "(  "
        strSQL = strSQL + "	SELECT DISTINCT  "
        strSQL = strSQL + "		g.CODIGO AS CODIGO_GRUPO  "
        strSQL = strSQL + "	FROM  "
        strSQL = strSQL + "		GRUPOS g,   "
        strSQL = strSQL + "		USUARIOS u,  "
        strSQL = strSQL + "		CATEGORIA_USUARIO cat  "
        strSQL = strSQL + "	WHERE  "
        strSQL = strSQL + "		u.CODIGO=" + Trim(Session("codigousuario"))
        strSQL = strSQL + " And cat.CODIGO_USUARIO = u.CODIGO  "
        strSQL = strSQL + "		AND (  "
        strSQL = strSQL + "			(  "
        strSQL = strSQL + "			cat.TIPO_USUARIO in ('AL','G','D','TD','TG')  "
        strSQL = strSQL + "			AND cat.CODIGO_GRUPO LIKE SUBSTR(TO_CHAR(g.CODIGO),1,LENGTH(cat.CODIGO_GRUPO))  "
        strSQL = strSQL + "			)  "
        strSQL = strSQL + "			OR cat.TIPO_USUARIO LIKE 'A' "
        strSQL = strSQL + "		)  "
        strSQL = strSQL + ") c  "
        'strSQL = strSQL + " WHERE r.numero_a=u.rml_numero_a(+) "
        strSQL = strSQL + " WHERE r.codigo_usuario=u.codigo(+) "
        strSQL = strSQL + "	AND r.grp_codigo=g.codigo "
        strSQL = strSQL + "	AND r.CODIGO_MODELO=rm.codigo_modelo(+) "
        strSQL = strSQL + "	AND r.grp_codigo=c.codigo_grupo "
        strSQL = strSQL + "	AND UPPER(r.NUMERO_A)<>'CELULAR' "
        strSQL = strSQL + "	AND UPPER(r.NUMERO_A)<>'SEM RAMAL' "

        '///////FIM QUERY DO GRID/////////////////////////////////

        Return strSQL

    End Function


    Public Sub CarregaData(ByVal numberOfRows As Integer, ByVal pageIndex As Integer)
        Dim strSQL2 As String = ""


        strSQL2 = _jqGrid.CarregaData(numberOfRows, pageIndex, sortColumnName, sortOrderBy, strSQL, arrayFilters)
        _jqGrid.StrConn = strConexao
        result = _jqGrid.CriaGridV2(strSQL2, pageIndex, Math.Ceiling(totalrecords / numberOfRows), totalrecords)

        Response.Write(result)
        'Response.Write("passou")
    End Sub

    Public Sub CarregaTotal()
        Dim strSQL2 As String
        Dim connection As New OleDbConnection(strConexao)
        strSQL2 = ""
        strSQL2 = "SELECT  count(*) total from (" & strSQL

        strSQL2 = strSQL2 + "	)"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL2
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                totalrecords = reader.Item("total")
            End While
        End Using


        'Response.Write("passou")
    End Sub


    Protected Sub Excluir()

        'Response.Write("entrou")
        'Response.End()

        If Me.textbox_hidden.Text <> "" Then
            Dim liks_codes As String() = textbox_hidden.Text.Split(New Char() {" "c})
            Dim teveErro As Boolean = False

            For Each item As String In liks_codes
                If Not String.IsNullOrEmpty(item) Then
                    Dim _registro As List(Of AppGeneric) = _dao_commons.GetGenericList(item, "CNPJ", "DESCRICAO", "FATURAS_CNPJS")

                    If _registro.Count > 0 Then
                        Dim _result As Boolean = _dao_commons.GenericRemove(item, DateTime.Now.ToString, Session("username_login"), "CNPJ", "DESCRICAO", "FATURAS_CNPJS", "FATURAS_CNPJS_LOG")

                        If _result Then
                            strResult += "CNPJ " & _registro.Item(0).Codigo & " exclúido com sucesso!"
                        Else
                            strResult += "CNPJ " & _registro.Item(0).Codigo & " não excluído! "
                            teveErro = True
                        End If
                    End If
                End If

            Next
            Me.lbMSG.Text = strResult

            If teveErro Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", " ExibeErro();", True)
            Else
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "ExibeExclusao();", True)
            End If



            Exit Sub

        End If

    End Sub

    Protected Sub btnExcluir_Click(sender As Object, e As System.EventArgs) Handles btnExcluir.Click
        Dim strResult As String = ""


        If textbox_hidden.Text <> "" Then
            Dim codes As String() = textbox_hidden.Text.Split(New Char() {" "c})
            Dim codes_list As New List(Of String)
            Dim ramal_list As New List(Of AppRamais)

            For Each item As String In codes
                If item <> "" Then
                    ramal_list = _dao.GetRamaisLivres(item)
                    If ramal_list.Count > 0 Then
                        If _dao.ExcluiRamal(item, Session("username_login")) Then
                            strResult = strResult + "Ramal " + item + " foi removido com sucesso!<br /><br />"
                        Else
                            strResult = strResult + "ERRO ! Operação (GRAVAR) NÃO realizada!Ramal " + item + "<br /><br />"
                        End If
                    Else
                        strResult = strResult + "O ramal " + item + " não está livre <br /><br />"
                    End If
                End If
            Next
        End If

        Me.lbMSG.Text = strResult
        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "ExibeExclusao();", True)

    End Sub

    Private Sub myAlert(ByVal msg As String, ByVal pClose As Boolean)


        Dim myscript As String = "alert(" & msg & ");"
        If pClose Then

            If ViewState("_reload") <> "N" Then
                myscript += "window.opener.location.reload();window.close();"
            Else
                myscript += "window.close();"
            End If

        End If

        If ViewState("novo") = "S" Then
            myscript += "__doPostBack('btNovo', '');"
        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)

        Dim strScript As String = "<script language=JavaScript>"
        strScript += "alert(""" & msg & """);"
        If pClose Then
            If ViewState("_reload") <> "N" Then
                strScript += "window.opener.location.reload();window.close();"
            Else
                strScript += "window.close();"
            End If
        End If
        If ViewState("novo") = "S" Then
            strScript += "__doPostBack('btNovo', '');"
        End If

        strScript += "</script>"

        If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", strScript)
        End If

    End Sub

    Protected Sub btHistorico_Click(sender As Object, e As System.EventArgs) Handles btHistorico.Click

        If textbox_hidden.Text <> "" Then

            Dim data_table As New DataTable
            Dim _registro_codes As String() = textbox_hidden.Text.Split(New Char() {" "c})
            Dim _registros As New List(Of String)
            Dim aux = 0

            For Each item As String In _registro_codes
                If aux >= 1 Then

                    _registros.Add(item)

                End If
                aux = aux + 1
            Next

            'Executa processamento do log para obter tabela
            data_table = _dao_commons.GetLogs(_registros, "NUMERO_A", "RAMAISLOG")

            'Formata cabeçalho da tabela
            'FormatTableHeader(data_table)

            'Passa contexto para paginá de logs
            Dim context As HttpContext = HttpContext.Current

            Session("Tabela") = data_table
            Session("Nome") = "Histórico de Ramais"
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "Open", "<script>window.open('GestaoHistoricos.aspx')</script>")
        Else
            Response.End()
        End If
    End Sub


End Class



