Public Class ThisAddIn

    Private Sub ThisAddIn_Startup() Handles Me.Startup

        Try
            sdSkype = New MySkype
            sdUI = New UI_EventHandlers
        Catch ex As Exception

        End Try

    End Sub

    Private Sub ThisAddIn_Shutdown() Handles Me.Shutdown

    End Sub

    Protected Overrides Function CreateRibbonExtensibilityObject() As Microsoft.Office.Core.IRibbonExtensibility
        Return New mnuSkypeDialerRibbons
    End Function

End Class
