Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Partial Class GestaoRel_ExtratoFixoResult
    Inherits System.Web.UI.Page

    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Dim celular As String = ""
    Dim dataini As String = ""
    Dim datafim As String = ""
    Dim mes As String = ""
    Dim ano As String = ""
    Dim _editParticulares As Boolean = True
    Dim _diaApontamento As String = "25"
    Dim _dataLimiteApontamento As Date
    Public _TotalRows As Integer
    Public _TotalParticular As Double = 0.0
    Dim _dao As New DAO_Commons
    Dim tipo As String = ""
    Private total_valor As Decimal
    Private total_chamadas As Integer
    Private total_duracao As Decimal
    Private total_duracao_resumo As Decimal
    Private total_valor_resumo As Decimal
    Private total_qtd_resumo As Decimal
    Private total_valor_extrato As Decimal
    Private total_valor_audit_extrato As Decimal
    Private total_minutos_extrato As Decimal
    Private fatura As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'pega as sessions do asp 3.0
        'GetSessions()


        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        Else
            strConexao = Session("conexao")
            _dao.strConn = Session("conexao")
        End If

        If Not Page.IsPostBack Then

            celular = Request.QueryString("celular")
            dataini = Request.QueryString("dataini")
            datafim = Request.QueryString("datafim")
            mes = Request.QueryString("mes")
            ano = Request.QueryString("ano")
            tipo = Request.QueryString("tipo")
            fatura = Request.QueryString("fatura")
            'Dim end_query As String = " and exists (select * from linhas l "
            'end_query = end_query + " where codigo = l.codigo_usuario and  replace(replace(replace(REPLACE(l.NUM_LINHA(+),')',''),'(',''),'-',''),' ','') ='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + "')"

            Dim end_query As String = " and exists(select distinct 0 from cdrs_celular p1, faturas_arquivos fa, faturas f where p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura and replace(p1.rml_numero_a,' ','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "").Replace(" ", "") + "' and p1.codigo_usuario=u.codigo "
            If Fatura <> "" Then
                end_query = end_query + " and f.codigo_fatura='" & Fatura & "'"
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
            Else
                lbUsuario.Text = "SEM USUÁRIO"
            End If

            If tipo.ToUpper = "EXCEL" Then
                'doExcel()
                Response.Clear()
                Response.AddHeader("Content-Disposition", "Attachment;Filename=ExtratoDeRamal.xls")
                Response.Buffer = True
                Response.Charset = "windows-1252"
                Response.ContentType = "application/vnd.ms-excel"

                Response.Write("<style> td{text-align: right;mso-number-format: \@;white-space: nowrap;} </style>")
            End If

            MontaQuery()
            VerificaParticulares()
            lbdatenow.Text = DateTime.Now.ToString

            If _editParticulares = False Then
                Me.gvExtrato.Columns(20).Visible = False
            End If
        End If


    End Sub

    Private Sub MontaQuery()

        Dim sql As String = ""

        sql = sql + " select p1.codigo, replace(p1.rml_numero_a, ' ', '') ramal,"
        sql = sql + " CDR_CODIGO as codigo, nvl(p1.DESTINO, ' ') fisico,"
        sql = sql + " nvl(origem, 'SEM INFO') origem, codigo_tipo_ligacao tipo,"
        sql = sql + " decode(nvl(p2.nome_configuracao, 'DEFAULT'), 'DEFAULT', 'SEM CLASSIFICAÇÃO',"
        sql = sql + " p2.nome_configuracao) categoria, p1.numero_b numero, p1.data_inicio dataini,"
        sql = sql + " p1.data_fim datafim,nvl(p1.route, '[NULO]') rota,"
        sql = sql + " nvl(round(((p1.data_fim-p1.data_inicio)*(1440)),28), 0) duracao,"
        sql = sql + " nvl(p1.valor_cdr, 0) valor, 0 valor_rateio,nvl(p1.valor_cdr, 0) valor_total,nvl(p1.valor_audit, 0) valor_audit,"
        sql = sql + " nvl(p1.tipo_serv, '-') tipo_serv,nvl(p1.tipo_serv2, '-') tipo_serv2,"
        sql = sql + " nvl(p1.obs, '') obs,nvl(p1.faturado, 0) faturado,nvl(p1.codigo_conta, 0) codigo_conta,"
        sql = sql + " nvl(p1.valor_ok, '0') valor_ok, p1.tarif_codigo tarif_codigo,"
        sql = sql + " decode(nvl(particular,'N'),'S','true','N','false') particular"
        sql = sql + " from CDRS_CELULAR p1, tarifacao p2"
        sql = sql + " where 1 = 1"
        sql = sql + " and exists (select a.codigo_conta"
        sql = sql + " from faturas f, faturas_arquivos a"
        sql = sql + " where f.codigo_fatura = a.codigo_fatura"
        sql = sql + " and a.codigo_conta = p1.codigo_conta"
        If fatura <> "" Then
            sql = sql + " and f.codigo_fatura='" & fatura & "'"
        End If
        sql = sql + " and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "')"
        sql = sql + " and p1.tarif_codigo = p2.codigo(+)"
        sql = sql + " and trim(p1.rml_numero_a) = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"

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

        'sql += "union " & CarregaValorRateio()

        sql = sql + " order by dataini"


        'Response.Write(sql)
        ' Response.End()

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

        sql = ""
        sql = sql + " select nvl(p1.tipo_serv2, 'SEM CLASSIFICAÇÃO') as categoria,"
        sql = sql + " count(*) qtd,"
        sql = sql + " sum(nvl(round(((p1.data_fim-p1.data_inicio)*(1440)),28), 0)) duracao,"
        sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        sql = sql + " from CDRS_CELULAR p1, tarifacao p2"
        sql = sql + " where(p1.tarif_codigo = p2.codigo)"
        sql = sql + " and replace(p1.rml_numero_a, ' ', '') = '" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'"
        sql = sql + " and exists"
        sql = sql + " (select a.codigo_conta"
        sql = sql + " from faturas f, faturas_arquivos a"
        sql = sql + " where(f.codigo_fatura = a.codigo_fatura)"
        sql = sql + "   and a.codigo_conta = p1.codigo_conta"
        sql = sql + "    and to_char(dt_vencimento, 'MM/YYYY') = '" + mes + "/" + ano + "')"
        sql = sql + " and replace(p1.rml_numero_a, ' ', '') in ('" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "')"
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


        sql = sql + " group by p1.tipo_serv2 "



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

        gvExtrato.Columns.Item(19).Visible = False

    End Sub

    Function CarregaValorRateio() As String

        Dim sql As String = "select 99999 codigo, '' ramal,0 as CDR_CODIGO,'' fisico,'' origem,0 tipo,'' categoria,'' numero,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') dataini,to_date('01/" + mes + "/" + ano + "','DD/MM/YYYY') datafim,'' rota,0 duracao,0 valor,nvl(rateio,0) valor_rateio,0 valor_audit, 0 VALOR_TOTAL,'AJUSTE RATEIO' tipo_serv,'AJUSTE RATEIO' tipo_serv2,'' obs,'0' faturado,0 codigo_conta,0 valor_ok,0 tarif_codigo,'false' particular from ( "
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
        'Dim connection2 As New OleDbConnection(strConexao)



        'Dim cmd2 As OleDbCommand = connection2.CreateCommand
        'cmd2.CommandText = sql
        'Dim reader2 As OleDbDataReader
        'connection2.Open()
        'reader2 = cmd2.ExecuteReader

        'Using connection2

        '    While reader2.Read
        '        valor_rateio = reader2.Item(0).ToString
        '    End While

        'End Using


    End Function

    Protected Sub gvResumo_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles GvResumo.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then


            total_qtd_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "qtd"), 0)
            total_valor_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "total"))
            total_duracao_resumo += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "duracao"))
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
                If e.Row.DataItem("PARTICULAR").ToString.ToUpper <> "TRUE" Then
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
            total_valor_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor"))
            total_valor_audit_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit"))
            total_minutos_extrato += Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "duracao"))

            If Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "tarif_codigo")) = 0 Then
                e.Row.Cells(14).BackColor = Drawing.Color.Yellow
            ElseIf Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit")) < Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor")) Then
                e.Row.Cells(14).BackColor = Drawing.Color.Red
            ElseIf Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor_audit")) > Decimal.Parse(DataBinder.Eval(e.Row.DataItem, "valor")) Then
                e.Row.Cells(14).BackColor = Drawing.Color.LawnGreen
            End If

        End If

        If e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(2).Text = gvExtrato.Rows.Count
            e.Row.Cells(11).Text = FormatNumber(total_minutos_extrato)
            e.Row.Cells(13).Text = FormatCurrency(total_valor_extrato)
            e.Row.Cells(14).Text = FormatCurrency(total_valor_audit_extrato)


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

        End If

    End Sub

    Function FormataDataApontamento(ByVal _dia As String) As String
        Dim _data As Date = New Date(Now.Year, Now.Month, _dia)

        If _dia < Date.Now.Day Then
            Return Format(DateAdd(DateInterval.Day, 1, _data), "dd/MM/yyyy")
        Else
            Return Format(_data, "dd/MM/yyyy")
        End If
    End Function
End Class

