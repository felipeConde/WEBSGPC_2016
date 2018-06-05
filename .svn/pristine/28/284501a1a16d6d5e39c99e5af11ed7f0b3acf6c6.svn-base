Imports Microsoft.VisualBasic
Imports System.Data
Imports System.Data.OleDb
Imports System.Configuration
Imports System.Collections.Generic
Imports System
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Collections

Public Class DAOUsuarios

    Private _strConn As String = ConfigurationManager.ConnectionStrings("ConnectionString").ConnectionString
    Dim _DAO_commons As New DAO_Commons

    Public Property strConn As String
        Get
            Return _strConn
        End Get
        Set(ByVal value As String)
            _strConn = value
        End Set
    End Property

    Public Function RetornaConexao() As String
        Return strConn
    End Function

    Public Function AtualizaCCUSTO_RAMAL(ByVal ccusto As String, ByVal numero_a As String, ByVal autor As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim cmd As OleDbCommand = connection.CreateCommand

            Dim strSQL As String = " insert into RAMAISLOG(ID, TIPO, DATA, AUTOR, NUMERO_A, GRP_CODIGO, CUSTO_RAMAL, "
            strSQL = strSQL + " TARIFAVEL, ATIVO, CREDITO_MENSAL, SALDO_ATUAL, HISTORICO, DATAHISTORICO, "
            strSQL = strSQL + "BLOQUEAVEL, POSSUI_BLOQUEIO, CATEGORIA)"
            strSQL = strSQL + "values ((select nvl(max(ID),0)+1 FROM RAMAISLOG),"
            'TIPO
            strSQL = strSQL + " 'A',"

            'DATA
            strSQL = strSQL + "to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss'),"
            'AUTOR
            strSQL = strSQL + "'" + autor + "',"
            'NUMERO_A
            strSQL = strSQL + "'" + numero_a + "',"
            'GRP_CODIGO
            strSQL = strSQL + "(Select GRP_CODIGO from ramais where NUMERO_A ='" + numero_a + "' ),"
            'CUSTO_RAMAL
            strSQL = strSQL + "(Select CUSTO_RAMAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'Tarifavel
            strSQL = strSQL + "(Select TARIFAVEL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'ATIVO
            strSQL = strSQL + "(Select ATIVO from ramais where NUMERO_A ='" + numero_a + "' ),"
            'CREDITO_MENSAL
            strSQL = strSQL + "(Select CREDITO_MENSAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'SALDO_ATUAL
            strSQL = strSQL + "(Select SALDO_ATUAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'HISTORICO
            strSQL = strSQL + "'',"
            'DATAHISTORICO
            strSQL = strSQL + "'',"
            'BLOQUEAVEL
            strSQL = strSQL + "(Select BLOQUEAVEL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'POSSUI_BLOQUEIO
            strSQL = strSQL + "(Select POSSUI_BLOQUEIO from ramais where NUMERO_A ='" + numero_a + "' ),"
            'CATEGORIA
            strSQL = strSQL + "(Select CATEGORIA FROM RAMAIS where RAMAIS.NUMERO_A ='" + numero_a + "'))"
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()


            strSQL = "update RAMAIS set grp_codigo='" + ccusto + "' where numero_a='" + numero_a + "'"
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()

            strSQL = " insert into RAMAISLOG(ID, TIPO, DATA, AUTOR, NUMERO_A, GRP_CODIGO, CUSTO_RAMAL, "
            strSQL = strSQL + " TARIFAVEL, ATIVO, CREDITO_MENSAL, SALDO_ATUAL, HISTORICO, DATAHISTORICO, "
            strSQL = strSQL + "BLOQUEAVEL, POSSUI_BLOQUEIO, CATEGORIA)"
            strSQL = strSQL + "values ((select nvl(max(ID),0)+1 FROM RAMAISLOG),"
            'TIPO
            strSQL = strSQL + " 'B',"

            'DATA
            strSQL = strSQL + "to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss'),"
            'AUTOR
            strSQL = strSQL + "'" + autor + "',"
            'NUMERO_A
            strSQL = strSQL + "'" + numero_a + "',"
            'GRP_CODIGO
            strSQL = strSQL + "'" + ccusto + "',"
            'CUSTO_RAMAL
            strSQL = strSQL + "(Select CUSTO_RAMAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'Tarifavel
            strSQL = strSQL + "(Select TARIFAVEL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'ATIVO
            strSQL = strSQL + "(Select ATIVO from ramais where NUMERO_A ='" + numero_a + "' ),"
            'CREDITO_MENSAL
            strSQL = strSQL + "(Select CREDITO_MENSAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'SALDO_ATUAL
            strSQL = strSQL + "(Select SALDO_ATUAL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'HISTORICO
            strSQL = strSQL + "'',"
            'DATAHISTORICO
            strSQL = strSQL + "'',"
            'BLOQUEAVEL
            strSQL = strSQL + "(Select BLOQUEAVEL from ramais where NUMERO_A ='" + numero_a + "' ),"
            'POSSUI_BLOQUEIO
            strSQL = strSQL + "(Select POSSUI_BLOQUEIO from ramais where NUMERO_A ='" + numero_a + "' ),"
            'CATEGORIA
            strSQL = strSQL + "(Select CATEGORIA FROM RAMAIS where RAMAIS.NUMERO_A ='" + numero_a + "'))"
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        Return True
    End Function

    Public Function AtualizaCCUSTOS(ByVal ccusto As String, ByVal CODIGO_USUARIO As String, ByVal autor As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim _daoMoveis As New DAO_LinhasMoveis
            Dim _cod_linhas As List(Of String) = GetLinhasByUsuario(CODIGO_USUARIO)


            For Each codigo_linha As String In _cod_linhas

                Dim cmd As OleDbCommand = connection.CreateCommand
                Dim strSQL As String = ""
                strSQL = _daoMoveis.Gerar_Log_Moveis(codigo_linha.ToString, "A", autor)
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = " delete from grupos_item where item = '" + codigo_linha.ToString + "'"
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = "insert into grupos_item(grupo,modalidade,item) values('" + ccusto.ToString + "', '4','" + codigo_linha.ToString + "')"
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = _daoMoveis.Gerar_Log_Moveis(codigo_linha.ToString, "B", autor)
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
            Next

            _cod_linhas.Clear()
            _cod_linhas = GetLinhasMoveisByUsuario(CODIGO_USUARIO)

            For Each codigo_linha As String In _cod_linhas

                Dim cmd As OleDbCommand = connection.CreateCommand
                Dim strSQL As String = ""
                strSQL = _daoMoveis.Gerar_Log_Moveis(codigo_linha.ToString, "A", autor)
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = " delete from grupos_item where item = '" + codigo_linha.ToString + "'"
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = "insert into grupos_item(grupo,modalidade,item) values('" + ccusto.ToString + "', '4','" + codigo_linha.ToString + "')"
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()

                strSQL = _daoMoveis.Gerar_Log_Moveis(codigo_linha.ToString, "B", autor)
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
            Next

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        connection.Close()
        Return True


    End Function

    Public Function ComboCidades(ByVal Uf As String) As List(Of AppCidades)
        Dim connection As New OleDbConnection(strConn)
        Dim listCidades As New List(Of AppCidades)

        Dim strSQL As String = "select MUNICIPIO, CODIGO_CIDADE, UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "where UF='" + Uf + "' "
        strSQL = strSQL + "order by MUNICIPIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppCidades(reader.Item("CODIGO_CIDADE").ToString, reader.Item("MUNICIPIO").ToString, reader.Item("UF").ToString)
                listCidades.Add(_registro)
            End While
        End Using

        Return listCidades
    End Function

    Public Function ComboRamais() As List(Of AppRamais)
        Dim connection As New OleDbConnection(strConn)
        Dim listRamais As New List(Of AppRamais)

        Dim strSQL As String = "select NUMERO_A from RAMAIS "

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppRamais(reader.Item("NUMERO_A").ToString)
                listRamais.Add(_registro)
            End While
        End Using

        Return listRamais
    End Function

    Public Function ComboUfs() As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listUfs As New List(Of String)

        Dim strSQL As String = "select DISTINCT UF "
        strSQL = strSQL + "from CIDADES "
        strSQL = strSQL + "order by UF"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New String(reader.Item("UF").ToString)
                listUfs.Add(_registro)
            End While
        End Using

        Return listUfs
    End Function

    Public Function ComboUsuarios() As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listUsuarios As New List(Of AppUsuarios)

        Dim strSQL As String = "select NOME_USUARIO, CODIGO "
        strSQL = strSQL + "from USUARIOS "
        strSQL = strSQL + "order by NOME_USUARIO"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader

        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("NOME_USUARIO").ToString)
                listUsuarios.Add(_registro)
            End While
        End Using

        Return listUsuarios
    End Function

    Public Function InsereUsuario(ByVal pgestao_usuario As AppUsuarios, ByVal autorlog As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "insert into USUARIOS(CODIGO"
            strSQL = strSQL + ", NOME_USUARIO, CARGO_USUARIO, LOGIN_USUARIO, RML_NUMERO_A, SENHA_USUARIO, "
            strSQL = strSQL + " EMAIL_USUARIO, EMAIL_SUPERVISOR, RECEBE_EMAIL, SENHA_WEB, RECEBE_RELATORIO, ACESSO_WEB, "
            strSQL = strSQL + " ENDERECO, BAIRRO, NUMERO, COMPLEMENTO, CPF, MATRICULA, ID, CEP, TELEFONE, "
            strSQL = strSQL + " CODIGO_CIDADE, UF,GRP_CODIGO, EXPIRACAO_SENHA_WEB, BLOQUEIO_WEB, "
            strSQL = strSQL + " DIAS_SENHA_EXPIRA, ID_USUARIO_PARENT, CODIGO_UC,  "
            strSQL = strSQL + "  RECEBE_CELULAR, CODIGO_LOCALIDADE, STATUS  "

            '*********************** SULAMERICA *****************************

            If AppIni.Sulamerica_Param = True Then

                strSQL = strSQL + "  ,MATRICULA_SUPERVISOR, VICE  "
                strSQL = strSQL + "  ,DIR, SUPTE, GER, SEC, NUC "
                strSQL = strSQL + " ,DATA_ADMISSAO, DATA_DEMISSAO"
            End If

            '*********************** SULAMERICA *****************************

            strSQL = strSQL + ") values ((select nvl(max(CODIGO),0)+1 from USUARIOS)"
            strSQL = strSQL + ",'" + pgestao_usuario.Nome_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Cargo_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Login_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Rml_Numero_A + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Senha_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Email_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Email_Supervisor + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Recebe_Email + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Senha_Web + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Recebe_Relatorio + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Acesso_Web + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Endereco + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Bairro + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Numero + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Complemento + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.CPF + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Matricula + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.ID_usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.CEP + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Telefone + "'"
            If pgestao_usuario.Codigo_Cidade.ToString <> "0" Then
                strSQL = strSQL + ",'" + pgestao_usuario.Codigo_Cidade.ToString + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.uf + "'"
            Else
                strSQL = strSQL + ",''"
                strSQL = strSQL + ",''"
            End If
            strSQL = strSQL + ",'" + pgestao_usuario.GRP_Codigo + "'"
            strSQL = strSQL + ",to_date('" + pgestao_usuario.Expiracao_Senha_Web + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",to_date('" + pgestao_usuario.Bloqueio_Web + "','dd/mm/yyyy hh24:mi:ss')"
            strSQL = strSQL + ",'" + pgestao_usuario.Dias_Senha_Expira.ToString + "'"
            If pgestao_usuario.ID_Usuario_Parent = 0 Then
                strSQL = strSQL + ",''"
            Else
                strSQL = strSQL + ",'" + pgestao_usuario.ID_Usuario_Parent.ToString + "'"
            End If
            strSQL = strSQL + ",'" + pgestao_usuario.Codigo_UC + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.RecebeCelular + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.CodigoLocalidade + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.STATUS + "'"

            '*********************** SULAMERICA *****************************

            If AppIni.Sulamerica_Param = True Then

                strSQL = strSQL + ",'" + pgestao_usuario.Matricula_sup + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.VICE + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.DIR + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.SUPTE + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.GER + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.SEC + "'"
                strSQL = strSQL + ",'" + pgestao_usuario.NUC + "'"
                strSQL = strSQL + ",to_date('" + pgestao_usuario.DATA_ADMISSAO + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",to_date('" + pgestao_usuario.DATA_DEMISSAO + "','dd/mm/yyyy hh24:mi:ss')"

            End If

            '*********************** SULAMERICA *****************************


            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()


        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        InsertUserLog(pgestao_usuario, "N", autorlog)
        Return True

    End Function

    Public Function InsereRelatórios(ByVal codigo As String, ByVal list_rel As String()) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim _dao_commons As New DAO_Commons
        _dao_commons.strConn = strConn

        Dim item_code As String = codigo

        If codigo = "" Then
            item_code = _dao_commons.GetMaximumCode("CODIGO", "usuarios")
        End If

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = " delete from relatorios_usuarios where codigo_usuario = '" & item_code & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If list_rel.Length > 0 Then
                For Each item As String In list_rel
                    If item <> "" Then
                        strSQL = "insert into relatorios_usuarios(codigo_usuario,codigo_relatorio) values('" + item_code + "','" + item.ToString + "')"
                        cmd.CommandText = strSQL
                        cmd.ExecuteNonQuery()
                    End If
                Next
            End If

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            '_dao_commons.EscreveLog("Erro na Insert Linhas Móveis: " & e.Message)
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        'InsertUserLog(pgestao_usuario, "N", autorlog)
        Return True
    End Function

    Public Function AtualizaUsuario(ByVal pgestao_usuario As AppUsuarios, ByVal autorlog As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        InsertUserLog(GetUsuarioById(pgestao_usuario.Codigo).Item(0), "A", autorlog)

        Try
            Dim strSQL As String = "update USUARIOS set "
            strSQL = strSQL + "NOME_USUARIO='" + pgestao_usuario.Nome_Usuario + "'"
            strSQL = strSQL + ",CARGO_USUARIO='" + pgestao_usuario.Cargo_Usuario + "'"
            strSQL = strSQL + ",LOGIN_USUARIO='" + pgestao_usuario.Login_Usuario + "'"
            strSQL = strSQL + ",RML_NUMERO_A='" + pgestao_usuario.Rml_Numero_A + "'"
            strSQL = strSQL + ",SENHA_USUARIO='" + pgestao_usuario.Senha_Usuario + "'"
            strSQL = strSQL + ",EMAIL_USUARIO='" + pgestao_usuario.Email_Usuario + "'"
            strSQL = strSQL + ",EMAIL_SUPERVISOR='" + pgestao_usuario.Email_Supervisor + "'"
            strSQL = strSQL + ",RECEBE_EMAIL='" + pgestao_usuario.Recebe_Email + "'"
            'strSQL = strSQL + ",SENHA_WEB='" + pgestao_usuario.Senha_Web + "'"
            strSQL = strSQL + ",RECEBE_RELATORIO='" + pgestao_usuario.Recebe_Relatorio + "'"
            strSQL = strSQL + ",ACESSO_WEB='" + pgestao_usuario.Acesso_Web + "'"
            strSQL = strSQL + ",ENDERECO='" + pgestao_usuario.Endereco + "'"
            strSQL = strSQL + ",BAIRRO='" + pgestao_usuario.Bairro + "'"
            strSQL = strSQL + ",NUMERO='" + pgestao_usuario.Numero + "'"
            strSQL = strSQL + ",COMPLEMENTO='" + pgestao_usuario.Complemento + "'"
            strSQL = strSQL + ",MATRICULA='" + pgestao_usuario.Matricula + "'"
            strSQL = strSQL + ",CPF='" + pgestao_usuario.CPF + "'"
            strSQL = strSQL + ",ID='" + pgestao_usuario.ID_usuario + "'"
            strSQL = strSQL + ",CEP='" + pgestao_usuario.CEP + "'"
            strSQL = strSQL + ",TELEFONE='" + pgestao_usuario.Telefone + "'"
            strSQL = strSQL + ",STATUS='" + pgestao_usuario.STATUS + "'"

            If pgestao_usuario.Codigo_Cidade.ToString <> "0" Then
                strSQL = strSQL + ",CODIGO_CIDADE='" + pgestao_usuario.Codigo_Cidade.ToString + "'"
                strSQL = strSQL + ",UF='" + pgestao_usuario.uf + "'"
            Else
                strSQL = strSQL + ",CODIGO_CIDADE=''"
                strSQL = strSQL + ",UF=''"
            End If
            strSQL = strSQL + ",GRP_CODIGO='" + pgestao_usuario.GRP_Codigo + "'"
            'strSQL = strSQL + ",EXPIRACAO_SENHA_WEB= to_date('" + pgestao_usuario.Expiracao_Senha_Web.ToString() + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + ",BLOQUEIO_WEB= to_date('" + pgestao_usuario.Bloqueio_Web.ToString() + "','dd/mm/yyyy hh24:mi:ss')"
            'strSQL = strSQL + ",DIAS_SENHA_EXPIRA='" + pgestao_usuario.Dias_Senha_Expira.ToString + "'"
            If pgestao_usuario.ID_Usuario_Parent = 0 Then
                strSQL = strSQL + ",ID_USUARIO_PARENT=''"
            Else
                strSQL = strSQL + ",ID_USUARIO_PARENT='" + pgestao_usuario.ID_Usuario_Parent.ToString + "'"
            End If
            strSQL = strSQL + ",CODIGO_UC='" + pgestao_usuario.Codigo_UC + "'"
            strSQL = strSQL + ",RECEBE_CELULAR='" + pgestao_usuario.RecebeCelular + "'"
            strSQL = strSQL + ",CODIGO_LOCALIDADE='" + pgestao_usuario.CodigoLocalidade + "'"

            '*********************** SULAMERICA *****************************

            If AppIni.Sulamerica_Param = True Then

                strSQL = strSQL + ",MATRICULA_SUPERVISOR='" + pgestao_usuario.Matricula_sup + "'"
                strSQL = strSQL + ",VICE='" + pgestao_usuario.VICE + "'"
                strSQL = strSQL + ",DIR='" + pgestao_usuario.DIR + "'"
                strSQL = strSQL + ",SUPTE='" + pgestao_usuario.SUPTE + "'"
                strSQL = strSQL + ",GER='" + pgestao_usuario.GER + "'"
                strSQL = strSQL + ",SEC='" + pgestao_usuario.SEC + "'"
                strSQL = strSQL + ",NUC='" + pgestao_usuario.NUC + "'"
                strSQL = strSQL + ",DATA_ADMISSAO= to_date('" + pgestao_usuario.DATA_ADMISSAO + "','dd/mm/yyyy hh24:mi:ss')"
                strSQL = strSQL + ",DATA_DEMISSAO= to_date('" + pgestao_usuario.DATA_DEMISSAO + "','dd/mm/yyyy hh24:mi:ss')"

            End If

            '*********************** SULAMERICA *****************************

            strSQL = strSQL + " where CODIGO = '" + pgestao_usuario.Codigo.ToString + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

        InsertUserLog(pgestao_usuario, "B", autorlog)
        Return True

    End Function

    Public Function RMV_CEL_USER(ByVal pcodigo As Integer) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Try
            Dim strSQL As String = "update linhas_moveis set CODIGO_usuario = ''"
            strSQL = strSQL + " where CODIGO_usuario = '" + Convert.ToString(pcodigo) + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function ExcluiUsuario(ByVal pcodigo As Integer, ByRef error_msg As String, ByVal user_login As String) As Boolean
        Dim connection As New OleDbConnection(strConn)
        Dim User As New List(Of AppUsuarios)

        If RemovingValidator(pcodigo) <> "OK" Then
            error_msg = ("<script>alert('" + RemovingValidator(pcodigo) + "')</script>")
            Return False
        Else
            Try
                User = GetUsuarioById(pcodigo)
                InsertUserLog(User.Item(0), "D", user_login)

                Dim strSQL As String = "delete USUARIOS "
                strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)

                Dim cmd As OleDbCommand = connection.CreateCommand
                cmd.CommandText = strSQL
                connection.Open()
                cmd.ExecuteNonQuery()
                connection.Close()
                cmd.Dispose()
                Return True

            Catch ex As Exception
                Return False
            End Try
        End If
        Return True
    End Function

    Public Function RemovingValidator(ByVal user_code As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim connection1 As New OleDbConnection(strConn)
        Dim connection2 As New OleDbConnection(strConn)
        Dim list_string As New List(Of String)

        Try
            Dim strSQL As String = "select REQUERENTE_USUARIO_CODIGO from SOLICITACOES "
            strSQL = strSQL + "where REQUERENTE_USUARIO_CODIGO = '" + Convert.ToString(user_code) + "'"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            Dim reader As OleDbDataReader
            connection.Open()
            reader = cmd.ExecuteReader
            Using connection
                While reader.Read
                    Dim _registro As New String(reader.Item("REQUERENTE_USUARIO_CODIGO").ToString)
                    list_string.Add(_registro)
                End While
            End Using

            If list_string.Count > 0 Then
                Return ("O usuário possui solicitações cadastradas")
            End If

            list_string = New List(Of String)

            strSQL = "select ID_USUARIO_PARENT from USUARIOS "
            strSQL = strSQL + " where ID_USUARIO_PARENT = '" + Convert.ToString(user_code) + "'"

            Dim cmd_1 As OleDbCommand = connection1.CreateCommand
            cmd_1.CommandText = strSQL
            Dim reader_1 As OleDbDataReader
            connection1.Open()
            reader_1 = cmd_1.ExecuteReader
            Using connection1
                While reader_1.Read
                    Dim _registro As New String(reader_1.Item("ID_USUARIO_PARENT").ToString)
                    list_string.Add(_registro)
                End While
            End Using

            If list_string.Count > 0 Then
                Return ("Este usuário é responsável de outro usuário")
            End If

            list_string = New List(Of String)

            strSQL = "select CODIGO_USUARIO from LINHAS_MOVEIS "
            strSQL = strSQL + "where CODIGO_USUARIO = '" + Convert.ToString(user_code) + "'"

            Dim cmd_2 As OleDbCommand = connection2.CreateCommand
            cmd_2.CommandText = strSQL
            Dim reader_2 As OleDbDataReader
            connection2.Open()
            reader_2 = cmd_2.ExecuteReader
            Using connection2
                While reader_2.Read
                    Dim _registro As New String(reader_2.Item("CODIGO_USUARIO").ToString)
                    list_string.Add(_registro)
                End While
            End Using

            If list_string.Count > 0 Then
                Return ("O usuário possui linhas móveis cadastradas")
            End If

        Catch ex As Exception
            Return ("Query Error")
        End Try

        Return ("OK")

    End Function

    Public Function GetUsuarioById(ByVal pcodigo As Integer) As List(Of AppUsuarios)
        Dim connection As New OleDbConnection(strConn)
        Dim listUsuario As New List(Of AppUsuarios)

        Dim strSQL As String = "select CODIGO"
        strSQL = strSQL + ", NOME_USUARIO, CARGO_USUARIO, LOGIN_USUARIO, RML_NUMERO_A"
        strSQL = strSQL + ", SENHA_USUARIO, EMAIL_USUARIO, EMAIL_SUPERVISOR, RECEBE_EMAIL"
        strSQL = strSQL + ", CASE WHEN senha_web is null THEN '' ELSE '1' END as SENHA_WEB, RECEBE_RELATORIO, ACESSO_WEB, ENDERECO"
        strSQL = strSQL + ", BAIRRO, NUMERO, COMPLEMENTO, MATRICULA, CPF"
        strSQL = strSQL + ", ID, CEP, TELEFONE, nvl(CODIGO_CIDADE,0) as CODIGO_CIDADE, MUNICIPIO, STATUS"
        strSQL = strSQL + ", GRP_CODIGO, EXPIRACAO_SENHA_WEB, BLOQUEIO_WEB, nvl(DIAS_SENHA_EXPIRA,0) as DIAS_SENHA_EXPIRA"
        strSQL = strSQL + ", nvl(ID_USUARIO_PARENT,0) as ID_USUARIO_PARENT, CODIGO_UC, UF, CIDADE, RECEBE_CELULAR, CODIGO_LOCALIDADE"

        '*********************** SULAMERICA *****************************

        If AppIni.Sulamerica_Param = True Then

            strSQL = strSQL + "  ,MATRICULA_SUPERVISOR, VICE  "
            strSQL = strSQL + "  ,DIR, SUPTE, GER, SEC, NUC "
            strSQL = strSQL + "  ,DATA_ADMISSAO ,DATA_DEMISSAO "
        End If

        '*********************** SULAMERICA *****************************

        strSQL = strSQL + " from USUARIOS "
        If pcodigo > 0 Then
            strSQL = strSQL + "where CODIGO = " + Convert.ToString(pcodigo)
        ElseIf pcodigo = 0 Then
            strSQL = strSQL + "order by CODIGO"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("NOME_USUARIO").ToString, reader.Item("CARGO_USUARIO").ToString, reader.Item("LOGIN_USUARIO").ToString, reader.Item("RML_NUMERO_A").ToString, reader.Item("SENHA_USUARIO").ToString, reader.Item("EMAIL_USUARIO").ToString, reader.Item("EMAIL_SUPERVISOR").ToString, reader.Item("RECEBE_EMAIL").ToString, reader.Item("SENHA_WEB").ToString, reader.Item("RECEBE_RELATORIO").ToString, reader.Item("ACESSO_WEB").ToString, reader.Item("ENDERECO").ToString, reader.Item("BAIRRO").ToString, reader.Item("NUMERO").ToString, reader.Item("COMPLEMENTO").ToString, reader.Item("MATRICULA").ToString, reader.Item("CPF").ToString, reader.Item("ID").ToString, reader.Item("CEP").ToString, reader.Item("TELEFONE").ToString, reader.Item("CODIGO_CIDADE").ToString, reader.Item("MUNICIPIO").ToString, reader.Item("GRP_CODIGO").ToString, reader.Item("EXPIRACAO_SENHA_WEB").ToString, reader.Item("BLOQUEIO_WEB").ToString, reader.Item("DIAS_SENHA_EXPIRA").ToString, reader.Item("ID_USUARIO_PARENT").ToString, reader.Item("CODIGO_UC").ToString, reader.Item("UF").ToString, reader.Item("CIDADE").ToString, reader.Item("RECEBE_CELULAR").ToString, reader.Item("CODIGO_LOCALIDADE").ToString)

                _registro.STATUS = reader.Item("STATUS").ToString

                '*********************** SULAMERICA *****************************

                If AppIni.Sulamerica_Param = True Then

                    _registro.Matricula_sup = reader.Item("MATRICULA_SUPERVISOR").ToString
                    _registro.VICE = reader.Item("VICE").ToString
                    _registro.DIR = reader.Item("DIR").ToString
                    _registro.GER = reader.Item("SUPTE").ToString
                    _registro.SUPTE = reader.Item("GER").ToString
                    _registro.SEC = reader.Item("SEC").ToString
                    _registro.NUC = reader.Item("NUC").ToString
                    _registro.DATA_ADMISSAO = reader.Item("DATA_ADMISSAO").ToString
                    _registro.DATA_DEMISSAO = reader.Item("DATA_DEMISSAO").ToString

                End If

                '*********************** SULAMERICA *****************************

                listUsuario.Add(_registro)
            End While
        End Using

        Return listUsuario
    End Function

    Public Function VerificaCampo(ByVal valor As String, ByVal campo As String, ByVal codigo As String) As Integer
        Dim connection As New OleDbConnection(strConn)
        Dim listUsuario As New List(Of AppUsuarios)

        Dim strSQL As String = "select *"
        strSQL = strSQL + " from USUARIOS "
        strSQL = strSQL + " where " + campo + "='" + valor + "'"
        If codigo <> "" Then
            strSQL = strSQL + " AND CODIGO <> '" + codigo + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, reader.Item("NOME_USUARIO").ToString)
                listUsuario.Add(_registro)
            End While
        End Using

        Return listUsuario.Count
    End Function

    Public Function InsertUserLog(ByVal pgestao_usuario As AppUsuarios, ByVal type As Char, ByVal autorlog As String) As Boolean

        Dim connection As New OleDbConnection(strConn)
        Try

            Dim strSQL As String

            strSQL = "insert into USUARIOSLOG(ID, TIPO, DATA, AUTOR, CODIGO"
            strSQL = strSQL + ", NOME_USUARIO,GRP_CODIGO, CARGO_USUARIO, LOGIN_USUARIO, RML_NUMERO_A, SENHA_USUARIO, EMAIL_USUARIO, EMAIL_SUPERVISOR, RECEBE_EMAIL, RECEBE_RELATORIO, ACESSO_WEB, ENDERECO, BAIRRO, NUMERO, COMPLEMENTO, MATRICULA, CPF, CEP,ID_USUARIO_PARENT, STATUS, UF, CIDADE, CODIGO_LOCALIDADE) "
            strSQL = strSQL + "values ((select nvl(max(ID),0)+1 from USUARIOSLOG),"
            'TIPO
            strSQL = strSQL + " '" + type + "',"

            'DATA         
            strSQL = strSQL + "to_date('" + Date.Now + "','dd/mm/yyyy hh24:mi:ss'),"
            'AUTOR
            strSQL = strSQL + "'" + autorlog + "'"

            If type = "N" Then
                strSQL = strSQL + ",(select nvl(max(CODIGO),0) from USUARIOS)"
            Else
                strSQL = strSQL + ",'" + pgestao_usuario.Codigo.ToString() + "'"
            End If
            strSQL = strSQL + ",'" + pgestao_usuario.Nome_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.GRP_Codigo + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Cargo_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Login_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Rml_Numero_A + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Senha_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Email_Usuario + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Email_Supervisor + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Recebe_Email + "'"
            'strSQL = strSQL + ",'" + pgestao_usuario.Senha_Web + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Recebe_Relatorio + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Acesso_Web + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Endereco + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Bairro + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Numero + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Complemento + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.Matricula + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.CPF + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.CEP + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.ID_Usuario_Parent.ToString + "'"
            strSQL = strSQL + ",'" + pgestao_usuario.STATUS.ToString + "'"

            If pgestao_usuario.uf <> "..." Then
                strSQL = strSQL + ",'" + pgestao_usuario.uf + "'"
                If pgestao_usuario.Codigo_Cidade = 0 Then
                    strSQL = strSQL + ", ''"
                Else
                    strSQL = strSQL + ", (select MUNICIPIO from CIDADES where CODIGO_CIDADE='" + pgestao_usuario.Codigo_Cidade.ToString + "')"

                End If
            Else
                strSQL = strSQL + ",''"
                strSQL = strSQL + ",''"
            End If
            strSQL = strSQL + ",'" + pgestao_usuario.CodigoLocalidade + "'"
            strSQL = strSQL + ")"

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = strSQL
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()
            Return True

        Catch ex As Exception
            connection.Close()
            Return False
        End Try

    End Function

    Public Function GetMovelByUsuario(ByVal pcodigo As Integer) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listMoveis As New List(Of String)

        Dim strSQL As String = "select p1.codigo_linha, p1.num_linha "
        strSQL = strSQL + " from linhas p1, linhas_moveis p2 "
        strSQL = strSQL + " where p1.codigo_linha=p2.codigo_linha "
        If pcodigo > 0 Then
            strSQL = strSQL + " and p2.CODIGO_usuario = '" + Convert.ToString(pcodigo) + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                'listMoveis.Add("<a href=javascript:window.open("""aparelhosaux.asp?operacao=3&codigo=""" & reader.Item("codigo_linha").ToString & """ & reader.Item("num_linha").ToString)
                'listMoveis.Add("<a href=javascript:window.open('aparelhosaux.asp?operacao=3&codigo=" + reader.Item("codigo_linha").ToString + "');void(0);>" & reader.Item("num_linha").ToString & "</a>")
                listMoveis.Add("<a href=javascript:window.open('GestaoAparelhosMoveisCadastro.aspx?codigo=" + reader.Item("codigo_linha").ToString + "');void(0);>" & reader.Item("num_linha").ToString & "</a>")
            End While
        End Using

        Return listMoveis
    End Function

    Public Function GetLinhasByUsuario(ByVal pcodigo As Integer) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listMoveis As New List(Of String)

        Dim strSQL As String = "select p1.codigo_linha "
        strSQL = strSQL + " from linhas p1 "
        If pcodigo > 0 Then
            strSQL = strSQL + " where p1.CODIGO_usuario = '" + Convert.ToString(pcodigo) + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                listMoveis.Add(reader.Item("codigo_linha").ToString)
            End While
        End Using

        Return listMoveis
    End Function

    Public Function GetLinhasMoveisByUsuario(ByVal pcodigo As Integer) As List(Of String)
        Dim connection As New OleDbConnection(strConn)
        Dim listMoveis As New List(Of String)

        Dim strSQL As String = "select p1.codigo_linha, p1.num_linha "
        strSQL = strSQL + " from linhas p1, linhas_moveis p2 "
        strSQL = strSQL + " where p1.codigo_linha=p2.codigo_linha "
        strSQL = strSQL + " and p2.CODIGO_usuario = '" + Convert.ToString(pcodigo) + "'"


        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                listMoveis.Add(reader.Item("codigo_linha").ToString)
            End While
        End Using

        Return listMoveis
    End Function


    Public Function VerificaSenha(ByVal valor As String, ByVal campo As String, ByVal codigo As String) As Integer
        Dim connection As New OleDbConnection(strConn)
        Dim listUsuario As New List(Of AppUsuarios)

        Dim strSQL As String = "select *"
        strSQL = strSQL + " from vsenhas "
        strSQL = strSQL + " where " + campo + "='" + valor + "'"
        If codigo <> "" Then
            strSQL = strSQL + " AND CODIGO <> '" + codigo + "'"
        End If

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Dim _registro As New AppUsuarios(reader.Item("CODIGO").ToString, "")
                listUsuario.Add(_registro)
            End While
        End Using

        Return listUsuario.Count
    End Function

    Public Function GerarSenha(ByVal tamanho_senha As Integer) As String
        Dim connection As New OleDbConnection(strConn)
        Dim senha As String = ""

        Dim strSQL As String = " select dbms_random.string ('L','1') FROM dual"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                senha = reader.Item(0).ToString
            End While
        End Using

        For i As Integer = 2 To tamanho_senha
            senha = senha & ReturnAuxSenha()
        Next

        Return senha

    End Function

    Public Function ReturnAuxSenha() As String
        Dim connection As New OleDbConnection(strConn)
        Dim connection2 As New OleDbConnection(strConn)
        Dim aux As String = ""

        Dim strSQL As String = " select round(dbms_random.value(0,1)) from dual"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read

                If reader.Item(0).ToString = 0 Then
                    strSQL = " select dbms_random.string ('L','1') FROM dual"

                    Dim cmd2 As OleDbCommand = connection2.CreateCommand
                    cmd2.CommandText = strSQL
                    Dim reader2 As OleDbDataReader
                    connection2.Open()
                    reader2 = cmd2.ExecuteReader
                    Using connection2
                        While reader2.Read
                            aux = reader2.Item(0).ToString
                        End While

                        Return aux

                    End Using
                Else
                    strSQL = " select round(dbms_random.value(0,9)) num from dual"

                    Dim cmd3 As OleDbCommand = connection2.CreateCommand
                    cmd3.CommandText = strSQL
                    Dim reader3 As OleDbDataReader
                    connection2.Open()
                    reader3 = cmd3.ExecuteReader
                    Using connection2
                        While reader3.Read
                            aux = reader3.Item(0).ToString
                        End While

                        Return aux

                    End Using
                End If
            End While
        End Using

        Return aux

    End Function

    Public Function VerificaSenhaJaUsada(ByVal codigo_usuario As String, ByVal senha_nova As String) As Boolean
        Dim connection As New OleDbConnection(strConn)

        Dim strSQL As String = " select count(*) from usuarios_senhas_antigas WHERE codigo_usuario='" & codigo_usuario & "' and senha_web_antiga=password.encrypt('" & senha_nova & "')"

        Dim cmd As OleDbCommand = connection.CreateCommand
        cmd.CommandText = strSQL
        Dim reader As OleDbDataReader
        connection.Open()
        reader = cmd.ExecuteReader
        Using connection
            While reader.Read
                Return True
            End While
        End Using

        Return False

    End Function

    Public Function GravaSenhaWeb(ByVal senha_web As String, ByVal codigo_usuario As String, ByVal dias_senha_expira As String, ByVal expiracao As String, ByVal bloquear_acesso_web As String) As Boolean
        Dim transaction As OleDbTransaction = Nothing
        Dim connection As New OleDbConnection(strConn)
        Dim cmd As OleDbCommand = connection.CreateCommand

        Dim _dao_commons As New DAO_Commons
        _dao_commons.strConn = strConn

        Try
            connection.Open()
            transaction = connection.BeginTransaction
            cmd = connection.CreateCommand
            cmd.Transaction = transaction

            Dim strSQL As String = ""

            strSQL = "update usuarios set acesso_web='S', senha_web=password.encrypt('" & senha_web.ToUpper.Trim & "') WHERE codigo='" & codigo_usuario & "'"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            strSQL = "insert into usuarios_senhas_antigas(codigo_usuario,senha_web_antiga) values ('" & codigo_usuario & "', password.encrypt('" & senha_web & "'))"

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            If (dias_senha_expira <> "") Then
                strSQL = "update usuarios set acesso_web='S', dias_senha_expira='" & dias_senha_expira & "' WHERE codigo='" & codigo_usuario & "'"
            End If

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            ' PRÓXIMA DATA DE EXPIRAÇÃO DE SENHA
            If (expiracao <> "") Then
                strSQL = "update usuarios set expiracao_senha_web=to_date('" & expiracao & "','dd/mm/yyyy') WHERE codigo='" & codigo_usuario & "'"
            End If

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            ' DATA DE REMOÇÃO DE ACESSO WEB
            If (bloquear_acesso_web <> "") Then
                strSQL = "update usuarios set bloqueio_web = to_date('" & bloquear_acesso_web & "','dd/mm/yyyy') WHERE codigo='" & codigo_usuario & "'"
            Else
                strSQL = "update usuarios set bloqueio_web = null WHERE codigo='" & codigo_usuario & "'"
            End If

            cmd.CommandText = strSQL
            cmd.ExecuteNonQuery()

            transaction.Commit()
            transaction.Dispose()
            connection.Close()
            connection.Dispose()
            Return True

        Catch e As Exception
            '_dao_commons.EscreveLog("Erro na Insert Linhas Móveis: " & e.Message)
            transaction.Rollback()
            transaction.Dispose()
            transaction = Nothing
            Return False
        End Try

        'InsertUserLog(pgestao_usuario, "N", autorlog)
        Return True
    End Function

    Public Function Login(username As String, password As String, Optional AD As String = "") As AppUsuarios

        Dim sql As String = "select t.codigo, t.login_usuario, t.email_usuario, t.nome_usuario from USUARIOS t"
        sql += " where upper(t.login_usuario)='" & username.ToUpper & "' "
        If AD.ToUpper = "TRUE" And password = "AUTOLOGON" Then
            sql += " And acesso_web='S' "
        Else
            sql += " And senha_web = password.encrypt(upper('" & UCase(password.ToUpper) & "')) "
        End If

        Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        If dt.Rows.Count > 0 Then

            Dim usuario As New AppUsuarios
            usuario.Login_Usuario = dt.Rows(0).Item("login_usuario").ToString
            usuario.Email_Usuario = dt.Rows(0).Item("email_usuario").ToString
            usuario.codigo = dt.Rows(0).Item("codigo").ToString
            usuario.Nome_Usuario = dt.Rows(0).Item("nome_usuario").ToString
            Return usuario
        Else
            Return Nothing
        End If

    End Function

    Public Function LoginV2(username As String, password As String, Optional AD As String = "") As AppUsuarios

        Dim sql As String = "select t.codigo, t.login_usuario, t.email_usuario, t.nome_usuario from USUARIOS t" & vbNewLine
        sql += " where upper(t.login_usuario)=upper(:login) And acesso_web='S' "
        If AD.ToUpper = "TRUE" And password = "AUTOLOGON" Then
            'sql += "  "
        Else
            sql += " And senha_web = password.encrypt(upper(:senha)) "
        End If

        Dim prmArray As New List(Of OleDbParameter)
        prmArray.Add(New OleDbParameter(":login", username.ToUpper))
        If Not (AD.ToUpper = "TRUE" And password = "AUTOLOGON") Then
            prmArray.Add(New OleDbParameter(":senha", UCase(password.ToUpper)))
        End If


        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        If dt.Rows.Count > 0 Then

            Dim usuario As New AppUsuarios
            usuario.Login_Usuario = dt.Rows(0).Item("login_usuario").ToString
            usuario.Email_Usuario = dt.Rows(0).Item("email_usuario").ToString
            usuario.codigo = dt.Rows(0).Item("codigo").ToString
            usuario.Nome_Usuario = dt.Rows(0).Item("nome_usuario").ToString

            'grava nas visitantes log
            Dim IP As String = GetIPAddress()
            sql = "insert into visitanteslog (data,usuario,ip,empresa) values (sysdate,'" & usuario.Nome_Usuario & "','" & IP & "','DEFAULT')"
            Dim connection As New OleDbConnection(strConn)

            Dim cmd As OleDbCommand = connection.CreateCommand
            cmd.CommandText = sql
            connection.Open()
            cmd.ExecuteNonQuery()
            connection.Close()
            cmd.Dispose()

            Return usuario
        Else
            Return Nothing
        End If

    End Function




#Region "UsuarioGastoDashboard"
    Public Shared Function GetIPAddress() As String
        Dim context As System.Web.HttpContext = System.Web.HttpContext.Current
        Dim sIPAddress As String = System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_ADDR")
        If String.IsNullOrEmpty(sIPAddress) Then
            Return System.Web.HttpContext.Current.Request.ServerVariables("REMOTE_HOST")
        Else
            'Dim ipArray As String() = sIPAddress.Split(New [Char]() {","c})
            Return sIPAddress
        End If
    End Function



    Public Function MontaResumoGasto(codigousuario As String, vencimento As String, Optional intervaloMes As Integer = 0) As DataTable

        Dim sql As String = ""
        sql += " Select p1.rml_numero_a linha, p4.descricao As Operadora, am.modelo,ma.marca, "
        sql += " sum(Case When p1.cdr_codigo='3' then p1.valor_franquia else 0 end)chamadas, "
        sql += " sum(case when p1.cdr_codigo<>'3'  and p1.cdr_codigo<>'5' then p1.total_gasto else 0 end)servicos, "
        sql += " sum(case when p1.cdr_codigo='5' then p1.valor_cdr else 0 end)descontos, "
        'sql += " sum(p1.valor_cdr) gasto "
        'sql += "  sum(case  when UPPER(p1.tipo_serv2) not like '%PARCELA%' then  p1.total_gasto else 0 end) gasto "
        sql += "  sum(p1.total_gasto) gasto "
        'sql += " , sum(case  when UPPER(p1.tipo_serv2) like '%PARCELA%' then  p1.valor_cdr else 0 end) parcelamento "
        sql += " , ' ' rateio "
        sql += " , 0 parcelamento "
        sql += " ,sum(p1.total_gasto) total, nvl(op.plano,'-')plano "
        sql += " ,GET_OCULTA_NUMLINHA(p1.rml_numero_a)LINHAX "
        sql += " , ' ' parcelas "

        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4, linhas l, linhas_moveis lm, aparelhos_moveis  ap,aparelhos_modelos am, aparelhos_marcas ma,OPERADORAS_PLANOS op "
        'sql += " , v_linhas_parcelas_custos pa"
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a=replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')   and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+) and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and p3.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and p3.codigo_tipo='1' "


        If vencimento <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"

        End If

        If codigousuario <> "" Then
            sql += " and p1.codigo_usuario=:codigousuario"
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by p1.rml_numero_a, am.modelo,ma.marca, p4.descricao,nvl(op.plano,'-') "

        'union com os aparelhos sem linha
        sql += " union "
        sql += "  select pa.num_linha linha,p4.descricao As Operadora,am.modelo,ma.marca,0 chamadas,pa.custo_servico  servicos,0 descontos,pa.valor gasto,' ' rateio,pa.valor parcelamento,pa.valor total,nvl(op.plano, '-') plano,'' LINHAX, case when pa.num_parcela is not null then '(' || pa.num_parcela || '/' || pa.qtd_parcelas || ')' else '' end  parcelas "
        sql += " from lancamentos_mensais pa, operadoras_teste p4, linhas l,linhas_moveis lm,aparelhos_moveis ap,aparelhos_modelos  am,aparelhos_marcas ma,OPERADORAS_PLANOS op"
        sql += " where pa.id_item=ap.codigo_aparelho and lm.codigo_aparelho=ap.codigo_aparelho and lm.codigo_linha=l.codigo_linha "
        sql += " and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and pa.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and pa.tipo_recurso=1"
        sql += " and pa.referencia=:vencimento"
        sql += " and pa.codigo_usuario = :codigousuario"
        sql += " and pa.num_linha is null"
        sql += " and (pa.valor>0 or pa.custo_servico>0)"


        'sql += " order by p4.descricao,nvl(op.plano,'-')  "

        'Response.Write(sql)
        'Response.End()

        Dim prmArray As New List(Of OleDbParameter)


        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If
        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))
        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento2", UCase(vencimento.ToUpper)))
        End If
        prmArray.Add(New OleDbParameter(":codigousuario2", codigousuario.ToUpper))

        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        'Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        ' dt.Columns(3).

        For Each _row As DataRow In dt.Rows
            If Not _row.Item("linha").ToString = "" Then
                _row.Item(8) = CarregaValorRateio(_row.Item(0), codigousuario, vencimento)
            Else
                _row.Item(8) = 0
            End If


            '_row.Item(9) = GetParcela(_row.Item(0), codigousuario, vencimento)
            If _row.Item("parcelamento") = "0" Then
                _row.Item("parcelamento") = GetParcelaNova(_row.Item(0), vencimento)
            End If
            Dim CustoFixo As Double = 0
            If _row.Item("linha").ToString = "" Then
                ' CustoFixo = _row.Item("custo_fixo")
                CustoFixo = 0
            Else
                CustoFixo = GetCustoFixo(_row.Item(0))
            End If


            _row.Item("total") = _row.Item(8) + _row.Item("total") + CustoFixo + _row.Item("parcelamento")

            '_row.Item(8) = FormatCurrency(_row.Item(8))
            _row.Item(7) = _row.Item(7) + _row.Item(8) + CustoFixo
            If Not _row.Item("linha").ToString = "" Then
                _row.Item("parcelas") = GetQTDParcelas(_row.Item(0), vencimento)
            End If

            If _row.Item("parcelamento") = "0" Then
                _row.Item("parcelas") = ""
            End If
        Next

        Return dt

    End Function


    Public Function MontaResumoGastoOLD(codigousuario As String, vencimento As String, Optional intervaloMes As Integer = 0) As DataTable

        Dim sql As String = ""
        sql += " Select p1.rml_numero_a linha, p4.descricao As Operadora, am.modelo,ma.marca, "
        sql += " sum(Case When p1.cdr_codigo='3' then p1.valor_franquia else 0 end)chamadas, "
        sql += " sum(case when p1.cdr_codigo<>'3'  and p1.cdr_codigo<>'5' then p1.total_gasto else 0 end)servicos, "
        sql += " sum(case when p1.cdr_codigo='5' then p1.valor_cdr else 0 end)descontos, "
        'sql += " sum(p1.valor_cdr) gasto "
        'sql += "  sum(case  when UPPER(p1.tipo_serv2) not like '%PARCELA%' then  p1.total_gasto else 0 end) gasto "
        sql += "  sum(p1.total_gasto) gasto "
        'sql += " , sum(case  when UPPER(p1.tipo_serv2) like '%PARCELA%' then  p1.valor_cdr else 0 end) parcelamento "
        sql += " , ' ' rateio "
        sql += " , 0 parcelamento "
        sql += " ,sum(p1.total_gasto) total, nvl(op.plano,'-')plano "
        sql += " ,GET_OCULTA_NUMLINHA(p1.rml_numero_a)LINHAX "
        sql += " , ' ' parcelas "

        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4, linhas l, linhas_moveis lm, aparelhos_moveis  ap,aparelhos_modelos am, aparelhos_marcas ma,OPERADORAS_PLANOS op "
        'sql += " , v_linhas_parcelas_custos pa"
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a=replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')   and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+) and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and p3.codigo_operadora=p4.codigo and l.codigo_plano=op.codigo_plano(+) and p3.codigo_tipo='1' "


        If vencimento <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"

        End If

        If codigousuario <> "" Then
            sql += " and p1.codigo_usuario=:codigousuario"
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by p1.rml_numero_a, am.modelo,ma.marca, p4.descricao,nvl(op.plano,'-') order by p4.descricao,nvl(op.plano,'-') "

        'Response.Write(sql)
        'Response.End()

        Dim prmArray As New List(Of OleDbParameter)


        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If
        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))
        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        'Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        ' dt.Columns(3).

        For Each _row As DataRow In dt.Rows
            _row.Item(8) = CarregaValorRateio(_row.Item(0), codigousuario, vencimento)
            '_row.Item(9) = GetParcela(_row.Item(0), codigousuario, vencimento)
            _row.Item("parcelamento") = GetParcelaNova(_row.Item(0), vencimento)
            Dim CustoFixo As Double = GetCustoFixo(_row.Item(0))
            _row.Item("total") = _row.Item(8) + _row.Item("total") + CustoFixo + _row.Item("parcelamento")

            '_row.Item(8) = FormatCurrency(_row.Item(8))
            _row.Item(7) = _row.Item(7) + _row.Item(8) + CustoFixo
            _row.Item("parcelas") = GetQTDParcelas(_row.Item(0), vencimento)
            If _row.Item("parcelamento") = "0" Then
                _row.Item("parcelas") = ""
            End If
        Next

        Return dt

    End Function

    Function GetCustoFixo(celular As String) As Double

        Dim Sql As String
        Sql = "  select nvl(sum(TOTAL),0)TOTAL from ("
        Sql = Sql + " select 'GESTÃO DE TELECOM GLOBO' categoria,"
        Sql = Sql + " 1 qtd,"
        Sql = Sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        Sql += "   nvl(op.custo_fixo,0) TOTAL,1 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        Sql += " from operadoras_planos op, linhas l "
        Sql += " where op.codigo_plano=l.codigo_plano and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'sql += " and op.custo_fixo is not null "
        Sql += " "

        'Sql += " union "
        'Sql = Sql + " select 'Parcela do aparelho' categoria,"
        'Sql = Sql + " 1 qtd,"
        'Sql = Sql + " 0 duracao,"
        ''sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        ''sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        'Sql += "   nvl(ap.valor,0)/nvl(ap.qtd_parcelas,1) TOTAL, 2 ordem "
        ''sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        '' Dim sql As String = " select nvl(sum(rateio),0) "
        'Sql += " from  linhas l, linhas_moveis lm, aparelhos_moveis ap "
        'Sql += "  where l.codigo_linha=lm.codigo_linha and lm.codigo_aparelho=ap.codigo_aparelho and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        ''sql += " and op.custo_fixo is not null "
        Sql += ") "




        Dim dt As DataTable = _DAO_commons.myDataTable(Sql)
        If dt.Rows.Count > 0 Then

            Return dt.Rows(0).Item("TOTAL")
        Else
            Return 0

        End If
    End Function

    Function GetParcelaNova(celular As String, vencimento As String) As String

        Dim Sql As String


        'Sql = "  select round(nvl(sum(TOTAL),0),2)TOTAL from ("
        'Sql = Sql + " select 'Parcela do aparelho' categoria,"
        'Sql = Sql + " 1 qtd,"
        'Sql = Sql + " 0 duracao,"
        ''sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        ''sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        'Sql += "   nvl(ap.valor,0)/nvl(ap.qtd_parcelas,1) TOTAL, 2 ordem "
        ''sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        '' Dim sql As String = " select nvl(sum(rateio),0) "
        'Sql += " from  linhas l, linhas_moveis lm, aparelhos_moveis ap "
        'Sql += "  where l.codigo_linha=lm.codigo_linha and lm.codigo_aparelho=ap.codigo_aparelho and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        ''sql += " and op.custo_fixo is not null "
        'Sql += ") "

        'Sql = "  select round(nvl(sum(TOTAL),0),2)TOTAL from ("
        'Sql = Sql + " select 'Parcela do aparelho' categoria,"
        'Sql = Sql + " 1 qtd,"
        'Sql = Sql + " 0 duracao,"
        ''sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        ''sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        'Sql += "   nvl(pa.parcela,0) TOTAL, 2 ordem "
        ''sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        '' Dim sql As String = " select nvl(sum(rateio),0) "
        'Sql += " from   V_LINHAS_PARCELAS_CUSTOS pa "
        'Sql += "  where replace(replace(replace(replace(pa.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'Sql += "  and nvl(pa.qtd_parcelas,1) - MONTHS_BETWEEN (TO_DATE('" + vencimento + "','MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>0"
        'Sql += "  and  MONTHS_BETWEEN (TO_DATE('" + vencimento + "','MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>=0"
        'Sql += "  and rownum<2 "
        ''sql += " and op.custo_fixo is not null "
        'Sql += ") "

        Sql = "  select round(nvl(sum(TOTAL),0),2)TOTAL from ("
        Sql = Sql + " select 'Parcela do aparelho' categoria,"
        Sql = Sql + " 1 qtd,"
        Sql = Sql + " 0 duracao,"
        'sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        'sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        Sql += "   nvl(pa.valor,0) TOTAL, 2 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        Sql += " from  lancamentos_mensais pa  "
        Sql += "  where pa.num_linha='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        Sql += "  and pa.referencia='" + vencimento + "'"
        Sql += "  and rownum<2 "
        'sql += " and op.custo_fixo is not null "
        Sql += ") "






        Dim dt As DataTable = _DAO_commons.myDataTable(Sql)

        If dt.Rows.Count > 0 Then

            Return dt.Rows(0).Item("TOTAL")
        Else
            Return 0

        End If

    End Function

    Function GetQTDParcelas(celular As String, vencimento As String) As String

        Dim Sql As String


        'Sql = "  select round(nvl(sum(TOTAL),0),2)TOTAL from ("
        'Sql = Sql + " select 'Parcela do aparelho' categoria,"
        'Sql = Sql + " 1 qtd,"
        'Sql = Sql + " 0 duracao,"
        ''sql = sql + " sum(nvl(p1.valor_cdr, 0)) total"
        ''sql += "   nvl(sum(rateio),0)-sum(nvl(valor,0)) TOTAL "
        'Sql += "   nvl(ap.valor,0)/nvl(ap.qtd_parcelas,1) TOTAL, 2 ordem "
        ''sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        '' Dim sql As String = " select nvl(sum(rateio),0) "
        'Sql += " from  linhas l, linhas_moveis lm, aparelhos_moveis ap "
        'Sql += "  where l.codigo_linha=lm.codigo_linha and lm.codigo_aparelho=ap.codigo_aparelho and replace(replace(replace(replace(l.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        ''sql += " and op.custo_fixo is not null "
        'Sql += ") "

        'Sql = "  select parcelas from ("
        'Sql = Sql + " select 'Parcela do aparelho' categoria,"
        'Sql = Sql + " 1 qtd,"
        'Sql = Sql + " 0 duracao,"
        'Sql = Sql + " '(' || MONTHS_BETWEEN(to_date('" + vencimento + "','MM/YYYY'),to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY')) || '/' || pa.qtd_parcelas ||')' parcelas"
        'Sql += " from   V_LINHAS_PARCELAS_CUSTOS pa "
        'Sql += "  where replace(replace(replace(replace(pa.num_linha,''),')',''),'(',''),'-','')='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "' "
        'Sql += "  and nvl(pa.qtd_parcelas,1) - MONTHS_BETWEEN (TO_DATE('" + vencimento + "','MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>0"
        'Sql += "  and rownum<2 "
        ''sql += " and op.custo_fixo is not null "
        'Sql += ") "


        Sql = "  select parcelas from ("
        Sql = Sql + " select "
        Sql += "    '(' ||  pa.num_parcela || '/' || pa.qtd_parcelas  || ')' parcelas, 2 ordem "
        'sql += "   nvl(sum(rateio+valor),0) TOTAL, 2 ordem "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        Sql += " from  lancamentos_mensais pa  "
        Sql += "  where pa.num_linha='" + celular.Trim.Replace("(", "").Replace(")", "").Replace("-", "") + "'  "
        Sql += "  and pa.referencia='" + vencimento + "'"
        Sql += "  and rownum<2 "
        'sql += " and op.custo_fixo is not null "
        Sql += ") "


        Dim dt As DataTable = _DAO_commons.myDataTable(Sql)

        If dt.Rows.Count > 0 Then

            Return dt.Rows(0).Item("parcelas")
        Else
            Return 0

        End If

    End Function


    Public Function getMediaArea(codigousuario As String, vencimento As String, Optional intervaloMes As Integer = 0) As DataTable

        Dim sql As String = ""
        sql += " select p1.rml_numero_a linha, p4.descricao as Operadora, am.modelo,ma.marca, "
        sql += " sum(case when p1.cdr_codigo='3' then p1.valor_franquia else 0 end)chamadas, "
        sql += " sum(case when p1.cdr_codigo<>'3'  and p1.cdr_codigo<>'5' then p1.valor_cdr else 0 end)servicos, "
        sql += " sum(case when p1.cdr_codigo='5' then p1.valor_cdr else 0 end)descontos, "
        'sql += " sum(p1.valor_cdr) gasto "
        sql += "  sum(case  when UPPER(p1.tipo_serv2) not like '%PARCELA%' then  p1.total_gasto else 0 end) gasto "
        'sql += " , sum(case  when UPPER(p1.tipo_serv2) like '%PARCELA%' then  p1.valor_cdr else 0 end) parcelamento "
        sql += " , ' ' rateio "
        sql += " , ' ' parcelamento "
        sql += " ,sum(p1.total_gasto) total "


        sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4, linhas l, linhas_moveis lm, aparelhos_moveis  ap,aparelhos_modelos am, aparelhos_marcas ma "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a=replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')    and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+) and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        sql += " and p3.codigo_operadora=p4.codigo "

        If vencimento <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date('" & vencimento & "','MM/YYYY')," & intervaloMes & "), 'MM/YYYY')"

        End If

        If codigousuario <> "" Then
            sql += " and p1.codigo_usuario='" & codigousuario & "'"
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by p1.rml_numero_a, am.modelo,ma.marca, p4.descricao order by p4.descricao "

        'Response.Write(sql)
        'Response.End()

        Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        ' dt.Columns(3).

        For Each _row As DataRow In dt.Rows
            _row.Item(8) = CarregaValorRateio(_row.Item(0), codigousuario, vencimento)
            _row.Item(9) = GetParcela(_row.Item(0), codigousuario, vencimento)
            _row.Item("total") = _row.Item(8) + _row.Item("total")
            '_row.Item(8) = FormatCurrency(_row.Item(8))
            _row.Item(7) = _row.Item(7) + _row.Item(8)
        Next

        Return dt

    End Function

    Function CarregaValorRateio(numLinha As String, codigousuario As String, vencimento As String) As String

        Dim sql As String = "select rateio from ( "
        'sql += "select nvl(sum(rateio),0)-sum(nvl(valor,0)) rateio "
        sql += "select nvl(sum(rateio),0) rateio "
        ' Dim sql As String = " select nvl(sum(rateio),0) "
        sql += " from ( "
        sql += " select distinct nvl(r.rateio,0)  rateio, r.codigo_fatura, sum(nvl(p1.valor_franquia,0))valor   "
        sql = sql + " from cdrs_celular p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        sql = sql + " where p1.codigo_conta=a.codigo_conta"
        sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
        sql = sql + " and to_char(f.dt_vencimento, 'MM/YYYY') = '" & vencimento & "'"
        sql = sql + "   and replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')='" & numLinha & "'"
        sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura)"
        sql = sql + ")"


        Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        Dim valor_rateio As String = 0
        If dt.Rows.Count > 0 Then
            valor_rateio = dt.Rows(0).Item(0)
        End If

        Return valor_rateio

    End Function

    Function GetParcela(numLinha As String, codigousuario As String, vencimento As String) As String

        Dim Sql As String = ""
        Sql = " select p1.tipo_serv2, sum(p1.valor_cdr)valor "
        Sql += " from CDRS_CELULAR_ANALITICO_MV p1, faturas_arquivos p2, faturas p3 , operadoras_teste p4, linhas l, linhas_moveis lm, aparelhos_moveis  ap,aparelhos_modelos am, aparelhos_marcas ma "
        Sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura and p1.rml_numero_a=replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')    and l.codigo_linha=lm.codigo_linha(+) and lm.codigo_aparelho=ap.codigo_aparelho(+) and ap.cod_modelo=am.cod_modelo(+) and am.cod_marca=ma.cod_marca(+)"
        Sql += " and p3.codigo_operadora=p4.codigo and UPPER(p1.tipo_serv2) like '%PARCELA%'"
        Sql += " and p1.rml_numero_a='" & numLinha & "'"

        If vencimento <> "" Then
            Sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')='" & vencimento & "'"
        End If

        If codigousuario <> "" Then
            Sql += " and p1.codigo_usuario='" & codigousuario & "'"
        End If
        Sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        Sql += " group by p1.tipo_serv2 "

        Dim dt As DataTable = _DAO_commons.myDataTable(Sql)

        If dt.Rows.Count > 0 Then

            Return dt.Rows(0).Item(0).ToString & "<b><br>Valor: " & FormatCurrency(dt.Rows(0).Item(1)) & "</b>"
        Else
            Return ""

        End If

    End Function


    Function getTotalServico(codigousuario As String, vencimento As String, tarifa As String, Optional intervaloMes As Integer = 0, Optional usuarioComum As String = "0") As DataTable

        Dim sql As String = ""
        sql += " select tarifa, sum(gasto)gasto, data from ("
        sql += " select 'VOZ' tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and p1.cdr_codigo='3' "

        If codigousuario > 0 Then
            sql += " and p1.codigo_usuario=:codigousuario"
        End If
        If vencimento <> "" Then
            sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If


        If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=:codigousuario"
            sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY') "

        sql += " union "
        sql += " select upper(replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS')) tarifa, sum(nvl(p1.total_gasto,p1.valor_cdr))gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += " and  p1.cdr_codigo <> '3' "

        If codigousuario > 0 Then
            sql += " and p1.codigo_usuario=:codigousuario"
        End If
        If vencimento <> "" Then
            sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If


        If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=:codigousuario"
            sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        End If
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),replace(NVL(p4.nome_configuracao,'SERVIÇOS'),'DEFAULT','SERVIÇOS') "


        'rateio
        sql += " union"
        sql += " select distinct 'SERVIÇOS' tarifa, nvl(r.rateio,0)  gasto, to_char(f.dt_vencimento, 'MM/YYYY')data "
        sql = sql + " from cdrs_celular_analitico_mv p1,faturas f,faturas_arquivos a,RateioGestao_MV r "
        sql = sql + " where p1.codigo_conta=a.codigo_conta"
        sql = sql + " and a.codigo_fatura=f.codigo_fatura and f.codigo_fatura=r.codigo_fatura "
        sql = sql + " and replace(replace(REPLACE(r.rml_numero_a(+), ')', ''), '(',''),'-','') = replace(replace(REPLACE(p1.rml_numero_a, ')', ''), '(',''),'-','')"
        'tira as cobranças de franquias
        sql = sql + " and not exists (select 0 from FRANQUIAS_COBRANCAS t where t.codigo_franquia in (select codigo_franquia from franquias where codigo_fatura=a.codigo_fatura and servico=p1.tipo_serv2))"
        sql += " and p1.codigo_usuario=:codigousuario"
        If vencimento <> "" Then
            sql += " and  to_char(f.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If
        sql += " and to_date(to_char(f.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql = sql + "group by nvl(r.rateio,0), r.codigo_fatura,to_char(f.dt_vencimento, 'MM/YYYY') "



        sql += ") "
        If tarifa <> "" Then
            sql += " where upper(tarifa)='" & tarifa.ToUpper & "'"
        End If


        sql += "group by tarifa,data  order  by to_date(data,'MM/YYYY')"

        Dim prmArray As New List(Of OleDbParameter)
        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        'If tarifa <> "" Then
        '    prmArray.Add(New OleDbParameter(":tarifa", UCase(tarifa.ToUpper)))
        'End If


        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        'Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        'Response.Write(sql)
        'Response.End()

        Return dt
    End Function

    Function getMediaMes(codigousuario As String, vencimento As String, tarifa As String, Optional intervaloMes As Integer = 0, Optional usuarioComum As String = "0") As DataTable

        Dim sql As String = ""



        'sql += " select tarifa,nvl(sum(gasto)/count(*),0) GASTO, DATA FROM ( select 'MÉDIA' tarifa,  sum(nvl(p1.total_gasto,p1.valor_cdr)) gasto,P1.RML_NUMERO_A,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        'sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 "
        'sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        'sql += " and p1.tarif_codigo = p4.codigo(+)"
        ''sql += " and  p1.cdr_codigo <> '3' "

        'sql += " and not exists (select 0 from linhas l where l.codigo_tipo=10 and replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')=p1.rml_numero_a) "

        'If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=:codigousuario"
        '    sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'ElseIf usuarioComum = "1" Then
        '    sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        'End If
        'If vencimento <> "" Then
        '    sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        'End If

        'sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "

        '''CUSTO FIXO
        'sql += " union "
        'sql += " select 'MÉDIA' tarifa,sum(gasto)/count(*) gasto,RML_NUMERO_A,data from (select P1.RML_NUMERO_A, max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        'sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        'sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        'sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        'sql += " and p1.tarif_codigo = p4.codigo(+)"
        ''sql += " and  p1.cdr_codigo <>'3' "
        'sql += " and p1.grp_codigo=p5.codigo(+)"


        'If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=:codigousuario "
        '    sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'ElseIf usuarioComum = "1" Then
        '    sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        'End If
        'If vencimento <> "" Then
        '    sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        'End If
        'sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "


        'sql += ") group by RML_NUMERO_A, data"

        ''PARCELA
        'sql += " union "
        'sql += " select 'MÉDIA' tarifa,sum(gasto)/case when count(*)=0 then 1 else  count(*) end  gasto,RML_NUMERO_A,data from (select P1.RML_NUMERO_A, max(nvl(pa.PARCELA,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        'sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        'sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        'sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        'sql += " and p1.tarif_codigo = p4.codigo(+)"
        ''sql += " and  p1.cdr_codigo <>'3' "
        'sql += " and p1.grp_codigo=p5.codigo(+)"


        'If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
        '    'não filtra o centro de custo dos gerentes
        '    sql = sql + " and exists(" & vbNewLine
        '    sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
        '    sql = sql + "     where p100.codigo_usuario=:codigousuario"
        '    sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
        '    sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        'ElseIf usuarioComum = "1" Then
        '    sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        'End If
        'If vencimento <> "" Then
        '    sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        'End If
        'sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "


        'sql += ") group by RML_NUMERO_A, data"



        'sql += " )  GROUP BY tarifa,data"
        ''sql += ") group by tarifa,data  order  by to_date(data,'MM/YYYY')"


        sql += " select tarifa,round(sum(gasto)/count(*),2) GASTO, to_char(DATA)DATA ,RML_NUMERO_A FROM ( select 'MÉDIA' tarifa,  sum(nvl(p1.total_gasto,p1.valor_cdr))+max(case when to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY')<= to_date(to_char(pa.fim_parcela,'MM/YYYY'),'MM/YYYY') then nvl(pa.parcela,0) else 0 end)+sum(nvl(pa.CUSTO_FIXO ,0)) gasto,'media' RML_NUMERO_A,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 , V_LINHAS_PARCELAS_CUSTOS pa"
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        sql += "  and p1.codigo_usuario=pa.codigo_usuario(+) and p1.rml_numero_a=pa.num_linha(+)"
        'sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        'sql += "  and nvl(pa.qtd_parcelas,1) - MONTHS_BETWEEN (to_date(to_char(p3.dt_vencimento,'MM/YYYY'),'MM/YYYY'), to_date(to_char(pa.inicio_parcela,'MM/YYYY'),'MM/YYYY'))>0"
        'sql += "  and rownum<2 "
        ' substr(p1.rml_numero_a,3,1)='9'
        sql += " and ((p3.codigo_tipo=1 and substr(p1.rml_numero_a,3,1)='9') ) "

        'sql += " and  p1.cdr_codigo <> '3' "

        If vencimento <> "" Then
            sql += " and to_char(p3.dt_vencimento, 'MM/YYYY')=:vencimento"
        End If
        sql += " and not exists (select 0 from linhas l where l.codigo_tipo=10 and replace(replace(replace(replace(l.num_linha,'(',''),')',''),'-',''),' ','')=p1.rml_numero_a) "

        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')>= to_date(to_char(add_months(to_date(:vencimento,'MM/YYYY'),-11),'MM/YYYY'),'MM/YYYY') "
        sql += " and to_date(to_char(p3.dt_vencimento, 'MM/YYYY'),'MM/YYYY')<= to_date(to_char(add_months(to_date(:vencimento,'MM/YYYY'),0),'MM/YYYY'),'MM/YYYY')  " & vbNewLine
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "
        sql += " )  GROUP BY tarifa,data,RML_NUMERO_A"
        'End If



        Dim prmArray As New List(Of OleDbParameter)
        '  prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        'prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        'prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        'If vencimento <> "" Then
        '    prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        'End If

        'prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        'prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        'prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        'If tarifa <> "" Then
        '    prmArray.Add(New OleDbParameter(":tarifa", UCase(tarifa.ToUpper)))
        'End If


        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        'Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        'Response.Write(sql)
        'Response.End()

        Return dt
    End Function

    Function getMediaMesOLD(codigousuario As String, vencimento As String, tarifa As String, Optional intervaloMes As Integer = 0, Optional usuarioComum As String = "0") As DataTable

        Dim sql As String = ""



        sql += " select tarifa,nvl(sum(gasto)/count(*),0) GASTO, DATA FROM ( select 'MÉDIA' tarifa,  sum(nvl(p1.total_gasto,p1.valor_cdr)) gasto,P1.RML_NUMERO_A,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4 "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <> '3' "

        sql += " and not exists (select 0 from linhas l where l.codigo_tipo=10 and replace(replace(replace(replace(l.num_linha(+),'(',''),')',''),'-',''),' ','')=p1.rml_numero_a) "

        If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=:codigousuario"
            sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        ElseIf usuarioComum = "1" Then
            sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        End If
        If vencimento <> "" Then
            sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If

        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "

        ''CUSTO FIXO
        sql += " union "
        sql += " select 'MÉDIA' tarifa,sum(gasto)/count(*) gasto,RML_NUMERO_A,data from (select P1.RML_NUMERO_A, max(nvl(pa.custo_fixo,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"


        If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=:codigousuario "
            sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        ElseIf usuarioComum = "1" Then
            sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        End If
        If vencimento <> "" Then
            sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "


        sql += ") group by RML_NUMERO_A, data"

        'PARCELA
        sql += " union "
        sql += " select 'MÉDIA' tarifa,sum(gasto)/case when count(*)=0 then 1 else  count(*) end  gasto,RML_NUMERO_A,data from (select P1.RML_NUMERO_A, max(nvl(pa.PARCELA,0)) gasto,to_char(p3.dt_vencimento, 'MM/YYYY')data  "
        sql += " from CDRS_CELULAR_analitico_mv p1, faturas_arquivos p2, faturas p3 , tarifacao p4, grupos p5, V_LINHAS_PARCELAS_CUSTOS pa "
        sql += " where p1.codigo_conta=p2.codigo_conta and p3.codigo_fatura=p2.codigo_fatura "
        sql += "  and p1.rml_numero_a=pa.num_linha(+)"
        sql += " and p1.tarif_codigo = p4.codigo(+)"
        'sql += " and  p1.cdr_codigo <>'3' "
        sql += " and p1.grp_codigo=p5.codigo(+)"


        If Not DALCGestor.AcessoAdmin(codigousuario) And usuarioComum <> "1" Then
            'não filtra o centro de custo dos gerentes
            sql = sql + " and exists(" & vbNewLine
            sql = sql + "   select 0 from categoria_usuario p100" & vbNewLine
            sql = sql + "     where p100.codigo_usuario=:codigousuario"
            sql = sql + "     and p100.tipo_usuario in('D','G','GC')" & vbNewLine
            sql = sql + "     and to_char(p1.grp_codigo) like p100.codigo_grupo||'%' )" & vbNewLine
        ElseIf usuarioComum = "1" Then
            sql = sql + " And to_char(p1.grp_codigo) = (select grp_codigo from usuarios where codigo=:codigousuario ) "
        End If
        If vencimento <> "" Then
            sql += " and  to_char(p3.dt_vencimento, 'MM/YYYY')=to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"
        End If
        sql += " group by to_char(p3.dt_vencimento, 'MM/YYYY'),P1.RML_NUMERO_A "


        sql += ") group by RML_NUMERO_A, data"



        sql += " )  GROUP BY tarifa,data"
        'sql += ") group by tarifa,data  order  by to_date(data,'MM/YYYY')"


        Dim prmArray As New List(Of OleDbParameter)
        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))
        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If

        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))

        'If tarifa <> "" Then
        '    prmArray.Add(New OleDbParameter(":tarifa", UCase(tarifa.ToUpper)))
        'End If


        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)

        'Dim dt As DataTable = _DAO_commons.myDataTable(sql)

        'Response.Write(sql)
        'Response.End()

        Return dt
    End Function
#End Region

#Region "Ramais"
    Public Function MontaResumoGastoRamal(codigousuario As String, vencimento As String, Optional intervaloMes As Integer = 0) As DataTable

        Dim sql As String = ""
        sql += " select t.ramal, sum(t.chamadas)chamadas, sum(t.duracao)/60 minutagem, sum(t.gasto)+nvl(t.custo_servico,0) servico, nvl(t.credito,0)credito, nvl(t.custo_ramal,0)custo_ramal,sum(t.gasto)+nvl(t.custo_servico,0)+nvl(t.custo_ramal,0) gasto, t.modelo,t.codigo_usuario "
        sql += " from V_TARIFACAO2 t where 1=1  "


        If vencimento <> "" Then
            sql += " and t.data =to_char(add_months(to_date(:vencimento,'MM/YYYY'),:intervaloMes), 'MM/YYYY')"

        End If

        If codigousuario <> "" And codigousuario > 0 Then
            sql += " and t.codigo_usuario=:codigousuario"
        End If
        sql += " and to_date(t.data, 'MM/YYYY')>= to_date(to_char(add_months(sysdate,-12),'MM/YYYY'),'MM/YYYY') "
        sql += " group by t.ramal,nvl(t.credito,0),nvl(t.custo_ramal,0),nvl(t.custo_servico,0),t.modelo,t.codigo_usuario "


        'Response.Write(sql)
        'Response.End()

        Dim prmArray As New List(Of OleDbParameter)


        If vencimento <> "" Then
            prmArray.Add(New OleDbParameter(":vencimento", UCase(vencimento.ToUpper)))
        End If
        prmArray.Add(New OleDbParameter(":intervaloMes", intervaloMes))
        prmArray.Add(New OleDbParameter(":codigousuario", codigousuario.ToUpper))

        Dim dt As DataTable = _DAO_commons.myDataTableWithParams(sql, prmArray)




        Return dt

    End Function
#End Region

End Class
