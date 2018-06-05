Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.Globalization
Imports System.Drawing


Partial Class AparelhosMoveisDetalhes
    Inherits System.Web.UI.Page
    Private _dao As New DAOOperadoras
    Private _dao_his As New DAO_Commons
    Private _dao_lin As New DAO_LinhasMoveis
    Private _dao_op As New DAOOperadoras
    Private _dao_user As New DAOUsuarios
    Private _dao_grupos As New DAO_Grupos
    Public _registro As AppAparelhosMoveis
    Public _usuario As New AppUsuarios

    Private Sub AparelhosMoveisDetalhes_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_his.strConn = Session("conexao").ToString
        _dao_lin.strConn = Session("conexao").ToString
        _dao_op.strConn = Session("conexao").ToString
        _dao_user.strConn = Session("conexao").ToString
        _dao_grupos.strConn = Session("conexao").ToString

        If Not Page.IsPostBack Then
            ViewState("codigo") = Request.QueryString("codigo")
            carregaAparelho()
        End If


    End Sub

    Sub carregaAparelho()


        Dim list_facilidades As List(Of AppGeneric)
        Dim list_projetos As List(Of AppGeneric)
        Dim list_ccusto As List(Of AppGeneric)
        Dim list_marca As New List(Of AppGeneric)
        Dim list_modelo As New List(Of AppGeneric)
        Dim list_fornecedor As New List(Of AppGeneric)
        Dim list_status As New List(Of AppGeneric)
        Dim list_plano As New List(Of AppGeneric)
        Dim list_classificacao As New List(Of AppGeneric)


        _registro = _dao_lin.GetMovelById(ViewState("codigo"))(0)
        If _registro.Usuario <> "" Then
            _usuario = _dao_user.GetUsuarioById(_registro.Usuario)(0)

        End If

        list_ccusto = _dao_his.GetGenericList(_registro.Codigo, "ITEM", "GRUPO", "GRUPOS_ITEM", "", " order by grupo")
        list_marca = _dao_his.GetGenericList(_registro.Marca, "COD_MARCA", "MARCA", "APARELHOS_MARCAS", "", " order by MARCA")
        list_modelo = _dao_his.GetGenericList(_registro.Modelo, "COD_MODELO", "MODELO", "APARELHOS_MODELOS", "", " order by MODELO")
        list_fornecedor = _dao_his.GetGenericList(_registro.Operadora, "CODIGO", "NOME_FANTASIA", "FORNECEDORES", "", " order by NOME_FANTASIA")
        list_status = _dao_his.GetGenericList(_registro.Status, "CODIGO_STATUS", "DESCRICAO", "STATUS_LINHAS", "", " order by DESCRICAO")
        list_plano = _dao_his.GetGenericList(_registro.Plano, "CODIGO_PLANO", "PLANO", "OPERADORAS_PLANOS", "", " order by PLANO")
        list_classificacao = _dao_his.GetGenericList(_registro.Classificacao, "CODIGO_TIPO", "TIPO", "LINHAS_TIPO", "", " order by TIPO")

        ViewState("modelo") = ""
        ViewState("marca") = ""
        ViewState("ccusto") = ""
        ViewState("fornecedor") = ""
        ViewState("status") = ""
        ViewState("plano") = ""
        ViewState("CLASSIFICACAO") = ""

        If list_marca.Count > 0 Then
            ViewState("marca") = list_marca(0).Descricao
        End If

        If list_modelo.Count > 0 Then
            ViewState("modelo") = list_modelo(0).Descricao
        End If
        If list_fornecedor.Count > 0 Then
            ViewState("fornecedor") = list_fornecedor(0).Descricao
        End If
        If list_status.Count > 0 Then
            ViewState("status") = list_status(0).Descricao
        End If
        If list_plano.Count > 0 Then
            ViewState("plano") = list_plano(0).Descricao
        End If
        If list_classificacao.Count > 0 Then
            ViewState("CLASSIFICACAO") = list_classificacao(0).Descricao
        End If


        If list_ccusto.Count > 0 Then
            Dim dt As New DataTable
            For Each item As AppGeneric In list_ccusto
                ViewState("ccusto") += item.Descricao & " "
            Next

        End If







    End Sub

End Class
