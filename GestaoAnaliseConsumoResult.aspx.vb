Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.IO

Module global_variables
    'Public busca_analise As Integer = 0
    'Public data_analise As String = ""
    'Public tipo_analise As String = ""
    'Public oper_analise As String = ""
End Module

Partial Class GestaoAnaliseConsumoResult
    Inherits System.Web.UI.Page
    Dim _jqGrid As New JQGrid
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
    Dim strSQL_grupos As String = ""
    Private _dao As New DAOOperadoras
    Private _dao_his As New DAO_Commons
    Public Aes_Param As Boolean = AppIni.Aes_Param
    Public GloboRJ_Parm As Boolean = AppIni.GloboRJ_Parm
    Public exibe_franquia As Boolean = AppIni.exibe_franquia
    Public exibe_auditado As Boolean = AppIni.exibe_auditado
    'totais
    Dim totalFaturado As Double = 0
    Dim totalFranquia As Double = 0
    Dim totalAuditoria As Double = 0
    Dim totalRateio As Double = 0
    Dim totalGeral As Double = 0
    Dim totalQTD As Integer = 0
    Dim totalMinutagem As Double = 0
    Dim totalResumoRateio As Double = 0
    Dim totalResumoFranquia As Double = 0
    Dim totalResumoSemRateio As Double = 0
    Dim totalResumoSemRateioAuditado As Double = 0
    'totais serviços
    Dim totalfaturadoServicos As Double = 0
    Dim totalAuditadoServicos As Double = 0
    Dim totalQTDServicos As Double = 0

    'totais serviços rateados
    Dim totalfaturadoServicosRateados As Double = 0
    Dim totalAuditadoServicosRateados As Double = 0
    Dim totalQTDServicosRateados As Double = 0

    Dim TotalfaturadoServRateio As Double = 0
    Dim TotalfaturadoservRateioAuditado As Double = 0
    Dim _contestou As Boolean = True



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        Else
            strConexao = Session("conexao")
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_his.strConn = Session("conexao").ToString

        If Not Page.IsPostBack Then

            '******************************* TEST PLACE ********************************************

            'Session("Contexto") = "HTML"
            'Session("Nome") = "Relatório Gerencial de Análise de Consumo"
            'Session("HTML_Context") = "<br/>Faturas:  - false - false - false - false - false - false - <br/>Centro(s) de Custo:  - <br/>Data: 05 de 2016"
            'Session("linhas_com_gasto") = False
            'Session("linhas_sem_gasto") = False
            'Session("faturas") = " 1474 1481 1482 1486 1487 1503"
            'Session("ccustos") = ""

            'ViewState("data") = "05/2016"

            'Session("rateio") = True
            'Session("rel_type") = "HTML"
            'Session("tipo") = "1"

            '***************************************************************************************


            ViewState("linhas_com_gasto") = Session("linhas_com_gasto")
            ViewState("linhas_sem_gasto") = Session("linhas_sem_gasto")
            ViewState("faturas") = Session("faturas")
            ViewState("ccustos") = Session("ccustos")
            ViewState("data") = Session("data")
            ViewState("operadora") = Session("operadora")
            ViewState("analitico") = Session("analitico")
            ViewState("rel_type") = Session("rel_type")

            'Response.Write(ViewState("ccustos"))
            'Response.End()

            'If Request.QueryString("tipo") <> "" Then
            '    Session("tipo") = Request.QueryString("tipo")
            'End If


            Me.lbdatenow.Text = Date.Now

            'Chama_relatorio(ViewState("rel_type"), "Relatório de Análise de Consumo")
            MontaCabecalho()

            If Session("tipo") = "1" Or Session("tipo") = "2" Then
                ResumoServicosFranquias()
                ResumoServicosRateados()
                ' ResumoRateio()
                'ResumoFranquia()
                ResumoServSemRateio()
            End If

            If Not ViewState("faturas").Split(" ").Length > 2 Then


                If Not String.IsNullOrEmpty(Session("faturas")) Then
                    If Session("faturas").ToString.Replace(" ", "") <> "" Then

                    Else
                        Me.lbFranquia.Visible = False
                        Me.lbSemRateio.Visible = False
                        Me.lbServicos.Visible = False
                        Me.lbServicosRateios.Visible = False

                        Me.gvRateio.Visible = False
                        Me.gvRelServicos.Visible = False
                        Me.gvSemrateio.Visible = False
                        Me.gvFranquia.Visible = False
                        Me.gvServicoRateio.Visible = False

                    End If
                Else
                    Me.lbFranquia.Visible = False
                    Me.lbSemRateio.Visible = False
                    Me.lbServicos.Visible = False
                    Me.lbServicosRateios.Visible = False

                    Me.gvRateio.Visible = False
                    Me.gvRelServicos.Visible = False
                    Me.gvSemrateio.Visible = False
                    Me.gvFranquia.Visible = False
                    Me.gvServicoRateio.Visible = False

                End If
            Else
                'Me.lbFranquia.Visible = False
                'Me.lbSemRateio.Visible = False
                'Me.lbServicos.Visible = False
                'Me.lbServicosRateios.Visible = False

                'Me.gvRateio.Visible = False
                'Me.gvRelServicos.Visible = False
                'Me.gvSemrateio.Visible = False
                'Me.gvFranquia.Visible = False
                'Me.gvServicoRateio.Visible = False
            End If
            Me.lbFranquia.Visible = False

            'CarregaCompartilhado()

            MontaGrid(ViewState("rel_type"), "Relatório de Análise de Consumo")
            ' VerificaContestacao()

            If Aes_Param = False And GloboRJ_Parm = False Then
                Me.gvRel.Columns("16").Visible = False
                Me.gvRel.Columns("17").Visible = False
            End If

            If Session("rateio") = False Then
                Me.gvRel.Columns("14").Visible = False
            End If

            If Session("rel_type") = "Excel" Then
                doExcel()
            End If


            'se não for administrador do sistema não ve os serviços

            If Not _dao_his.Is_Administrator(Trim(Session("codigousuario"))) Or ViewState("ccustos") <> "" Then
                Me.phServicos.Visible = False
                Me.gvCabecalho.Visible = False

            End If


            'If Request.QueryString("VP") <> "" And Me.gvRel.Rows.Count >= 2 Then

            '    Me.gvRel.Rows(gvRel.Rows.Count - 2).Visible = False
            '    Me.gvRel.Rows(gvRel.Rows.Count - 1).Visible = False


            'End If


        End If

    End Sub


    Sub MontaCabecalho()
        Dim faturas() As String
        Dim ccustos() As String
        faturas = ViewState("faturas").Split(" ")
        ccustos = ViewState("ccustos").Split(" ")
        Dim mes As String = ViewState("data").ToString.Substring(0, 2)
        Dim ano As String = ViewState("data").ToString.Substring(3, 4)


        'se não filtrou a operadora pega os códigos

        If String.IsNullOrEmpty(ViewState("faturas").ToString.Replace(" ", "")) Then

            Dim sql As String = "select f.codigo_fatura from faturas f where to_char(f.dt_vencimento,'MM/YYYY')='" & ViewState("data") & "'"
            If Not String.IsNullOrEmpty(ViewState("operadora")) Then
                sql = sql + "             and f.codigo_operadora = '" & ViewState("operadora") & "' "
            End If
            If Session("tipo") <> "" Then
                sql = sql + "             and f.codigo_tipo in (" & Session("tipo") & ") "
            End If
            Dim dt As DataTable = _dao_his.myDataTable(sql)

            Dim i As Integer = 0
            For Each _row As DataRow In dt.Rows
                ViewState("faturas") += _row.Item("codigo_fatura") & " "
            Next

        End If


        Dim sql2 As String = ""
        sql2 += " select f.codigo_fatura, f.descricao fatura, op.descricao operadora , TO_CHAR(f.dt_vencimento,'DD/MM/YYYY')dt_vencimento,"
        sql2 += " case when exists(select 0 from contestacao c where c.codigo_fatura=f.codigo_fatura and c.status=2) then 'SIM' else 'NÃO' end contestada"
        sql2 += " from faturas f, operadoras_teste op"
        sql2 += " where f.codigo_operadora=op.codigo"
        sql2 += " and to_char(f.dt_vencimento,'MM/YYYY')='" & ViewState("data") & "' "
        If Not String.IsNullOrEmpty(ViewState("operadora")) Then
            sql2 = sql2 + "             and f.codigo_operadora = '" & ViewState("operadora") & "' "
        End If
        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        sql2 = sql2 + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        sql2 = sql2 + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                sql2 = sql2 + ")"
            End If
        End If

        Dim dtCabecalho As DataTable = _dao_his.myDataTable(sql2)
        Me.gvCabecalho.DataSource = dtCabecalho
        Me.gvCabecalho.DataBind()

        'pega as areas

        If faturas.Length > 2 Or faturas.Length < 2 Then
            ViewState("nome_area") = ""

            If ccustos.Length > 1 Then
                Dim sqlAreas As String = "select distinct area from grupos p1 where 1=1 "
                Dim count As Integer = 0

                For Each codigo_ccusto As String In ccustos
                    If codigo_ccusto <> "" Then
                        If count = 0 Then
                            sqlAreas = sqlAreas + " and p1.codigo in ('" + codigo_ccusto + "'"
                            count = count + 1
                        Else
                            sqlAreas = sqlAreas + " ,'" + codigo_ccusto + "'"
                        End If
                    End If
                Next
                If count = 1 Then
                    sqlAreas = sqlAreas + ")"
                End If

                Dim dtAreas As DataTable = _dao_his.myDataTable(sqlAreas)
                ViewState("nome_area") = "Área(s): "

                For Each _row As DataRow In dtAreas.Rows
                    ViewState("nome_area") = ViewState("nome_area") & _row.Item(0) & "; "
                Next
            End If
        End If

        'Dim html As String = "<br/>Faturas: "

        'For Each item As String In faturas
        '    html = html + item + " - "
        'Next

        'html = html + "<br/>Centro(s) de Custo: "

        'For Each item As String In ccustos
        '    html = html + item + " - "
        'Next

        'html = html + "<br/>Data: " + mes.ToString + " de " + ano.ToString
        'Session("HTML_Context") = html
        'Dim info_rel As New System.Web.UI.HtmlControls.HtmlGenericControl
        'info_rel.InnerHtml = Session("HTML_Context")

        'Information.Controls.Add(info_rel)
        'lbdatenow.Text = Now


    End Sub


    Sub MontaGrid(rel_type As String, titulo As String)
        Dim cod_user As Integer = Session("codigousuario")
        Dim Sql As String = ""
        Dim order As String = ""
        Dim tipo As String = ""
        Dim faturas() As String
        Dim ccustos() As String
        Dim VP As String = ""

        faturas = ViewState("faturas").Split(" ")
        ccustos = ViewState("ccustos").Split(" ")
        VP = Request.QueryString("VP")

        Sql = Sql + " select replace(replace(replace(nvl(celular,' '),'(',''),')',''),'-','') as linha,nvl(TO_CHAR(p00.grupo), '[SEM CADASTRO]') grupo,"
        Sql = Sql + " nvl(p1.nome_grupo, '[SEM CADASTRO]') nome_grupo,"
        Sql = Sql + " nvl(nome_usuario, 'Sem usuário') usuario,"
        Sql = Sql + " nvl(p0.matricula, '-') matricula,"
        Sql = Sql + " fatura,"
        Sql = Sql + " "
        Sql = Sql + " nvl(sl.descricao, 'SEM CADASTRO') status,"
        'sql = sql + " decode(nvl(cod_tipo, '0'),'1','CELULAR','2','RADIO','3','MODEM','4','SMARTPHONE',"
        'sql = sql + " '5','BLACKBERRY','6','GATEWAY','OUTROS') as tipo,"
        Sql += " nvl(at.nome,'-')  as tipo,"
        Sql = Sql + " nvl(lt.tipo, '-') classificacao,"
        Sql = Sql + " (chamadas) QTD,"
        Sql = Sql + " (round(duracao, 10)) duracao,"
        Sql = Sql + " (nvl(gasto, 0)) ""GASTO"", "
        Sql = Sql + " (round(nvl(consumo, 0),4)) ""CONSUMO(FRANQUIA)"", "
        'Sql = Sql + " to_char(nvl(valor_audit, 0)) ""AUDITADO"", "
        Sql = Sql + " (nvl(gasto, 0)-valor_devolvido) ""AUDITADO"", "

        'If Session("tipo") = "1" Then
        '    'linhas moveis
        '    Sql = Sql + " case when exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (nvl(gasto, 0)-valor_devolvido) "

        'Else
        '    'linhas fixas
        '    Sql = Sql + " case when exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (nvl(gasto, 0)-valor_devolvido) "
        'End If
        'Sql += " else 0 end  ""AUDITADO"","



        Sql = Sql + " (ROUND(nvl(Rateio, 0), 4)) Rateio "
        Sql = Sql + " ,(ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) ""GASTO+RATEIO"" "
        Sql = Sql + " ,1 ordem "
        Sql = Sql & " ,nvl(p1.VP, 'SEM VP') as VP  "
        Sql = Sql & " , nvl(p1.diretoria, 'SEM DIRETORIA') as DIRETORIA"

        'If Session("tipo") = "1" Then
        '    'linhas moveis
        '    Sql = Sql + " ,case when exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) "

        'Else
        '    'linhas fixas
        '    Sql = Sql + " ,case when exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
        '    Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) "
        'End If
        'Sql += " else 0 end  ""GASTO+RATEIO"" "


        Sql = Sql + "    from (select to_char(p1.grp_codigo) grupo,"
        Sql = Sql + "    nvl(replace(p1.rml_numero_a, ' ', ''), '[NULO]') Celular,"
        Sql = Sql + "    f.descricao fatura,"
        Sql = Sql + "    f.codigo_fatura,"
        'sql = sql + "    nvl(r.rateio,0) rateio,"

        'Se a linha for inválida e teve rateio deve ser zero
        Sql = Sql + "    case when exists "
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')  "
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "') then 0"
        Sql = Sql + "   else nvl(r.rateio,0) end rateio, "
        Sql = Sql + "    count(*) chamadas,"
        Sql = Sql + "    round(sum((p1.data_fim - p1.data_inicio))*1440, 2) duracao,"
        Sql = Sql + "    sum(p1.valor_cdr) gasto,"
        'Sql = Sql + "    sum(p1.valor_cdr2) consumo,"
        'Sql = Sql + " sum(case when p1.tipo_serv2 in (select fs.servico from franquias_servicos fs, franquias fr where fs.codigo_franquia=fr.codigo and fr.codigo_fatura=f.codigo_fatura) and p1.valor_cdr=0  "
        'Sql = Sql + " then round(p1.valor_franquia,4) else 0 end) consumo,"

        Sql = Sql + " sum(round(case when p1.valor_cdr=0 "

        Sql = Sql + " then p1.valor_franquia else 0 end,4)) consumo,"
        Sql = Sql + " "



        Sql = Sql + "    sum(p1.valor_audit) valor_audit,"
        'Sql += " (select nvl(sum(c.valor_faturado-c.valor_audit),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.aprovada='S')valor_devolvido,"
        Sql += " (select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.aprovada='S' "
        Sql += " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=c.codigo_fatura and servico=c.tipo_serv2))"

        Sql += ")valor_devolvido,"
        Sql = Sql + "    p1.codigo_usuario "
        'Sql = Sql + "    from cdrs_celular_analitico_mv p1,"
        Sql = Sql + "    from cdrs_celular p1,"
        Sql = Sql + "         faturas                   f,"
        Sql = Sql + "         faturas_arquivos          a,"
        Sql = Sql + "         RateioGestao_MV r "
        Sql = Sql + "     where(f.codigo_fatura = a.codigo_fatura)"

        'Filtra faturas

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If

        'If Not String.IsNullOrEmpty(Me.tbCCUsuario.Text) Then
        '    sql = sql + " and p1.grp_codigo = '" + Me.tbCCUsuario.Text.ToString + "'"
        'End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            Sql = Sql + " and exists(" & vbNewLine
            Sql = Sql + "   select 0 from categoria_usuario cat" & vbNewLine
            Sql = Sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            Sql = Sql + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            Sql = Sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        Sql = Sql + "     and a.codigo_conta = p1.codigo_conta"
        Sql = Sql + "     and f.codigo_tipo in (" + Session("tipo") + ")"
        Sql = Sql + "     and p1.rml_numero_a = r.rml_numero_a(+)"
        Sql = Sql + "    and p1.codigo_conta = r.codigo_conta(+)"
        Sql = Sql + "    and exists"
        Sql = Sql + "    (select a.codigo_conta from faturas f, faturas_arquivos a"
        Sql = Sql + "    where(   f.codigo_fatura = a.codigo_fatura)"
        Sql = Sql + "             and a.codigo_conta = p1.codigo_conta"
        'sql = sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        If Not String.IsNullOrEmpty(ViewState("operadora")) Then
            Sql = Sql + "             and codigo_operadora = '" & ViewState("operadora") & "' "
        End If
        Sql = Sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"

        'tira as cobranças de franquias
        Sql = Sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"

        Sql = Sql + "    and not exists"
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"
        Sql = Sql + "           group by p1.grp_codigo,"
        Sql = Sql + "           p1.rml_numero_a,"
        Sql = Sql + "           f.descricao,"
        Sql = Sql + "           f.codigo_fatura,"
        Sql = Sql + "           r.rateio,"
        Sql = Sql + "           p1.codigo_usuario) p00,"
        Sql = Sql + "                        linhas p2,"
        Sql = Sql + "                 linhas_moveis p3,"
        Sql = Sql + "                        grupos p1,"
        Sql = Sql + "              aparelhos_moveis ap,"
        Sql = Sql + "             aparelhos_modelos mo,"
        Sql = Sql + "                 status_linhas sl,"
        Sql = Sql + "                   linhas_tipo lt,"
        Sql = Sql + "                      usuarios p0 "
        Sql = Sql + "                      ,aparelhos_tipos at "
        Sql = Sql + " where replace(replace(REPLACE(p2.NUM_LINHA(+), ')', ''), '(', ''), '-', '') ="
        Sql = Sql + " replace(replace(REPLACE(celular, ')', ''), '(', ''), '-', '')"

        'retira as linhas invalidas se houver rateio
        'Sql = Sql + " and ( exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "

        'descarta as linhas validas
        If Session("tipo") = "1" Then
            'linhas moveis
            Sql = Sql + " and  (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
            Sql = Sql + " )"
        ElseIf Session("tipo") = "2" Then
            'linhas fixas
            Sql = Sql + " and  ( exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
            Sql = Sql + " )"
        End If
        'Sql = Sql + " or not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"

        'FIM retira as linhas invalidas se houver rateio

        Sql = Sql + " and p00.codigo_usuario = p0.codigo(+)"
        Sql = Sql + " and p2.codigo_linha = p3.codigo_linha(+)"
        Sql = Sql + " and to_char(p00.grupo) = to_char(p1.codigo(+))"
        Sql = Sql + " and p3.codigo_aparelho = ap.codigo_aparelho(+)"
        Sql = Sql + " and ap.cod_modelo = mo.cod_modelo(+)"
        Sql = Sql + " and to_char(p2.status) = to_char(sl.codigo_status(+))"
        Sql = Sql + " and p2.codigo_tipo = lt.codigo_tipo(+)"
        Sql = Sql + " and mo.cod_tipo=at.codigo_tipo(+)"

        If Request.QueryString("VP") <> "" Then
            Sql = Sql + " and p1.VP ='" & VP & "'"
        End If


        If ViewState("linhas_com_gasto") = True Then
            Sql = Sql + " and p00.gasto > 0"
        End If
        If ViewState("linhas_sem_gasto") = True Then
            Sql = Sql + " and p00.gasto = 0"
        End If

        If ccustos.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_ccusto As String In ccustos
                If codigo_ccusto <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and p1.codigo in ('" + codigo_ccusto + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_ccusto + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If

        'If tbCCUsuario.Text <> "" Then
        '    sql = sql + " and p1.codigo = '" + tbCCUsuario.Text + "'"
        'End If

        If Not String.IsNullOrEmpty(ViewState("classificacao")) Then
            ' sql = sql + " and upper(nvl(lt.tipo, 'MOVEL')) = '" + ViewState("classificacao").ToString.ToUpper + "'"
            Sql = Sql + " and case when lt.tipo is not null then lt.tipo when p3.codigo_linha is not null then 'MOVEL' when P00.CODIGO_USUARIO is null then '" + ViewState("classificacao").ToString.ToUpper + "'  else 'LINHA DIRETA' end  = '" + ViewState("classificacao").ToString.ToUpper + "'"

        End If


        Sql = Sql + "  order by ordem, grupo, linha, gasto desc"

        'Response.Write(Sql)
        'Response.End()



        If Not ViewState("analitico") Then
            'se for consolidado
            Dim Sql2 As String = ""
            Sql2 = Sql2 + " select  nvl(to_char(grupo), '[SEM CADASTRO]') grupo,"
            Sql2 = Sql2 + " nvl(nome_grupo, '[SEM CADASTRO]') nome_grupo,"
            Sql2 += " count(linha) qtd_celular,"
            Sql2 += " (sum(QTD)) QTD,"
            Sql2 += " (round(sum(duracao), 10)) duracao,"
            Sql2 += " (nvl(sum(gasto), 0)) gasto, "
            Sql2 += " (nvl(sum(AUDITADO), 0)) ""AUDITADO"", "
            Sql2 += " (nvl(sum(""CONSUMO(FRANQUIA)""), 0)) ""CONSUMO(FRANQUIA)"", "
            Sql2 += "  (ROUND(nvl(sum(Rateio), 0), 4)) Rateio "
            Sql2 += " ,(sum(ROUND(nvl(NVL(Rateio,0)+AUDITADO+""CONSUMO(FRANQUIA)"", 0), 4))) ""GASTO+RATEIO"" "

            Sql2 += " from ("
            Sql2 += Sql

            Sql2 += " )"

            Sql2 += "  group by grupo, nome_grupo"

            Sql2 += "  order by gasto desc, grupo, nome_grupo"
            Sql = Sql2
        End If






        Dim connection As New OleDbConnection(strConexao)

        'Dim cmd As OleDbCommand = connection.CreateCommand
        'cmd.CommandText = Sql
        'Dim reader As OleDbDataReader
        'connection.Open()
        'reader = cmd.ExecuteReader
        Dim dt As DataTable = _dao_his.myDataTable(Sql)

        If rel_type <> "CSV" Then
            If Not ViewState("analitico") Then
                Me.gvRelConsolidado.DataSource = dt
                Me.gvRelConsolidado.DataBind()
                gvRel.Visible = False
            Else
                Me.gvRel.DataSource = dt
                Me.gvRel.DataBind()
                gvRelConsolidado.Visible = False
            End If

            'gvRel.HeaderRow.TableSection = TableRowSection.TableHeader
            'gvRel.FooterRow.TableSection = TableRowSection.TableFooter
        Else
            Dim file_name As String = "analise_consumo"
            HttpContext.Current.Response.ContentEncoding = Encoding.GetEncoding("Windows-1252")
            _dao_his.CSVFromReader(Sql, file_name)

        End If






    End Sub




    Protected Sub gvRel_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRel.RowDataBound

        Dim mes As String = ViewState("data").ToString.Substring(0, 2)
        Dim ano As String = ViewState("data").ToString.Substring(3, 4)

        If e.Row.RowType = DataControlRowType.DataRow Then

            totalQTD += e.Row.Cells(9).Text.Replace("R$", "").Replace(" ", "")
            totalMinutagem += e.Row.Cells(10).Text.Replace("R$", "").Replace(" ", "")

            totalFaturado += e.Row.Cells(11).Text.Replace("R$", "").Replace(" ", "")
            totalAuditoria += e.Row.Cells(12).Text.Replace("R$", "").Replace(" ", "")
            totalFranquia += FormatNumber(e.Row.Cells(13).Text.Replace("R$", "").Replace(" ", ""), 4)
            totalRateio += FormatNumber(e.Row.Cells(14).Text.Replace("R$", "").Replace(" ", ""), 4)
            totalGeral += FormatNumber(e.Row.Cells(15).Text.Replace("R$", "").Replace(" ", ""), 4)


            e.Row.Cells(13).Text = FormatCurrency(e.Row.Cells(13).Text.Replace("R$", "").Replace(" ", ""), 2)
            e.Row.Cells(14).Text = FormatCurrency(e.Row.Cells(14).Text.Replace("R$", "").Replace(" ", ""), 2)
            e.Row.Cells(15).Text = FormatCurrency(e.Row.Cells(15).Text.Replace("R$", "").Replace(" ", ""), 2)


            'If Server.HtmlDecode(e.Row.Cells(5).Text) <> "SERVIÇOS COMPARTILHADOS" Then
            If IsNumeric(e.Row.Cells(5).Text.Replace(" ", "").Replace("-", "").Replace("(", "").Replace(")", "")) And ViewState("rel_type").ToString.ToUpper <> "EXCEL" Then


                If Session("tipo") = "1" Then
                    If e.Row.DataItem("LINHA").ToString.Length > 5 Then

                        'Row.Item("LINHA") = "<a href='javascript:window.open(""GestaoRel_ExtratoCelularResult.aspx?celular=" & e.Row.DataItem("LINHA") & "&mes=" & mes & "&ano=" &ano & "&tipo=HTML&dataini=&datafim="");void(0);' title='Extrato'>(" & Row.Item("LINHA").ToString.Substring(0, 2) & ")" & Row.Item("LINHA").ToString.Substring(2, 4) & "-" & Row.Item("LINHA").ToString.Substring(6, IIf(Row.Item("LINHA").ToString.Length = 11, 5, 4)) & "</a>"
                        e.Row.Cells(5).Text = "<a href='javascript:window.open(""GestaoRel_ExtratoCelularResult.aspx?celular=" & e.Row.DataItem("LINHA") & "&fatura=" & ViewState("faturas") & "&mes=" & mes & "&ano=" & ano & "&tipo=HTML&dataini=&datafim="");void(0);' title='Extrato'>" & e.Row.DataItem("LINHA").ToString & "</a>"


                    Else

                        e.Row.Cells(5).Text = "<a href='javascript:window.open(""GestaoRel_ExtratoCelularResult.aspx?celular=" & e.Row.DataItem("LINHA") & "&fatura=" & ViewState("faturas") & "&mes=" & mes & "&ano=" & ano & "&tipo=HTML&dataini=&datafim="");void(0);' title='Extrato'>" & e.Row.DataItem("LINHA").ToString & "</a>"
                    End If
                Else

                    e.Row.Cells(5).Text = "<a href='javascript:window.open(""GestaoRel_ExtratoFixoResult.aspx?celular=" & e.Row.DataItem("LINHA") & "&mes=" & mes & "&ano=" & ano & "&tipo=HTML&dataini=&datafim"");void(0);' title='Extrato'>" & e.Row.DataItem("LINHA").ToString & "</a>"
                End If
            Else

                e.Row.BackColor = Drawing.Color.Beige
                e.Row.BorderStyle = BorderStyle.Double
                e.Row.Font.Bold = True

            End If



        End If

        If e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(9).Text = FormatNumber(totalQTD, 0)
            e.Row.Cells(10).Text = FormatNumber(totalMinutagem, 2)

            e.Row.Cells(11).Text = FormatCurrency(totalFaturado)
            e.Row.Cells(12).Text = FormatCurrency(totalAuditoria)
            e.Row.Cells(13).Text = FormatCurrency(totalFranquia)

            e.Row.Cells(14).Text = FormatCurrency(totalRateio)
            e.Row.Cells(15).Text = FormatCurrency(totalGeral)

            e.Row.Cells(12).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(13).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(14).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(15).BackColor = Drawing.Color.LightBlue

            Dim mytable As New Table
            Dim myRow As New TableRow
            Dim myCell As New TableCell
            myCell.Text = "Serviços Compartilhados"
            myRow.Cells.Add(myCell)
            mytable.Rows.Add(myRow)

            'e.Row.Controls.Add(mytable)

            'mytable.Rows.Add(

            'MontaTotaisServicosComp()

        End If



    End Sub

    Sub doExcel()

        'se o grid tiver mais que 65536  linhas não podemos exportar
        If Me.gvRel.Rows.Count.ToString + 1 < 65536 Then

            'Me.Controls.Remove(Me.FindControl("ScriptManager1"))
            'gvRel.AllowPaging = "False"

            Dim tw As New StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim frm As HtmlForm = New HtmlForm()

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=" & Session("Nome") & ".xls")
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252")
            Response.Charset = "ISO-8859-1"
            'EnableViewState = False

            'Controls.Add(frm)
            'frm.Controls.Add(pagina)
            'frm.RenderControl(hw)

            Response.Write("<style> td{text-align: right;mso-number-format: \@;white-space: nowrap;} </style>")
            'Response.Write(tw.ToString())
            'Response.End()

            'gvRel.AllowPaging = "True"
            'gvRel.DataBind()

        Else
            'LblError.Text = " planilha possui muitas linhas, não é possível exportar para o EXcel"
        End If
    End Sub


    Sub VerificaContestacao()
        Dim faturas() As String
        faturas = ViewState("faturas").Split(" ")


        If (Not ViewState("faturas") = "" And _dao_his.Is_Administrator(Session("codigo_usuario"))) Then
            Dim sql As String = "select count(*) from contestacao c, faturas f where c.codigo_fatura=f.codigo_fatura "
            If faturas.Length > 0 Then
                Dim count As Integer = 0

                For Each codigo_fatura As String In faturas
                    If codigo_fatura <> "" Then
                        If count = 0 Then
                            sql = sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                            count = count + 1
                        Else
                            sql = sql + " ,'" + codigo_fatura + "'"
                        End If
                    End If
                Next
                If count = 1 Then
                    sql = sql + ")"
                End If
            End If


            Dim dt As DataTable = _dao_his.myDataTable(sql)


            If dt.Rows(0).Item(0) < 1 Then
                'não teve contestação então oculta as colunas

                'Me.gvRel.Columns(12).Visible = False
                'Me.gvRel.Columns(13).Visible = False
                'Me.gvRel.Columns(14).Visible = False
                'Me.gvRel.Columns(15).Visible = False

                txtMSG.Text = "Esta fatura não foi contestada"
            Else
                'se contestou mostra o resumo do rateio
                'ResumoRateio()

            End If
        Else


            Me.gvRel.Columns(12).Visible = False
            Me.gvRel.Columns(13).Visible = False
            Me.gvRel.Columns(14).Visible = False
            Me.gvRel.Columns(15).Visible = False

        End If
    End Sub

    Sub ResumoRateio()
        Dim faturas() As String
        faturas = ViewState("faturas").Split(" ")

        Dim sql As String = ""
        sql += " SELECT rf.descricao servico,f.descricao fatura, rf.valor, rt.tipo"
        sql += " FROM rateio_faturas rf, rateios_tipo rt, faturas f "
        sql += " where rf.rateio_tipo=rt.codigo "
        sql += " and rf.codigo_fatura=f.codigo_fatura"

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        sql = sql + " and rf.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        sql = sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                sql = sql + ")"
            End If
        End If

        Dim dt As DataTable = _dao_his.myDataTable(sql)

        If dt.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then


            For Each _row As DataRow In dt.Rows
                Dim achou As Boolean = False
                For Each _item As GridViewRow In gvServicoRateio.Rows

                    If _row.Item(0).ToString.ToUpper = _item.Cells(5).Text.ToUpper Then
                        'já exite nao adiciona
                    Else
                        Dim row As String() = New String() {"", "", "", "", _row.Item(0).ToString, "", "", "", 1, "", _row.Item(2).ToString, _row.Item(2).ToString}



                    End If

                Next


            Next


            Me.gvRateio.DataSource = dt
            Me.gvRateio.DataBind()
            Me.txtRateio.Visible = True
        Else
            Me.gvRateio.Visible = False
            Me.txtRateio.Visible = False
        End If




    End Sub


    Protected Sub gvRateio_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRateio.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            totalResumoRateio += e.Row.Cells(2).Text.Replace("R$", "").Replace(" ", "")
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(1).Text = "Total"
            e.Row.Cells(2).Text = FormatCurrency(totalResumoRateio)
        End If




    End Sub



    Sub ResumoFranquia()
        Dim faturas() As String
        faturas = ViewState("faturas").Split(" ")

        Dim sql As String = ""
        sql += " select f.nome franquia, f.valor consumo_contratado, ft.descricao tipo, "
        sql += " (select sum(fc.valor_faturado)valor from franquias_cobrancas fc where fc.codigo_franquia=f.codigo)valor_pacote "
        sql += " from FRANQUIAS f,franquias_tipos ft "
        sql += " where f.tipo_franquia=ft.codigo"

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        sql = sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        sql = sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                sql = sql + ")"
            End If
        End If

        Dim dt As DataTable = _dao_his.myDataTable(sql)

        If dt.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then
            Me.gvFranquia.DataSource = dt
            Me.gvFranquia.DataBind()
            Me.gvFranquia.Visible = True
        Else
            Me.gvFranquia.Visible = False
            Me.lbFranquia.Visible = False
        End If
    End Sub




    Protected Sub gvFranquia_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvFranquia.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            totalResumoFranquia += e.Row.Cells(3).Text.Replace("R$", "").Replace(" ", "")
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(2).Text = "Total"
            e.Row.Cells(3).Text = FormatCurrency(totalResumoFranquia)
        End If
    End Sub


    Sub ResumoServSemRateio()
        Dim faturas() As String
        faturas = ViewState("faturas").Split(" ")

        Dim sql As String = "select servico, sum(valor)valor, sum(AUDITADO)AUDITADO from("
        sql += " select p1.tipo_serv2 servico, sum(p1.valor_cdr)valor "
        sql += " , sum(p1.valor_cdr)-(select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.aprovada='S' and c.tipo_serv2=p1.tipo_serv2  "
        sql += " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=c.codigo_fatura and servico=c.tipo_serv2))"

        sql += ")AUDITADO"
        sql += " from cdrs_celular_analitico_mv p1, faturas_arquivos fa, faturas f "
        sql += " where p1.codigo_conta=fa.codigo_conta and fa.codigo_fatura=f.codigo_fatura "

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        sql = sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        sql = sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                sql = sql + ")"
            End If
        End If



        'descarta as linhas validas
        If Session("tipo") = "1" Then
            'linhas moveis
            sql = sql + " and  (not exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9') "
        Else
            'linhas fixas
            sql = sql + " and  (not  exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo))) "
        End If
        sql = sql + " )"
        sql += " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=fa.codigo_fatura) and servico=p1.tipo_serv2)"
        sql = sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=f.codigo_fatura and descricao=p1.tipo_serv2)"
        sql = sql + " group by p1.tipo_serv2,f.codigo_fatura,p1.rml_numero_a"
        sql = sql + " ) group by servico"

        'Response.Write(sql)
        'Response.End()

        Dim dt As DataTable = _dao_his.myDataTable(sql)

        If dt.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then
            Me.gvSemrateio.DataSource = dt
            Me.gvSemrateio.DataBind()
            Me.gvSemrateio.Visible = True
        Else
            Me.gvSemrateio.Visible = False
            Me.lbSemRateio.Visible = False
        End If
    End Sub

    Protected Sub gvSemrateio_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSemrateio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            totalResumoSemRateio += e.Row.Cells(1).Text.Replace("R$", "").Replace(" ", "")
            totalResumoSemRateioAuditado += e.Row.Cells(2).Text.Replace("R$", "").Replace(" ", "")
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            'e.Row.Cells(2).Text = "Total"
            e.Row.Cells(1).Text = FormatCurrency(totalResumoSemRateio)
            e.Row.Cells(2).Text = FormatCurrency(totalResumoSemRateioAuditado)
        End If
    End Sub

    Sub ResumoServicosFranquias()
        Dim cod_user As Integer = Session("codigousuario")
        Dim Sql As String = ""
        Dim order As String = ""
        Dim tipo As String = ""
        Dim faturas() As String
        Dim ccustos() As String

        faturas = ViewState("faturas").Split(" ")
        ccustos = ViewState("ccustos").Split(" ")

        Sql = Sql + " select "
        Sql = Sql + " linha,"
        Sql = Sql + " '-' grupo,'-' nome_grupo,'Sem usuário' usuario,'-' matricula,fatura,'SEM CADASTRO' status,'-' as tipo,'-' classificacao,"
        Sql = Sql + " sum(QTD)qtd,0  duracao,sum(""GASTO"")GASTO,0 ""CONSUMO(FRANQUIA)"","
        'Sql += "0 ""AUDITADO"","
        Sql += "sum(AUDITADO)AUDITADO,"
        Sql += "0 Rateio,0 ""GASTO+RATEIO"", 2 ordem, franquia,rateado"
        Sql += " from ( "


        Sql = Sql + " select "
        Sql = Sql + " p1.tipo_serv2 as linha,"
        Sql = Sql + " '-' grupo,'-' nome_grupo,'Sem usuário' usuario,'-' matricula,F.DESCRICAO fatura,'SEM CADASTRO' status,'-' as tipo,'-' classificacao,"
        Sql = Sql + " sum(chamadas) QTD,0  duracao,sum(p1.valor_cdr)""GASTO"",0 ""CONSUMO(FRANQUIA)"","
        'Sql += "0 ""AUDITADO"","
        Sql += "sum(p1.valor_cdr)-(select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.tipo_serv2=p1.tipo_serv2 and c.aprovada='S' "
        Sql += " and exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=c.codigo_fatura and servico=c.tipo_serv2))"
        Sql += " "
        Sql += ")AUDITADO,"


        Sql += "0 Rateio,0 ""GASTO+RATEIO"", 2 ordem"
        Sql += " , case when exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=F.codigo_fatura and servico=P1.tipo_serv2)) then 'SIM' else 'NÂO' end franquia"

        Sql = Sql + "    ,case when exists"
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2) then 'SIM' else 'NÃO' end rateado"

        Sql = Sql + " from cdrs_celular_analitico_mv p1,"
        Sql = Sql + " faturas                   f,"
        Sql = Sql + " faturas_arquivos          a"
        Sql = Sql + " where f.codigo_fatura = a.codigo_fatura "

        'Filtra faturas

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            Sql = Sql + " and exists(" & vbNewLine
            Sql = Sql + "   select 0 from categoria_usuario cat" & vbNewLine
            Sql = Sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            Sql = Sql + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            Sql = Sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        Sql = Sql + "     and a.codigo_conta = p1.codigo_conta"
        Sql = Sql + "     and f.codigo_tipo in (" + Session("tipo") + ")"
        Sql = Sql + "    and exists"
        Sql = Sql + "    (select a.codigo_conta from faturas f, faturas_arquivos a"
        Sql = Sql + "    where(   f.codigo_fatura = a.codigo_fatura)"
        Sql = Sql + "             and a.codigo_conta = p1.codigo_conta"
        'sql = sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        If Not String.IsNullOrEmpty(ViewState("operadora")) Then
            Sql = Sql + "             and codigo_operadora = '" & ViewState("operadora") & "' "
        End If
        Sql = Sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"

        'Sql = Sql + " and (exists "
        'Sql = Sql + " (select 0 from FRANQUIAS_COBRANCAS t "
        'Sql = Sql + " where t.codigo_franquia in"
        'Sql = Sql + "   (select codigo_franquia "
        'Sql = Sql + "   from franquias "
        'Sql = Sql + "  where codigo_fatura = a.codigo_fatura and servico = p1.tipo_serv2)"
        'linhas validas com pacotes
        'descarta as linhas validas
        'If Session("tipo") = "1" Then
        '    'linhas moveis
        '    Sql = Sql + " and  (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9')) ) "
        '    Sql = Sql + "   or not exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9')"
        'Else
        '    'linhas fixas
        '    Sql = Sql + " and  ( exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo))) )"
        '    Sql = Sql + "   or not exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9')"
        'End If


        ''Sql = Sql + "   and  (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9'))"
        ''Sql = Sql + "   )"

        'Sql = Sql + "   )"

        Sql += " and exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=F.codigo_fatura and servico=P1.tipo_serv2))"

        'retira os servs rateados
        'Sql = Sql + "    and not exists"
        'Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        'Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        'Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
        ''sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        'Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"

        Sql = Sql + " GROUP BY F.DESCRICAO,p1.rml_numero_a,p1.tipo_serv2,f.Codigo_Fatura"
        Sql = Sql + "   ) group by fatura,linha, franquia,rateado"
        Sql = Sql + " order by linha "

        'Response.Write(Sql)
        'Response.End()

        Dim dt As DataTable = _dao_his.myDataTable(Sql)

        If dt.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then
            Me.gvRelServicos.DataSource = dt
            Me.gvRelServicos.DataBind()
            Me.gvRelServicos.Visible = True
        Else
            Me.gvRelServicos.Visible = False
            Me.lbServicos.Visible = False
        End If

    End Sub

    Sub ResumoServicosRateados()
        Dim cod_user As Integer = Session("codigousuario")
        Dim Sql As String = ""
        Dim order As String = ""
        Dim tipo As String = ""
        Dim faturas() As String
        Dim ccustos() As String

        faturas = ViewState("faturas").Split(" ")
        ccustos = ViewState("ccustos").Split(" ")

        Sql = Sql + " select "
        Sql = Sql + " linha,"
        Sql = Sql + " '-' grupo,'-' nome_grupo,'Sem usuário' usuario,'-' matricula,fatura,'SEM CADASTRO' status,'-' as tipo,'-' classificacao,"
        Sql = Sql + " sum(QTD)qtd,0  duracao,sum(""GASTO"")GASTO,0 ""CONSUMO(FRANQUIA)"","
        'Sql += "0 ""AUDITADO"","
        Sql += "sum(AUDITADO)AUDITADO,"
        Sql += "0 Rateio,0 ""GASTO+RATEIO"", 2 ordem, franquia,rateado"
        Sql += " from ( "


        Sql = Sql + " select "
        Sql = Sql + " p1.tipo_serv2 as linha,"
        Sql = Sql + " '-' grupo,'-' nome_grupo,'Sem usuário' usuario,'-' matricula,F.DESCRICAO fatura,'SEM CADASTRO' status,'-' as tipo,'-' classificacao,"
        Sql = Sql + " sum(chamadas) QTD,0  duracao,sum(p1.valor_cdr)""GASTO"",0 ""CONSUMO(FRANQUIA)"","
        'Sql += "0 ""AUDITADO"","
        Sql += "sum(p1.valor_cdr)-(select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.tipo_serv2=p1.tipo_serv2 and c.aprovada='S' "
        'sql += " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=c.codigo_fatura and servico=c.tipo_serv2))"
        Sql += " "
        Sql += ")AUDITADO,"


        Sql += "0 Rateio,0 ""GASTO+RATEIO"", 2 ordem"
        Sql += " , case when exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=F.codigo_fatura and servico=P1.tipo_serv2)) then 'SIM' else 'NÂO' end franquia"

        Sql = Sql + "    ,case when exists"
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2) then 'SIM' else 'NÃO' end rateado"

        Sql = Sql + " from cdrs_celular_analitico_mv p1,"
        Sql = Sql + " faturas                   f,"
        Sql = Sql + " faturas_arquivos          a"
        Sql = Sql + " where f.codigo_fatura = a.codigo_fatura "

        'Filtra faturas

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            Sql = Sql + " and exists(" & vbNewLine
            Sql = Sql + "   select 0 from categoria_usuario cat" & vbNewLine
            Sql = Sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            Sql = Sql + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            Sql = Sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        Sql = Sql + "     and a.codigo_conta = p1.codigo_conta"
        Sql = Sql + "     and f.codigo_tipo in (" + Session("tipo") + ")"
        Sql = Sql + "    and exists"
        Sql = Sql + "    (select a.codigo_conta from faturas f, faturas_arquivos a"
        Sql = Sql + "    where(   f.codigo_fatura = a.codigo_fatura)"
        Sql = Sql + "             and a.codigo_conta = p1.codigo_conta"
        'sql = sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        If Not String.IsNullOrEmpty(ViewState("operadora")) Then
            Sql = Sql + "             and codigo_operadora = '" & ViewState("operadora") & "' "
        End If
        Sql = Sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"

        Sql = Sql + " and (exists "
        Sql = Sql + " (select 0 from FRANQUIAS_COBRANCAS t "
        Sql = Sql + " where t.codigo_franquia in"
        Sql = Sql + "   (select codigo_franquia "
        Sql = Sql + "   from franquias "
        Sql = Sql + "  where codigo_fatura = a.codigo_fatura and servico = p1.tipo_serv2)"
        'linhas validas com pacotes
        'descarta as linhas validas
        If Session("tipo") = "1" Then
            'linhas moveis
            Sql = Sql + " and  (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9')) ) "
            Sql = Sql + "   or not exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9')"
        Else
            'linhas fixas
            Sql = Sql + " and  ( exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo))) ) )"
            Sql = Sql + "   or not exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)))"
        End If


        'Sql = Sql + "   and  (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(p1.rml_numero_a, 1,length(pre.prefixo)) or substr(p1.rml_numero_a, 3,1)='9'))"
        'Sql = Sql + "   )"

        Sql = Sql + "   )"

        'somente rateados
        Sql = Sql + "    and exists"
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2)"

        'retira os servs rateados
        'Sql = Sql + "    and not exists"
        'Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        'Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        'Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
        ''sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        'Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"

        Sql = Sql + " GROUP BY F.DESCRICAO,p1.rml_numero_a,p1.tipo_serv2,f.Codigo_Fatura"
        Sql = Sql + "   ) group by fatura,linha, franquia,rateado"

        Dim sql2 As String = Sql

        'union com as sobras
        Sql = Sql + " union "
        Sql = Sql + " SELECT"
        Sql = Sql + " rf.descricao || '(Sobra de Franquia)'servico, "
        Sql = Sql + "        '-' grupo,'-' nome_grupo,'Sem usuário' usuario,'-' matricula,f.descricao fatura,'SEM CADASTRO' status,'-' as tipo,'-' classificacao,1 qtd,0 duracao,"
        Sql = Sql + "    rf.valor gasto,0,rf.valor auditado,0 Rateio,0 ""GASTO+RATEIO"",2 ordem,'NAO','SIM'  rateado"
        Sql = Sql + " FROM rateio_faturas rf, rateios_tipo rt, faturas f"
        Sql = Sql + " where rf.rateio_tipo=rt.codigo "
        Sql = Sql + " and rf.codigo_fatura=f.codigo_fatura "
        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If
        Sql = Sql + "and not exists( select linha from (" & sql2 & ") where linha=rf.descricao)"
        Sql = Sql + " "
        Sql = Sql + " "
        Sql = Sql + " "
        Sql = Sql + " "
        Sql = Sql + " "
        Sql = Sql + " "
        Sql = Sql + " "

        'Sql = Sql + " order by linha "

        'Response.Write(Sql)
        'Response.End()

        Dim dt As DataTable = _dao_his.myDataTable(Sql)


        Sql = " SELECT rf.descricao servico,f.descricao fatura, rf.valor, rt.tipo"
        Sql += " FROM rateio_faturas rf, rateios_tipo rt, faturas f "
        Sql += " where rf.rateio_tipo=rt.codigo "
        Sql += " and rf.codigo_fatura=f.codigo_fatura"

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and rf.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If

        Dim dt2 As DataTable = _dao_his.myDataTable(Sql)

        If dt2.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then

            Dim _rows As New List(Of DataRow)


            For Each _row As DataRow In dt2.Rows
                Dim achou As Boolean = False

                For Each _item As DataRow In dt.Rows

                    If _row.Item(0).ToString.ToUpper = _item.Item("linha").ToString.ToUpper Then
                        'já exite nao adiciona
                        TotalfaturadoServRateio += _item.Item("GASTO")
                        TotalfaturadoservRateioAuditado += _item.Item("AUDITADO")
                        achou = True
                    Else
                    End If

                Next
                'dt.Rows.Add(_rows.ForEach()


            Next
            'For Each row As DataRow In _rows
            '    dt.Rows.Add(row)
            'Next


        End If


        If dt.Rows.Count > 0 And DALCGestor.AcessoAdmin() Then
            Me.gvServicoRateio.DataSource = dt
            Me.gvServicoRateio.DataBind()
            Me.gvServicoRateio.Visible = True
        Else
            Me.gvServicoRateio.Visible = False
            Me.lbServicosRateios.Visible = False
        End If

    End Sub

    Protected Sub gvRelServicos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRelServicos.RowDataBound

        If e.Row.RowType = DataControlRowType.DataRow Then
            totalQTDServicos += e.Row.Cells(9).Text.Replace("R$", "").Replace(" ", "")
            totalfaturadoServicos += e.Row.Cells(11).Text.Replace("R$", "").Replace(" ", "")
            totalAuditadoServicos += e.Row.Cells(12).Text.Replace("R$", "").Replace(" ", "")
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(5).Text = "Total"
            e.Row.Cells(9).Text = FormatNumber(totalQTDServicos, 0)
            e.Row.Cells(11).Text = FormatCurrency(totalfaturadoServicos)
            e.Row.Cells(12).Text = FormatCurrency(totalAuditadoServicos)
        End If

    End Sub

    Protected Sub gvServicoRateio_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvServicoRateio.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            totalQTDServicosRateados += e.Row.Cells(9).Text.Replace("R$", "").Replace(" ", "")
            totalfaturadoServicosRateados += e.Row.Cells(11).Text.Replace("R$", "").Replace(" ", "")
            totalAuditadoServicosRateados += e.Row.Cells(12).Text.Replace("R$", "").Replace(" ", "")
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(5).Text = "Total"
            e.Row.Cells(9).Text = FormatNumber(totalQTDServicosRateados, 0)
            e.Row.Cells(11).Text = FormatCurrency(totalfaturadoServicosRateados)
            e.Row.Cells(12).Text = FormatCurrency(totalAuditadoServicosRateados)
        End If
    End Sub

    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender

        'gvRel.FooterRow.TableSection = TableRowSection.TableFooter
        MontaTotaisServicosComp()
       

    End Sub

    Sub MontaTotaisServicosComp()
        'total de serviços compartilhados
        Dim totalFranquias As Double = 0
        Dim totalFranquiasAuditado As Double = 0
        If Request.QueryString("VP") = "" Then

            ' Me.gvRel.Rows(gvRel.Rows.Count - 2).Visible = False
            'Me.gvRel.Rows(gvRel.Rows.Count - 1).Visible = False




            If gvRelServicos.Rows.Count > 0 Then
                totalFranquias = Convert.ToDouble(gvRelServicos.FooterRow.Cells(11).Text.Replace(" ", "").Replace("R$", ""))
                totalFranquiasAuditado = Convert.ToDouble(gvRelServicos.FooterRow.Cells(12).Text.Replace(" ", "").Replace("R$", ""))
            End If

            Dim totalCompartilhado As Double = TotalfaturadoServRateio + totalFranquias + totalResumoSemRateio
            Dim totalCompartilhadoAuditado As Double = TotalfaturadoservRateioAuditado + totalFranquiasAuditado + totalResumoSemRateioAuditado

            If ViewState("ccustos") = "" Then
                If ViewState("analitico") = "1" Then


                    If gvRel.Rows.Count > 0 Then


                        'Me.gvRel.Controls(0).Controls.Add(New GridViewRowCollection)



                        'gvRel.Controls.Add(New HtmlControls.HtmlGenericControl("<tbody>"))

                        Dim totalfaturado As Double = Convert.ToDouble(gvRel.FooterRow.Cells(11).Text.Replace(" ", "").Replace("R$", ""))
                        'Dim totalfaturado As Double = totalfaturado


                        Dim rowCompartilhado As GridViewRow = New GridViewRow(Me.gvRel.Rows.Count, 0, DataControlRowType.Footer, DataControlRowState.Normal)
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        'rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView("Total de Serviços Compartilhados: ", 3))
                        'class="avoid-sort"
                        'rowCompartilhado.CssClass = "avoid-sort"

                        rowCompartilhado.Cells.Add(GeraColunaGridView(FormatCurrency(totalCompartilhado)))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(FormatCurrency(totalCompartilhadoAuditado), , True))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                        rowCompartilhado.Visible = True


                        Me.gvRel.Controls(0).Controls.Add(rowCompartilhado)



                        Dim rowTotalfaturado As GridViewRow = New GridViewRow(Me.gvRel.Rows.Count, 0, DataControlRowType.Footer, DataControlRowState.Normal)
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView("Total: ", 3))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(FormatCurrency(totalfaturado + totalCompartilhado)))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(FormatCurrency(totalAuditoria + totalCompartilhadoAuditado), , True))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                        rowTotalfaturado.Visible = True

                        Me.gvRel.Controls(0).Controls.Add(rowTotalfaturado)
                        'Me.gvRel.Controls(0).DataBind()

                        'gvRel.Controls.Add(New HtmlControls.HtmlGenericControl("</tbody>"))
                    End If
                Else

                    If Me.gvRelConsolidado.Rows.Count > 0 Then
                        Dim totalfaturado As Double = Convert.ToDouble(gvRelConsolidado.FooterRow.Cells(5).Text.Replace(" ", "").Replace("R$", ""))
                    End If


                    Dim rowCompartilhado As GridViewRow = New GridViewRow(Me.gvRelConsolidado.Rows.Count, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                    rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                    rowCompartilhado.Cells.Add(GeraColunaGridView(""))
                    rowCompartilhado.Cells.Add(GeraColunaGridView("Total de Serviços Compartilhados: ", 3))
                    rowCompartilhado.Cells.Add(GeraColunaGridView(FormatCurrency(totalCompartilhado)))
                    rowCompartilhado.Cells.Add(GeraColunaGridView(FormatCurrency(totalCompartilhadoAuditado), , True))
                    rowCompartilhado.Visible = True


                    Me.gvRelConsolidado.Controls(0).Controls.Add(rowCompartilhado)


                    Dim rowTotalfaturado As GridViewRow = New GridViewRow(Me.gvRelConsolidado.Rows.Count, 0, DataControlRowType.DataRow, DataControlRowState.Normal)
                    rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                    rowTotalfaturado.Cells.Add(GeraColunaGridView(""))
                    rowTotalfaturado.Cells.Add(GeraColunaGridView("Total: ", 3))
                    rowTotalfaturado.Cells.Add(GeraColunaGridView(FormatCurrency(totalFaturado + totalCompartilhado)))
                    rowTotalfaturado.Cells.Add(GeraColunaGridView(FormatCurrency(totalAuditoria + totalCompartilhadoAuditado), , True))
                    rowTotalfaturado.Visible = True

                    If Me.gvRelConsolidado.Rows.Count > 0 Then
                        Me.gvRelConsolidado.Controls(0).Controls.Add(rowTotalfaturado)
                    End If



                End If
            End If

            If gvRel.Rows.Count > 0 Then
                gvRel.HeaderRow.TableSection = TableRowSection.TableHeader
                'gvRel.FooterRow.TableSection = TableRowSection.TableFooter

                'e.Row.Cells(14).BackColor = Drawing.Color.LightBlue
                'gvRel.DataBind()
            End If




            If _contestou = False Then

                Me.txtMSG.Text = "Existem faturas sem contestações concluídas que poderão gerar inconsistências no relatório."
                Me.txtMSG.Visible = True
            End If
        End If
    End Sub

    Private Function GeraColunaGridView(ByVal texto As String, Optional ByVal cellSpan As Integer = 0, Optional ByVal pinta As Boolean = False, Optional ordena As Boolean = True) As TableCell


        Dim myCell As TableCell = New TableCell
        myCell.Text = texto

        If cellSpan > 0 Then
            myCell.ColumnSpan = cellSpan
        End If

        If pinta Then

            myCell.BackColor = Drawing.Color.LightBlue

        End If

        If Not ordena Then
            myCell.CssClass = "avoid-sort"
        End If

        Return myCell

    End Function





    Protected Sub gvCabecalho_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCabecalho.RowDataBound



        If e.Row.RowType = DataControlRowType.DataRow Then

            If Server.HtmlDecode(e.Row.Cells(4).Text.ToUpper) = "NÃO" Then
                _contestou = False
            End If

        End If




    End Sub

    Protected Sub gvRelConsolidado_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRelConsolidado.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            totalQTD += e.Row.Cells(3).Text
            totalMinutagem += e.Row.Cells(4).Text
            totalFaturado += e.Row.Cells(5).Text.Replace("R$", "").Replace(" ", "")
            totalAuditoria += e.Row.Cells(6).Text.Replace("R$", "").Replace(" ", "")
            totalFranquia += FormatNumber(e.Row.Cells(7).Text.Replace("R$", "").Replace(" ", ""), 4)
            totalRateio += FormatNumber(e.Row.Cells(8).Text.Replace("R$", "").Replace(" ", ""), 4)
            totalGeral += FormatNumber(e.Row.Cells(9).Text.Replace("R$", "").Replace(" ", ""), 4)
        End If


        If e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(3).Text = FormatNumber(totalQTD, 0)
            e.Row.Cells(4).Text = FormatNumber(totalMinutagem, 2)

            e.Row.Cells(5).Text = FormatCurrency(totalFaturado)
            e.Row.Cells(6).Text = FormatCurrency(totalAuditoria)
            e.Row.Cells(7).Text = FormatCurrency(totalFranquia)

            e.Row.Cells(8).Text = FormatCurrency(totalRateio)
            e.Row.Cells(9).Text = FormatCurrency(totalGeral)

            e.Row.Cells(6).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(7).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(8).BackColor = Drawing.Color.LightBlue
            e.Row.Cells(9).BackColor = Drawing.Color.LightBlue

        End If

    End Sub


    Sub CarregaCompartilhado()
        Dim cod_user As Integer = Session("codigousuario")
        Dim Sql As String = ""
        Dim order As String = ""
        Dim tipo As String = ""
        Dim faturas() As String
        Dim ccustos() As String

        faturas = ViewState("faturas").Split(" ")
        ccustos = ViewState("ccustos").Split(" ")

        Sql = Sql + " select replace(replace(replace(nvl(celular,' '),'(',''),')',''),'-','') as linha,nvl(TO_CHAR(p00.grupo), '[SEM CADASTRO]') grupo,"
        Sql = Sql + " nvl(p1.nome_grupo, '[SEM CADASTRO]') nome_grupo,"
        Sql = Sql + " nvl(nome_usuario, 'Sem usuário') usuario,"
        Sql = Sql + " nvl(p0.matricula, '-') matricula,"
        Sql = Sql + " fatura,"
        Sql = Sql + " "
        Sql = Sql + " nvl(sl.descricao, 'SEM CADASTRO') status,"
        'sql = sql + " decode(nvl(cod_tipo, '0'),'1','CELULAR','2','RADIO','3','MODEM','4','SMARTPHONE',"
        'sql = sql + " '5','BLACKBERRY','6','GATEWAY','OUTROS') as tipo,"
        Sql += " nvl(at.nome,'-')  as tipo,"
        Sql = Sql + " nvl(lt.tipo, '-') classificacao,"
        Sql = Sql + " (chamadas) QTD,"
        Sql = Sql + " (round(duracao/60, 10)) duracao,"
        Sql = Sql + " (nvl(gasto, 0)) ""GASTO"", "
        Sql = Sql + " (round(nvl(consumo, 0),4)) ""CONSUMO(FRANQUIA)"", "
        'Sql = Sql + " to_char(nvl(valor_audit, 0)) ""AUDITADO"", "
        Sql = Sql + " (nvl(gasto, 0)-valor_devolvido) ""AUDITADO"", "

        'If Session("tipo") = "1" Then
        '    'linhas moveis
        '    Sql = Sql + " case when exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (nvl(gasto, 0)-valor_devolvido) "

        'Else
        '    'linhas fixas
        '    Sql = Sql + " case when exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (nvl(gasto, 0)-valor_devolvido) "
        'End If
        'Sql += " else 0 end  ""AUDITADO"","



        Sql = Sql + " (ROUND(nvl(Rateio, 0), 4)) Rateio "
        Sql = Sql + " ,(ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) ""GASTO+RATEIO"" "
        Sql = Sql + " ,1 ordem "

        'If Session("tipo") = "1" Then
        '    'linhas moveis
        '    Sql = Sql + " ,case when exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
        '    'Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) "

        'Else
        '    'linhas fixas
        '    Sql = Sql + " ,case when exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
        '    Sql = Sql + " and not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"
        '    Sql = Sql + " then (ROUND(nvl(NVL(Rateio,0)+nvl(consumo, 0)+(nvl(gasto, 0)-valor_devolvido), 0), 4)) "
        'End If
        'Sql += " else 0 end  ""GASTO+RATEIO"" "


        Sql = Sql + "    from (select to_char(p1.grp_codigo) grupo,"
        Sql = Sql + "    nvl(replace(p1.rml_numero_a, ' ', ''), '[NULO]') Celular,"
        Sql = Sql + "    f.descricao fatura,"
        Sql = Sql + "    f.codigo_fatura,"
        'sql = sql + "    nvl(r.rateio,0) rateio,"

        'Se a linha for inválida e teve rateio deve ser zero
        Sql = Sql + "    case when exists "
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')  "
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "') then 0"
        Sql = Sql + "else nvl(r.rateio,0) end rateio, "
        Sql = Sql + "    sum(nvl(chamadas, 0)) chamadas,"
        Sql = Sql + "    round(sum(duracao), 2) duracao,"
        Sql = Sql + "    sum(p1.valor_cdr) gasto,"
        'Sql = Sql + "    sum(p1.valor_cdr2) consumo,"
        Sql = Sql + " sum(case when p1.tipo_serv2 in (select fs.servico from franquias_servicos fs, franquias fr where fs.codigo_franquia=fr.codigo and fr.codigo_fatura=f.codigo_fatura) and p1.valor_cdr=0  "
        Sql = Sql + " then round(p1.valor_franquia,4) else 0 end) consumo,"
        Sql = Sql + " "



        Sql = Sql + "    sum(p1.valor_audit) valor_audit,"
        'Sql += " (select nvl(sum(c.valor_faturado-c.valor_audit),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.aprovada='S')valor_devolvido,"
        Sql += " (select nvl(sum(c.valor_devolvido),0) from VCONTESTACOESFATURASLINHAS c where c.codigo_fatura=f.codigo_fatura and c.linha=p1.rml_numero_a and c.aprovada='S' "
        Sql += " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=c.codigo_fatura and servico=c.tipo_serv2))"

        Sql += ")valor_devolvido,"
        Sql = Sql + "    p1.codigo_usuario "
        Sql = Sql + "    from cdrs_celular_analitico_mv p1,"
        Sql = Sql + "         faturas                   f,"
        Sql = Sql + "         faturas_arquivos          a,"
        Sql = Sql + "         RateioGestao_MV r "
        Sql = Sql + "     where(f.codigo_fatura = a.codigo_fatura)"

        'Filtra faturas

        If faturas.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_fatura As String In faturas
                If codigo_fatura <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and f.codigo_fatura in ('" + codigo_fatura + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_fatura + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If

        'If Not String.IsNullOrEmpty(Me.tbCCUsuario.Text) Then
        '    sql = sql + " and p1.grp_codigo = '" + Me.tbCCUsuario.Text.ToString + "'"
        'End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            Sql = Sql + " and exists(" & vbNewLine
            Sql = Sql + "   select 0 from categoria_usuario cat" & vbNewLine
            Sql = Sql + "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            Sql = Sql + "     and cat.tipo_usuario in('D','G')" & vbNewLine
            Sql = Sql + "     and to_char(p1.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        Sql = Sql + "     and a.codigo_conta = p1.codigo_conta"
        Sql = Sql + "     and f.codigo_tipo in (" + Session("tipo") + ")"
        Sql = Sql + "     and p1.rml_numero_a = r.rml_numero_a(+)"
        Sql = Sql + "    and p1.codigo_conta = r.codigo_conta(+)"
        Sql = Sql + "    and exists"
        Sql = Sql + "    (select a.codigo_conta from faturas f, faturas_arquivos a"
        Sql = Sql + "    where(   f.codigo_fatura = a.codigo_fatura)"
        Sql = Sql + "             and a.codigo_conta = p1.codigo_conta"
        'sql = sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        If Not String.IsNullOrEmpty(ViewState("operadora")) Then
            Sql = Sql + "             and codigo_operadora = '" & ViewState("operadora") & "' "
        End If
        Sql = Sql + "             and to_char(dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"



        Sql = Sql + "    and not exists"
        Sql = Sql + "   (select 0 from rateio_faturas ra, faturas fa"
        Sql = Sql + "    where(ra.codigo_fatura = f.codigo_fatura)"
        Sql = Sql + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
        'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"
        Sql = Sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & ViewState("data") & "')"
        Sql = Sql + "           group by p1.grp_codigo,"
        Sql = Sql + "           p1.rml_numero_a,"
        Sql = Sql + "           f.descricao,"
        Sql = Sql + "           f.codigo_fatura,"
        Sql = Sql + "           r.rateio,"
        Sql = Sql + "           p1.codigo_usuario) p00,"
        Sql = Sql + "                        linhas p2,"
        Sql = Sql + "                 linhas_moveis p3,"
        Sql = Sql + "                        grupos p1,"
        Sql = Sql + "              aparelhos_moveis ap,"
        Sql = Sql + "             aparelhos_modelos mo,"
        Sql = Sql + "                 status_linhas sl,"
        Sql = Sql + "                   linhas_tipo lt,"
        Sql = Sql + "                      usuarios p0 "
        Sql = Sql + "                      ,aparelhos_tipos at "
        Sql = Sql + " where replace(replace(REPLACE(p2.NUM_LINHA(+), ')', ''), '(', ''), '-', '') ="
        Sql = Sql + " replace(replace(REPLACE(celular, ')', ''), '(', ''), '-', '')"

        'retira as linhas invalidas se houver rateio
        'Sql = Sql + " and ( exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "

        'descarta as linhas validas
        If Session("tipo") = "1" Then
            'linhas moveis
            Sql = Sql + " and  not (exists (select 0 from prefixos_celulares pre where pre.prefixo=substr(celular, 1,length(pre.prefixo)) or substr(celular, 3,1)='9') "
            'tira as cobranças de franquias
            Sql = Sql + " or exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
            Sql = Sql + " )"
        ElseIf Session("tipo") = "2" Then
            'linhas fixas
            Sql = Sql + " and  not ( exists (select 0 from PREFIXOS_FIXOS pre where pre.prefixo=substr(celular, 1,length(pre.prefixo))) "
            Sql = Sql + " or exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
            Sql = Sql + " )"
        End If
        'Sql = Sql + " or not exists (select 0 from rateio_faturas where codigo_fatura=p00.codigo_fatura)"

        'FIM retira as linhas invalidas se houver rateio

        Sql = Sql + " and p00.codigo_usuario = p0.codigo(+)"
        Sql = Sql + " and p2.codigo_linha = p3.codigo_linha(+)"
        Sql = Sql + " and to_char(p00.grupo) = to_char(p1.codigo(+))"
        Sql = Sql + " and p3.codigo_aparelho = ap.codigo_aparelho(+)"
        Sql = Sql + " and ap.cod_modelo = mo.cod_modelo(+)"
        Sql = Sql + " and p2.status = sl.codigo_status(+)"
        Sql = Sql + " and p2.codigo_tipo = lt.codigo_tipo(+)"
        Sql = Sql + " and mo.cod_tipo=at.codigo_tipo(+)"

        If ViewState("linhas_com_gasto") = True Then
            Sql = Sql + " and p00.gasto > 0"
        End If
        If ViewState("linhas_sem_gasto") = True Then
            Sql = Sql + " and p00.gasto = 0"
        End If

        If ccustos.Length > 0 Then
            Dim count As Integer = 0

            For Each codigo_ccusto As String In ccustos
                If codigo_ccusto <> "" Then
                    If count = 0 Then
                        Sql = Sql + " and p1.codigo in ('" + codigo_ccusto + "'"
                        count = count + 1
                    Else
                        Sql = Sql + " ,'" + codigo_ccusto + "'"
                    End If
                End If
            Next
            If count = 1 Then
                Sql = Sql + ")"
            End If
        End If

        'If tbCCUsuario.Text <> "" Then
        '    sql = sql + " and p1.codigo = '" + tbCCUsuario.Text + "'"
        'End If

        If Not String.IsNullOrEmpty(ViewState("classificacao")) Then
            ' sql = sql + " and upper(nvl(lt.tipo, 'MOVEL')) = '" + ViewState("classificacao").ToString.ToUpper + "'"
            Sql = Sql + " and case when lt.tipo is not null then lt.tipo when p3.codigo_linha is not null then 'MOVEL' when P00.CODIGO_USUARIO is null then '" + ViewState("classificacao").ToString.ToUpper + "'  else 'LINHA DIRETA' end  = '" + ViewState("classificacao").ToString.ToUpper + "'"

        End If


        Sql = Sql + "  order by ordem, grupo, linha, gasto desc"

        Response.Write(Sql)
        Response.End()



        If Not ViewState("analitico") Then
            'se for consolidado
            Dim Sql2 As String = ""
            Sql2 = Sql2 + " select  nvl(to_char(grupo), '[SEM CADASTRO]') grupo,"
            Sql2 = Sql2 + " nvl(nome_grupo, '[SEM CADASTRO]') nome_grupo,"
            Sql2 += " count(linha) qtd_celular,"
            Sql2 += " (sum(QTD)) QTD,"
            Sql2 += " (round(sum(duracao), 10)) duracao,"
            Sql2 += " (nvl(sum(gasto), 0)) gasto, "
            Sql2 += " (nvl(sum(AUDITADO), 0)) ""AUDITADO"", "
            Sql2 += " (nvl(sum(""CONSUMO(FRANQUIA)""), 0)) ""CONSUMO(FRANQUIA)"", "
            Sql2 += "  (ROUND(nvl(sum(Rateio), 0), 4)) Rateio "
            Sql2 += " ,(sum(ROUND(nvl(NVL(Rateio,0)+AUDITADO+""CONSUMO(FRANQUIA)"", 0), 4))) ""GASTO+RATEIO"" "

            Sql2 += " from ("
            Sql2 += Sql

            Sql2 += " )"

            Sql2 += "  group by grupo, nome_grupo"

            Sql2 += "  order by gasto desc, grupo, nome_grupo"
            Sql = Sql2
        End If



        Dim connection As New OleDbConnection(strConexao)

        'Dim cmd As OleDbCommand = connection.CreateCommand
        'cmd.CommandText = Sql
        'Dim reader As OleDbDataReader
        'connection.Open()
        'reader = cmd.ExecuteReader
        Dim dt As DataTable = _dao_his.myDataTable(Sql)








    End Sub

End Class
