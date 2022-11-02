Imports GrapeCity.ActiveReports 
Imports GrapeCity.ActiveReports.Document

Public Class CNSREP013

    Private Sub PageHeader1_Format(sender As Object, e As EventArgs) Handles PageHeader1.Format
        Dim ci As New System.Globalization.CultureInfo("ja-JP")
        Dim jp As New System.Globalization.JapaneseCalendar
        Dim dt As System.DateTime

        ci.DateTimeFormat.Calendar = jp
        dt = DateTime.Parse(System.DateTime.Now)
        TxtDate.Text = dt.ToString("yyyy年M月d日")
    End Sub


    Private Sub Detail1_Format(sender As Object, e As EventArgs) Handles Detail1.Format

        TxtSERIAL_NO1_1.Text = TxtSERIAL_NO1_1.Text.Substring(0, 1)
        If TxtSERIAL_NO1_2.Text.Length < 2 Then
            TxtSERIAL_NO1_2.Text = String.Empty
        Else
            TxtSERIAL_NO1_2.Text = TxtSERIAL_NO1_2.Text.Substring(1, 1)
        End If
        If TxtSERIAL_NO1_3.Text.Length < 3 Then
            TxtSERIAL_NO1_3.Text = String.Empty
        Else
            TxtSERIAL_NO1_3.Text = TxtSERIAL_NO1_3.Text.Substring(2, 1)
        End If
        If TxtSERIAL_NO1_4.Text.Length < 4 Then
            TxtSERIAL_NO1_4.Text = String.Empty
        Else
            TxtSERIAL_NO1_4.Text = TxtSERIAL_NO1_4.Text.Substring(3, 1)
        End If
        If TxtSERIAL_NO1_5.Text.Length < 5 Then
            TxtSERIAL_NO1_5.Text = String.Empty
        Else
            TxtSERIAL_NO1_5.Text = TxtSERIAL_NO1_5.Text.Substring(4, 1)
        End If
        If TxtSERIAL_NO1_6.Text.Length < 6 Then
            TxtSERIAL_NO1_6.Text = String.Empty
        Else
            TxtSERIAL_NO1_6.Text = TxtSERIAL_NO1_6.Text.Substring(5, 1)
        End If
        If TxtSERIAL_NO1_7.Text.Length < 7 Then
            TxtSERIAL_NO1_7.Text = String.Empty
        Else
            TxtSERIAL_NO1_7.Text = TxtSERIAL_NO1_7.Text.Substring(6, 1)
        End If
        If TxtSERIAL_NO1_8.Text.Length < 8 Then
            TxtSERIAL_NO1_8.Text = String.Empty
        Else
            TxtSERIAL_NO1_8.Text = TxtSERIAL_NO1_8.Text.Substring(7, 1)
        End If


        TxtSERIAL_NO2_1.Text = TxtSERIAL_NO2_1.Text.Substring(0, 1)
        If TxtSERIAL_NO2_2.Text.Length < 2 Then
            TxtSERIAL_NO2_2.Text = String.Empty
        Else
            TxtSERIAL_NO2_2.Text = TxtSERIAL_NO2_2.Text.Substring(1, 1)
        End If
        If TxtSERIAL_NO2_3.Text.Length < 3 Then
            TxtSERIAL_NO2_3.Text = String.Empty
        Else
            TxtSERIAL_NO2_3.Text = TxtSERIAL_NO2_3.Text.Substring(2, 1)
        End If
        If TxtSERIAL_NO2_4.Text.Length < 4 Then
            TxtSERIAL_NO2_4.Text = String.Empty
        Else
            TxtSERIAL_NO2_4.Text = TxtSERIAL_NO2_4.Text.Substring(3, 1)
        End If
        If TxtSERIAL_NO2_5.Text.Length < 5 Then
            TxtSERIAL_NO2_5.Text = String.Empty
        Else
            TxtSERIAL_NO2_5.Text = TxtSERIAL_NO2_5.Text.Substring(4, 1)
        End If
        If TxtSERIAL_NO2_6.Text.Length < 6 Then
            TxtSERIAL_NO2_6.Text = String.Empty
        Else
            TxtSERIAL_NO2_6.Text = TxtSERIAL_NO2_6.Text.Substring(5, 1)
        End If
        If TxtSERIAL_NO2_7.Text.Length < 7 Then
            TxtSERIAL_NO2_7.Text = String.Empty
        Else
            TxtSERIAL_NO2_7.Text = TxtSERIAL_NO2_7.Text.Substring(6, 1)
        End If
        If TxtSERIAL_NO2_8.Text.Length < 8 Then
            TxtSERIAL_NO2_8.Text = String.Empty
        Else
            TxtSERIAL_NO2_8.Text = TxtSERIAL_NO2_8.Text.Substring(7, 1)
        End If

    End Sub
End Class
