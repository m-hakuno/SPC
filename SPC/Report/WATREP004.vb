Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class WATREP004

    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format
        If TxtClct.Text = "ＴＢＯＸ照会不可" Then
            TxtClct.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(3.0F)
            TxtClct.Font = New System.Drawing.Font("MS UI Gothic", 10)
            TxtClct.Alignment = Section.TextAlignment.Center
            Label53.Text = ""
        Else
            TxtClct.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(2.5F)
            TxtClct.Font = New System.Drawing.Font("MS UI Gothic", 11)
            TxtClct.Alignment = Section.TextAlignment.Right
            Label53.Text = "円"
        End If
        If TxtClct2.Text = "ＴＢＯＸ照会不可" Then
            TxtClct2.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(3.0F)
            TxtClct2.Font = New System.Drawing.Font("MS UI Gothic", 10)
            TxtClct2.Alignment = Section.TextAlignment.Center
            Label60.Text = ""
        Else
            TxtClct2.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(2.5F)
            TxtClct2.Font = New System.Drawing.Font("MS UI Gothic", 11)
            TxtClct2.Alignment = Section.TextAlignment.Right
            Label60.Text = "円"
        End If
        If TxtClct3.Text = "ＴＢＯＸ照会不可" Then
            TxtClct3.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(3.0F)
            TxtClct3.Font = New System.Drawing.Font("MS UI Gothic", 10)
            TxtClct3.Alignment = Section.TextAlignment.Center
            Label67.Text = ""
        Else
            TxtClct3.Width = GrapeCity.ActiveReports.SectionReport.CmToInch(2.5F)
            TxtClct3.Font = New System.Drawing.Font("MS UI Gothic", 11)
            TxtClct3.Alignment = Section.TextAlignment.Right
            Label67.Text = "円"
        End If
    End Sub

End Class
