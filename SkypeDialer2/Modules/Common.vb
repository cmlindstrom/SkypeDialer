Imports Ceptara.SystemInterface
Imports stdole

Module Common

    ' Common classes
    Public sdUI As UI_EventHandlers
    Public sdSkype As MySkype


#Region "OutlookInterface"

    Public Function GetDefaultContactsListInATable() As Outlook.Table

        Dim strErrorInfo As String = "General Error."
        Dim strRoutine As String = "Common:GetDefaultContactsListInATable"
        Try
            Dim olApp As Outlook.Application = Nothing
            Dim olNS As Outlook.NameSpace = Nothing

            olApp = Globals.ThisAddIn.Application
            olNS = olApp.GetNamespace("MAPI")

            ' Retrieve items into a table
            strErrorInfo = "Retrieving items into an Outlook Table."
            Dim tblContacts As Outlook.Table
            tblContacts = olNS.GetDefaultFolder(Outlook.OlDefaultFolders.olFolderContacts) _
                        .GetTable()

            ' Remove default columns
            tblContacts.Columns.RemoveAll()

            ' Add specific columns
            strErrorInfo = "Adding needed columns of information."
            tblContacts.Columns.Add("EntryID")
            tblContacts.Columns.Add("FullName")
            tblContacts.Columns.Add("Title")
            tblContacts.Columns.Add("FirstName")
            tblContacts.Columns.Add("LastName")
            tblContacts.Columns.Add("Email1Address")
            tblContacts.Columns.Add("Email2Address")
            tblContacts.Columns.Add("Email3Address")
            tblContacts.Columns.Add("LastModificationTime")
            tblContacts.Columns.Add("CarTelephoneNumber")
            tblContacts.Columns.Add("CompanyName")
            tblContacts.Columns.Add("CreationTime")
            tblContacts.Columns.Add("Categories")
            'tblContacts.Columns.Add("Owner") - errors out in German, "The Owner property is unknown."
            tblContacts.Columns.Add("MessageClass")
            tblContacts.Columns.Add("FileAs")

            Dim PhoneNumberFields() As String = {"BusinessTelephoneNumber", _
                                                 "Business2TelephoneNumber", _
                                                 "CallbackTelephoneNumber", _
                                                 "CompanyMainTelephoneNumber", _
                                                 "HomeTelephoneNumber", _
                                                 "Home2TelephoneNumber", _
                                                 "MobileTelephoneNumber", _
                                                 "CarTelephoneNumber", _
                                                 "OtherTelephoneNumber", _
                                                 "RadioTelephoneNumber", _
                                                 "PrimaryTelephoneNumber", _
                                                 "TTYTDDTelephoneNumber"}

            For i = 0 To PhoneNumberFields.Count - 1
                strErrorInfo = "Adding '" & PhoneNumberFields(i) & "' to the table."
                tblContacts.Columns.Add(PhoneNumberFields(i))
            Next

            'Dim PR_POSCONTACT_PROPERTIES As String = "http://schemas.microsoft.com/mapi/string/" & _
            '        "{00020329-0000-0000-C000-000000000046}/POSContactProperties/0x0000001f"
            'tblContacts.Columns.Add(PR_POSCONTACT_PROPERTIES)

            Dim iCnt As Integer = tblContacts.GetRowCount()
            strErrorInfo = "Found " & iCnt.ToString & " records."

            Return tblContacts

        Catch ex As Exception
            TraceLogger(strErrorInfo, ex, strRoutine)
            Errorlogger(ex.Message & " - " & strErrorInfo, Err.Number, strRoutine)
            Return Nothing
        End Try

    End Function

    ''' <summary>
    ''' Returns an Outlook ContactItem given a phone number.
    ''' </summary>
    ''' <param name="PhoneNumber">String: Phone Number to evaluate</param>
    ''' <returns>Outlook.ContactItem if successful otherwise Nothing</returns>
    ''' <remarks></remarks>
    Public Function FindContactByPhoneNumber(ByVal PhoneNumber As String) As Outlook.ContactItem
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "Common:FindContactByPhoneNumber"
        Try
            If String.IsNullOrEmpty(PhoneNumber) Then
                strTrace = "No phone number supplied."
                Throw New Exception("Unable to locate the contact from '" & PhoneNumber & "'.")
            End If

            Dim retContact As Outlook.ContactItem = Nothing

            Dim strEID As String = String.Empty
            Dim strFields() As String = {"BusinessTelephoneNumber", _
                                     "Business2TelephoneNumber", _
                                     "CallbackTelephoneNumber", _
                                     "CompanyMainTelephoneNumber", _
                                     "HomeTelephoneNumber", _
                                     "Home2TelephoneNumber", _
                                     "MobileTelephoneNumber", _
                                     "CarTelephoneNumber", _
                                     "OtherTelephoneNumber", _
                                     "RadioTelephoneNumber", _
                                     "PrimaryTelephoneNumber", _
                                     "TTYTDDTelephoneNumber"}

            strTrace = "Retrieving contacts list from default folder."
            Dim tblContacts As Outlook.Table = GetDefaultContactsListInATable()
            If Not IsNothing(tblContacts) Then
                tblContacts.Sort("FullName")
                Dim bFound As Boolean = False
                Do Until tblContacts.EndOfTable
                    Dim nextRow As Outlook.Row = tblContacts.GetNextRow()
                    If Not IsNothing(nextRow) Then
                        Dim strContactName As String = nextRow("FullName")
                        strTrace = "Evaluating row: " & strContactName
                        For i = 0 To strFields.Count - 1
                            ' Get the specific Outlook property to evaluate
                            Dim strField As String = strFields(i)
                            strTrace = "Evaluating phone property: " & strField

                            Dim strCompareNumber As String = PhoneNumber

                            Dim strNumber As String = nextRow(strField)
                            If Not String.IsNullOrEmpty(strNumber) Then
                                strTrace = "Checking this number: '" & strNumber & "against '" & PhoneNumber & _
                                    "' for " & strContactName & " while evaluating property '" & strField & "'."

                                If PhoneNumber.IndexOf("+1") >= 0 Then strCompareNumber = GetUSTendigitPhoneNumber(PhoneNumber)

                                strCompareNumber = CompressPhoneNumber(strCompareNumber)
                                strNumber = CompressPhoneNumber(strNumber)

                                If strNumber.IndexOf(strCompareNumber) >= 0 Then
                                    strEID = nextRow("EntryID").ToString
                                    bFound = True
                                    Exit Do
                                End If

                            End If

                        Next
                    End If
                Loop

                If bFound Then
                    Dim olApp As Outlook.Application = Globals.ThisAddIn.Application
                    Dim olNS As Outlook.NameSpace = olApp.GetNamespace("MAPI")
                    retContact = CType(olNS.GetItemFromID(strEID), Outlook.ContactItem)
                    olApp = Nothing
                    olNS = Nothing
                End If
            Else
                strTrace = "Table returned as nothing."
                Throw New Exception("Unable to locate the contact from '" & PhoneNumber & "'.")
            End If

            Return retContact
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return Nothing
        End Try
    End Function

    Public Function FindContactByEmailAddress(ByVal strEmailAddress As String) As Outlook.ContactItem
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "Common:FindContactByEmailAddress"
        Try
            Dim retContact As Outlook.ContactItem = Nothing

            Dim strTemp As String = String.Empty

            Dim strMyAddress As String = strEmailAddress
            Dim strAddress As String = String.Empty
            Dim strEID As String = String.Empty
            Dim strField As String = String.Empty
            Dim bFound As Boolean = False

            ' Get the contacts folder in table - much faster than the object model
            strTrace = "Retrieving contacts list from default folder."
            Dim tblContacts As Outlook.Table = GetDefaultContactsListInATable()
            If Not IsNothing(tblContacts) Then
                Dim strFields() As String = {"Email1Address", "Email2Address", "Email3Address"}
                tblContacts.Sort("FullName")
                Do Until tblContacts.EndOfTable
                    ' Check row email addresses for MyAddress
                    strTrace = "Evaluating rows."
                    Dim i As Integer
                    Dim nextRow As Outlook.Row = tblContacts.GetNextRow()
                    If Not IsNothing(nextRow) Then
                        strTemp = nextRow("FullName")
                        strTrace = "Evaluating row: " & strTemp
                        For i = 0 To strFields.Count - 1
                            strField = strFields(i)
                            strTrace = "Evaluating email addresss: " & strField
                            strAddress = nextRow(strField)
                            If IsNothing(strAddress) Then strAddress = String.Empty
                            strTrace = "Checking this address: '" & strAddress & "' for " & strTemp & _
                                " while evaluating " & strField
                            If strAddress.Length > 0 Then
                                ' Something to check    
                                If strAddress.ToUpper = strMyAddress.ToUpper Then
                                    strEID = nextRow("EntryID").ToString
                                    bFound = True
                                    Exit Do
                                End If
                            End If
                            strTrace = "No match found for: '" & strAddress & "'"
                        Next
                    End If
                Loop

                If bFound Then
                    Dim olApp As Outlook.Application = SkypeDialer2.Globals.ThisAddIn.Application
                    Dim olNS As Outlook.NameSpace = olApp.GetNamespace("MAPI")
                    retContact = CType(olNS.GetItemFromID(strEID), Outlook.ContactItem)
                    olApp = Nothing
                    olNS = Nothing
                End If
            End If

            Return retContact
        Catch ex As Exception
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
            Return Nothing
        End Try
    End Function

    ''' <summary>
    ''' Looks for +1 at the beginning of the number, if found, removes country code and removes
    ''' unnecessary characters such as .-(), etc.
    ''' </summary>
    ''' <param name="PhoneNumber">String: Phone Number to evaluate</param>
    ''' <returns>String</returns>
    ''' <remarks></remarks>
    Public Function GetUSTendigitPhoneNumber(ByVal PhoneNumber As String) As String
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "Common:GetUSTenDigitPhoneNumber"
        Try
            If String.IsNullOrEmpty(PhoneNumber) Then
                strTrace = "No phone number was passed to process."
                Throw New Exception("Unable to Get the US 10 digit phone number.")
            End If

            Dim strReturn As String = PhoneNumber

            If PhoneNumber.IndexOf("+1") >= 0 Then
                strReturn = PhoneNumber.Substring(2)
            End If

            Return CompressPhoneNumber(strReturn)
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return PhoneNumber
        End Try
    End Function

    ''' <summary>
    ''' Removes unneeded characters from a phone number string, i.e. +.-()
    ''' </summary>
    ''' <param name="PhoneNumber">String: Phone number to process</param>
    ''' <returns>String: Compressed Phone Number</returns>
    ''' <remarks></remarks>
    Public Function CompressPhoneNumber(ByVal PhoneNumber As String) As String
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "Common:CompressPhoneNumber"
        Try
            If String.IsNullOrWhiteSpace(PhoneNumber) Then
                strTrace = "No phone number was passed to process."
                Throw New Exception("Unable to compress the phone number.")
            End If

            Dim strReturn As String = PhoneNumber

            Dim strReplacements() As String = {"+", "-", ".", "(", ")", " "}
            For i = 0 To strReplacements.Count - 1
                strReturn = strReturn.Replace(strReplacements(i), "")
            Next

            Return strReturn
        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(strTrace, ex, Err, strRoutine)
            Return PhoneNumber
        End Try
    End Function

    Public Function GetImageFromOfficeMSOName(ByVal strName As String) As System.Drawing.Image

        Try
            Dim Converter As New Ceptara.ImageToPictureDispConverter
            Dim imgReturn As System.Drawing.Image = Nothing

            Dim olApp As Outlook.Application = Globals.ThisAddIn.Application
            Dim olExplorer As Outlook.Explorer = olApp.ActiveExplorer
            Dim ipd As IPictureDisp = olExplorer.CommandBars.GetImageMso(strName, 32, 32)
            imgReturn = Converter.GetImageFromIPictureDisp(ipd)

            Return imgReturn
        Catch ex As Exception
            Return Nothing
        End Try

    End Function

#End Region

#Region "Error Handling"

    Public Sub Errorlogger(ByVal Message As String, ByVal ErrorNumber As Integer, ByVal OriginatingRoutine As String)
        Try
            Dim c As New Ceptara.ErrorHandling
            c.ErrorLogger(Message, ErrorNumber, OriginatingRoutine)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub ErrorLogger(ByVal Message As String, _
                           ByVal ErrorException As System.Exception, _
                           ByVal ErrorObject As ErrObject, _
                           ByVal OriginatingRoutine As String)

        Dim c As New Ceptara.ErrorHandling
        c.ErrorLogger(Message & " " & ErrorException.Message & " " & ErrorObject.Description, _
                      ErrorObject.Number, OriginatingRoutine)
    End Sub

    Public Sub TraceLogger(ByVal Action As String, ByVal OriginatingRoutine As String)
        Try
            Dim c As New Ceptara.ErrorHandling
            c.TraceLogger(Action, OriginatingRoutine)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub TraceLogger(ByVal Action As String, ByVal CallingException As System.Exception, ByVal OriginatingRoutine As String)
        Try
            Dim c As New Ceptara.ErrorHandling
            c.TraceLogger(Action, CallingException, OriginatingRoutine)

        Catch ex As Exception

        End Try
    End Sub

    Public Sub SyncLogger(ByVal Action As String, ByVal Message As String, ByVal OriginatingRoutine As String)
        Try
            Dim c As New Ceptara.ErrorHandling
            c.SyncLogger(Action, Message, OriginatingRoutine)

        Catch ex As Exception

        End Try
    End Sub

#End Region


    Public Function IsOrganizerLoaded() As Boolean
        Dim strTrace As String = "General Fault."
        Dim strRoutine As String = "Common:IsOrganizerLoaded"
        Try
            Dim bReturn As Boolean = False

            ' C:\Users\Chris\AppData\Roaming\Ceptara\OutlookFocus

            'Dim strCompanyDir As String = "\Ceptara"
            Dim strAppDir As String = "\OutlookFocus"

            Dim strRoot As String = GetCeptaraApplicationsRootPath()

            Dim strTemp As String = strRoot & strAppDir

            strTrace = "Finding directory: '" & strTemp & "'."
            If DirectoryExists(strTemp) Then
                bReturn = True
            End If

            Return bReturn

        Catch ex As Exception
            TraceLogger(strTrace, ex, strRoutine)
            Errorlogger(ex.Message & " " & strTrace, Err.Number, strRoutine)
            Return False
        End Try
    End Function

End Module
