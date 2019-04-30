Imports System.IO
Imports System.IO.Compression
Public Class Form1
    'System Key Hooker Declarations
    Private hkkp0 As VBHotkeys.GlobalHotkey
    Private hkkp1 As VBHotkeys.GlobalHotkey
    Private hkkp2 As VBHotkeys.GlobalHotkey
    Private hkkp3 As VBHotkeys.GlobalHotkey
    Private hkkp4 As VBHotkeys.GlobalHotkey
    Private hkkp5 As VBHotkeys.GlobalHotkey
    Private hkkp6 As VBHotkeys.GlobalHotkey
    Private hkkp7 As VBHotkeys.GlobalHotkey
    Private hkkp8 As VBHotkeys.GlobalHotkey
    Private hkkp9 As VBHotkeys.GlobalHotkey
    Private hkkpd As VBHotkeys.GlobalHotkey
    Private hkkpp As VBHotkeys.GlobalHotkey
    Private hkkpm As VBHotkeys.GlobalHotkey
    Private hkkpq As VBHotkeys.GlobalHotkey
    'Table storage variables
    Dim relatedCharTable As String()
    Dim mappedCharTable As String()
    Dim cj5_mapping As String()
    Dim b5xp_mapping As String()
    'Logical Processing Variables
    Dim charset As String = ""
    Dim table As String()
    Dim assoicateMode As Boolean = False
    Dim punctMode As Boolean = False
    Dim textArray As New ArrayList
    Dim showingTextArrayIndex As Integer = 0
    Dim lastusedword As String = ""
    Dim cj5buffer As String = ""

    Dim drag As Boolean
    Dim mouseX As Integer
    Dim mouseY As Integer

    Dim cyinputEnable As Boolean

    Private Function appendToCharSet(number As String)
        charset = charset & number.ToString
        Return charset.ToString
    End Function

    Private Sub LoadTable()
        relatedCharTable = My.Resources.related_char.ToString.Split(vbNewLine)
        mappedCharTable = My.Resources.mapped_char.Split(vbNewLine)
        cj5_mapping = My.Resources.cj5_map.Split(vbNewLine)
        b5xp_mapping = My.Resources.b5xp_map.Split(vbNewLine)
        textArray.Clear()
        textArray.AddRange({"個", "能", "的", "到", "資", "就", "你", "這", "好"})
        drawText()
        paintPictureBoxes(0)
        showPicturebox()
    End Sub

    Private Sub showPicturebox()
        PictureBox1.Visible = True
        PictureBox2.Visible = True
        PictureBox3.Visible = True
        PictureBox4.Visible = True
        PictureBox5.Visible = True
        PictureBox6.Visible = True
        PictureBox7.Visible = True
        PictureBox8.Visible = True
        PictureBox9.Visible = True
    End Sub

    Private Sub hidePicturebox()
        PictureBox1.Visible = False
        PictureBox2.Visible = False
        PictureBox3.Visible = False
        PictureBox4.Visible = False
        PictureBox5.Visible = False
        PictureBox6.Visible = False
        PictureBox7.Visible = False
        PictureBox8.Visible = False
        PictureBox9.Visible = False
    End Sub
    Private Sub handlePage()
        If charset.Length = 1 Then
            If charset = "0" Then
                If lastusedword = "" Then
                    'Not typing any thing yet
                    punctMode = True
                    Dim wordlist As String = getWordList()
                    textArray.Clear()
                    For Each c As Char In wordlist
                        textArray.Add(c.ToString)
                    Next
                    showingTextArrayIndex = 0
                    drawText()
                    hidePicturebox()
                    Label10.Text = "下頁"
                    Label11.Text = "取消"
                    Console.WriteLine("Punct Mode Enabled")
                Else
                    'Select related char
                    hidePicturebox()
                    assoicateMode = True
                    lastusedword = ""
                    Label10.Text = "下頁"
                    Label11.Text = "取消"
                    Console.WriteLine("Selecting Assoicated Word")
                End If

            Else
                paintPictureBoxes(charset)
                showPicturebox()
                Label10.Text = "  "
                Label11.Text = "取消"
            End If
        ElseIf charset.Length = 2 And punctMode = False And assoicateMode = False Then
            If charset.Substring(charset.Length - 1, 1) = "0" Then
                'First name system is not enabled on this version of cyinput
                charset = charset.Substring(0, 1)
            Else
                'Typing normally
                paintPictureBoxes(0)
                showPicturebox()
                textArray.Clear()
                drawText()
                Label10.Text = "確定"
                Label11.Text = "取消"
            End If

        ElseIf charset.Length = 2 And punctMode = False And assoicateMode = True Then
            'Selecting Assoicated Word
            Dim selectedPos = charset.Substring(charset.Length - 1, 1)
            Dim wordtosend As String = ""
            Select Case selectedPos
                Case 1
                    wordtosend = (Label7.Text)
                Case 2
                    wordtosend = (Label8.Text)
                Case 3
                    wordtosend = (Label9.Text)
                Case 4
                    wordtosend = (Label4.Text)
                Case 5
                    wordtosend = (Label5.Text)
                Case 6
                    wordtosend = (Label6.Text)
                Case 7
                    wordtosend = (Label1.Text)
                Case 8
                    wordtosend = (Label2.Text)
                Case 9
                    wordtosend = (Label3.Text)
            End Select
            SendOut(wordtosend)
            lastusedword = wordtosend
            handleNextWord()
            assoicateMode = False


        ElseIf charset.Length = 2 And punctMode = True Then
            'Choosing punctuation from the list
            If charset.Substring(charset.Length - 1, 1) = "9" Then
                'Choosing advance punct mode (09)
                Dim wordlist As String = getWordList()
                textArray.Clear()
                For Each c As Char In wordlist
                    textArray.Add(c.ToString)
                Next
                showingTextArrayIndex = 0
                drawText()
                hidePicturebox()
                Label10.Text = "下頁"
                Label11.Text = "取消"
            Else
                'Select and send the selected punct
                Dim selectedPos As Integer = charset.Substring(charset.Length - 1, 1)
                Dim wordtosend As String = "*"
                Select Case selectedPos
                    Case 1
                        wordtosend = (Label7.Text)
                    Case 2
                        wordtosend = (Label8.Text)
                    Case 3
                        wordtosend = (Label9.Text)
                    Case 4
                        wordtosend = (Label4.Text)
                    Case 5
                        wordtosend = (Label5.Text)
                    Case 6
                        wordtosend = (Label6.Text)
                    Case 7
                        wordtosend = (Label1.Text)
                    Case 8
                        wordtosend = (Label2.Text)
                End Select
                SendOut(wordtosend)
                lastusedword = ""
                dothandler()
                punctMode = False
            End If

        ElseIf charset.Length = 3 And punctMode = False Then
            'Typing normal text in 3rd stage
            Dim wordlist As String = getWordList()
            textArray.Clear()
            For Each c As Char In wordlist
                textArray.Add(c.ToString)
            Next
            showingTextArrayIndex = 0
            drawText()
            hidePicturebox()
            Label10.Text = "下頁"
            Label11.Text = "取消"

        ElseIf charset.Length = 3 And punctMode = True Then
            'Selecting punct in 09 mode
            If charset.Substring(charset.Length - 1, 1) = "0" Then
                showingTextArrayIndex += 9
                drawText()
            Else
                Dim selectedPos As Integer = charset.Substring(charset.Length - 1, 1)
                Dim wordtosend As String = ""
                Select Case selectedPos
                    Case 1
                        wordtosend = (Label7.Text)
                    Case 2
                        wordtosend = (Label8.Text)
                    Case 3
                        wordtosend = (Label9.Text)
                    Case 4
                        wordtosend = (Label4.Text)
                    Case 5
                        wordtosend = (Label5.Text)
                    Case 6
                        wordtosend = (Label6.Text)
                    Case 7
                        wordtosend = (Label1.Text)
                    Case 8
                        wordtosend = (Label2.Text)
                    Case 9
                        wordtosend = (Label3.Text)
                End Select
                SendOut(wordtosend)
                lastusedword = ""
                dothandler()
                punctMode = False
            End If
        Else
            If charset.Substring(charset.Length - 1, 1) = "0" Then
                'Next page in word selection
                showingTextArrayIndex += 9
                drawText()
            Else
                'Send the selected word
                Dim selectedPos As Integer = charset.Substring(charset.Length - 1, 1)
                Dim wordtosend As String = ""
                Select Case selectedPos
                    Case 1
                        wordtosend = (Label7.Text)
                    Case 2
                        wordtosend = (Label8.Text)
                    Case 3
                        wordtosend = (Label9.Text)
                    Case 4
                        wordtosend = (Label4.Text)
                    Case 5
                        wordtosend = (Label5.Text)
                    Case 6
                        wordtosend = (Label6.Text)
                    Case 7
                        wordtosend = (Label1.Text)
                    Case 8
                        wordtosend = (Label2.Text)
                    Case 9
                        wordtosend = (Label3.Text)
                End Select
                SendOut(wordtosend)
                lastusedword = wordtosend
                handleNextWord()
                If punctMode = True Then
                    punctMode = False
                End If
            End If
        End If

    End Sub
    Private Sub handleNextWord()
        'After finish typing one word
        charset = ""
        paintPictureBoxes(0)
        showPicturebox()
        textArray.Clear()
        Dim returnassiate = loadAssoicatedWords()
        Dim tmparray(10) As String
        Dim counter As Integer = 1
        For Each c As Char In returnassiate
            tmparray(counter) = c.ToString
            counter += 1
        Next
        'Return array reordering
        textArray.Add(tmparray(7))
        textArray.Add(tmparray(8))
        textArray.Add(tmparray(9))
        textArray.Add(tmparray(4))
        textArray.Add(tmparray(5))
        textArray.Add(tmparray(6))
        textArray.Add(tmparray(1))
        textArray.Add(tmparray(2))
        textArray.Add(tmparray(3))
        showingTextArrayIndex = 0
        drawText()
        Label10.Text = "選字"
        Label11.Text = "標點"
    End Sub

    Private Function loadAssoicatedWords()
        Dim target As String = lastusedword
        Dim returnvalue As String = "個能的到資就你這好"
        For Each i As String In relatedCharTable
            If i.Length > 2 Then
                If i.Substring(1, 1).Contains(target) Then
                    i = i.Substring(2, i.Length - 2)
                    If i.Length < returnvalue.Length Then
                        returnvalue = i & returnvalue.Substring(i.Length, returnvalue.Length - i.Length)
                        Return returnvalue
                    Else
                        Return i
                    End If
                End If
            End If
        Next
        Return returnvalue
    End Function

    Private Function getWordList()
        For Each i As String In mappedCharTable
            If i.Contains(charset.ToString) Then
                Dim returnmsg As String
                If charset = "0" Then
                    returnmsg = i.Replace(i.Substring(0, 1), "")
                Else
                    returnmsg = i.Replace(i.Substring(0, charset.Length + 1), "")
                End If
                Return returnmsg
            End If
        Next
        Return "*********"
    End Function
    Private Sub paintPictureBoxes(id As String)
        Dim path As String = IO.Path.GetTempPath.ToString & "cyinput\img\"
        PictureBox7.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\1.png")
        PictureBox8.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\2.png")
        PictureBox9.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\3.png")
        PictureBox4.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\4.png")
        PictureBox5.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\5.png")
        PictureBox6.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\6.png")
        PictureBox1.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\7.png")
        PictureBox2.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\8.png")
        PictureBox3.BackgroundImage = System.Drawing.Bitmap.FromFile(path & id & "\9.png")


    End Sub
    Private Sub drawText()
        On Error Resume Next
        Dim sp As Integer = showingTextArrayIndex
        If textArray.Count = 0 Then
            textArray.AddRange({"", "", "", "", "", "", "", "", ""})
        End If
        If sp + 8 > textArray.Count Then
            sp = 0
            showingTextArrayIndex = 0
        End If
        Label1.Text = textArray(sp)
        Label2.Text = textArray(sp + 1)
        Label3.Text = textArray(sp + 2)
        Label4.Text = textArray(sp + 3)
        Label5.Text = textArray(sp + 4)
        Label6.Text = textArray(sp + 5)
        Label7.Text = textArray(sp + 6)
        Label8.Text = textArray(sp + 7)
        Label9.Text = textArray(sp + 8)
    End Sub

    Private Sub enterPunctMode()
        punctMode = True
        charset = "0"
        Dim wordlist As String = getWordList()
        textArray.Clear()
        For Each c As Char In wordlist
            textArray.Add(c.ToString)
        Next
        showingTextArrayIndex = 0
        drawText()
        hidePicturebox()
        Label10.Text = "下頁"
        Label11.Text = "取消"
        Console.WriteLine("Punct Mode Enabled")
    End Sub
    Private Sub dothandler(Optional reset As Boolean = False)
        If charset.Length > 0 Or reset Then
            If lastusedword = "" And assoicateMode = False And punctMode = False Then
                'Reset
                charset = ""
                lastusedword = ""
                paintPictureBoxes(0)
                showPicturebox()
                textArray.Clear()
                textArray.AddRange({"個", "能", "的", "到", "資", "就", "你", "這", "好"})
                showingTextArrayIndex = 0
                drawText()
                Label10.Text = "標點"
                Label11.Text = "取消"
            ElseIf assoicateMode = True Then
                'Cancel associate mode and return to default
                assoicateMode = False
                charset = ""
                paintPictureBoxes(0)
                showPicturebox()
                textArray.Clear()
                textArray.AddRange({"個", "能", "的", "到", "資", "就", "你", "這", "好"})
                showingTextArrayIndex = 0
                drawText()
                Label10.Text = "標點"
                Label11.Text = "取消"
            Else
                'Cancel punct mode
                charset = ""
                paintPictureBoxes(0)
                showPicturebox()
                textArray.Clear()
                textArray.AddRange({"個", "能", "的", "到", "資", "就", "你", "這", "好"})
                showingTextArrayIndex = 0
                drawText()
                lastusedword = ""
                punctMode = False
                Label10.Text = "標點"
                Label11.Text = "取消"


            End If
        ElseIf charset.Length = 0 And lastusedword <> "" Then
            'Punct mode
            enterPunctMode()
        ElseIf charset.Length = 0 And lastusedword = "" Then
            'Cancel button, do nothing
            If punctMode = True Then
                punctMode = False
            End If
        End If
    End Sub

    Private Sub unzippingToTemp()
        Dim temppath As String = IO.Path.GetTempPath.ToString & "cyinput\"
        My.Computer.FileSystem.CreateDirectory(temppath)
        Dim b As Byte() = My.Resources.img
        My.Computer.FileSystem.WriteAllBytes(temppath & "tmp.zip", b, False)
        ZipFile.ExtractToDirectory(temppath & "tmp.zip", temppath)
    End Sub

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        If My.Computer.FileSystem.DirectoryExists(IO.Path.GetTempPath.ToString & "cyinput\") = False Then
            'MsgBox(IO.Path.GetTempPath.ToString & "cyinput\")
            unzippingToTemp()
        End If

        'Start key binding process
        hkkp0 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad0, Me)
        hkkp1 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad1, Me)
        hkkp2 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad2, Me)
        hkkp3 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad3, Me)
        hkkp4 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad4, Me)
        hkkp5 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad5, Me)
        hkkp6 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad6, Me)
        hkkp7 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad7, Me)
        hkkp8 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad8, Me)
        hkkp9 = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.NumPad9, Me)
        hkkpd = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Decimal, Me)
        'Added in new support for changing page with + and - sign
        hkkpp = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Add, Me)
        hkkpm = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Subtract, Me)
        hkkpq = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Divide, Me)

        'Register keypad /
        hkkpq.Register()

        OnInputEnable()

    End Sub

    Private Sub OnInputEnable()
        'Sending "Cancel" for reset (Reset charset that last input left)
        dothandler(True)

        hkkp0.Register()
        hkkp1.Register()
        hkkp2.Register()
        hkkp3.Register()
        hkkp4.Register()
        hkkp5.Register()
        hkkp6.Register()
        hkkp7.Register()
        hkkp8.Register()
        hkkp9.Register()
        hkkpd.Register()
        'Register the + and - button as well
        hkkpp.Register()
        hkkpm.Register()
        'Loading logical program section
        LoadTable()
        Me.TopLevel = True
        Me.TopMost = True

        cyinputEnable = True

    End Sub

    Private Function ResolveAssemblies(sender As Object, e As System.ResolveEventArgs) As Reflection.Assembly
        Dim desiredAssembly = New Reflection.AssemblyName(e.Name)

        If desiredAssembly.Name = "VBHotkeyLib" Then             'replace DLL_REFERENCE_NAME with reference name
            Return Reflection.Assembly.Load(My.Resources.VBHotkeyLib)     'replace DLL_RESOURCE_NAME with your assembly's resource name
        Else
            Return Nothing
        End If
    End Function
    Private Sub HandleHotkey(keycode As Integer)
        Select Case (keycode)
            Case 96
                'Keypad 0
                appendToCharSet(0)
                handlePage()
            Case 97
                'Keypad 1
                appendToCharSet(1)
                handlePage()
            Case 98
                'Keypad 2
                appendToCharSet(2)
                handlePage()
            Case 99
                'Keypad 3
                appendToCharSet(3)
                handlePage()
            Case 100
                'Keypad 4
                appendToCharSet(4)
                handlePage()
            Case 101
                'Keypad 5
                appendToCharSet(5)
                handlePage()
            Case 102
                'Keypad 6
                appendToCharSet(6)
                handlePage()
            Case 103
                'Keypad 7
                appendToCharSet(7)
                handlePage()
            Case 104
                'Keypad 8
                appendToCharSet(8)
                handlePage()
            Case 105
                'Keypad 9
                appendToCharSet(9)
                handlePage()
            Case 110
                'Keypad dot
                dothandler()
            Case 107
                '+ pressed
                If (charset.Length >= 3 Or (punctMode And charset.Length >= 2)) Then
                    appendToCharSet(0)
                    handlePage()
                End If
            Case 109
                '- pressed
                If (charset.Length > 3 Or (punctMode And charset.Length > 2)) Then
                    charset = charset.Substring(0, charset.Length - 1)
                    handlePage()
                End If
            Case 111
                '/ pressed
                ToggleInputWindow()
        End Select
        Console.WriteLine("Charset: " & charset & "," & lastusedword & ", Punct: " & punctMode & ", Assoc: " & assoicateMode & ",Keycode" & keycode)

    End Sub

    Private Function GetKey(IntPtr As Int32)
        Return IntPtr >> 16
    End Function

    Protected Overrides Sub WndProc(ByRef m As System.Windows.Forms.Message)
        If (m.Msg = VBHotkeys.Constants.WM_HOTKEY_MSG_ID) Then
            HandleHotkey(GetKey(Convert.ToInt32(m.LParam.ToString)))
        End If
        MyBase.WndProc(m)
    End Sub

    Private Sub Form1_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If cyinputEnable Then
            OnInputDisable()
        End If

        If Not hkkpq.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If

    End Sub

    Private Sub OnInputDisable()
        If Not hkkp0.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp1.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp2.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp3.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp4.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp5.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp6.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp7.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp8.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkp9.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkpd.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If

        If Not hkkpp.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If
        If Not hkkpm.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If

        cyinputEnable = False

    End Sub

    Private Sub ToggleInputWindow()
        If cyinputEnable Then
            Me.Hide()
            OnInputDisable()
        Else
            Me.Show()
            OnInputEnable()
        End If
    End Sub

    Private Sub WriteLine(ByVal message As String)
        'TextBox1.Text &= message & Environment.NewLine
    End Sub
    Private Function getCJ5(text)
        Dim code As String = ""
        For Each i As String In cj5_mapping
            If i.Substring(0, 2).Contains(text) Then
                Return i.Substring(2, i.Length - 2)
            End If
        Next
        Return code
    End Function

    Private Function getb5xp(text)
        Dim code As String = ""
        For Each i As String In b5xp_mapping
            If i.Substring(0, 2).Contains(text) Then
                Return i.Substring(2, i.Length - 2)
            End If
        Next
        Return code
    End Function


    Private Sub SendOut(text As String)
        If text = "*" Then
            'This word do not exists
            Return
        End If

        Dim mode As Integer = My.Settings.outputMode

        If mode = 0 Then
            Clipboard.SetText(text)
            SendKeys.Send("^v")
        ElseIf mode = 1 Then
            SendKeys.Send(text)

            'ElseIf ComboBox1.Text = "倉頡轉碼" Then
            '    cj5buffer = getCJ5(text)
            '    For Each c As Char In cj5buffer
            '        SendKeys.Send(c.ToString)
            '    Next
            '    SendKeys.Send(" ")

        ElseIf mode = 2 Then
            cj5buffer = getb5xp(text)
            For Each c As Char In cj5buffer
                SendKeys.Send(c.ToString)
            Next
            SendKeys.Send(" ")
        End If

    End Sub

    Private Sub TrayMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles TrayMenu.Opening
        UpdateMenuCheckState()
    End Sub

    Private Sub ExitAppItem_Click(sender As Object, e As EventArgs) Handles ExitAppItem.Click
        Application.Exit()
    End Sub

    Private Sub ClipboardModeItem_Click(sender As Object, e As EventArgs) Handles ClipboardModeItem.Click
        SaveOutputModeToSetting(0)
    End Sub

    Private Sub DirectOutputModeItem_Click(sender As Object, e As EventArgs) Handles DirectOutputModeItem.Click
        SaveOutputModeToSetting(1)
    End Sub

    Private Sub CangjieConversionModeItem_Click(sender As Object, e As EventArgs) Handles CangjieConversionModeItem.Click
        SaveOutputModeToSetting(2)
    End Sub

    Private Sub SaveOutputModeToSetting(mode As Integer)
        'Save selected mode to settings
        My.Settings.outputMode = mode
        My.Settings.Save()
    End Sub

    Private Sub UpdateMenuCheckState()

        'Reset Item CheckState
        ClipboardModeItem.CheckState = CheckState.Unchecked
        DirectOutputModeItem.CheckState = CheckState.Unchecked
        CangjieConversionModeItem.CheckState = CheckState.Unchecked

        'Change Item CheckState based on the mode which user uses
        Dim mode As Integer = My.Settings.outputMode

        Select Case mode
            Case 0
                ClipboardModeItem.CheckState = CheckState.Checked
            Case 1
                DirectOutputModeItem.CheckState = CheckState.Checked
            Case 2
                CangjieConversionModeItem.CheckState = CheckState.Checked
        End Select

    End Sub

    Private Sub DragWindow_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown, PictureBox9.MouseDown, PictureBox8.MouseDown, PictureBox7.MouseDown, PictureBox6.MouseDown, PictureBox5.MouseDown, PictureBox4.MouseDown, PictureBox3.MouseDown, PictureBox2.MouseDown, PictureBox1.MouseDown, Label11.MouseDown, Label10.MouseDown
        drag = True
        mouseX = Cursor.Position.X - Left
        mouseY = Cursor.Position.Y - Top
    End Sub

    Private Sub DragWindow_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseMove, PictureBox9.MouseMove, PictureBox8.MouseMove, PictureBox7.MouseMove, PictureBox6.MouseMove, PictureBox5.MouseMove, PictureBox4.MouseMove, PictureBox3.MouseMove, PictureBox2.MouseMove, PictureBox1.MouseMove, Label11.MouseMove, Label10.MouseMove
        If drag Then
            Top = Cursor.Position.Y - mouseY
            Left = Cursor.Position.X - mouseX
        End If
    End Sub

    Private Sub DragWindow_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseUp, PictureBox9.MouseUp, PictureBox8.MouseUp, PictureBox7.MouseUp, PictureBox6.MouseUp, PictureBox5.MouseUp, PictureBox4.MouseUp, PictureBox3.MouseUp, PictureBox2.MouseUp, PictureBox1.MouseUp, Label11.MouseUp, Label10.MouseUp
        drag = False
    End Sub

End Class
