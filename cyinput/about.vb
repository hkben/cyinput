Imports System.Reflection

Public Class about
    Private Sub about_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Label5.Text = Application.ProductVersion
        Dim fecha As Date = IO.File.GetCreationTime(Assembly.GetExecutingAssembly().Location)
        Label6.Text = fecha.ToLocalTime.ToString()
    End Sub

    Private Sub LinkLabel1_LinkClicked(sender As Object, e As LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        Process.Start("https://github.com/tobychui/cyinput")
    End Sub
End Class