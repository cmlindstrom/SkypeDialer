Imports Office = Microsoft.Office.Core
Imports Outlook = Microsoft.Office.Interop.Outlook
Imports System.Windows.Forms
Imports System.Collections
Imports Microsoft.Office.Tools.Ribbon

Public Class UI_EventHandlers

    ' Outlook interface events
    Private WithEvents oApplication As Outlook.Application
    Private WithEvents oInspectors As Outlook.Inspectors

    Private WithEvents ui_skype As MySkype = sdSkype

    ' Buttons
    Private WithEvents oContactCallViaSkype As Office.CommandBarButton

    ' Internal Fields
    Public Shared oCurrentOutlookItem As Object = Nothing

#Region "Methods"

    Public Sub New()
        Try
            oApplication = SkypeDialer2.Globals.ThisAddIn.Application
            oInspectors = oApplication.Inspectors
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "UI_EventHandlers")
        End Try
    End Sub

#End Region

#Region "Event Handlers"

    Private Sub oApplication_ItemContextMenuDisplay(ByVal CommandBar As Microsoft.Office.Core.CommandBar, _
                                                    ByVal Selection As Microsoft.Office.Interop.Outlook.Selection) Handles oApplication.ItemContextMenuDisplay
        Try
            ' Only process context menu if one item selected
            If Selection.Count = 1 Then

                Dim oItem As Object = Selection.Item(1)
                Dim olClass As Outlook.OlObjectClass = oItem.Class

                Select Case olClass
                    Case Outlook.OlObjectClass.olMail
                    Case Outlook.OlObjectClass.olAppointment
                    Case Outlook.OlObjectClass.olTask
                    Case Outlook.OlObjectClass.olContact
                        ' Assures hook events are renewed
                        CommandBar.Reset()
                        ' Prepare context menu
                        Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)
                        PrepareContactContextMenu(CommandBar, oContact)
                    Case Outlook.OlObjectClass.olJournal
                    Case Outlook.OlObjectClass.olMeetingRequest
                    Case Outlook.OlObjectClass.olNote
                    Case Outlook.OlObjectClass.olPost
                    Case Else
                End Select

            End If
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "UI_EventHandlers:oApplication_ItemContextMenuDisplay")
        End Try
    End Sub

    Private Sub oInspectors_NewInspector(ByVal Inspector As Microsoft.Office.Interop.Outlook.Inspector) Handles oInspectors.NewInspector
        Try
            ' Set a Global variable to help with dynamic ribbon setting
            oCurrentOutlookItem = Inspector.CurrentItem
        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "UI_EventHandlers:oInspector_NewInspector")
        End Try
    End Sub

    ''' <summary>
    ''' Handles any incoming calls handled by the Skype application.
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub ui_skype_IncomingCall(ByVal iCall As SKYPE4COMLib.Call) Handles ui_skype.IncomingCall
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "clsUI_EventHandlers:ui_skype_IncomingCall"
        Try

            ' If Not enabled then Exit sub

            strTrace = "Capture the incoming call object."
            'Dim iCall As SKYPE4COMLib.Call = ui_skype.CurrentCall

            If Not IsNothing(iCall) Then

                strTrace = "Incoming call from - Identity: " & iCall.TargetIdentity & " PSTN #: " & iCall.PstnNumber & _
                    " Skype Handle: " & iCall.PartnerHandle & " Skype Display Name: " & iCall.PartnerDisplayName
                TraceLogger(strTrace, strRoutine)

                Dim oContact As Outlook.ContactItem = sdSkype.FindContactFromCall(iCall)
                If Not IsNothing(oContact) Then
                    strTrace = "Initiating call manager for contact: '" & oContact.FullName & "'"
                    Dim frmCallMgr As New frmCallManager
                    frmCallMgr.oContact = oContact
                    frmCallMgr.Text = oContact.FullName & " - " & My.Application.Info.ProductName
                    frmCallMgr.StartSession() ' to monitor for status changes
                    frmCallMgr.Show()
                End If
            Else
                strTrace = "Instance of the call object was nothing."
                Throw New Exception("Unable to handle the incoming call.")
            End If

        Catch ex As Exception
            TraceLogger(strTrace & " " & ex.StackTrace, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
        End Try
    End Sub

#End Region

#Region "Context Menus"

    Private Sub PrepareContactContextMenu(ByVal ContextMenu As Office.CommandBar, _
                                            ByVal oContact As Outlook.ContactItem)
        Try
            Dim Control As Office.CommandBarControl

            ' Built in 'Call Contact...' Starts the context menu
            Dim iMenuPosition As Integer = 7

            Control = ContextMenu.Controls.Add(Type:=Office.MsoControlType.msoControlButton, _
                                               Before:=iMenuPosition)
            '    Set up control
            With Control
                .Tag = oContact.EntryID
                .Caption = "&Call via Skype..."
                .TooltipText = "Initiate call via Skype Application"
                .Priority = 1
                .Visible = True
                .BeginGroup = False
                .style = Office.MsoButtonStyle.msoButtonIconAndCaption
                '.FaceId = 1100
            End With

            '    Hook the Click event
            oContactCallViaSkype = Control

            ' Increment position
            iMenuPosition += 1

        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "UI_EventHandlers:oApplication_ItemContextMenuDisplay")
        End Try
    End Sub

    Private Sub oContactCallViaSkype_Click(ByVal Ctrl As Microsoft.Office.Core.CommandBarButton, _
                                           ByRef CancelDefault As Boolean) Handles oContactCallViaSkype.Click

        Dim strTrace As String = "General Fault."
        Try
            strTrace = "Checking incoming control."
            If IsNothing(Ctrl) Then
                Exit Sub
            End If

            Dim strTag As String = Ctrl.Tag

            Dim olNS As Outlook.NameSpace = oApplication.GetNamespace("MAPI")

            strTrace = "Getting referenced Outlook object ID: '" & strTag & "'"
            Dim oItem As Object = olNS.GetItemFromID(strTag)
            If Not IsNothing(oItem) Then
                If TypeOf oItem Is Outlook.ContactItem Then

                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    strTrace = "Calling call manager with contact: '" & oContact.FullName & "'"
                    Dim frmCallMgr As New frmCallManager
                    frmCallMgr.oContact = oContact
                    frmCallMgr.Text = oContact.FullName & " - " & My.Application.Info.ProductName

                    frmCallMgr.Show()

                End If
            End If

            olNS = Nothing

        Catch ex As Exception
            ErrorLogger(ex.Message, Err.Number, "UI_EventHandlers:oApplication_ItemContextMenuDisplay")
        End Try
    End Sub

#End Region

End Class
