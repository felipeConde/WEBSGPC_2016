﻿Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Reflection
Imports System.IO
Imports System.Web
Imports System.Web.SessionState
Imports System.Collections.Generic

Public Class DALCGestor
    Inherits Page

    Private Shared strConn As String = ConfigurationManager.ConnectionStrings("connectionString").ConnectionString
    Public Shared Codigousuario As String


    Public Shared Function GetMetaByCcusto(ByVal pCcusto As String) As Double
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim Meta As Double
        Dim strSql As String = ""
        strSql += "Select nvl(sum(p1.credito_mensal),0)meta from ramais p1 "
        If Not String.IsNullOrEmpty(pCcusto) Then
            strSql += " where p1.grp_codigo like '" & pCcusto & "'%"
        End If

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Meta = reader.Item("meta")
            End While
        End Using

        Return Meta

    End Function

    Public Shared Function TemAcessoGestao() As Integer
        '1- administrador geral
        '2- Diretores, Gerentes, Administradores Locais
        Dim session As HttpSessionState = HttpContext.Current.Session

        Dim strSql As String = ""
        strSql += "select * from categoria_usuario where tipo_usuario='A' and codigo_usuario=" + session("codigousuario")

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader

        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            If reader.HasRows Then
                'É administrador geral
                Return 1
            End If
        End Using

        strSql = "select * from categoria_usuario where tipo_usuario in ('A','AL','D','G') and codigo_usuario=" + session("codigousuario")
        cmd.CommandText = strSql
        Dim reader2 As OleDbDataReader
        connection.Open()
        reader2 = cmd.ExecuteReader
        Using connection
            If reader2.HasRows Then
                'Tem acesso
                Return 2
            End If
        End Using

        'não tem acesso
        Return -1

    End Function

    Public Shared Function AcessoAdmin(Optional codigousuario As String = "") As Boolean
        '1- administrador geral
        Dim result As Boolean = False
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim strSql As String = ""
        If codigousuario <> "" Then
            strSql += "select * from categoria_usuario where tipo_usuario='A' and codigo_usuario='" + codigousuario.ToString + "'"
        Else
            strSql += "select * from categoria_usuario where tipo_usuario='A' and codigo_usuario='" + session("codigousuario").ToString + "'"
        End If
        'strSql += "select * from categoria_usuario where tipo_usuario='A' and codigo_usuario='" + Fabrica.usuario.CodigoUsuario.ToString + "'"


        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader

        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            If reader.HasRows Then
                'É administrador geral
                'Return True
                result = True
            End If
        End Using

        Return result

    End Function

    Public Shared Function GetCCustoByUsuario(ByVal pCodigoUsuario As Integer, Optional ByVal area As String = "", Optional ByVal area_interna As String = "") As List(Of CCusto)
        Dim _list As New List(Of CCusto)
        Dim strSql As String = ""
        Dim session As HttpSessionState = HttpContext.Current.Session

        If pCodigoUsuario > -1 Then
            'usuário especifico
            'strSql += "select distinct cat.codigo_grupo,g.nome_grupo from categoria_usuario cat, grupos g where cat.codigo_grupo=g.codigo  and codigo_usuario='" + session("codigousuario").ToString + "' order by g.nome_grupo"
            strSql += "select distinct g.nome_grupo nome_grupo, g.codigo codigo_grupo from grupos g"
            strSql += "  where g.ATIVO='S' and exists(  select 0 from categoria_usuario p100"
            strSql += " where p100.codigo_usuario='" + pCodigoUsuario.ToString + "'"
            strSql += " and to_char(g.codigo) like p100.codigo_grupo||'%' )  "

            If AppIni.GloboRJ_Parm = True Then
                If area <> "" Then
                    strSql += " and g.area='" & area & "'"
                End If
                If area_interna <> "" Then
                    strSql += " and g.area_interna='" & area_interna & "'"
                End If
                strSql += " order by g.codigo "
            Else
                strSql += "order by g.nome_grupo "
            End If

        Else
            'administrador
            strSql += "select g.codigo codigo_grupo,g.nome_grupo from grupos g where  g.ATIVO='S' "

            If AppIni.GloboRJ_Parm = True Then
                strSql += " and 1=1 "
                If area <> "" Then
                    strSql += " and g.area='" & area & "'"
                End If
                If area_interna <> "" Then
                    strSql += " and g.area_interna='" & area_interna & "'"
                End If
                strSql += " order by g.codigo "
            Else
                strSql += "order by g.nome_grupo "
            End If
        End If

        'System.Web.HttpContext.Current.Response.Write(strSql)
        'System.Web.HttpContext.Current.Response.End()

        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader

        connection.Open()
        reader = cmd.ExecuteReader
        Using connection

            While reader.Read
                '_list.Add(New CCusto(reader.Item("codigo_grupo").ToString, reader.Item("codigo_grupo").ToString & "-" & reader.Item("nome_grupo").ToString))
                _list.Add(New CCusto(reader.Item("codigo_grupo").ToString, reader.Item("codigo_grupo").ToString & " - " & reader.Item("nome_grupo").ToString))
            End While
        End Using

        Return _list

    End Function

    Public Shared Function GetMetaByCodigoUsuario() As Double
        Dim session As HttpSessionState = HttpContext.Current.Session

        Dim Meta As Double
        Dim strSql As String = ""
        strSql += "Select nvl(sum(nvl(p1.credito_mensal,0)),0)meta from ramais p1 "
        strSql += "where 1=1 "
        If Not DALCGestor.AcessoAdmin() Then
            'não filtra o centro de custo dos gerentes
            strSql = strSql + " and exists(" & vbNewLine
            strSql = strSql + "   select 0 from categoria_usuario p100" & vbNewLine
            strSql = strSql + "     where p100.codigo_usuario='" & session("codigousuario") & "'" & vbNewLine
            'strSql = strSql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            strSql = strSql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Meta = reader.Item("meta")
            End While
        End Using

        Return Meta

    End Function

    Public Shared Function GetUltimaDataFatura() As String
        Dim session As HttpSessionState = HttpContext.Current.Session

        Dim data As String = ""
        Dim strSql As String = ""
        strSql += "select to_char(nvl(max(dt_referencia),sysdate),'MM/YYYY')dt_referencia from faturas where CODIGO_TIPO='2' "

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                data = reader.Item("dt_referencia")
            End While
        End Using

        Return data

    End Function


    Public Shared Function MaxUltimaDataFatura() As String
        Dim session As HttpSessionState = HttpContext.Current.Session

        Dim data As String = ""
        Dim strSql As String = ""
        strSql += "select to_char(nvl(max(dt_vencimento),sysdate),'DD/MM/YYYY')dt_vencimento from faturas where dt_vencimento<to_date(to_char(sysdate,'MM/YYYY' ),'MM/YYYY') "

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                data = reader.Item("dt_vencimento")
            End While
        End Using

        Return data

    End Function

    Public Shared Function MaxUltimaDataFaturaV2() As String
        Dim session As HttpSessionState = HttpContext.Current.Session

        Dim data As String = ""
        Dim strSql As String = ""
        strSql += "select to_char(nvl(max(dt_vencimento),sysdate),'MM/YYYY')dt_vencimento from faturas where dt_vencimento<=to_date(to_char(sysdate,'MM/YYYY' ),'MM/YYYY') "

        Dim connection As New OleDbConnection(session("conexao"))
        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSql
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                data = reader.Item("dt_vencimento")
            End While
        End Using

        Return data

    End Function


    Public Shared Function getPerfilByCodigoUsuario(pCodigoUsuario As String) As String
        Dim _list As New List(Of CCusto)
        Dim strSql As String = ""
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim _dao_commons As New DAO_Commons
        Dim _perfil As String = ""
        Dim dt As DataTable

        If pCodigoUsuario > -1 Then
            'usuário especifico


            If AppIni.GloboRJ_Parm = True Then

                'strSql += "select distinct cat.codigo_grupo,g.nome_grupo from categoria_usuario cat, grupos g where cat.codigo_grupo=g.codigo  and codigo_usuario='" + session("codigousuario").ToString + "' order by g.nome_grupo"
                strSql = ""
                strSql += "select distinct g.nome_grupo nome_grupo, g.codigo codigo_grupo from grupos g"
                strSql += "  where exists(  select 0 from categoria_usuario p100"
                strSql += " where p100.codigo_usuario='" + session("codigousuario").ToString + "'"
                strSql += " and to_char(g.codigo) like p100.codigo_grupo||'%' )  "

                dt = _dao_commons.myDataTable(strSql)

                If dt.Rows.Count > 0 Then
                    'GERENTE
                    _perfil = dt.Rows(0).Item(0)
                End If

                strSql = ""
                'strSql += "select distinct cat.codigo_grupo,g.nome_grupo from categoria_usuario cat, grupos g where cat.codigo_grupo=g.codigo  and codigo_usuario='" + session("codigousuario").ToString + "' order by g.nome_grupo"
                strSql += "select distinct nvl(g.area_interna,0) area_interna , nvl(g.codigo,0) codigo_grupo from grupos g"
                strSql += "  where exists(  select 0 from categoria_usuario p100"
                strSql += " where p100.codigo_usuario='" + session("codigousuario").ToString + "'"
                strSql += " and to_char(g.codigo) like p100.codigo_grupo||'%' )  "
                strSql += " order by nvl(g.area_interna,0) "

                dt = _dao_commons.myDataTable(strSql)
                If dt.Rows.Count > 0 Then
                    'MAIS DE UM CCUSTO - DIRETOR INTERNA
                    _perfil = dt.Rows(0).Item(0).ToString
                End If
                strSql = ""

                'strSql += "select distinct cat.codigo_grupo,g.nome_grupo from categoria_usuario cat, grupos g where cat.codigo_grupo=g.codigo  and codigo_usuario='" + session("codigousuario").ToString + "' order by g.nome_grupo"
                strSql += "select distinct g.area area , g.area_interna from grupos g"
                strSql += "  where exists(  select 0 from categoria_usuario p100"
                strSql += " where p100.codigo_usuario='" + session("codigousuario").ToString + "'"
                strSql += " and to_char(g.codigo) like p100.codigo_grupo||'%' )  "
                strSql += " order by g.area "

                dt = _dao_commons.myDataTable(strSql)
                If dt.Rows.Count > 0 Then
                    'MAIS DE UMA ÁREA INTERNA - DIRETOR DE AREA
                    _perfil = dt.Rows(0).Item(0)
                End If



            Else
                strSql = ""
                'strSql += "select distinct cat.codigo_grupo,g.nome_grupo from categoria_usuario cat, grupos g where cat.codigo_grupo=g.codigo  and codigo_usuario='" + session("codigousuario").ToString + "' order by g.nome_grupo"
                strSql += "select distinct g.nome_grupo nome_grupo, g.codigo codigo_grupo from grupos g"
                strSql += "  where exists(  select 0 from categoria_usuario p100"
                strSql += " where p100.codigo_usuario='" + session("codigousuario").ToString + "'"
                strSql += " and to_char(g.codigo) like p100.codigo_grupo||'%' ) and rownum  "
                strSql += " order by g.codigo "

                dt = _dao_commons.myDataTable(strSql)
                If dt.Rows.Count > 0 Then
                    _perfil = dt.Rows(0).Item(0)
                End If
            End If


        End If

        Return _perfil


    End Function

End Class
