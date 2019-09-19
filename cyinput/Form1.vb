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
    Private hkkps As VBHotkeys.GlobalHotkey
    'Table storage variables
    Public relatedCharTable As String()
    Dim mappedCharTable As String()
    Dim cj5_mapping As String()
    Dim b5xp_mapping As String()
    Public homograph_mapping As String()
    'Logical Processing Variables
    Dim charset As String = ""
    Dim table As String()
    Dim textArray As New ArrayList
    Dim showingTextArrayIndex As Integer = 0
    Dim useScrollLockKey As Boolean = My.Settings.useScrollLockInstead
    Public lastusedword As String = ""
    Dim cj5buffer As String = ""
    Public isSelecting As Boolean = False

    Dim drag As Boolean
    Dim mouseX As Integer
    Dim mouseY As Integer

    Dim cyinputEnable As Boolean

    'Scaling factor controls
    Dim defaultPictureboxSize As Integer = 30
    Dim currentSize = 0

    'currentSize = 0 means it is in mini state
    'currentSize = 1 means it is in normal state (larger than default)

    'Temp files checking variables
    Dim tempFilelistOriginal As String()
    Dim temppath As String = IO.Path.GetTempPath.ToString & "cyinput\"

    Private Function appendToCharSet(number As String)

        If isSelecting = True Then
            'Special Case "09" 
            If charset = "0" And number = "9" Then
                charset = "09"
                handlePage()
                Return False
            End If

            '-1 = Last Page , 0 = Next Page , 1-9 = SelectedWord
            If number <= 0 Then

                If number = 0 Then
                    showingTextArrayIndex += 9
                ElseIf number = -1 Then
                    showingTextArrayIndex -= 9
                End If

                'Go to Update Text Directly
                drawText()
            ElseIf number <> 0 Or number <> -1 Then
                'If not last or next page , then its selecting word
                SendWord(number)
            End If

            Return False
        End If

        'Default 
        charset = charset & number.ToString
        handlePage()
        Return False
    End Function

    Private Sub LoadTable()
        'Solve CRLF/LF Problem 
        Dim arg() As String = {vbCrLf, vbLf}
        relatedCharTable = My.Resources.related_char.ToString.Split(arg, StringSplitOptions.None)
        mappedCharTable = My.Resources.mapped_char.Split(arg, StringSplitOptions.None)
        cj5_mapping = My.Resources.cj5_map.Split(arg, StringSplitOptions.None)
        b5xp_mapping = My.Resources.b5xp_map.Split(arg, StringSplitOptions.None)
        homograph_mapping = My.Resources.mapped_homograph.Split(arg, StringSplitOptions.None)
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

    Private Sub SendWord(selectedPos As Byte)
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

        isSelecting = False
    End Sub

    Private Sub loadWords(Optional mode As Short = 0)
        isSelecting = True

        Dim wordlist As String = ""

        If mode = 0 Then
            'load from mappedChar
            wordlist = getWordList()
        ElseIf mode = 1 Then
            'load from AssoicatedWords
            wordlist = loadAssoicatedWords()
        End If

        textArray.Clear()
        For Each c As Char In wordlist
            textArray.Add(c.ToString)
        Next
        drawText()
        hidePicturebox()
        Label10.Text = "下頁"
        Label11.Text = "取消"
    End Sub

    Private Sub handlePage()

        If charset = "0" Then
            If lastusedword = "" Then
                'Not typing any thing yet
                loadWords()
                Console.WriteLine("Punct Mode Enabled")
            Else
                'Select related char
                loadWords(1)
                charset = ""
                Console.WriteLine("Selecting Assoicated Word")
            End If
            Return
        End If

        If charset.Length = 1 Then
            paintPictureBoxes(charset)
            showPicturebox()
            Label10.Text = "姓氏"
            Label11.Text = "取消"
            Return
        End If

        If charset.Length = 2 Then
            'All Last Name Mode Ends with 0  , 09 is special case
            If charset.Substring(charset.Length - 1, 1) = "0" Then
                loadWords()
            ElseIf charset = "09" Then
                loadWords()
            Else
                'Typing normally
                paintPictureBoxes(0)
                showPicturebox()
                Label10.Text = "確定"
                Label11.Text = "取消"
            End If
            Return
        End If

        If charset.Length = 3 Then
            loadWords()
            Return
        End If

    End Sub
    Private Sub handleNextWord()
        'After finish typing one word
        charset = ""
        paintPictureBoxes(0)
        showPicturebox()
        textArray.Clear()
        Dim returnassiate = loadAssoicatedWords()
        Dim tmparray() As Char = returnassiate.ToCharArray
        textArray.AddRange(tmparray)
        showingTextArrayIndex = 0
        drawText()
        Label10.Text = "選字"
        Label11.Text = "標點"
    End Sub

    Public Function loadAssoicatedWords()
        Dim target As String = lastusedword
        Dim returnvalue As String = "個能的到資就你這好"
        For Each i As String In relatedCharTable
            If i.Length > 1 Then
                If i.Substring(0, 1).Contains(target) Then
                    i = i.Substring(1, i.Length - 1)
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
                Dim returnmsg = i.Split(",")

                If returnmsg(1).Length = 0 Then
                    Return "*********"
                End If

                'Fill Empty Spot with *
                Dim emptySpot = 9 - (returnmsg(1).Length Mod 9)
                If emptySpot <> 9 Then
                    For index As Integer = 1 To emptySpot
                        returnmsg(1) += "*"
                    Next
                End If

                Return returnmsg(1)
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
        Label7.Text = textArray(sp)
        Label8.Text = textArray(sp + 1)
        Label9.Text = textArray(sp + 2)
        Label4.Text = textArray(sp + 3)
        Label5.Text = textArray(sp + 4)
        Label6.Text = textArray(sp + 5)
        Label1.Text = textArray(sp + 6)
        Label2.Text = textArray(sp + 7)
        Label3.Text = textArray(sp + 8)

        'Update text on largeUI as well, filter the stars icon to nothing (Empty string). Only do filtering on large UI
        largeUI.l1.Text = filterStarToEmptyString(textArray(sp))
        largeUI.l2.Text = filterStarToEmptyString(textArray(sp + 1))
        largeUI.l3.Text = filterStarToEmptyString(textArray(sp + 2))
        largeUI.l4.Text = filterStarToEmptyString(textArray(sp + 3))
        largeUI.l5.Text = filterStarToEmptyString(textArray(sp + 4))
        largeUI.l6.Text = filterStarToEmptyString(textArray(sp + 5))
        largeUI.l7.Text = filterStarToEmptyString(textArray(sp + 6))
        largeUI.l8.Text = filterStarToEmptyString(textArray(sp + 7))
        largeUI.l9.Text = filterStarToEmptyString(textArray(sp + 8))
    End Sub

    Private Function filterStarToEmptyString(inString As String)
        If (inString = "*") Then
            Return ""
        Else
            Return inString
        End If
    End Function

    Private Sub enterPunctMode()
        lastusedword = ""
        charset = "0"
        handlePage()
    End Sub

    Public Sub enterHomophonicMode(keyword As String)
        isSelecting = True
        charset = "h"
        Dim wordlist As String = searchHomophonicWords(keyword)
        textArray.Clear()
        For Each c As Char In wordlist
            textArray.Add(c.ToString)
        Next
        While textArray.Count < 9
            textArray.Add("*")
        End While
        showingTextArrayIndex = 0
        drawText()
        hidePicturebox()
        Label10.Text = "下頁"
        Label11.Text = "取消"
        Console.WriteLine("Homophonic Mode Enabled")
    End Sub

    Public Function searchHomophonicWords(keyword As String)
        For Each searchIndex As String In homograph_mapping
            Dim tmp = searchIndex.Split(" ")
            Dim thisKey = tmp(0)
            If (thisKey = keyword) Then
                Return tmp(1)
            End If
        Next
        Return ""
    End Function

    Private Sub dothandler(Optional reset As Boolean = False)

        If charset.Length = 0 And lastusedword <> "" And isSelecting = False And reset = False Then
            'Punct mode
            enterPunctMode()
            Return
        End If

        'Reset
        isSelecting = False
        charset = ""
        paintPictureBoxes(0)
        showPicturebox()
        textArray.Clear()
        textArray.AddRange({"個", "能", "的", "到", "資", "就", "你", "這", "好"})
        showingTextArrayIndex = 0
        drawText()
        Label10.Text = "標點"
        Label11.Text = "取消"
    End Sub

    Private Sub unzippingToTemp()
        If My.Computer.FileSystem.DirectoryExists(IO.Path.GetTempPath.ToString & "cyinput\") = False Then
            'If cyinput directory do not exists, create one.
            My.Computer.FileSystem.CreateDirectory(temppath)
        Else
            'If the directory already exists, remove it and create a new one. This can prevent any problem left due to Windows 10 Hibernation Mode
            'Yes, Windows are problematic.
            My.Computer.FileSystem.DeleteDirectory(temppath, FileIO.DeleteDirectoryOption.DeleteAllContents)
            My.Computer.FileSystem.CreateDirectory(temppath)
        End If

        'Unzip small UI symbols
        If My.Computer.FileSystem.DirectoryExists(temppath & "img\") = False Then
            Dim b As Byte() = My.Resources.img
            My.Computer.FileSystem.WriteAllBytes(temppath & "tmp.zip", b, False)
            ZipFile.ExtractToDirectory(temppath & "tmp.zip", temppath)
        End If

        'Unzip large UI interfaces
        Dim l As Byte() = My.Resources.lui
        If My.Computer.FileSystem.DirectoryExists(temppath & "lui\") = False Then
            My.Computer.FileSystem.WriteAllBytes(temppath & "lui.zip", l, False)
            ZipFile.ExtractToDirectory(temppath & "lui.zip", temppath)
        End If

        'Build the filelist for compare
        tempFilelistOriginal = recursiveListFile(temppath)
        TempFolderChecker.Enabled = True
    End Sub

    Private Function recursiveListFile(dir As String)
        On Error Resume Next 'To prevent permission error
        Dim fileList As String()
        fileList = IO.Directory.GetFiles(dir, "*.*", IO.SearchOption.AllDirectories)
        Return fileList
    End Function

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        On Error Resume Next
        unzippingToTemp()

        'Create a ding sound to show the process is started.
        If (My.Settings.initSound = True) Then
            My.Computer.Audio.PlaySystemSound(Media.SystemSounds.Beep)
        Else
            停用ToolStripMenuItem.Checked = True
            啟用ToolStripMenuItem.Checked = False
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

        'Added in new shortcut mode using *
        hkkps = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Multiply, Me)

        'Add support for toggling to use scoll lock or keypad divide 
        If (useScrollLockKey) Then
            'Use scrollLock for toggling
            scrollLockListener.Enabled = True
            useScrollLockInstead.Checked = True
        Else
            'Use Keypad / as toggling. Register keypad /
            hkkpq = New VBHotkeys.GlobalHotkey(VBHotkeys.NOMOD, Keys.Divide, Me)
            hkkpq.Register()
        End If



        OnInputEnable()

        'if starting position is set and numberOfMonitors not changed , use position in setting
        If My.Settings.numberOfMonitors = Screen.AllScreens.Length Then
            If Not My.Settings.startPositionTop = 0 And Not My.Settings.startPositionLeft = 0 Then
                Top = My.Settings.startPositionTop
                Left = My.Settings.startPositionLeft
            End If
        End If

        'Check if the size is set to Normal Mode or Default (mini) mode
        If My.Settings.startSize = 1 Then
            'resizeAllElements(1.7, 1, New Size(160, 204), 18, 21)
            currentSize = 1
            MsizeToolStripMenuItem.Checked = False
            NormalSizedToggle.Checked = True
            showLargeUI()
            onLoadHide.Enabled = True
        Else
            MsizeToolStripMenuItem.Checked = True
            NormalSizedToggle.Checked = False
        End If

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
        hkkps.Register()
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
                appendToCharSet(0)
            Case 97
                'Keypad 1
                appendToCharSet(1)
            Case 98
                'Keypad 2
                appendToCharSet(2)
            Case 99
                'Keypad 3
                appendToCharSet(3)
            Case 100
                'Keypad 4
                appendToCharSet(4)
            Case 101
                'Keypad 5
                appendToCharSet(5)
            Case 102
                'Keypad 6
                appendToCharSet(6)
            Case 103
                'Keypad 7
                appendToCharSet(7)
            Case 104
                'Keypad 8
                appendToCharSet(8)
            Case 105
                'Keypad 9
                appendToCharSet(9)
            Case 110
                'Keypad dot
                dothandler()
            Case 107
                '+ pressed
                If isSelecting Then
                    appendToCharSet(0)
                End If
            Case 109
                '- pressed
                If isSelecting Then
                    appendToCharSet(-1)
                End If
            Case 106
                '* pressed
                Return
                'This function is work in progress. Remove the return above to perform testing
                If shortcuts.visable = True Then
                    shortcuts.setVisable(False)
                Else
                    shortcuts.setVisable(True)
                End If
            Case 111
                '/ pressed (Keypad)
                ToggleInputWindow()
        End Select
        largeUI.updateUIbyCharCode(charset)
        'Console.WriteLine("Charset: " & charset & "," & lastusedword & ", Punct: " & punctMode & ", Assoc: " & assoicateMode & ",Homophonic: " & homoMode & ",Keycode" & keycode)

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

        If Not useScrollLockKey Then
            If Not hkkpq.Unregister() Then
                '//Ignore the message and continues.
                'MessageBox.Show("Hide hotkey failed to unregister.")
            End If
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

        If Not hkkps.Unregister() Then
            MessageBox.Show("Hotkey failed to unregister!")
        End If

        cyinputEnable = False

    End Sub

    Private Sub ToggleInputWindow()
        If cyinputEnable Then
            Me.Hide()
            largeUI.Hide()
            OnInputDisable()
        Else
            If (currentSize = 0) Then
                Me.Show()
            Else
                largeUI.Show()
            End If
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
        On Error Resume Next
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

    Private Sub DragWindow_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseDown, PictureBox9.MouseDown, PictureBox8.MouseDown, PictureBox7.MouseDown, PictureBox6.MouseDown, PictureBox5.MouseDown, PictureBox4.MouseDown, PictureBox3.MouseDown, PictureBox2.MouseDown, PictureBox1.MouseDown, Label11.MouseDown, Label10.MouseDown, Label9.MouseDown, Label8.MouseDown, Label7.MouseDown, Label6.MouseDown, Label5.MouseDown, Label4.MouseDown, Label3.MouseDown, Label2.MouseDown, Label1.MouseDown
        drag = True
        mouseX = Cursor.Position.X - Left
        mouseY = Cursor.Position.Y - Top
    End Sub

    Private Sub DragWindow_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseMove, PictureBox9.MouseMove, PictureBox8.MouseMove, PictureBox7.MouseMove, PictureBox6.MouseMove, PictureBox5.MouseMove, PictureBox4.MouseMove, PictureBox3.MouseMove, PictureBox2.MouseMove, PictureBox1.MouseMove, Label11.MouseMove, Label10.MouseMove, Label9.MouseMove, Label8.MouseMove, Label7.MouseMove, Label6.MouseMove, Label5.MouseMove, Label4.MouseMove, Label3.MouseMove, Label2.MouseMove, Label1.MouseMove
        If drag Then
            Top = Cursor.Position.Y - mouseY
            Left = Cursor.Position.X - mouseX
        End If
    End Sub

    Private Sub DragWindow_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs) Handles Me.MouseUp, PictureBox9.MouseUp, PictureBox8.MouseUp, PictureBox7.MouseUp, PictureBox6.MouseUp, PictureBox5.MouseUp, PictureBox4.MouseUp, PictureBox3.MouseUp, PictureBox2.MouseUp, PictureBox1.MouseUp, Label11.MouseUp, Label10.MouseUp, Label9.MouseUp, Label8.MouseUp, Label7.MouseUp, Label6.MouseUp, Label5.MouseUp, Label4.MouseUp, Label3.MouseUp, Label2.MouseUp, Label1.MouseUp
        drag = False

        'Save position to settings
        My.Settings.startPositionTop = Top
        My.Settings.startPositionLeft = Left
        My.Settings.numberOfMonitors = Screen.AllScreens.Length
        My.Settings.Save()

    End Sub

    'Handle startup sound effect settings
    Private Sub 停用ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 停用ToolStripMenuItem.Click
        My.Settings.initSound = False
        啟用ToolStripMenuItem.Checked = False
        停用ToolStripMenuItem.Checked = True
        My.Settings.Save()
    End Sub

    Private Sub 啟用ToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles 啟用ToolStripMenuItem.Click
        My.Settings.initSound = True
        啟用ToolStripMenuItem.Checked = True
        停用ToolStripMenuItem.Checked = False
        My.Settings.Save()
    End Sub

    Private Sub ResetWindowPositionItem_Click(sender As Object, e As EventArgs) Handles ResetWindowPositionItem.Click
        Top = 0
        Left = 0
    End Sub

    Private Sub ToggleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ToggleToolStripMenuItem.Click
        'Handle window toggle from tool strip menu 
        ToggleInputWindow()
    End Sub


    Private Sub NormalToolStripMenuItem_Click(sender As Object, e As EventArgs)
        'Deprecated function for direct zooming
        NormalSizedToggle.Checked = True
        MsizeToolStripMenuItem.Checked = False

        If (currentSize <> 1) Then
            'Scale this up to the size where a normal input method should be
            ' Original size x 1.7 times
            resizeAllElements(1.7, 1, New Size(160, 204), 18, 21)
            currentSize = 1
            My.Settings.startSize = 1
            My.Settings.Save()
        End If

    End Sub

    Private Sub resizeAllElements(scaleFactor As Double, yOffset As Integer, WindowSize As Size, fontSize As Integer, labelSize As Integer)
        'Deprecated Method for making large UI. Keep here for reference only.
        Dim pictureboxSize = defaultPictureboxSize * scaleFactor
        Me.Size = WindowSize
        Me.MaximumSize = WindowSize
        Me.MinimumSize = WindowSize
        UpdateControlSizes(PictureBox1, 0 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox2, 30 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox3, 60 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox4, 0 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox5, 30 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox6, 60 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox7, 0 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox8, 30 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)
        UpdateControlSizes(PictureBox9, 60 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)

        UpdateControlSizes(Label1, 0 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label2, 30 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label3, 60 * scaleFactor, yOffset + 0 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label4, 0 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label5, 30 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label6, 60 * scaleFactor, yOffset + 30 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label7, 0 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label8, 30 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)
        UpdateControlSizes(Label9, 60 * scaleFactor, yOffset + 60 * scaleFactor, pictureboxSize)

        Label1.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label2.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label3.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label4.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label5.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label6.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label7.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label8.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)
        Label9.Font = New Font("MingLiU_HKSCS", fontSize, FontStyle.Bold)

        Label1.TextAlign = ContentAlignment.MiddleCenter
        Label2.TextAlign = ContentAlignment.MiddleCenter
        Label3.TextAlign = ContentAlignment.MiddleCenter
        Label4.TextAlign = ContentAlignment.MiddleCenter
        Label5.TextAlign = ContentAlignment.MiddleCenter
        Label6.TextAlign = ContentAlignment.MiddleCenter
        Label7.TextAlign = ContentAlignment.MiddleCenter
        Label8.TextAlign = ContentAlignment.MiddleCenter
        Label9.TextAlign = ContentAlignment.MiddleCenter

        UpdateControlSizes(Label10, 0, 94 * scaleFactor, 50 * scaleFactor, 29 * scaleFactor)
        UpdateControlSizes(Label11, 44 * scaleFactor, 94 * scaleFactor, 50 * scaleFactor, 29 * scaleFactor)
        Label10.Font = New Font("MingLiU_HKSCS", labelSize)
        Label11.Font = New Font("MingLiU_HKSCS", labelSize)
    End Sub

    Private Sub UpdateControlSizes(element As Control, posx As Integer, posy As Integer, width As Integer, Optional height As Integer = -1)
        If (height = -1) Then
            height = width 'This is a square
        End If
        element.Width = width
        element.Height = width
        element.Left = posx
        element.Top = posy
    End Sub

    Private Sub MiniToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles MsizeToolStripMenuItem.Click
        NormalSizedToggle.Checked = False
        MsizeToolStripMenuItem.Checked = True
        Me.Show()
        'Update this form location to match the larger UI
        Me.Left = largeUI.Left
        Me.Top = largeUI.Top
        largeUI.Hide()
        If (currentSize <> 0) Then
            'Deprecated method for direct zooming
            'resizeAllElements(1, 1, New Size(92, 120), 14.25, 14.25)
            currentSize = 0
            My.Settings.startSize = 0
            My.Settings.Save()
        End If

    End Sub

    Private Sub TrayIcon_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles TrayIcon.MouseDoubleClick
        'Toggle Input by DoubleClick TrayIcon
        ToggleInputWindow()
    End Sub

    Private Sub NormalSizedToggleClick(sender As Object, e As EventArgs) Handles NormalSizedToggle.Click
        'New method for zooming into the input method
        showLargeUI()
        currentSize = 1
        My.Settings.startSize = 1
        My.Settings.Save()
    End Sub

    Private Sub showLargeUI()
        Me.Hide()
        largeUI.Show()
        largeUI.Left = Me.Left
        largeUI.Top = Me.Top
        MsizeToolStripMenuItem.Checked = False
        NormalSizedToggle.Checked = True
    End Sub

    Private Sub onLoadHide_Tick(sender As Object, e As EventArgs) Handles onLoadHide.Tick
        onLoadHide.Enabled = False
        Me.Hide()

    End Sub

    Private Sub useScrollLockIsnteadClick(sender As Object, e As EventArgs) Handles useScrollLockInstead.Click
        If My.Settings.useScrollLockInstead = True Then
            My.Settings.useScrollLockInstead = False
        Else
            My.Settings.useScrollLockInstead = True
        End If
        My.Settings.Save()
        System.Windows.Forms.Application.Restart()
    End Sub

    Dim scrollLockState As Boolean = My.Computer.Keyboard.ScrollLock
    Private Sub scrollLockListener_Tick(sender As Object, e As EventArgs) Handles scrollLockListener.Tick
        'Check if scroll lock state change. If yes, toggle hide / show state
        If My.Computer.Keyboard.ScrollLock <> scrollLockState Then
            scrollLockState = My.Computer.Keyboard.ScrollLock
            ToggleInputWindow()
        End If
    End Sub

    Private Sub TempFolderChecker_Tick(sender As Object, e As EventArgs) Handles TempFolderChecker.Tick
        If checkTempImageAllExists() = False Then
            'Something missing. Restart the application
            Application.Restart()
        End If
    End Sub

    Private Function checkTempImageAllExists()
        Dim currentTempList As String() = recursiveListFile(temppath)
        'Compare the currentTempList to the original temp list. If the list is difference (aka some file is missing, return false)
        Dim allFileExists = True
        For Each file As String In tempFilelistOriginal
            If Array.IndexOf(currentTempList, file) = -1 Then
                allFileExists = False
            End If
        Next
        Return allFileExists
    End Function
End Class
