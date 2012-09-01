'TODO:  Follow these steps to enable the Ribbon (XML) item:

'1: Copy the following code block into the ThisAddin, ThisWorkbook, or ThisDocument class.

'Protected Overrides Function CreateRibbonExtensibilityObject() As Microsoft.Office.Core.IRibbonExtensibility
'    Return New mnuSkypeDialerRibbons
'End Function

'2. Create callback methods in the "Ribbon Callbacks" region of this class to handle user
'   actions, such as clicking a button. Note: if you have exported this Ribbon from the
'   Ribbon designer, move your code from the event handlers to the callback methods and
'   modify the code to work with the Ribbon extensibility (RibbonX) programming model.

'3. Assign attributes to the control tags in the Ribbon XML file to identify the appropriate callback methods in your code.

'For more information, see the Ribbon XML documentation in the Visual Studio Tools for Office Help.

<Runtime.InteropServices.ComVisible(True)> _
    Public Class mnuSkypeDialerRibbons
    Implements Office.IRibbonExtensibility

    Private ribbon As Office.IRibbonUI

    Public Function GetCustomUI(ByVal ribbonID As String) As String Implements Office.IRibbonExtensibility.GetCustomUI

        Try
            Dim strReturn As String = Nothing

            Select Case ribbonID.ToUpper
                Case "MICROSOFT.OUTLOOK.CONTACT"
                    If IsOrganizerLoaded() Then
                        strReturn = My.Resources.mnuContactRibbonRewrite ' GetResourceText("SkypeDialer2.mnuContactRibbonRewrite.xml")
                    Else
                        strReturn = My.Resources.mnuContactRibbonNewButton ' GetResourceText("SkypeDialer2.mnuContactRibbonNewButton.xml")
                    End If
            End Select

            Return strReturn

        Catch ex As Exception
            Return String.Empty
        End Try

    End Function

