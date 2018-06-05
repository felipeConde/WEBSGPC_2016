Imports System.Data
Imports System.Data.OleDb
Imports System.Globalization
Imports Microsoft.VisualBasic
Imports System.Web.UI

Partial Class uc_grafico_areas
    Inherits System.Web.UI.Page
    Dim strConexao As String = ""
    Dim strSQL As String = ""
    Public _dao_commons As New DAO_Commons
    Dim _tipoRel As String = ""
    Dim nomeTipo As String = ""
    Public myURL As String = ""
    Public negativeValue As Decimal = 0
    Public RowsCount As Integer
    Public labelCCusto As String = ""

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
    'link de dadoss
    Public Property GraficoData6() As String
        Get
            Return ViewState("graficoData6")
        End Get
        Set(ByVal value As String)
            ViewState("graficoData6") = value
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

    Public Property GraficoLabelOper() As String
        Get
            Return ViewState("GraficoLabelOper")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoLabelOper") = value
        End Set
    End Property
    Public Property GraficoDataOper() As String
        Get
            Return ViewState("GraficoDataOper")
        End Get
        Set(ByVal value As String)
            ViewState("GraficoDataOper") = value
        End Set
    End Property

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

    Function getQueryTotalARs() As String

        Dim sql As String = ""

        sql += " select * from("
        sql += " select usuario, gasto,data "
        sql += " ,RANK() OVER (ORDER BY gasto desc)ordem "
        sql += " from( "
        sql += " select nvl(sum(p1.gasto),0)gasto, p1.data data,"
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
        End If
        'sql += " , p1.grp_codigo as codigo  "
        sql += " from V_GESTAO_GASTO_CONSOLIDADO2 p1, grupos p5 "
        sql += " where   "
        sql += " p1.grp_codigo(+)=p5.codigo  "
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p1.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p1.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If


        If ViewState("nomeOper") <> "" Then
            sql += " and p1.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
        End If
        sql += " and p1.data='" & ViewState("vencimento") & "'"

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
        End If

        sql += " ,p1.data))"
        'sql += " group by fs.status_desc"

        If Session("exibir_todos") = False Then
            sql += " where ordem <=10 and rownum<=10 order by ordem"
        End If

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalARsOld() As String

        Dim sql As String = ""

        sql += " select * from("
        sql += " select usuario, gasto,data "
        sql += " ,RANK() OVER (ORDER BY gasto desc)ordem "
        sql += " from( "
        sql += " select nvl(sum(nvl(p1.total_gasto,p1.valor_cdr)),0)gasto, to_char(p3.dt_vencimento, 'MM/YYYY') data,"
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
        End If
        'sql += " , p1.grp_codigo as codigo  "
        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , faturas_tipo p4, grupos p5 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura  "
        sql += " and p3.codigo_tipo=p4.codigo_tipo(+) and p1.grp_codigo(+)=p5.codigo  "
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
        End If
        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If
        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.TARIF_CODIGO in(select t.codigo from TARIFACAO t where upper(t.nome_configuracao)='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "')"
            Else
                sql += " and p1.cdr_codigo not in ('5') "
            End If
        End If

        If ViewState("nomeOper") <> "" Then
            sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
        End If
        sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
        End If

        sql += " ,to_char(p3.dt_vencimento, 'MM/YYYY')))"
        'sql += " group by fs.status_desc"

        If Session("exibir_todos") = False Then
            sql += " where ordem <=10 and rownum<=10 order by ordem"
        End If

        Return sql
    End Function

    Function getQueryTotalARsRamal() As String

        Dim sql As String = ""

        sql += " select * from("
        sql += " select usuario, gasto,data "
        sql += " ,RANK() OVER (ORDER BY gasto desc)ordem "
        sql += " from( "
        sql += " select usuario, sum(gasto)gasto, data from (select nvl(sum(p1.gasto),0)gasto, p1.data,"
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
        End If
        'sql += " , p1.grp_codigo as codigo  "
        sql += " from v_tarifacao p1, grupos p5 "
        sql += " where   "
        sql += "  p1.grupo(+)=p5.codigo  "


        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If
        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
            Else
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "' "
            End If
        End If


        sql += " and p1.data='" & ViewState("vencimento") & "'"

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
        End If

        sql += " ,p1.data "

        'aparelho
        sql += " union"
        sql += " select nvl(sum(gasto),0) gasto, data,usuario"
        sql += " from(select usuario, r.custo_ramal gasto, data"
        sql += " from (select p1.data data,p1.ramal, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from v_tarifacao p1, grupos p5 "
        sql += " where   "
        sql += "  p1.grupo(+)=p5.codigo  "
        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If

        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
            Else
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "' "
            End If
        End If


        sql += " and p1.data='" & ViewState("vencimento") & "'"

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
        'sql += " group by "
        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
        End If
        sql += ", p1.data,  p1.ramal "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        sql += "   group by usuario,  data,r.custo_ramal "
        sql += " )group by usuario,  data"

        'custo gestao
        sql += " union"
        sql += " select nvl(sum(gasto),0) gasto, data,usuario"
        sql += " from(select usuario, r.custo_servico gasto, data"
        sql += " from (select p1.data data,p1.ramal, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from v_tarifacao p1, grupos p5 "
        sql += " where   "
        sql += "  p1.grupo(+)=p5.codigo  "
        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If
        If ViewState("codigo_usuario") <> "" And ViewState("codigo_usuario") > 0 Then
            sql += " and p1.codigo_usuario='" & ViewState("codigo_usuario") & "'"
        End If
        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
            Else
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "' "
            End If
        End If


        sql += " and p1.data='" & ViewState("vencimento") & "'"

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
        'sql += " group by "
        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
        End If
        sql += " , p1.data,  p1.ramal "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        sql += "   group by usuario,  data,r.custo_servico "
        sql += " )group by usuario,  data"
        sql += " )group by usuario, data)"
        sql += " )"
        'sql += " group by fs.status_desc"

        If Session("exibir_todos") = False Then
            sql += " where ordem <=10 And rownum<=10 order by ordem"
        End If

        'Response.Write(sql)
        'Response.End()

        Return sql
    End Function

    Function getQueryTotalARs2Ramal(Optional areas As String = "", Optional internas As String = "", Optional ars As String = "") As String

        Dim sql As String = ""



        sql += " SELECT * FROM( select tarifa, nvl(sum(gasto),0)gasto, data, usuario from ("
        'sql += " select 'VOZ' tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,nvl(r.rateio, 0)rateio,to_char(p3.dt_vencimento, 'MM/YYYY')data, "
        sql += " select p1.tarifa, nvl(sum(p1.gasto),0)gasto,0 rateio,p1.data, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If
        sql += " from v_tarifacao2 p1, grupos p5 "
        sql += " where  "
        sql += " p1.grupo=p5.codigo(+) "
        'sql += " and (p1.cdr_codigo='3' or p1.valor_cdr<0 )"

        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If


        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grupo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by p1.data,p1.tarifa "

        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " ,  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If

        ' sql += " )   group by data, usuario"
        sql += ") "


        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            sql += " where upper(tarifa)='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
        End If

        sql += " group by tarifa,data, usuario "

        'aparelho
        sql += " union"
        sql += " select 'APARELHO' TARIFA, nvl(sum(gasto),0) gasto, data,usuario"
        sql += " from(select usuario, sum(p100.custo_ramal) gasto, data"
        sql += " from (select p1.data data,p1.ramal,p1.custo_ramal, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            'sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
            sql += "  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from v_tarifacao2 p1, grupos p5 "
        sql += " where   "
        sql += "  p1.grupo(+)=p5.codigo  "

        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If

        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
            Else
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "' "
            End If
        End If


        sql += " and p1.data='" & ViewState("vencimento") & "'"

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
        'sql += " group by "
        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            'sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
            sql += "  group by NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If
        sql += ", p1.data,  p1.ramal,p1.custo_ramal "
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        sql += "   group by usuario,  data "
        sql += " )group by usuario,  data"

        'custo gestao
        sql += " union"
        sql += " select 'CUSTO GESTÃO' TARIFA, nvl(sum(gasto),0) gasto, data,usuario"
        sql += " from(select usuario, sum(p100.custo_servico) gasto, data"
        sql += " from (select p1.data data,p1.ramal,p1.custo_servico, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " nvl(p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            'sql += " nvl(p5.codigo, 'NÃO CADASTRADO') USUARIO"
            sql += "  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from v_tarifacao p1, grupos p5 "
        sql += " where   "
        sql += "  p1.grupo(+)=p5.codigo  "

        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If
        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            If ViewState("nomeTipo").ToString.ToUpper.Trim <> "VOZ" Then
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
            Else
                sql += " and p1.tarifa='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "' "
            End If
        End If


        sql += " and p1.data='" & ViewState("vencimento") & "'"

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
        'sql += " group by "
        If ViewState("nivel") = "0" Then
            sql += " group by nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " group by nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            'sql += " group by nvl(p5.nome_grupo, 'NÃO CADASTRADO')"
            'sql += " group by nvl(p5.codigo, 'NÃO CADASTRADO')"
            sql += "  group by NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If
        sql += " , p1.data,  p1.ramal ,p1.custo_servico"
        sql += "  )p100, ramais r "
        sql += " where p100.ramal=r.numero_a(+) "
        sql += "   group by usuario,  data"
        sql += " )group by usuario,  data"

        sql += " ) order by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        'Response.End()


        Return sql
    End Function

    Function getQueryTotalARs2(Optional areas As String = "", Optional internas As String = "", Optional ars As String = "") As String

        Dim sql As String = ""

        'Dim strTipovalor As String = " sum(nvl(p1.valor_cdr,0))gasto "
        'If Not DALCGestor.AcessoAdmin() Then
        '    strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        'End If

        Dim strTarifa As String = "tarifa"
        If ViewState("tipoVisao") = "Oper" Then
            strTarifa = " (select op.descricao from operadoras_teste op where op.codigo=p1.codigo_operadora and rownum<2)tarifa "
        End If

        sql += " select tarifa, nvl(sum(gasto),0)gasto, data, usuario from ("
        'sql += " select 'VOZ' tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,nvl(r.rateio, 0)rateio,to_char(p3.dt_vencimento, 'MM/YYYY')data, "
        sql += " select  " & strTarifa & ", sum(nvl(p1.gasto,0))gasto,0 rateio,p1.data, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If
        sql += " from V_GESTAO_GASTO_CONSOLIDADO2 p1, grupos p5 "
        sql += " where  "
        sql += " p1.grp_codigo=p5.codigo(+) "

        'sql += " and (p1.cdr_codigo='3' or p1.valor_cdr<0 )"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p1.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p1.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and p1.data='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If

        If ViewState("grupo") <> "" Then
            sql += " and p5.nome_grupo like '" & ViewState("grupo") & "%'"
        End If
        If ViewState("ccusto") <> "" Then
            sql += " and p5.codigo like '" & ViewState("ccusto") & "%'"
        End If
        If ViewState("area") <> "" Then
            sql += " and p5.area = '" & ViewState("area") & "'"
        End If
        If ViewState("area_interna") <> "" Then
            sql += " and p5.area_interna = '" & ViewState("area_interna") & "'"
        End If

        If ViewState("nomeOper") <> "" Then
            sql += " and p1.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(p1.data,'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by p1.data, "
        If ViewState("tipoVisao") = "Oper" Then
            sql += " p1.codigo_operadora "
        Else
            sql += "p1.tarifa "
        End If


        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " ,  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If

        'sql += ") "

        sql += " ) "
        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            sql += " where upper(tarifa)='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
        End If
        sql += " group by tarifa,data, usuario order by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        'Response.End()


        Return sql
    End Function


    Function getQueryTotalARs2Old(Optional areas As String = "", Optional internas As String = "", Optional ars As String = "") As String

        Dim sql As String = ""

        Dim strTipovalor As String = " sum(nvl(p1.valor_cdr,0))gasto "
        If Not DALCGestor.AcessoAdmin() Then
            strTipovalor = " sum(nvl(p1.total_gasto,p1.valor_cdr))gasto "
        End If

        sql += " select tarifa, nvl(sum(gasto),0)gasto, data, usuario from ("
        'sql += " select 'VOZ' tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,nvl(r.rateio, 0)rateio,to_char(p3.dt_vencimento, 'MM/YYYY')data, "
        sql += " select 'VOZ' tarifa, " & strTipovalor & ",0 rateio,to_char(p3.dt_vencimento, 'MM/YYYY')data, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+) and p1.grp_codigo=p5.codigo(+) "
        sql += " and p1.cdr_codigo='3'"
        'sql += " and (p1.cdr_codigo='3' or p1.valor_cdr<0 )"
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If

        If ViewState("nomeOper") <> "" Then
            sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY') "

        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " ,  NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If




        sql += " union "
        sql += " select  replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS') tarifa, " & strTipovalor & ",0 rateio,to_char(p3.dt_vencimento, 'MM/YYYY')data, "
        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "   NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.grp_codigo=p5.codigo(+) "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        ' sql += " and  p1.cdr_codigo <> '3' and nvl(p1.total_gasto,p1.valor_cdr)>0 "
        If Not DALCGestor.AcessoAdmin() Then
            sql += " and  p1.cdr_codigo <> '3' and nvl(p1.total_gasto,p1.valor_cdr)>0 "
        Else
            sql += " and  p1.cdr_codigo <> '3' and p1.valor_cdr>0 "
        End If

        'sql += " and  p1.cdr_codigo <> '3'  "
        'sql += " "
        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If
        If ViewState("nomeOper") <> "" Then
            sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql = sql + "     " & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'), replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS')"

        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " , NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If

        If Not DALCGestor.AcessoAdmin() Then
            'rateio para gestores
            sql += " union "
            sql += " select 'rateio' tarifa,sum(gasto)gasto,0 rateio,data, usuario from (select P1.RML_NUMERO_A, nvl(pa.rateio,0) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data , "

            If ViewState("nivel") = "0" Then
                sql += " nvl(p5.area, 'SEM AREA') USUARIO"
            ElseIf ViewState("nivel") = "1" Then
                sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
            ElseIf ViewState("nivel") = "2" Then
                sql += "   NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
            End If

            sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, rateiogestao_mv pa "
            sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
            sql += "  and  p1.rml_numero_a=pa.rml_numero_a and p1.codigo_conta = pa.codigo_conta "
            sql += " and p1.tarif_codigo = p4.codigo(+)"
            'sql += " and  p1.cdr_codigo <>'3' "
            sql += " and p1.grp_codigo=p5.codigo(+)"

            If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
                sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
            End If
            If ViewState("codServico") <> "" Then
                sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
            End If
            If ViewState("vencimento") <> "" Then
                sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
            End If
            If areas <> "" And areas <> "'SEM AREA'" Then
                sql += " and  (p5.area in(" & areas & "))"
            End If
            If internas <> "" Then
                sql += " and  (p5.area_interna in(" & internas & "))"
            End If
            If ars <> "" Then
                sql += " and  (p5.codigo in(" & ars & "))"
            End If
            If ViewState("nomeOper") <> "" Then
                sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
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
            sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
            sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A,pa.rateio "
            If ViewState("nivel") = "0" Then
                sql += ",nvl(p5.area, 'SEM AREA')"
            ElseIf ViewState("nivel") = "1" Then
                sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
            ElseIf ViewState("nivel") = "2" Then
                sql += " , NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
            End If
            sql += " )   group by data, usuario"
        End If

        'PARCELAS
        sql += " union "
        sql += " select 'APARELHO' tarifa,sum(gasto)gasto,0 rateio,data, usuario from (select P1.RML_NUMERO_A, max(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') and  MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0 then nvl(pa.parcela,0) else 0 end) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data , "

        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "   NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"

        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If
        If ViewState("nomeOper") <> "" Then
            sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
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
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " , NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If
        sql += " )   group by data, usuario"

        'CUSTO FIXO
        sql += " union "
        sql += " select 'SERVIÇOS' tarifa,sum(gasto)gasto,0 rateio,data, usuario from (select P1.RML_NUMERO_A,  max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data , "

        If ViewState("nivel") = "0" Then
            sql += " nvl(p5.area, 'SEM AREA') USUARIO"
        ElseIf ViewState("nivel") = "1" Then
            sql += " nvl(p5.area_interna, 'SEM AREA INTERNA') USUARIO"
        ElseIf ViewState("nivel") = "2" Then
            sql += "   NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') USUARIO"
        End If

        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.codigo_usuario=pa.codigo_usuario and p1.rml_numero_a=pa.num_linha"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"

        If ViewState("codOper") <> "" And ViewState("codOper") > 0 Then
            sql += " and p3.codigo_operadora='" & ViewState("codOper") & "'"
        End If
        If ViewState("codServico") <> "" Then
            sql += " and p3.codigo_tipo='" & ViewState("codServico") & "'"
        End If
        If ViewState("vencimento") <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & ViewState("vencimento") & "'"
        End If
        If areas <> "" And areas <> "'SEM AREA'" Then
            sql += " and  (p5.area in(" & areas & "))"
        End If
        If internas <> "" Then
            sql += " and  (p5.area_interna in(" & internas & "))"
        End If
        If ars <> "" Then
            sql += " and  (p5.codigo in(" & ars & "))"
        End If
        If ViewState("nomeOper") <> "" Then
            sql += " and p3.codigo_operadora in (select t.codigo from OPERADORAS_TESTE t where upper(t.descricao)='" & ViewState("nomeOper").ToString.ToUpper.Trim & "')"
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
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        If ViewState("nivel") = "0" Then
            sql += ",nvl(p5.area, 'SEM AREA')"
        ElseIf ViewState("nivel") = "1" Then
            sql += " ,nvl(p5.area_interna, 'SEM AREA INTERNA')"
        ElseIf ViewState("nivel") = "2" Then
            sql += " , NVL(p5.codigo ||' - '|| p5.nome_grupo, 'NÃO CADASTRADO') "
        End If
        sql += " )   group by data, usuario"
        sql += ") "


        If ViewState("nomeTipo") <> "" Then
            'filtra o tipo
            sql += " where upper(tarifa)='" & ViewState("nomeTipo").ToString.ToUpper.Trim & "'"
        End If

        sql += " group by tarifa,data, usuario order by to_date(data,'MM/YYYY')"

        'Response.Write(sql)
        ' Response.End()


        Return sql
    End Function


    Sub CarregaGraficoARs()


        Dim _Strcodigos As String = "select distinct * from(" & getQueryTotalARs() & ") "
        Dim dt As DataTable = _dao_commons.myDataTable(_Strcodigos)

        Dim _codigos As String = ""
        For Each _item As DataRow In dt.Rows
            _codigos += "'" & _item.Item(0) & "',"
        Next

        If _codigos.Replace(",", "") = "" Then
            containerGrafico.Visible = False
            'Me.div_sem_usuarios.Visible = True
            Exit Sub
        End If

        If String.IsNullOrEmpty(_codigos) = False Then
            _codigos = _codigos.Substring(0, _codigos.Length - 1)

        End If

        Dim strlegenda As String = ""

        If ViewState("nivel") = "0" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2(_codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If
        If ViewState("nivel") = "1" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2("", _codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If
        If ViewState("nivel") = "2" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2("", "", _codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If

        Dim categorias As New List(Of String)
        Dim _serieDt As New DataTable
        _serieDt.Columns.Add("nome")


        dt = _dao_commons.myDataTable(strlegenda)
        Dim i As Integer = 0
        For i = 0 To dt.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dt.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dt.Rows(i).Item(0))
            _serieDt.Columns.Add(dt.Rows(i).Item(0))
            _serieDt.Columns(dt.Rows(i).Item(0)).DefaultValue = 0

            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        RowsCount = dt.Rows.Count

        'agora vamos montar as series
        Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalARs2() & ") order by tarifa asc "
        dt = _dao_commons.myDataTable(strNomeSerie)
        Dim _series As New List(Of Serie)

        For i = 0 To dt.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = "'" & dt.Rows(i).Item(0) & "'"
            _serie.Data = "["
            _series.Add(_serie)
        Next



        'dt = _dao_commons.myDataTable("select * from(" & getQueryTotalUsuarios2(_codigos) & ") order by usuario,tarifa")
        'For Each _item As Serie In _series
        '    For i = 0 To dt.Rows.Count - 1
        '        If dt.Rows(i).Item(0) = _item.Nome.Replace("'", "") Then
        '            'adiciona na serie
        '            _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
        '        Else
        '            _item.Data = _item.Data & 0 & ","
        '        End If
        '    Next
        'Next

        Dim maxvalue As Decimal = 0

        For Each _serie As Serie In _series
            For Each _categoria As String In categorias
                dt = _dao_commons.myDataTable("select * from(" & getQueryTotalARs2() & ") where usuario='" & _categoria & "' and tarifa=" & _serie.Nome & " order by usuario,tarifa")
                If dt.Rows.Count > 0 Then
                    'achou a tarifa
                    _serie.Data = _serie.Data & dt.Rows(0).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                    If negativeValue > Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        negativeValue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If

                    If maxvalue < Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        maxvalue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If
                Else
                    _serie.Data = _serie.Data & 0 & ","
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

            GraficoData += "{name: " & _item.Nome & ",data:" & _item.Data & "},"

        Next


        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

    End Sub

    Sub CarregaGraficoARsRamal()


        Dim _Strcodigos As String = "select distinct * from(" & getQueryTotalARsRamal() & ") "
        Dim dt As DataTable = _dao_commons.myDataTable(_Strcodigos)

        Dim _codigos As String = ""
        For Each _item As DataRow In dt.Rows
            _codigos += "'" & _item.Item(0) & "',"
        Next

        If _codigos.Replace(",", "") = "" Then
            containerGrafico.Visible = False
            'Me.div_sem_usuarios.Visible = True
            Exit Sub
        End If

        If String.IsNullOrEmpty(_codigos) = False Then
            _codigos = _codigos.Substring(0, _codigos.Length - 1)

        End If

        Dim strlegenda As String = ""

        If ViewState("nivel") = "0" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2Ramal(_codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If
        If ViewState("nivel") = "1" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2Ramal("", _codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If
        If ViewState("nivel") = "2" Then
            strlegenda = "select distinct usuario, sum(gasto) from(" & getQueryTotalARs2Ramal("", "", _codigos) & ") group by usuario order by sum(gasto) desc, usuario asc  "
        End If

        Dim categorias As New List(Of String)
        Dim _serieDt As New DataTable
        _serieDt.Columns.Add("nome")


        dt = _dao_commons.myDataTable(strlegenda)
        Dim i As Integer = 0
        For i = 0 To dt.Rows.Count - 1
            GraficoLabel += ""
            GraficoLabel += "'" & dt.Rows(i).Item(0) & "'"
            GraficoLabel += ","
            categorias.Add(dt.Rows(i).Item(0))
            _serieDt.Columns.Add(dt.Rows(i).Item(0))
            _serieDt.Columns(dt.Rows(i).Item(0)).DefaultValue = 0

            'coloca os valores
            'movel
            'GraficoData += dt.Rows(0).Item(i).ToString.Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
        Next

        RowsCount = dt.Rows.Count

        'agora vamos montar as series
        Dim strNomeSerie As String = "select distinct tarifa from(" & getQueryTotalARs2Ramal() & ") order by tarifa asc "
        dt = _dao_commons.myDataTable(strNomeSerie)
        Dim _series As New List(Of Serie)

        For i = 0 To dt.Rows.Count - 1
            Dim _serie As New Serie
            _serie.Nome = "'" & dt.Rows(i).Item(0) & "'"
            _serie.Data = "["
            _series.Add(_serie)
        Next



        'dt = _dao_commons.myDataTable("select * from(" & getQueryTotalUsuarios2(_codigos) & ") order by usuario,tarifa")
        'For Each _item As Serie In _series
        '    For i = 0 To dt.Rows.Count - 1
        '        If dt.Rows(i).Item(0) = _item.Nome.Replace("'", "") Then
        '            'adiciona na serie
        '            _item.Data = _item.Data & dt.Rows(i).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
        '        Else
        '            _item.Data = _item.Data & 0 & ","
        '        End If
        '    Next
        'Next

        Dim maxvalue As Decimal = 0

        For Each _serie As Serie In _series
            For Each _categoria As String In categorias
                dt = _dao_commons.myDataTable("select * from(" & getQueryTotalARs2Ramal() & ") where usuario='" & _categoria & "' and tarifa=" & _serie.Nome & " order by usuario,tarifa")
                If dt.Rows.Count > 0 Then
                    'achou a tarifa
                    _serie.Data = _serie.Data & dt.Rows(0).Item(1).ToString.Replace(".", "").Replace(",", ".") & ","
                    If negativeValue > Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        negativeValue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If

                    If maxvalue < Convert.ToDouble(dt.Rows(0).Item(1).ToString) Then
                        maxvalue = Convert.ToDouble(dt.Rows(0).Item(1).ToString)
                    End If
                Else
                    _serie.Data = _serie.Data & 0 & ","
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

            GraficoData += "{name: " & _item.Nome & ",data:" & _item.Data & "},"

        Next


        If GraficoData <> "" Then
            If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
                GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
            End If
        End If

    End Sub

    Private Sub uc_grafico_areas_Load(sender As Object, e As EventArgs) Handles Me.Load
        _dao_commons.strConn = Session("conexao")
        GraficoLabel = ""
        GraficoData = ""
        myURL = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + ResolveUrl("~/")

        'Response.Write("entrou")
        'Response.End()

        If Not Page.IsPostBack Then

            GraficoData = ""
            GraficoData2 = ""
            GraficoLabel = ""
            Session("exibir_todos") = False

            If String.IsNullOrEmpty(Request.QueryString("nivel")) = True Then
                ViewState("nivel") = "0"
                If Not DALCGestor.AcessoAdmin() Then
                    getPerfil()
                End If
            Else
                ViewState("nivel") = Request.QueryString("nivel")
            End If

            If Not DALCGestor.AcessoAdmin() And AppIni.GloboRJ_Parm = True Then
                ViewState("area") = Request.QueryString("area")
                ViewState("area_interna") = Request.QueryString("area_interna")
                ViewState("ccusto") = Request.QueryString("ccusto")
            Else
                ViewState("area") = IIf(Session("area") = "", Request.QueryString("area"), Session("area"))
                ViewState("area_interna") = IIf(Session("area_interna") = "", Request.QueryString("area_interna"), Session("area_interna"))
                ViewState("ccusto") = IIf(Session("grupo") = "", Request.QueryString("ccusto"), Session("grupo"))
            End If
            If ViewState("area") <> "" Then
                ViewState("area") = System.Uri.UnescapeDataString(ViewState("area")).ToString.Replace("undefined,", "").Replace("? string: ?", "")
            End If
            If ViewState("area_interna") <> "" Then
                ViewState("area_interna") = System.Uri.UnescapeDataString(ViewState("area_interna")).ToString.Replace("undefined,", "").Replace("? string: ?", "")
            End If


            ViewState("vencimento") = Request.QueryString("mesAno")
            ViewState("tipoGrafico") = Request.QueryString("tipoGrafico")
            ViewState("nomeServico") = Request.QueryString("nomeServico")
            ViewState("codServico") = Request.QueryString("codigoTipo")
            ViewState("tipoVisao") = Request.QueryString("tipoVisao")

            If Not ViewState("vencimento").ToString.Contains("/") Then
                ViewState("vencimento") = ViewState("vencimento").ToString.Substring(0, 2) & "/" & ViewState("vencimento").ToString.Substring(2, 4)
            End If

            If AppIni.GloboRJ_Parm = False Then
                ViewState("nivel") = "2"
            End If

            If ViewState("area") <> "" Then
                ViewState("nivel") = "1"
            End If

            If ViewState("area_interna") <> "" Then
                ViewState("nivel") = "2"
            End If
            If ViewState("ccusto") <> "" Then
                ViewState("nivel") = "2"
            End If


            If Request.QueryString("tipoGrafico") = "3" Then
                'ramal
                If AppIni.GloboRJ_Parm = False And ViewState("nivel") = "2" Then
                    CarregaGraficoARsRamal()
                ElseIf AppIni.GloboRJ_Parm = True And ViewState("nivel") <> "3" And ViewState("nivel") <> "4" Then
                    CarregaGraficoARsRamal()
                Else
                    ' CarregaGraficoUsuarios2()
                End If

            Else
                'fatura
                If AppIni.GloboRJ_Parm = False And ViewState("nivel") = "2" Then
                    CarregaGraficoARs()
                ElseIf AppIni.GloboRJ_Parm = True And ViewState("nivel") <> "3" And ViewState("nivel") <> "4" Then
                    CarregaGraficoARs()
                Else
                    ' CarregaGraficoUsuarios2()
                End If

            End If

            labelCCusto = getLabel("NOME_CCUSTO")

        End If

        'Dim script As String = "setTimeout(ResizeMe, 1000);"
        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>ResizeMe()</script>")


    End Sub

    Sub getPerfilOld()
        'Dim _dao_dashBoard As New DAO_Dashboard
        'Dim dt As DataTable = _dao_dashBoard.getUsuariosPerfis("", "", "", "", Trim(Session("codigousuario")))
        'If dt.Rows.Count > 0 Then
        '    ViewState("nivel") = dt.Rows(0).Item("nivel")
        'End If


        'verifica se é administrador
        'verifica se é administrador
        Dim _list As New List(Of AppGeneric)
        Dim admin As String = ""




        Dim sql As String = "select min(nivel)nivel  from  (select codigo,nome_usuario, sum(total)total, max(nivel)nivel from ("
        'diretor de central
        sql += "select codigo,nome_usuario, count(*)total, 0 nivel "
        sql += " from("
        sql += " Select distinct g.area_interna, u.codigo,u.nome_usuario from grupos g, usuarios u, categoria_usuario cat where g.area_interna is not null "
        sql += " and cat.codigo_usuario=u.codigo and to_char(g.codigo) like cat.codigo_grupo||'%' "
        sql += " and  cat.tipo_usuario In('D','G','GC') "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += " and exists(" & vbNewLine
            sql += "   select 0 from categoria_usuario cat" & vbNewLine
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(u.codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If
        sql += " )"
        sql += " group by codigo,nome_usuario"
        sql += " having count(*)>1 "

        'diretor area interna
        sql += " union select codigo,nome_usuario, count(*)total, 1 nivel "
        sql += " from("
        sql += " Select distinct g.codigo ccusto, u.codigo,u.nome_usuario from grupos g, usuarios u, categoria_usuario cat where g.codigo is not null "
        sql += " and cat.codigo_usuario=u.codigo and to_char(g.codigo) like cat.codigo_grupo||'%' "
        sql += " and  cat.tipo_usuario In('D','G','GC') "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += " and exists(" & vbNewLine
            sql += "   select 0 from categoria_usuario cat" & vbNewLine
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(g.codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If
        sql += " )"
        sql += " group by codigo,nome_usuario"
        sql += " having count(*)>1 "

        sql += " union select codigo,nome_usuario, count(*)total, 2 nivel "
        sql += " from("
        sql += " Select distinct g.codigo ccusto, u.codigo,u.nome_usuario from grupos g, usuarios u, categoria_usuario cat where g.codigo is not null "
        sql += " and cat.codigo_usuario=u.codigo and to_char(g.codigo) like cat.codigo_grupo||'%' "
        sql += " and  cat.tipo_usuario In('D','G','GC') "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += " and exists(" & vbNewLine
            sql += "   select 0 from categoria_usuario cat" & vbNewLine
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(g.codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If
        sql += " )"
        sql += " group by codigo,nome_usuario"
        sql += " having count(*)=1 "

        'usuarios comuns
        sql += " union select codigo,nome_usuario, count(*)total, 3 nivel "
        sql += " from("
        sql += " Select distinct u.codigo,u.nome_usuario from usuarios u where not exists (select 0 from categoria_usuario where codigo_usuario=u.codigo)  "

        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += " and exists(" & vbNewLine
            sql += "   select 0 from categoria_usuario cat" & vbNewLine
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(u.grp_codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If
        sql += " )"
        sql += " group by codigo,nome_usuario"
        sql += " having count(*)=1 "

        sql += ") group by codigo,nome_usuario)"

        'Response.Write(sql)
        'Response.End()


        Dim dt As DataTable = _dao_commons.myDataTable(sql)



        If dt.Rows.Count > 0 Then
            ViewState("nivel") = dt.Rows(0).Item("nivel")
        End If



    End Sub

    Sub getPerfil()
        'Dim _dao_dashBoard As New DAO_Dashboard
        'Dim dt As DataTable = _dao_dashBoard.getUsuariosPerfis("", "", "", "", Trim(Session("codigousuario")))
        'If dt.Rows.Count > 0 Then
        '    ViewState("nivel") = dt.Rows(0).Item("nivel")
        'End If


        'verifica se é administrador
        'verifica se é administrador
        Dim _list As New List(Of AppGeneric)
        Dim admin As String = ""



        Dim sql As String = "select nvl(min(nivel),2)nivel  from  ("
        sql += " select  count(*),min(nivel)nivel  from  ("
        'diretor de central
        sql += " select codigo, count(*), 0 nivel from( "
        sql += " select g.area codigo"
        sql += " from grupos g, categoria_usuario cat "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(g.codigo) like cat.codigo_grupo||'%'  " & vbNewLine
            sql += " group by g.area "
        End If
        sql += " )"
        sql += " group by codigo )"
        sql += " having count(*)>1 "

        sql += " union "
        sql += " select  count(*),min(nivel)nivel  from  ( select codigo, count(*), 1 nivel from( "
        sql += " select g.area_interna codigo"
        sql += " from grupos g, categoria_usuario cat "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(g.codigo) like cat.codigo_grupo||'%'  " & vbNewLine
            sql += " group by g.area_interna "
        End If
        sql += " )"
        sql += " group by codigo )"
        sql += " having count(*)>1 "

        sql += " union "
        sql += " select  count(*),min(nivel)nivel  from  ( select codigo, count(*), 2 nivel from( "
        sql += " select g.codigo codigo"
        sql += " from grupos g, categoria_usuario cat "
        If Not DALCGestor.AcessoAdmin(Trim(Session("codigousuario"))) Then
            'não filtra o centro de custo dos gerentes
            sql += "     where cat.codigo_usuario=" + Trim(Session("codigousuario")) & vbNewLine
            sql += "     and cat.tipo_usuario In('D','G','GC') and to_char(g.codigo) like cat.codigo_grupo||'%'  " & vbNewLine
            sql += " group by g.codigo "
        End If
        sql += " )"
        sql += " group by codigo )"
        sql += " having count(*)>1 "


        sql += " )"



        'Response.Write(sql)
        'Response.End()


        Dim dt As DataTable = _dao_commons.myDataTable(sql)



        If dt.Rows.Count > 0 Then
            ViewState("nivel") = dt.Rows(0).Item("nivel")
        End If



    End Sub

    Function getLabel(campo As String) As String
        Dim myItems As List(Of AppGeneric) = _dao_commons.GetGenericList(campo, "NOME_PARAMETRO", "VALOR_PARAMETRO", "PARAMETROS_SGPC")
        'myItems.Add(New Items(grupo, 12456))
        If myItems.Count > 0 Then
            Return myItems.Item(0).Descricao.ToString
        Else
            Return ""
        End If
    End Function
End Class

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
