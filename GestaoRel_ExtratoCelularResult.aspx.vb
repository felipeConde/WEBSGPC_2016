Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports System.IO
Imports ClosedXML.Excel


Partial Class GestaoRel_ExtratoCelularResult
    Inherits System.Web.UI.Page

    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Public celular As String = ""
    Public dataini As String = ""
    Public datafim As String = ""
    Public nome_mes As String = ""
    Public mes As String = ""
    Public ano As String = ""
    Dim _editParticulares As Boolean = False
    Dim _diaApontamento As String = "25"
    Dim _dataLimiteApontamento As Date
    Public _TotalRows As Integer
    Public _TotalParticular As Double = 0.0
    Dim _dao As New DAO_Commons
    Dim tipo As String = ""
    Private total_valor As Decimal
    Private total_valor_geral As Decimal = 0
    Private total_chamadas As Integer
    Private total_duracao As Decimal
    Private total_duracao_resumo As Decimal
    Private total_valor_resumo As Decimal
    Private total_qtd_resumo As Decimal
    Private total_valor_extrato As Decimal = 0
    Private total_valor_audit_extrato As Decimal = 0
    Private total_valor_servico As Decimal = 0
    Private total_minutos_extrato As Decimal = 0
    Private total_dados As Decimal = 0
    Private fatura As String = ""
    Private valor_rateio As Double = 0
    Public _descFatura As String = ""
    Public exibe_colunas_audit As Boolean = False
    Public nome_gestao_telecom_fixo As String = ""
    Dim email As String = ""


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'pega as sessions do asp 3.0
        'GetSessions()

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

        If Not Page.IsPostBack Then

            'If email <> "1" Then

            'Else
            'nome_mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Request.QueryString("mes"))
            'End If



            celular = Request.QueryString("celular")
                dataini = Request.QueryString("dataini")
                datafim = Request.QueryString("datafim")
                mes = Request.QueryString("mes")
                ano = Request.QueryString("ano")
                tipo = Request.QueryString("tipo")
                fatura = Request.QueryString("fatura")
                If fatura <> "" Then
                    If fatura.Trim.Contains(" ") Then
                        fatura = fatura.Replace(" ", ",")
                        fatura = " " & fatura & " "
                        fatura = fatura.Replace(" ,", "").Replace(", ", "")
                    End If
                Else
                    For Each item As String In PegaCodigoFaturas()
                        If fatura = "" Then
                            fatura = item
                        Else
                            fatura = fatura & "," & item
                        End If
                    Next
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

                celular = wrapper.DecryptData(celular.Replace(" ", "+"))
                mes = wrapper.DecryptData(mes.Replace(" ", "+"))
                ano = wrapper.DecryptData(ano.Replace(" ", "+"))
                Session("codigousuario") = wrapper.DecryptData(Session("codigousuario").Replace(" ", "+"))
                tipo = "HTML"


            End If
            nome_mes = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(mes)
            Dim ti As TextInfo = CultureInfo.CurrentCulture.TextInfo
            nome_mes = ti.ToTitleCase(nome_mes)

            'Dim end_query As String = " and exists (select * from linhas l, "
            'end_query = end_query + " linhas_moveis lm where l.codigo_linha = lm.codigo_linha"
            'end_query = end_query + " and codigo = lm.codigo_usuario and replace(replace(replace(REPLACE(l.NUM_LINHA(+),')',''),'(',''),'-',''),' ','') ='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + "')"

            Dim end_query As String = " and exists(select distinct 0 from cdrs_celular p1, faturas_arquivos fa, faturas f where p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura and replace(p1.rml_numero_a,' ','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + "' and p1.codigo_usuario=u.codigo "
                If fatura <> "" Then
                    end_query = end_query + " and f.codigo_fatura in(" & fatura & ")"
                End If
                end_query = end_query + " and to_char(f.dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "' "
                end_query = end_query + " and trim(replace(p1.rml_numero_a,' ','')) = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
                If dataini <> "" And datafim <> "" Then
                    end_query = end_query + "   and p1.data_inicio>=to_date('" + dataini + " 00:00:00','DD/MM/YYYY HH24:MI:SS')"
                    end_query = end_query + "   and p1.data_inicio<=to_date('" + datafim + " 23:59:59','DD/MM/YYYY HH24:MI:SS')"
                End If
                end_query = end_query + " )"


                If _dao.GetGenericList("", "codigo", "nome_usuario", "usuarios u", "", end_query).Count > 0 Then
                    lbUsuario.Text = _dao.GetGenericList("", "codigo", "nome_usuario", "usuarios u", "", end_query).Item(0).Descricao
                    lbUsuarioTop.Text = lbUsuario.Text
                Else
                    lbUsuario.Text = "SEM USUÁRIO"
                End If
            'If _dao.Get_CellPhoneType(celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "")) <> "" Then
            '    lbTipo.Text = _dao.Get_CellPhoneType(celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", ""))
            'Else
            '    lbTipo.Text = "NÃO CADASTRADO"
            'End If

            'pega o modelo
            Dim sqlModelo As String = "select mo.modelo from linhas l,linhas_moveis lm, aparelhos_moveis am, aparelhos_modelos mo where l.codigo_linha=lm.codigo_linha and lm.codigo_aparelho=am.codigo_aparelho and am.cod_modelo=mo.cod_modelo and  replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" & celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & "'"
            Dim dtModelo As DataTable = _dao.myDataTable(sqlModelo)

            If dtModelo.Rows.Count > 0 Then
                Me.lbTipo.Text = dtModelo.Rows(0).Item(0)
            End If


            'pega o plano
            Dim sqlPlano As String = "select op.plano from linhas l, operadoras_planos op where l.codigo_plano=op.codigo_plano and replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')='" & celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") & "'"
                Dim dtPlano As DataTable = _dao.myDataTable(sqlPlano)

                If dtPlano.Rows.Count > 0 Then
                    Me.lbPlano.Text = dtPlano.Rows(0).Item(0)
                End If


            Dim dtLinha As DataTable = _dao.myDataTable("select GET_OCULTA_NUMLINHA('" & celular & "') from dual ")

            If dtLinha.Rows.Count > 0 Then
                    Me.lbLinha.Text = dtLinha.Rows(0).Item(0)
                End If


                'NOME_GESTAO_TELECOM
                nome_gestao_telecom_fixo = _dao.getLabel("NOME_GESTAO_TELECOM")

                MontaQuery()
                VerificaParticulares()
                CarregaOperadora()
                lbdatenow.Text = DateTime.Now.ToString


                If Not _dao.Is_Administrator(Session("codigo_usuario")) Then

                    'oculta a coluna de auditoria
                    Me.gvExtrato.Columns(14).Visible = False
                    Me.gvExtrato.Columns(15).Visible = False
                    Me.gvExtrato.Columns(17).Visible = False

                End If

                'deixa so a coluna total
                If AppIni.ExibeSoTotalExtrato Then
                    Me.gvExtrato.Columns(13).Visible = False
                    Me.gvExtrato.Columns(14).Visible = False
                    Me.gvExtrato.Columns(15).Visible = False
                    Me.gvExtrato.Columns(17).Visible = False
                    Me.gvExtrato.Columns(16).HeaderText = "Valor"
                End If

                Me.gvExtrato.Columns(2).Visible = False
                Me.gvExtrato.Columns(3).Visible = False

                If _editParticulares = False Then
                    Me.gvExtrato.Columns(20).Visible = False
                End If

                gvExtrato.HeaderRow.TableSection = TableRowSection.TableHeader
                gvExtrato.FooterRow.TableSection = TableRowSection.TableFooter

                If tipo.ToUpper = "EXCEL" Then
                    ''doExcel()
                    'Response.ContentType = "application/vnd.ms-excel"
                    'Response.AddHeader("content-disposition", "attachment;filename=ExtratoCel_" & celular & "_" & mes & "_" & ano & ".xls")
                    'Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252")
                    'Response.Charset = "ISO-8859-1"
                    'EnableViewState = False

                    'Response.Write("<style> td{text-align: right;mso-number-format: \@;white-space: nowrap;} </style>")

                    Dim wb As New XLWorkbook()


                    Dim dt As New DataTable("Resumo Linha " & celular)
                    For z As Integer = 0 To GvResumo.Columns.Count - 1
                        dt.Columns.Add(GvResumo.Columns(z).HeaderText)
                    Next

                    For Each row As GridViewRow In GvResumo.Rows
                        dt.Rows.Add()
                        For c As Integer = 0 To row.Cells.Count - 1
                            dt.Rows(dt.Rows.Count - 1)(c) = Server.HtmlDecode(row.Cells(c).Text.Replace("&nbsp;", ""))
                        Next
                    Next

                    dt.Rows.Add()
                    For c As Integer = 0 To GvResumo.FooterRow.Cells.Count
                        Try
                            dt.Rows(dt.Rows.Count - 1)(c) = GvResumo.FooterRow.Cells(c).Text.Replace("&nbsp;", "")
                        Catch ex As Exception

                        End Try
                    Next

                    wb.Worksheets.Add(dt)

                    GvResumo.AllowPaging = True

                    Dim dt2 As New DataTable("Extrato Linha " & celular)
                    For z As Integer = 0 To gvExtrato.Columns.Count - 1
                        dt2.Columns.Add(gvExtrato.Columns(z).HeaderText)
                    Next

                    For Each row As GridViewRow In gvExtrato.Rows
                        dt2.Rows.Add()
                        For c As Integer = 0 To row.Cells.Count - 1
                            dt2.Rows(dt2.Rows.Count - 1)(c) = Server.HtmlDecode(row.Cells(c).Text.Replace("&nbsp;", ""))
                        Next
                    Next

                    dt2.Rows.Add()
                    For c As Integer = 0 To gvExtrato.FooterRow.Cells.Count
                        Try
                            dt2.Rows(dt2.Rows.Count - 1)(c) = gvExtrato.FooterRow.Cells(c).Text.Replace("&nbsp;", "")
                        Catch ex As Exception

                        End Try
                    Next


                    dt2.Columns.Remove("OBS")
                    dt2.Columns.Remove("FATURADO")
                    dt2.Columns.Remove("CODIGO_CONTA")
                    dt2.Columns.Remove("VALOR_OK")
                    dt2.Columns.Remove("TARIFA")
                    dt2.Columns.Remove("Column1")

                    wb.Worksheets.Add(dt2)

                    gvExtrato.AllowPaging = True

                    Response.Clear()
                    Response.Buffer = True
                    Response.Charset = ""
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
                    Response.AddHeader("content-disposition", "attachment;filename=ExtratoCel_" & celular & "_" & mes & "_" & ano & ".xlsx")
                    Response.Charset = "ISO-8859-1"
                    Using MyMemoryStream As New MemoryStream()
                        wb.SaveAs(MyMemoryStream)
                        MyMemoryStream.WriteTo(Response.OutputStream)
                        Response.Flush()
                        Response.[End]()
                    End Using

                End If


            End If

    End Sub


    Private Sub MontaQuery()

        Dim sql As String = ""

        sql = sql + "select * from (select p1.codigo, GET_OCULTA_NUMLINHA(replace(p1.rml_numero_a, ' ', '')) ramal,"
        sql = sql + " CDR_CODIGO as CDR_CODIGO, nvl(p1.DESTINO, ' ') fisico,"
        sql = sql + " nvl(origem, 'SEM INFO') origem, codigo_tipo_ligacao tipo,"
        sql = sql + " decode(nvl(p2.nome_configuracao, 'DEFAULT'), 'DEFAULT', 'OUTROS SERVIÇOS',"
        sql = sql + " p2.nome_configuracao) categoria, p1.numero_b numero, p1.data_inicio dataini,"
        sql = sql + " p1.data_fim datafim,nvl(p1.route, '[NULO]') rota,"
        sql = sql + " nvl(round(((p1.data_fim-p1.data_inicio)*(1440)),28), 0) duracao, f.descricao fatura,"
        'sql = sql + " nvl(p1.valor_cdr, 0) valor,nvl(p1.valor_audit, 0) valor_audit,"
        'sql = sql + " nvl(case when p1.valor_cdr = 0 and exists (select 0 from rateio_faturas rf, faturas_arquivos fa where rf.rateio_tipo='3' and rf.codigo_fatura=fa.codigo_fatura and fa.codigo_conta=p1.codigo_conta ) then p1.valor_franquia else p1.valor_cdr end , 0) valor,"
        sql = sql + " nvl(p1.valor_cdr, 0) valor,"
        sql = sql + " nvl((case when p1.tipo_serv2 in (select fs.servico from franquias_servicos fs, franquias fr where fs.codigo_franquia=fr.codigo "
        If fatura <> "" Then
            sql = sql + " and fr.codigo_fatura in(" & fatura & ") "
        End If
        ''tira as chamadas intragrupo (tarifa zero)- pedido CL
        'sql = sql + " AND NOT( "
        'sql = sql + " exists (select 0 from linhas p2 where replace(replace(replace(p2.num_linha,'(',''),')',''),'-','')=p1.rml_numero_a and p2.intragrupo='S') "
        'sql = sql + " and exists ( select 0 from linhas p2 where replace(replace(replace(p2.num_linha,'(',''),')',''),'-','')=replace(replace(replace(replace(p1.numero_b,'(',''),')',''),'-',''),' ','') and p2.codigo_fornecedor in(select p4.codigo from fornecedores p4 where p4.codigo_operadora IN(select codigo_operadora from faturas where codigo_fatura in (select codigo_fatura from faturas_arquivos where codigo_conta=p1.codigo_conta)))) "
        'sql = sql + " ) "

        sql = sql + " ) and p1.valor_cdr=0  "
        sql = sql + " then round(p1.valor_franquia,4) else 0 end),0) valor_rateio,"
        'sql = sql + " nvl(p1.valor_audit, 0) valor_audit,"
        sql = sql + " nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0) valor_audit,"
        sql = sql + " 0 valor_total,"

        sql = sql + " nvl(p1.tipo_serv, '-') tipo_serv,nvl(p1.tipo_serv2, '-') tipo_serv2,"
        sql = sql + " nvl(p1.obs, '') obs,nvl(p1.faturado, 0) faturado,nvl(p1.codigo_conta, 0) codigo_conta,"
        sql = sql + " nvl(p1.valor_ok, '0') valor_ok, nvl(p1.tarif_codigo,0) tarif_codigo,"
        sql = sql + " decode(nvl(particular,'N'),'S','true','N','false') particular"
        sql += "  , nvl(round(p1.dados_KB/1024,2),0) ""VALOR TRAFEGADO(MB)"" "
        sql = sql + " from CDRS_CELULAR p1, tarifacao p2, faturas_arquivos fa, faturas f"
        sql = sql + " where 1 = 1 and p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura "
        sql = sql + " and exists (select a.codigo_conta"
        sql = sql + " from faturas f, faturas_arquivos a"
        sql = sql + " where f.codigo_fatura = a.codigo_fatura"
        sql = sql + " and a.codigo_conta = p1.codigo_conta"
        If fatura <> "" Then
            sql = sql + " and f.codigo_fatura in(" & fatura & ")"
        End If

        sql = sql + " and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "')"

        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=fa.codigo_fatura and servico=p1.tipo_serv2))"
        sql = sql + " and p1.tarif_codigo = p2.codigo(+)"
        sql = sql + " and REPLACE(REPLACE(REPLACE(REPLACE(p1.rml_numero_a,')',''),'(',''),'-',''),' ','')= '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        If dataini <> "" And datafim <> "" Then
            sql = sql + "   and p1.data_inicio>=to_date('" + dataini + " 00:00:00','DD/MM/YYYY HH24:MI:SS')"
            sql = sql + "   and p1.data_inicio<=to_date('" + datafim + " 23:59:59','DD/MM/YYYY HH24:MI:SS')"
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and (exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario cat" & vbNewLine
            sql = sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine

            'usuário comum
            sql = sql + "  or exists( select rml_numero_a from usuarios where upper(nome_usuario) in(select upper(nome_usuario) from usuarios where codigo='" + Trim(Session("codigousuario")) + "')"
            sql = sql + "     and p1.rml_numero_a = '" + celular.Trim + "')"
            sql = sql + ")"

        End If

        sql += "union " & CarregaValorRateio()



        'pega os parcelamento
        'sql += " union "
        'sql += " select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,'' fatura, 0 valor,nvl(op.custo_fixo,0)  valor_rateio,0 valor_audit, 0 VALOR_TOTAL, '" & nome_gestao_telecom_fixo & "'  tipo_serv, '" & nome_gestao_telecom_fixo & "'  tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular, 0 ""VALOR TRAFEGADO(MB)""  "
        'sql += " from operadoras_planos op, linhas l "
        'sql += " where op.codigo_plano=l.codigo_plano and REPLACE(REPLACE(REPLACE(REPLACE(l.NUM_LINHA,')',''),'(',''),'-',''),' ','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        ''sql += " and op.custo_fixo is not null "
        'sql += " "
        'sql += " union "
        'sql += " select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,'' fatura, 0 valor,nvl(pa.PARCELA,0) valor_rateio,0 valor_audit, 0 VALOR_TOTAL,'Parcela do aparelho (' || MONTHS_BETWEEN(to_date('" + mes + "/" + ano + "','MM/YYYY'),to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY')) || '/' || pa.qtd_parcelas ||')'   tipo_serv,'Parcela do aparelho'  tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular,0 ""VALOR TRAFEGADO(MB)""  "
        'sql += " from  V_LINHAS_PARCELAS_CUSTOS pa "
        'sql += "  where  replace(replace(replace(replace(pa.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'sql += "  and nvl(pa.qtd_parcelas,1) - MONTHS_BETWEEN (TO_DATE('" + mes + "/" + ano + "','MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>0"
        'sql += "  and  MONTHS_BETWEEN (TO_DATE('" + mes + "/" + ano + "','MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0"
        'sql += "  and rownum<2 "

        'pega os parcelamento e custo fixo - Novo
        sql += " union "
        sql += " select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,'' fatura, 0 valor,nvl(pa.custo_servico,0)  valor_rateio,0 valor_audit, 0 VALOR_TOTAL, '" & nome_gestao_telecom_fixo & "'  tipo_serv, '" & nome_gestao_telecom_fixo & "'  tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular, 0 ""VALOR TRAFEGADO(MB)""  "
        sql += " from lancamentos_mensais pa, operadoras_teste p4, linhas l,linhas_moveis lm,aparelhos_moveis ap,aparelhos_modelos  am,aparelhos_marcas ma,OPERADORAS_PLANOS op"
        sql += " where pa.id_item=ap.codigo_aparelho and lm.codigo_aparelho=ap.codigo_aparelho and lm.codigo_linha=l.codigo_linha "
        sql += " and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and pa.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and pa.tipo_recurso=1"
        sql += " and pa.referencia='" + mes + "/" + ano + "'"
        sql += " and pa.num_linha = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        'sql += " and pa.num_linha is null"
        'sql += " and op.custo_fixo is not null "
        sql += " "
        sql += " union "
        sql += " select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,'' fatura, 0 valor,nvl(pa.valor,0) valor_rateio,0 valor_audit, 0 VALOR_TOTAL,'Parcela do aparelho (' || pa.num_parcela || '/' || pa.qtd_parcelas ||')'   tipo_serv,'Parcela do aparelho'  tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular,0 ""VALOR TRAFEGADO(MB)""  "
        sql += " from lancamentos_mensais pa, operadoras_teste p4, linhas l,linhas_moveis lm,aparelhos_moveis ap,aparelhos_modelos  am,aparelhos_marcas ma,OPERADORAS_PLANOS op"
        sql += " where pa.id_item=ap.codigo_aparelho and lm.codigo_aparelho=ap.codigo_aparelho and lm.codigo_linha=l.codigo_linha "
        sql += " and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and pa.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and pa.tipo_recurso=1"
        sql += " and pa.referencia='" + mes + "/" + ano + "'"
        sql += " and pa.num_linha = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"



        sql = sql + ") order by ramal,dataini"
        'sql = sql + ") "

        'Response.Write(sql)
        'Response.End()

        Dim sqlFaturas As String = "select distinct fatura from (" & sql & ")"
        Dim dt As DataTable = _dao.myDataTable(sql)
        If dt.Rows.Count > 0 Then
            _descFatura = dt.Rows(0).Item(0)
        End If


        Dim connection As New OleDbConnection(strConexao)

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = sql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        If Not reader.HasRows Then

            'não tem extrato no mês
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('Nenhum registro encontrado.');window.close();</script>")


        End If

        Me.gvExtrato.DataSource = reader
        Me.gvExtrato.DataBind()
        _TotalRows = Me.gvExtrato.Rows.Count - 2



        reader.Close()
        reader = Nothing

        sql = "select * from("
        sql = sql + " select nvl(nvl(p1.tipo_serv,p1.tipo_serv2), 'SEM CLASSIFICAÇÃO') as categoria,"
        sql = sql + " count(*) qtd,"
        sql = sql + " sum(nvl(round(((p1.data_fim-p1.data_inicio)*(1440)),28), 0)) duracao,"
        If VerificaContestacao() Then
            'sql = sql + " sum(nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0)) total,"
            sql = sql + " sum(case when nvl(p1.valor_cdr,0)<>0 then nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0) else nvl(round(p1.valor_franquia,4),0) end) total,"
        Else
            sql = sql + " sum(nvl(p1.valor_cdr, 0)) total, "
        End If
        sql = sql + " 1 ordem"
        sql = sql + " from CDRS_CELULAR p1, tarifacao p2"
        sql = sql + " where(p1.tarif_codigo = p2.codigo(+))"
        sql = sql + " and REPLACE(REPLACE(REPLACE(REPLACE(p1.rml_numero_a,')',''),'(',''),'-',''),' ','') = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias  "
        If fatura <> "" Then
            sql = sql + " where codigo_fatura in(" & fatura & ") and servico=p1.tipo_serv2)) "
        Else
            sql = sql + "  where servico=p1.tipo_serv2))"
        End If
        sql = sql + " and exists"
        sql = sql + " (select a.codigo_conta"
        sql = sql + " from faturas f, faturas_arquivos a"
        sql = sql + " where(f.codigo_fatura = a.codigo_fatura)"
        sql = sql + "   and a.codigo_conta = p1.codigo_conta"
        If fatura <> "" Then
            sql = sql + " and f.codigo_fatura in(" & fatura & ")"
        End If
        sql = sql + "    and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "')"
        sql = sql + " and REPLACE(REPLACE(REPLACE(REPLACE(p1.rml_numero_a,')',''),'(',''),'-',''),' ','') in ('" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "')"
        If dataini <> "" And datafim <> "" Then
            sql = sql + "   and p1.data_inicio>=to_date('" + dataini + " 00:00:00','DD/MM/YYYY HH24:MI:SS')"
            sql = sql + "   and p1.data_inicio<=to_date('" + datafim + " 23:59:59','DD/MM/YYYY HH24:MI:SS')"
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and (exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario cat" & vbNewLine
            sql = sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine

            'usuário comum
            sql = sql + "  or exists( select rml_numero_a from usuarios where upper(nome_usuario) in(select upper(nome_usuario) from usuarios where codigo='" + Trim(Session("codigousuario")) + "')"
            sql = sql + "     and p1.rml_numero_a = '" + celular.Trim + "')"
            sql = sql + ")"

        End If
        sql = sql + " group by nvl(nvl(p1.tipo_serv,p1.tipo_serv2), 'SEM CLASSIFICAÇÃO') "

        'sql += " union "
        'sql = sql + " select 'RATEIO' categoria,"
        'sql = sql + " 0 qtd,"
        'sql = sql + " 0 duracao,"
        ''sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        ''sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        'sql += "   nvl(sum(rateio),0) TOTAL, 2 ordem "
        ''sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        '' Dim sql As String = " select nvl(sum(rateio),0) "
        'sql += " from ( "
        'sql += " select distinct nvl(r.rateio,0)  rateio, r.codigo_fatura, sum(nvl(p1.valor_franquia,0))valor   "
        'sql = sql + " from cdrs_celular p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        'sql = sql + " where p1.codigo_conta=a.codigo_conta"
        'sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        'sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        'sql = sql + " and to_char(f.dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "'"
        'sql = sql + "   and replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        'sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura)"

        sql += " union "
        'sql = sql + " select descricao ||'(Rateio)' categoria,"
        sql = sql + " select 'Rateio de Franquia' categoria,"
        sql = sql + " 1 qtd,"
        sql = sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        sql += "   nvl(sum(rateio),0) TOTAL, 2 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from ( "
        sql += " select distinct r.descricao, nvl(r.rateio,0)  rateio, r.codigo_fatura, sum(nvl(p1.valor_franquia,0))valor   "
        'sql = sql + " from cdrs_celular p1,faturas f,faturas_arquivos a,RATEIOGESTAO_LINHAS_MV r "
        sql = sql + " from cdrs_celular p1,faturas f,faturas_arquivos a,RATEIOGESTAO_LINHAS_MV r "
        sql = sql + " where p1.codigo_conta=a.codigo_conta"
        sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        sql = sql + " and to_char(f.dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "'"
        sql = sql + "   and REPLACE(REPLACE(REPLACE(REPLACE(p1.rml_numero_a,')',''),'(',''),'-',''),' ','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        ' sql = sql + "  and nvl(round(r.rateio,2),0)>0 "
        sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura,r.descricao) group by descricao"



        sql += " union "
        sql = sql + " select '" & nome_gestao_telecom_fixo & "' categoria,"
        sql = sql + " 1 qtd,"
        sql = sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        sql += "   nvl(pa.custo_servico,0) TOTAL, 3 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from lancamentos_mensais pa, operadoras_teste p4, linhas l,linhas_moveis lm,aparelhos_moveis ap,aparelhos_modelos  am,aparelhos_marcas ma,OPERADORAS_PLANOS op"
        sql += " where pa.id_item=ap.codigo_aparelho and lm.codigo_aparelho=ap.codigo_aparelho and lm.codigo_linha=l.codigo_linha "
        sql += " and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and pa.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and pa.tipo_recurso=1"
        sql += " and pa.referencia='" + mes + "/" + ano + "'"
        sql += " and pa.num_linha = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        'sql += " and op.custo_fixo is not null "
        sql += " "

        sql += " union "
        sql = sql + " select 'Parcela do aparelho (' || pa.num_parcela || '/' || pa.qtd_parcelas ||')'  categoria,"
        sql = sql + " 1 qtd,"
        sql = sql + " 0 duracao,"
        sql += "   nvl(pa.valor,0) TOTAL, 3 ordem "
        sql += " "
        sql += " from lancamentos_mensais pa, operadoras_teste p4, linhas l,linhas_moveis lm,aparelhos_moveis ap,aparelhos_modelos  am,aparelhos_marcas ma,OPERADORAS_PLANOS op"
        sql += " where pa.id_item=ap.codigo_aparelho and lm.codigo_aparelho=ap.codigo_aparelho and lm.codigo_linha=l.codigo_linha "
        sql += " and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and pa.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and pa.tipo_recurso=1"
        sql += " and pa.referencia='" + mes + "/" + ano + "'"
        sql += " and pa.num_linha = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        sql = sql + ") order by ordem"


        'Response.Write(sql)
        'Response.End()


        Dim connection2 As New OleDbConnection(strConexao)

        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = sql
        Dim reader2 As OleDbDataReader
        connection2.Open()
        reader2 = cmd2.ExecuteReader

        Me.GvResumo.DataSource = reader2
        Me.GvResumo.DataBind()
        _TotalRows = Me.GvResumo.Rows.Count - 2

        reader2.Close()
        reader2 = Nothing

        gvExtrato.Columns.Item(21).Visible = False



        'se não tiver contestação
        'If Not VerificaContestacao() Then
        'gvExtrato.Columns.Item(14).Visible = False
        'gvExtrato.Columns.Item(15).Visible = False
        'gvExtrato.Columns.Item(16).Visible = False
        'End If

        sql = "  select nvl(sum(TOTAL),0) from ("
        sql = sql + " select '" & nome_gestao_telecom_fixo & "' categoria,"
        sql = sql + " 1 qtd,"
        sql = sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        sql += "   nvl(op.custo_fixo,0) TOTAL, 3 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from operadoras_planos op, linhas l "
        sql += " where op.codigo_plano=l.codigo_plano and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'sql += " and op.custo_fixo is not null "
        sql += " "

        sql += " union "
        sql = sql + " select 'Parcela do aparelho' categoria,"
        sql = sql + " 1 qtd,"
        sql = sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        sql += "   nvl(nvl(ap.valor,0)/nvl(ap.qtd_parcelas,1),0) TOTAL, 3 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from  linhas l, linhas_moveis lm, aparelhos_moveis ap "
        sql += "  where l.codigo_linha=lm.codigo_linha and lm.codigo_aparelho=ap.codigo_aparelho and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'sql += " and op.custo_fixo is not null "
        sql += ") "




        Dim dtGatosFixos As DataTable = _dao.myDataTable(sql)
        Dim totalFixos As Double = 0
        If dtGatosFixos.Rows.Count > 0 Then
            totalFixos = dtGatosFixos.Rows(0).Item(0)

        End If



        If Me.GvResumo.Rows.Count > 0 Then
            If VerificaContestacao() Then
                'Me.GvResumo.FooterRow.Cells(3).Text = FormatCurrency(total_valor_audit_extrato + total_valor_servico + totalFixos)
                Me.GvResumo.FooterRow.Cells(3).Text = FormatCurrency(total_valor_audit_extrato + total_valor_servico)
            Else
                'Me.GvResumo.FooterRow.Cells(3).Text = FormatCurrency(total_valor_extrato + total_valor_servico + totalFixos)
                Me.GvResumo.FooterRow.Cells(3).Text = FormatCurrency(total_valor_extrato + total_valor_servico)
            End If


        End If



    End Sub

    Protected Sub gvResumo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvResumo.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then

            total_qtd_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "qtd"), 0)
            total_valor_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "total"))
            total_duracao_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "duracao"))

            'Dim cell As TableCell
            'Dim _myRow As New GridViewRow(e.Row.RowIndex + 1, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
            'cell = New TableCell
            'cell.Text = "SubTotal"
            '_myRow.Cells.Add(cell)
            ''duracao
            'cell = New TableCell
            'cell.Text = total_duracao_resumo
            '_myRow.Cells.Add(cell)
            ''qtd
            'cell = New TableCell
            'cell.Text = total_qtd_resumo
            '_myRow.Cells.Add(cell)

            'GvResumo.Controls(0).Controls.Add(_myRow)

            '_myRow.Cells(0).Text = "SubTotal"




        End If

        If e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(1).Text = FormatNumber(total_duracao_resumo)
            e.Row.Cells(2).Text = FormatNumber(total_qtd_resumo, 0)
            e.Row.Cells(3).Text = FormatCurrency(total_valor_resumo)


        End If

    End Sub

    Protected Sub gvExtrato_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvExtrato.RowDataBound



        If e.Row.RowType = DataControlRowType.DataRow Then

            If _editParticulares Then
                ' Display the company name in italics.
                Dim _chkParticular As System.Web.UI.HtmlControls.HtmlInputCheckBox = DirectCast(e.Row.FindControl("chkParticular"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
                If e.Row.DataItem("PARTICULAR").ToString.ToUpper <> "True" Then
                    'chkParticular.ToolTip = "Converte esta ligação de serviço para particular."
                    '_chkParticular.Checked = False
                    _chkParticular.Attributes.Remove("checked")
                Else
                    'chkParticular.ToolTip = "Converte esta ligação de particular para serviço."
                    _chkParticular.Checked = True

                    '_TotalParticular = _TotalParticular + e.Row.DataItem("VALOR")

                End If

                If _diaApontamento > 0 Then
                    Dim _diaLimiteEdicao As Integer = DateDiff("D", e.Row.Cells(8).Text, FormataDataApontamento(_diaApontamento))

                    If e.Row.DataItem("FATURADO") <> "1" And _diaLimiteEdicao >= 0 And DateDiff("D", FormataDataApontamento(_diaApontamento), Now()) < 1 Then

                        '_chkParticular.Checked = True
                        '_chkParticular.Attributes.Add("OnClick", "SomaParticular(this);")
                        '_chkParticular.Disabled = True
                    Else
                        _chkParticular.Disabled = True

                    End If
                End If
            End If



            e.Row.Cells(16).Text = FormatCurrency(e.Row.DataItem("valor_audit") + e.Row.DataItem("valor_rateio"), 2)

            total_valor_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor"))
            total_valor_audit_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit"))
            total_minutos_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "duracao"))
            total_valor_servico += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_rateio"))
            total_valor_geral += Decimal.Parse(e.Row.DataItem("valor_audit") + e.Row.DataItem("valor_rateio"))
            total_dados += Decimal.Parse(e.Row.DataItem("VALOR TRAFEGADO(MB)"))


            If Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "tarif_codigo")) = "0" Then
                e.Row.Cells(15).BackColor = Drawing.Color.LightYellow
            ElseIf Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit")) < Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor")) Then
                e.Row.Cells(15).BackColor = Drawing.Color.Red
            ElseIf Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit")) > Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor")) Then
                e.Row.Cells(15).BackColor = Drawing.Color.LawnGreen
            End If

        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            CarregaValorRateio()
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(2).Text = gvExtrato.Rows.Count
            e.Row.Cells(11).Text = FormatNumber(total_minutos_extrato)
            e.Row.Cells(13).Text = FormatCurrency(total_valor_extrato)
            If valor_rateio > 0 Then
                e.Row.Cells(14).Text = "Serviço: " + FormatCurrency(total_valor_servico) + "<br>Rateio: " + FormatCurrency(valor_rateio - total_valor_servico)
            Else
                e.Row.Cells(14).Text = FormatCurrency(total_valor_servico)
            End If

            e.Row.Cells(15).Text = FormatCurrency(total_valor_audit_extrato)
            ' e.Row.Cells(16).Text = "Total: " & FormatCurrency(total_valor_extrato + total_valor_servico)
            e.Row.Cells(16).Text = FormatCurrency(total_valor_geral)
            e.Row.Cells(16).Font.Bold = True


            e.Row.Cells(23).Text = FormatNumber(total_dados) & " MB"

            'e.Row.Cells(17).Text = "Total: " & FormatCurrency(total_valor_audit_extrato + total_valor_servico)
            'e.Row.Cells(17).Font.Bold = True



        End If

    End Sub

    Function SalvaParticulares() As Boolean

        Dim totalParticular As Double = 0.0
        Dim _msg As String = ""
        For Each _row As GridViewRow In Me.gvExtrato.Rows


            '_row.Cells(7).Controls(1)
            Dim _chkParticular As System.Web.UI.HtmlControls.HtmlInputCheckBox = DirectCast(_row.Cells(20).FindControl("chkParticular"), System.Web.UI.HtmlControls.HtmlInputCheckBox)
            Dim _codigo As System.Web.UI.HtmlControls.HtmlInputHidden = DirectCast(_row.Cells(20).FindControl("tbCodigo"), System.Web.UI.HtmlControls.HtmlInputHidden)
            Dim _dataIni As String = _row.Cells(8).Text
            Dim _particular As String = "N"

            If _chkParticular.Checked = True Then
                'chama a função que faz o update
                _particular = "S"
                totalParticular = totalParticular + _row.Cells(13).Text
            End If


            _dao.UpdateParticularesCelular(_codigo.Value, Session("Username"), _particular, _dataIni)

        Next
        _msg = "Atualização no valor de R$ " + FormatCurrency(totalParticular.ToString, 2) + "  efetuada."
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('" + _msg + "');</script>")

        Return True

    End Function

    Protected Sub btSavaParticularExec_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btSavaParticularExec.Click

        SalvaParticulares()

    End Sub

    Sub VerificaParticulares()

        If _editParticulares Then

            Me.btParticular.Visible = True
            Me.lbParticulares.Text = "<br><center><b> Você tem até " + FormataDataApontamento(_diaApontamento) + " para apontar suas ligações particulares.</b></center><br>"

        Else
            phParticulares.Visible = False
            Me.lbParticulares.Visible = False
            Me.gvExtrato.Columns(22).Visible = False
        End If

    End Sub

    Function FormataDataApontamento(ByVal _dia As String) As String
        'Dim _data As Date = New Date(Now.Year, Now.Month, _dia)

        'If _dia < Date.Now.Day Then
        '    Return Format(DateAdd(DateInterval.Day, 1, _data), "dd/MM/yyyy")
        'Else
        '    Return Format(_data, "dd/MM/yyyy")
        'End If
        '" + mes + "/" + ano + "'

        Dim _data As Date = New Date(ano, mes, _dia)
        Return Format(DateAdd(DateInterval.Month, 1, _data), "dd/MM/yyyy")

    End Function

    Sub CarregaOperadora()

        Dim sql As String = "select distinct p4.descricao operadora "
        sql += " from cdrs_celular p1, faturas_arquivos p2, faturas p3, operadoras_teste p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p2.codigo_fatura=p3.codigo_fatura and p3.codigo_operadora=p4.codigo "
        sql = sql + " and replace(p1.rml_numero_a, ' ', '') = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        sql = sql + " and exists"
        sql = sql + " (select a.codigo_conta"
        sql = sql + " from faturas f, faturas_arquivos a"
        sql = sql + " where(f.codigo_fatura = a.codigo_fatura)"
        sql = sql + "   and a.codigo_conta = p1.codigo_conta"
        sql = sql + "    and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "')"
        sql = sql + " and replace(p1.rml_numero_a, ' ', '') in ('" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "')"

        Dim connection2 As New OleDbConnection(strConexao)

        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = sql
        Dim reader2 As OleDbDataReader
        connection2.Open()
        reader2 = cmd2.ExecuteReader

        Using connection2

            While reader2.Read
                Me.lbOperadora.Text = reader2.Item(0).ToString
            End While

        End Using

    End Sub

    Function PegaCodigoFaturas() As List(Of String)

        Dim sql As String = " select codigo_fatura from faturas where codigo_fatura in ( "
        sql = sql + " select codigo_fatura from faturas_arquivos where codigo_conta in (select distinct codigo_conta from cdrs_celular where rml_numero_a = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "')"
        sql = sql + " ) and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "' "

        Dim codigos_faturas As New List(Of String)

        Dim connection3 As New OleDbConnection(strConexao)

        Dim cmd3 As OleDbCommand = connection3.CreateCommand
        cmd3.CommandText = sql
        Dim reader3 As OleDbDataReader
        connection3.Open()
        reader3 = cmd3.ExecuteReader

        Using connection3
            While reader3.Read
                codigos_faturas.Add(reader3.Item(0).ToString)
            End While
        End Using

        Return codigos_faturas

    End Function


    Function CarregaValorRateio() As String

        Dim sql As String = "select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,'' fatura,0 valor,nvl(rateio,0) valor_rateio,0 valor_audit, 0 VALOR_TOTAL,'AJUSTE RATEIO' tipo_serv,'AJUSTE RATEIO' tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular, 0 ""VALOR TRAFEGADO(MB)"" from ( "
        'sql += "select nvl(sum(rateio),0)-sum(nvl(valor,0)) rateio "
        sql += "select nvl(sum(rateio),0) rateio "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from ( "
        sql += " select distinct nvl(r.rateio,0)  rateio, r.codigo_fatura, sum(nvl(p1.valor_franquia,0))valor   "
        sql = sql + " from cdrs_celular p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        sql = sql + " where p1.codigo_conta=a.codigo_conta"
        sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
        sql = sql + " and to_char(f.dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "'"
        sql = sql + "   and replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura)"
        sql = sql + ")"

        Return sql
        Dim connection2 As New OleDbConnection(strConexao)



        Dim cmd2 As OleDbCommand = connection2.CreateCommand
        cmd2.CommandText = sql
        Dim reader2 As OleDbDataReader
        connection2.Open()
        reader2 = cmd2.ExecuteReader

        Using connection2

            While reader2.Read
                valor_rateio = reader2.Item(0).ToString
            End While

        End Using


    End Function


    Function VerificaContestacao() As Boolean
        ViewState("faturas") = fatura
        Dim result As Boolean = False
        If Not ViewState("faturas") = "" Then
            Dim sql As String = "select count(*) from contestacao c where  c.codigo_fatura in(" & ViewState("faturas") & ")"
            Dim dt As DataTable = _dao.myDataTable(sql)


            If dt.Rows(0).Item(0) < 1 Then
                'não teve contestação então oculta as colunas
                result = False
            Else
                'exibe as colunas de auditoria
                result = True


            End If
        Else
            'não passou o codigo da fatura
            result = False
        End If

        Return result
    End Function



End Class

