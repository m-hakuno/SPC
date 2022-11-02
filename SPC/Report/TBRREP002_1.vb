Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class TBRREP002_1
    Dim intPage As Integer = 0
    Public SbRep As New TBRREP002_2

    Sub New()
        InitializeComponent()

        Me.SubTBRREP002_2.Report = SbRep
    End Sub

End Class
