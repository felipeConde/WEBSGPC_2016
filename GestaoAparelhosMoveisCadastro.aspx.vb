Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Net
Imports System.Globalization
Imports System.Drawing

Module global_variables_linemobile
    Public objDt_linemobilet As New System.Data.DataTable
    Public contGrupo_linemobile As Integer
    Public list_Bytes_linemobile As New List(Of Byte())
    Public list_name_linemobile As New List(Of String)
End Module


Partial Class GestaoAparelhosMoveisCadastro
    Inherits System.Web.UI.Page
    Private _dao As New DAOOperadoras
    Private _dao_his As New DAO_Commons
    Private _dao_lin As New DAO_LinhasMoveis
    Private _dao_op As New DAOOperadoras
    Private _dao_user As New DAOUsuarios
    Private _dao_grupos As New DAO_Grupos

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

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

        div_termos.Visible = False
        div_troca.Visible = False
        div_desvincula.Visible = False
        div_vincula.Visible = False

        If Not Page.IsPostBack Then

            lbCCusto.Text = "Nenhum"
            Session("GvCCustos") = ""
            carregaMarcas()
            carregaModelos()
            carregaOperadora()
            carregaNatureza()
            carrega_classificacao()
            carregaPlanos()
            carregaParcelas()
            carregaStatus()
            carregaTecnologia()
            carregaContabil()
            list_Bytes_linemobile.Clear()
            list_name_linemobile.Clear()
            GV_ArquivosPopulator("")
            btHistorico.Enabled = False
            btHistorico_IMEI.Enabled = False
            btHistorico_SIM.Enabled = False
            div_chamados.Visible = False

            '***************************************************************

            If AppIni.Ageradora_Param = True Then
                pnAGeradora.Visible = True
                btTermo.Enabled = True
                btTermo_2.Enabled = True
                btTermo_3.Enabled = True
                btTermo_4.Enabled = True
            Else
                pnAGeradora.Visible = False
                btTermo.Enabled = False
                btTermo_2.Enabled = False
                btTermo_3.Enabled = False
            End If

            If AppIni.GloboRJ_Parm = True Then
                pnGlobo.Visible = True
            End If

            '***************************************************************

            If AppIni.Vonpar_Param = True Then
                btTermoVonpar.Enabled = True
                pnVonpar.Visible = True
            Else
                btTermoVonpar.Enabled = False
                pnVonpar.Visible = False
            End If

            '***************************************************************

            If AppIni.CCusto_Editable = True Then
                pnCCusto_Editable.Visible = True
                pnCCusto_Not_Editable.Visible = False
            Else
                pnCCusto_Editable.Visible = False
                pnCCusto_Not_Editable.Visible = True
            End If

            '***************************************************************
        Else
            tbCodigo_cliente_name.Text = tbCodigo_cliente_name_mirror.Text
            tbUsuario.Text = tbUsuario_mirror.Text
            If txtSucursal_code.Text <> "" Then
                txtSucursal.Text = _dao_his.GetGenericList(txtSucursal_code.Text, "codigo", "localidade", "localidades").Item(0).Descricao
            End If

        End If

        If Not (ScriptManager.GetCurrent(Me.Page) Is Nothing) Then

            If (Not Page.ClientScript.IsStartupScriptRegistered("AjaxToolkitTempFix")) Then

                Page.ClientScript.RegisterStartupScript(Me.GetType(), "AjaxToolkitTempFix", "Date.parseLocale = function(s, f){return Date(s);};" + Environment.NewLine + "Sys.Debug = new Object();Sys.Debug.isDebug = function(){return true};", True)

            End If

        End If
    End Sub

    Protected Sub Page_PreRenderComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRenderComplete
        If Not Page.IsPostBack Then
            If Not String.IsNullOrEmpty(Request.QueryString("codigo")) And Request.QueryString("codigo") > 0 Then
                Try
                    EditAparelho(Request.QueryString("codigo"))
                Catch ex As Exception
                    Response.Write("codigo inválido")
                    Response.End()
                End Try
                divChamadoNew.Visible = False
            Else
                btnTermoGLOBO.Visible = False
                div_comodato.Visible = False
                divChamadoedit.Visible = False
                sem_aparelho()
                div_vincula.Visible = False
                GridViewPopulator()
                cmbStatus.SelectedValue = 1
                PnBackup.Attributes.Add("style", "display:none")
                PnSucata.Attributes.Add("style", "display:none")
            End If

        End If
    End Sub

    Public Sub EditAparelho(ByVal pcodigo As Integer)
        Dim _registro As List(Of AppAparelhosMoveis)
        Dim list_facilidades As List(Of AppGeneric)
        Dim list_projetos As List(Of AppGeneric)
        Dim list_ccusto As List(Of AppGeneric)

        _registro = _dao_lin.GetMovelById(pcodigo)

        ViewState("codigo") = _registro.Item(0).Codigo.ToString
        tbCodigo.Text = ViewState("codigo")

        cmbClassificacao.SelectedValue = _registro.Item(0).Classificacao
        cmbOperadora.SelectedValue = _registro.Item(0).Operadora
        'Carregando Combos
        carregaPlanos()

        div_chamados.Visible = True

        tbContrato.Text = _registro.Item(0).Contrato
        cmbPlanos.SelectedValue = _registro.Item(0).Plano

        ViewState("planoAnterior") = _registro.Item(0).Plano
        ViewState("planoAtual") = _registro.Item(0).Plano

        If _registro.Item(0).Exibe_parcel_rel = "S" Then
            chkMostraParcela.Checked = True
        Else
            chkMostraParcela.Checked = False
        End If

        cmbQtdParcel.SelectedValue = _registro.Item(0).QTD_parcel
        tbInicioParcl.Text = _registro.Item(0).Inicio_Parcel

        tbPIN1.Text = _registro.Item(0).Pin1
        tbPIN2.Text = _registro.Item(0).Pin2
        tbPUK1.Text = _registro.Item(0).Puk1
        tbPUK2.Text = _registro.Item(0).Puk2
        tbIp.Text = _registro.Item(0).Ip
        tbTelefone.Text = _registro.Item(0).Telefone
        tbFleet.Text = _registro.Item(0).Fleet
        tbSIMCARD.Text = _registro.Item(0).Simcard.Trim
        tbSIMCARD_value.Text = _registro.Item(0).Simcard_value.Trim
        tbDt_ativ.Text = _registro.Item(0).Ativacao.Replace("00:00:00", "").Replace(" ", "")
        tbDt_des.Text = _registro.Item(0).Desativacao.Replace("00:00:00", "").Replace(" ", "")
        cmbStatus.SelectedValue = _registro.Item(0).Status
        'FACILIDADES
        'PROJETOS
        tbCodigo_cliente.Text = _registro.Item(0).Codigo_cliente
        If _registro.Item(0).Intragrupo = "S" Then
            tbIntragrupo.Checked = True
        Else
            tbIntragrupo.Checked = False
        End If

        cmbTecnologia.SelectedIndex = IIf(_registro.Item(0).Tecnologia = "", 0, _registro.Item(0).Tecnologia)
        cmbMarca.SelectedValue = _registro.Item(0).Marca

        sem_aparelho()

        carregaModelos()
        cmbModelo.SelectedValue = _registro.Item(0).Modelo
        cmbNatureza.SelectedIndex = IIf(_registro.Item(0).Natureza = "", 0, _registro.Item(0).Natureza)
        If cmbNatureza.SelectedValue = "Comodato" Then
            div_comodato.Visible = True
        Else
            div_comodato.Visible = False
        End If
        tbCodigo_aparelho.Text = _registro.Item(0).Codigo_aparelho
        tbValor_aparelho.Text = _registro.Item(0).Valor_aparelho
        tbNotaFiscal.Text = _registro.Item(0).Nota_fiscal
        tbIMEI.Text = _registro.Item(0).Identificacao
        tbVencimento_Comodato.Text = _registro.Item(0).Venc_comodato.Replace("00:00:00", "").Replace(" ", "")
        tbVencimento_Garantia.Text = _registro.Item(0).Venc_garantia.Replace("00:00:00", "").Replace(" ", "")
        tbPin_Aparelho.Text = _registro.Item(0).Pin_Aparelho
        tbProtocolo_cancel.Text = _registro.Item(0).Protocolo_cancel
        tbSerialNumber.Text = _registro.Item(0).Serial_Number
        cmbContaContabil.SelectedValue = _registro.Item(0).Conta_cont

        If _registro.Item(0).Sucursal <> "" Then

            Try
                txtSucursal.Text = _dao_his.GetGenericList(_registro.Item(0).Sucursal, "codigo", "localidade", "localidades").Item(0).Descricao
                txtSucursal_code.Text = _registro.Item(0).Sucursal
            Catch ex As Exception
            End Try

        End If

        If _registro.Item(0).Estoque = "S" Then
            tbEstoque.Checked = True
        Else
            tbEstoque.Checked = False
        End If
        If _registro.Item(0).Backup = "S" Then
            btnBackup.Checked = True
            tbProp_Estoque.Text = _registro.Item(0).Prop_estoque
            tbOrdem_Serviço.Text = _registro.Item(0).Ordem_serv
            tbEmissão.Text = _registro.Item(0).Emissao
        Else
            btnBackup.Checked = False
            PnBackup.Attributes.Add("style", "display:none")
        End If
        If _registro.Item(0).Perdido = "S" Then
            btnPerdido.Checked = True
        Else
            btnPerdido.Checked = False
        End If

        If cmbNatureza.SelectedValue = "Comodato" Then
            div_comodato.Visible = True
        End If

        If _registro.Item(0).Sucata = "S" Then
            btnSucata.Checked = True
            tbChamado_Retirada.Text = _registro.Item(0).Chamada_retirada
            tbData_Retirada.Text = _registro.Item(0).Data_retirada
        Else
            btnSucata.Checked = False
            PnSucata.Attributes.Add("style", "display:none")
        End If

        tbOBS.Text = _registro.Item(0).Obs
        tbObs_aparelho.Text = _registro.Item(0).Obs_aparelho
        tb_user_code.Text = _registro.Item(0).Usuario
        tbLimite_Uso.Text = _registro.Item(0).Limite_uso.Trim

        If (_registro.Item(0).Usuario <> "" And _registro.Item(0).Usuario <> "0") Then
            tbUsuario.Text = _dao_his.GetGenericList(_registro.Item(0).Usuario, "codigo", "nome_usuario", "usuarios").Item(0).Descricao
        End If
        If (_registro.Item(0).Codigo_cliente <> "" And _registro.Item(0).Codigo_cliente <> "0") Then
            tbCodigo_cliente_name.Text = _dao_his.GetGenericList(_registro.Item(0).Codigo_cliente, "codigo_cliente", "cliente", "codigos_cliente").Item(0).Descricao
        End If

        'Inicializa Preenchimento dos Grids

        GridViewPopulator()

        list_facilidades = _dao_his.GetGenericList(_registro.Item(0).Codigo, "CODIGO_LINHA", "CODIGO_VAS", "LINHAS_VAS")

        For Each _row As GridViewRow In Me.GvFacilidades.Rows
            For Each item As AppGeneric In list_facilidades
                If item.Descricao.ToString = DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value Then
                    DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True
                End If
            Next
        Next

        list_projetos = _dao_his.GetGenericList(_registro.Item(0).Codigo, "CODIGO_LINHA", "CODIGO_PROJETO", "LINHAS_PROJETOS")

        For Each _row As GridViewRow In Me.GvProjetos.Rows
            For Each item As AppGeneric In list_projetos
                If item.Descricao.ToString = DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value Then
                    DirectCast(_row.Cells(1).FindControl("chkProjeto"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True
                End If
            Next
        Next

        list_ccusto = _dao_his.GetGenericList(_registro.Item(0).Codigo, "ITEM", "GRUPO", "GRUPOS_ITEM", "", " order by grupo")

        If pnCCusto_Editable.Visible = True Then

            If list_ccusto.Count > 0 Then
                Dim dt As New DataTable

                dt = Session("GvCCustos")

                For Each item As AppGeneric In list_ccusto
                    If item.Descricao <> " " Then
                        Dim newRow As DataRow = dt.NewRow()
                        newRow("CODIGO") = item.Descricao
                        newRow("DESCRICAO") = _dao_his.GetGenericList(item.Descricao, "CODIGO", "NOME_GRUPO", "GRUPOS").Item(0).Descricao

                        dt.Rows.Add(newRow)
                    End If
                Next
                GvCCustos.DataSource = dt
                Session("GvCCustos") = GvCCustos.DataSource
                GvCCustos.DataBind()
            End If

        Else

            If list_ccusto.Count > 0 Then
                lbCCusto_code.Text = list_ccusto.Item(0).Descricao
                lbCCusto.Text = _dao_his.GetGenericList(list_ccusto.Item(0).Descricao, "CODIGO", "NOME_GRUPO", "GRUPOS").Item(0).Descricao
            Else
                lbCCusto.Text = "Nenhum"
            End If

        End If

        tbCodigo_cliente_name_mirror.Text = tbCodigo_cliente_name.Text
        tbUsuario_mirror.Text = tbUsuario.Text

        Try
            lbUltimoOEM.Text = _dao_his.GetGenericList("", "p1.OEM", "nvl(p1.abertura, '01/07/2015')", " chamados p1, chamados_items p2 ", "", " and p1.oem = p2.oem and p2.codigo_item='" & _registro.Item(0).Codigo & "' and p1.tipo_item ='1' order by descricao desc").Item(0).Codigo
        Catch ex As Exception
            lbUltimoOEM.Text = "Sem chamado"
            imgChamEdit.Visible = False
        End Try


        btExcluir.Enabled = True
        'CENTROS DE CUSTO

        btTermo.Enabled = True
        btTermo_2.Enabled = True
        btTermo_3.Enabled = True

        btTermoVonpar.Enabled = True

        btGravar_novo.Visible = True

        Dim msg As String = ""
        msg = _dao_lin.VerificaIntegridadeCadastro(ViewState("codigo"), tbTelefone.Text, tbSIMCARD.Text, tbIMEI.Text, tbCodigo_aparelho.Text)

        If msg <> "ok" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'" + msg + "');</script>")
        End If

        CarregaFoto(tb_user_code.Text)

        btHistorico.Enabled = True
        btHistorico_IMEI.Enabled = True
        btHistorico_SIM.Enabled = True

        If tb_user_code.Text <> "" Then
            div_termos.Visible = True
        Else
            div_termos.Visible = False
        End If

        If cmbMarca.SelectedValue <> 0 Then
            If tbTelefone.Text.Replace(" ", "") <> "" Then
                div_troca.Visible = True
                div_desvincula.Visible = True
            End If
        End If

    End Sub

    Private Function SalvaRegistro(ByRef msg As String) As Boolean
        Dim _registro As New AppAparelhosMoveis()
        Dim log_string As New List(Of String)
        Dim ccustos_list As New List(Of String)
        Dim list_facilidades As String()
        Dim list_projetos As String()
        Dim list_ccusto As String()

        'Carrega string do log 
        'log_string.Add(Session("username_login"))

        '_registro.Codigo = tbCodigo.Text

        _registro.Codigo = ViewState("codigo")

        _registro.Contrato = tbContrato.Text
        _registro.Classificacao = cmbClassificacao.SelectedValue
        _registro.Plano = cmbPlanos.SelectedValue
        _registro.Pin1 = tbPIN1.Text
        _registro.Pin2 = tbPIN2.Text
        _registro.Puk1 = tbPUK1.Text
        _registro.Puk2 = tbPUK2.Text
        _registro.Ip = tbIp.Text
        _registro.Telefone = tbTelefone.Text.Replace("_", "").Replace("()-", "").Replace(" ", "").Replace("-", "")
        _registro.Fleet = tbFleet.Text
        _registro.Simcard = tbSIMCARD.Text
        _registro.Simcard_value = tbSIMCARD_value.Text
        If tbDt_ativ.Text <> "" Then
            _registro.Ativacao = tbDt_ativ.Text.Substring(0, 10)
        End If
        If tbDt_des.Text <> "" Then
            _registro.Desativacao = tbDt_des.Text.Substring(0, 10)
        End If
        _registro.Status = cmbStatus.SelectedValue
        _registro.Pin_Aparelho = tbPin_Aparelho.Text.Trim

        'Facilidades
        'Projetos

        _registro.Codigo_cliente = tbCodigo_cliente.Text

        If tbIntragrupo.Checked = True Then
            _registro.Intragrupo = "S"
        Else
            _registro.Intragrupo = "N"
        End If

        _registro.Tecnologia = cmbTecnologia.SelectedIndex
        _registro.Marca = cmbMarca.SelectedValue
        _registro.Modelo = cmbModelo.SelectedValue
        _registro.Codigo_aparelho = tbCodigo_aparelho.Text
        _registro.Valor_aparelho = tbValor_aparelho.Text.Replace(".", "")
        _registro.Nota_fiscal = tbNotaFiscal.Text
        _registro.Identificacao = tbIMEI.Text
        If tbVencimento_Comodato.Text <> "" Then
            _registro.Venc_comodato = tbVencimento_Comodato.Text.Substring(0, 10)
        End If
        If tbVencimento_Garantia.Text <> "" Then
            _registro.Venc_garantia = tbVencimento_Garantia.Text.Substring(0, 10)
        End If

        _registro.Sucursal = txtSucursal_code.Text

        If tbEstoque.Checked = True Then
            _registro.Estoque = "S"
        Else
            _registro.Estoque = "N"
        End If

        If btnBackup.Checked = True Then
            _registro.Backup = "S"
            _registro.Emissao = tbEmissão.Text
            _registro.Prop_estoque = tbProp_Estoque.Text
            _registro.Ordem_serv = tbOrdem_Serviço.Text
        Else
            _registro.Backup = "N"
            _registro.Emissao = ""
            _registro.Prop_estoque = ""
            _registro.Ordem_serv = ""
        End If

        If btnSucata.Checked = True Then
            _registro.Sucata = "S"
        Else
            _registro.Sucata = "N"
        End If
        _registro.Chamada_retirada = tbChamado_Retirada.Text
        _registro.Data_retirada = tbData_Retirada.Text

        If btnPerdido.Checked = True Then
            _registro.Perdido = "S"
        Else
            _registro.Perdido = "N"
        End If

        _registro.Natureza = cmbNatureza.SelectedIndex
        _registro.Usuario = tb_user_code.Text
        _registro.Limite_uso = tbLimite_Uso.Text
        _registro.Protocolo_cancel = tbProtocolo_cancel.Text
        'CENTRO DE CUSTO
        _registro.Obs = tbOBS.Text
        _registro.Obs_aparelho = tbObs_aparelho.Text
        _registro.Operadora = cmbOperadora.SelectedValue
        _registro.Serial_Number = tbSerialNumber.Text
        _registro.Conta_cont = cmbContaContabil.SelectedValue
        '_registro.Chamado = tbChamado.Text
        'Preenche listas com valores dos GridsViews

        _registro.QTD_parcel = cmbQtdParcel.SelectedValue
        _registro.Inicio_Parcel = tbInicioParcl.Text

        If chkMostraParcela.Checked = True Then
            _registro.Exibe_parcel_rel = "S"
        Else
            _registro.Exibe_parcel_rel = "N"
        End If

        Dim string_aux As String = ""
        For Each _row As GridViewRow In Me.GvFacilidades.Rows
            If DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True Then
                string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
            End If

        Next

        list_facilidades = string_aux.Split(" ")

        string_aux = ""
        For Each _row As GridViewRow In Me.GvProjetos.Rows
            If DirectCast(_row.Cells(1).FindControl("chkProjeto"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True Then
                string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
            End If

        Next

        list_projetos = string_aux.Split(" ")

        If pnCCusto_Editable.Visible = True Then '************************* CENTRO DE CUSTOS - EDITAVEIS ***************************************************

            string_aux = ""
            For Each _row As GridViewRow In Me.GvCCustos.Rows
                string_aux = string_aux + " " + _row.Cells(1).Text
            Next

            list_ccusto = string_aux.Split(" ")
        Else
            list_ccusto = New String() {lbCCusto_code.Text}
        End If

        If ViewState("codigo") = "" Then

            If _dao_lin.InsereLinhaMovel(_registro, _dao.GetCodOperadorasByFornec(cmbOperadora.SelectedValue), list_facilidades, list_projetos, list_ccusto, Session("username_login"), msg) <> True Then
                Return False
            End If
        Else
            If _dao_lin.AlteraAparelho(_registro, _dao.GetCodOperadorasByFornec(cmbOperadora.SelectedValue), list_facilidades, list_projetos, list_ccusto, Session("username_login"), msg, AlterLog.Checked) <> True Then
                Return False
            Else
                'If lbChamado.Text = "Chamado Selecionado" Or lbUltimoOEM.Text <> "Sem chamado" Then
                '    Dim list_aux As New List(Of AppGeneric)
                '    list_aux.Add(New AppGeneric("codigo_tipo", "1"))

                '    _dao_his.GenericInsert(New AppGeneric(lbUltimoOEM.Text, ViewState("codigo")), "", "", "oem", "codigo_item", "chamados_items", "", list_aux)

                'End If
            End If
        End If

        btGravar_novo.Visible = True

        Return True
    End Function

    Private Function SalvarNovo(ByRef msg As String) As Boolean
        Dim _registro As New AppAparelhosMoveis()
        Dim log_string As New List(Of String)
        Dim ccustos_list As New List(Of String)
        Dim list_facilidades As String()
        Dim list_projetos As String()
        Dim list_ccusto As String()

        'Carrega string do log 
        'log_string.Add(Session("username_login"))

        '_registro.Codigo = tbCodigo.Text

        _registro.Codigo = ViewState("codigo")

        _registro.Contrato = tbContrato.Text
        _registro.Classificacao = cmbClassificacao.SelectedValue
        _registro.Plano = cmbPlanos.SelectedValue
        _registro.Pin1 = tbPIN1.Text
        _registro.Pin2 = tbPIN2.Text
        _registro.Puk1 = tbPUK1.Text
        _registro.Puk2 = tbPUK2.Text
        _registro.Ip = tbIp.Text
        _registro.Telefone = tbTelefone.Text.Replace("_", "").Replace("()-", "").Replace(" ", "").Replace("-", "")
        _registro.Fleet = tbFleet.Text
        _registro.Simcard = tbSIMCARD.Text
        _registro.Simcard_value = tbSIMCARD_value.Text
        If tbDt_ativ.Text <> "" Then
            _registro.Ativacao = tbDt_ativ.Text.Substring(0, 10)
        End If
        If tbDt_des.Text <> "" Then
            _registro.Desativacao = tbDt_des.Text.Substring(0, 10)
        End If
        _registro.Status = cmbStatus.SelectedIndex
        _registro.Pin_Aparelho = tbPin_Aparelho.Text

        'Facilidades
        'Projetos

        _registro.Codigo_cliente = tbCodigo_cliente.Text

        If tbIntragrupo.Checked = True Then
            _registro.Intragrupo = "S"
        Else
            _registro.Intragrupo = "N"
        End If

        _registro.Tecnologia = cmbTecnologia.SelectedIndex
        _registro.Marca = cmbMarca.SelectedValue
        _registro.Modelo = cmbModelo.SelectedValue
        _registro.Codigo_aparelho = tbCodigo_aparelho.Text
        _registro.Valor_aparelho = tbValor_aparelho.Text.Replace(".", "")
        _registro.Nota_fiscal = tbNotaFiscal.Text
        _registro.Identificacao = tbIMEI.Text
        If tbVencimento_Comodato.Text <> "" Then
            _registro.Venc_comodato = tbVencimento_Comodato.Text.Substring(0, 10)
        End If
        If tbVencimento_Garantia.Text <> "" Then
            _registro.Venc_garantia = tbVencimento_Garantia.Text.Substring(0, 10)
        End If

        If tbEstoque.Checked = True Then
            _registro.Estoque = "S"
        Else
            _registro.Estoque = "N"
        End If

        If btnBackup.Checked = True Then
            _registro.Backup = "S"
            _registro.Emissao = tbEmissão.Text
            _registro.Prop_estoque = tbProp_Estoque.Text
            _registro.Ordem_serv = tbOrdem_Serviço.Text
        Else
            _registro.Backup = "N"
            _registro.Emissao = ""
            _registro.Prop_estoque = ""
            _registro.Ordem_serv = ""
        End If

        If btnSucata.Checked = True Then
            _registro.Sucata = "S"
        Else
            _registro.Sucata = "N"
        End If
        _registro.Chamada_retirada = tbChamado_Retirada.Text
        _registro.Data_retirada = tbData_Retirada.Text

        If btnPerdido.Checked = True Then
            _registro.Perdido = "S"
        Else
            _registro.Perdido = "N"
        End If

        _registro.Natureza = cmbNatureza.SelectedIndex
        _registro.Usuario = tb_user_code.Text
        _registro.Limite_uso = tbLimite_Uso.Text
        _registro.Protocolo_cancel = tbProtocolo_cancel.Text
        'CENTRO DE CUSTO
        _registro.Obs = tbOBS.Text
        _registro.Operadora = cmbOperadora.SelectedValue
        _registro.Serial_Number = tbSerialNumber.Text
        _registro.Conta_cont = cmbContaContabil.SelectedValue
        _registro.Chamado = tbChamado.Text

        'Preenche listas com valores dos GridsViews

        Dim string_aux As String = ""
        For Each _row As GridViewRow In Me.GvFacilidades.Rows
            If DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True Then
                string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
            End If

        Next

        list_facilidades = string_aux.Split(" ")

        string_aux = ""
        For Each _row As GridViewRow In Me.GvProjetos.Rows
            If DirectCast(_row.Cells(1).FindControl("chkProjeto"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True Then
                string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
            End If

        Next

        list_projetos = string_aux.Split(" ")

        If pnCCusto_Editable.Visible = True Then '************************* CENTRO DE CUSTOS - EDITAVEIS ***************************************************

            string_aux = ""
            For Each _row As GridViewRow In Me.GvCCustos.Rows
                string_aux = string_aux + " " + _row.Cells(1).Text
            Next

            list_ccusto = string_aux.Split(" ")
        Else
            list_ccusto = New String() {lbCCusto_code.Text}
        End If

        'If lbChamado.Text = "Chamado Selecionado" Or lbUltimoOEM.Text <> "Sem chamado" Then
        '    _registro.Chamado = lbUltimoOEM.Text
        'End If

        If _dao_lin.InsereLinhaMovel(_registro, _dao.GetCodOperadorasByFornec(cmbOperadora.SelectedValue), list_facilidades, list_projetos, list_ccusto, Session("username_login"), msg) <> True Then
            Return False
        End If

        Return True
    End Function


    Sub TextBoxTextChanged_Handler(ByVal sender As Object, ByVal e As EventArgs)
        Dim TextBox As TextBox = DirectCast(sender, TextBox)
        Response.Write(TextBox.ID.ToString & " foi alterado")
    End Sub


    Protected Sub btGravar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btGravar.Click
        If Page.IsValid Then

            Dim script As String = "<script>"
            Dim msg As String = ""

            If (tbIMEI.Text = "" And tbSIMCARD.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O IMEI ou Número do SIMCARD devem ser fornecidos'});"

            ElseIf cmbMarca.SelectedValue <> 0 And (cmbClassificacao.SelectedValue = "9" And tbIMEI.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O IMEI deve ser fornecido para linha do tipo movel'});"
            ElseIf (tb_user_code.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O usuário deve ser fornecido'});"
            ElseIf (cmbOperadora.SelectedValue = "0") Then
                script = script & "Lobibox.notify('error', {msg:'Selecione uma operadora'});"
                'end if if (cmbPlanos.SelectedValue = "0") Then
                '    Response.Write("<script>Lobibox.notify('error', {msg:'O Plano da linha deve ser selecionado'});"
            ElseIf (cmbStatus.SelectedValue = "ATIVADO" And tbDt_ativ.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'Forneça a data de ativação'});"
            ElseIf (cmbStatus.SelectedValue = "DESATIVADO" And tbDt_des.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'Forneça a data de desativação'});"
            Else
                If SalvaRegistro(msg) And script = "<script>" Then
                    script = "<script>window.opener.jQuery('#list1').trigger('reloadGrid');Lobibox.notify('success', {msg:'Operação realizada com sucesso'});"
                    If AppIni.GloboRJ_Parm = True Then
                        btnTermoGLOBO.Visible = True
                    End If
                Else
                    script = script & "Lobibox.notify('error', {msg:'" + msg + "'});"
                End If
            End If

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", script & "</script>")

        End If
    End Sub

    Protected Sub btGravar_novo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btGravar_novo.Click
        If Page.IsValid Then

            Dim script As String = "<script>"
            Dim msg As String = ""

            If (tbIMEI.Text = "" And tbSIMCARD.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O IMEI ou Número do SIMCARD devem ser fornecidos'});"

            ElseIf cmbMarca.SelectedValue <> 0 And (cmbClassificacao.SelectedValue = "9" And tbIMEI.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O IMEI deve ser fornecido para linha do tipo movel'});"
            ElseIf (tb_user_code.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'O usuário deve ser fornecido'});"
            ElseIf (cmbOperadora.SelectedValue = "0") Then
                script = script & "Lobibox.notify('error', {msg:'Selecione uma operadora'});"
                'end if if (cmbPlanos.SelectedValue = "0") Then
                '    Response.Write("<script>Lobibox.notify('error', {msg:'O Plano da linha deve ser selecionado'});"
            ElseIf (cmbStatus.SelectedValue = "ATIVADO" And tbDt_ativ.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'Forneça a data de ativação'});"
            ElseIf (cmbStatus.SelectedValue = "DESATIVADO" And tbDt_des.Text = "") Then
                script = script & "Lobibox.notify('error', {msg:'Forneça a data de desativação'});"
            Else
                If SalvarNovo(msg) Then
                    script = "<script>Lobibox.notify('success',  {msg:'Operação realizada com sucesso'});window.opener.jQuery('#list1').trigger('reloadGrid'); window.location.href = window.location.href.replace('?codigo=" & tbCodigo.Text & "','?codigo=" & _dao_lin.GetLinhaCodeByNumber(tbTelefone.Text) & "');"
                Else
                    script = script & "Lobibox.notify('error', {msg:'" + msg + "'});"
                End If
            End If

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", script & "</script>")
        End If
    End Sub

    Protected Sub btExcluir_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btExcluir.Click
        'Carrega string do log 
        If _dao_lin.DeletarAparelho(_dao_lin.GetMovelById(ViewState("codigo")).Item(0), Session("username_login"), "") Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('success',  {msg:'Operação realizada com sucesso'});window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'ERRO ! Operação NÃO realizada!'});window.opener.jQuery('#list1').trigger('reloadGrid');window.close();</script>")
        End If
    End Sub

    Private Sub carregaTecnologia()

        Dim listOP As New List(Of String)

        listOP.Insert(0, "SELECIONE")
        listOP.Insert(1, "CMDA")
        listOP.Insert(2, "TDMA")
        listOP.Insert(3, "GSM")
        listOP.Insert(4, "RADIO")
        listOP.Insert(5, "GPRS")

        cmbTecnologia.DataSource = listOP
        cmbTecnologia.DataBind()

    End Sub

    Private Sub carregaNatureza()

        Dim listOP As New List(Of String)

        listOP.Insert(0, "SELECIONE")
        listOP.Insert(1, "Próprio")
        listOP.Insert(2, "Comodato")
        listOP.Insert(3, "Locado")

        cmbNatureza.DataSource = listOP
        cmbNatureza.DataBind()

    End Sub

    Private Sub carregaOperadora()
        Dim listOP As List(Of AppOperadoras)
        listOP = _dao.GetFornecedoresOperadoras()

        listOP.Insert("0", New AppOperadoras("0", "SEM OPERADORA", vbNull, vbNull))

        cmbOperadora.DataSource = listOP
        cmbOperadora.DataBind()

    End Sub

    Private Sub carregaContabil()
        Dim listOP As List(Of AppGeneric)
        listOP = _dao_his.GetGenericList("", "codigo_conta", "codigo_conta", "conta_contabil")

        listOP.Insert("0", New AppGeneric("0", "SEM CONTA"))

        cmbContaContabil.DataSource = listOP
        cmbContaContabil.DataBind()

    End Sub

    Private Sub carregaPlanos()
        Dim planos_code As New List(Of String)
        Dim listOP As New List(Of AppGeneric)
        Dim codigo_op As String
        Dim aux As Integer = 0

        codigo_op = _dao.GetCodOperadorasByFornec(cmbOperadora.SelectedValue)
        _dao.ComboOperadorasPlanos(codigo_op, listOP)

        listOP.Insert(0, New AppGeneric("0", "SEM PLANO"))

        cmbPlanos.DataSource = listOP
        cmbPlanos.DataBind()
    End Sub

    Private Sub carrega_classificacao()
        Dim listOP As New List(Of AppGeneric)

        listOP = _dao_lin.GetClassificacao()

        cmbClassificacao.DataSource = listOP
        cmbClassificacao.DataBind()

    End Sub

    Private Sub carregaStatus()
        Dim list As New List(Of AppGeneric)
        Dim status_code As New List(Of String)
        Dim listStatus As New List(Of String)
        Dim aux As Integer = 0


        list = _dao_his.GetGenericList("", "codigo_status", "descricao", "status_linhas")


        cmbStatus.DataSource = list
        cmbStatus.DataBind()
        cmbStatus.SelectedValue = "ATIVADO"


    End Sub

    Private Sub carregaMarcas()
        Dim listOP As New List(Of AppPesquisa)

        listOP = _dao_his.SearchField("", "MARCA", "COD_MARCA", "APARELHOS_MARCAS", "", "", " order by marca")

        listOP.Insert(0, New AppPesquisa("0", "SEM APARELHO"))

        cmbMarca.DataSource = listOP
        cmbMarca.DataBind()

    End Sub

    Private Sub carregaParcelas()
        Dim listOP As New List(Of AppGeneric)

        For i As Integer = 1 To 24
            listOP.Add(New AppGeneric(i.ToString, i.ToString))
        Next

        cmbQtdParcel.DataSource = listOP
        cmbQtdParcel.DataBind()

        cmbQtdParcel.SelectedValue = "1"

    End Sub

    Private Sub carregaModelos()
        Dim listOP As New List(Of AppGeneric)

        listOP = _dao_lin.GetComboModelos(cmbMarca.SelectedValue)

        cmbModelo.DataSource = listOP
        cmbModelo.DataBind()


    End Sub

    Protected Sub cmbMarca_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbMarca.SelectedIndexChanged
        sem_aparelho()
    End Sub

    Protected Sub sem_aparelho()
        If cmbMarca.SelectedValue = 0 Then
            PnSemAparelho.Visible = False
            PnSucata.Visible = False
            PnBackup.Visible = False
            cmbTecnologia.Enabled = False
            cmbTecnologia.SelectedValue = "SELECIONE"
            cmbModelo.Enabled = False
            cmbNatureza.Enabled = False
            tbPin_Aparelho.Enabled = False
            tbPin_Aparelho.Text = ""
            tbValor_aparelho.Enabled = False
            tbValor_aparelho.Text = ""
            tbNotaFiscal.Enabled = False
            tbNotaFiscal.Text = ""
            tbIMEI.Enabled = False
            tbIMEI.Text = ""
            tbVencimento_Garantia.Enabled = False
            tbVencimento_Garantia.Text = ""
            tbVencimento_Comodato.Enabled = False
            tbVencimento_Comodato.Text = ""
            tbEstoque.Enabled = False
            tbEstoque.Text = ""
            btnSucata.Checked = False
            btnSucata.Enabled = False
            btnBackup.Checked = False
            btnBackup.Enabled = False
            btnPerdido.Checked = False
            btnPerdido.Enabled = False
            tbSerialNumber.Enabled = False
            div_troca.Visible = False
            div_desvincula.Visible = False

            If tbTelefone.Text.Replace(" ", "") <> "" Then
                div_vincula.Visible = True
            End If

        Else
            PnSemAparelho.Visible = True
            PnSucata.Visible = True
            PnBackup.Visible = True
            cmbTecnologia.Enabled = True
            cmbModelo.Enabled = True
            cmbNatureza.Enabled = True
            tbPin_Aparelho.Enabled = True
            tbValor_aparelho.Enabled = True
            tbNotaFiscal.Enabled = True
            tbIMEI.Enabled = True
            tbVencimento_Garantia.Enabled = True
            tbVencimento_Comodato.Enabled = True
            tbEstoque.Enabled = True
            btnSucata.Enabled = True
            btnBackup.Enabled = True
            btnPerdido.Enabled = True
            tbSerialNumber.Enabled = True
            carregaModelos()
            cmbTecnologia.SelectedIndex = 3
            If tbTelefone.Text.Replace(" ", "") <> "" Then
                div_troca.Visible = True
                div_desvincula.Visible = True
            End If
            div_vincula.Visible = False
        End If

    End Sub

    Protected Sub cmbOperadora_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmbOperadora.SelectedIndexChanged
        carregaPlanos()

        'Dim dt As New DataTable

        'Session("GvFacilidades") = ""
        'GvFacilidades.DataSource = _dao_his.GetGenericList("", "codigo_vas", "nome", "vas", "", " and codigo_operadora=(select codigo_operadora from fornecedores where codigo='" + cmbOperadora.SelectedValue + "') order by nome")
        'dt = DAO_Commons.ConvertToDataTable(GvFacilidades.DataSource)
        'dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        'dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        'GvFacilidades.DataSource = dt

        'Session("GvFacilidades") = dt
        'GvFacilidades.DataBind()

        Session("GvFacilidades") = ""
        'GvFacilidades.DataSource = _dao_his.GetGenericList("", "codigo_vas", "nome", "vas", "", " and codigo_operadora=(select codigo_operadora from fornecedores where codigo='" + cmbOperadora.SelectedValue + "') order by nome")
        Dim dt As DataTable = _dao_his.myDataTable("select codigo_vas CODIGO,nome DESCRICAO, valor from vas where codigo_operadora=(select codigo_operadora from fornecedores where codigo='" + cmbOperadora.SelectedValue + "') order by nome")
        'dt = DAO_Commons.ConvertToDataTable(GvFacilidades.DataSource)
        'dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        'dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        GvFacilidades.DataSource = dt

        Session("GvFacilidades") = dt
        GvFacilidades.DataBind()
    End Sub

    Protected Sub bbtnPesquisarFacilidade_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnPesquisarFacilidade.Click
        Dim op_code As String
        op_code = _dao_op.GetCodOperadorasByFornec(cmbOperadora.SelectedValue)
        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoPesquisarCampo.aspx?table=VAS&name=NOME&code_field=CODIGO_VAS&titulo=Facilidade&op_campo=CODIGO_OPERADORA&op_valor=" + op_code + "', 'Busca' ,'width=310,height=200,scrollbars=1');void(0);</script>")

    End Sub

    Public Sub GridViewPopulator()
        'Dim dt As New DataTable

        'Carrega Grid de Facilidades

        Session("GvFacilidades") = ""
        'GvFacilidades.DataSource = _dao_his.GetGenericList("", "codigo_vas", "nome", "vas", "", " and codigo_operadora=(select codigo_operadora from fornecedores where codigo='" + cmbOperadora.SelectedValue + "') order by nome")
        Dim dt As DataTable = _dao_his.myDataTable("select codigo_vas CODIGO,nome DESCRICAO, valor from vas where codigo_operadora=(select codigo_operadora from fornecedores where codigo='" + cmbOperadora.SelectedValue + "') order by nome")
        'dt = DAO_Commons.ConvertToDataTable(GvFacilidades.DataSource)
        'dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        'dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        GvFacilidades.DataSource = dt

        Session("GvFacilidades") = dt
        GvFacilidades.DataBind()

        ''Carrega Grid de Projetos

        Session("GvProjetos") = ""
        GvProjetos.DataSource = _dao_his.GetGenericList("", "codigo_projeto", "nome", "projetos", "", " order by nome ")
        dt = DAO_Commons.ConvertToDataTable(GvProjetos.DataSource)
        dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        GvProjetos.DataSource = dt

        Session("GvProjetos") = dt
        GvProjetos.DataBind()

        ''Carrega Grid de CCUSTOS

        Session("GvCCustos") = ""
        GvCCustos.DataSource = _dao_his.GetGenericList("", "codigo", "nome_grupo", "grupos", "", " and nome_grupo=''")
        dt = DAO_Commons.ConvertToDataTable(GvCCustos.DataSource)
        dt.Columns.Item("_DESCRICAO").ColumnName = "DESCRICAO"
        dt.Columns.Item("_CODIGO").ColumnName = "CODIGO"
        GvCCustos.DataSource = dt

        Session("GvCCustos") = dt
        GvCCustos.DataBind()

        ViewState("First_run") = 1
    End Sub

    Protected Sub btnAddCCusto_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCCusto.Click

        If pnCCusto_Editable.Visible = True Then

            For Each _row As GridViewRow In GvCCustos.Rows
                If tbCCusto_codes.Text = DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString Then
                    Return
                End If
            Next

            Dim dt As New DataTable

            If ViewState("First_run") = 0 Then
                GridViewPopulator()
                dt = Session("GvCCustos")
                If dt.Rows.Count > 0 Then
                    dt.Clear()
                End If
            Else
                dt = Session("GvCCustos")
            End If

            Dim newRow As DataRow = dt.NewRow()
            Dim tipo_tarifa As String = ""

            'newRow("CODIGO") = 
            newRow("CODIGO") = tbCCusto_codes.Text
            newRow("DESCRICAO") = tbCCusto.Text

            dt.Rows.Add(newRow)
            GvCCustos.DataSource = dt
            Session("GvCCustos") = GvCCustos.DataSource
            GvCCustos.DataBind()
            ViewState("First_run") = 1

        End If

    End Sub

    Protected Sub btnAddUser_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddUser.Click

        If AppIni.CCusto_Editable = False Then
            lbCCusto_code.Text = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0).GRP_Codigo
            lbCCusto.Text = _dao_grupos.GetGruposById(_dao_user.GetUsuarioById(tb_user_code.Text).Item(0).GRP_Codigo).Item(0).Grupo
            CarregaFoto(tb_user_code.Text)
        End If

        If tb_user_code.Text <> "" And Not String.IsNullOrEmpty(Request.QueryString("codigo")) Then
            div_termos.Visible = True
        Else
            div_termos.Visible = False
        End If

        AlterLog.Checked = True

    End Sub

    Protected Sub btndesvincular_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btndesvincular.Click

        Dim _registro As New AppAparelhosMoveis()
        Dim log_string As New List(Of String)
        Dim ccustos_list As New List(Of String)
        Dim msg As String = ""
        Dim list_empty As String() = msg.Split(" ")


        _registro.Codigo = ViewState("codigo")

        _registro.Classificacao = cmbClassificacao.SelectedValue
        _registro.Plano = "0"
        _registro.Telefone = ""
        _registro.Status = "0"
        _registro.Pin_Aparelho = tbPin_Aparelho.Text


        _registro.Tecnologia = cmbTecnologia.SelectedIndex
        _registro.Marca = cmbMarca.SelectedValue
        _registro.Modelo = cmbModelo.SelectedValue
        _registro.Valor_aparelho = tbValor_aparelho.Text
        _registro.Nota_fiscal = tbNotaFiscal.Text
        _registro.Identificacao = tbIMEI.Text
        If tbVencimento_Comodato.Text <> "" Then
            _registro.Venc_comodato = tbVencimento_Comodato.Text.Substring(0, 10)
        End If
        If tbVencimento_Garantia.Text <> "" Then
            _registro.Venc_garantia = tbVencimento_Garantia.Text.Substring(0, 10)
        End If

        If tbEstoque.Checked = True Then
            _registro.Estoque = "S"
        Else
            _registro.Estoque = "N"
        End If

        If btnBackup.Checked = True Then
            _registro.Backup = "S"
            _registro.Emissao = tbEmissão.Text
            _registro.Prop_estoque = tbProp_Estoque.Text
            _registro.Ordem_serv = tbOrdem_Serviço.Text
        Else
            _registro.Backup = "N"
            _registro.Emissao = ""
            _registro.Prop_estoque = ""
            _registro.Ordem_serv = ""
        End If

        If btnSucata.Checked = True Then
            _registro.Sucata = "S"
        Else
            _registro.Sucata = "N"
        End If
        _registro.Chamada_retirada = tbChamado_Retirada.Text
        _registro.Data_retirada = tbData_Retirada.Text

        If btnPerdido.Checked = True Then
            _registro.Perdido = "S"
        Else
            _registro.Perdido = "N"
        End If

        _registro.Natureza = cmbNatureza.SelectedIndex
        _registro.Serial_Number = tbSerialNumber.Text

        cmbMarca.SelectedValue = "0"
        sem_aparelho()

        If SalvaRegistro("") Then
            If _dao_lin.InsereLinhaMovel(_registro, _dao.GetCodOperadorasByFornec(cmbOperadora.SelectedValue), list_empty, list_empty, list_empty, Session("username_login"), msg) <> True Then
                ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'" + msg + "'});window.opener.location.reload();</script>")
            End If
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('success',  {msg:'Operação realizada com sucesso'});window.opener.jQuery('#list1').trigger('reloadGrid');</script>")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'ERRO ! Operação NÃO realizada!'});window.opener.jQuery('#list1').trigger('reloadGrid');</script>")
        End If

    End Sub


    Protected Sub GvCCustos_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GvCCustos.RowCommand
        If e.CommandName = "Excluir" Then
            Dim dt As New DataTable

            dt = Session("GvCCustos")

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            dt.Rows(index).Delete()
            'Response.Write("<script>Lobibox.notify('error', {msg:'Excluir: " + index + "!'});</script>")
            GvCCustos.DataSource = dt
            Session("GvCCustos") = GvCCustos.DataSource
            GvCCustos.DataBind()
        End If

    End Sub

    Protected Sub btTermo_GLOBO_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnTermoGLOBO.Click

        Dim usuario As New AppUsuarios
        Dim modelo As String = ""

        If cmbModelo.SelectedItem IsNot Nothing Then
            modelo = cmbModelo.SelectedItem.Text
        End If
        usuario = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoTermoAparelhoGlobo.aspx?&nome=" & tb_user_code.Text & "&linha=" & tbTelefone.Text.Replace("_", "") & "&simcard=" & tbSIMCARD.Text & "&imei=" & tbIMEI.Text & "&marca=" + cmbMarca.SelectedItem.Text + "&modelo=" & IIf(cmbMarca.SelectedValue <> "0", modelo, "SEM MODELO") & "', 'Termo', 'width=800,height=600,scrollbars=1,resizable=yes');void(0);</script>")
    End Sub

    Protected Sub btTermo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTermo.Click

        Dim usuario As New AppUsuarios
        Dim modelo As String = ""

        If cmbModelo.SelectedItem IsNot Nothing Then
            modelo = cmbModelo.SelectedItem.Text
        End If
        usuario = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('termodoc.asp?operacao=8&usuario=" & tbUsuario.Text & "&numlinha=" & tbTelefone.Text.Replace("_", "") & "&simcard=" & tbSIMCARD.Text & "&imei=" & tbIMEI.Text & "&marca=" + cmbMarca.SelectedItem.Text + "&modelo=" + IIf(cmbMarca.SelectedValue <> "0", modelo, "SEM MODELO") + "&cpf=" & usuario.CPF & "&endereco=" & usuario.Endereco & "&municipio=' + escape('" & _dao_his.RetornaCidade(usuario.Codigo_Cidade.ToString) & "') + '&bairro=" & usuario.Bairro & "&uf=" & usuario.uf & "&numero=" & usuario.Numero & "&ano=" & Date.Now.Year.ToString & "','_blank', 'Termo', 'width=600,height=600,scrollbars=1,resizable=yes');window.open('GestaoTermoAnexo.aspx?usuario=" & usuario.Codigo & "&municipio=" & _dao_his.RetornaCidade(usuario.Codigo_Cidade.ToString) & "&bairro=" & usuario.Bairro & "&uf=" & usuario.uf & "','_blank', 'Termo_anexo', 'width=600,height=600,scrollbars=1,resizable=yes');void(0);</script>")
    End Sub

    Protected Sub btTermo_2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTermo_2.Click

        Dim usuario As New AppUsuarios

        usuario = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('Gestao_Termo_Linha.aspx?code_line=" & tbCodigo.Text & "&numlinha=" & tbTelefone.Text.Replace("_", "") & "&usuario=" & tb_user_code.Text & "&endereco=" & usuario.Endereco & "&municipio=" & usuario.Municipio & "&bairro=" & usuario.Bairro & "&uf=" & usuario.uf & "&numero=" & usuario.Numero & "', 'Termo', 'width=600,height=600,scrollbars=1,resizable=yes');void(0);</script>")
    End Sub

    Protected Sub btTermo_3_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTermo_3.Click

        Dim usuario As New AppUsuarios
        Dim modelo As String = ""

        If cmbModelo.SelectedItem IsNot Nothing Then
            modelo = cmbModelo.SelectedItem.Text
        End If

        usuario = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('termo_devolucao.asp?operacao=8&usuario=" & tbUsuario.Text & "&numlinha=" & tbTelefone.Text.Replace("_", "") & "&simcard=" & tbSIMCARD.Text & "&imei=" & tbIMEI.Text & "&marca=" + cmbMarca.SelectedItem.Text + "&modelo=" + IIf(cmbMarca.SelectedValue <> "0", modelo, "SEM MODELO") + "&cpf=" & usuario.CPF & "&endereco=" & usuario.Endereco & "&municipio=" & _dao_his.RetornaCidade(usuario.Codigo_Cidade.ToString) & "&bairro=" & usuario.Bairro & "&uf=" & usuario.uf & "&numero=" & usuario.Numero & "&ano=" & Date.Now.Year.ToString & "', 'Termo', 'width=600,height=600,scrollbars=1,resizable=yes');void(0);</script>")
    End Sub

    Protected Sub btTermo_4_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTermo_4.Click

        Dim usuario As New AppUsuarios
        Dim modelo As String = ""

        If cmbModelo.SelectedItem IsNot Nothing Then
            modelo = cmbModelo.SelectedItem.Text
        End If

        usuario = _dao_user.GetUsuarioById(tb_user_code.Text).Item(0)

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoTermoDoacao.aspx?usuario=" & usuario.Codigo & "&numlinha=" & tbTelefone.Text.Replace("_", "") & "&simcard=" & tbSIMCARD.Text & "&imei=" & tbIMEI.Text & "&marca=" + cmbMarca.SelectedItem.Text + "&modelo=" + IIf(cmbMarca.SelectedValue <> "0", modelo, "SEM MODELO") + "&cpf=" & usuario.CPF & "&endereco=" & usuario.Endereco & "&municipio=" & _dao_his.RetornaCidade(usuario.Codigo_Cidade.ToString) & "&bairro=" & usuario.Bairro & "&uf=" & usuario.uf & "&numero=" & usuario.Numero & "&ano=" & Date.Now.Year.ToString & "&cep=" & usuario.CEP & "', 'Termo', 'width=600,height=600,scrollbars=1,resizable=yes');void(0);</script>")
    End Sub

    Protected Sub btTermoVonpar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btTermoVonpar.Click

        If cmbMarca.SelectedValue = "0" Then
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'Selecione a marca do aparelho');void(0);</script>")
        Else
            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('Gestao_termo_Vonpar.aspx?cod_usuario=" & tb_user_code.Text & "&numlinha=" & tbTelefone.Text.Replace("_", "") & "&simcard=" & tbSIMCARD.Text & "&marca=" + cmbMarca.SelectedItem.Text + "&modelo=" + cmbModelo.SelectedItem.Text + "', 'Termo', 'width=600,height=600,scrollbars=1,resizable=yes');void(0);</script>")
        End If

    End Sub

    Protected Sub btHistorico_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btHistorico.Click, btHistorico_IMEI.Click, btHistorico_SIM.Click


        Dim data_table As New DataTable
        Dim registrys As New List(Of String)
        'Dim reg_aux As New List(Of AppLinks)
        Dim aux As Integer = 0

        If hidden_tipo.Text = "1" Then
            registrys.Add(tbTelefone.Text)
        ElseIf hidden_tipo.Text = "2" Then
            registrys.Add(tbSIMCARD.Text)
        Else
            registrys.Add(tbIMEI.Text)
        End If

        'Executa processamento do log para obter tabela
        data_table = Resolve_table(registrys, hidden_tipo.Text)

        'Passa contexto para paginá de logs
        Dim context As HttpContext = HttpContext.Current

        Session("Contexto") = "HTML"
        Session("Tabela") = data_table
        Session("Nome") = "Histórico de Linha(s)"

        If hidden_tipo.Text = "1" Then
            Session("HTML_Context") = "<br /> Por número de Linha: <br />"
        ElseIf hidden_tipo.Text = "2" Then
            Session("HTML_Context") = "<br /> Por SIMCARD: <br />"
        Else
            Session("HTML_Context") = "<br /> Por IMEI: <br />"
        End If

        For Each item As String In registrys
            Session("HTML_Context") = Session("HTML_Context") + item + "  "
        Next

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoHistoricos.aspx');</script>")

    End Sub

    Public Function Resolve_table(ByVal list As List(Of String), ByVal tipo_log As String) As DataTable

        Dim sql As String
        Dim table As New DataTable
        Dim aux As Integer = 0
        Dim aux_2 As Integer = 0
        'Variaveis tiradas CODIGO_LINHA

        sql = "select DATA,decode(nvl(TIPO, ''),'N','Criado','D','Excluído','A','Antes', 'B', 'Depois') as TIPO,AUTOR,replace(replace(replace(NUM_TEL,'(',''),')',''),'-','') as NUM_TEL,CODIGO_GRUPO,us.nome_usuario as usuario,nvl((select op.descricao from operadoras_teste op where op.codigo = f.codigo_operadora), '') as Operadora,nvl(pl.plano, 'SEM PLANO') as PLANO,sl.descricao as STATUS,  to_char(ATIVACAO,'DD/MM/YYYY') as ATIVADO,to_char(DESATIVADO,'DD/MM/YYYY') as DESATIVADO,lm_log.imei as imei, SIM_CARD, OBS,OEM as CHAMADO,VALOR_UNIT, to_char(VENC_GARAN,'DD/MM/YYYY') as VENC_GARANTIA, DESC_ACESS, PIM, PUC, HEXA, TERMO_RESP "
        sql = sql + " , lm_log.CONTRATO, NOTA_FISCAL, VENC_CONTA, COD_CONTA, lm_log.CODIGO, decode(nvl(NATU_OPERACAO, ''),'1','Proprio','2','Comodato','3','Locado') as NAT_OPERACAO, SERVICOS, MENSALIDADE, FLEET, CODIGO_CLIENTE, PIN_APARELHO, PIN2, PUK2, LIMITE_USO "
        sql = sql + " ,ESTOQUE, BACKUP, SUCATA, PROPRIEDADE_ESTOQUE, ORDEM_SERVICO, CHAMADO_RETIRADA, DATA_RETIRADA, to_char(lm_log.EMISSAO,'DD/MM/YYYY') as EMISSAO, PERDIDO, CONTA_CONTABIL "
        sql = sql + " from LINHAS_MOVEIS_LOG lm_log, status_linhas sl, operadoras_planos pl, fornecedores f, aparelhos_marcas ma, aparelhos_modelos mo, usuarios us "
        sql = sql + " where CODIGO_LINHA is not null "
        sql = sql + " and lm_log.codigo_usuario = us.codigo "
        sql = sql + " and sl.codigo_status = lm_log.status(+) "
        sql = sql + " and lm_log.codigo_plano = pl.codigo_plano(+) "
        sql = sql + "and lm_log.COD_MODELO = mo.COD_MODELO(+) "
        sql = sql + "and mo.COD_MARCA = ma.COD_MARCA(+) "
        sql = sql + "and lm_log.CODIGO_FORNECEDOR = f.CODIGO(+) "
        'Query do relatório

        If tipo_log = "1" Then
            sql = sql + " and  replace(replace(replace(replace(replace(lm_log.NUM_TEL, '(', ''), ')', ''), '-', ''), '_', ''),' ','') in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace("(", "").Replace(")", "").Replace("-", "").Replace("_", "").Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        ElseIf tipo_log = "2" Then

            sql = sql + " and lm_log.SIM_CARD in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        Else

            sql = sql + " and lm_log.IMEI in ('' "

            For Each number As String In list
                sql = sql + " , '" + number.Replace(" ", "") + "' "
            Next

            sql = sql + " )"

        End If

        'order
        sql = sql + "order by codigo"

        'Response.Write(sql)
        'Response.End()

        table = _dao_his.myDataTable(sql)

        Return table

    End Function

    Protected Sub GvFacilidades_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GvFacilidades.RowCommand
        If e.CommandName = "Editar" Then
            Dim dt As New DataTable

            dt = Session("GvFacilidades")

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)

            ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoCadastroFacilidades.aspx?codigo=" & dt.Rows(index).Item(0) & "', '','_blank'});</script>")
        End If

    End Sub


    Protected Sub btPlanosFacilidades_Click(sender As Object, e As System.EventArgs) Handles btPlanosFacilidades.Click


        'pega as facilidades daquele plano
        Dim sql As String = "select t.codigo_vas from PLANOS_VAS t where t.codigo_plano='" & Me.cmbPlanos.SelectedValue & "'"
        Dim dt As DataTable = _dao_his.myDataTable(sql)

        'If dt.Rows.Count < 1 Then
        '    'não deixa mudar de plano
        '    Me.cmbPlanos.SelectedValue = ViewState("planoAnterior")
        '    ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>Lobibox.notify('error', {msg:'Este plano não possui facilidades. Favor cadastrar.');</script>")

        'End If


        For Each _row As GridViewRow In Me.GvFacilidades.Rows

            If dt.Rows.Count > 0 Then
                DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = False
            End If


            For Each _item As DataRow In dt.Rows


                Dim string_aux As String = ""
                'For Each _row As GridViewRow In Me.GvFacilidades.Rows
                If DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString = _item.Item(0).ToString Then
                    'marca a facilidade
                    DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True

                End If

                'If DirectCast(_row.Cells(1).FindControl("chkFacilidade"), System.Web.UI.HtmlControls.HtmlInputCheckBox).Checked = True Then
                'string_aux = string_aux + " " + DirectCast(_row.Cells(1).FindControl("chkhidden"), System.Web.UI.HtmlControls.HtmlInputHidden).Value.ToString
                'End If

                'Next

            Next

        Next

    End Sub

    Protected Sub cmbPlanos_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbPlanos.SelectedIndexChanged

        ViewState("planoAnterior") = ViewState("planoAtual")

        ViewState("planoAtual") = Me.cmbPlanos.SelectedValue

    End Sub

    Protected Sub cmbNatureza_SelectedIndexChanged(sender As Object, e As System.EventArgs) Handles cmbNatureza.SelectedIndexChanged

        If cmbNatureza.SelectedValue = "Comodato" Then
            div_comodato.Visible = True
        Else
            div_comodato.Visible = False
        End If

    End Sub

    Protected Sub btnChamadoSelected_Click(sender As Object, e As System.EventArgs) Handles btnChamadoSelected.Click
        lbChamado.Text = "Chamado Selecionado"
        lbChamado.ForeColor = Drawing.Color.Red
        lbChamado.Font.Bold = True
        lbUltimoOEM.Text = tbChamadoSelected.Text

        Dim list_aux As New List(Of AppGeneric)

        Dim script As String = ""

        list_aux.Add(New AppGeneric("codigo_tipo", "1"))
        If _dao_his.GenericInsert(New AppGeneric(lbUltimoOEM.Text, ViewState("codigo")), "", "", "oem", "codigo_item", "chamados_items", "", list_aux) = True Then
            script = "<script>window.opener.jQuery('#list1').trigger('reloadGrid');Lobibox.notify('success', {msg:'Chamado " & lbUltimoOEM.Text & " atribuído com sucesso'});"
        Else
            script = "<script>window.opener.jQuery('#list1').trigger('reloadGrid');Lobibox.notify('error', {msg:'Erro o chamado não pode ser atríbuido ou a linha já possui esse chamado'});"
        End If

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", script & "</script>")

    End Sub

    Protected Sub CarregaFoto(ByVal cod_user As String)

        Dim _bytes As New List(Of Byte())
        Dim image As Byte()

        Try
            _dao_his.GetBytesByField(cod_user, "codigo", "usuarios", "FOTO", _bytes)

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

    Protected Sub btnPostFoto_Click(sender As Object, e As System.EventArgs) Handles btnPostFoto.Click
        Dim dt As New DataTable

        If ViewState("First_run") = 0 Then
            GV_ArquivosPopulator("")
            dt = Session("GvArquivos")
            If dt.Rows.Count > 0 Then
                dt.Clear()
            End If
        Else
            dt = Session("GvArquivos")
        End If

        Dim newRow As DataRow = dt.NewRow()

        For Each row As DataRow In dt.Rows
            If UploadArquivo.PostedFile.FileName = row.Item(1) Then
                Return
            End If
            If UploadArquivo.PostedFile.FileName = "" Then
                Return
            End If
        Next

        newRow("Codigo") = "0"
        newRow("Descricao") = UploadArquivo.PostedFile.FileName.Substring(UploadArquivo.PostedFile.FileName.LastIndexOf("\") + 1)

        Dim _byte(UploadArquivo.PostedFile.InputStream.Length) As Byte
        UploadArquivo.PostedFile.InputStream.Read(_byte, 0, UploadArquivo.PostedFile.InputStream.Length)

        list_Bytes_linemobile.Add(_byte)
        list_name_linemobile.Add(UploadArquivo.PostedFile.FileName)

        dt.Rows.Add(newRow)
        GvFotos.DataSource = dt
        Session("GvArquivos") = GvFotos.DataSource
        GvFotos.DataBind()
        ViewState("First_run") = 1

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>$('tabs li:eq(2)').tab('show');</script>")

    End Sub

    Public Sub GV_ArquivosPopulator(ByVal id As String)
        Dim dt As New DataTable
        Session("GvArquivos") = ""
        If id = "" Then
            GvFotos.DataSource = New List(Of AppGeneric)
            dt = DAO_Commons.ConvertToDataTable(GvFotos.DataSource)
            dt.Columns.Item("_codigo").ColumnName = "CODIGO"
            dt.Columns.Item("_descricao").ColumnName = "DESCRICAO"
            GvFotos.DataSource = dt
        End If

        Session("GvArquivos") = dt
        GvFotos.DataBind()
        ViewState("First_run") = 1
    End Sub

    Protected Sub GvTipoLig_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles GvFotos.RowCommand
        If e.CommandName = "Excluir" Then
            Dim dt As New DataTable

            dt = Session("GvArquivos")

            Dim index As Integer = Convert.ToInt32(e.CommandArgument)
            list_name_linemobile.RemoveAt(index)
            list_Bytes_linemobile.RemoveAt(index)
            dt.Rows(index).Delete()
            'Response.Write("<script>alert('Excluir: " + index + "!');</script>")
            GvFotos.DataSource = dt
            Session("GvArquivos") = GvFotos.DataSource
            GvFotos.DataBind()
        End If
    End Sub

    Protected Sub btNovoChamado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btNovoChamado.Click

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoCadastroChamado_Grid.aspx','_blank', 'top=50,left=50,width=940,height=560,location=no,scrollbars=yes,resizablole=no,toolbar=no,directories=no')</script>")

    End Sub

    Protected Sub btVincularChamado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btVincularChamado.Click

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoPesquisarCampo.aspx?table=CHAMADOS p2&name=OEM&code_field=OEM&titulo=Chamados','Busca','width=510,height=200,scrollbars=1');</script>")


    End Sub

    Protected Sub btEditarChamado_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btEditarChamado.Click

        ClientScript.RegisterClientScriptBlock(Me.GetType(), "resultscript", "<script>window.open('GestaoCadastroChamado.aspx?item=" & tbCodigo.Text & "&item_nome=" & tbTelefone.Text & "&tipo=1" & "&page=grid','Busca','width=940,height=600,scrollbars=1')</script>")

    End Sub



End Class



