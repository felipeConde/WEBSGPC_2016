Imports System.Web.UI
Imports System.Data
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Collections

Partial Class altera_senha
    Inherits System.Web.UI.Page
    Dim _dao_usuario As New DAOUsuarios

    Private Sub altera_senha_Load(sender As Object, e As EventArgs) Handles Me.Load
        Session("conexao") = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString

        _dao_usuario.strConn = Session("conexao")
    End Sub

    Private Sub btGravar_Click(sender As Object, e As EventArgs) Handles btGravar.Click

        'banco.execute("update usuarios set acesso_web='S', senha_web=password.encrypt('" & senha_web & "') WHERE codigo='" & codigo_usuario & "'")


        If ValidaForm() Then
            If _dao_usuario.GravaSenhaWeb(Me.txtsenha.Text.Trim.ToUpper, Session("codigousuario"), 99999, "01/01/2100", "") Then
                Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "sweetAlertInitialize();Sucesso2('Operação Efetuada com Sucesso');", True)
            End If
        End If



    End Sub

    Function ValidaForm() As Boolean
        Dim valida As Boolean = True
        If Me.txtsenha.Text.Trim = "" Or Me.txtConfirmaSenha.Text.Trim = "" Then
            Me.divErro.Text = "Digite a senha."
            Me.divErro.Visible = True
            valida = False
        Else
            If Me.txtsenha.Text.Trim.ToUpper <> Me.txtConfirmaSenha.Text.Trim.ToUpper Then
                Me.divErro.Text = "A confirmação da senha não confere."
                Me.divErro.Visible = True
                valida = False
            End If
        End If
        Return valida
    End Function
End Class
