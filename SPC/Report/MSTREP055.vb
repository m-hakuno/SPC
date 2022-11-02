Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class MSTREP055
    Dim m_intRecord As Integer = 0

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        m_intRecord += 1
        If m_intRecord < 32 Then
            LinePageEnd.Visible = False
        Else
            LinePageEnd.Visible = True
            m_intRecord = 0
        End If
    End Sub


End Class
