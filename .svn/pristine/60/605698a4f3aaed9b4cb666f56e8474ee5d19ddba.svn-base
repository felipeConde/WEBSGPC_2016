Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.Globalization
Imports System.Drawing

Partial Class RamalDetalhe
    Inherits System.Web.UI.Page

    Private _dao_his As New DAO_Commons
    Private _dao As New DAORamais
    Private _dao_user As New DAOUsuarios
    Private _dao_grupos As New DAO_Grupos
    Public _usuario As New AppUsuarios
    Public _registro As AppRamais

    Private Sub RamalDetalhe_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_his.strConn = Session("conexao").ToString
        _dao_user.strConn = Session("conexao").ToString
        _dao_grupos.strConn = Session("conexao").ToString

        If Not Page.IsPostBack Then
            ViewState("codigo") = Request.QueryString("codigo")
            CarregaItem()
        End If
    End Sub

    Sub CarregaItem()

        Dim _list As New List(Of AppRamais)
        Dim list_modelo As New List(Of AppGeneric)
        Dim list_tipo As New List(Of AppGeneric)

        _dao.GetRamaisById(ViewState("codigo"), _list)

        _registro = _list(0)
        ViewState("ccusto") = _registro.Grupo

        list_modelo = _dao_his.GetGenericList(_registro.Codigo_Modelo, "CODIGO_MODELO", "MODELO", "RAMAIS_MODELOS", "", " order by MODELO")

        If list_modelo.Count > 0 Then
            ViewState("modelo") = list_modelo(0).Descricao
        End If

    End Sub
End Class
