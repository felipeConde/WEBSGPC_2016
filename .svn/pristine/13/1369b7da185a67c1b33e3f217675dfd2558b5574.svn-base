Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Web.UI

Partial Class GestaoCadastroUsuario
    Inherits System.Web.UI.Page

    Public _dao As New DAOUsuarios
    Public _dao_ramal As New DAORamais
    Public _dao_commons As New DAO_Commons
    Private image As Byte()
    Public Image_Url As String

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        End If

        _dao.strConn = Session("conexao").ToString
        _dao_ramal.strConn = Session("conexao").ToString
        _dao_commons.strConn = Session("conexao").ToString


        If Not Page.IsPostBack Then

            carregaUF()
            carregaCidade("0")
            CarregaLocalidades()
            CarregaStatus()
            btExcluir.Enabled = False
            btCategoria.Enabled = False
            'btRelatorio.Enabled = False
            btSenhaWEB.Enabled = True
            btMenus.Enabled = False
            PnRelatorios.Visible = False

            If _dao_commons.Is_Administrator(Session("codigousuario")) = False Then
                btCategoriaNova.Visible = False
                btMenus.Visible = False
                'btRelatorio.Visible = False
                btSenhaWEB.Visible = False
            End If

            If AppIni.Sulamerica_Param = True Then
                PnSulamerica.Visible = True
                chk_recursos_btn.Checked = True
            End If

            GridViewPopulator()

        End If
    End Sub

    Protected Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("codigo")) And Request.QueryString("codigo") > 0 Then
                EditUsuario(Request.QueryString("codigo"))
            End If
        End If
    End Sub

    Public Sub EditUsuario(ByVal pcodigo As String)
        Dim _registro As List(Of AppUsuarios)
        _registro = _dao.GetUsuarioById(pcodigo)

        'Ramais
        If _registro.Item(0).Rml_Numero_A <> "" Then
            tbRamal.Text = _registro.Item(0).Rml_Numero_A
            tbRamal_mirror.Text = _registro.Item(0).Rml_Numero_A
        Else
            tbRamal.Text = "SEM RAMAL"
            tbRamal_mirror.Text = "SEM RAMAL"
        End If
        cmbUF.SelectedValue() = _registro.Item(0).uf
        carregaCidade(_registro.Item(0).uf)
        'tbRamal.Text = _registro.Item(0).Rml_Numero_A
        tbCCRamal.Text = _dao_commons.GetGenericList(_registro.Item(0).Rml_Numero_A, "NUMERO_A", "GRP_CODIGO", "RAMAIS").Item(0).Descricao
        'preeche os campos
        ViewState("codigo") = _registro.Item(0).Codigo
        'div codigo
        tbCodigo.Text = _registro.Item(0).Codigo
        'div nome
        tbNome.Text = _registro.Item(0).Nome_Usuario
        'div endereco
        tbEndereco.Text = _registro.Item(0).Endereco
        tbNumero.Text = _registro.Item(0).Numero
        tbComplemento.Text = _registro.Item(0).Complemento
        tbBairro.Text = _registro.Item(0).Bairro
        tbCEP.Text = _registro.Item(0).CEP
        cmbCidade.SelectedValue() = _registro.Item(0).Codigo_Cidade
        'div usuario
        tbCPF.Text = _registro.Item(0).CPF
        tbTelefone.Text = _registro.Item(0).Telefone
        tbMatricula.Text = _registro.Item(0).Matricula
        tbCargo.Text = _registro.Item(0).Cargo_Usuario
        tbLogin.Text = _registro.Item(0).Login_Usuario
        txtsenha.Text = _registro.Item(0).Senha_Usuario
        tbEmail.Text = _registro.Item(0).Email_Usuario
        cbRecebeEmail.Checked = IIf(_registro.Item(0).Recebe_Email = "S", True, False)
        cbRecebRelatorio.Checked = IIf(_registro.Item(0).Recebe_Relatorio = "S", True, False)
        cbExtratoCelular.Checked = IIf(_registro.Item(0).RecebeCelular = "S", True, False)
        'div ramal

        tbCCUsuario.Text = _registro.Item(0).GRP_Codigo
        tbCCUsuario_mirror.Text = _registro.Item(0).GRP_Codigo
        tbEmailSup.Text = _registro.Item(0).Email_Supervisor
        'div web
        cbAcessaWEB.Checked = IIf(_registro.Item(0).Acesso_Web = "S", True, False)
        tbSenhaWEB.Text = _registro.Item(0).Senha_Web
        cmbStatus.SelectedValue = _registro.Item(0).STATUS

        If _registro.Item(0).Senha_Web <> "" Then
            LbPassword.Text = "Cadastrada"
        Else
            LbPassword.Text = "Não cadastrada"
        End If
        tbExpiraSenha.Text = _registro.Item(0).Expiracao_Senha_Web
        tbBloqWEB.Text = _registro.Item(0).Bloqueio_Web
        tbDiasExpiraSenha.Text = _registro.Item(0).Dias_Senha_Expira
        tb_user_code.Text = _registro.Item(0).ID_Usuario_Parent
        If tb_user_code.Text <> "" And tb_user_code.Text <> 0 Then

            tbUsuario.Text = _dao_commons.GetGenericList(_registro.Item(0).ID_Usuario_Parent, "CODIGO", "NOME_USUARIO", "USUARIOS").Item(0).Descricao
            tbUsuario_mirror.Text = _dao_commons.GetGenericList(_registro.Item(0).ID_Usuario_Parent, "CODIGO", "NOME_USUARIO", "USUARIOS").Item(0).Descricao

        End If
        If _registro.Item(0).CodigoLocalidade <> "" Then
            cmbLocalidades.SelectedValue = _registro.Item(0).CodigoLocalidade
        End If

        'carrega os aparelhos moveis dos usuários
        CarregaMoveis(pcodigo)

        If (pcodigo > 0) Then
            'plcodigo.Visible = True
            btExcluir.Enabled = True
            btCategoria.Enabled = True
            btCategoriaNova.Enabled = True
            'btRelatorio.Enabled = True
            btSenhaWEB.Enabled = True
            btMenus.Enabled = True
            btHistorico.Enabled = True
        End If

        '*********************** SULAMERICA *****************************

        If AppIni.Sulamerica_Param = True Then
            tbMatriculaSuperv.Text = _registro.Item(0).Matricula_sup
            tbVICE.Text = _registro.Item(0).VICE
            tbDIR.Text = _registro.Item(0).DIR
            tbGER.Text = _registro.Item(0).GER
            TbSUPTE.Text = _registro.Item(0).SUPTE
            tbSEC.Text = _registro.Item(0).SEC
            tbNUC.Text = _registro.Item(0).NUC
            tbDtAdmissao.Text = _registro.Item(0).DATA_ADMISSAO
            tbDtDesligamento.Text = _registro.Item(0).DATA_DEMISSAO

        End If

        '*********************** SULAMERICA *****************************

        Dim list As List(Of AppGeneric) = _dao_commons.GetGenericList(_registro.Item(0).Codigo, "codigo_usuario", "codigo_relatorio", "relatorios_usuarios")

        For Each _row As GridViewRow In Me.GvRelatorios.Rows
            For Each item As AppGeneric In list
                If item.Descricao.ToString = DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value Then
                    DirectCast(_row.FindControl("chkRelatorios"), CheckBox).Checked = True
                End If
            Next
        Next

        Dim _bytes As New List(Of Byte())

        Try
            _dao_commons.GetBytesByField(ViewState("codigo"), "codigo", "usuarios", "FOTO", _bytes)

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

    Protected Sub CarregaMoveis(ByVal pcodigo As Integer)
        'carrega os aparelhos moveis dos usuários
        Dim listMoveis As List(Of String) = _dao.GetMovelByUsuario(pcodigo)
        If listMoveis.Count > 0 Then
            Dim total As Integer = listMoveis.Count
            Dim i As Integer = 0
            If total > 4 Then
                total = 4
            End If

            While i < total
                Me.lbCelular.Text = Me.lbCelular.Text & listMoveis(i).ToString & "</br>"
                i = i + 1
            End While
            If listMoveis.Count > 4 Then
                Me.lbCelular.Text = Me.lbCelular.Text & "..."
            End If
            Me.phCelulares.Visible = True
        End If
    End Sub

    Protected Sub CarregaLocalidades()
        'carrega os aparelhos moveis dos usuários
        Dim List As List(Of AppGeneric) = _dao_commons.GetGenericList("", "codigo", "localidade", "localidades", "", " order by localidade")

        List.Insert(0, New AppGeneric(0, "SEM LOCALIDADE"))

        cmbLocalidades.DataSource = List
        cmbLocalidades.DataBind()
    End Sub

    Protected Sub CarregaStatus()
        'carrega os aparelhos moveis dos usuários
        Dim List As List(Of AppGeneric) = _dao_commons.GetGenericList("", "codigo", "descricao", "usuario_status")

        List.Insert(0, New AppGeneric(0, "SEM STATUS"))

        cmbStatus.DataSource = List
        cmbStatus.DataBind()
    End Sub

    Private Sub carregaUF()
        Dim listUf As List(Of String)
        listUf = _dao.ComboUfs()

        listUf.Insert(0, "...")

        cmbUF.DataSource = listUf
        cmbUF.DataBind()

    End Sub

    'Private Sub carregaRamais(ByVal ramal_usuario As String)
    '    Dim list As List(Of AppRamais)
    '    list = _dao_ramal.GetRamaisLivres("")
    '    If ramal_usuario <> "" Then
    '        list.Add(New AppRamais(ramal_usuario))
    '    End If

    '    list.Add(New AppRamais("SEM RAMAL"))

    '    'Dim strSQL As String = "select r.NUMERO_A from ramais r where not exists "
    '    'strSQL = strSQL + "(select 0 from usuarios where rml_numero_a=r.NUMERO_A) "

    '    'Dim dt As DataTable = _dao_commons.myDataTable(strSQL)


    '    'cmbRamais.DataSource = list
    '    'cmbRamais.DataBind()

    'End Sub

    Private Sub carregaCidade(ByVal Uf As String)

        Dim listCidade As List(Of AppGeneric)
        listCidade = _dao_commons.GetGenericList("", "codigo_cidade", "municipio", "cidades", "", "order by municipio")

        listCidade.Insert(0, New AppGeneric("", "..."))

        cmbCidade.DataSource = listCidade
        cmbCidade.DataBind()

    End Sub

    Private Function carregaCCRamal(ByVal _ramal As String) As String
        Dim list_ramal As New List(Of AppRamais)

        _dao_ramal.GetRamaisById(_ramal, list_ramal)

        If list_ramal.Count > 0 Then
            Return list_ramal.Item(0).Grp_Codigo
        End If

        Return ""
    End Function


    Private Function SalvaRegistro() As Boolean
        Dim _lRet As Boolean = False
        Dim _registro As New AppUsuarios

        _registro.Nome_Usuario = tbNome.Text
        _registro.Endereco = tbEndereco.Text
        _registro.Numero = tbNumero.Text
        _registro.Complemento = tbComplemento.Text
        _registro.Bairro = tbBairro.Text
        _registro.CEP = tbCEP.Text
        _registro.Municipio = cmbCidade.SelectedItem().ToString
        _registro.Codigo_Cidade = cmbCidade.SelectedValue().ToString
        _registro.CPF = tbCPF.Text
        _registro.Telefone = tbTelefone.Text
        _registro.Matricula = tbMatricula.Text
        _registro.Cargo_Usuario = tbCargo.Text
        _registro.Login_Usuario = tbLogin.Text
        _registro.Senha_Usuario = txtsenha.Text
        _registro.Email_Usuario = tbEmail.Text
        _registro.Recebe_Email = IIf(cbRecebeEmail.Checked, "S", "N")
        _registro.Recebe_Relatorio = IIf(cbRecebRelatorio.Checked, "S", "N")
        _registro.RecebeCelular = IIf(cbExtratoCelular.Checked, "S", "N")
        _registro.uf = cmbUF.SelectedValue
        If tbRamal_mirror.Text <> "SEM RAMAL" Then
            _registro.Rml_Numero_A = tbRamal_mirror.Text
        Else
            _registro.Rml_Numero_A = ""
        End If
        _registro.GRP_Codigo = tbCCUsuario.Text
        _registro.Email_Supervisor = tbEmailSup.Text
        _registro.Acesso_Web = IIf(cbAcessaWEB.Checked, "S", "N")
        _registro.Senha_Web = tbSenhaWEB.Text
        If tbExpiraSenha.Text = "" Then
            _registro.Expiracao_Senha_Web = "01/01/2000"
        Else
            _registro.Expiracao_Senha_Web = tbExpiraSenha.Text
        End If
        _registro.Bloqueio_Web = tbBloqWEB.Text
        If tbDiasExpiraSenha.Text = "" Then
            _registro.Dias_Senha_Expira = 0
        Else
            _registro.Dias_Senha_Expira = tbDiasExpiraSenha.Text
        End If

        _registro.ID_Usuario_Parent = IIf(tb_user_code.Text = "", 0, tb_user_code.Text)

        If cmbLocalidades.SelectedValue <> 0 Then
            _registro.CodigoLocalidade = cmbLocalidades.SelectedValue
        End If

        _registro.STATUS = cmbStatus.SelectedValue

        If AppIni.Sulamerica_Param = True Then
            _registro.Matricula_sup = tbMatriculaSuperv.Text
            _registro.VICE = tbVICE.Text
            _registro.DIR = tbDIR.Text
            _registro.GER = tbGER.Text
            _registro.SUPTE = TbSUPTE.Text
            _registro.SEC = tbSEC.Text
            _registro.NUC = tbNUC.Text
            _registro.DATA_ADMISSAO = tbDtAdmissao.Text
            _registro.DATA_DEMISSAO = tbDtDesligamento.Text
        End If

        Dim string_aux As String = ""
        Dim list_rel As String()

        For Each _row As GridViewRow In Me.GvRelatorios.Rows
            If DirectCast(_row.FindControl("chkRelatorios"), CheckBox).Checked = True Then
                string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
            End If

        Next

        list_rel = string_aux.Split(" ")

        Dim ret As Boolean

        If tbCodigo.Text = "" Then
            _dao.InsereRelatórios("", list_rel)
            ret = _dao.InsereUsuario(_registro, Session("username_login"))

            If UploadArquivo.PostedFile.FileName <> "" Then
                Dim _byte(UploadArquivo.PostedFile.InputStream.Length) As Byte
                UploadArquivo.PostedFile.InputStream.Read(_byte, 0, UploadArquivo.PostedFile.InputStream.Length)

                ' _dao_commons.InsertFiles(_registro.Id_produto, "produto_id", "produtos_files", Session("list_produto_name").Item(count_list), _byte, "BYTES", "FILE_NAME")

                'Session("list_produto_Bytes").Add(_byte)
            End If

            Return ret
        Else
            _registro.Codigo = tbCodigo.Text
            _dao.InsereRelatórios(_registro.Codigo, list_rel)
            ret = _dao.AtualizaUsuario(_registro, Session("username_login"))

            If foto.Visible = True Then
                _dao_commons.UpdateFileField(_registro.Codigo, "codigo", "usuarios", Session("image"), "FOTO")
            Else
                _dao_commons.UpdateFileField(_registro.Codigo, "codigo", "usuarios", Nothing, "FOTO")
            End If

            Return ret
        End If

        Return _lRet
    End Function

    Protected Sub btExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btExcluir.Click
        Dim error_msg As String = ""
        If phCelulares.Visible = True Then
            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "RM_CEL();", True)
        Else
            If _dao.ExcluiUsuario(ViewState("codigo"), error_msg, Session("username_login")) Then
                Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
            Else
                Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
            End If
        End If
    End Sub

    Protected Sub btnRM_YES_CEL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRM_YES_CEL.Click
        Dim error_msg As String = ""
        If _dao.RMV_CEL_USER(ViewState("codigo")) Then
            If _dao.ExcluiUsuario(ViewState("codigo"), error_msg, Session("username_login")) Then
                Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
            Else
                Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
            End If
        Else
            Response.Write("<script>alert('ERRO ! Operação NÃO realizada - Erro ao tentar desvincular o usuário!');window.opener.location.reload();window.close();</script>")
        End If

    End Sub

    Protected Sub btSenhaWEB_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btSenhaWEB.Click
        If Page.IsValid Then
            myscript("<script>window.open('GestaoGerarSenha.aspx?codigo=" + tbCodigo.Text + "', 'alterasenha','width=280,height=280');</script>", True)
        End If
    End Sub

    Protected Sub btCategoria_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btCategoria.Click
        If Page.IsValid Then
            myscript("<script>window.open('novacategoriausuario.asp?operacao=2&idusuario=" + tbCodigo.Text + "', 'senhaweb', 'width=450,height=360,scrollbars=1');</script>", True)
        End If
    End Sub

    Protected Sub btCategoriaNova_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btCategoriaNova.Click
        If Page.IsValid Then
            myscript("<script>window.open('GestaoCategoriaUsuario.aspx?idusuario=" + tbCodigo.Text + "', 'senhaweb', 'width=588,height=680');</script>", True)
        End If
    End Sub

    Protected Sub btRelatorio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btRelatorio.Click
        'If Page.IsValid Then
        '    myscript("<script>window.open('acessoweb.asp?codigo=" + tbCodigo.Text + "','acessoweb', 'width=700,height=600, scrollbars=1');</script>", True)
        'End If

        If PnRelatorios.Visible = True Then
            PnRelatorios.Visible = False
        Else
            PnRelatorios.Visible = True
        End If

    End Sub

    Protected Sub btMenus_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btMenus.Click
        If Page.IsValid Then
            myscript("<script>window.open('acessoweb-menus.asp?codigo=" + tbCodigo.Text + "', 'acessoweb', 'width=500,height=450,scrollbars=1');</script>", True)
        End If
    End Sub

    Protected Sub cmbUF_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbUF.SelectedIndexChanged

        carregaCidade(cmbUF.SelectedValue)
    End Sub

    Private Sub myscript(ByVal myscript As String, ByVal pClose As Boolean)
        If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", myscript)
        End If
    End Sub

    'Protected Sub tbRamal_TextChanged(sender As Object, e As System.EventArgs) Handles tbRamal.TextChanged
    '    tbCCRamal.Text = carregaCCRamal(tbRamal.Text)
    'End Sub

    Protected Sub btHistorico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btHistorico.Click
        If Page.IsValid Then
            myscript("<script>window.open('velog.asp?operacao=3&dataini=01/01/1987&datafim=31/12/2100&usuario=" + tbCodigo.Text + "','historico', 'width=500,height=450,scrollbars=1,resizable=1');</script>", True)
        End If
    End Sub

    Protected Sub btGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btGravar.Click
        If (tbRamal_mirror.Text <> "SEM RAMAL" Or Me.lbCelular.Text <> "") And tbCodigo.Text <> "" And chk_recursos_btn.Checked = True Then
            AtualizaCCustoRamal()
            tbCCUsuario_mirror.Text = tbCCUsuario.Text
        Else
            If Page.IsValid Then

                If tbCCUsuario.Text = "" Then
                    tbCCUsuario.Text = tbCCRamal.Text
                    tbCCUsuario_mirror.Text = tbCCRamal.Text
                    tbCCUsuario.Text = tbCCRamal.Text
                End If

                If tbNome.Text = "" Then
                    Response.Write("<script>alert('INFORME O NOME DO USUÁRIO');</script>")
                ElseIf _dao.VerificaCampo(tbLogin.Text, "LOGIN_USUARIO", tbCodigo.Text) > 0 Then
                    Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE LOGIN. O LOGIN DO USUARIO DEVE SER EXCLUSIVO');</script>")
                ElseIf _dao.VerificaSenha(txtsenha.Text, "SENHA_USUARIO", tbCodigo.Text) > 0 Then
                    Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESTA SENHA.A SENHA DO USUARIO DEVE SER EXCLUSIVA');</script>")
                ElseIf tbCCUsuario.Text = "" Then
                    Response.Write("<script>alert('INFORME O CENTRO DE CUSTO DO USUÁRIO');</script>")
                ElseIf tbMatricula.Text <> "" And _dao_commons.GetGenericList(tbMatricula.Text, "matricula", "matricula", "usuarios", "", IIf(Request.QueryString("codigo") <> "", "and codigo <> '" & Request.QueryString("codigo") & "'", "")).Count > 0 Then
                    Response.Write("<script>alert('MATRICULA JÁ ESTÁ CADASTRADA');</script>")
                    'ElseIf _dao.VerificaCampo(tbRamal.Text, "RML_NUMERO_A", tbCodigo.Text) > 0 Then
                    '    Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE RAMAL. O RAMAL DO USUARIO DEVE SER EXCLUSIVO');</script>")
                Else

                    If tbCodigo.Text = "" Then
                        If Not VerificaNomeRepetido() Then
                            If SalvaRegistro() Then
                                Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                            Else
                                Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                            End If
                        End If
                    Else
                        If SalvaRegistro() Then
                            Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                        Else
                            Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                        End If
                    End If

                End If

            End If
        End If
    End Sub


    Function VerificaNomeRepetido() As Boolean
        Dim nome As String = Me.tbNome.Text.ToUpper.Trim
        Dim dt As DataTable = _dao_commons.myDataTable("select 1 from usuarios where upper(nome_usuario)='" & nome & "'")

        If dt.Rows.Count > 0 Then

            Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "ConfirmaInsert();", True)
            Return True

        End If

        Return False


        'quando for excluir verificar se tem cdrs não deixa excluir

    End Function


    Protected Function AtualizaCCustoRamal() As Boolean

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", "CCUSTO_RAMAL();", True)

        Return False
    End Function

    Protected Sub btnCCUSTO_RAMAL_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCCUSTO_RAMAL.Click
        If _dao.AtualizaCCUSTO_RAMAL(tbCCUsuario.Text, tbRamal_mirror.Text, Session("username_login")) = True Then
        Else
            Response.Write("<script>alert('Não foi possivel atualizar centro de custo do ramal');</script>")
        End If


        If _dao.AtualizaCCUSTOS(tbCCUsuario.Text, Request.QueryString("codigo"), Session("username_login")) = True Then
        Else
            Response.Write("<script>alert('Não foi possivel atualizar centro de custo dos celulares');</script>")
        End If

        If Page.IsValid Then

            If tbCCUsuario_mirror.Text = "" Then
                tbCCUsuario.Text = tbCCRamal.Text
                tbCCUsuario_mirror.Text = tbCCRamal.Text
            End If

            If tbNome.Text = "" Then
                Response.Write("<script>alert('INFORME O NOME DO USUÁRIO');</script>")
            ElseIf _dao.VerificaCampo(tbLogin.Text, "LOGIN_USUARIO", tbCodigo.Text) > 0 Then
                Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE LOGIN. O LOGIN DO USUARIO DEVE SER EXCLUSIVO');</script>")
            ElseIf tbCCUsuario.Text = "" Then
                Response.Write("<script>alert('INFORME O CENTRO DE CUSTO DO USUÁRIO');</script>")
            ElseIf tbMatricula.Text <> "" And _dao_commons.GetGenericList(tbMatricula.Text, "matricula", "matricula", "usuarios", "", IIf(Request.QueryString("codigo") <> "", "and codigo <> '" & Request.QueryString("codigo") & "'", "")).Count > 0 Then
                Response.Write("<script>alert('MATRICULA JÁ ESTÁ CADASTRADA');</script>")
                'ElseIf cmbCidade.SelectedValue = "" Then
                '    Response.Write("<script>alert('SELECIONE A CIDADE');</script>")
                'ElseIf _dao.VerificaCampo(tbRamal.Text, "RML_NUMERO_A", tbCodigo.Text) > 0 Then
                '    Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE RAMAL. O RAMAL DO USUARIO DEVE SER EXCLUSIVO');</script>")
            Else
                If SalvaRegistro() Then
                    Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                Else
                    Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                End If
            End If

        End If
    End Sub

    Protected Sub btnCCUSTO_RAMAL_NO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCCUSTO_RAMAL_NO.Click
        If Page.IsValid Then

            If tbCCUsuario_mirror.Text = "" Then
                tbCCUsuario.Text = tbCCRamal.Text
                tbCCUsuario_mirror.Text = tbCCRamal.Text
            End If

            If tbNome.Text = "" Then
                Response.Write("<script>alert('INFORME O NOME DO USUÁRIO');</script>")
            ElseIf _dao.VerificaCampo(tbLogin.Text, "LOGIN_USUARIO", tbCodigo.Text) > 0 Then
                Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE LOGIN. O LOGIN DO USUARIO DEVE SER EXCLUSIVO');</script>")
            ElseIf tbCCUsuario.Text = "" Then
                Response.Write("<script>alert('INFORME O CENTRO DE CUSTO DO USUÁRIO');</script>")
            ElseIf tbMatricula.Text <> "" And _dao_commons.GetGenericList(tbMatricula.Text, "matricula", "matricula", "usuarios", "", IIf(Request.QueryString("codigo") <> "", "and codigo <> '" & Request.QueryString("codigo") & "'", "")).Count > 0 Then
                Response.Write("<script>alert('MATRICULA JÁ ESTÁ CADASTRADA');</script>")
                'ElseIf cmbCidade.SelectedValue = "" Then
                ' Response.Write("<script>alert('SELECIONE A CIDADE');</script>")
                'ElseIf _dao.VerificaCampo(tbRamal.Text, "RML_NUMERO_A", tbCodigo.Text) > 0 Then
                '    Response.Write("<script>alert('ALGUM USUARIO JÁ POSSUI ESSE RAMAL. O RAMAL DO USUARIO DEVE SER EXCLUSIVO');</script>")
            Else
                If SalvaRegistro() Then
                    Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                Else
                    Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
                End If
            End If

        End If
    End Sub

    Private Sub myAlert(ByVal msg As String, ByVal pClose As Boolean)


        Dim myscript As String = "alert(" & msg & ");"
        If pClose Then

            If ViewState("_reload") <> "N" Then
                myscript += "window.opener.location.reload();window.close();"
            Else
                myscript += "window.close();"
            End If

        End If

        If ViewState("novo") = "S" Then
            myscript += "__doPostBack('btNovo', '');"
        End If

        Page.ClientScript.RegisterStartupScript(Me.GetType(), "myscript", myscript, True)

        Dim strScript As String = "<script language=JavaScript>"
        strScript += "alert(""" & msg & """);"
        If pClose Then
            If ViewState("_reload") <> "N" Then
                strScript += "window.opener.location.reload();window.close();"
            Else
                strScript += "window.close();"
            End If
        End If
        If ViewState("novo") = "S" Then
            strScript += "__doPostBack('btNovo', '');"
        End If

        strScript += "</script>"

        If (Not ClientScript.IsStartupScriptRegistered("clientScript")) Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "clientScript", strScript)
        End If

    End Sub


    Protected Sub BtnChangeRamal_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles BtnChangeRamal.Click
        If tbRamal_mirror.Text = "SEM RAMAL" Then
            tbCCRamal.Text = ""
        Else
            tbCCRamal.Text = _dao_commons.GetGenericList(tbRamal_mirror.Text, "NUMERO_A", "GRP_CODIGO", "RAMAIS").Item(0).Descricao
        End If
        tbRamal.Text = tbRamal_mirror.Text
        tbCCUsuario_mirror.Text = tbCCUsuario.Text
    End Sub

    Protected Sub cmbLocalidades_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbLocalidades.SelectedIndexChanged
        If cmbLocalidades.SelectedValue <> 0 Then
            Try
                Dim codigo_cidade As String = _dao_commons.GetUF_and_citycode_ByLocalidades(cmbLocalidades.SelectedValue).Item(0).Codigo
                Dim uf As String = _dao_commons.GetUF_and_citycode_ByLocalidades(cmbLocalidades.SelectedValue).Item(0).Descricao
                cmbUF.SelectedValue = uf
                carregaCidade(cmbUF.SelectedValue)
                cmbCidade.SelectedValue = codigo_cidade
            Catch ex As Exception

            End Try
        End If
    End Sub

    Public Sub GridViewPopulator()
        Dim dt As New DataTable

        ''Carrega Grid de Projetos

        Session("GvProjetos") = ""
        GvRelatorios.DataSource = _dao_commons.GetGenericList("", "codigo", "nome", "relatorios", "", " and URL <> '#' order by nome ")
        dt = DAO_Commons.ConvertToDataTable(GvRelatorios.DataSource)
        dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        GvRelatorios.DataSource = dt

        Session("GvRelatorios") = dt
        GvRelatorios.DataBind()

        ''Carrega Grid de CCUSTOS

        ViewState("First_run") = 1
    End Sub

    Protected Sub chkboxSelectAll_CheckedChanged(sender As Object, e As EventArgs)
        Dim ChkBoxHeader As CheckBox = DirectCast(GvRelatorios.HeaderRow.FindControl("chkboxSelectAll"), CheckBox)
        For Each row As GridViewRow In GvRelatorios.Rows
            Dim ChkBoxRows As CheckBox = DirectCast(row.FindControl("chkRelatorios"), CheckBox)
            If ChkBoxHeader.Checked = True Then
                ChkBoxRows.Checked = True
            Else
                ChkBoxRows.Checked = False
            End If
        Next
    End Sub

    Protected Sub btConfimaNovo_Click(sender As Object, e As System.EventArgs) Handles btConfimaNovo.Click
        'SalvaRegistro()
        If SalvaRegistro() Then
            Response.Write("<script>alert('Operação realizada com sucesso!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
        Else
            Response.Write("<script>alert('ERRO ! Operação NÃO realizada!');window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
        End If
    End Sub

    Protected Sub btnPostFoto_Click(sender As Object, e As System.EventArgs) Handles btnPostFoto.Click
        If UploadArquivo.PostedFile.FileName <> "" Then
            Dim _byte(UploadArquivo.PostedFile.InputStream.Length) As Byte
            UploadArquivo.PostedFile.InputStream.Read(_byte, 0, UploadArquivo.PostedFile.InputStream.Length)

            Dim count As Integer = 0
            Dim list_produtos_Bytes As New List(Of Byte())
            Dim list_fatura_name As New List(Of String)

            image = _byte
            Session("image") = _byte
            Dim base64String As String = Convert.ToBase64String(image, 0, image.Length)
            foto.ImageUrl = Convert.ToString("data:image/png;base64,") & base64String
            foto.Visible = True
            noImage.Visible = False

        End If
    End Sub

    Protected Sub btnRemove_foto_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnRemove_foto.Click
        Session("image") = Nothing
        foto.Visible = False
        noImage.Visible = True
    End Sub
End Class
