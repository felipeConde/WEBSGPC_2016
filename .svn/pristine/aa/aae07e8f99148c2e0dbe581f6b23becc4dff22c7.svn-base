Imports System.Data

Partial Public Class Cadastros
    Inherits System.Web.UI.MasterPage


    Public _googleAnalytics As String = ""
    Dim _dao_commons As New DAO_Commons


    Sub getGoogleAnalytics()
        'Dim myItems As List(Of AppGeneric) = _dao_commons.GetGenericList("ANALYTICS_GESTAO", "NOME_PARAMETRO", "VALOR_PARAMETRO", "PARAMETROS_SGPC")
        'If myItems.Count > 0 Then
        '    Dim _code As String = myItems.Item(0).Descricao.ToString

        '    'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", _code, True)
        '    _googleAnalytics = _code


        'End If

        _googleAnalytics = GetGoogleCode()

    End Sub

    Private Sub Cadastros_Load(sender As Object, e As EventArgs) Handles Me.Load
        getGoogleAnalytics()
    End Sub

    Private Function GetGoogleCode() As String
        '_dao_commons.strConn = System.Web.HttpContext.Current.Session("conexao").ToString


        Dim googleKey As String = ""
        Dim strScript As String = ""
        Dim dt As DataTable = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='GOOGLEKEY'")
        If dt.Rows.Count > 0 Then
            googleKey = dt.Rows(0).Item(0).ToString

            strScript += " <script>" & vbNewLine
            'strScript += " //google analytics" & vbNewLine
            strScript += " (function (i, s, o, g, r, a, m) {" & vbNewLine
            strScript += "i['GoogleAnalyticsObject'] = r; i[r] = i[r] || function () {" & vbNewLine
            strScript += "(i[r].q = i[r].q || []).push(arguments)" & vbNewLine
            strScript += "}, i[r].l = 1 * new Date(); a = s.createElement(o)," & vbNewLine
            strScript += " m = s.getElementsByTagName(o)[0]; a.async = 1; a.src = g; m.parentNode.insertBefore(a, m)" & vbNewLine
            strScript += "})(window, document, 'script', '//www.google-analytics.com/analytics.js', 'ga');" & vbNewLine

            strScript += "ga('create', '" & googleKey & "', 'clconsult.com.br');" & vbNewLine
            strScript += "ga('send', 'pageview');" & vbNewLine

            strScript += "</script>" & vbNewLine

            'Return strScript

            'System.Web.HttpContext.Current.Response.Write(strScript)
            'System.Web.HttpContext.Current..ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScriptGoogle", Script, False)
            'System.Web.UI.Page.

        End If
        Return strScript
    End Function

End Class

