Imports System.Collections.Generic
Imports System.Security.Claims
Imports System.Security.Principal
Imports System.Web
Imports System.Web.Security
Imports System.Web.UI
Imports System.Web.UI.WebControls
Imports System.Data

Public Partial Class SiteMaster
    Inherits MasterPage
    Private Const AntiXsrfTokenKey As String = "__AntiXsrfToken"
    Private Const AntiXsrfUserNameKey As String = "__AntiXsrfUserName"
    Private _antiXsrfTokenValue As String
    Dim _dao_commons As New DAO_Commons
    Private _dao As New DAO_Menu
    Public strMenu As String = ""
    Public myUrl As String = ""
    Public _googleAnalytics As String = ""
    Public _totalAvisos As String = ""
    Public _scriptAvisos As String = ""
    Public url_agenda As String = ""
    Public url_tel_contato As String = ""
    Private image As Byte()
    Public Image_Url As String

    Protected Sub Page_Init(sender As Object, e As EventArgs)
        ' The code below helps to protect against XSRF attacks
        Dim requestCookie = Request.Cookies(AntiXsrfTokenKey)
        Dim requestCookieGuidValue As Guid
        If requestCookie IsNot Nothing AndAlso Guid.TryParse(requestCookie.Value, requestCookieGuidValue) Then
            ' Use the Anti-XSRF token from the cookie
            _antiXsrfTokenValue = requestCookie.Value
            Page.ViewStateUserKey = _antiXsrfTokenValue
        Else
            ' Generate a new Anti-XSRF token and save to the cookie
            _antiXsrfTokenValue = Guid.NewGuid().ToString("N")
            Page.ViewStateUserKey = _antiXsrfTokenValue

            Dim responseCookie = New HttpCookie(AntiXsrfTokenKey) With {
                .HttpOnly = True,
                .Value = _antiXsrfTokenValue
            }
            If FormsAuthentication.RequireSSL AndAlso Request.IsSecureConnection Then
                responseCookie.Secure = True
            End If
            Response.Cookies.[Set](responseCookie)
        End If

        AddHandler Page.PreLoad, AddressOf master_Page_PreLoad


        getGoogleAnalytics()

    End Sub

    Protected Sub master_Page_PreLoad(sender As Object, e As EventArgs)
        myUrl = Request.Url.AbsoluteUri.Substring(0, Request.Url.AbsoluteUri.LastIndexOf("/")) & "/"
        If Not IsPostBack Then
            ' Set Anti-XSRF token
            ViewState(AntiXsrfTokenKey) = Page.ViewStateUserKey
            ViewState(AntiXsrfUserNameKey) = If(Context.User.Identity.Name, [String].Empty)
        Else
            ' Validate the Anti-XSRF token
            If DirectCast(ViewState(AntiXsrfTokenKey), String) <> _antiXsrfTokenValue OrElse DirectCast(ViewState(AntiXsrfUserNameKey), String) <> (If(Context.User.Identity.Name, [String].Empty)) Then
                Throw New InvalidOperationException("Validation of Anti-XSRF token failed.")
            End If
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs)



        If Session("codigousuario") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

        Dim menu As New AppMenu(Session("codigousuario"))
        Session("conexao") = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
        _dao.strConn = Session("conexao")
        menu.lcid = "1046"
        Session("lcid") = menu.lcid
        _dao.GetMenuData(menu)
        Session("DescIdioma") = menu.DescIdioma
        Session("IdIdioma") = menu.IdIdioma

        'vamos ver o perfil
        If Not DALCGestor.AcessoAdmin() Then
            Session("perfil") = DALCGestor.getPerfilByCodigoUsuario(Session("codigousuario"))
        End If


        MontaMenu(menu)
        mostraAviso()
        ExibeLinksTopo()
        CerregaFotoPerfil()

    End Sub

    Protected Sub Unnamed_LoggingOut(sender As Object, e As LoginCancelEventArgs)
        Context.GetOwinContext().Authentication.SignOut()
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="menu_data"></param>
    Protected Sub MontaMenu(ByVal menu_data As AppMenu)

        Dim TableMenu As New DataTable()
        Dim TableMenu2 As New DataTable()
        Dim TableMenu3 As New DataTable()
        Dim myurl As String
        Dim idMenu As String
        Dim url As String
        Dim codigoPai As String
        TableMenu = _dao.MontaMenu(menu_data)

        If _dao_commons.Is_Commom_User(Session("codigousuario")) Then
            ViewState("usuarioComum") = "1"
        End If

        strMenu += ""

        'strMenu += "<div id='menu' runat='server'>"
        'strMenu += "<ul class='tabs'>"

        'MENU HOME
        If ViewState("usuarioComum") <> "1" Then
            strMenu += "<li class='active'><a href='main.aspx' ><i class='zmdi zmdi-view-dashboard'></i><span><b>Home</b></span></a></li>"
        End If

        strMenu += "<li class='active'><a href='GastoUsuario.aspx' ><i class='zmdi zmdi-accounts-list'></i><span><b>Meus Gastos</b></span></a></li>"

        'usuário comum só ve o proprio gasto
        If ViewState("usuarioComum") <> "1" Then
            strMenu += "<li class='active'><a href='GastoUsuario.aspx?mostraArea=S' ><i class='zmdi zmdi-chart'></i><span><b>Minha Área</b></span></a></li>"


            strMenu += "<li class='sub-menu' id='divInventario'><a href=''><i class='zmdi zmdi-smartphone-android'></i><span><b>Inventário</b></span></a>"
            strMenu += "<ul> "
            If TemFaturasCarregadas() Then
                strMenu += "<li><a href='GestaoAparelhosMoveis.aspx?' >Aparelhos Móveis</a></li>"
            End If
            If temRamais() Then
                strMenu += "<li><a href='GestaoRamais.aspx?' >Ramais</a></li>"
            End If
            strMenu += "</ul> "
            strMenu += "</li> "


            'Response.Write("<li><a href='" & Request.Url.PathAndQuery & "?" & Request.ServerVariables("QUERY_STRING") & "'><span>Home</span></a></li>")
            'Do While Not TableMenu.eof

            'For Each Row As DataRow In TableMenu.Rows

            'If Row.Item("url") <> "" And Row.Item("url") <> "#" And Row.Item("url") <> "menuright2.asp" Then
            'myurl = Row.Item("url")
            'Else
            '   myurl = "#"
            'End If

            'strMenu += "<li  class='sub-menu'><a href=" + CStr(myurl) + "><i class='" & Row.Item("icon") & "'></i><span><b>" & Row.Item("label") & "</b></span></a>"

            'ConstroiArvore(_dao_commons.GetGenericList("", "codigo", "nome", "relatorios", "", " and id_menu ='" & Row("id") & "'").Item(0).Codigo)

            'strMenu += "</li>"
            'Next

        End If

                    Dim dt As DataTable = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='URL_FORM_NORMAS' and t.valor_parametro is not null ")
        If dt.Rows.Count > 0 Then
            strMenu += "<li class='active'><a href='" & dt.Rows(0).Item(0) & "'><i class='zmdi zmdi-border-color'></i><span><b>Formulários e Normas</b></span></a></li>"
        End If

        dt = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='URL_PORT_SERVICOS' and t.valor_parametro is not null  ")
        If dt.Rows.Count > 0 Then
            strMenu += "<li class='sub-menu'  id='divPortifolio'><a href=''><i class='zmdi zmdi-smartphone'></i><span><b>Portfolio de Serviços</b></span></a>"
            strMenu += "<ul> "
            strMenu += "<li><a href='" & dt.Rows(0).Item(0) & "'>Aparelhos Móveis</a></li>"
            dt = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='URL_PORT_SERVICOS_RAMAIS' and t.valor_parametro is not null  ")
            If dt.Rows.Count > 0 Then
                strMenu += "<li><a href='" & dt.Rows(0).Item(0) & "'>Ramais</a></li>"
            End If
            strMenu += "</ul> "
                strMenu += "</li> "
            End If

            dt = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='CONSUMO_SITE_OPER' and t.valor_parametro is not null")
        If dt.Rows.Count > 0 Then
            strMenu += "<li class='active'><a href='" & dt.Rows(0).Item(0) & "'><i class='zmdi zmdi-trending-up zmdi-hc-fw'></i><span><b>Verificar consumo de dados</b></span></a></li>"
        End If

        dt = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='ALTERA_SENHA' and t.valor_parametro='N'")
        If dt.Rows.Count < 1 Then
            strMenu += " <li class='active'><a href = 'altera_senha.aspx'><i Class='zmdi zmdi-key zmdi-hc-fw'></i> Alterar Senha</a> </li> "
        End If
        'Response.Write("<li  id='ENCERRAR' class='hasmore'><a id='btnEncerrar'" + "a href='javascript:Logout();' ><b><span>Encerrar</span></b></a> </li>")
        'strMenu += "</li>"

        'strMenu += "</ul>"
        'strMenu += "</div>"

    End Sub

    Protected Sub ConstroiArvore(ByVal id_parent As String)
        Dim list_child As New List(Of AppGeneric)

        If id_parent = "94" Then
            Dim ONE As Integer = 1
        End If

        list_child = _dao_commons.GetGenericList("", "codigo", "nome", "relatorios", "", " and id_parent ='" & id_parent & "' order by nome")

        strMenu += "<ul>"

        For Each item As AppGeneric In list_child

            Dim url As String = _dao_commons.GetGenericList(item.Codigo, "codigo", "url", "relatorios").Item(0).Descricao

            'força a tralha p/ demo
            'url = "#"

            If url <> "#" Then

                If item.Descricao.Length <= 28 Then
                    strMenu += "<li  id='" & item.Codigo & "' ><a href='" + HttpContext.Current.Request.ApplicationPath + "/" + CStr(url).Replace("xml/", "") + "' >" & item.Descricao & "</a>"
                Else
                    Dim descricao As String = ""
                    Dim aux As String() = item.Descricao.Split(" ")
                    Dim count As Integer = 0
                    For Each palavra As String In aux
                        If count = 0 Then
                            descricao = palavra
                        ElseIf count = 3 Then
                            descricao = descricao + "</br> " + palavra
                        Else
                            descricao = descricao + " " + palavra
                        End If
                        count = count + 1
                    Next

                    strMenu += "<li  id='" & item.Codigo & "' ><a href='" + HttpContext.Current.Request.ApplicationPath + "/" + CStr(url).Replace("xml/", "") + "'  > " & descricao & "</a>"
                End If
                strMenu += "</li>"
            Else
                'expande submenu
                strMenu += "<li  class='sub-menu' id='" & item.Codigo & "'  ><a href='javascript:void(0);'><b>" & item.Descricao & "</b></a>"
                ConstroiArvore(item.Codigo)
                strMenu += "</li>"
            End If
        Next

        strMenu += "</ul>"

    End Sub

    Private Sub SiteMaster_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Session("codigousuario") Is Nothing Or Session("usuario") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If

    End Sub

    Private Sub SiteMaster_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Session("codigousuario") Is Nothing Or Session("usuario") Is Nothing Then
            Response.Redirect("Default.aspx")
        End If
    End Sub

    Public Sub getGoogleAnalytics()
        'Dim myItems As List(Of AppGeneric) = _dao_commons.GetGenericList("ANALYTICS_GESTAO", "NOME_PARAMETRO", "VALOR_PARAMETRO", "PARAMETROS_SGPC")
        'If myItems.Count > 0 Then
        '    Dim _code As String = myItems.Item(0).Descricao.ToString

        '    'Page.ClientScript.RegisterStartupScript(Type.GetType("System.String"), "addScript", _code, True)
        '    _googleAnalytics = _code



        'End If
        _googleAnalytics = GetGoogleCode()

    End Sub


    Private Function GetGoogleCode() As String
        ' _dao_commons.strConn = System.Web.HttpContext.Current.Session("conexao").ToString


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

    Sub ExibeLinksTopo()
        'link da agenda
        Dim dt As DataTable = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='URL_AGENDA_CORP'")
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item(0).ToString.Trim <> "" Then
                url_agenda = dt.Rows(0).Item(0).ToString.Trim
                Me.divAgenda.Visible = True
            Else
                Me.divAgenda.Visible = False
            End If
        Else
            Me.divAgenda.Visible = False
        End If

        'link do contato
        dt = _dao_commons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where upper(t.nome_parametro)='URL_TEL_SUPORTE'")
        If dt.Rows.Count > 0 Then
            If dt.Rows(0).Item(0).ToString.Trim <> "" Then
                url_tel_contato = dt.Rows(0).Item(0).ToString.Trim
                Me.divContato.Visible = True
            Else
                Me.divContato.Visible = False
            End If
        Else
            Me.divContato.Visible = False
        End If
    End Sub


    Sub mostraAviso()
        Dim script As String = ""
        Dim msg As String = ""



        'Dim sql As String = "select t.aviso, nvl(t.tipo,1)tipo from gestao_avisos t where nvl(t.lido,'N')<>'S'"
        Dim sql As String = "select * from(select t.codigo,t.aviso, nvl(t.tipo,1)tipo from gestao_avisos t"
        sql += "  where not exists( select * from GESTAO_AVISOS_USUARIOS p1 where p1.codigo_usuario='" & Session("codigousuario") & "' and p1.codigo_aviso=t.codigo  and nvl(p1.lido,'N')<>'N')"
        sql += "  and t.categoria in(select c.tipo_usuario from CATEGORIA_USUARIO c where c.codigo_usuario='" & Session("codigousuario") & "') order by t.codigo desc) where rownum<=10"
        Dim dt As DataTable = _dao_commons.myDataTable(sql)

        'Response.Write(sql)
        'Response.End()

        For Each _row As DataRow In dt.Rows
            Dim tipo As String = "warning"

            Select Case _row.Item("tipo")
                Case "1"
                    tipo = "warning"
                Case "2"
                    tipo = "success"
                Case "3"
                    tipo = "error"
                Case Else
                    tipo = "warning"
            End Select

            'script += "Lobibox.notify('" & tipo & "', {msg:'" & _row.Item("aviso").ToString & "', delay: '15000'});"
            script += "<a class='lv-item' href=''> "
            script += " <div class='media'>"
            script += "  <div class='media-body'>"
            script += " <div class='lv-title'>" & tipo & "</div>"
            script += " <small class='lv-small'>" & _row.Item("aviso").ToString & "</small>"
            script += " </div></div></a>"


            If _row.Item("tipo") = "2" Then
                'se não for aviso de erros já coloca como lido
                _dao_commons.ExecuteSQLCommand("insert into GESTAO_AVISOS_USUARIOS(codigo_aviso,codigo_usuario,lido) values ('" & _row.Item("codigo").ToString & "','" & Session("codigousuario") & "','S')")
            Else

                _dao_commons.ExecuteSQLCommand("insert into GESTAO_AVISOS_USUARIOS(codigo_aviso,codigo_usuario,lido) values ('" & _row.Item("codigo").ToString & "','" & Session("codigousuario") & "','N')")
            End If
        Next
        _scriptAvisos = script
        If _scriptAvisos <> "" Then
            divAvisos.visible = True
            _totalAvisos = dt.Rows.Count
        End If
        'If dt.Rows.Count > 0 Then
        '    '_daoCommons.ExecuteSQLCommand("update GESTAO_AVISOS t set t.lido='S' where nvl(t.lido,'N')='N'")

        'End If


        'ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", script & "</script>")
    End Sub

    Function TemFaturasCarregadas() As Boolean

        If _dao_commons.getLabel("EXIBE_TARIFACAO") = "S" Then

            'se nao tiver fatura redireciona p/ tarifação
            Dim sql As String = "select count(*) from faturas"
            Dim dt As DataTable = _dao_commons.myDataTable(sql)
            If dt.Rows(0).Item(0) < 1 Then

                'If _dao_commons.Is_Commom_User(Session("codigousuario")) Then
                '    'usuario comum
                '    Response.Redirect("GastoUsuarioRamal.aspx")
                'Else

                'End If
                Return False
            Else
                Return True
            End If


        End If
        Return True
    End Function

    Function temRamais() As Boolean

        If _dao_commons.getLabel("EXIBE_TARIFACAO") = "S" Then

            'se nao tiver fatura redireciona p/ tarifação
            Dim sql As String = "select count(*) from ramais"
            Dim dt As DataTable = _dao_commons.myDataTable(sql)
            If dt.Rows(0).Item(0) < 1 Then

                'If _dao_commons.Is_Commom_User(Session("codigousuario")) Then
                '    'usuario comum
                '    Response.Redirect("GastoUsuarioRamal.aspx")
                'Else

                'End If
                Return False
            Else
                Return True
            End If


        End If
        Return True
    End Function

    Sub CerregaFotoPerfil()
        Dim _bytes As New List(Of Byte())

        Try
            _dao_commons.GetBytesByField(Session("codigousuario"), "codigo", "usuarios", "FOTO", _bytes)

            '****************************************************************************

            Dim count As Integer = 0
            Dim list_produtos_Bytes As New List(Of Byte())
            Dim list_fatura_name As New List(Of String)

            For Each bt As Byte() In _bytes
                image = bt
                Dim base64String As String = Convert.ToBase64String(image, 0, image.Length)
                foto.ImageUrl = Convert.ToString("data:image/png;base64,") & base64String
                foto.Visible = True
                noImage.Visible = False
            Next

        Catch ex As Exception

        End Try
    End Sub


End Class
