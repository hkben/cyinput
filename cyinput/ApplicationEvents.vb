Namespace My
    ' MyApplication 可以使用下列事件: 
    ' Startup: 在應用程式啟動時，但尚未建立啟動表單之前引發。
    ' Shutdown:  在所有應用程式表單關閉之後引發。如果應用程式不正常終止，就不會引發此事件。
    ' UnhandledException: 在應用程式發生未處理的例外狀況時引發。
    ' StartupNextInstance: 在啟動單一執行個體應用程式且應用程式已於使用中時引發。
    ' NetworkAvailabilityChanged: 在連接或中斷網路連接時引發。
    Partial Friend Class MyApplication
        Private Sub AppStart(ByVal sender As Object,
            ByVal e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            AddHandler AppDomain.CurrentDomain.AssemblyResolve, AddressOf ResolveAssemblies
        End Sub

        Private Function ResolveAssemblies(sender As Object, e As System.ResolveEventArgs) As Reflection.Assembly
            Dim desiredAssembly = New Reflection.AssemblyName(e.Name)
            If desiredAssembly.Name = "VBHotkeyLib" Then
                Return Reflection.Assembly.Load(My.Resources.VBHotkeyLib) 'replace with your assembly's resource name
            Else
                Return Nothing
            End If
        End Function
    End Class


End Namespace
