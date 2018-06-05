Imports System.Net
Imports System.Web.Http
Imports System.Web.HttpContext
Imports System.Collections.Generic
Imports System.Data
Imports System.Web.SessionState

Public Class HomeController
    Inherits ApiController
    Private _dao_commons As New DAO_Commons
    Private _dao As New DAO_Dashboard



    Partial Public Class Items
        Private _descricao As String
        Private _valor As String
        Public Property Descricao As String
            Get
                Return _descricao
            End Get
            Set(value As String)
                _descricao = value
            End Set
        End Property

        Public Property Valor As String
            Get
                Return _valor
            End Get
            Set(value As String)
                _valor = value
            End Set
        End Property

        Public Sub New()

        End Sub

        Public Sub New(pDescricao As String, pvalor As String)
            _descricao = pDescricao
            _valor = pvalor
        End Sub
    End Class

    ' GET api/<controller>

    '
    <HttpGet>
    Public Function GetTop10Movel(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim dt As DataTable = _dao.GetTop10("1", vencimento, DAO_Commons.PrepareString(grupo), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna), DAO_Commons.PrepareString(codigousuario))


        Dim myItems As New List(Of Items)

        'preenche a lista
        For Each _row As DataRow In dt.Rows
            myItems.Add(New Items(_row.Item("usuario"), Microsoft.VisualBasic.Strings.FormatCurrency(_row.Item("gasto"))))
        Next

        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function

    <HttpGet>
    Public Function GetTop10Fixo(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim dt As DataTable = _dao.GetTop10("2", vencimento, DAO_Commons.PrepareString(grupo), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna), DAO_Commons.PrepareString(codigousuario))


        Dim myItems As New List(Of Items)

        'preenche a lista
        For Each _row As DataRow In dt.Rows
            myItems.Add(New Items(_row.Item("usuario"), Microsoft.VisualBasic.Strings.FormatCurrency(_row.Item("gasto"))))
        Next

        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function

    <HttpGet>
    Public Function GetTop10Ramais(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim dt As DataTable = _dao.GetTop10Ramal("", vencimento, DAO_Commons.PrepareString(grupo), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna), DAO_Commons.PrepareString(codigousuario))


        Dim myItems As New List(Of Items)

        'preenche a lista
        For Each _row As DataRow In dt.Rows
            myItems.Add(New Items(_row.Item("usuario"), Microsoft.VisualBasic.Strings.FormatCurrency(_row.Item("gasto"))))
        Next

        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function

    <HttpGet>
    Public Function ExibeTarifacao() As Boolean
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Return _dao.ExibeTarifacao()
    End Function

    <HttpGet>
    Public Function GetServicosMes(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim dt As DataTable = _dao.GetServicosMes("2", vencimento, DAO_Commons.PrepareString(grupo), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna), DAO_Commons.PrepareString(codigousuario))


        Dim myItems As New List(Of Items)

        'preenche a lista
        For Each _row As DataRow In dt.Rows
            myItems.Add(New Items(_row.Item("tarifa") & " (" & _row.Item("tipo") & ")", Microsoft.VisualBasic.Strings.FormatCurrency(_row.Item("gasto"))))
        Next

        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function

    <HttpGet>
    Public Function GetAreas(codigousuario As String) As List(Of AppGeneric)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim myItems As List(Of AppGeneric) = _dao.CarregaAreas(DAO_Commons.PrepareString(codigousuario))


        Return myItems


    End Function

    <HttpGet>
    Public Function GetAreasInternas(codigousuario As String, Optional area As String = "") As List(Of AppGeneric)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim myItems As List(Of AppGeneric) = _dao.CarregaAreasInternas(DAO_Commons.PrepareString(codigousuario), DAO_Commons.PrepareString(area))


        Return myItems


    End Function

    <HttpGet>
    Public Function GetGrupos(codigousuario As String, Optional area As String = "", Optional areaInterna As String = "") As List(Of CCusto)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = Current.Session
        Dim myItems As List(Of CCusto) = _dao.CarregaGrupos(DAO_Commons.PrepareString(codigousuario), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna))


        Return myItems


    End Function

    <HttpGet>
    Public Function getUsuariosPerfilOld(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = HttpContext.Current.Session
        Dim dt As DataTable = _dao.getUsuariosPerfis(vencimento, grupo, area, areaInterna, codigousuario)
        Dim codigoUsuariosDiretores As String = ""
        Dim codigoUsuariosGerenteCentral As String = ""
        Dim codigoUsuariosGerenteCC As String = ""
        Dim codigoUsuarioFuncionarios As String = ""
        Dim qtdTotal As Integer = 0

        Dim myItems As New List(Of Items)

        'preenche a lista
        For Each _row As DataRow In dt.Rows

            If _row.Item("nivel") = "3" Then
                'diretor de central
                'myItems.Add(New Items(_row.Item("codigo") & " (" & _row.Item("nome_usuario") & ")", _row.Item("total")))
                codigoUsuariosDiretores += _row.Item("codigo") & ","
            End If
            If _row.Item("nivel") = "2" Then
                'diretor de interna
                'myItems.Add(New Items(_row.Item("codigo") & " (" & _row.Item("nome_usuario") & ")", _row.Item("total")))
                codigoUsuariosGerenteCentral += _row.Item("codigo") & ","
            End If
            If _row.Item("nivel") = "1" Then
                'diretor de ar
                'myItems.Add(New Items(_row.Item("codigo") & " (" & _row.Item("nome_usuario") & ")", _row.Item("total")))
                codigoUsuariosGerenteCC += _row.Item("codigo") & ","
            End If
            If _row.Item("nivel") = "0" Then
                'funcionarios
                'myItems.Add(New Items(_row.Item("codigo") & " (" & _row.Item("nome_usuario") & ")", _row.Item("total")))
                codigoUsuarioFuncionarios += _row.Item("codigo") & ","
            End If
        Next

        If codigoUsuariosDiretores.Length > 0 Then
            'tira a virgula do final
            codigoUsuariosDiretores = codigoUsuariosDiretores.Substring(0, codigoUsuariosDiretores.Length - 1)
            myItems.Add(New Items("Diretores", FormatNumber(_dao.getTotalLinhasusuarios(codigoUsuariosDiretores, codigousuario).Rows(0).Item(0), 0)))
            qtdTotal += _dao.getTotalLinhasusuarios(codigoUsuariosDiretores, codigousuario).Rows(0).Item(0)
        End If
        If codigoUsuariosGerenteCentral.Length > 0 Then
            'tira a virgula do final
            codigoUsuariosGerenteCentral = codigoUsuariosGerenteCentral.Substring(0, codigoUsuariosGerenteCentral.Length - 1)
            myItems.Add(New Items("Gerentes (Área Interna) ", FormatNumber(_dao.getTotalLinhasusuarios(codigoUsuariosGerenteCentral, codigousuario).Rows(0).Item(0), 0)))
            qtdTotal += _dao.getTotalLinhasusuarios(codigoUsuariosGerenteCentral, codigousuario).Rows(0).Item(0)
        End If
        If codigoUsuariosGerenteCC.Length > 0 Then
            'tira a virgula do final
            codigoUsuariosGerenteCC = codigoUsuariosGerenteCC.Substring(0, codigoUsuariosGerenteCC.Length - 1)
            myItems.Add(New Items("Gerentes (Ar) ", FormatNumber(_dao.getTotalLinhasusuarios(codigoUsuariosGerenteCC, codigousuario).Rows(0).Item(0), 0)))
            qtdTotal += _dao.getTotalLinhasusuarios(codigoUsuariosGerenteCC, codigousuario).Rows(0).Item(0)
        End If
        If codigoUsuarioFuncionarios.Length > 0 Then
            'tira a virgula do final
            Dim valor As Integer = _dao.getTotalLinhasusuarios("", codigousuario, "S").Rows(0).Item(0)
            codigoUsuarioFuncionarios = codigoUsuarioFuncionarios.Substring(0, codigoUsuarioFuncionarios.Length - 1)
            myItems.Add(New Items("Funcionários ", FormatNumber(valor, 0)))
            qtdTotal += FormatNumber(valor, 0)
        End If


        myItems.Add(New Items("Total ", FormatNumber(qtdTotal, 0)))


        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function


    <HttpGet>
    Public Function getUsuariosPerfil(vencimento As String, grupo As String, area As String, areaInterna As String, codigousuario As String) As List(Of Items)
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}
        Dim session As HttpSessionState = Current.Session
        Dim dt As DataTable = _dao.getLinhasPerfil(vencimento, DAO_Commons.PrepareString(grupo), DAO_Commons.PrepareString(area), DAO_Commons.PrepareString(areaInterna), DAO_Commons.PrepareString(codigousuario))
        Dim myItems As New List(Of Items)

        For Each _row As DataRow In dt.Rows
            myItems.Add(New Items(_row.Item(0), FormatNumber(_row.Item(1), 0)))
        Next

        'myItems.Add(New Items(grupo, 12456))

        Return myItems



    End Function

    <HttpGet>
    Public Function getLabel(campo As String) As String
        'Return New String() {"value1", "value2"}
        'Return New String() {ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString}

        'Dim dt As DataTable = 
        Dim myItems As List(Of AppGeneric) = _dao_commons.GetGenericList(campo, "NOME_PARAMETRO", "VALOR_PARAMETRO", "PARAMETROS_SGPC")



        'myItems.Add(New Items(grupo, 12456))
        If myItems.Count > 0 Then
            Return myItems.Item(0).Descricao.ToString
        Else
            Return ""
        End If





    End Function




End Class