#Region "Ribbon Callbacks"
    'Create callback methods here. For more information about adding callback methods, select the Ribbon XML item in Solution Explorer and then press F1.
    Public Sub Ribbon_Load(ByVal ribbonUI As Office.IRibbonUI)
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "mnuSkypeDialerRibbons:Ribbon_Load"
        Try

            Me.ribbon = ribbonUI

        Catch ex As Exception

        End Try
    End Sub

    Public Sub btn_LaunchCallManager(ByVal control As Office.IRibbonControl)
        Dim strTrace As String = "General Fault."
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

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
        Catch ex As Exception
            ErrorLogger(ex.Message & " " & strTrace, Err.Number, "muuSkypeDialerRibbons:btn_LaunchCallManager")
        End Try
    End Sub

    Public Sub btn_SendMail(ByVal control As Office.IRibbonControl)
        Dim strTrace As String = "General Fault."
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

            strTrace = "Checking current inspector item."
            If Not IsNothing(oItem) Then
                strTrace = "Checking current inspector item's type."
                If TypeOf oItem Is Outlook.ContactItem Then
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    strTrace = "Addressing the email."
                    Dim strTo As String = GetToLine(oContact)

                    strTrace = "Check for addresses."
                    If strTo.Length = 0 Then
                        Dim strMsg As String = "No e-mail address found, aborting request."
                        MsgBox(strMsg, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                               My.Application.Info.ProductName & " - Send E-Mail to Contact")
                        Exit Sub
                    End If

                    strTrace = "Creating mail item."
                    Dim oMail As Outlook.MailItem = olApp.CreateItem(Outlook.OlItemType.olMailItem)

                    strTrace = "Assigning mail item properties."
                    oMail.To = strTo

                    strTrace = "Displaying mail item."
                    oMail.Display()

                End If
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message & " " & strTrace, Err.Number, "muuSkypeDialerRibbons:btn_SendMail")
        End Try
    End Sub

    Public Sub btn_ContactWebPage(ByVal control As Office.IRibbonControl)
        Dim strTrace As String = "General Fault."
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

            If Not IsNothing(oItem) Then
                If TypeOf oItem Is Outlook.ContactItem Then
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    If Not IsNothing(oContact.WebPage) Then
                        If oContact.WebPage.Length > 0 Then
                            strTrace = "Going to web page: " & oContact.WebPage
                            Dim c As New Ceptara.NetInterface
                            c.GoToWebPageUsingDefaultBrowser(oContact.WebPage)
                            Exit Sub
                        End If
                    End If

                    strTrace = "No URL Found."
                    Dim strMsg As String = "No web page found, aborting request."
                    MsgBox(strMsg, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                           My.Application.Info.ProductName & " - Go to Web Page")

                End If
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message & " " & strTrace, Err.Number, "muuSkypeDialerRibbons:btn_ContactWebPage")
        End Try

    End Sub

    Public Sub btn_MakeMeeting(ByVal control As Office.IRibbonControl)
        Dim strTrace As String = "General Fault."
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

            If Not IsNothing(oItem) Then
                If TypeOf oItem Is Outlook.ContactItem Then
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    strTrace = "Addressing the email."
                    Dim strTo As String = GetToLine(oContact)

                    strTrace = "Check for addresses."
                    If strTo.Length = 0 Then
                        Dim strMsg As String = "No e-mail address found, aborting request."
                        MsgBox(strMsg, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                               My.Application.Info.ProductName & " - Make Meeting")
                        Exit Sub
                    End If

                    strTrace = "Creating meeting request."
                    Dim oAppt As Outlook.AppointmentItem = _
                        olApp.CreateItem(Outlook.OlItemType.olAppointmentItem)

                    strTrace = "Assigning appointment item properties."
                    oAppt.MeetingStatus = Outlook.OlMeetingStatus.olMeeting
                    oAppt.RequiredAttendees = strTo


                    strTrace = "Displaying meeting request."
                    oAppt.Display()

                End If
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message & " " & strTrace, Err.Number, "muuSkypeDialerRibbons:btn_MakeMeeting")
        End Try

    End Sub

    Public Sub btn_GetMap(ByVal control As Office.IRibbonControl)
        ' http://www.mapquest.com/maps?
        '       city=Mill+Creek&state=WA&
        '       address=15607+30th+Dr+SE&
        '       zipcode=98012-4804&
        '       country=US&
        '       latitude = 47.85596 & longitude = -122.1914 & geocode = ADDRESS
        Dim strTrace As String = "General Fault."
        Try
            Dim olApp As Outlook.Application = Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

            If Not IsNothing(oItem) Then
                If TypeOf oItem Is Outlook.ContactItem Then
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    Dim oAddr As Outlook.OlMailingAddress
                    Select Case control.Tag.ToUpper
                        Case "ADDR_MAILING"
                            oAddr = oContact.SelectedMailingAddress
                        Case "ADDR_BUSINESS"
                            oAddr = Outlook.OlMailingAddress.olBusiness
                        Case "ADDR_HOME"
                            oAddr = Outlook.OlMailingAddress.olHome
                        Case "ADDR_OTHER"
                            oAddr = Outlook.OlMailingAddress.olOther
                    End Select

                    Dim strURL As String = GetMapQueryString(oContact, oAddr)

                    If strURL.Length > 0 Then
                        Dim c As New Ceptara.NetInterface
                        c.GoToWebPageUsingDefaultBrowser(strURL)
                    Else
                        Dim strMsg As String = "No address found, aborting request."
                        MsgBox(strMsg, MsgBoxStyle.Information Or MsgBoxStyle.OkOnly, _
                               My.Application.Info.ProductName & " - Map Address")
                    End If

                End If
            End If
        Catch ex As Exception
            ErrorLogger(ex.Message & " " & strTrace, Err.Number, "muuSkypeDialerRibbons:btn_GetMap")
        End Try
    End Sub

    ''' <summary>
    ''' Send an SMS message to the Active Contact
    ''' </summary>
    ''' <param name="control">Ribbon button</param>
    ''' <remarks></remarks>
    Public Sub btn_SendSMS(ByVal control As Office.IRibbonControl)

        Dim olApp As Outlook.Application = Nothing

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "mnuSkypeDialerRibbons:btn_SendSMS"
        Try
            olApp = Globals.ThisAddIn.Application

            Dim oItem As Object = olApp.ActiveInspector.CurrentItem
            If TypeOf (oItem) Is Outlook.ContactItem Then
                Dim _Contact As Outlook.ContactItem = DirectCast(oItem, Outlook.ContactItem)
                Dim strContactSMSNumber As String = MySkype.GetSkypePhoneNumber(_Contact, _Contact.MobileTelephoneNumber)

                Dim strMsg As String = InputBox("Text Message to send?", "Send SMS - " & My.Application.Info.ProductName)

                If strMsg.Length > 0 Then sdSkype.SendSMSUsingPhoneNumber(strContactSMSNumber, strMsg)

            End If

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        Finally
            olApp = Nothing
        End Try
    End Sub

    Public Sub btn_StartChat(ByVal control As Office.IRibbonControl)
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "mnuSkypeDialerRibbons:btn_StartChat"
        Try
            Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
            Dim oItem As Object = olApp.ActiveInspector.CurrentItem

            If Not IsNothing(oItem) Then
                If TypeOf oItem Is Outlook.ContactItem Then
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)

                    strTrace = "Initiating chat manager with contact: '" & oContact.FullName & "'"

                    Dim frmChatMgr As New frmChatManager
                    frmChatMgr.Contact = oContact
                    frmChatMgr.Text = "Chat Session - " & My.Application.Info.ProductName
                    frmChatMgr.Show()

                End If
            End If
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
        End Try
    End Sub

    Public Function btn_SendEmail_Visible(ByVal control As Office.IRibbonControl) As Boolean
        Return False
    End Function

    Public Function spbtn_Connect_Visible(ByVal control As Office.IRibbonControl) As Boolean
        Return True
    End Function

    ''' <summary>
    ''' Callback for enabling/disabling Send SMS menu button
    ''' </summary>
    ''' <param name="control">RibbonButton</param>
    ''' <returns>Boolean: Returns True if mobile number exists otherwise false</returns>
    ''' <remarks></remarks>
    Public Function btn_SendSMS_Enabled(ByVal control As Office.IRibbonControl) As Boolean
        Dim olApp As Outlook.Application = Nothing

        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "mnuSkypeDialerRibbons:btn_SendSMS_Enabled"
        Try
            Dim bReturn As Boolean = False
            olApp = Globals.ThisAddIn.Application

            Dim oItem As Object = olApp.ActiveInspector.CurrentItem
            If TypeOf (oItem) Is Outlook.ContactItem Then
                Dim _Contact As Outlook.ContactItem = DirectCast(oItem, Outlook.ContactItem)
                If Not String.IsNullOrEmpty(_Contact.MobileTelephoneNumber) Then bReturn = True
            End If
            Return bReturn
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return False
        Finally
            olApp = Nothing
        End Try


    End Function

    Public Function btn_GetMapSingle_Visible(ByVal control As Office.IRibbonControl) As Boolean
        Dim strTrace As String = "General Fault."
        Try
            Dim bReturn As Boolean = False

            ' Global Item Pointer set via UI_EventHandlers
            Dim oItem As Object = UI_EventHandlers.oCurrentOutlookItem

            strTrace = "Checking current inspector item."
            If Not IsNothing(oItem) Then
                strTrace = "Checking for type for current inspector item."
                If TypeOf oItem Is Outlook.ContactItem Then
                    strTrace = "Casting contact item type."
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)
                    strTrace = "Checking for Addresses"
                    If GetContactAddressCount(oContact) <= 1 Then
                        bReturn = True
                    End If
                End If
            End If

            Return bReturn
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, _
                        "mnuSkypeDialerRibbons:spbtn_GetMapMultiple_Visible")
            Return False
        End Try
    End Function

    Public Function spbtn_GetMapMultiple_Visible(ByVal control As Office.IRibbonControl) As Boolean
        Dim strTrace As String = "General Fault."
        Try
            Dim bReturn As Boolean = False

            ' Global Item Pointer set via UI_EventHandlers
            Dim oItem As Object = UI_EventHandlers.oCurrentOutlookItem

            strTrace = "Checking current inspector item."
            If Not IsNothing(oItem) Then
                strTrace = "Checking for type for current inspector item."
                If TypeOf oItem Is Outlook.ContactItem Then
                    strTrace = "Casting contact item type."
                    Dim oContact As Outlook.ContactItem = CType(oItem, Outlook.ContactItem)
                    strTrace = "Checking for Addresses"
                    If GetContactAddressCount(oContact) > 1 Then
                        bReturn = True
                    End If
                End If
            End If

            Return bReturn
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, _
                        "mnuSkypeDialerRibbons:spbtn_GetMapMultiple_Visible")
            Return False
        End Try
    End Function

    Public Sub btn_NoAction(ByVal control As Office.IRibbonControl)
        MsgBox("Not Active.")
    End Sub

#End Region

#Region "Supporting Methods"

    Private Function GetToLine(ByVal oContact As Outlook.ContactItem) As String
        Try
            Dim strTo As String = String.Empty
            If Not IsNothing(oContact.Email1Address) Then
                If oContact.Email1Address.Length > 0 Then
                    strTo = strTo & oContact.Email1DisplayName & ";"
                End If
            End If
            If Not IsNothing(oContact.Email2Address) Then
                If oContact.Email2Address.Length > 0 Then
                    strTo = strTo & oContact.Email2DisplayName & ";"
                End If
            End If
            If Not IsNothing(oContact.Email3Address) Then
                If oContact.Email3Address.Length > 0 Then
                    strTo = strTo & oContact.Email3DisplayName & ";"
                End If
            End If

            Return strTo

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function GetMapQueryString(ByVal oContact As Outlook.ContactItem, _
                                       ByVal oAddress As Outlook.OlMailingAddress) As String
        ' http://www.mapquest.com/maps?
        '       city=Mill+Creek&
        '       state = WA&
        '       address=15607+30th+Dr+SE&
        '       zipcode=98012-4804&
        '       country=US&
        '       latitude = 47.85596 & longitude = -122.1914 & geocode = ADDRESS
        Try
            Dim strReturn As String = String.Empty

            Dim strQuery As String = ""
            Select Case oAddress
                Case Outlook.OlMailingAddress.olBusiness
                    If Not IsNothing(oContact.BusinessAddress) Then
                        If oContact.BusinessAddress.Length > 0 Then
                            ' City
                            If Not IsNothing(oContact.BusinessAddressCity) Then
                                strQuery = strQuery & "city=" & oContact.BusinessAddressCity & "&"
                            End If
                            ' State
                            If Not IsNothing(oContact.BusinessAddressState) Then
                                strQuery = strQuery & "state=" & oContact.BusinessAddressState & "&"
                            End If
                            ' Address
                            If Not IsNothing(oContact.BusinessAddressStreet) Then
                                strQuery = strQuery & "address=" & oContact.BusinessAddressStreet & "&"
                            End If
                            ' Zip
                            If Not IsNothing(oContact.BusinessAddressPostalCode) Then
                                strQuery = strQuery & "zipcode=" & oContact.BusinessAddressPostalCode & "&"
                            End If
                        End If
                    End If
                Case Outlook.OlMailingAddress.olHome
                    If Not IsNothing(oContact.HomeAddress) Then
                        If oContact.HomeAddress.Length > 0 Then
                            ' City
                            If Not IsNothing(oContact.HomeAddressCity) Then
                                strQuery = strQuery & "city=" & oContact.HomeAddressCity & "&"
                            End If
                            ' State
                            If Not IsNothing(oContact.HomeAddressState) Then
                                strQuery = strQuery & "state=" & oContact.HomeAddressState & "&"
                            End If
                            ' Address
                            If Not IsNothing(oContact.HomeAddressStreet) Then
                                strQuery = strQuery & "address=" & oContact.HomeAddressStreet & "&"
                            End If
                            ' Zip
                            If Not IsNothing(oContact.HomeAddressPostalCode) Then
                                strQuery = strQuery & "zipcode=" & oContact.HomeAddressPostalCode & "&"
                            End If
                        End If
                    End If
                Case Outlook.OlMailingAddress.olOther
                    If Not IsNothing(oContact.OtherAddress) Then
                        If oContact.OtherAddress.Length > 0 Then
                            ' City
                            If Not IsNothing(oContact.OtherAddressCity) Then
                                strQuery = strQuery & "city=" & oContact.HomeAddressCity & "&"
                            End If
                            ' State
                            If Not IsNothing(oContact.OtherAddressState) Then
                                strQuery = strQuery & "state=" & oContact.HomeAddressState & "&"
                            End If
                            ' Address
                            If Not IsNothing(oContact.OtherAddressStreet) Then
                                strQuery = strQuery & "address=" & oContact.HomeAddressStreet & "&"
                            End If
                            ' Zip
                            If Not IsNothing(oContact.OtherAddressPostalCode) Then
                                strQuery = strQuery & "zipcode=" & oContact.HomeAddressPostalCode & "&"
                            End If
                        End If
                    End If
                Case Else
                    strQuery = String.Empty
            End Select

            If strQuery.Length > 0 Then
                strReturn = "http://www.mapquest.com/maps?" & strQuery
            End If

            Return strReturn

        Catch ex As Exception
            Return String.Empty
        End Try
    End Function

    Private Function GetContactAddressCount(ByVal oContact As Outlook.ContactItem) As Integer
        Dim strTrace As String = "General Fault."
        Try
            Dim iCount As Integer = 0
            strTrace = "Evaluating the business address."
            If Not IsNothing(oContact.BusinessAddress) Then
                If oContact.BusinessAddress.Length > 0 Then iCount += 1
            End If
            strTrace = "Evaluating the home address."
            If Not IsNothing(oContact.HomeAddress) Then
                If oContact.HomeAddress.Length > 0 Then iCount += 1
            End If
            strTrace = "Evaluating the other address."
            If Not IsNothing(oContact.OtherAddress) Then
                If oContact.OtherAddress.Length > 0 Then iCount += 1
            End If

            Return iCount
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, _
                        "muuSkypeDialerRibbons:GetContactAddressCount")
            Return 0
        End Try
    End Function

#End Region

#Region "Helpers"

    Private Shared Function GetResourceText(ByVal resourceName As String) As String
        Dim asm As Reflection.Assembly = Reflection.Assembly.GetExecutingAssembly()
        Dim resourceNames() As String = asm.GetManifestResourceNames()
        For i As Integer = 0 To resourceNames.Length - 1
            If String.Compare(resourceName, resourceNames(i), StringComparison.OrdinalIgnoreCase) = 0 Then
                Using resourceReader As IO.StreamReader = New IO.StreamReader(asm.GetManifestResourceStream(resourceNames(i)))
                    If resourceReader IsNot Nothing Then
                        Return resourceReader.ReadToEnd()
                    End If
                End Using
            End If
        Next
        Return Nothing
    End Function

#End Region

End Class
