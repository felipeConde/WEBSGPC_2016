﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Collections.Generic
Imports System

Partial Class GestaoAparelhosMoveis
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
    Private current As String = "1"
    Private rowCount As String = "10"
    Private sort As String = ""
    Private searchPhrase As String = ""
    Public myUrl As String = ""
    Public strResult As String = ""
    Dim _exibeHistorico As Boolean = False


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

            End If

        End If

        'CarregaGridNovo()

    End Sub


    Private Function MontaQuery() As String
        '///////QUERY DO GRID/////////////////////////////////

        _exibeHistorico = Session("exibeHistorico")

        strSQL = ""
        strSQL = " select * from (Select distinct to_char(l.CODIGO_LINHA) AS ID,"
        strSQL = strSQL + " nvl(rpad(Replace(Replace(Replace(Replace(l.NUM_LINHA,')',''),' ',''),'(',''),'-',''),14,' '),' ')NUM_LINHA," '
        strSQL = strSQL + " nvl(ma.MARCA,'SEM MARCA') MARCA, nvl(mo.MODELO,'SEM MODELO') MODELO, "
        'strSQL = strSQL + " decode(nvl(lm.codigo_tecnologia,'0'), 1, 'CDMA', 2, 'TDMA', '3','GSM','4','RADIO','NÃO DEFINIDA' ) TECNOLOGIA, "
        strSQL = strSQL + " nvl(u.NOME_USUARIO,' ') NOME_USUARIO,"
        strSQL = strSQL + " nvl(u.MATRICULA,' ') MATRICULA,"
        strSQL = strSQL + " nvl(to_char(g.codigo),'-') CODIGO_GRUPO,"
        strSQL = strSQL + " nvl(g.NOME_GRUPO,'SEM GRUPO') GRUPO,"
        strSQL = strSQL + " nvl(lt.TIPO, '') AS CLASSIFICACAO,"
        strSQL = strSQL + " st.DESCRICAO STATUS, "
        strSQL = strSQL + " f.NOME_FANTASIA AS FORNECEDOR,nvl(a.IMEI,' ') IMEI, "
        strSQL = strSQL + " nvl(s.numero,' ') SIMCARD, "
        'strSQL = strSQL + " LISTAGG(nvl(tr.NUMERO,' '), ', ') WITHIN GROUP (ORDER BY tr.NUMERO) AS TERMO, "
	strSQL = strSQL + " '' AS TERMO, "
        strSQL = strSQL + " nvl(a.SERIAL_NUMBER,' ') SERIAL_NUMBER, "
        strSQL = strSQL + " nvl(lm.FLEET,' ') FLEET, "
        strSQL = strSQL + " case when (a.estoque='S' or a.backup='S' or a.sucata='S')then 'SIM' else 'NÃO' end ESTOQUE "
        'strSQL = strSQL + "'Editar' as EDITAR "
        strSQL = strSQL + " from "
        strSQL = strSQL + "     aparelhos_moveis a, linhas l, linhas_moveis lm, sim_cards s, usuarios u, "
        strSQL = strSQL + "     aparelhos_marcas ma, aparelhos_modelos mo, fornecedores f, TERMOS_RESPONSABILIDADE tr,"
        strSQL = strSQL + "      grupos g, grupos_item gi, status_linhas st, LINHAS_TIPO lt "
        strSQL = strSQL + " where"
        strSQL = strSQL + " l.CODIGO_LINHA = lm.CODIGO_LINHA and lm.CODIGO_APARELHO = a.CODIGO_APARELHO(+) and "
        strSQL = strSQL + " lm.CODIGO_SIM = s.CODIGO_SIM(+) and "
        strSQL = strSQL + " l.CODIGO_TIPO = lt.CODIGO_TIPO(+) and "
        strSQL = strSQL + " lm.CODIGO_LINHA = tr.CODIGO_LINHA(+) and "
        strSQL = strSQL + " a.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = ma.COD_MARCA(+) and "
        strSQL = strSQL + " l.CODIGO_FORNECEDOR = f.CODIGO(+) and "
        strSQL = strSQL + " l.codigo_linha=gi.item(+) and "
        strSQL = strSQL + " ( nvl(gi.modalidade,'4')='4'  or "
        strSQL = strSQL + "  nvl(gi.modalidade,'4')='1') and gi.grupo = g.codigo(+) and"
        strSQL = strSQL + " nvl(trim(l.STATUS),'1') = to_CHAR(st.CODIGO_STATUS(+)) and "
        strSQL = strSQL + " lm.codigo_usuario = u.codigo(+)  "

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
            strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            strSQL = strSQL + "     and to_char(gi.grupo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        strSQL = strSQL + " group by l.CODIGO_LINHA,ma.MARCA,mo.MODELO,u.NOME_USUARIO,  "
        strSQL = strSQL + " u.MATRICULA,g.codigo,g.NOME_GRUPO,lt.TIPO,st.DESCRICAO, "
        strSQL = strSQL + " l.NUM_LINHA,f.NOME_FANTASIA,a.IMEI,s.numero, "
        strSQL = strSQL + " a.SERIAL_NUMBER,lm.fleet,a.ESTOQUE,a.backup, "
        strSQL = strSQL + " a.sucata) "

        '///////FIM QUERY DO GRID/////////////////////////////////

        'If _exibeHistorico Then

        '    '///////////traz as linhas do log ///////////////////////
        '    strSQL = strSQL + " union "
        '    strSQL = strSQL + " Select distinct l.CODIGO_LINHA AS ID, nvl(ma.MARCA,'SEM MARCA') MARCA, nvl(mo.MODELO,'SEM MODELO') MODELO, "
        '    'strSQL = strSQL + " decode(nvl(lm.codigo_tecnologia,'0'), 1, 'CDMA', 2, 'TDMA', '3','GSM','4','RADIO','NÃO DEFINIDA' ) TECNOLOGIA, "
        '    strSQL = strSQL + " nvl(u.NOME_USUARIO,' ') NOME_USUARIO,"
        '    strSQL = strSQL + " nvl(u.MATRICULA,' ') MATRICULA,"
        '    strSQL = strSQL + " nvl(g.codigo,'-') CODIGO_GRUPO,"
        '    strSQL = strSQL + " nvl(g.NOME_GRUPO,'SEM GRUPO') GRUPO,"
        '    strSQL = strSQL + " nvl(lt.TIPO, '') AS CLASSIFICACAO,"
        '    strSQL = strSQL + " 'DESATIVADO' STATUS, "
        '    'strSQL = strSQL + " nvl(l.NUM_LINHA,' ')NUM_LINHA,"
        '    strSQL = strSQL + " nvl(rpad(l.NUM_LINHA,14,' '),' ') NUM_LINHA," '
        '    strSQL = strSQL + " f.NOME_FANTASIA AS FORNECEDOR,nvl(lo.IMEI,' ') IMEI, "
        '    strSQL = strSQL + "nvl(lo.SIM_CARD,' ') SIMCARD, "
        '    strSQL = strSQL + "LISTAGG(nvl(tr.NUMERO,' '), ', ') WITHIN GROUP (ORDER BY tr.NUMERO) AS TERMO, "
        '    strSQL = strSQL + "nvl(a.SERIAL_NUMBER,' ') SERIAL_NUMBER, "
        '    strSQL = strSQL + "nvl(lm.fleet,' ') ID_RADIO, "
        '    strSQL = strSQL + "case when (a.estoque='S' or a.backup='S' or a.sucata='S')then 'SIM' else 'NÂO' end ESTOQUE, "
        '    strSQL = strSQL + "'Editar' as EDITAR "
        '    strSQL = strSQL + " from "
        '    strSQL = strSQL + "     aparelhos_moveis a, linhas l, linhas_moveis lm, sim_cards s, usuarios u, "
        '    strSQL = strSQL + "     aparelhos_marcas ma, aparelhos_modelos mo, fornecedores f, TERMOS_RESPONSABILIDADE tr,linhas_moveis_log lo,"
        '    strSQL = strSQL + "      grupos g, grupos_item gi, status_linhas st "
        '    strSQL = strSQL + " where"
        '    strSQL = strSQL + " l.CODIGO_LINHA = lm.CODIGO_LINHA and lm.CODIGO_APARELHO = a.CODIGO_APARELHO(+) and "
        '    strSQL = strSQL + " lm.CODIGO_SIM = s.CODIGO_SIM(+) and "
        '    strSQL = strSQL + " a.COD_MODELO = mo.COD_MODELO(+) and mo.COD_MARCA = ma.COD_MARCA(+) and "
        '    strSQL = strSQL + " lo.CODIGO_FORNECEDOR = f.CODIGO(+) and "
        '    strSQL = strSQL + " lo.CODIGO_FORNECEDOR = f.CODIGO(+) and "
        '    strSQL = strSQL + " lm.CODIGO_LINHA = tr.CODIGO_LINHA(+) and "
        '    strSQL = strSQL + " l.codigo_linha=gi.item(+) and "
        '    strSQL = strSQL + " ( nvl(gi.modalidade,'4')='4'  or "
        '    strSQL = strSQL + "  nvl(gi.modalidade,'4')='1') and gi.grupo = g.codigo(+) and"
        '    strSQL = strSQL + " nvl(trim(lo.STATUS),'1') = to_CHAR(st.CODIGO_STATUS(+)) and "
        '    strSQL = strSQL + " lo.codigo_usuario = u.codigo(+)  "

        '    If Not DALCGestor.AcessoAdmin() Then
        '        'não filtra o centro de custo dos gerentes
        '        strSQL = strSQL + " and exists(" & vbNewLine
        '        strSQL = strSQL + "   select 0 from categoria_usuario cat" & vbNewLine
        '        strSQL = strSQL + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '        'strSQL = strSQL + "     and cat.tipo_usuario in('D','G')" & vbNewLine
        '        strSQL = strSQL + "     and to_char(gi.grupo) like cat.codigo_grupo||'%' ) " & vbNewLine
        '    End If

        '    strSQL = strSQL + " and lo.num_tel=l.num_linha "
        '    strSQL = strSQL + " and lo.CODIGO_FORNECEDOR<>l.CODIGO_FORNECEDOR "

        '    strSQL = strSQL + " group by l.CODIGO_LINHA,ma.MARCA,mo.MODELO,u.NOME_USUARIO,  "
        '    strSQL = strSQL + " u.MATRICULA,g.codigo,g.NOME_GRUPO,lt.TIPO,st.DESCRICAO, "
        '    strSQL = strSQL + " l.NUM_LINHA,f.NOME_FANTASIA,a.IMEI,s.numero, "
        '    strSQL = strSQL + " a.SERIAL_NUMBER,lm.fleet,a.ESTOQUE,a.backup, "
        '    strSQL = strSQL + " a.sucata,'Editar' "

        'End If

        'Response.Write(strSQL)
        'Response.End()

        Return strSQL

    End Function


    Public Sub CarregaData(ByVal numberOfRows As Integer, ByVal pageIndex As Integer)
        Dim strSQL2 As String = ""


        strSQL2 = _jqGrid.CarregaData(numberOfRows, pageIndex, sortColumnName, sortOrderBy, strSQL, arrayFilters)
        _jqGrid.StrConn = strConexao
        result = _jqGrid.CriaGridV2(strSQL2, pageIndex, Math.Ceiling(totalrecords / numberOfRows), totalrecords)
        'result = _jqGrid.CriaGridV2(strSQL2, pageIndex, Math.Ceiling(totalrecords / numberOfRows), totalrecords)

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

    Protected Sub btHistorico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btHistorico.Click, btHistorico_IMEI.Click, btHistorico_SIM.Click

        If textbox_hidden.Text <> "" Then

            Dim data_table As New DataTable
            Dim _codes As String() = textbox_hidden.Text.Split(New Char() {" "c})
            Dim registrys As New List(Of String)
            'Dim reg_aux As New List(Of AppLinks)
            Dim aux As Integer = 0

            If hidden_tipo.Text = "1" Then
                _codes = textHiddenNumber.Text.Split(New Char() {" "c})
            End If
            If hidden_tipo.Text = "2" Then
                _codes = textHiddenSIM.Text.Split(New Char() {" "c})
            End If
            If hidden_tipo.Text = "3" Then
                _codes = textHiddenIMEI.Text.Split(New Char() {" "c})
            End If

            For Each item As String In _codes
                If item.Replace(" ", "") <> "" Then
                    registrys.Add(item)
                End If
            Next

            'Executa processamento do log para obter tabela
            data_table = Resolve_table(registrys, hidden_tipo.Text)

            'Passa contexto para paginá de logs
            Dim context As HttpContext = HttpContext.Current

            Session("Tabela") = data_table
            Session("Nome") = "Histórico de Linha(s)"

            If hidden_tipo.Text = "1" Then
                Session("HTML_Context") = "<br /> Por número de Linha: <br />"
            ElseIf hidden_tipo.Text = "2" Then
                Session("HTML_Context") = "<br /> Por SIMCARD: <br />"
            Else
                Session("HTML_Context") = "<br /> Por IMEI: <br />"
            End If

            For Each item As String In registrys
                Session("HTML_Context") = Session("HTML_Context") + item + "  "
            Next

            ' ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoHistoricos.aspx')</script>")
            Dim script As String = "window.open('GestaoHistoricos.aspx');"
            ScriptManager.RegisterStartupScript(Me.upButtons, Me.upButtons.GetType(), "openWindow", script, True)
        Else
        End If
    End Sub

    Public Function Resolve_table(ByVal list As List(Of String), ByVal tipo_log As String) As DataTable

        Dim sql As String
        Dim table As New DataTable
        Dim aux As Integer = 0
        Dim aux_2 As Integer = 0
        'Variaveis tiradas CODIGO_LINHA

        sql = "select DATA,decode(nvl(TIPO, ''),'N','Criado','D','Excluído','A','Antes', 'B', 'Depois') as TIPO,AUTOR,replace(replace(replace(NUM_TEL,'(',''),')',''),'-','') as NUM_TEL,lm_log.CODIGO_GRUPO,us.nome_usuario as usuario,nvl((select op.descricao from operadoras_teste op where op.codigo = f.codigo_operadora), '') as Operadora,nvl(pl.plano, 'SEM PLANO') as PLANO,sl.descricao as STATUS,  to_char(ATIVACAO,'DD/MM/YYYY') as ATIVADO,to_char(DESATIVADO,'DD/MM/YYYY') as DESATIVADO,OEM, PROTOCOLO_CANCEL as PROTOCOLO,lm_log.imei as imei, SIM_CARD, SERIAL_NUMBER, OBS,VALOR_UNIT, to_char(VENC_GARAN,'DD/MM/YYYY') as VENC_GARANTIA, DESC_ACESS, PIM, PUC, HEXA, TERMO_RESP "
        sql = sql + " , lm_log.CONTRATO, NOTA_FISCAL, VENC_CONTA, COD_CONTA, lm_log.CODIGO, decode(nvl(NATU_OPERACAO, ''),'1','Proprio','2','Comodato','3','Locado') as NAT_OPERACAO, SERVICOS, MENSALIDADE, FLEET, CODIGO_CLIENTE, PIN_APARELHO, PIN2, PUK2, LIMITE_USO "
        sql = sql + " ,ESTOQUE, BACKUP, SUCATA, PROPRIEDADE_ESTOQUE, ORDEM_SERVICO, CHAMADO_RETIRADA, DATA_RETIRADA, to_char(lm_log.EMISSAO,'DD/MM/YYYY') as EMISSAO, PERDIDO,CONTA_CONTABIL "
        sql = sql + " from LINHAS_MOVEIS_LOG lm_log, status_linhas sl, operadoras_planos pl, fornecedores f, aparelhos_marcas ma, aparelhos_modelos mo, usuarios us "
        sql = sql + " where CODIGO_LINHA is not null "
        sql = sql + " and lm_log.codigo_usuario = us.codigo "
        sql = sql + " and sl.codigo_status = lm_log.status(+) "
        sql = sql + " and lm_log.codigo_plano = pl.codigo_plano(+) "
        sql = sql + "and lm_log.COD_MODELO = mo.COD_MODELO(+) "
        sql = sql + "and mo.COD_MARCA = ma.COD_MARCA(+) "
        sql = sql + "and lm_log.CODIGO_FORNECEDOR = f.CODIGO(+) "
        'Query do relatório

        If hidden_tipo.Text = "1" Then
            sql = sql + " and  replace(replace(replace(replace(replace(lm_log.NUM_TEL, '(', ''), ')', ''), '-', ''), '_', ''),' ','') in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        ElseIf hidden_tipo.Text = "2" Then

            sql = sql + " and lm_log.SIM_CARD in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        Else

            sql = sql + " and lm_log.IMEI in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        End If


        'order
        sql = sql + "order by codigo"

        'Response.Write(sql)
        'Response.End()

        table = _dao_commons.myDataTable(sql)

        Return table

    End Function

    'Protected Sub ts1_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ts1.CheckedChanged
    '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('O parametro de verificação de CNPJ foi atualizado na base!');</script>")

    '    _dao_commons.GenericRemove("verificaCNPJ", Session("username_login"), DateTime.Now.ToString, "NOME_PARAMETRO", "NOME_PARAMETRO", "PARAMETROS_SGPC")

    '    Dim data As New AppGeneric("verificaCNPJ", "CARGA_FATURA")
    '    Dim extras As New List(Of AppGeneric)


    '    If ts1.Checked = True Then
    '        extras.Add(New AppGeneric("VALOR_PARAMETRO", "S"))
    '        _dao_commons.GenericInsert(data, DateTime.Now.ToString, Session("username_login"), "NOME_PARAMETRO", "CATEGORIA", "PARAMETROS_SGPC", "", extras)
    '    Else
    '        extras.Add(New AppGeneric("VALOR_PARAMETRO", "N"))
    '        _dao_commons.GenericInsert(data, DateTime.Now.ToString, Session("username_login"), "NOME_PARAMETRO", "CATEGORIA", "PARAMETROS_SGPC", "", extras)
    '    End If

    'End Sub

End Class
