Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb

Module global_variables_extratocel
    Public commom_user_ext As Integer = 0
End Module

Partial Class GestaoRel_ExtratoFixo

    Inherits System.Web.UI.Page
    Private _dao As New DAOOperadoras
    Private _dao_his As New DAO_Commons

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'Session("conexao") = ConfigurationManager.AppSettings("ConnectionString")

        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_his.strConn = Session("conexao").ToString

        If _dao_his.Is_Commom_User(Session("codigousuario")) Then
            PnGerencial.Visible = False
            PnUser_commom.Visible = True
            rbUser_common_lines.DataSource = _dao_his.Get_Phone_List_User(Session("codigousuario"))
            rbUser_common_lines.DataBind()
        End If

        tbUsuario_mirror.Text = tbUsuario.Text

        If Not Page.IsPostBack Then
            carregaOperadora()
            carregaAno()
            carregaMes()
            cmbMes.SelectedValue = IIf(Date.Now.Month < 10, "0" & Date.Now.Month.ToString, Date.Now.Month.ToString)
            cmbAno.SelectedValue = DateTime.Now.Year
        End If
    End Sub

    Private Sub carregaOperadora()
        Dim listOP As List(Of AppOperadoras)
        listOP = _dao.ComboOperadorasMoveis()
        listOP.Insert(0, New AppOperadoras(0, "TODAS", "", ""))

    End Sub

    Private Sub carregaMes()
        Dim listOP As New List(Of AppGeneric)
        listOP.Add(New AppGeneric("01", "Janeiro"))
        listOP.Add(New AppGeneric("02", "Fevereiro"))
        listOP.Add(New AppGeneric("03", "Março"))
        listOP.Add(New AppGeneric("04", "Abril"))
        listOP.Add(New AppGeneric("05", "Maio"))
        listOP.Add(New AppGeneric("06", "Junho"))
        listOP.Add(New AppGeneric("07", "Julho"))
        listOP.Add(New AppGeneric("08", "Agosto"))
        listOP.Add(New AppGeneric("09", "Setembro"))
        listOP.Add(New AppGeneric("10", "Outubro"))
        listOP.Add(New AppGeneric("11", "Novembro"))
        listOP.Add(New AppGeneric("12", "Dezembro"))

        cmbMes.DataSource = listOP
        cmbMes.DataBind()

    End Sub


    Private Sub carregaAno()
        Dim listOP As New List(Of Integer)

        For aux As Integer = 0 To 10
            listOP.Add(Now.Year - aux)
        Next

        listOP.Insert(0, (Now.Year + 1))

        cmbAno.DataSource = listOP
        cmbAno.DataBind()

    End Sub


    Protected Sub btnHtml_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHtml.Click

        If tbLinha.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "") <> "" Then

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoRel_ExtratoFixoResult.aspx?celular=" + tbLinha.Text.Replace("_", "") + "&mes=" + cmbMes.SelectedValue + "&ano=" + cmbAno.SelectedValue + "&tipo=HTML&dataini=" + tbDt_ativ.Text + "&datafim=" + tbDt_des.Text + "');</script>")
        Else

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('Selecione um número de celular');</script>")

        End If

    End Sub
    Protected Sub btnExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnExcel.Click

        If tbLinha.Text.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "") <> "" Then

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoRel_ExtratoFixoResult.aspx?celular=" + tbLinha.Text.Replace("_", "") + "&mes=" + cmbMes.SelectedValue + "&ano=" + cmbAno.SelectedValue + "&tipo=Excel&dataini=" + tbDt_ativ.Text + "&datafim=" + tbDt_des.Text + "');</script>")

        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('Selecione um número de linha');</script>")

        End If
    End Sub

    Protected Sub tb_user_code_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tb_user_code.TextChanged
        If _dao_his.Get_Phone_List_UserNoCELL(tb_user_code.Text).Count = 1 Then
            tbLinha.Text = _dao_his.Get_Phone_List_UserNoCELL(tb_user_code.Text).Item(0)
        ElseIf _dao_his.Get_Phone_List_UserNoCELL(tb_user_code.Text).Count > 1 Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>alert('O usuário possui mais de uma linha fixa. Selecione uma linha');</script>")
        End If

    End Sub
End Class