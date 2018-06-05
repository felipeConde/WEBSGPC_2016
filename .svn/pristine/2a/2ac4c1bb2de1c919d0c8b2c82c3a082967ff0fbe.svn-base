Imports System.Net
Partial Class testePDF
    Inherits System.Web.UI.Page

    Private Sub testePDF_Load(sender As Object, e As EventArgs) Handles Me.Load

        'geraPDF()

    End Sub

    Sub geraPDF()
        Dim ArquivoPDF As String = "http://www.google.com"
        If ArquivoPDF = "" Then
            ArquivoPDF = Server.MapPath("teste.pdf")
        End If
        Try
            Dim clienteWeb As New WebClient()
            Dim arquivoBuffer As [Byte]() = clienteWeb.DownloadData(ArquivoPDF)
            If arquivoBuffer IsNot Nothing Then
                Response.ContentType = "application/pdf"
                Response.AddHeader("content-length", arquivoBuffer.Length.ToString())
                Response.AddHeader("Content-Disposition", "attachment;filename=arquivo.pdf")
                Response.BinaryWrite(arquivoBuffer)
            End If
        Catch ex As Exception
            'lblmsg.Text = " Erro : " & ex.Message()
        End Try
    End Sub

    Sub teste()

        Dim strString As String = ""

        For Each _item As Object In Page.Controls
            strString += _item.InnerHtml
        Next

        Response.Write(strString)

    End Sub

    Private Sub testePDF_PreRenderComplete(sender As Object, e As EventArgs) Handles Me.PreRenderComplete
        ' teste()



    End Sub

    Private Sub testePDF_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim eventArg As String = Request("__EVENTARGUMENT")
        If eventArg = "printPDF" Then
            'Response.Write("You got it !")
            teste2()
        End If
    End Sub

    Sub teste2()
        Dim texto As String = Me.txtHTML.Text

        Response.Write(texto)
        Response.End()
    End Sub
End Class
