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
            My.Settings.startPositionTop = Top
            My.Settings.startPositionLeft = Left
            My.Settings.numberOfMonitors = Screen.AllScreens.Length
            My.Settings.Save()
        End If
    End Sub

    Public Sub updateUIbyCharCode(charcode As String)

        If Form1.isSelecting Then
            'for selecting Mode only
            loadInterface("e")
            setAssoCharMode(False)
            ShowAllLabels()
            Return
        End If

        If (charcode.Length = 0 And Form1.lastusedword = "") Then
            'Home page with lastusedword - display associate  char
            loadInterface("s")
            setAssoCharMode(False)
            HideAllLabels()
            Return
        End If

        If (charcode.Length = 0 And Form1.lastusedword <> "") Then
            'Home page with no lastusedword
            loadInterface("c")
            setAssoCharMode(True)
            ShowAllLabels()
            Return
        End If

        If (charcode.Length = 1) Then
            loadInterface(charcode)
            HideAllLabels()
            Return
        End If

        If (charcode.Length = 2) Then
            setAssoCharMode(False)
            'Char with two parts
            loadInterface("n")
            HideAllLabels()
        End If

        'no need for charcode.Length = 3 because if charcode.Length = 3 , it will in selecting mode
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
        l1.Location = PictureBox2.PointToClient(Me.PointToScreen(l1.Location))
        l1.Parent = PictureBox2
        l2.Location = PictureBox2.PointToClient(Me.PointToScreen(l2.Location))
        l2.Parent = PictureBox2
        l3.Location = PictureBox2.PointToClient(Me.PointToScreen(l3.Location))
        l3.Parent = PictureBox2
        l4.Location = PictureBox2.PointToClient(Me.PointToScreen(l4.Location))
        l4.Parent = PictureBox2
        l5.Location = PictureBox2.PointToClient(Me.PointToScreen(l5.Location))
        l5.Parent = PictureBox2
        l6.Location = PictureBox2.PointToClient(Me.PointToScreen(l6.Location))
        l6.Parent = PictureBox2
        l7.Location = PictureBox2.PointToClient(Me.PointToScreen(l7.Location))
        l7.Parent = PictureBox2
        l8.Location = PictureBox2.PointToClient(Me.PointToScreen(l8.Location))
        l8.Parent = PictureBox2
        l9.Location = PictureBox2.PointToClient(Me.PointToScreen(l9.Location))
        l9.Parent = PictureBox2


        If My.Computer.Info.OSVersion = "6.2.9200.0" And My.Computer.Info.OSFullName.Contains("Windows 8.1") Then
            'Unknown reason caused all items to shift left on this version of Windows 8.1. Fixing it with exceptional patches
            ApplyWin8ThemeBugPatch()
        End If
    End Sub

    Private Sub ApplyWin8ThemeBugPatch()
        shiftAllLabels(18, 0)

    End Sub

    Private Sub shiftAllLabels(x As Integer, y As Integer)
        l1.Top += y
        l2.Top += y
        l3.Top += y
        l4.Top += y
        l5.Top += y
        l6.Top += y
        l7.Top += y
        l8.Top += y
        l9.Top += y

        l1.Left += x
        l2.Left += x
        l3.Left += x
        l4.Left += x
        l5.Left += x
        l6.Left += x
        l7.Left += x
        l8.Left += x
        l9.Left += x
    End Sub

    Dim assoCharSize = False
    Private Sub setAssoCharMode(mode As Boolean)
        If (mode = assoCharSize) Then
            'Already in this mode. Need not to set again.
            Return
        End If
        If (mode = False) Then
            Dim labelsize As Single = 21.75
            l1.Size = New Size(43, 43)
            l1.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l1)
            l2.Size = New Size(43, 43)
            l2.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l2)
            l3.Size = New Size(43, 43)
            l3.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l3)
            l4.Size = New Size(43, 43)
            l4.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l4)
            l5.Size = New Size(43, 43)
            l5.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l5)
            l6.Size = New Size(43, 43)
            l6.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l6)
            l7.Size = New Size(43, 43)
            l7.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l7)
            l8.Size = New Size(43, 43)
            l8.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l8)
            l9.Size = New Size(43, 43)
            l9.Font = New Font("MingLiU_HKSCS", labelsize, FontStyle.Bold)
            shiftDownCorner(l9)
            assoCharSize = False
        Else
            Dim labelsize As Integer = 18
            Dim labelWidth As Integer = 28
            l1.Size = New Size(labelWidth, labelWidth)
            l1.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l1)
            l2.Size = New Size(labelWidth, labelWidth)
            l2.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l2)
            l3.Size = New Size(labelWidth, labelWidth)
            l3.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l3)
            l4.Size = New Size(labelWidth, labelWidth)
            l4.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l4)
            l5.Size = New Size(labelWidth, labelWidth)
            l5.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l5)
            l6.Size = New Size(labelWidth, labelWidth)
            l6.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l6)
            l7.Size = New Size(labelWidth, labelWidth)
            l7.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l7)
            l8.Size = New Size(labelWidth, labelWidth)
            l8.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l8)
            l9.Size = New Size(labelWidth, labelWidth)
            l9.Font = New Font("MingLiU_HKSCS", labelsize)
            shiftUpCorner(l9)
            assoCharSize = True
        End If
    End Sub

    Private Sub shiftUpCorner(label As Object)
        label.top -= 3
        label.left -= 3
    End Sub

    Private Sub shiftDownCorner(label As Object)
        label.top += 3
        label.left += 3
    End Sub

    Private Sub Panel1_MouseClick(sender As Object, e As MouseEventArgs) Handles Panel1.MouseClick

    End Sub

    Private Sub Panel1_Paint(sender As Object, e As PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub l1_MouseDown(sender As Object, e As MouseEventArgs) Handles l1.MouseDown, l2.MouseDown, l3.MouseDown, l4.MouseDown, l5.MouseDown, l6.MouseDown, l7.MouseDown, l8.MouseDown, l9.MouseDown
        If e.Button = MouseButtons.Right And Form1.isSelecting = True Then
            Dim labelString As String = sender.Text.ToString.Trim
            Form1.enterHomophonicMode(labelString)
        End If
    End Sub
End Class