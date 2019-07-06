Public Class shortcuts
    Public visable As Boolean = False
    Dim previousLocation As Point
    Public Sub setVisable(ByVal states As Boolean)
        If states = True Then
            Me.Show()
            Me.Location = New Point(largeUI.Left - Me.Width, largeUI.Top)
            previousLocation = Me.Location
            visable = True
        Else
            Me.Hide()
            visable = False
        End If
    End Sub

    Public Sub adjustThisFormLocation(ByVal newLocation As Point)
        Me.Location = New Point(newLocation.X - Me.Width, newLocation.Y)
    End Sub

    Private Sub shortcuts_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

End Class