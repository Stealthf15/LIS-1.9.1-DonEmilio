Imports System.Drawing.Printing
Imports DevExpress.Xpo
Imports DevExpress.XtraBars
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraPrinting.BarCode

Public Class frmPhlebotomy

    Public NoCopies As Integer = 1

    Public Sub LoadRecords()
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            Dim SQL As String = "SELECT 
                        `id` AS ID, `status` AS `Status`, `sample_id` AS SampleID, `patient_id` AS PatientID, `patient_name`AS PatientName, 
                        `test` AS Request, `bdate` AS DateOfBirth, `sex` AS Sex, DATE_FORMAT(`date`, '%m/%d/%Y') AS DateReceived, `time` AS TimeReceived, `priority` AS `Priority`,
                        `testtype` AS Section, `sub_section` AS SubSection, `main_id` AS RefID 
                        FROM `tmpWorklist` WHERE (`status` = 'Ordered' OR `status` = 'Rejected' OR `status` = 'Cancelled' OR `status` = 'Warding') 
                        AND (`date` BETWEEN @DateFrom and @DateTo) ORDER BY `id` DESC"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

            command.Parameters.Clear()
            command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
            command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtList.DataSource = myTable

            GridView.Columns("RefID").Visible = False
            GridView.Columns("Section").Visible = False
            GridView.Columns("SubSection").Visible = False

            ' Make the grid read-only. 
            GridView.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridView.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridView.OptionsSelection.MultiSelect = True
            GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridView_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If (e.Column.FieldName = "ID") Or (e.Column.FieldName = "Status") Then
            If view.GetRowCellValue(e.RowHandle, "Status") = "Warding" Then
                e.Appearance.BackColor = Color.Orange
                e.Appearance.BackColor2 = Color.Orange
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Rejected" Then
                e.Appearance.BackColor = Color.Crimson
                e.Appearance.BackColor2 = Color.Crimson
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Cancelled" Then
                e.Appearance.BackColor = Color.Gray
                e.Appearance.BackColor2 = Color.Gray
                e.Appearance.ForeColor = Color.White
            End If
        End If

        If view.GetRowCellValue(e.RowHandle, "Priority").ToString = "STAT" Then
            e.Appearance.ForeColor = Color.DarkRed
        End If

    End Sub

    Public Sub LoadRecordsFilterWard()
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            Dim SQL As String = "SELECT 
                        `id` AS ID, `status` AS `Status`, `sample_id` AS SampleID, `patient_id` AS PatientID, `patient_name`AS PatientName, 
                        `test` AS Request, `bdate` AS DateOfBirth, `sex` AS Sex, DATE_FORMAT(`date`, '%m/%d/%Y') AS DateReceived, `time` AS TimeReceived, `priority` AS `Priority`,
                        `testtype` AS Section, `sub_section` AS SubSection, `main_id` AS RefID 
                        FROM `tmpWorklist` WHERE (`status` = 'Ordered' OR `status` = 'Rejected' OR `status` = 'Cancelled' OR `status` = 'Warding') 
                        AND (`date` BETWEEN @DateFrom AND @DateTo) AND `dept` = @Search ORDER BY `id` DESC"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

            command.Parameters.Clear()
            command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
            command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")
            command.Parameters.AddWithValue("@Search", cboWard.Text)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtList.DataSource = myTable

            ' Make the grid read-only. 
            GridView.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridView.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridView.OptionsSelection.MultiSelect = True
            GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub frmNewOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadRecords()
        LoadWard()
    End Sub

    Private Sub LoadWard()
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT DISTINCT `dept` FROM tmpworklist"
        reader = rs.ExecuteReader
        While reader.Read
            cboWard.Properties.Items.Add(reader(0).ToString)
        End While
        Disconnect()
    End Sub

    Private Sub btnAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCheckin.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                Dim Result As DialogResult = MessageBox.Show("You're about to Check-In Patient " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & "." & vbCrLf & vbCrLf & "Do you want to continue to print Barcode Sticker " & GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")) & "?", "System Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                If (Result = DialogResult.Yes) Then
                    Try
                        frmNoCopiesBC.ShowDialog()

                        PrintBarcode(GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("DateOfBirth")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Sex")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")).ToString, NoCopies, "ROUTINE")

                        GoTo A
                    Catch ex As Exception
                        MessageBox.Show("Error in connection on printer. " + ex.Message, "Barcode Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End Try
                ElseIf (Result = DialogResult.No) Then
                    GoTo A
                ElseIf (Result = DialogResult.Cancel) Then
                    Exit Sub
                End If

A:

                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@mainID", GridView.GetRowCellValue(rowHandle, GridView.Columns("RefID")))
                rs.Parameters.AddWithValue("@SampleID", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")))
                rs.Parameters.AddWithValue("@status", "Checked-In")
                rs.Parameters.AddWithValue("@Section", GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")))
                rs.Parameters.AddWithValue("@SubSection", GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
                rs.Parameters.AddWithValue("@time_checked_in", Now)
                rs.Parameters.AddWithValue("@Comment", txtComment.Text)

                UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
                    & "`status` = @status," _
                    & "`priority` = 'ROUTINE'" _
                    & " WHERE main_id = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )

                UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                    & "`sample_id` = @SampleID," _
                    & "`accession_no` = @mainID" _
                    & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                    )

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `specimen_tracking` WHERE `sample_id` = @SampleID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                        & "`extracted` = @time_checked_in" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `specimen_tracking` (`sample_id`, `extracted`, `section`, `sub_section`) VALUES " _
                        & "(" _
                        & "@SampleID," _
                        & "@time_checked_in," _
                        & "@Section," _
                        & "@SubSection" _
                        & ")"
                        )
                End If
                Disconnect()

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
                End If
                Disconnect()

                'Log activity
                SpecimenActivity("z_logs_specimen", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")), CurrUser, "Checked-In Specimen", "", GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")), GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")), GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
            End If
        Next rowHandle
        LoadRecords()

    End Sub

    Private Sub btnStat_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btnStat.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                Dim Result As DialogResult = MessageBox.Show("You're about to Check-In Patient " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & " as STAT mode." & vbCrLf & vbCrLf & "Do you want to continue to print Barcode Sticker " & GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")) & "?", "System Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                If (Result = DialogResult.Yes) Then
                    Try
                        frmNoCopiesBC.ShowDialog()

                        PrintBarcode(GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("DateOfBirth")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Sex")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")).ToString, NoCopies, "STAT")

                        GoTo A
                    Catch ex As Exception
                        MessageBox.Show("Error in connection on printer. " + ex.Message, "Barcode Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Exit Sub
                    End Try
                ElseIf (Result = DialogResult.No) Then
                    GoTo A
                ElseIf (Result = DialogResult.Cancel) Then
                    Exit Sub
                End If

A:

                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@mainID", GridView.GetRowCellValue(rowHandle, GridView.Columns("RefID")))
                rs.Parameters.AddWithValue("@SampleID", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")))
                rs.Parameters.AddWithValue("@status", "Checked-In")
                rs.Parameters.AddWithValue("@Section", GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")))
                rs.Parameters.AddWithValue("@SubSection", GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
                rs.Parameters.AddWithValue("@time_checked_in", Now)
                rs.Parameters.AddWithValue("@Comment", txtComment.Text)

                UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
                    & "`status` = @status," _
                    & "`priority` = 'STAT'" _
                    & " WHERE main_id = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )

                UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                    & "`sample_id` = @SampleID," _
                    & "`accession_no` = @mainID" _
                    & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                    )

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `specimen_tracking` WHERE `sample_id` = @SampleID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                        & "`extracted` = @time_checked_in" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `specimen_tracking` (`sample_id`, `extracted`, `section`, `sub_section`) VALUES " _
                        & "(" _
                        & "@SampleID," _
                        & "@time_checked_in," _
                        & "@Section," _
                        & "@SubSection" _
                        & ")"
                        )
                End If
                Disconnect()

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
                End If
                Disconnect()

                'Log activity
                SpecimenActivity("z_logs_specimen", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")), CurrUser, "Checked-In Specimen", "", GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")), GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")), GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
            End If
        Next rowHandle
        LoadRecords()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.ItemClick
        LoadRecords()
    End Sub

    Private Sub btnClose_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnClose.ItemClick
        Me.Close()
        Me.Dispose()
    End Sub

    Private Sub btnWarding_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnWarding.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                Dim Result As DialogResult = MessageBox.Show("You're about to Check-In Patient " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & "." & vbCrLf & vbCrLf & "Do you want to continue to print Barcode Sticker " & GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")) & "?", "System Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                If (Result = DialogResult.Yes) Then
                    Try
                        frmNoCopiesBC.ShowDialog()

                        PrintBarcode(GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("DateOfBirth")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Sex")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")).ToString, NoCopies, "ROUTINE")

                    Catch ex As Exception
                        MessageBox.Show("Error in connection on printer. " + ex.Message, "Barcode Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                    GoTo A
                ElseIf (Result = DialogResult.No) Then
                    GoTo A
                ElseIf (Result = DialogResult.Cancel) Then
                    Exit Sub
                End If

A:

                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@mainID", GridView.GetRowCellValue(rowHandle, GridView.Columns("RefID")))
                rs.Parameters.AddWithValue("@SampleID", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")))
                rs.Parameters.AddWithValue("@status", "Warding")
                rs.Parameters.AddWithValue("@Section", GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")))
                rs.Parameters.AddWithValue("@SubSection", GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
                rs.Parameters.AddWithValue("@time_warding", Now)
                rs.Parameters.AddWithValue("@Comment", txtComment.Text)

                UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
                    & "`status` = @status," _
                    & "`priority` = 'ROUTINE'" _
                    & " WHERE main_id = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )

                UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                    & "`sample_id` = @SampleID," _
                    & "`accession_no` = @mainID" _
                    & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                    )

                'UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                'UpdateRecordwthoutMSG("UPDATE `patient_order` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                'UpdateRecordwthoutMSG("UPDATE `lis_order` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `specimen_tracking` WHERE `sample_id` = @SampleID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`warding` = @time_warding" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `specimen_tracking` (`sample_id`, `warding`, `section`, `sub_section`) VALUES " _
                        & "(" _
                        & "@SampleID," _
                        & "@time_warding," _
                        & "@Section," _
                        & "@SubSection" _
                        & ")"
                        )
                End If
                Disconnect()

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
                End If
                Disconnect()

                'Log activity
                SpecimenActivity("z_logs_specimen", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")), CurrUser, "Ward Specimen", "", GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")), GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")), GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
            End If
        Next rowHandle
        LoadRecords()
    End Sub

    Private Sub btnWardingStat_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnWardingStat.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                Dim Result As DialogResult = MessageBox.Show("You're about to Check-In Patient " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & "." & vbCrLf & vbCrLf & "Do you want to continue to print Barcode Sticker " & GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")) & "?", "System Message", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)

                If (Result = DialogResult.Yes) Then
                    Try
                        frmNoCopiesBC.ShowDialog()

                        PrintBarcode(GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("DateOfBirth")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Sex")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")).ToString, NoCopies, "STAT")

                    Catch ex As Exception
                        MessageBox.Show("Error in connection on printer. " + ex.Message, "Barcode Printing Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                    GoTo A
                ElseIf (Result = DialogResult.No) Then
                    GoTo A
                ElseIf (Result = DialogResult.Cancel) Then
                    Exit Sub
                End If

A:

                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@mainID", GridView.GetRowCellValue(rowHandle, GridView.Columns("RefID")))
                rs.Parameters.AddWithValue("@SampleID", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")))
                rs.Parameters.AddWithValue("@status", "Warding")
                rs.Parameters.AddWithValue("@Section", GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")))
                rs.Parameters.AddWithValue("@SubSection", GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
                rs.Parameters.AddWithValue("@time_warding", Now)
                rs.Parameters.AddWithValue("@Comment", txtComment.Text)

                UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
                    & "`status` = @status," _
                    & "`priority` = 'STAT'" _
                    & " WHERE main_id = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )

                UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                    & "`sample_id` = @SampleID," _
                    & "`accession_no` = @mainID" _
                    & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                    )

                'UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                'UpdateRecordwthoutMSG("UPDATE `patient_order` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                'UpdateRecordwthoutMSG("UPDATE `lis_order` SET " _
                '    & "`sample_id` = @SampleID" _
                '    & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @SubSection"
                '    )

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `specimen_tracking` WHERE `sample_id` = @SampleID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`warding` = @time_warding" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `specimen_tracking` (`sample_id`, `warding`, `section`, `sub_section`) VALUES " _
                        & "(" _
                        & "@SampleID," _
                        & "@time_warding," _
                        & "@Section," _
                        & "@SubSection" _
                        & ")"
                        )
                End If
                Disconnect()

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
                End If
                Disconnect()

                'Log activity
                SpecimenActivity("z_logs_specimen", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")), GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")), CurrUser, "Ward Specimen", "", GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")), GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")), GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
            End If
        Next rowHandle
        LoadRecords()
    End Sub

    Private Sub frm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'MainFOrm.aceFecal.Appearance.Normal.BackColor = Color.FromArgb(6, 31, 71)
        MainFOrm.accPhlebotomy.Appearance.Normal.BackColor = Color.FromArgb(16, 110, 190)
        MainFOrm.accPhlebotomy.Appearance.Normal.ForeColor = Color.FromArgb(255, 255, 255)
    End Sub

    Private Sub frm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MainFOrm.lblTitle.Text = ""
        MainFOrm.accPhlebotomy.Appearance.Normal.BackColor = Color.FromArgb(240, 240, 240)
        MainFOrm.accPhlebotomy.Appearance.Normal.ForeColor = Color.FromArgb(27, 41, 62)
        Me.Dispose()
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        LoadRecords()
    End Sub

    Private Sub cboWard_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWard.SelectedIndexChanged
        LoadRecordsFilterWard()
    End Sub

    Private Sub btnDelete_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                If MessageBox.Show("Are you sure you want to reject " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & " order?", "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                    frmRejectOrder.ID = GridView.GetRowCellValue(rowHandle, GridView.Columns("ID"))
                    frmRejectOrder.sID = GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID"))
                    frmRejectOrder.pID = GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID"))
                    frmRejectOrder.pName = GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName"))
                    frmRejectOrder.pTest = GridView.GetRowCellValue(rowHandle, GridView.Columns("Request"))
                    frmRejectOrder.pSection = GridView.GetRowCellValue(rowHandle, GridView.Columns("Section"))
                    frmRejectOrder.pSubSection = GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection"))
                    frmRejectOrder.ShowDialog()
                End If
            End If
        Next rowHandle

        LoadRecords()
    End Sub

    Private Sub btnCancel_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnCancel.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                If MessageBox.Show("Are you sure you want to cancel " & GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")) & " order?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
                    frmCancelOR.ID = GridView.GetRowCellValue(rowHandle, GridView.Columns("ID"))
                    frmCancelOR.sID = GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID"))
                    frmCancelOR.pID = GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID"))
                    frmCancelOR.pName = GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName"))
                    frmCancelOR.pTest = GridView.GetRowCellValue(rowHandle, GridView.Columns("Request"))
                    frmCancelOR.pSection = GridView.GetRowCellValue(rowHandle, GridView.Columns("Section"))
                    frmCancelOR.pSubSection = GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection"))
                    frmCancelOR.ShowDialog()
                End If
            End If
        Next rowHandle

        LoadRecords()
    End Sub

    Private Sub btnSearch_KeyPress(sender As Object, e As KeyPressEventArgs) Handles btnSearch.KeyPress
        If e.KeyChar = Chr(13) Then
            LoadRecordsFilterWard()
        End If
    End Sub

    Private Sub NotifyMe()
        ToastNotificationsManager.ShowNotification(ToastNotificationsManager.Notifications(0))
    End Sub

    Private Sub txtSearch_SelectedIndexChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            If rgSelect.SelectedIndex = 0 Then
                Dim SQL As String = "SELECT 
                        `id` AS ID, `status` AS `Status`, `sample_id` AS SampleID, `patient_id` AS PatientID, `patient_name`AS PatientName, 
                        `test` AS Request, `bdate` AS DateOfBirth, `sex` AS Sex, DATE_FORMAT(`date`, '%m/%d/%Y') AS DateReceived, `time` AS TimeReceived, `priority` AS `Priority`,
                        `testtype` AS Section, `sub_section` AS SubSection, `main_id` AS RefID 
                        FROM `tmpWorklist` WHERE (`sample_id` LIKE '" & txtSearch.Text & "%') AND (`status` = 'Ordered' OR `status` = 'Rejected' OR `status` = 'Cancelled' OR `status` = 'Warding') 
                        AND (`date` BETWEEN @DateFrom and @DateTo) ORDER BY `id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect
            ElseIf rgSelect.SelectedIndex = 1 Then
                Dim SQL As String = "SELECT 
                        `id` AS ID, `status` AS `Status`, `sample_id` AS SampleID, `patient_id` AS PatientID, `patient_name`AS PatientName, 
                        `test` AS Request, `bdate` AS DateOfBirth, `sex` AS Sex, DATE_FORMAT(`date`, '%m/%d/%Y') AS DateReceived, `time` AS TimeReceived, `priority` AS `Priority`,
                        `testtype` AS Section, `sub_section` AS SubSection, `main_id` AS RefID 
                        FROM `tmpWorklist` WHERE (`patient_id` LIKE '" & txtSearch.Text & "%') AND (`status` = 'Ordered' OR `status` = 'Rejected' OR `status` = 'Cancelled' OR `status` = 'Warding') 
                        AND (`date` BETWEEN @DateFrom and @DateTo) ORDER BY `id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect
            ElseIf rgSelect.SelectedIndex = 2 Then
                Dim SQL As String = "SELECT 
                        `id` AS ID, `status` AS `Status`, `sample_id` AS SampleID, `patient_id` AS PatientID, `patient_name`AS PatientName, 
                        `test` AS Request, `bdate` AS DateOfBirth, `sex` AS Sex, DATE_FORMAT(`date`, '%m/%d/%Y') AS DateReceived, `time` AS TimeReceived, `priority` AS `Priority`,
                        `testtype` AS Section, `sub_section` AS SubSection, `main_id` AS RefID 
                        FROM `tmpWorklist` WHERE (`patient_name` LIKE '" & txtSearch.Text & "%') AND (`status` = 'Ordered' OR `status` = 'Rejected' OR `status` = 'Cancelled' OR `status` = 'Warding') 
                        AND (`date` BETWEEN @DateFrom and @DateTo) ORDER BY `id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridView_RowClick(sender As Object, e As RowClickEventArgs) Handles GridView.RowClick
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@RefID", GridView.GetFocusedRowCellValue(GridView.Columns("RefID")))
        rs.Parameters.AddWithValue("@Section", GridView.GetFocusedRowCellValue(GridView.Columns("Section")))
        rs.Parameters.AddWithValue("@SubSection", GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")))

        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT `diagnosis` FROM `patient_remarks` WHERE `sample_id` = @RefID AND `section` = @Section AND `sub_section` = @SubSection"
        reader = rs.ExecuteReader
        reader.Read()
        If reader.HasRows Then
            Me.txtComment.Text = reader(0).ToString
        Else
            Me.txtComment.Text = ""
        End If
        Disconnect()

        Dim SQL As String = "SELECT `sample_id` AS `RefID`, `test_name` AS `TestName`, `testtype` AS `Section`, `sub_section` AS `SubSection` FROM `patient_order` WHERE `sample_id` = '" & GridView.GetFocusedRowCellValue(GridView.Columns("RefID")) & "' AND testtype = '" & GridView.GetFocusedRowCellValue(GridView.Columns("Section")) & "' AND sub_section = '" & GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")) & "'"

        Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

        Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

        Dim myTable As DataTable = New DataTable
        adapter.Fill(myTable)

        dtOrderList.DataSource = myTable

        GridViewList.Columns("RefID").Visible = False
        GridViewList.Columns("Section").Visible = False
        GridViewList.Columns("SubSection").Visible = False

        ' Make the grid read-only. 
        GridViewList.OptionsBehavior.Editable = False
        ' Prevent the focused cell from being highlighted. 
        GridViewList.OptionsSelection.EnableAppearanceFocusedCell = False
        ' Draw a dotted focus rectangle around the entire row. 
        GridViewList.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

    End Sub

    Private Sub btnPrintList_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrintList.ItemClick
        GenerateWardReport("warding_list", dtFrom1, dtTo1, RPTWardingList, "Wardinglist.rdlc", RPTWardingList.ReportViewer1)
    End Sub

    Private Sub btnPrintBC_ItemClick(sender As Object, e As ItemClickEventArgs) Handles btnPrintBC.ItemClick
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                PrintBarcode(GridView.GetRowCellValue(rowHandle, GridView.Columns("Request")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientID")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("PatientName")),
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("DateOfBirth")).ToString.ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Sex")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")).ToString,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")).ToString, NoCopies,
                                     GridView.GetRowCellValue(rowHandle, GridView.Columns("Priority")).ToString)
            End If
        Next
    End Sub

    Private Sub btnSaveComment_Click(sender As Object, e As EventArgs) Handles btnSaveComment.Click
        Dim selectedRows() As Integer = GridView.GetSelectedRows()
        For Each rowHandle As Integer In selectedRows
            If rowHandle >= 0 Then
                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@mainID", GridView.GetRowCellValue(rowHandle, GridView.Columns("RefID")))
                rs.Parameters.AddWithValue("@SampleID", GridView.GetRowCellValue(rowHandle, GridView.Columns("SampleID")))
                rs.Parameters.AddWithValue("@Section", GridView.GetRowCellValue(rowHandle, GridView.Columns("Section")))
                rs.Parameters.AddWithValue("@SubSection", GridView.GetRowCellValue(rowHandle, GridView.Columns("SubSection")))
                rs.Parameters.AddWithValue("@Comment", txtComment.Text)

                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET " _
                        & "`sample_id` = @SampleID," _
                        & "`diagnosis` = @Comment" _
                        & " WHERE sample_id = @mainID AND `section` = @Section AND `sub_section` = @SubSection"
                        )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`sample_id`, `diagnosis`, `section`, `sub_section` ) VALUES (@SampleID, @Comment, @Section, @SubSection)")
                End If
                Disconnect()
            End If
        Next

        MessageBox.Show("Comment has been saved.", "Save Comment", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub


    'Private Sub GridView_FocusedRowChanged(ByVal sender As Object, ByVal e As DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs) Handles GridView.FocusedRowChanged
    '    Dim view As GridView = TryCast(sender, GridView)
    '    view.LayoutChanged()
    'End Sub

    'Private Sub GridView_CalcRowHeight(ByVal sender As Object, ByVal e As RowHeightEventArgs) Handles GridView.CalcRowHeight
    '    Dim view As GridView = TryCast(sender, GridView)
    '    If e.RowHandle = view.FocusedRowHandle Then
    '        e.RowHeight = 50
    '    End If
    'End Sub

End Class