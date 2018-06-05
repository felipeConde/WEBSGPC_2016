﻿Imports System.Data
Imports System.Web.Script.Serialization

Partial Class uc_graficoHome
    Inherits System.Web.UI.Page
    Public usuario As AppUsuarios
    Public nomeusuario As String
    Dim _dao_commons As New DAO_Commons
    Public myUrl As String = ""

    Public Property GraficoData() As String
        Get
            Return ViewState("graficoData")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData") = value
        End Set
    End Property
    Public Property GraficoData2() As String
        Get
            Return ViewState("graficoData2")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData2") = value
        End Set
    End Property
    Public Property GraficoData3() As String
        Get
            Return ViewState("graficoData3")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData3") = value
        End Set
    End Property
    Public Property GraficoData4() As String
        Get
            Return ViewState("graficoData4")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData4") = value
        End Set
    End Property
    Public Property GraficoData5() As String
        Get
            Return ViewState("graficoData5")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData5") = value
        End Set
    End Property
    'link de dados
    Public Property GraficoData6() As String
        Get
            Return ViewState("graficoData6")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData6") = value
        End Set
    End Property
    'ramais
    Public Property GraficoData7() As String
        Get
            Return ViewState("GraficoData7")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoData7") = value
        End Set
    End Property
    Public Property GraficoLabel() As String
        Get
            Return ViewState("graficoLabel")
        End Get
        Set(ByVal value As String)
            ViewState("graficoLabel") = value
        End Set
    End Property

    Public Class Serie
        Private _nome As String
        Public Property Nome As String
            Get
                Return _nome
            End Get
            Set(value As String)
                _nome = value
            End Set
        End Property

        Private _data As String
        Public Property Data As String
            Get
                Return _data
            End Get
            Set(value As String)
                _data = value
            End Set
        End Property
    End Class

    Public strGrafico As String = ""
    Public strSQL As String = ""
    Public Meta As Double
    Public ExibeMovel As Boolean = False
    Public ExibeFixo As Boolean = False
    Public Exibe0800 As Boolean = False
    Public Exibe3003 As Boolean = False
    Public ExibeServico As Boolean = False
    Public exibeDados As Boolean = False
    Public exibeRamail As Boolean = False
    Public _excluirServico As String = ""
    Public total12meses As Double = 0
    Public sqlTotal As String = ""
    Public totalMesAtual As Double = 0
    Public MesAtual As String = ""
    Public VariacaoMesAnterior As Double = 0
    Dim controller As New HomeController
    Dim _dao As New DAO_Dashboard
    Public tipoGrafico As String = ""
    Dim grupo As String = ""
    Dim area As String = ""
    Dim area_interna As String = ""
    Dim hierarquia As String = ""
    Dim tipoValor As String = 1
    Public negativeValue As Decimal = 0
    Public tipoFatura As String = 1
    Public TipoVisao As String = "Tipo"
    Public virgulaGrafico As String = ""


    Public GraficoDataDouble As New List(Of Double)

    Private Sub uc_graficoHome_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("codigousuario") Is Nothing Or Session("usuario") Is Nothing Then
            Response.Write("Logue Novamente")

            Response.Write("<script>window.location.href ='default.aspx';</script>")

            Exit Sub
            'Response.End()
            'Response.Redirect("default.aspx")
        End If
        usuario = Session("usuario")
        nomeusuario = usuario.Nome_Usuario
        myUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) & "/"

        'If _dao_commons.getLabel("EXIBE_TARIFACAO") = "S" Then

        '    'Me.btTarifacao.visible = True

        'End If

        Session.Timeout = 99999
        'CarregaGrupos()

        If Not Request.QueryString("grupo") Is Nothing Then
            Try
                'grupo = Request.QueryString("grupo")
                grupo = HttpUtility.UrlDecode(Request.QueryString("grupo"), System.Text.Encoding.Default)
            Catch ex As Exception
            End Try
        End If
        If Not Request.QueryString("area") Is Nothing Then
            Try
                'area = Server.HtmlDecode(Request.QueryString("area"))
                area = HttpUtility.UrlDecode(Request.QueryString("area"), System.Text.Encoding.Default)

            Catch ex As Exception
            End Try
        End If
        If Not Request.QueryString("area_interna") Is Nothing Then
            Try
                'area_interna = Request.QueryString("area_interna")
                area_interna = HttpUtility.UrlDecode(Request.QueryString("area_interna"), System.Text.Encoding.Default)
            Catch ex As Exception
            End Try
        End If
        If Not Request.QueryString("tipoGrafico") Is Nothing Then
            Try
                tipoGrafico = Request.QueryString("tipoGrafico")
            Catch ex As Exception
                tipoGrafico = "1"
            End Try
        Else
            tipoGrafico = "1"
        End If

        If Not Request.QueryString("tipoFatura") Is Nothing Then
            Try
                tipoFatura = Request.QueryString("tipoFatura")
            Catch ex As Exception
                tipoFatura = "1"
            End Try
        Else
            tipoFatura = "1"
        End If


        If Not Request.QueryString("TipoVisao") Is Nothing Then
            Try
                TipoVisao = Request.QueryString("TipoVisao")
            Catch ex As Exception
                TipoVisao = "Tipo"
            End Try
        Else
            TipoVisao = "Tipo"
        End If



        'If (tipoValor = 2) And DALCGestor.AcessoAdmin() Then
        '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "DEVIDO", area, area_interna)
        '    'Carrega_Grafico(grupo, hierarquia, "Exibe-Ramal", "DEVIDO")
        '    'Me.lstRbTipoValor.SelectedValue = 2
        'Else
        '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna)
        '    'Me.lstRbTipoValor.SelectedValue = 1
        'End If
        'Response.Write(ViewState("tipoGrafico"))
        'Response.End()
        Me.txtTipoGrafico.Text = tipoGrafico
        Me.txtTipoVisao.Text = TipoVisao
        Dim exibe_ramal = ""
        '
        If _dao_commons.getLabel("EXIBE_TARIFACAO") = "S" Then
            exibe_ramal = "Exibe-Ramal"
            ExibeFixo = False


        End If


        If tipoGrafico = "1" Or String.IsNullOrEmpty(tipoGrafico) Or tipoGrafico = "" Then
            'grafico geral
            Carrega_Grafico(grupo, hierarquia, exibe_ramal, "FATURADO", area, area_interna)
            spanExibePor.Visible = False
            CarregaGastoUltimoMes()
            Me.divDetalhar.Visible = False
            Me.divDetalharGeral.Visible = True
        ElseIf tipoGrafico = "2" Then
            'gráfico móvel
            Me.spanExibePor.Visible = True

            Me.divDetalhar.Visible = True
            Me.divDetalharGeral.Visible = False
            If TipoVisao = "Tipo" Then
                CarregaGraficoServicos(tipoFatura)
            Else
                'operadora
                CarregaGraficoOperadora(tipoFatura)
                Me.spanExibePor.Visible = True
            End If

            CarregaGastoUltimoMes()
        ElseIf tipoGrafico = "3" Then
            'ramais
            Me.divDetalhar.Visible = True
            Me.divDetalharGeral.Visible = False
            CarregaGraficoServicosRamais(tipoFatura)
            CarregaGastoUltimoMes()
            Me.spanExibePor.Visible = False
        End If


    End Sub

    Sub CarregaGastoUltimoMes()

        Dim sql As String = " select to_char(nvl(max(f.dt_vencimento),sysdate),'MM/YYYY')vencimento from faturas f "
        sql += " where TRUNC(f.dt_vencimento,'MM')<= TRUNC(add_months(sysdate,0),'MM')  " & vbNewLine
        Dim dt1 As DataTable = _dao_commons.myDataTable(sql)
        ViewState("vencimento") = Replace(dt1.Rows(0).Item("vencimento"), "/", "").ToString

        sql = "select * from (select nvl(sum(gasto),0)gasto,data,1 ordem from(" & sqlTotal & ") where data=to_char(add_months(to_date('" & ViewState("vencimento") & "','mmyyyy'),-1),'MM/YYYY') group by data "
        sql += " union "
        sql += "select sum(gasto)gasto,data, 2 ordem from(" & sqlTotal & ") where data=to_char(add_months(to_date('" & ViewState("vencimento") & "','mmyyyy'),-2),'MM/YYYY') group by data "
        sql += " union "
        sql += "select nvl(sum(gasto),0)gasto,'' data, 3 ordem from (select sum(gasto)gasto,data, 3 ordem from(" & sqlTotal & ")  group by data )"
        sql += " ) order by ordem"
        Dim dt As DataTable = _dao_commons.myDataTable(sql)

        'Response.Write(sql)
        'Response.End()

        If dt.Rows.Count > 0 Then
            'gasto atual
            totalMesAtual = dt.Rows(0).Item("gasto")
            MesAtual = dt.Rows(0).Item("data").ToString
            If MesAtual <> "" Then
                ViewState("vencimento") = Replace(MesAtual, "/", "").ToString
            End If

            If dt.Rows.Count > 1 Then
                VariacaoMesAnterior = dt.Rows(0).Item("gasto") - dt.Rows(1).Item("gasto")
            End If
            If dt.Rows.Count > 2 Then
                total12meses = dt.Rows(2).Item("gasto")
            Else
                total12meses = totalMesAtual
            End If

        End If



    End Sub

    Protected Sub Carrega_Grafico(ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "")
        'Dim _data As String = DALCGestor.MaxUltimaDataFatura()
        Dim _data As String = _dao_commons.myDataTable("select to_char(add_months(sysdate, -1),'DD/MM/YYYY') from dual").Rows(0).Item(0)

        Dim strTipovalor As String = "sum(nvl(p1.valor_cdr,0))gasto"
        'Dim strTipovalor As String = "sum(nvl(p1.total_gasto,p1.valor_cdr))gasto"


        If Not DALCGestor.AcessoAdmin() Then
            strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        End If

        'If tipoValor.ToUpper = "DEVIDO" Then
        '    'strTipovalor = "sum(p1.valor_cdr)-nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) gasto"
        '    'strTipovalor = "sum(p1.valor_cdr) -(((select nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) from dual ))) gasto"
        '    strTipovalor = "sum(p1.valor_cdr)-nvl(p4.valor_contestado,0) gasto"

        'End If

        'monta query
        strSQL = "select  tipo, sum(gasto)gasto, data, codigo_tipo from" & vbNewLine
        strSQL += "(select  tipo, sum(gasto)gasto, data, codigo_tipo from " & vbNewLine
        strSQL += " V_GESTAO_GASTO_CONSOLIDADO2 p1, grupos g"
        strSQL += " where p1.grp_codigo=g.codigo(+) "
        If Not String.IsNullOrEmpty(grupo) Then

            If hierarquia = "1" Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            Else
                strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
            End If
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        strSQL += " and to_date(p1.data, 'MM/YYYY')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and to_date(p1.data,'MM/YYYY')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        strSQL += " group by tipo,data,codigo_tipo"
        strSQL += " "



        If Cliente = "Exibe-Ramal" Then
            strSQL += " union all " & vbNewLine
            strSQL += " select 'ramal' tipo, sum(p1.gasto) gasto ,p1.data,0 codigo_tipo "
            strSQL += " from v_tarifacao p1, grupos g "
            strSQL += " where g.codigo(+) = p1.grupo "
            strSQL += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY')  "
            strSQL += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     " & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            strSQL += " group by p1.data "
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If

            'aparelho
            Dim Sql = ""
            Sql += " union"
            Sql += " select 'ramal' tipo, sum(gasto) gasto, data,0 codigo_tipo"
            Sql += " from(select tarifa, sum(r.custo_ramal) gasto, data"
            Sql += " from (select 'APARELHO' tarifa,p1.data data,p1.ramal "

            Sql += " from v_tarifacao2 p1, grupos g where p1.grupo=g.codigo(+)"

            If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
                Sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                Sql += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                Sql += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                Sql += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If


            If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
                'não filtra o centro de custo dos gerentes
                Sql = Sql + " and exists(" & vbNewLine
                Sql = Sql + "   select 0 from categoria_usuario p100" & vbNewLine
                Sql = Sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                Sql = Sql + "     " & vbNewLine
                Sql = Sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If

            Sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
            Sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-1),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
            Sql += " group by   p1.data,  p1.ramal "
            Sql += "  )p100, ramais r "
            Sql += " where p100.ramal=r.numero_a(+) "
            Sql += "   group by tarifa,  data "
            Sql += " )group by tarifa,  data"

            'custo gestao
            Sql += " union"
            Sql += " select 'ramal' tipo, sum(gasto) gasto, data,0 codigo_tipo"
            Sql += " from(select tarifa, sum(r.custo_servico) gasto, data"
            Sql += " from (select 'CUSTO GESTÃO' tarifa,p1.data data,p1.ramal "

            Sql += " from v_tarifacao p1, grupos g where p1.grupo=g.codigo(+) "

            If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
                Sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                Sql += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                Sql += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                Sql += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If


            If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
                'não filtra o centro de custo dos gerentes
                Sql = Sql + " and exists(" & vbNewLine
                Sql = Sql + "   select 0 from categoria_usuario p100" & vbNewLine
                Sql = Sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                Sql = Sql + "     " & vbNewLine
                Sql = Sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If

            Sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
            Sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-1),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
            Sql += " group by   p1.data,  p1.ramal "
            Sql += "  )p100, ramais r "
            Sql += " where p100.ramal=r.numero_a(+) "
            Sql += "   group by tarifa,  data "
            Sql += " )group by  data"

            strSQL += Sql

        Else

        End If



        'faturas manuais
        'If DALCGestor.AcessoAdmin() Then
        '    strSQL += " union all " & vbNewLine
        '    strSQL += " select UPPER(ft.tipo) tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
        '    strSQL += " from faturas p1, fornecedores p2,  faturas_tipo ft " & vbNewLine
        '    strSQL += " where p1.codigo_tipo=ft.codigo_tipo and not exists(select 0 from faturas_arquivos where codigo_fatura=p1.codigo_fatura) " & vbNewLine
        '    strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
        '    strSQL += " and TRUNC(p1.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        '    strSQL += " and TRUNC(p1.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine&
        '    strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo,ft.tipo " & vbNewLine
        'End If





        strSQL += " ) group by tipo, data, codigo_tipo " & vbNewLine
        strSQL += " order by to_date(data, 'MM/YYYY') asc,codigo_tipo " & vbNewLine

        'Response.Write(strSQL)
        'Response.End()

        sqlTotal = strSQL

        Me.SqlDataSourceResumoGeral.SelectCommand = strSQL
        Me.SqlDataSourceResumoGeral.ConnectionString = Session("conexao")
        Me.SqlDataSourceResumoGeral.DataBind()
        Me.gvResumoMensal.DataBind()
        Meta = DALCGestor.GetMetaByCcusto("")
    End Sub

    Protected Sub Carrega_GraficoOLD(ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "")
        'Dim _data As String = DALCGestor.MaxUltimaDataFatura()
        Dim _data As String = _dao_commons.myDataTable("select to_char(add_months(sysdate, -1),'DD/MM/YYYY') from dual").Rows(0).Item(0)

        Dim strTipovalor As String = "sum(nvl(p1.valor_cdr,0))gasto"
        'Dim strTipovalor As String = "sum(nvl(p1.total_gasto,p1.valor_cdr))gasto"


        If Not DALCGestor.AcessoAdmin() Then
            strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        End If

        'If tipoValor.ToUpper = "DEVIDO" Then
        '    'strTipovalor = "sum(p1.valor_cdr)-nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) gasto"
        '    'strTipovalor = "sum(p1.valor_cdr) -(((select nvl(sum(distinct RetornaUltimaContestacao(p3.codigo_fatura)),0) from dual ))) gasto"
        '    strTipovalor = "sum(p1.valor_cdr)-nvl(p4.valor_contestado,0) gasto"

        'End If

        'monta query
        strSQL = "select  tipo, sum(gasto)gasto, data, codigo_tipo from" & vbNewLine
        strSQL += "(select 'MÓVEL' tipo, " & strTipovalor & " ,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3, grupos g " & vbNewLine
        strSQL += " where p3.codigo_fatura=p2.codigo_fatura and p2.codigo_conta=p1.codigo_conta   " & vbNewLine
        strSQL += " and p3.codigo_tipo in(1) " & vbNewLine
        strSQL += " and p1.grp_codigo=g.codigo(+) "
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then

            If hierarquia = "1" Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            Else
                strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
            End If

        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'tira linha rateada
        If Not DALCGestor.AcessoAdmin() Then
            strSQL = strSQL + "    and not exists"
            strSQL = strSQL + "   (select 0 from rateio_faturas ra, faturas fa"
            strSQL = strSQL + "    where(ra.codigo_fatura = p3.codigo_fatura)"
            strSQL = strSQL + "           and replace(replace(REPLACE(ra.NUM_LINHA(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','') and ra.descricao=p1.tipo_serv2"
            'sql = sql + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & mes & "/" & ano & "')"

            strSQL = strSQL + "           and to_char(fa.dt_vencimento, 'MM/YYYY') = '" & _data & "')"

            'tira as cobranças de franquias
            strSQL = strSQL + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=p2.codigo_fatura and servico=p1.tipo_serv2))"
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If
        If Not DALCGestor.AcessoAdmin() Then
            'rateio para gestores
            strSQL += " union SELECT TIPO, SUM(GASTO)GASTO,DATA,codigo_tipo from (select 'MÓVEL' tipo, r.rateio gasto ,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
            strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3, grupos g,rateiogestao_mv r " & vbNewLine
            strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a = r.rml_numero_a(+) and p1.codigo_conta = r.codigo_conta(+) " & vbNewLine
            strSQL += " and p3.codigo_tipo in(1) " & vbNewLine
            strSQL += " and g.codigo(+) = p1.grp_codigo "
            'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
            strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
            strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
            'filtra o c.custo
            If Not String.IsNullOrEmpty(grupo) Then

                If hierarquia = "1" Then
                    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
                Else
                    strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
                End If

            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     " & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(_excluirServico) Then
                strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,r.rateio,p1.rml_numero_a " & vbNewLine
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If

            strSQL += " ) GROUP BY tipo, DATA,codigo_tipo "
        End If


        'strSQL += " union all " & vbNewLine
        'strSQL += "select 'fixo' tipo, sum(p1.valor_cdr)gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo=2 " & vbNewLine
        'strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= add_months(nvl(to_date('" & _data & "','DD/MM/YYYY'),sysdate),-12) " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(_excluirServico) Then
        '    strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
        If Cliente = "Exibe-Ramal" Then
            strSQL += " union all " & vbNewLine
            strSQL += " select 'ramal' tipo, sum(p1.gasto)gasto ,p1.data,0 codigo_tipo "
            strSQL += " from v_tarifacao p1, grupos g "
            strSQL += " where g.codigo(+) = p1.grupo "
            strSQL += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY')  "
            strSQL += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     " & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
            strSQL += " group by p1.data "
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If

        Else

        End If
        strSQL += " union all " & vbNewLine
        strSQL += "select 'FIXO' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo in(2) " & vbNewLine
        strSQL += " and  p1.grp_codigo = g.codigo(+)"
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(_excluirServico) Then
            strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If

        '### COMENTADO EM 19/04/2017 - VAMOS MANTER SOMENTE MOVEL E FIXO
        'strSQL += " union all " & vbNewLine
        'strSQL += "select '0800' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo=4 " & vbNewLine
        'strSQL += " and g.codigo(+) = p1.grp_codigo "
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " and g.area='" & area & "'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        'End If
        ''verifica nível de acesso
        'If Not DALCGestor.AcessoAdmin() Then
        '    'não filtra o centro de custo dos gerentes
        '    strSQL = strSQL + " and exists(" & vbNewLine
        '    strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
        '    strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    'strSQL = strSQL + "     " & vbNewLine
        '    strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(_excluirServico) Then
        '    strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " ,g.area"
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " ,g.area_interna"
        'End If

        'strSQL += " union all " & vbNewLine
        'strSQL += "select 'Número Único' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo=6 " & vbNewLine
        'strSQL += " and g.codigo(+) = p1.grp_codigo "
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " and g.area='" & area & "'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        'End If
        ''verifica nível de acesso
        'If Not DALCGestor.AcessoAdmin() Then
        '    'não filtra o centro de custo dos gerentes
        '    strSQL = strSQL + " and exists(" & vbNewLine
        '    strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
        '    strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    'strSQL = strSQL + "     " & vbNewLine
        '    strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " ,g.area"
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " ,g.area_interna"
        'End If
        ''LINK de Dados - ENtra como Serviço

        'strSQL += " union all " & vbNewLine
        'strSQL += "select 'LINK DE DADOS' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        'strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g " & vbNewLine
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        'strSQL += " and p3.codigo_tipo in(5) " & vbNewLine
        'strSQL += " and g.codigo(+) = p1.grp_codigo "
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        ''filtra o c.custo
        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " and g.area='" & area & "'" & vbNewLine
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        'End If
        ''verifica nível de acesso
        'If Not DALCGestor.AcessoAdmin() Then
        '    'não filtra o centro de custo dos gerentes
        '    strSQL = strSQL + " and exists(" & vbNewLine
        '    strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
        '    strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    'strSQL = strSQL + "     " & vbNewLine
        '    strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If

        'If Not String.IsNullOrEmpty(grupo) Then
        '    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        'End If
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo,p4.valor_contestado " & vbNewLine
        'If Not String.IsNullOrEmpty(area) Then
        '    strSQL += " ,g.area"
        'End If
        'If Not String.IsNullOrEmpty(area_interna) Then
        '    strSQL += " ,g.area_interna"
        'End If

        'SERVIÇOS
        strSQL += " union all " & vbNewLine
        strSQL += "select 'SERVIÇOS' tipo, " & strTipovalor & ",to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
        strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3, grupos g " & vbNewLine
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura " & vbNewLine
        strSQL += " and p3.codigo_tipo in(3) " & vbNewLine
        strSQL += " and g.codigo(+) = p1.grp_codigo "
        'strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        'filtra o c.custo
        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " and g.area='" & area & "'" & vbNewLine
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " and g.area_interna='" & area_interna & "'" & vbNewLine
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If Not String.IsNullOrEmpty(grupo) Then
            strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        End If
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),p3.codigo_tipo  " & vbNewLine
        If Not String.IsNullOrEmpty(area) Then
            strSQL += " ,g.area"
        End If
        If Not String.IsNullOrEmpty(area_interna) Then
            strSQL += " ,g.area_interna"
        End If


        'faturas manuais
        If DALCGestor.AcessoAdmin() Then
            strSQL += " union all " & vbNewLine
            strSQL += " select UPPER(ft.tipo) tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
            strSQL += " from faturas p1, fornecedores p2,  faturas_tipo ft " & vbNewLine
            strSQL += " where p1.codigo_tipo=ft.codigo_tipo and not exists(select 0 from faturas_arquivos where codigo_fatura=p1.codigo_fatura) " & vbNewLine
            strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
            strSQL += " and TRUNC(p1.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
            strSQL += " and TRUNC(p1.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine&
            strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo,ft.tipo " & vbNewLine
        End If



        'PARCELAS E CUSTO FIXO
        strSQL += " union "
        strSQL += " select 'MÓVEL' tarifa,sum(gasto)gasto,data, 1 codigo_tipo from (select P1.RML_NUMERO_A, max(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then nvl(pa.parcela,0) else 0 end) + max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        strSQL += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        'strSQL += "  and p1.codigo_usuario=pa.codigo_usuario and p1.rml_numero_a=pa.num_linha"
        'strSQL += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        strSQL += "   and p1.rml_numero_a=pa.num_linha(+)"
        strSQL += " and p1.tarif_codigo = p4.codigo(+)"
        'strSQL += " and  p1.cdr_codigo <>'3' "
        strSQL += " and p1.grp_codigo=p5.codigo(+)"
        strSQL += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            strSQL += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            strSQL += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If

        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                strSQL += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                strSQL += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                strSQL += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        'strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        strSQL += " )   group by data"


        'CUSTO FIXO
        'strSQL += " union "
        'strSQL += " select 'MÓVEL' tarifa,sum(gasto)gasto,data, 1 codigo_tipo from (select P1.RML_NUMERO_A, max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        'strSQL += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        'strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        ''strSQL += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        'strSQL += "   and p1.rml_numero_a=pa.num_linha"
        'strSQL += " and p1.tarif_codigo = p4.codigo(+)"
        ''strSQL += " and  p1.cdr_codigo <>'3' "
        'strSQL += " and p1.grp_codigo=p5.codigo(+)"
        'strSQL += " and p3.codigo_tipo='" & tipoFatura & "'"
        'If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
        '    strSQL += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        'End If
        'If ViewState("codServico") <> "" Then
        '    strSQL += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        'End If

        ''If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        'If AppIni.GloboRJ_Parm = True Then
        '    If area <> "" Then
        '        strSQL += " and p5.area = '" & area & "'"
        '    End If
        '    If area_interna <> "" Then
        '        strSQL += " and p5.area_interna = '" & area_interna & "'"
        '    End If
        '    If grupo <> "" Then
        '        strSQL += " and p5.codigo like '" & grupo & "%'"
        '    End If
        'Else
        '    If Not String.IsNullOrEmpty(grupo) Then
        '        strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
        '    End If
        'End If

        'If Not DALCGestor.AcessoAdmin() Then
        '    'não filtra o centro de custo dos gerentes
        '    strSQL = strSQL + " and exists(" & vbNewLine
        '    strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
        '    strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    'strSQL = strSQL + "     " & vbNewLine
        '    strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If
        ''strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        ''strSQL += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        'strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        'strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        'strSQL += " )   group by data"

        strSQL += " ) group by tipo, data, codigo_tipo " & vbNewLine
        strSQL += " order by to_date(data, 'MM/YYYY') asc,codigo_tipo " & vbNewLine

        'Response.Write(strSQL)
        'Response.End()

        sqlTotal = strSQL

        Me.SqlDataSourceResumoGeral.SelectCommand = strSQL
        Me.SqlDataSourceResumoGeral.ConnectionString = Session("conexao")
        Me.SqlDataSourceResumoGeral.DataBind()
        Me.gvResumoMensal.DataBind()
        Meta = DALCGestor.GetMetaByCcusto("")
    End Sub

    Private Sub main_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If tipoGrafico = "1" Then
            InverteGridView(Me.gvResumoMensal)
        End If

    End Sub
    Public Sub InverteGridView(ByVal mygrid As GridView)
        Dim jsserialize As New JavaScriptSerializer

        Dim dt As New DataTable
        Dim i As Integer = 1

        'totais
        'Dim total(12) As Double


        'cria as colunas de acordo com as linhas
        dt.Columns.Add(" ")

        For Each linha As GridViewRow In mygrid.Rows
            Try
                Dim nomeColuna As String = MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)

                dt.Columns.Add(nomeColuna)
                dt.Columns(nomeColuna).DefaultValue = "R$ 0,00"


            Catch ex As Exception
            End Try

        Next

        Dim myRow As DataRow = dt.NewRow
        Dim myRowFixo As DataRow = dt.NewRow
        Dim myRowServicos As DataRow = dt.NewRow
        Dim myRow0800 As DataRow = dt.NewRow
        Dim myRow4004 As DataRow = dt.NewRow
        Dim myRowDADOS As DataRow = dt.NewRow
        Dim myRowRamais As DataRow = dt.NewRow
        Dim myRowTotais As DataRow = dt.NewRow
        'coloca os valores das colunas
        myRow.Item(0) = "Móveis"
        myRowFixo.Item(0) = "Fixos"
        myRowServicos.Item(0) = "Serviços"
        myRow0800.Item(0) = "0800"
        myRow4004.Item(0) = "Núm. Único"
        myRowDADOS.Item(0) = "Link de Dados"
        myRowRamais.Item(0) = "Ramais"
        myRowTotais.Item(0) = "Total"

        Dim TotalTipos As Integer = 0
        For Each linha As GridViewRow In mygrid.Rows
            Dim valor As String = linha.Cells(1).Text.Replace(" ", "")
            If linha.Cells(2).Text = 1 Then
                'movel
                myRow.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                    ExibeMovel = True

                End If
            ElseIf linha.Cells(2).Text = 2 Then
                'fixo
                myRowFixo.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    ExibeFixo = True
                End If
            ElseIf linha.Cells(2).Text = 4 Then
                '0800
                myRow0800.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                ' myRow.Item(linha.Cells(0).Text) = 0
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    Exibe0800 = True
                End If
            ElseIf linha.Cells(2).Text = 6 Then
                '0800
                myRow4004.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    Exibe3003 = True
                End If
                'ElseIf linha.Cells(2).Text = 3 Then
                '    'servicos
                '    myRowServicos.Item(linha.Cells(0).Text) = valor

                '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                '        ExibeServico = True

                '    End If
                'ElseIf linha.Cells(2).Text = 5 Then
                '    'servicos
                '    myRowServicos.Item(linha.Cells(0).Text) = valor

                '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
                '        ExibeServico = True

                '    End If
            ElseIf linha.Cells(2).Text = 5 Then
                'LINK de DADOS
                myRowDADOS.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    exibeDados = True
                End If
            ElseIf linha.Cells(2).Text = 0 Then
                'RAMAIS
                myRowRamais.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor
                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    exibeRamail = True

                End If
            Else
                'serviços
                myRowServicos.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = valor

                If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then

                    ExibeServico = True

                End If
            End If

            Try
                'coloca os totais
                myRowTotais.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = Convert.ToDecimal(myRowTotais.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)).ToString.Trim.Replace("R$", "")) + valor
                myRowTotais.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2)) = FormatCurrency(myRowTotais.Item(MonthName(linha.Cells(0).Text.Substring(0, 2), True) & linha.Cells(0).Text.Substring(2))).ToString.Trim.Replace("R$ ", "R$")

            Catch ex As Exception

            End Try

            i += 1
        Next

        dt.Rows.Add(myRow)
        dt.Rows.Add(myRowFixo)
        dt.Rows.Add(myRow0800)
        dt.Rows.Add(myRow4004)
        dt.Rows.Add(myRowServicos)
        dt.Rows.Add(myRowDADOS)
        dt.Rows.Add(myRowRamais)
        dt.Rows.Add(myRowTotais)
        Me.gvResumoGeral.DataSource = dt
        Me.gvResumoGeral.DataBind()
        Me.gvResumoGeral.Rows(6).Style.Add("font-weight", "bold")
        ' Me.gvResumoGeral.Rows(6).Style.Add("background-color", "#D8D9DE")
        Me.gvResumoMensal.Visible = False

        'coloca a legenda dos graficos

        For i = 1 To dt.Columns.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dt.Columns(i).ColumnName & "'"
            GraficoLabel += ","

            'coloca os valores
            'movel
            If ExibeMovel Then
                GraficoData += "" & dt.Rows(0).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
                'total12meses += dt.Rows(0).Item(i)
            Else
                gvResumoGeral.Rows(0).Visible = False
            End If


            'GraficoDataDouble.Add(dt.Rows(0).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", "."))
            'fixo
            If ExibeFixo Then
                GraficoData2 += dt.Rows(1).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(1).Visible = False
                gvResumoGeral.Rows(5).Visible = False
            End If

            '0800
            If Exibe0800 Then
                GraficoData3 += dt.Rows(2).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(2).Visible = False
            End If

            '4004
            If Exibe3003 Then
                GraficoData4 += dt.Rows(3).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(3).Visible = False
            End If

            'serviços
            If ExibeServico Then
                GraficoData5 += dt.Rows(4).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(4).Visible = False
            End If

            'link de dados
            If exibeDados Then
                GraficoData6 += dt.Rows(5).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(5).Visible = False
            End If

            'ramais
            If exibeRamail Then
                GraficoData7 += dt.Rows(6).Item(i).Replace(".", "").Replace("R$", "").Replace(",", ".") & ","
            Else
                gvResumoGeral.Rows(6).Visible = False
            End If

            'coloca o link para o RIT
            'dt.Columns(i).ColumnName = Context.Server.HtmlDecode("<a href='#'>" + dt.Columns(i).ColumnName.ToString + "</a>")

        Next

        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

        If GraficoData2 <> "" Then
            If GraficoData2.Substring(GraficoData2.Length - 1, 1) = "," Then
                GraficoData2 = GraficoData2.Substring(0, GraficoData2.Length - 1)
            End If
        End If


        If GraficoData3 <> "" Then
            If GraficoData3.Substring(GraficoData3.Length - 1, 1) = "," Then
                GraficoData3 = GraficoData3.Substring(0, GraficoData3.Length - 1)
            End If
        End If
        If GraficoData4 <> "" Then
            If GraficoData4.Substring(GraficoData4.Length - 1, 1) = "," Then
                GraficoData4 = GraficoData4.Substring(0, GraficoData4.Length - 1)
            End If
        End If

        If GraficoData5 <> "" Then
            If GraficoData5.Substring(GraficoData5.Length - 1, 1) = "," Then
                GraficoData5 = GraficoData5.Substring(0, GraficoData5.Length - 1)
            End If
        End If

        If GraficoData6 <> "" Then
            If GraficoData6.Substring(GraficoData6.Length - 1, 1) = "," Then
                GraficoData6 = GraficoData6.Substring(0, GraficoData6.Length - 1)
            End If
        End If
        If GraficoData7 <> "" Then
            If GraficoData7.Substring(GraficoData7.Length - 1, 1) = "," Then
                GraficoData7 = GraficoData7.Substring(0, GraficoData7.Length - 1)
            End If
        End If

        If ExibeMovel Then
            TotalTipos += 1
        End If
        If ExibeFixo Then
            TotalTipos += 1
        End If
        If exibeDados Then
            TotalTipos += 1
        End If
        If Exibe0800 Then
            TotalTipos += 1
        End If
        If Exibe3003 Then
            TotalTipos += 1
        End If
        If exibeRamail Then
            TotalTipos += 1
        End If
        If ExibeServico Then
            TotalTipos += 1
        End If


        If TotalTipos > 1 Then
            virgulaGrafico = ","
        End If

        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "CarregaGrafico();", True)

        'Dim Script As String = "dados=500"
        'ScriptManager.RegisterStartupScript(Me.upMain, Me.upMain.GetType(), "openWindow", Script, True)
        'GraficoData = jsserialize.Serialize(GraficoData)
        'GraficoData2 = jsserialize.Serialize(GraficoData2)
        'Dim Script As String = "CarregaGrafico();"
        'ScriptManager.RegisterStartupScript(Me.upGrafico, Me.upGrafico.GetType(), "openWindow", Script, True)
        'ScriptManager.GetCurrent(Me).RegisterPostBackControl(Me.cmbCentral)

        'Me.upMain.
        'Me.gvResumoGeral.DataSource = dt
        'Me.gvResumoGeral.DataBind()
    End Sub
    Protected Sub gvResumoGeral_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResumoGeral.RowDataBound
        For Each cell As TableCell In e.Row.Cells
            If e.Row.RowType = DataControlRowType.Header Then
                If Not String.IsNullOrEmpty(cell.Text.Trim) Then
                    'cell.Text = Server.HtmlDecode("<a href='Rit.aspx?competencia='" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & " '>" & cell.Text & "</a>")
                    'cell.Text = Server.HtmlDecode("<a href=""RIT.aspx?competencia=" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & "&grupo=" & Request.QueryString("grupo") & """>" & cell.Text & "</a>")
                End If
            End If

            If e.Row.Cells.GetCellIndex(cell) > 0 Then
                cell.HorizontalAlign = HorizontalAlign.Right
            End If
            Dim nomeServico As String = ""
            If e.Row.RowType = DataControlRowType.DataRow Then
                Dim codigoTipo As String = ""
                If Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS MÓVEIS" Then
                    codigoTipo = 1
                    nomeServico = " - Linhas Móveis"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS FIXAS" Then
                    codigoTipo = 2
                    nomeServico = " - Linhas Fixas"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS 0800" Then
                    codigoTipo = 4
                    nomeServico = " - Linhas 0800"
                ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "NÚMERO ÚNICO" Then
                    codigoTipo = 6
                    nomeServico = " - Número Único"
                End If

                'e.Row.Cells(0).Text = "<b><a href='graficoOperServico.aspx?codigoTipo=" & codigoTipo & "&nomeServico=" & nomeServico & "'>" & e.Row.Cells(0).Text & "</a></b>"
            End If

        Next
    End Sub



    Function getQueryTotalServico(tipoFatura As String, ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "") As String

        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        'Dim strTipovalor As String = " sum(nvl(p1.valor_cdr,0))gasto "
        'If Not DALCGestor.AcessoAdmin() Then
        '    strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        'End If


        strSQL = "select  tarifa , sum(gasto)gasto, data from" & vbNewLine
        strSQL += "(select  tarifa, sum(gasto)gasto, data from " & vbNewLine
        strSQL += " V_GESTAO_GASTO_CONSOLIDADO2 p1, grupos g"
        strSQL += " where p1.grp_codigo=g.codigo(+) "
        strSQL += " and p1.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            strSQL += " and p1.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            strSQL += " and p1.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                strSQL += " and g.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                strSQL += " and g.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                strSQL += " and g.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If
        'verifica nível de acesso
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSQL = strSQL + " and exists(" & vbNewLine
            strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
            strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'strSQL = strSQL + "     " & vbNewLine
            strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        strSQL += " and to_date(p1.data, 'MM/YYYY')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        strSQL += " and to_date(p1.data,'MM/YYYY')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        strSQL += " group by tarifa,data"
        strSQL += " "


        ''faturas manuais
        'If DALCGestor.AcessoAdmin() Then
        '    strSQL += " union all " & vbNewLine
        '    strSQL += " select UPPER(ft.tipo) tipo, sum(p1.valor)gasto,to_char(p1.dt_vencimento, 'MM/YYYY')data,p1.codigo_tipo " & vbNewLine
        '    strSQL += " from faturas p1, fornecedores p2,  faturas_tipo ft " & vbNewLine
        '    strSQL += " where p1.codigo_tipo=ft.codigo_tipo and not exists(select 0 from faturas_arquivos where codigo_fatura=p1.codigo_fatura) " & vbNewLine
        '    strSQL += " and p1.codigo_fornecedor=p2.codigo" & vbNewLine
        '    strSQL += " and TRUNC(p1.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        '    strSQL += " and TRUNC(p1.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine&
        '    strSQL += " group by to_char(p1.dt_vencimento, 'MM/YYYY'),p1.codigo_tipo,ft.tipo " & vbNewLine
        'End If





        strSQL += " ) group by tarifa, data " & vbNewLine
        strSQL += " order by to_date(data, 'MM/YYYY') asc " & vbNewLine

        'Response.Write(strSQL)
        'Response.End()

        sqlTotal = strSQL


        Return strSQL
    End Function

    Function getQueryTotalServicoOld(tipoFatura As String, ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "") As String

        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim strTipovalor As String = " sum(nvl(p1.valor_cdr,0))gasto "
        If Not DALCGestor.AcessoAdmin() Then
            strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        End If


        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select 'VOZ' tarifa, " + strTipovalor + ",to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and p1.cdr_codigo='3'"
        sql += " and p1.grp_codigo=p5.codigo(+)"
        sql += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine

        If Not DALCGestor.AcessoAdmin() Then
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
        End If
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY') "

        If Not DALCGestor.AcessoAdmin() Then
            'rateio para gestores
            strSQL += " union SELECT TARIFA, SUM(GASTO)gasto,DATA from (select 'VOZ' tarifa, r.rateio gasto ,to_char(p3.dt_vencimento, 'MM/YYYY')data " & vbNewLine
            strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3, grupos g,rateiogestao_mv r " & vbNewLine
            strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a = r.rml_numero_a and p1.codigo_conta = r.codigo_conta " & vbNewLine
            'strSQL += " and p3.codigo_tipo=1 " & vbNewLine
            strSQL += " and p3.codigo_tipo in(1) " & vbNewLine
            strSQL += " and g.codigo(+) = p1.grp_codigo "
            strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
            strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
            'filtra o c.custo
            If Not String.IsNullOrEmpty(grupo) Then

                If hierarquia = "1" Then
                    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
                Else
                    strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
                End If

            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     " & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(_excluirServico) Then
                strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'), R.RATEIO,P1.RML_NUMERO_A)GROUP BY TARIFA, DATA " & vbNewLine

            sql += strSQL
        End If

        sql += " union "
        sql += " select replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS') tarifa, " + strTipovalor + ",to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"
        sql += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If

        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If
        If Not DALCGestor.AcessoAdmin() Then
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
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " And exists(" & vbNewLine
            sql = sql + "   Select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS') "


        'PARCELAS
        sql += " union "
        sql += " select 'APARELHO' tarifa,sum(gasto)gasto,data from (select P1.RML_NUMERO_A, max(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then nvl(pa.parcela,0) else 0 end) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 ,  grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        'sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        sql += " "
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"
        sql += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If

        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine

        sql += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine

        ' sql += " and to_char(p3.dt_vencimento, 'MMYYYY')='072016'"
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        sql += " )   group by data"


        'CUSTO FIXO
        sql += " union "
        sql += " select 'SERVIÇOS' tarifa,sum(gasto)gasto,data from (select P1.RML_NUMERO_A, max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        sql += " "
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"
        sql += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If

        'If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        'sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
        sql += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        sql += " )   group by data"

        sql += ") group by tarifa,data order by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        'Response.End()


        sqlTotal = sql

        Return sql
    End Function

    Function getQueryTotalServicoRamais(tipoFatura As String, ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "") As String

        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim sql As String = ""
        sql += " select tarifa, nvl(sum(gasto),0)gasto, data from ("


        sql += " select p1.tarifa tarifa,"
        'sql += " sum(p1.valor_cdr)gasto"
        'sql += " sum(nvl(p1.total_gasto,p1.valor_cdr)) + case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') then nvl(pa.parcela,0) else 0 end +nvl(op.custo_fixo,0)  gasto "
        sql += " nvl(sum(p1.gasto),0)  gasto "

        'sql = sql + " sum(nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0)) gasto"

        sql += ",p1.data data  "
        'sql += " from v_tarifacao p1, grupos p5 where p1.grupo=p5.codigo(+) and p1.gasto>0 "
        'sql += " from v_tarifacao p1, grupos p5 where p1.grupo=p5.codigo(+) and p1.gasto_total>0 "
        sql += " from v_tarifacao2 p1, grupos p5 where p1.grupo=p5.codigo(+) "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
        End If


        If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-1),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by  p1.tarifa, p1.data "

        'aparelho
        sql += " union"
        sql += " select tarifa, sum(gasto) gasto, data"
        sql += " from(select tarifa, sum(p100.custo_ramal) gasto, data"
        sql += " from (select 'APARELHO' tarifa,p1.data data,p1.ramal, p1.custo_ramal "

        sql += " from v_tarifacao2 p1, grupos p5 where p1.grupo=p5.codigo(+) "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-1),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by   p1.data,  p1.ramal,p1.custo_ramal "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "




        sql += "   group by tarifa,  data "
        sql += " )group by tarifa,  data"

        'custo gestao

        sql += " union"
        sql += " select tarifa, sum(gasto) gasto, data"
        sql += " from(select tarifa, sum(p100.custo_servico) gasto, data"
        sql += " from (select 'CUSTO GESTÃO' tarifa,p1.data data,p1.ramal, p1.custo_servico "

        sql += " from v_tarifacao2 p1, grupos p5 where p1.grupo=p5.codigo(+) "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If AppIni.GloboRJ_Parm = True Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grupo like '" & grupo & "%'" & vbNewLine
            End If
        End If


        If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-1),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by  p1.data,  p1.ramal,p1.custo_servico "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        'sql += "   group by tarifa,  data,r.custo_servico "
        sql += "   group by tarifa,  data "
        sql += " )group by tarifa,  data"


        sql += ") group by tarifa,data  order by to_date(data,'MM/YYYY') "


        'Response.Write(sql)
        'Response.End()

        sqlTotal = sql

        Return sql
    End Function

    Sub CarregaGraficoServicos(tipoFatura As String)
        GraficoLabel = ""
        GraficoData = ""
        Dim temDesconto As Boolean = False

        'Dim strlegenda As String = "select distinct data from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by to_date(data,'MM/YYYY') asc "

        'Dim dt As DataTable = _dao_commons.myDataTable(strlegenda)

        Dim dt As DataTable = _dao_commons.myDataTable(getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna))
        Dim _viewCat As DataView = New DataView(dt)
        Dim dtCAT As DataTable = _viewCat.ToTable(True, "data")

        Dim categorias As New List(Of String)
        Dim dtNew As New DataTable

        Dim _column As New DataColumn("Tipo")
        dtNew.Columns.Add(_column)


        Dim i As Integer = 0
        For i = 0 To dtCAT.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dtCAT.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dtCAT.Rows(i).Item(0))


            'adiona a linha do datatable
            _column = New DataColumn(dtCAT.Rows(i).Item(0))
            _column.DefaultValue = 0
            dtNew.Columns.Add(_column)

            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        'agora vamos montar as series
        'Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by tarifa asc "

        'Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ")  "
        'dt = _dao_commons.myDataTable(strNomeSerie)

        Dim _view As DataView = New DataView(dt)
        _view.Sort = "tarifa ASC"
        Dim dtSeries As DataTable = _view.ToTable(True, "tarifa")

        Dim _series As New List(Of Serie)

        For i = 0 To dtSeries.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = dtSeries.Rows(i).Item(0)
            _serie.Data = "["
            _series.Add(_serie)
        Next

        Dim maxvalue As Decimal = 0
        'Dim dtTotal As DataTable = _dao_commons.myDataTable(getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna))
        Dim dtTotal As DataTable = dt

        Dim totalRow As DataRow = dtNew.NewRow
        For Each _item As Serie In _series
            Dim _row As DataRow = dtNew.NewRow
            _row.Item(0) = _item.Nome
            dtNew.Rows.Add(_row)

            For Each _categoria As String In categorias
                'dt = _dao_commons.myDataTable("select * from (" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") where data='" & _categoria & "' and tarifa='" & _item.Nome & "' order by data,tarifa")
                Dim _valor As Double = GetValorServico(dtTotal, _item.Nome, _categoria)
                '
                'If _valor > 0 Then

                'achou a tarifa
                '_item.Data = _item.Data & dt.Rows(0).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                _item.Data = _item.Data & _valor.ToString.Replace(".", "").Replace(",", ".") & ","
                    'totalRow.Item(_categoria) = FormatCurrency(totalRow.Item(_categoria) + dt.Rows(0).Item(1)).Replace(" ", "")
                    totalRow.Item(_categoria) = FormatCurrency(totalRow.Item(_categoria) + _valor).Replace(" ", "")
                    '_row.Item(_categoria) = FormatCurrency(dt.Rows(0).Item(1)).Replace(" ", "")
                    _row.Item(_categoria) = FormatCurrency(_valor).Replace(" ", "")
                    'total12meses += dt.Rows(0).Item(1)
                    'If dt.Rows(0).Item(1) < 0 Then
                    If _valor < 0 Then
                        temDesconto = True
                    End If

                    'If negativeValue > Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                    If negativeValue > Convert.ToDouble(_valor.ToString) Then
                        'negativeValue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                        negativeValue = Convert.ToDouble(_valor.ToString)
                    End If

                    ' If maxvalue < Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                    If maxvalue < Convert.ToDouble(_valor.ToString) Then
                        'maxvalue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                        maxvalue = Convert.ToDouble(_valor.ToString)
                    End If

                'Else
                '    _row.Item(_categoria) = FormatCurrency(0)
                '    _item.Data = _item.Data & 0 & ","
                'End If
            Next

            'For i = 0 To dt.Rows.Count - 1
            '    If dt.Rows(i).Item(0) = _item.Nome Then
            '        'adiciona na serie
            '        _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
            '    End If
            'Next
        Next

        If negativeValue <> 0 Then
            negativeValue = negativeValue - maxvalue / 10
        End If

        'retiramos a ultima virgula e fechamos a TAG
        For Each _item As Serie In _series
            _item.Data = _item.Data.Substring(0, _item.Data.Length - 1)
            _item.Data = _item.Data & "]"

            GraficoData += "{name: '" & _item.Nome & " ',data:" & _item.Data & "},"

        Next


        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

        For Each _coluna As DataColumn In dtNew.Columns
            If Not _coluna.ColumnName.ToUpper = "TIPO" Then
                _coluna.ColumnName = MonthName(_coluna.ColumnName.Substring(0, 2), True) & _coluna.ColumnName.Substring(2)
            End If

        Next
        totalRow.Item(0) = "Total"
        dtNew.Rows.Add(totalRow)

        Me.gvServicos.DataSource = dtNew
        Me.gvServicos.DataBind()
        Me.gvServicos.Rows(gvServicos.Rows.Count - 1).Font.Bold = True
        Me.gvServicos.Visible = True

        If dtNew.Rows.Count < 1 Then
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "EscondeGrid();", True)
        End If

    End Sub

    Sub CarregaGraficoServicosRamais(tipoFatura As String)
        GraficoLabel = ""
        GraficoData = ""
        Dim temDesconto As Boolean = False

        'Dim strlegenda As String = "select distinct data from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by to_date(data,'MM/YYYY') asc "

        'Dim dt As DataTable = _dao_commons.myDataTable(strlegenda)

        Dim dt As DataTable = _dao_commons.myDataTable(getQueryTotalServicoRamais(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna))
        Dim _viewCat As DataView = New DataView(dt)
        Dim dtCAT As DataTable = _viewCat.ToTable(True, "data")

        Dim categorias As New List(Of String)
        Dim dtNew As New DataTable

        Dim _column As New DataColumn("Tipo")
        dtNew.Columns.Add(_column)


        Dim i As Integer = 0
        For i = 0 To dtCAT.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dtCAT.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dtCAT.Rows(i).Item(0))


            'adiona a linha do datatable
            _column = New DataColumn(dtCAT.Rows(i).Item(0))
            _column.DefaultValue = 0
            dtNew.Columns.Add(_column)

            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        'agora vamos montar as series
        'Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by tarifa asc "

        'Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ")  "
        'dt = _dao_commons.myDataTable(strNomeSerie)

        Dim _view As DataView = New DataView(dt)
        _view.Sort = "tarifa ASC"
        Dim dtSeries As DataTable = _view.ToTable(True, "tarifa")

        Dim _series As New List(Of Serie)

        For i = 0 To dtSeries.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = dtSeries.Rows(i).Item(0)
            _serie.Data = "["
            _series.Add(_serie)
        Next

        Dim maxvalue As Decimal = 0
        'Dim dtTotal As DataTable = _dao_commons.myDataTable(getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna))
        Dim dtTotal As DataTable = dt

        Dim totalRow As DataRow = dtNew.NewRow
        For Each _item As Serie In _series
            Dim _row As DataRow = dtNew.NewRow
            _row.Item(0) = _item.Nome
            dtNew.Rows.Add(_row)

            For Each _categoria As String In categorias
                'dt = _dao_commons.myDataTable("select * from (" & getQueryTotalServico(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") where data='" & _categoria & "' and tarifa='" & _item.Nome & "' order by data,tarifa")
                Dim _valor As Double = GetValorServico(dtTotal, _item.Nome, _categoria)
                '
                If _valor > 0 Then

                    'achou a tarifa
                    '_item.Data = _item.Data & dt.Rows(0).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                    _item.Data = _item.Data & _valor.ToString.Replace(".", "").Replace(",", ".") & ","
                    'totalRow.Item(_categoria) = FormatCurrency(totalRow.Item(_categoria) + dt.Rows(0).Item(1)).Replace(" ", "")
                    totalRow.Item(_categoria) = FormatCurrency(totalRow.Item(_categoria) + _valor).Replace(" ", "")
                    '_row.Item(_categoria) = FormatCurrency(dt.Rows(0).Item(1)).Replace(" ", "")
                    _row.Item(_categoria) = FormatCurrency(_valor).Replace(" ", "")
                    'total12meses += dt.Rows(0).Item(1)
                    'If dt.Rows(0).Item(1) < 0 Then
                    If _valor < 0 Then
                        temDesconto = True
                    End If

                    'If negativeValue > Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                    If negativeValue > Convert.ToDouble(_valor.ToString) Then
                        'negativeValue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                        negativeValue = Convert.ToDouble(_valor.ToString)
                    End If

                    ' If maxvalue < Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                    If maxvalue < Convert.ToDouble(_valor.ToString) Then
                        'maxvalue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                        maxvalue = Convert.ToDouble(_valor.ToString)
                    End If

                Else
                    _row.Item(_categoria) = FormatCurrency(0)
                    _item.Data = _item.Data & 0 & ","
                End If
            Next

            'For i = 0 To dt.Rows.Count - 1
            '    If dt.Rows(i).Item(0) = _item.Nome Then
            '        'adiciona na serie
            '        _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
            '    End If
            'Next
        Next

        If negativeValue <> 0 Then
            negativeValue = negativeValue - maxvalue / 10
        End If

        'retiramos a ultima virgula e fechamos a TAG
        For Each _item As Serie In _series
            _item.Data = _item.Data.Substring(0, _item.Data.Length - 1)
            _item.Data = _item.Data & "]"

            GraficoData += "{name: '" & _item.Nome & " ',data:" & _item.Data & "},"

        Next


        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

        For Each _coluna As DataColumn In dtNew.Columns
            If Not _coluna.ColumnName.ToUpper = "TIPO" Then
                _coluna.ColumnName = MonthName(_coluna.ColumnName.Substring(0, 2), True) & _coluna.ColumnName.Substring(2)
            End If

        Next
        totalRow.Item(0) = "Total"
        dtNew.Rows.Add(totalRow)

        Me.gvServicos.DataSource = dtNew
        Me.gvServicos.DataBind()
        Me.gvServicos.Rows(gvServicos.Rows.Count - 1).Font.Bold = True
        Me.gvServicos.Visible = True

        If dtNew.Rows.Count < 1 Then
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "EscondeGrid();", True)
        End If

    End Sub

    Function GetValorServico(dt As DataTable, servico As String, data As String) As String

        For Each _item As DataRow In dt.Rows

            If _item(0) = servico And _item(2) = data Then

                Return _item(1)
            End If


        Next
        Return 0

    End Function


    Function getQueryTotalOperadora(tipoFatura As String, ByVal grupo As String, ByVal hierarquia As String, ByVal Cliente As String, ByVal tipoValor As String, Optional area As String = "", Optional area_interna As String = "") As String
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim sql As String = ""
        sql += " select operadora,sum(gasto)gasto,data from ("
        sql += " select p4.descricao operadora, sum(p1.valor_cdr)gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4, grupos p5 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p3.codigo_operadora=p4.codigo(+)"
        sql += " and p1.grp_codigo=p5.codigo(+)"
        sql += " and p3.codigo_tipo='" & tipoFatura & "'"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If

        If AppIni.GloboRJ_Parm = True And DALCGestor.AcessoAdmin() Then
            If area <> "" Then
                sql += " and p5.area = '" & area & "'"
            End If
            If area_interna <> "" Then
                sql += " and p5.area_interna = '" & area_interna & "'"
            End If
            If grupo <> "" Then
                sql += " and p5.codigo like '" & grupo & "%'"
            End If
        Else
            If Not String.IsNullOrEmpty(grupo) Then
                sql += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),  p4.descricao "
        'sql += " order  by to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')"

        If Not DALCGestor.AcessoAdmin() Then
            'rateio para gestores
            strSQL += " union SELECT operadora, SUM(GASTO)GASTO,DATA from (select op.descricao operadora, r.rateio gasto ,to_char(p3.dt_vencimento, 'MM/YYYY')data,p3.codigo_tipo " & vbNewLine
            strSQL += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3,vContestacoesFaturas p4, grupos g,rateiogestao_mv r, operadoras_teste op " & vbNewLine
            strSQL += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a = r.rml_numero_a(+) and p1.codigo_conta = r.codigo_conta(+) and p3.codigo_operadora=op.codigo(+) " & vbNewLine
            strSQL += " and p3.codigo_tipo=1 " & vbNewLine
            strSQL += " and g.codigo(+) = p1.grp_codigo "
            strSQL += " and p3.codigo_fatura=p4.codigo_fatura(+) " & vbNewLine
            strSQL += " and TRUNC(p3.dt_vencimento, 'MM')>=TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),-11),'MM')  " & vbNewLine
            strSQL += " and TRUNC(p3.dt_vencimento,'MM')<= TRUNC(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM')  " & vbNewLine
            'filtra o c.custo
            If Not String.IsNullOrEmpty(grupo) Then

                If hierarquia = "1" Then
                    strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
                Else
                    strSQL += " and p1.grp_codigo='" & grupo & "'" & vbNewLine
                End If

            End If
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " and g.area='" & area & "'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " and g .area_interna='" & area_interna & "'" & vbNewLine
            End If
            'verifica nível de acesso
            If Not DALCGestor.AcessoAdmin() Then
                'não filtra o centro de custo dos gerentes
                strSQL = strSQL + " and exists(" & vbNewLine
                strSQL = strSQL + "   select 0 from categoria_usuario p100" & vbNewLine
                strSQL = strSQL + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
                'strSQL = strSQL + "     " & vbNewLine
                strSQL = strSQL + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(grupo) Then
                strSQL += " and p1.grp_codigo like '" & grupo & "%'" & vbNewLine
            End If
            If Not String.IsNullOrEmpty(_excluirServico) Then
                strSQL += " and p1.tipo_serv2 not like '" & _excluirServico & "%'" & vbNewLine
            End If
            strSQL += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),op.descricao,r.rateio,p1.rml_numero_a " & vbNewLine
            If Not String.IsNullOrEmpty(area) Then
                strSQL += " ,g.area"
            End If
            If Not String.IsNullOrEmpty(area_interna) Then
                strSQL += " ,g.area_interna"
            End If

            strSQL += " ) GROUP BY operadora, DATA,operadora "
            sql += strSQL
        End If
        sql += " )group by operadora,data order by to_date(data, 'MM/YYYY') "
        'Response.Write(sql)
        'Response.End()

        sqlTotal = sql

        Return sql
    End Function

    Sub CarregaGraficoOperadora(tipoFatura As String)
        Dim strlegenda As String = "select distinct data from(" & getQueryTotalOperadora(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by to_date(data,'MM/YYYY') asc "

        Dim dt As DataTable = _dao_commons.myDataTable(strlegenda)
        Dim categorias As New List(Of String)
        Dim dtNew As New DataTable

        Dim _column As New DataColumn("Operadora")
        dtNew.Columns.Add(_column)

        Dim i As Integer = 0
        For i = 0 To dt.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dt.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dt.Rows(i).Item(0))
            'adiona a linha do datatable
            _column = New DataColumn(dt.Rows(i).Item(0))
            dtNew.Columns.Add(_column)
            _column.DefaultValue = 0


            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        'agora vamos montar as series
        Dim strNomeSerie As String = "select distinct operadora from(" & getQueryTotalOperadora(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") order by operadora asc "
        dt = _dao_commons.myDataTable(strNomeSerie)
        Dim _series As New List(Of Serie)

        For i = 0 To dt.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = dt.Rows(i).Item(0)
            _serie.Data = "["
            _series.Add(_serie)
        Next


        Dim totalRow As DataRow = dtNew.NewRow
        Dim maxvalue As Decimal = 0

        For Each _item As Serie In _series
            Dim _row As DataRow = dtNew.NewRow
            _row.Item(0) = _item.Nome
            dtNew.Rows.Add(_row)

            For Each _categoria As String In categorias
                dt = _dao_commons.myDataTable("select * from (" & getQueryTotalOperadora(tipoFatura, grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna) & ") where data='" & _categoria & "' and operadora='" & _item.Nome & "' order by data,operadora")

                If dt.Rows.Count > 0 Then
                    'achou a tarifa
                    _item.Data = _item.Data & dt.Rows(0).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                    _row.Item(_categoria) = FormatCurrency(dt.Rows(0).Item(1)).Replace(" ", "")
                    totalRow.Item(_categoria) = FormatCurrency(totalRow.Item(_categoria) + dt.Rows(0).Item(1)).Replace(" ", "")

                    If negativeValue > Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        negativeValue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If

                    If maxvalue < Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        maxvalue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If

                Else
                    _row.Item(_categoria) = FormatCurrency(0)
                    _item.Data = _item.Data & 0 & ","
                End If

            Next

        Next

        If negativeValue <> 0 Then
            negativeValue = negativeValue - maxvalue / 10
        End If


        'dt = _dao_commons.myDataTable(getQueryTotalOperadora)
        'For Each _item As Serie In _series
        '    For i = 0 To dt.Rows.Count - 1
        '        If dt.Rows(i).Item(0) = _item.Nome Then
        '            'adiciona na serie
        '            _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
        '        End If
        '    Next
        'Next

        'retiramos a ultima virgula e fechamos a TAG
        For Each _item As Serie In _series
            _item.Data = _item.Data.Substring(0, _item.Data.Length - 1)
            _item.Data = _item.Data & "]"

            GraficoData += "{name: '" & _item.Nome & " ',data:" & _item.Data & "},"

        Next


        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If


        For Each _coluna As DataColumn In dtNew.Columns
            If Not _coluna.ColumnName.ToUpper = "OPERADORA" Then
                _coluna.ColumnName = MonthName(_coluna.ColumnName.Substring(0, 2), True) & _coluna.ColumnName.Substring(2)

            End If

        Next
        totalRow.Item(0) = "Total"
        dtNew.Rows.Add(totalRow)
        Me.gvServicos.DataSource = dtNew
        Me.gvServicos.DataBind()
        Me.gvServicos.Rows(gvServicos.Rows.Count - 1).Font.Bold = True
        Me.gvServicos.Visible = True

        If dtNew.Rows.Count < 1 Then
            Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "EscondeGrid();", True)
        End If

    End Sub

    Private Sub uc_graficoHome_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub
End Class
