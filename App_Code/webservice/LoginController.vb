﻿Imports System.Collections.Generic
Imports System.Net
Imports System.Web
Imports System.Web.Http
Imports System.Web.HttpContext
Imports System.Web.SessionState
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Data


Public Class LoginController
    Inherits ApiController

    Dim _dao As New DAOUsuarios
    Dim _daoCommons As New DAO_Commons
    Dim session As HttpSessionState = HttpContext.Current.Session

    ' GET api/<controller>
    <HttpGet>
    Public Function Index(ByVal id As String) As IEnumerable(Of String)
        Return New String() {"Você entrou no Index - " & id}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
    End Function

    ' GET api/<controller>
    <HttpGet>
    Public Function GetValues() As IEnumerable(Of String)
        Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
    End Function

    ' GET api/<controller>/5
    <HttpGet>
    Public Function GetValue(ByVal id As Integer) As String
        Return "value"
    End Function

    ' POST api/<controller>
    <HttpPost>
    Public Function PostValue(<FromBody()> ByVal usuario As AppUsuarios) As IHttpActionResult
        Dim _usuario As AppUsuarios = Nothing
        If Not usuario Is Nothing Then




            usuario.Login_Usuario = DAO_Commons.PrepareString(usuario.Login_Usuario)
            'usuario.Senha_Usuario = DAO_Commons.PrepareString(usuario.Senha_Usuario)


            Dim dt As DataTable
            dt = _daoCommons.myDataTable("select t.valor_parametro from PARAMETROS_SGPC t where t.nome_parametro='HOST_INTEGRACAO_AD' ")
            If dt.Rows.Count > 0 Then
                'AuthenticateUserAD("LDAP://ad_jgs.weg.net", usuario.Login_Usuario, usuario.Senha_Usuario)
                Dim HOST As String = dt.Rows(0).Item("valor_parametro").ToString.Trim
                Dim _login As String = AuthenticateUserAD("LDAP://" & HOST, usuario.Login_Usuario.Trim, usuario.Senha_Usuario.Trim)
                If _login <> "" Then
                    usuario.Login_Usuario = _login
                    usuario.Senha_Usuario = "AUTOLOGON"
                    usuario.AD = True
                End If
            End If


            _usuario = _dao.LoginV2(usuario.Login_Usuario.Trim, usuario.Senha_Usuario.Trim, usuario.AD)
        End If


        If Not _usuario Is Nothing Then

            'session.Add("codigousuario", _usuario.codigo)
            'session("nomeusuario") = _usuario.Nome_Usuario
            'OK
            'Session("FirstName") = _usuario.Codigo
            Return Ok(_usuario)
        Else
            'Return httpres
            Return NotFound()
        End If

    End Function

    ' PUT api/<controller>/5
    Public Sub PutValue(ByVal id As Integer, <FromBody()> ByVal value As String)

    End Sub

    ' DELETE api/<controller>/5
    Public Sub DeleteValue(ByVal id As Integer)

    End Sub

    Function AuthenticateUserAD(ByVal path As String, ByVal user As String, ByVal pass As String) As String
        Dim de As New DirectoryEntry(path, user, pass)


        Try
            'run a search using those credentials.  
            'If it returns anything, then you're authenticated
            Dim ds As DirectorySearcher = New DirectorySearcher(de)
            Dim myResult As DirectoryServices.SearchResult
            Dim filterString As String = "(sAMAccountName=" + user + ")"
            'Dim filterString As String = "(userPrincipalName= " + user + ")"

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
