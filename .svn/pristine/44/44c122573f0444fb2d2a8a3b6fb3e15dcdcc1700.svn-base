Imports System.Data
Imports System.IO

Imports ClosedXML.Excel

Partial Class GestaoHistoricos
    Inherits System.Web.UI.Page
    Dim _dao_commons As New DAO_Commons
    Private _dao As New DAO_Gerencial


    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session("conexao") Is Nothing Then
            Response.Write("conecte novamente")
            Response.End()
        End If

        _dao_commons.strConn = Session("conexao")
        _dao.strConn = Session("conexao")

        If Not Page.IsPostBack Then
            Dim context As HttpContext = HttpContext.Current
            Title = Session("Titulo")
            ' Create a new table.
            Dim taskTable As New DataTable("TaskList")
            Dim info_rel As New System.Web.UI.HtmlControls.HtmlGenericControl
            info_rel.InnerHtml = Session("HTML_Context")


            '*************************************************************************************************************
            '**************** ROTINA ESPECIFICA PARA RELATÓRIO DE NUMEROS MAIS CHAMADOS **********************************
            '*************************************************************************************************************

            If Request.QueryString("linhaschamadas") = "1" Then
                taskTable = _dao_commons.myDataTable(Session("Query2") + Request.QueryString("parametro") + "' group by p1.rml_numero_a order by CAST(Valor as NUMERIC) desc")

                For Each Row As DataRow In taskTable.Rows
                    Row.Item("duracao") = TimeSpan.FromSeconds(Row.Item("duracao"))
                    Row.Item("Valor") = FormatCurrency(Row.Item("Valor").ToString.Replace(".", ","))
                Next

                Title = "NÚMEROS QUE CHAMARAM"
                lbtitle.Text = "Números que chamaram a linha: " + Request.QueryString("parametro")
                'lbtitle.Text = "Consumo Detalhado Linha: " + Request.QueryString("parametro")
                'Page.Title = "Consumo Detalhado Linha: " + Request.QueryString("parametro")

                '*************************************************************************************************************
                '**************** FIM ROTINA ESPECIFICA PARA RELATÓRIO DE NUMEROS MAIS CHAMADOS ******************************
                '*************************************************************************************************************
                '*************************************************************************************************************

            Else
                taskTable = Session("Tabela")
                lbtitle.Text = Session("Nome")
                Page.Title = Session("Nome")
            End If


            If lbtitle.Text = "" Then
                Me.divWell.Visible = False
            Else
                Me.divWell.Visible = True
            End If

            Information.Controls.Add(info_rel)
            lbdatenow.Text = Now

            'Persist the table in the Session object.
            Session("TaskTable") = taskTable

            'se for HTML com mais de 10000 registros coloca paginação
            If taskTable.Rows.Count > 1000000 And Session("Contexto") = "HTML" Then
                TaskGridView.AllowPaging = True
                TaskGridView.PageSize = 1000
            End If

            TaskGridView.DataSource = Session("TaskTable")
            TaskGridView.DataBind()
            If TaskGridView.Rows.Count > 0 Then
                TaskGridView.HeaderRow.TableSection = TableRowSection.TableHeader
                TaskGridView.FooterRow.TableSection = TableRowSection.TableFooter

                If Session("PutCssInFooter") = "1" Then
                    TaskGridView.Rows.Item(TaskGridView.Rows.Count - 1).CssClass = "GridRelatorioFooter"
                End If

            End If

            Session("PutCssInFooter") = "0"
            If Session("Contexto") = "Excel" Then
                ExportExcel()
                'doExcel()
            End If


        End If
    End Sub

    Protected Sub TaskGridView_Sorting(ByVal sender As Object, ByVal e As GridViewSortEventArgs)

        'Retrieve the table from the session object.
        Dim dt = TryCast(Session("TaskTable"), DataTable)

        If dt IsNot Nothing Then

            'Sort the data.
            Try
                dt.DefaultView.Sort = e.SortExpression.Trim & " " & GetSortDirection(e.SortExpression.Trim)
                TaskGridView.DataSource = Session("TaskTable")
                TaskGridView.DataBind()
            Catch ex As Exception

            End Try

        End If

    End Sub


    Private Function GetSortDirection(ByVal column As String) As String

        ' By default, set the sort direction to ascending.
        Dim sortDirection = "ASC"

        ' Retrieve the last column that was sorted.
        Dim sortExpression = TryCast(ViewState("SortExpression"), String)

        If sortExpression IsNot Nothing Then
            ' Check if the same column is being sorted.
            ' Otherwise, the default value can be returned.
            If sortExpression = column Then
                Dim lastDirection = TryCast(ViewState("SortDirection"), String)
                If lastDirection IsNot Nothing _
                  AndAlso lastDirection = "ASC" Then

                    sortDirection = "DESC"

                End If
            End If
        End If
    End Function

    Sub doExcel()

        'se o grid tiver mais que 65536  linhas não podemos exportar
        If Me.TaskGridView.Rows.Count.ToString + 1 < 65536 Then

            'Me.Controls.Remove(Me.FindControl("ScriptManager1"))
            TaskGridView.AllowPaging = "False"

            Dim tw As New StringWriter()
            Dim hw As New System.Web.UI.HtmlTextWriter(tw)
            Dim frm As HtmlForm = New HtmlForm()

            Response.ContentType = "application/vnd.ms-excel"
            Response.AddHeader("content-disposition", "attachment;filename=" & Session("Nome") & ".xls")
            Response.ContentEncoding = System.Text.Encoding.GetEncoding("Windows-1252")
            Response.Charset = "ISO-8859-1"
            EnableViewState = False

            Controls.Add(frm)
            frm.Controls.Add(pagina)
            frm.RenderControl(hw)

            Response.Write("<style> td{text-align: right;mso-number-format: \@;white-space: nowrap;} </style>")
            Response.Write(tw.ToString())
            Response.End()

            TaskGridView.AllowPaging = "True"
            TaskGridView.DataBind()

        Else
            'LblError.Text = " planilha possui muitas linhas, não é possível exportar para o EXcel"
        End If
    End Sub

    Sub ExportExcel()
        Dim dt As New DataTable("GridView_Data")
        For Each cell As TableCell In TaskGridView.HeaderRow.Cells
            dt.Columns.Add(cell.Text)
        Next
        For Each row As GridViewRow In TaskGridView.Rows
            dt.Rows.Add()
            For i As Integer = 0 To row.Cells.Count - 1
                Dim texto As String = StripHtml(row.Cells(i).Text)
                dt.Rows(dt.Rows.Count - 1)(i) = texto
            Next
        Next
        Using wb As New XLWorkbook()
            wb.Worksheets.Add(dt)

            Response.Clear()
            Response.Buffer = True
            Response.Charset = ""
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"
            Response.AddHeader("content-disposition", "attachment;filename=" & Session("Nome") & ".xlsx")
            Using MyMemoryStream As New MemoryStream()
                wb.SaveAs(MyMemoryStream)
                MyMemoryStream.WriteTo(Response.OutputStream)
                Response.Flush()
                Response.[End]()
            End Using
        End Using
    End Sub

    Protected Function StripHtml(Txt As String) As String
        Return Regex.Replace(Txt, "<(.|\n)*?>", String.Empty)
    End Function

    Protected Sub TaskGridView_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles TaskGridView.PageIndexChanging
        Me.TaskGridView.PageIndex = e.NewPageIndex
        TaskGridView.DataSource = Session("TaskTable")
        TaskGridView.DataBind()
    End Sub

    Protected Sub TaskGridView_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles TaskGridView.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            For Each cell As TableCell In e.Row.Cells
                cell.Text = Server.HtmlDecode(cell.Text)
            Next
        End If
        If e.Row.RowType = DataControlRowType.Header Then
            For Each cell As TableCell In e.Row.Cells
                cell.Text = Server.HtmlDecode(cell.Text)
            Next
        End If
    End Sub

    Private Sub btExcel_Click(sender As Object, e As EventArgs) Handles btExcel.Click
        doExcel()
    End Sub

    Private Sub GestaoHistoricos_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete


    End Sub

End Class
