﻿Imports MailBee
Imports MailBee.Mime
Imports MailBee.ImapMail
Imports MailBee.Pop3Mail
Imports MailBee.SmtpMail
Imports MailBee.Security
Imports MailBee.Html
Imports MailBee.BounceMail


'Imports System.Net.Mail
Imports System.IO

Module modEmail

    'For Email Maintenance Access
    Dim SMTP_Server As String
    Dim SMTP_PORT As Integer
    Dim SMTP_Sender As String
    Dim SMTP_Username As String
    Dim SMTP_Password As String
    Dim SMTP_CC As String
    Dim SMTP_BC As String

    'For email content
    Dim Sender As String
    Dim Header As String
    Dim Body As String

#Region "Send Email"
    Public Sub sendmail(ByVal Receiver As String, ByVal Name As String, ByVal Attachment As String)
        'Get Email Details will be use in below codes
        Try

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT * FROM `email_maintenance` WHERE `access` = 'Result' AND `status` = 'Active'"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                SMTP_Server = reader(1).ToString
                SMTP_PORT = reader(2).ToString
                SMTP_Sender = reader(3).ToString
                SMTP_Username = reader(4).ToString
                SMTP_Password = reader(5).ToString
                SMTP_CC = reader(6).ToString
                SMTP_BC = reader(7).ToString
            End If
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT * FROM `email_content`"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Sender = reader(1).ToString
                Header = reader(2).ToString
                Body = reader(3).ToString
            End If
            Disconnect()


            Dim mailer As Smtp = New Smtp()

            Dim msg As New MailMessage()

            mailer.SmtpServers.Add(SMTP_Server, SMTP_PORT, 0)
            mailer.To.AsString = Receiver

            If Not SMTP_CC = "" Then
                mailer.Cc.AsString = SMTP_CC
            End If

            mailer.From.AsString = Sender & "<" & SMTP_Sender & ">"
            mailer.Subject = Header

            mailer.SmtpServers(0).SmtpOptions = ExtendedSmtpOptions.NoChunking

            mailer.BodyHtmlText = Body

            'mailer.BodyHtmlText = "<html><body>Good Day!<br/><br/>" &
            '    "This is LabExpress - Las Pinas.<br/><br/>" &
            '    "Please see attached file as reference re: " & StrConv(Name, vbProperCase) & "'s official laboratory test result.<br/><br/>" &
            '    "For clarifications, you may call our Laboratory Department at 1234567 Local 123.<br/><br/>" &
            '    "Thank you and stay safe.</body></html>"

            mailer.AddAttachment(Attachment)

            msg.From.AsString = mailer.From.AsString
            msg.To.AsString = mailer.To.AsString

            If Not SMTP_CC = "" Then
                msg.Cc = mailer.Cc
            End If

            msg.Subject = mailer.Subject
            msg.BodyHtmlText = mailer.BodyHtmlText
            msg.DateSent = Now
            msg.Date = Now

            msg.Attachments.Add(Attachment)

            Dim imp As New Imap()
            imp.Connect(SMTP_Server)
            imp.Login(SMTP_Username, SMTP_Password)

            Try
                mailer.Send()
                imp.UploadMessage(msg, "Sent Items")
            Catch ex As MailBeeImapNegativeResponseException
                imp.UploadMessage(msg, "INBOX.Sent")
            End Try
            imp.Disconnect()
            MessageBox.Show("Email successfully sent.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "System Message", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

    'Function sendmail(ByVal Receiver As String, ByVal Name As String, ByVal Attach As String) As String
    '    Dim Email As New MailMessage()
    '    Try
    '        Connect()
    '        rs.Connection = conn
    '        rs.CommandType = CommandType.Text
    '        rs.CommandText = "SELECT * FROM `email_maintenance` WHERE `access` = 'Result' AND `status` = 'Active'"
    '        reader = rs.ExecuteReader
    '        reader.Read()
    '        If reader.HasRows Then
    '            SMTP_Server = reader(1).ToString
    '            SMTP_PORT = reader(2).ToString
    '            SMTP_Sender = reader(3).ToString
    '            SMTP_Username = reader(4).ToString
    '            SMTP_Password = reader(5).ToString
    '            SMTP_CC = reader(6).ToString
    '            SMTP_BC = reader(7).ToString
    '        End If
    '        Disconnect()

    '        Dim SMTPServer As New SmtpClient
    '        ''For Each Attachment As String In Attachments
    '        ''    Email.Attachments.Add(New Attachment(Attachment))
    '        'Next
    '        'Dim mailattach As String = Attach
    '        'Dim attachment As System.Net.Mail.Attachment
    '        'attachment = New System.Net.Mail.Attachment(mailattach)
    '        'Email.Attachments.Add(attachment)

    '        Email.From = New MailAddress("LabExpress <" & SMTP_Sender & ">")
    '        'For Each Recipient As String In Receiver
    '        Email.Bcc.Add(Receiver)
    '        'Next
    '        'Email.To.Add(Receiver)
    '        Email.Subject = "Laboratory Test Result"
    '        Email.IsBodyHtml = True
    '        Email.Body = "<html><body>Good Day!<br/><br/>" &
    '                "This is LabExpress - Las Pinas.<br/><br/>" &
    '                "Please see attached file as reference re: " & StrConv(Name, vbProperCase) & "'s official laboratory test result.<br/><br/>" &
    '                "For clarifications, you may call our Laboratory Department at 1234567 Local 123.<br/><br/>" &
    '                "Thank you and stay safe.</body></html>"

    '        SMTPServer.Host = SMTP_Server
    '        SMTPServer.Port = SMTP_PORT
    '        SMTPServer.Credentials = New System.Net.NetworkCredential(SMTP_Username, SMTP_Password)
    '        SMTPServer.EnableSsl = True
    '        SMTPServer.Send(Email)
    '        Email.Dispose()
    '        Return "Email to Derek was successfully sent."
    '    Catch ex As SmtpException
    '        Email.Dispose()
    '        Return ex.Message
    '    Catch ex As ArgumentOutOfRangeException
    '        Email.Dispose()
    '        Return ex.Message
    '    Catch Ex As InvalidOperationException
    '        Email.Dispose()
    '        Return Ex.Message
    '    End Try
    '    'MessageBox.Show("Nothing")
    'End Function

    'Public Sub SendMailOneAttachment(ByVal Receiver As String, ByVal Name As String, ByVal Attachment As String, ByVal Sex As String)

    '    Dim myMessage As MailMessage
    '    Dim SmtpMail As New SmtpClient

    '    Try
    '        myMessage = New MailMessage()
    '        With myMessage
    '            .To = sendTo
    '            .From = From
    '            .Subject = Subject
    '            .Body = Body
    '            .BodyFormat = MailFormat.Text
    '            'CAN USER MAILFORMAT.HTML if you prefer

    '            If CC <> "" Then .CC = CC
    '            If BCC <> "" Then .Bcc = ""

    '            If FileExists(AttachmentFile) Then _
    '             .Attachments.Add(AttachmentFile)

    '        End With

    '        If SMTPServer <> "" Then _
    '           SmtpMail.SmtpServer = SMTPServer
    '        SmtpMail.Send(myMessage)

    '    Catch myexp As Exception
    '        Throw myexp
    '    End Try

    'End Sub

    'Public Sub SendMailMultipleAttachments(ByVal From As String,
    'ByVal sendTo As String, ByVal Subject As String,
    'ByVal Body As String,
    'Optional ByVal AttachmentFiles As ArrayList = Nothing,
    'Optional ByVal CC As String = "",
    'Optional ByVal BCC As String = "",
    'Optional ByVal SMTPServer As String = "")

    '    Dim myMessage As MailMessage
    '    Dim i, iCnt As Integer

    '    Try
    '        myMessage = New MailMessage()
    '        With myMessage
    '            .To = sendTo
    '            .From = From
    '            .Subject = Subject
    '            .Body = Body
    '            .BodyFormat = MailFormat.Text
    '            'CAN USER MAILFORMAT.HTML if you prefer

    '            If CC <> "" Then .Cc = CC
    '            If BCC <> "" Then .Bcc = ""

    '            If Not AttachmentFiles Is Nothing Then
    '                iCnt = AttachmentFiles.Count - 1
    '                For i = 0 To iCnt
    '                    If FileExists(AttachmentFiles(i)) Then _
    '                      .Attachments.Add(AttachmentFiles(i))
    '                Next

    '            End If

    '        End With

    '        If SMTPServer <> "" Then _
    '          SmtpMail.SmtpServer = SMTPServer
    '        SmtpMail.Send(myMessage)


    '    Catch myexp As Exception
    '        Throw myexp
    '    End Try
    'End Sub

    Private Function FileExists(ByVal FileFullPath As String) _
     As Boolean
        If Trim(FileFullPath) = "" Then Return False

        Dim f As New IO.FileInfo(FileFullPath)
        Return f.Exists

    End Function

#End Region

#Region "Create Daily Folder"

    Dim folderName As String = DateTime.Today.ToString("yyyyMMdd")
    Dim FolderPath As String = My.Settings.PDFLocation
    Dim CreatedFolder As String = ""

    Public Function CreateFolder(Section As String) As String
        Directory.CreateDirectory(FolderPath & folderName & "\" & Section)
        CreatedFolder = FolderPath & folderName & "\" & Section & "\"
        Return CreatedFolder
    End Function

#End Region
End Module
