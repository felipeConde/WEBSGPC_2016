﻿Imports System.Data
Imports System.Web.Script.Serialization

Partial Class main
    Inherits System.Web.UI.Page
    Public usuario As AppUsuarios
    Public nomeusuario As String
    Dim _dao_commons As New DAO_Commons

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
    Public Property GraficoLabel() As String
        Get
            Return ViewState("graficoLabel")
        End Get
        Set(ByVal value As String)
            ViewState("graficoLabel") = value
        End Set
    End Property

    Public strGrafico As String = ""
    Public strSQL As String = ""
    Public Meta As Double
    Public ExibeMovel As Boolean = True
    Public ExibeFixo As Boolean = False
    Public Exibe0800 As Boolean = False
    Public Exibe3003 As Boolean = False
    Public ExibeServico As Boolean = False
    Public exibeDados As Boolean = False
    Public _excluirServico As String = ""
    Public total12meses As Double = 0
    Public sqlTotal As String = ""
    Public totalMesAtual As Double = 0
    Public MesAtual As String = ""
    Public VariacaoMesAnterior As Double = 0
    Dim controller As New HomeController
    Dim _dao As New DAO_Dashboard
    Public myUrl As String = ""


    Public GraficoDataDouble As New List(Of Double)



    Private Sub main_Load(sender As Object, e As EventArgs) Handles Me.Load

        myUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) & "/"





        If Session("codigousuario") Is Nothing Or Session("usuario") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If
        usuario = Session("usuario")
        nomeusuario = usuario.Nome_Usuario

        If Not usuario.AD = "True" Then
            VerificaSenha()
        End If




        Dim grupo As String = ""
        Dim area As String = ""
        Dim area_interna As String = ""
        Dim hierarquia As String = ""
        Dim tipoValor As String = 1


        'TemFaturasCarregadas()

        Session.Timeout = 99999
        'CarregaGrupos()

        If Not Request.QueryString("grupo") Is Nothing Then
            Try
                Me.ddlGrupos.SelectedValue = Request.QueryString("grupo")
            Catch ex As Exception
            End Try
        End If
        If Not Request.QueryString("area") Is Nothing Then
            Try
                'Me.cmbCentral.SelectedValue = Request.QueryString("area")
            Catch ex As Exception
            End Try
        End If
        If Not Request.QueryString("area_interna") Is Nothing Then
            Try
                Me.cmbAreaInterna.SelectedValue = Request.QueryString("area_interna")
            Catch ex As Exception
            End Try
        End If

        'If (Session("conexao") Is Nothing) Or (Session("LOGIN") <> "True" And Session("LOGIN") <> "Verdadeiro") Then

        '    Response.Write("Logue novamente")
        '    Response.End()
        'End If



        'Response.Cache.SetCacheability(HttpCacheability.Public)
        'Response.Cache.SetMaxAge(New TimeSpan(24, 0, 0))
        GraficoData = ""
        GraficoData2 = ""
        GraficoLabel = ""
        _dao_commons.strConn = Session("conexao")


        If Not Page.IsPostBack Then

            'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "Bemvindo();", True)
            'MontaGraficoTotalOperadora()

            'pega o C.Custo

            hierarquia = "1"
            'tipoValor = Request.QueryString("tipoValor")
            tipoValor = Session("tipoValor")
            If Not DALCGestor.AcessoAdmin() Or grupo <> "" Then
                'exibe só o valor faturado
                'Me.lstRbTipoValor.Items.RemoveAt(1)
                'Me.lstRbTipoValor.SelectedValue = 1
                Session("tipoValor") = 1
                tipoValor = Session("tipoValor")
                'Me.lstRbTipoValor.Visible = False
            End If
        End If

        If _dao_commons.Is_Commom_User(Session("codigousuario")) Then
            'usuario comum
            Response.Redirect("GastoUsuario.aspx")
        End If

        'If (tipoValor = 2 Or tipoValor = "") And DALCGestor.AcessoAdmin() Then
        'If (tipoValor = 2) And DALCGestor.AcessoAdmin() Then
        '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "DEVIDO", area, area_interna)
        '    'Carrega_Grafico(grupo, hierarquia, "Exibe-Ramal", "DEVIDO")
        '    'Me.lstRbTipoValor.SelectedValue = 2
        'Else
        '    Carrega_Grafico(grupo, hierarquia, "Exibe-Fixo", "FATURADO", area, area_interna)
        '    'Me.lstRbTipoValor.SelectedValue = 1
        'End If
        'CarregaGastoUltimoMes()

        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addBemvindo", "Bemvindo();", True)



    End Sub

    Private Sub CarregaAreas()
        'verifica se é administrador
        Dim _list As New List(Of AppGeneric)
        Dim admin As String = ""

        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            admin += " and exists(" & vbNewLine
            admin += "   select 0 from categoria_usuario cat" & vbNewLine
            admin += "     where cat.codigo_usuario=" + Session("codigousuario") & vbNewLine
            admin += "     and cat.tipo_usuario In('D','G','GC') and to_char(u.codigo) like cat.codigo_grupo||'%' ) " & vbNewLine
        End If

        _list = _dao_commons.GetGenericList("", "u.area", "u.area", "grupos u", "", admin & " and u.area is not null order by u.area ")

        If _list.Count > 1 Then
            _list.Insert("0", New AppGeneric("0", "Todos"))
            ViewState("manyCentral") = True
        Else
            Session("nivel" & Session("codigousuario")) = "3"
        End If


        'cmbCentral.DataSource = _list
        'cmbCentral.DataBind()
        'Me.upGrafico.DataBind()

    End Sub

    'Private Sub cmbCentral_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCentral.SelectedIndexChanged

    '    'CarregaAreasInternas(IIf(Me.cmbCentral.SelectedValue = "0", "", Me.cmbCentral.SelectedValue))
    '    'CarregaGrupos(IIf(Me.cmbCentral.SelectedValue = "0", "", Me.cmbCentral.SelectedValue), IIf(Me.cmbAreaInterna.SelectedValue = "0", "", Me.cmbAreaInterna.SelectedValue))
    '    'ViewState("area") = Me.cmbCentral.SelectedValue


    'End Sub



    Private Sub main_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        'InverteGridView(Me.gvResumoMensal)
    End Sub
    'Public Sub InverteGridView(ByVal mygrid As GridView)
    '    Dim jsserialize As New JavaScriptSerializer

    '    Dim dt As New DataTable
    '    Dim i As Integer = 1

    '    'totais
    '    'Dim total(12) As Double


    '    'cria as colunas de acordo com as linhas
    '    dt.Columns.Add(" ")

    '    For Each linha As GridViewRow In mygrid.Rows
    '        Try
    '            Dim nomeColuna As String = linha.Cells(0).Text

    '            dt.Columns.Add(nomeColuna)
    '            dt.Columns(nomeColuna).DefaultValue = "R$ 0,00"


    '        Catch ex As Exception
    '        End Try

    '    Next

    '    Dim myRow As DataRow = dt.NewRow
    '    Dim myRowFixo As DataRow = dt.NewRow
    '    Dim myRowServicos As DataRow = dt.NewRow
    '    Dim myRow0800 As DataRow = dt.NewRow
    '    Dim myRow4004 As DataRow = dt.NewRow
    '    Dim myRowDADOS As DataRow = dt.NewRow
    '    Dim myRowTotais As DataRow = dt.NewRow
    '    'coloca os valores das colunas
    '    myRow.Item(0) = "Linhas Móveis"
    '    myRowFixo.Item(0) = "Linhas Fixas"
    '    myRowServicos.Item(0) = "Serviços"
    '    myRow0800.Item(0) = "Linhas 0800"
    '    myRow4004.Item(0) = "Número Único"
    '    myRowDADOS.Item(0) = "Link de Dados"
    '    myRowTotais.Item(0) = "Total"
    '    For Each linha As GridViewRow In mygrid.Rows
    '        Dim valor As String = linha.Cells(1).Text
    '        If linha.Cells(2).Text = 1 Then
    '            'movel
    '            myRow.Item(linha.Cells(0).Text) = valor
    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                ExibeMovel = True
    '            End If
    '        ElseIf linha.Cells(2).Text = 2 Then
    '            'fixo
    '            myRowFixo.Item(linha.Cells(0).Text) = valor
    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                ExibeFixo = True
    '            End If
    '        ElseIf linha.Cells(2).Text = 4 Then
    '            '0800
    '            myRow0800.Item(linha.Cells(0).Text) = valor
    '            ' myRow.Item(linha.Cells(0).Text) = 0
    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                Exibe0800 = True
    '            End If
    '        ElseIf linha.Cells(2).Text = 6 Then
    '            '0800
    '            myRow4004.Item(linha.Cells(0).Text) = valor
    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                Exibe3003 = True
    '            End If
    '            'ElseIf linha.Cells(2).Text = 3 Then
    '            '    'servicos
    '            '    myRowServicos.Item(linha.Cells(0).Text) = valor

    '            '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '            '        ExibeServico = True

    '            '    End If
    '            'ElseIf linha.Cells(2).Text = 5 Then
    '            '    'servicos
    '            '    myRowServicos.Item(linha.Cells(0).Text) = valor

    '            '    If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '            '        ExibeServico = True

    '            '    End If
    '        ElseIf linha.Cells(2).Text = 5 Then
    '            'LINK de DADOS
    '            myRowDADOS.Item(linha.Cells(0).Text) = valor
    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                exibeDados = True
    '            End If
    '        Else
    '            'serviços
    '            myRowServicos.Item(linha.Cells(0).Text) = valor

    '            If (Not String.IsNullOrEmpty(valor) And valor <> "R$ 0,00") Then
    '                ExibeServico = True

    '            End If
    '        End If

    '        Try
    '            'coloca os totais
    '            myRowTotais.Item(linha.Cells(0).Text) = Convert.ToDecimal(myRowTotais.Item(linha.Cells(0).Text).ToString.Trim.Replace("R$ ", "")) + valor
    '            myRowTotais.Item(linha.Cells(0).Text) = FormatCurrency(myRowTotais.Item(linha.Cells(0).Text))

    '        Catch ex As Exception

    '        End Try

    '        i += 1
    '    Next

    '    dt.Rows.Add(myRow)
    '    dt.Rows.Add(myRowFixo)
    '    dt.Rows.Add(myRow0800)
    '    dt.Rows.Add(myRow4004)
    '    dt.Rows.Add(myRowServicos)
    '    dt.Rows.Add(myRowDADOS)
    '    dt.Rows.Add(myRowTotais)
    '    Me.gvResumoGeral.DataSource = dt
    '    Me.gvResumoGeral.DataBind()
    '    Me.gvResumoGeral.Rows(6).Style.Add("font-weight", "bold")
    '    ' Me.gvResumoGeral.Rows(6).Style.Add("background-color", "#D8D9DE")
    '    Me.gvResumoMensal.Visible = False

    '    'coloca a legenda dos graficos

    '    For i = 1 To dt.Columns.Count - 1
    '        GraficoLabel += ""
    '        GraficoLabel += "'" & dt.Columns(i).ColumnName & "'"
    '        GraficoLabel += ","

    '        'coloca os valores
    '        'movel
    '        GraficoData += "" & dt.Rows(0).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        total12meses += dt.Rows(0).Item(i)

    '        'GraficoDataDouble.Add(dt.Rows(0).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", "."))
    '        'fixo
    '        If ExibeFixo Then
    '            GraficoData2 += dt.Rows(1).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        Else
    '            gvResumoGeral.Rows(1).Visible = False
    '            gvResumoGeral.Rows(5).Visible = False
    '        End If

    '        '0800
    '        If Exibe0800 Then
    '            GraficoData3 += dt.Rows(2).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        Else
    '            gvResumoGeral.Rows(2).Visible = False
    '        End If

    '        '4004
    '        If Exibe3003 Then
    '            GraficoData4 += dt.Rows(3).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        Else
    '            gvResumoGeral.Rows(3).Visible = False
    '        End If

    '        'serviços
    '        If ExibeServico Then
    '            GraficoData5 += dt.Rows(4).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        Else
    '            gvResumoGeral.Rows(4).Visible = False
    '        End If

    '        'link de dados
    '        If exibeDados Then
    '            GraficoData6 += dt.Rows(5).Item(i).Replace(".", "").Replace("R$ ", "").Replace(",", ".") & ","
    '        Else
    '            gvResumoGeral.Rows(5).Visible = False
    '        End If

    '        'coloca o link para o RIT
    '        'dt.Columns(i).ColumnName = Context.Server.HtmlDecode("<a href='#'>" + dt.Columns(i).ColumnName.ToString + "</a>")

    '    Next

    '    If GraficoData <> "" Then
    '        If GraficoData.Substring(GraficoData.Length - 1, 1) = "," Then
    '            GraficoData = GraficoData.Substring(0, GraficoData.Length - 1)
    '        End If
    '    End If

    '    If GraficoData2 <> "" Then
    '        If GraficoData2.Substring(GraficoData2.Length - 1, 1) = "," Then
    '            GraficoData2 = GraficoData2.Substring(0, GraficoData2.Length - 1)
    '        End If
    '    End If


    '    If GraficoData3 <> "" Then
    '        If GraficoData3.Substring(GraficoData3.Length - 1, 1) = "," Then
    '            GraficoData3 = GraficoData3.Substring(0, GraficoData3.Length - 1)
    '        End If
    '    End If
    '    If GraficoData4 <> "" Then
    '        If GraficoData4.Substring(GraficoData4.Length - 1, 1) = "," Then
    '            GraficoData4 = GraficoData4.Substring(0, GraficoData4.Length - 1)
    '        End If
    '    End If

    '    If GraficoData5 <> "" Then
    '        If GraficoData5.Substring(GraficoData5.Length - 1, 1) = "," Then
    '            GraficoData5 = GraficoData5.Substring(0, GraficoData5.Length - 1)
    '        End If
    '    End If

    '    If GraficoData6 <> "" Then
    '        If GraficoData6.Substring(GraficoData6.Length - 1, 1) = "," Then
    '            GraficoData6 = GraficoData6.Substring(0, GraficoData6.Length - 1)
    '        End If
    '    End If

    '    'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "CarregaGrafico();", True)

    '    'Dim Script As String = "dados=500"
    '    'ScriptManager.RegisterStartupScript(Me.upMain, Me.upMain.GetType(), "openWindow", Script, True)
    '    'GraficoData = jsserialize.Serialize(GraficoData)
    '    'GraficoData2 = jsserialize.Serialize(GraficoData2)
    '    Dim Script As String = "CarregaGrafico();"
    '    ScriptManager.RegisterStartupScript(Me.upGrafico, Me.upGrafico.GetType(), "openWindow", Script, True)
    '    'ScriptManager.GetCurrent(Me).RegisterPostBackControl(Me.cmbCentral)

    '    'Me.upMain.
    '    'Me.gvResumoGeral.DataSource = dt
    '    'Me.gvResumoGeral.DataBind()
    'End Sub
    'Protected Sub gvResumoGeral_RowDataBound1(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvResumoGeral.RowDataBound
    '    For Each cell As TableCell In e.Row.Cells
    '        If e.Row.RowType = DataControlRowType.Header Then
    '            If Not String.IsNullOrEmpty(cell.Text.Trim) Then
    '                'cell.Text = Server.HtmlDecode("<a href='Rit.aspx?competencia='" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & " '>" & cell.Text & "</a>")
    '                cell.Text = Server.HtmlDecode("<a href=""RIT.aspx?competencia=" & IIf(Now.Day < 10, "0" & Now.Day, Now.Day) & "/" & cell.Text & "&grupo=" & Request.QueryString("grupo") & """>" & cell.Text & "</a>")
    '            End If
    '        End If

    '        If e.Row.Cells.GetCellIndex(cell) > 0 Then
    '            cell.HorizontalAlign = HorizontalAlign.Right
    '        End If
    '        Dim nomeServico As String = ""
    '        If e.Row.RowType = DataControlRowType.DataRow Then
    '            Dim codigoTipo As String = ""
    '            If Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS MÓVEIS" Then
    '                codigoTipo = 1
    '                nomeServico = " - Linhas Móveis"
    '            ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS FIXAS" Then
    '                codigoTipo = 2
    '                nomeServico = " - Linhas Fixas"
    '            ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "LINHAS 0800" Then
    '                codigoTipo = 4
    '                nomeServico = " - Linhas 0800"
    '            ElseIf Server.HtmlDecode((e.Row.Cells(0).Text)).ToUpper = "NÚMERO ÚNICO" Then
    '                codigoTipo = 6
    '                nomeServico = " - Número Único"
    '            End If

    '            e.Row.Cells(0).Text = "<b><a href='graficoOperServico.aspx?codigoTipo=" & codigoTipo & "&nomeServico=" & nomeServico & "'>" & e.Row.Cells(0).Text & "</a></b>"
    '        End If

    '    Next
    'End Sub


    Private Sub cmbAreaInterna_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAreaInterna.SelectedIndexChanged
        ' CarregaGrupos(IIf(Me.cmbCentral.SelectedValue = "0", "", Me.cmbCentral.SelectedValue), IIf(Me.cmbAreaInterna.SelectedValue = "0", "", Me.cmbAreaInterna.SelectedValue))
    End Sub



    Sub CarregaGastoUltimoMes()

        Dim sql As String = "select * from (select sum(gasto)gasto,data,1 ordem from(" & sqlTotal & ") where data=to_char(add_months(sysdate,-1),'MM/YYYY') group by data "
        sql += " union "
        sql += "select sum(gasto)gasto,data, 2 ordem from(" & sqlTotal & ") where data=to_char(add_months(sysdate,-2),'MM/YYYY') group by data "
        sql += " ) order by ordem"
        Dim dt As DataTable = _dao_commons.myDataTable(sql)

        If dt.Rows.Count > 0 Then
            'gasto atual
            totalMesAtual = dt.Rows(0).Item("gasto")
            MesAtual = dt.Rows(0).Item("data")
            ViewState("vencimento") = Replace(MesAtual, "/", "")
            If dt.Rows.Count > 1 Then
                VariacaoMesAnterior = dt.Rows(0).Item("gasto") - dt.Rows(1).Item("gasto")
            End If

        End If



    End Sub


    Sub TemFaturasCarregadas()

        If _dao_commons.getLabel("EXIBE_TARIFACAO") = "S" Then

            'se nao tiver fatura redireciona p/ tarifação
            Dim sql As String = "select count(*) from faturas"
            Dim dt As DataTable = _dao_commons.myDataTable(sql)
            If dt.Rows(0).Item(0) < 1 Then

                If _dao_commons.Is_Commom_User(Session("codigousuario")) Then
                    'usuario comum
                    Response.Redirect("GastoUsuarioRamal.aspx")
                Else
                    Response.Redirect("GastoUsuarioRamal.aspx?mostraArea=S")
                End If



            End If


        End If

    End Sub

    Private Sub main_Init(sender As Object, e As EventArgs) Handles Me.Init
        If Request.QueryString("logout") = "S" Then
            Session("codigousuario") = Nothing
            Session("usuario") = Nothing
            Response.Redirect("Default.aspx")
        End If
    End Sub
    Sub VerificaSenha()

        Dim sql = "select nvl(trunc(expiracao_senha_web) - trunc(sysdate),'0') from usuarios where codigo='" & Session("codigousuario") & "'"

        'Response.Write(sql)
        'Response.End()
        Dim dt As DataTable = _dao_commons.myDataTable(sql)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item(0) <= 0 Then
                'SENHA WEB EXPIROU
                Response.Redirect("altera_senha.aspx")
                Response.End()
            End If


        End If


    End Sub
End Class
