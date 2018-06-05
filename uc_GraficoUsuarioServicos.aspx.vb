﻿Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports System.Web.UI

Partial Class uc_GraficoUsuarioServicos
    Inherits System.Web.UI.Page
    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Public _dao_commons As New DAO_Commons
    Public countservs As Integer = 10
    Dim _tipoRel As String = ""
    Public negativeValue As Decimal = 0

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

    Public Property GraficoLabelServicos() As String
        Get
            Return ViewState("GraficoLabelServicos")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoLabelServicos") = value
        End Set
    End Property
    Public Property GraficoDataServicos() As String
        Get
            Return ViewState("GraficoDataServicos")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoDataServicos") = value
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
    Public Property GraficoData() As String
        Get
            Return ViewState("graficoData")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData") = value
        End Set
    End Property


    Public Property GraficoDataTotal() As String
        Get
            Return ViewState("GraficoDataTotal")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoDataTotal") = value
        End Set
    End Property

    Public Property GraficoLabelTotal() As String
        Get
            Return ViewState("GraficoLabelTotal")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoLabelTotal") = value
        End Set
    End Property


    Sub CarregaGraficoServicos()
        Dim strlegenda As String = "select data from (select data from(" & getQueryTotalGeral() & ") group by data order by to_date(data,'MM/YYYY') asc) "

        Dim dtCat As DataTable = _dao_commons.myDataTable(strlegenda)
        Dim i As Integer = 0
        Dim categorias As New List(Of String)
        For i = 0 To dtCat.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dtCat.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dtCat.Rows(i).Item(0))
            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        'agora vamos montar as series
        Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServico() & ") order by tarifa asc "
        Dim dt As DataTable = _dao_commons.myDataTable(strNomeSerie)
        Dim _series As New List(Of Serie)

        For i = 0 To dt.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = dt.Rows(i).Item(0)
            _serie.Data = "["
            _series.Add(_serie)
        Next

        Dim maxvalue As Decimal = 0

        dt = _dao_commons.myDataTable(getQueryTotalServico)
        For Each _item As Serie In _series
            For Each _categoria As String In categorias
                Dim conta As Integer = 0
                Dim achou = False
                For i = 0 To dt.Rows.Count - 1

                    If dt.Rows(i).Item(0) = _item.Nome And _categoria = dt.Rows(i).Item(2) Then
                        'adiciona na serie
                        _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                        'achou = True
                        If negativeValue > Convert.ToDouble(dt.Rows(i).Item(1).ToString) Then
                            negativeValue = Convert.ToDouble(dt.Rows(i).Item(1).ToString)
                        End If

                        If maxvalue < Convert.ToDouble(dt.Rows(i).Item(1).ToString) Then
                            maxvalue = Convert.ToDouble(dt.Rows(i).Item(1).ToString)
                        End If
                        achou = True
                    Else
                        conta += 1

                    End If
                Next
                If Not achou Then
                    _item.Data = _item.Data & "0,"
                End If
            Next


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

    End Sub

    Sub CarregaGraficoServicosRamal()
        Dim strlegenda As String = "select data from (select data from(" & getQueryTotalGeralRamal() & ") group by data order by to_date(data,'MM/YYYY') asc) "

        Dim dtCat As DataTable = _dao_commons.myDataTable(strlegenda)
        Dim i As Integer = 0
        Dim categorias As New List(Of String)
        For i = 0 To dtCat.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dtCat.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dtCat.Rows(i).Item(0))
            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        'agora vamos montar as series
        Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalServicoRamal() & ") order by tarifa asc "
        Dim dt As DataTable = _dao_commons.myDataTable(strNomeSerie)
        Dim _series As New List(Of Serie)

        For i = 0 To dt.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = dt.Rows(i).Item(0)
            _serie.Data = "["
            _series.Add(_serie)
        Next

        Dim maxvalue As Decimal = 0

        dt = _dao_commons.myDataTable(getQueryTotalServicoRamal)
        For Each _item As Serie In _series
            For Each _categoria As String In categorias
                Dim conta As Integer = 0
                Dim achou = False
                For i = 0 To dt.Rows.Count - 1

                    If dt.Rows(i).Item(0) = _item.Nome And _categoria = dt.Rows(i).Item(2) Then
                        'adiciona na serie
                        _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                        'achou = True
                        If negativeValue > Convert.ToDouble(dt.Rows(i).Item(1).ToString) Then
                            negativeValue = Convert.ToDouble(dt.Rows(i).Item(1).ToString)
                        End If

                        If maxvalue < Convert.ToDouble(dt.Rows(i).Item(1).ToString) Then
                            maxvalue = Convert.ToDouble(dt.Rows(i).Item(1).ToString)
                        End If
                        achou = True
                    Else
                        conta += 1

                    End If
                Next
                If Not achou Then
                    _item.Data = _item.Data & "0,"
                End If
            Next


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

    End Sub
    Function getQueryTotalServico() As String
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select * from V_GESTAO_GASTO_CONSOLIDADO2 p1 WHERE 1=1 "
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If
        If Not DALCGestor.AcessoAdmin() And Not _dao_commons.Is_Commom_User(Session("codigousuario")) And ViewState("codigo_usuario") < 1 Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " ) group by tarifa, data order by to_date(data,'MM/YYYY')"



        'sql += ") group by tarifa,data  order  by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalServicoOld() As String
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select 'VOZ' tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p3.codigo_tipo in (1) "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and p1.cdr_codigo='3' "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If


        If Not DALCGestor.AcessoAdmin() And Not _dao_commons.Is_Commom_User(Session("codigousuario")) Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months('" & _data & "',-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY') "

        sql += " union "
        sql += " select tarifa, sum(gasto)gasto, data from( "
        sql += " select upper(replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS')) tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, operadoras_planos op, linhas l, linhas_moveis lm,aparelhos_moveis ap "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p3.codigo_tipo in (1)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and  p1.cdr_codigo <> '3' "


        sql += "  and   replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')=p1.rml_numero_a "
        sql += " and op.codigo_plano(+)=l.codigo_plano and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+)  "
        'tira as cobranças de franquias


        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If



        If Not DALCGestor.AcessoAdmin() And Not _dao_commons.Is_Commom_User(Session("codigousuario")) Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
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

        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months('" & _data & "',-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine

        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS'),op.custo_fixo "
        sql += " ) group by data,tarifa"


        'rateio
        sql += " union"
        sql += " select tarifa, sum(gasto), data from (select distinct 'GASTO' tarifa, nvl(r.rateio,0)  gasto, to_char(f.dt_vencimento, 'MM/YYYY')data, p1.rml_numero_a  "
        sql = sql + " from cdrs_celular_analitico_mv p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        sql = sql + " where p1.codigo_conta=a.codigo_conta"
        sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
        '  sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            'sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(to_char(f.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months('" & _data & "',-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(f.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura,to_char(f.dt_vencimento, 'MM/YYYY'), p1.rml_numero_a ) group by tarifa, data"

        'custo fixo
        'CUSTO FIXO
        sql += " union "
        sql += " select 'SERVIÇOS' tarifa,sum(gasto)gasto,data from (select P1.RML_NUMERO_A, max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
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

        'PARCELAS
        sql += " union "
        sql += " select 'APARELHO' tarifa,sum(gasto)gasto,data from (select P1.RML_NUMERO_A, max(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then nvl(pa.parcela,0) else 0 end) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
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
        sql += "  and nvl(pa.qtd_parcelas,1) - MONTHS_BETWEEN (TO_DATE(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>0"
        sql += "  and  MONTHS_BETWEEN (TO_DATE(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0"
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        sql += " )   group by data"

        'rateio
        'sql += " union"
        'sql += " select distinct 'SERVIÇO' tarifa, nvl(r.rateio,0)  gasto, to_char(f.dt_vencimento, 'MM/YYYY')data "
        'sql = sql + " from cdrs_celular_analitico_mv p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        'sql = sql + " where p1.codigo_conta=a.codigo_conta"
        'sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        'sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        ''tira as cobranças de franquias
        'sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
        ''sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        ''  sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        'If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
        '    sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        'End If

        'If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    'sql = sql + "     " & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If
        'sql += " and to_date(to_char(f.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months('" & _data & "',-11),'MM/YYYY'),'MM/YYYY') "
        'sql += " and to_date(to_char(f.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','DD/MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine

        'sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura,to_char(f.dt_vencimento, 'MM/YYYY') "

        sql += ") group by tarifa,data  order  by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalGeral(Optional pdata As String = "") As String
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        _data = _data.Substring(3)


        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select * from V_GESTAO_GASTO_CONSOLIDADO2 p1 WHERE 1=1 and p1.codigo_tipo='1' "
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If
        If Not DALCGestor.AcessoAdmin() And Not _dao_commons.Is_Commom_User(Session("codigousuario")) Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        sql += " and to_date(data,'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(data,'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine

        If pdata <> "" Then
            sql += " and p1.data ='" & pdata & "'"
        End If

        sql += " ) group by tarifa, data order by to_date(data,'MM/YYYY')"

        Return sql

    End Function

    Function getQueryTotalGeralOLD(Optional pdata As String = "") As String
        Dim _data As String = DALCGestor.MaxUltimaDataFatura()

        _data = _data.Substring(3)

        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select 'GASTO' tarifa,"
        'sql += " sum(p1.valor_cdr)gasto"
        sql += " sum(nvl(p1.total_gasto,p1.valor_cdr)) +(nvl(ap.valor,0)/nvl(ap.qtd_parcelas,1))+nvl(op.custo_fixo,0)  gasto "

        'sql = sql + " sum(nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0)) gasto"

        sql += ",to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4,  operadoras_planos op, linhas l, linhas_moveis lm,aparelhos_moveis ap "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p3.codigo_tipo='1' "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and p1.cdr_codigo='3' "
        sql += "  and   replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')=p1.rml_numero_a "
        sql += " and op.codigo_plano(+)=l.codigo_plano and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+)  "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If pdata <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & pdata & "'"
        End If


        'If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    sql = sql + "     " & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date('" & _data & "','MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date('" & _data & "','MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),nvl(ap.qtd_parcelas,1),nvl(op.custo_fixo,0),nvl(ap.valor,0), p1.rml_numero_a "


        sql += ") group by tarifa,data  "
        'sql += " order  by to_date(data,'MM/YYYY'),tarifa "

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalGeralRamal(Optional pdata As String = "") As String
        Dim _data As String = Date.Now.ToShortDateString

        _data = _data.Substring(3)

        Dim sql As String = ""
        sql += " select tarifa, nvl(sum(gasto),0)gasto, data from ("


        sql += " select 'GASTO' tarifa,"
        'sql += " sum(p1.valor_cdr)gasto"
        'sql += " sum(nvl(p1.total_gasto,p1.valor_cdr)) + case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') then nvl(pa.parcela,0) else 0 end +nvl(op.custo_fixo,0)  gasto "
        sql += " nvl(sum(p1.gasto_total),0) gasto "

        'sql = sql + " sum(nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0)) gasto"

        sql += ",p1.data data  "
        'sql += " from v_tarifacao p1 where 1=1 and p1.gasto>0 "
        sql += " from v_tarifacao p1 where 1=1 "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If pdata <> "" Then
            sql += " and p1.data='" & pdata & "'"
        End If


        'If Not DALCGestor.AcessoAdmin() And ViewState("usuarioComum") <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
        '    sql = sql + "     " & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'End If
        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by p1.data "





        sql += ") group by tarifa,data  "
        'sql += " order  by to_date(data,'MM/YYYY'),tarifa "

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalServicoRamal() As String
        Dim _data As String = Date.Now.ToShortDateString

        _data = _data.Substring(3)

        Dim sql As String = ""
        sql += " select tarifa, nvl(sum(gasto),0)gasto, data from ("


        sql += " select p1.tarifa tarifa,"
        'sql += " sum(p1.valor_cdr)gasto"
        'sql += " sum(nvl(p1.total_gasto,p1.valor_cdr)) + case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') then nvl(pa.parcela,0) else 0 end +nvl(op.custo_fixo,0)  gasto "
        sql += " nvl(sum(p1.gasto),0) gasto "

        'sql = sql + " sum(nvl(p1.valor_cdr-(case when p1.aprovada='S' then p1.valor_devolvido else 0 end), 0)) gasto"

        sql += ",p1.data data  "
        'sql += " from v_tarifacao p1 where 1=1 and p1.gasto>0 "
        sql += " from v_tarifacao2 p1 where 1=1  "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
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
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by  p1.tarifa, p1.data "

        'aparelho
        sql += " union"
        sql += " select tarifa, sum(gasto) gasto, data"
        sql += " from(select tarifa, sum(p100.custo_ramal) gasto, data"
        sql += " from (select 'APARELHO' tarifa,p1.data data,p1.ramal,p1.custo_ramal  "

        sql += " from v_tarifacao p1 where 1=1 "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
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
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
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

        sql += " from v_tarifacao p1 where 1=1 "

        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
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
        sql += " and to_date(p1.data,'MM/YYYY')<= to_date(to_char(add_months(to_date(to_char(sysdate,'MM/YYYY'),'MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by p1.data,  p1.ramal, p1.custo_servico "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        sql += "   group by tarifa,  data "
        sql += " )group by tarifa,  data"



        sql += ") group by tarifa,data  "

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Private Sub uc_GraficoUsuarioServicos_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("codigousuario") Is Nothing Or Session("usuario") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

        If Not Page.IsPostBack Then
            ViewState("codigo_usuario") = Request.QueryString("codigousuario")
            If Request.QueryString("ramal") = 1 Then
                CarregaGraficoServicosRamal()
            Else
                'movel
                CarregaGraficoServicos()
            End If


        End If
    End Sub
End Class