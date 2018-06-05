Imports Microsoft.VisualBasic


Public Class DataSerie
    Private _valor As String

    Public Property Valor As Double
        Get
            Return _valor
        End Get
        Set(value As Double)
            _valor = value
        End Set
    End Property

    Public Sub New(pvalor As Double)
        _valor = pvalor
    End Sub

End Class

Public Class appSerie


    Private _name As String

    Public Property name As String
        Get
            Return _name
        End Get
        Set(value As String)
            _name = value
        End Set
    End Property

    Public Property data As List(Of DataSerie)
        Get
            Return _data
        End Get
        Set(value As List(Of DataSerie))
            _data = value
        End Set
    End Property

    Private _data As New List(Of DataSerie)

End Class
