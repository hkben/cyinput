Public Class largeUI
    Private Sub largeUI_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        'Close Form1 to terminate the program
        Form1.Close()
    End Sub
    Private newpoint As System.Drawing.Point
    Private xpos1 As Integer
    Private ypos1 As Integer

    Private Sub pnlTopBorder_MouseDown(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseDown, Label1.MouseDown
        xpos1 = Control.MousePosition.X - Me.Location.X
        ypos1 = Control.MousePosition.Y - Me.Location.Y
    End Sub

    Private Sub pnlTopBorder_MouseMove(sender As Object, e As System.Windows.Forms.MouseEventArgs) Handles Panel1.MouseMove, Label1.MouseMove
        If e.Button = Windows.Forms.MouseButtons.Left Then
            newpoint = Control.MousePosition
            newpoint.X -= (xpos1)
            newpoint.Y -= (ypos1)
            Me.Location = newpoint
        End If
    End Sub

    Public Sub updateUIbyCharCode(charcode As String)
        If (charcode.Length = 1) Then
            'Selected first key shape
            If (charcode.Substring(0, 1) = "0") Then
                loadInterface("e")
                ShowAllLabels()
            Else
                loadInterface(charcode)
                HideAllLabels()
            End If
        ElseIf (charcode.Length >= 2 AndAlso charcode.Substring(0, 2) = "09") Then
            'Punct selectio mode and scroll down
            loadInterface("e")
            ShowAllLabels()
        ElseIf (charcode.Length = 0 And Form1.lastusedword <> "") Then
            'Show word select interface instead of index
            loadInterface("c")
            HideAllLabels()
        ElseIf (charcode.Length = 2) Then
            If (charcode.Substring(1, 1) = "0") Then
                'First name, not implemented yet
                loadInterface("e")
                ShowAllLabels()
            Else
                'Char with two parts
                loadInterface("n")
                HideAllLabels()
            End If

        ElseIf (charcode.Length >= 3) Then
            loadInterface("e")
            ShowAllLabels()
            'Move the words from Form1 to this form

        Else
            'No char inside charcode
            HideAllLabels()
            loadInterface("s")
        End If
        Console.WriteLine(charcode.Length)

    End Sub
    Private Sub loadTextFromMiniUI()
        l1.Text = Form1.Label7.Text
        l2.Text = Form1.Label8.Text
        l3.Text = Form1.Label9.Text
        l4.Text = Form1.Label4.Text
        l5.Text = Form1.Label5.Text
        l6.Text = Form1.Label6.Text
        l7.Text = Form1.Label1.Text
        l8.Text = Form1.Label2.Text
        l9.Text = Form1.Label3.Text
    End Sub

    Private Sub HideAllLabels()
        l1.Visible = False
        l2.Visible = False
        l3.Visible = False
        l4.Visible = False
        l5.Visible = False
        l6.Visible = False
        l7.Visible = False
        l8.Visible = False
        l9.Visible = False

    End Sub

    Private Sub ShowAllLabels()
        l1.Visible = True
        l2.Visible = True
        l3.Visible = True
        l4.Visible = True
        l5.Visible = True
        l6.Visible = True
        l7.Visible = True
        l8.Visible = True
        l9.Visible = True

    End Sub
    Private Sub loadInterface(key As String)
        Dim path As String = IO.Path.GetTempPath.ToString & "cyinput\lui\"
        PictureBox2.BackgroundImage = System.Drawing.Bitmap.FromFile(path & "cyi_" & key & ".png")
    End Sub

    Private Sub largeUI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        HideAllLabels()
    End Sub

    Private Sub Panel1_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel1.MouseClick
    End Sub
End Class