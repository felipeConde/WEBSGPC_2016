Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Imports ClosedXML.Excel

Partial Public Class gestaoRel_ConsumoLinhasResult
    Inherits System.Web.UI.Page

    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Dim celular As String = ""
    Dim dataini As String = ""
    Dim datafim As String = ""
    Public nome_mes As String = ""
    Public mes As String = ""
    Public ano As String = ""
    Public _TotalRows As Integer
    Public _TotalParticular As Double = 0.0
    Dim _dao As New DAO_Commons
    Dim tipo As String = ""
    Private total_valor As Decimal
    Dim label_codigo_ar As String = ""
    Dim label_nome_ar As String = ""
    Dim email As String = ""
    Dim grupo As String = ""

    Private Sub gestaoRel_ConsumoLinhasResult_Load(sender As Object, e As EventArgs) Handles Me.Load


        email = Request.QueryString("email")
        If Session("conexao") Is Nothing And email <> "1" Then
            Response.Write("conecte novamente")
            Response.End()
        ElseIf email = "1" Then
            'envio de email
            strConexao = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
            _dao.strConn = strConexao
            Session("conexao") = strConexao
        Else
            strConexao = Session("conexao")
            _dao.strConn = Session("conexao")
        End If


        'Response.Write(CultureInfo.CurrentCulture.Name)
        'Response.End()

        If Not Page.IsPostBack Then


            celular = Request.QueryString("celular")
            dataini = Request.QueryString("dataini")
            datafim = Request.QueryString("datafim")
            mes = Request.QueryString("mes")
            ano = Request.QueryString("ano")
            tipo = Request.QueryString("tipo")
            grupo = Request.QueryString("grupo")

            label_codigo_ar = _dao.getLabel("NOME_CCUSTO")
            label_nome_ar = "NOME " & _dao.getLabel("NOME_CCUSTO")

            If mes = "" Or ano = "" Then
                Response.Write("Infome o mês e ano!")
                Response.End()
            End If

            If email = "1" Then
                Dim wrapper As New Encrypt("clperi")

                'vamos criptografar para testes
                'celular = wrapper.EncryptData(celular)
                'mes = wrapper.EncryptData(mes)
                'ano = wrapper.EncryptData(ano)
                'Session("codigousuario") = wrapper.EncryptData(Request.QueryString("codigousuario"))

                'vamos descriptografar
                Session("codigousuario") = Request.QueryString("codigousuario")

                'celular = wrapper.DecryptData(celular.Replace(" ", "+"))
                mes = wrapper.DecryptData(mes.Replace(" ", "+"))
                ano = wrapper.DecryptData(ano.Replace(" ", "+"))
                grupo = wrapper.DecryptData(grupo.Replace(" ", "+"))
                Session("codigousuario") = wrapper.DecryptData(Session("codigousuario").Replace(" ", "+"))
                tipo = "HTML"


            End If
            nome_mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes)
            Dim ti As TextInfo = CultureInfo.CurrentCulture.TextInfo
            nome_mes = ti.ToTitleCase(nome_mes)
            lbdatenow.Text = Date.Now
            MontaQuery()

            'Dim strHtml As String = _dao.RenderControl(Me.gvRel)
            'Response.Write(strHtml)
            'Response.End()

        End If

    End Sub

    Private Sub MontaQuery()

        'Dim sqlCustoFixo = "(select nvl(sum(pa2.valor),0)custo_fixo from LANCAMENTOS_MENSAIS pa2 where pa2.codigo_usuario=p100.codigo_usuario and p100.linha=pa2.num_linha and pa2.descricao='CUSTO SERVIÇO' AND PA2.TIPO_RECURSO='1' and  pa2.referencia='" & mes & "/" & ano & "' )"

        Dim sql As String = "select grp_codigo """ & label_codigo_ar & """, nome_grupo """ & label_nome_ar & """ ,USUARIO , nvl(""NUM_LINHA"",'APARELHO ' || MODELO) LINHA, OPERADORA,PLANO,to_char(""SERVIÇO (R$)"")""SERVIÇO (R$)"", to_char(""PARCELA APARELHO (R$)"")""PARCELA APARELHO (R$)"", ""Num. Parcela"",to_char(""CUSTO GESTÃO (R$)"")""CUSTO GESTÃO (R$)"", to_char(""SERVIÇO (R$)"" + ""CUSTO GESTÃO (R$)"" + ""PARCELA APARELHO (R$)"") ""GASTO (R$)"" from ("
        sql = sql + " Select p100.*,op.plano,o.descricao operadora, u.nome_usuario USUARIO, g.nome_grupo,am.modelo from ( select nvl(sum(pa.gasto)+pa.rateio,0) ""SERVIÇO (R$)"", pa.""NUM_LINHA"",nvl(sum(pa.""PARCELA""),0) ""PARCELA APARELHO (R$)"" ,nvl(sum(pa.""CUSTO_FIXO""),0)/nvl(count(*)over (partition by  pa.""NUM_LINHA""),0) ""CUSTO GESTÃO (R$)"",pa.""CODIGO_USUARIO"", pa.qtd_parcelas, case when pa.num_parcela is not null then to_char(pa.num_parcela) || '/' ||  to_char(pa.qtd_parcelas) else '' end   ""Num. Parcela"", pa.grp_codigo,pa.codigo_operadora "
        sql = sql + " FROM V_LINHAS_PARCELAS_CUSTOS3 PA"
        sql = sql + "  where  pa.data(+)='" & mes & "/" & ano & "'"

        If grupo <> "" Then
            sql = sql + "     and to_char(PA.grp_codigo) like '" & grupo & "%' " & vbNewLine
        End If

        '#FILTRA SOMENTE AS LINHAS MOVEIS
        'sql += "  and PA.codigo_tipo='1' "

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and (exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario cat" & vbNewLine
            sql = sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     and to_char(PA.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
            sql = sql + ")"

        End If





        sql = sql + "  group by pa.""NUM_LINHA"",pa.""CODIGO_USUARIO"",pa.grp_codigo,pa.CODIGO_USUARIO,pa.grp_codigo,pa.codigo_operadora,pa.qtd_parcelas,pa.num_parcela, pa.rateio"
        'sql = sql + " order by  p1.grp_codigo, u.nome_usuario"
        sql = sql + ") p100, linhas l, operadoras_teste o, operadoras_planos op, linhas_moveis  lm, aparelhos_moveis ap, grupos g, usuarios u,linhas_tipo lt, aparelhos_modelos am"
        sql = sql + " where 1=1 "
        sql = sql + " And p100.codigo_operadora=o.codigo "
        sql = sql + " And p100.grp_codigo = g.codigo(+) "
        sql = sql + " And p100.codigo_usuario=u.codigo(+) "
        sql = sql + " And p100.num_linha=replace(replace(replace(replace(l.num_linha(+), '(', ''),')',''),'-',''),' ','') "
        sql = sql + " and op.codigo_plano(+) = l.codigo_plano  and l.codigo_linha = lm.codigo_linha(+) and lm.codigo_aparelho = ap.codigo_aparelho(+) and l.codigo_tipo=lt.codigo_tipo(+) and ap.cod_modelo=am.cod_modelo(+)"
        sql = sql + ") WHERE (""SERVIÇO (R$)"" + ""CUSTO GESTÃO (R$)"" + ""PARCELA APARELHO (R$)"") <> 0 "





        'Response.Write(sql)
        'Response.End()


        Dim dt As DataTable = _dao.myDataTable(sql)
        Dim _rowTotal As DataRow = dt.NewRow
        _rowTotal.Item(0) = "Total"
        _rowTotal.Item("SERVIÇO (R$)") = 0
        _rowTotal.Item("PARCELA APARELHO (R$)") = 0
        _rowTotal.Item("CUSTO GESTÃO (R$)") = 0
        _rowTotal.Item("GASTO (R$)") = 0

        For Each _row As DataRow In dt.Rows
            _rowTotal("SERVIÇO (R$)") += Convert.ToDouble(_row("SERVIÇO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("PARCELA APARELHO (R$)") += Convert.ToDouble(_row("PARCELA APARELHO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("CUSTO GESTÃO (R$)") += Convert.ToDouble(_row("CUSTO GESTÃO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("GASTO (R$)") += Convert.ToDouble(_row("GASTO (R$)").ToString.Replace(",", "").Replace(".", ","))

            _row("SERVIÇO (R$)") = FormatNumber(_row("SERVIÇO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("PARCELA APARELHO (R$)") = FormatNumber(_row("PARCELA APARELHO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("CUSTO GESTÃO (R$)") = FormatNumber(_row("CUSTO GESTÃO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("GASTO (R$)") = FormatNumber(_row("GASTO (R$)").ToString.Replace(",", "").Replace(".", ","))
            '_row(3) = FormatNumber(_row(3).ToString.Replace(",", "").Replace(".", ","))
        Next

        _rowTotal("SERVIÇO (R$)") = FormatNumber(_rowTotal("SERVIÇO (R$)"))
        _rowTotal("PARCELA APARELHO (R$)") = FormatNumber(_rowTotal("PARCELA APARELHO (R$)"))
        _rowTotal("CUSTO GESTÃO (R$)") = FormatNumber(_rowTotal("CUSTO GESTÃO (R$)"))
        _rowTotal("GASTO (R$)") = FormatNumber(_rowTotal("GASTO (R$)"))
        dt.Rows.Add(_rowTotal)

        Me.gvRel.DataSource = dt
        Me.gvRel.DataBind()



        If tipo.ToUpper = "EXCEL" Then
            Dim wb As New XLWorkbook()


            'Dim dt2 As New DataTable("Consumo Linhas ")
            'For z As Integer = 0 To gvRel.Columns.Count - 1
            '    dt2.Columns.Add(gvRel.Columns(z).HeaderText)
            'Next

            'For Each row As GridViewRow In gvRel.Rows
            '    dt2.Rows.Add()
            '    For c As Integer = 0 To row.Cells.Count - 1
            '        dt2.Rows(dt2.Rows.Count - 1)(c) = Server.HtmlDecode(row.Cells(c).Text.Replace("&nbsp;", ""))
            '    Next
            'Next

            'dt2.Rows.Add()
            'For c As Integer = 0 To gvRel.FooterRow.Cells.Count
            '    Try
            '        dt2.Rows(dt.Rows.Count - 1)(c) = gvRel.FooterRow.Cells(c).Text.Replace("&nbsp;", "")
            '    Catch ex As Exception

            '    End Try
            'Next

            wb.Worksheets.Add(dt)

            gvRel.AllowPaging = True

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=RelConsumo_" & mes & "_" & ano & ".xlsx")
            Response.Charset = "ISO-8859-1"
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.[End]()
            End Using

        End If
        If gvRel.Rows.Count > 0 Then
            gvRel.Rows(gvRel.Rows.Count - 1).CssClass = "active"
            gvRel.Rows(gvRel.Rows.Count - 1).Style.Add("font-weight", "bold")


            gvRel.HeaderRow.TableSection = TableRowSection.TableHeader
            'gvRel.FooterRow.TableSection = TableRowSection.TableFooter


        End If







    End Sub

    Private Sub MontaQueryOLD()

        Dim sql As String = ""

        sql = sql + " Select distinct "
        sql = sql + " p1.grp_codigo """ & label_codigo_ar & """, g.nome_grupo """ & label_nome_ar & """ , u.nome_usuario usuario, GET_OCULTA_NUMLINHA(p1.rml_numero_a) linha, o.descricao OPERADORA"
        sql = sql + " , NVL(OP.plano,'-') ""PLANO"", to_char(nvl(sum(nvl(p1.total_gasto, p1.valor_cdr))+nvl(r.rateio,0),0))""SERVIÇO (R$)"""
        sql = sql + " , to_char(nvl(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY')  and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then nvl(pa.parcela,0) else 0 end ,0))""PARCELA APARELHO (R$)"""
        sql = sql + " , to_char(nvl(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY')  and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then MONTHS_BETWEEN(to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'),to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY')) || '/' || pa.qtd_parcelas  else '0' end ,'0'))""Num. Parcela"""
        sql = sql + " , to_char(nvl(pa.custo_fixo, 0))""CUSTO GESTÃO (R$)"""
        'sql = sql + " , to_char(sum(nvl(p1.total_gasto, p1.valor_cdr)) + (nvl(ap.valor, 0) / nvl(ap.qtd_parcelas, 1)) + nvl(op.custo_fixo, 0)+p1.rateio) gasto"
        sql = sql + " , to_char(nvl(sum(nvl(p1.total_gasto, p1.valor_cdr)) + case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0  then nvl(pa.parcela,0) else 0 end + nvl(pa.custo_fixo,0)+nvl(r.rateio,0),0)) ""GASTO (R$)"""
        sql = sql + " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3, operadoras_teste o, "
        sql = sql + " tarifacao p4, operadoras_planos op, linhas l, linhas_moveis  lm, aparelhos_moveis ap, grupos g, usuarios u, LINHAS_TIPO lt, V_LINHAS_PARCELAS_CUSTOS2 pa, RateioGestao_MV r "
        sql = sql + " where p1.codigo_conta = p2.codigo_conta"
        sql = sql + " And p3.codigo_fatura = p2.codigo_fatura and p3.codigo_operadora=o.codigo "
        sql = sql + " and p1.codigo_conta=r.codigo_conta(+) and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        sql = sql + " And p1.tarif_codigo = p4.codigo(+)"
        sql = sql + " And p1.grp_codigo = g.codigo(+) "
        sql = sql + " And p1.codigo_usuario=u.codigo(+) "
        sql = sql + " And p1.rml_numero_a=replace(replace(replace(replace(l.num_linha(+), '(', ''),')',''),'-',''),' ','') "
        sql = sql + " and op.codigo_plano(+) = l.codigo_plano  and l.codigo_linha = lm.codigo_linha(+) and lm.codigo_aparelho = ap.codigo_aparelho(+) and l.codigo_tipo=lt.codigo_tipo(+)"
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        'sql += "  and to_char(p3.dt_vencimento, 'MM/YYYY')=pa.data "
        sql = sql + " and  pa.data(+)='" & mes & "/" & ano & "'    "

        'sql += "   and p1.rml_numero_a=pa.num_linha(+)"
        'sql = sql + " and lm.codigo_usuario(+) = p1.codigo_usuario"
        sql = sql + " and  to_char(p3.dt_vencimento, 'MM/YYYY')='" & mes & "/" & ano & "'    "

        If grupo <> "" Then
            sql = sql + "     and to_char(p1.grp_codigo) like '" & grupo & "%' " & vbNewLine
        End If

        '#FILTRA SOMENTE AS LINHAS MOVEIS
        sql += "  and P3.codigo_tipo='1' "

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and (exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario cat" & vbNewLine
            sql = sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
            sql = sql + ")"

        End If

        'tira linha rateada
        sql = sql + "    and not exists"
        sql = sql + "   (select 0 from rateio_faturas ra"
        sql = sql + "    where ra.codigo_fatura = p3.codigo_fatura "
        sql = sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & _data & "')"
        sql = sql + " ) "
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t , franquias f where t.codigo_franquia= f.codigo and t.servico=p1.tipo_serv2 and f.codigo_fatura= p2.codigo_fatura )"

        sql = sql + "  group by to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'),nvl(r.rateio,0),"
        sql = sql + " nvl(pa.PARCELA,0),pa.fim_parcela,pa.inicio_parcela,pa.qtd_parcelas,"
        sql = sql + " nvl(pa.custo_fixo,0),"
        'sql = sql + " nvl(ap.valor, 0),"
        sql = sql + " p1.rml_numero_a,p1.rateio,"
        sql = sql + " p1.grp_codigo, g.nome_grupo"
        sql = sql + " , u.nome_usuario,NVL(op.plano,'-'), o.descricao "
        sql = sql + " order by  p1.grp_codigo, u.nome_usuario"





        'Response.Write(sql)
        'Response.End()


        Dim dt As DataTable = _dao.myDataTable(sql)
        Dim _rowTotal As DataRow = dt.NewRow
        _rowTotal.Item(0) = "Total"
        _rowTotal.Item("SERVIÇO (R$)") = 0
        _rowTotal.Item("PARCELA APARELHO (R$)") = 0
        _rowTotal.Item("CUSTO GESTÃO (R$)") = 0
        _rowTotal.Item("GASTO (R$)") = 0

        For Each _row As DataRow In dt.Rows
            _rowTotal("SERVIÇO (R$)") += Convert.ToDouble(_row("SERVIÇO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("PARCELA APARELHO (R$)") += Convert.ToDouble(_row("PARCELA APARELHO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("CUSTO GESTÃO (R$)") += Convert.ToDouble(_row("CUSTO GESTÃO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _rowTotal("GASTO (R$)") += Convert.ToDouble(_row("GASTO (R$)").ToString.Replace(",", "").Replace(".", ","))

            _row("SERVIÇO (R$)") = FormatNumber(_row("SERVIÇO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("PARCELA APARELHO (R$)") = FormatNumber(_row("PARCELA APARELHO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("CUSTO GESTÃO (R$)") = FormatNumber(_row("CUSTO GESTÃO (R$)").ToString.Replace(",", "").Replace(".", ","))
            _row("GASTO (R$)") = FormatNumber(_row("GASTO (R$)").ToString.Replace(",", "").Replace(".", ","))
            '_row(3) = FormatNumber(_row(3).ToString.Replace(",", "").Replace(".", ","))
        Next

        _rowTotal("SERVIÇO (R$)") = FormatNumber(_rowTotal("SERVIÇO (R$)"))
        _rowTotal("PARCELA APARELHO (R$)") = FormatNumber(_rowTotal("PARCELA APARELHO (R$)"))
        _rowTotal("CUSTO GESTÃO (R$)") = FormatNumber(_rowTotal("CUSTO GESTÃO (R$)"))
        _rowTotal("GASTO (R$)") = FormatNumber(_rowTotal("GASTO (R$)"))
        dt.Rows.Add(_rowTotal)

        Me.gvRel.DataSource = dt
        Me.gvRel.DataBind()



        If tipo.ToUpper = "EXCEL" Then
            Dim wb As New XLWorkbook()


            'Dim dt2 As New DataTable("Consumo Linhas ")
            'For z As Integer = 0 To gvRel.Columns.Count - 1
            '    dt2.Columns.Add(gvRel.Columns(z).HeaderText)
            'Next

            'For Each row As GridViewRow In gvRel.Rows
            '    dt2.Rows.Add()
            '    For c As Integer = 0 To row.Cells.Count - 1
            '        dt2.Rows(dt2.Rows.Count - 1)(c) = Server.HtmlDecode(row.Cells(c).Text.Replace("&nbsp;", ""))
            '    Next
            'Next

            'dt2.Rows.Add()
            'For c As Integer = 0 To gvRel.FooterRow.Cells.Count
            '    Try
            '        dt2.Rows(dt.Rows.Count - 1)(c) = gvRel.FooterRow.Cells(c).Text.Replace("&nbsp;", "")
            '    Catch ex As Exception

            '    End Try
            'Next

            wb.Worksheets.Add(dt)

            gvRel.AllowPaging = True

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=RelConsumo_" & mes & "_" & ano & ".xlsx")
            Response.Charset = "ISO-8859-1"
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.[End]()
            End Using

        End If
        If gvRel.Rows.Count > 0 Then
            gvRel.Rows(gvRel.Rows.Count - 1).CssClass = "active"
            gvRel.Rows(gvRel.Rows.Count - 1).Style.Add("font-weight", "bold")


            gvRel.HeaderRow.TableSection = TableRowSection.TableHeader
            'gvRel.FooterRow.TableSection = TableRowSection.TableFooter


        End If







    End Sub
    Protected Sub gvRel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gvRel.SelectedIndexChanged

    End Sub

    Private Sub gvRel_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvRel.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(6).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(7).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(8).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(9).HorizontalAlign = HorizontalAlign.Right
        End If

    End Sub
End Class
