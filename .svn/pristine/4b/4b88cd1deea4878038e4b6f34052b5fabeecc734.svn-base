Imports System.Data
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System

Partial Class GestaoUsuarios
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
    Private _dao As New DAOUsuarios
    Private _dao_commons As New DAO_Commons
    Private current As String = "1"
    Private rowCount As String = "10"
    Private sort As String = ""
    Private searchPhrase As String = ""
    Public myUrl As String = ""
    Public strResult As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("conexao") = "Provider=OraOLEDB.Oracle;Password=light;User ID=light;Data Source=server;"
        myUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) & "/"


        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        Else
            strConexao = Session("conexao")
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_commons.strConn = Session("conexao").ToString

        If Not Page.IsPostBack Then
            operacao = Request.QueryString("operacao")
            If operacao = 1 Then

                'pegamos os parametros
                Dim JSonString = Request.QueryString(1)
                Dim ThisToken As bootGrid.GridResult = Newtonsoft.Json.JsonConvert.DeserializeObject(Of bootGrid.GridResult)(JSonString)

                current = ThisToken.Current
                rowCount = ThisToken.RowCount
                sort = ThisToken.SortBy
                'searchPhrase = Request.QueryString("searchPhrase")
                sortColumnName = sort
                sortOrderBy = ThisToken.sortDirection
                search = ThisToken.search
                'numberOfRows = IIf(String.IsNullOrEmpty(Request.QueryString("rows")), 10, Request.QueryString("rows"))
                'pageIndex = IIf(String.IsNullOrEmpty(Request.QueryString("page")), 1, Request.QueryString("page"))
                'search = IIf(String.IsNullOrEmpty(Request.QueryString("_search")), "", Request.QueryString("_search"))
                'filters = IIf(String.IsNullOrEmpty(Request.QueryString("filters")), "", Request.QueryString("filters"))
                numberOfRows = IIf(String.IsNullOrEmpty(rowCount), 10, rowCount)
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
                Historico()

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
        strSQL = " select distinct "
        strSQL = strSQL + " u.codigo AS ID, "
        strSQL = strSQL + "u.nome_usuario AS NOME, "
        strSQL = strSQL + "u.cargo_usuario AS CARGO_USUARIO, "
        strSQL = strSQL + "u.senha_usuario AS SENHA, "
        strSQL = strSQL + "u.rml_numero_a AS RAMAL,"
        strSQL = strSQL + "u.email_usuario AS EMAIL, "
        strSQL = strSQL + "u.login_usuario AS LOGIN, "
        strSQL = strSQL + "u.email_supervisor AS SUPERVISOR,"
        strSQL = strSQL + "g.nome_grupo AS GRUPO,"
        strSQL = strSQL + "g.codigo as CCUSTO,"
        strSQL = strSQL + "r.GRP_CODIGO as CCUSTO_RAMAL,"

        strSQL = strSQL + "u.matricula AS MATRICULA "

        'strSQL = strSQL + "Editar as EDITAR "
        strSQL = strSQL + " FROM USUARIOS u, "
        strSQL = strSQL + "	GRUPOS g, "
        strSQL = strSQL + "	RAMAIS r "

        strSQL = strSQL + " WHERE "
        strSQL = strSQL + "	r.numero_a(+)=u.rml_numero_a "
        strSQL = strSQL + "	AND U.grp_codigo=g.codigo(+)	"

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(g.codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        'strSQL = strSQL + "	AND U.GRP_CODIGO(+) = c.codigo_grupo "

        'Response.Write(strSQL)
        'Response.End()

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

    Protected Sub btnSyncLines_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSyncLines.Click

        Dim user_code_list As New List(Of AppGeneric)

        user_code_list = _dao_commons.GetGenericList("", "CODIGO", "GRP_CODIGO", "USUARIOS")

        For Each item As AppGeneric In user_code_list
            _dao.AtualizaCCUSTOS(item.Descricao, item.Codigo, "Administrador")
        Next
    End Sub

    Protected Sub Historico()

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
            data_table = _dao_commons.GetLogs(_registros, "CODIGO", "USUARIOSLOG")

            'Formata cabeçalho da tabela
            'FormatTableHeader(data_table)

            'Passa contexto para paginá de logs
            Dim context As HttpContext = HttpContext.Current

            Session("Tabela") = data_table
            Session("Nome") = "Histórico de Usuários"
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "Open", "<script>window.open('GestaoHistoricos.aspx')</script>")
        Else
            Response.End()
        End If
    End Sub

    Protected Sub Excluir()

        'Response.Write("entrou")
        'Response.End()

        If Me.textbox_hidden.Text <> "" Then
            Dim liks_codes As String() = textbox_hidden.Text.Split(New Char() {" "c})
            Dim teveErro As Boolean = False

            For Each item As String In liks_codes
                If Not String.IsNullOrEmpty(item) Then
                    Dim _registro As List(Of AppUsuarios) = _dao.GetUsuarioById(Convert.ToInt32(item))

                    If _registro.Count > 0 Then
                        Dim _result As Boolean = _dao.ExcluiUsuario(Convert.ToInt32(item), Session("username_login"), DateTime.Now.ToString)

                        If _result Then
                            strResult += "Usuário " & _registro.Item(0).Nome_Usuario & " exclúido com sucesso!"
                        Else
                            strResult += "Usuário " & _registro.Item(0).Nome_Usuario & " não excluído! "
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

End Class
