Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class TBRREP002
    Dim SbRep As New TBRREP002_1
    Dim m_dsData As DataSet

    Public Sub New(ByVal dsData As DataSet)
        InitializeComponent()

        m_dsData = dsData
        SbRep.SbRep.DataSource = m_dsData.Tables(0)
        Me.SubTBRREP002_1.Report = SbRep

    End Sub
End Class
