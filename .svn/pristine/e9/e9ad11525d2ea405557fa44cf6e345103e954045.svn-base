Imports System.Web.UI
Imports System.Data
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Collections

Partial Class _Default
    Inherits Page
    Public loginAD As String = ""
    Public senhaAD As String = ""
    Public _googleAnalytics As String = ""
    Dim _dao_commons As New DAO_Commons


    Private Sub _Default_Load(sender As Object, e As EventArgs) Handles Me.Load
        'autoLogon
        'Response.End()
        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", "autoLogon();", True)
        '        Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addBemvindo", "autoLogon()", True)

        getGoogleAnalytics()
    End Sub

    Private Sub _Default_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete

        If Not String.IsNullOrEmpty(Request.Form("AD")) Then
            If Request.Form("AD").ToString = "true" Then
                'AD LOGIN AUTOMATICO
                loginAD = Request.Form("txtusername")
                senhaAD = Request.Form("txtpassword")

                'loginAD = "administrador"
                ' senhaAD = "AUTOLOGON"

                Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addBemvindo", "autoLogon()", True)
            End If
        Else
            'se vier do https e for globosite veio do ad(vamos redirecionar)
            Dim url As String = Request.Url.AbsoluteUri
            'Response.Write(url)
            If ConfigurationManager.AppSettings("urlAD").ToString <> "" Then
                If url.Contains("https") And url.Contains("globosite") Then

                    Response.Redirect(ConfigurationManager.AppSettings("urlAD"))

                End If
            End If


        End If


    End Sub

    Private Sub _Default_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addBemvindo", "autoLogon()", True)
        'Response.Write("<script>autoLogon();</script>")
    End Sub
    Sub getGoogleAnalytics()
        'Dim myItems As List(Of AppGeneric) = _dao_commons.GetGenericList("ANALYTICS_GESTAO", "NOME_PARAMETRO", "VALOR_PARAMETRO", "PARAMETROS_SGPC")
        'If myItems.Count > 0 Then
        '    Dim _code As String = myItems.Item(0).Descricao.ToString

        '    'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", _code, True)
        '    _googleAnalytics = _code


        'End If
        _googleAnalytics = GetGoogleCode()

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

    Function AuthenticateUserAD(ByVal path As String, ByVal user As String, ByVal pass As String) As String
        Dim de As New DirectoryEntry(path, user, pass)


        Try
            'run a search using those credentials.  
            'If it returns anything, then you're authenticated
            Dim ds As DirectorySearcher = New DirectorySearcher(de)
            Dim myResult As DirectoryServices.SearchResult
            Dim filterString As String = "(sAMAccountName=" + user + ")"
            'Dim filterString As String = "(userPrincipalName=" + user + ")"

            ds.Filter = filterString
            ds.PropertiesToLoad.Add("cn")
            myResult = ds.FindOne()
            '
            Dim myLogin As String = ""
            'myLogin = myResult.Properties("cn").Item(0).ToString()
            'Dim myLogin As String = myResult.Properties("userPrincipalName").Item(0).ToString()
            'Response.Write(myLogin)

            'Return True
            'Return myLogin
            Return user
        Catch ex As Exception
            'Response.Write(ex.Message)
            'otherwise, it will crash out so return false
            Return ""
        End Try
    End Function

End Class