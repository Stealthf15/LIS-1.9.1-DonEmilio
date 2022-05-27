Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Xml
Imports System.Text.RegularExpressions
Imports System.Drawing.Printing
Imports DevExpress.XtraPrinting.BarCode
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmChemWorklist

    Public mainID As String = ""
    Public Section As String = ""
    Public SubSection As String = ""
    Public Patient_ID As String = ""
    Public Validation As String = ""
    Dim arrImage() As Byte
    Dim bDate As String
    Dim age As String
    Dim list_Test As String
    Dim patientGender As String

    'For Worklist
    Dim SequenceNo, Status, SampleID, PatientID, PatientName, PatientDoB, PatientAge, PatientSex, Test, ResultDate, ResultTime As String

    'For Result
    Dim ResultName, ResultValue, ResultFlag, ResultOrder, ResultCode, ResultUnit, ResultUnitConv, ResultID, ResultGroup As String

    Private PrintDocType As String = "Barcode"
    Private StrPrinterName As String = My.Settings.BCPrinterName

    Public Sub LoadRecords()
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            Dim SQL As String = "SELECT
                                    `tmpWorklist`.`id` AS SequenceNo,
                                    `tmpWorklist`.`status` AS Status,
                                    `tmpWorklist`.`sample_id` AS SampleID,
                                    `tmpWorklist`.`patient_id` AS PatientID,
                                    `tmpWorklist`.`patient_name` AS PatientName, 
                                    `tmpWorklist`.`test` AS Request,
                                    DATE_FORMAT(`tmpWorklist`.`date`, '%m/%d/%Y') AS DateReceived,
                                    `tmpWorklist`.`time` AS TimeReceived,
                                    DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
                                    CONCAT(T1.fname, ' ', T1.mname, ' ', T1.lname, ', ', T1.designation) AS PerformedBy,
                                    CONCAT(T2.fname, ' ', T2.mname, ' ', T2.lname, ', ', T2.designation) AS ReleasedBy,
                                    `tmpWorklist`.`physician` AS Physician,
                                    `tmpWorklist`.`bdate` AS DateOfBirth,
                                    `tmpWorklist`.`sex` AS Sex,
                                    `tmpWorklist`.`age` AS Age,
                                    `tmpWorklist`.`dept` AS RoomWard,
                                    `tmpWorklist`.`testtype` AS Section,
                                    `tmpWorklist`.`sub_section` AS SubSection,
                                    `tmpWorklist`.`main_id` AS RefID,
                                    `tmpWorklist`.`patient_type` AS PatientType,
                                    patient_info.address AS Address,
                                    patient_info.contact_no AS ContactNo,
                                    patient_info.civil_status AS CivilStatus,
                                    additional_info.accession_no AS AccessionNo,
                                    additional_info.or_no AS ORNo,
                                    additional_info.cs_no AS ChargeSlip,
                                    patient_remarks.remarks AS Remarks,
                                    patient_remarks.diagnosis AS Diagnosis,
                                    email_details.email_address as EmailAddress
                                FROM `tmpWorklist` 
                                    Left JOIN `specimen_tracking` ON `specimen_tracking`.`sample_id` = `tmpWorklist`.`main_id` And specimen_tracking.section = tmpworklist.testtype And specimen_tracking.sub_section = tmpworklist.sub_section
                                    Left JOIN `patient_info` ON`patient_info`.`patient_id` = `tmpWorklist`.`patient_id`
                                    Left JOIN `additional_info` ON `additional_info`.`sample_id` = `tmpWorklist`.`main_id` And additional_info.section = tmpworklist.testtype And additional_info.sub_section = tmpworklist.sub_section
                                    Left JOIN `patient_remarks` ON `patient_remarks`.`sample_id` = `tmpWorklist`.`main_id` And patient_remarks.section = tmpworklist.testtype And patient_remarks.sub_section = tmpworklist.sub_section
                                    Left JOIN `email_details` ON `email_details`.`sample_id` = `tmpWorklist`.`main_id` And email_details.section = tmpworklist.testtype And email_details.sub_section = tmpworklist.sub_section
                                    LEFT JOIN `medtech` T1 ON `T1`.`id` = `tmpWorklist`.`medtech`
                                    LEFT JOIN `medtech_verificator` T2 ON `T2`.`id` = `tmpWorklist`.`verified_by`
                                WHERE(`tmpWorklist`.`status` = 'Result Received' OR `tmpWorklist`.`status` = 'Validated' OR `tmpWorklist`.`status` = 'Processing') 
                                    And (`tmpWorklist`.`testtype` = `specimen_tracking`.`section`)
                                    And (`tmpWorklist`.`sub_section` = `specimen_tracking`.`sub_section`)
                                    And (`tmpWorklist`.`testtype` = 'Chemistry')
                                    And (`tmpWorklist`.`location` = @Location)
                                    ORDER BY `tmpWorklist`.`main_id` DESC"

            'Dim SQL As String = "worklist"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)
            command.CommandType = CommandType.Text

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@Location", cboLocation.Text)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtList.DataSource = myTable

            'GridView.Columns("PerformedBy").Visible = False
            'GridView.Columns("ReleasedBy").Visible = False
            GridView.Columns("PatientType").Visible = False
            GridView.Columns("RefID").Visible = False
            GridView.Columns("Section").Visible = False
            GridView.Columns("SubSection").Visible = False
            GridView.Columns("Age").Visible = False
            GridView.Columns("Address").Visible = False
            GridView.Columns("ContactNo").Visible = False
            GridView.Columns("CivilStatus").Visible = False
            GridView.Columns("AccessionNo").Visible = False
            GridView.Columns("ORNo").Visible = False
            GridView.Columns("ChargeSlip").Visible = False
            GridView.Columns("Remarks").Visible = False
            GridView.Columns("Diagnosis").Visible = False
            GridView.Columns("EmailAddress").Visible = False

            ' Make the grid read-only. 
            GridView.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridView.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridView.OptionsSelection.MultiSelect = True
            GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

            GridView.Columns("SequenceNo").Width = 100
            GridView.Columns("Status").Width = 150
            GridView.Columns("SampleID").Width = 100
            GridView.Columns("PatientID").Width = 100
            GridView.Columns("PatientName").Width = 250
            GridView.Columns("Request").Width = 100
            GridView.Columns("DateOfBirth").Width = 100
            GridView.Columns("Sex").Width = 100
            GridView.Columns("RoomWard").Width = 150
            GridView.Columns("Physician").Width = 250
            GridView.Columns("PerformedBy").Width = 250
            GridView.Columns("ReleasedBy").Width = 250
            GridView.Columns("DateReceived").Width = 100
            GridView.Columns("TimeReceived").Width = 100
            GridView.Columns("DateCheckedIn").Width = 200

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridView_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If (e.Column.FieldName = "SequenceNo") Or (e.Column.FieldName = "Status") Then
            If view.GetRowCellValue(e.RowHandle, "Status") = "Processing" Then
                e.Appearance.BackColor = Color.Gold
                e.Appearance.BackColor2 = Color.Gold
                e.Appearance.ForeColor = Color.Black
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Result Received" Then
                e.Appearance.BackColor = Color.LightGreen
                e.Appearance.BackColor2 = Color.LightGreen
                e.Appearance.ForeColor = Color.Black
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Validated" Then
                e.Appearance.BackColor = Color.Green
                e.Appearance.BackColor2 = Color.Green
                e.Appearance.ForeColor = Color.Black
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Printed" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            Else
                e.Appearance.BackColor = Color.Gray
                e.Appearance.BackColor2 = Color.Gray
                e.Appearance.ForeColor = Color.White
            End If
        End If
    End Sub

    Public Sub LoadRecordsCompleted()
        Try
            GridCompleted.Columns.Clear()
            GridCompleted.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridCompleted.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            Dim SQL As String = "Select
							    `order`.`id` AS SequenceNo,
							    `order`.`status` AS Status,
							    `order`.`sample_id` AS SampleID,
							    `order`.`patient_id` AS PatientID,
							    `order`.`patient_name` AS PatientName, 
							    `order`.`test` AS Request,
							    DATE_FORMAT(`order`.`date`, '%m/%d/%Y') AS DateReceived,
							    `order`.`time` AS TimeReceived,
                                DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
                                DATE_FORMAT(STR_TO_DATE( `order`.`dt_released`, '%Y-%m-%d %l:%i:%S %p' ), '%m/%d/%Y %r') AS DateReleased,
                                CONCAT(T1.fname, ' ', T1.mname, ' ', T1.lname, ', ', T1.designation) AS PerformedBy,
                                CONCAT(T2.fname, ' ', T2.mname, ' ', T2.lname, ', ', T2.designation) AS ReleasedBy,
							    `order`.`physician` AS Physician,
							    `order`.`bdate` AS DateOfBirth,
							    `order`.`sex` AS Sex,
							    `order`.`age` AS Age,
							    `order`.`dept` AS RoomWard,
							    `order`.`testtype` AS Section,
							    `order`.`sub_section` AS SubSection,
							    `order`.`main_id` AS RefID,
							    `order`.`patient_type` AS PatientType,
                                patient_info.address As Address,
                                patient_info.contact_no AS ContactNo,
                                patient_info.civil_status As CivilStatus,
                                additional_info.accession_no AS AccessionNo,
                                additional_info.or_no As ORNo,
                                additional_info.cs_no AS ChargeSlip,
                                patient_remarks.remarks As Remarks,
                                patient_remarks.diagnosis AS Diagnosis,
                                email_details.email_address as EmailAddress
                            FROM `order` 
							    Left Join `specimen_tracking` ON `specimen_tracking`.`sample_id` = `order`.`main_id` And specimen_tracking.section = order.testtype And specimen_tracking.sub_section = order.sub_section
                                Left Join `patient_info` ON`patient_info`.`patient_id` = `order`.`patient_id`
                                Left Join `additional_info` ON `additional_info`.`sample_id` = `order`.`main_id` And additional_info.section = order.testtype And additional_info.sub_section = order.sub_section
                                Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `order`.`main_id` And patient_remarks.section = order.testtype And patient_remarks.sub_section = order.sub_section
                                Left Join `email_details` ON `email_details`.`sample_id` = `order`.`main_id` And email_details.section = order.testtype And email_details.sub_section = order.sub_section
                                Left Join `medtech` T1 ON `T1`.`id` = `order`.`medtech`
                                Left Join `medtech_verificator` T2 ON `T2`.`id` = `order`.`verified_by`
                            WHERE(`order`.`status` = 'Printed' OR `order`.`status` = 'Validated' OR `order`.`status` = 'Released') 
						        And (`order`.`testtype` = `specimen_tracking`.`section`)
						        And (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
						        And (`order`.`testtype` = 'Chemistry')
						        And (`order`.`location` = @Location)
						        And (DATE(DATE_FORMAT(`order`.`dt_released`, '%Y-%m-%d')) BETWEEN @DateFrom AND @DateTo)
						    ORDER BY `order`.`main_id` DESC"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

            command.Parameters.Clear()
            command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
            command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")
            command.Parameters.AddWithValue("@Location", cboLocation1.Text)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)
            command.CommandType = CommandType.Text

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtCompleted.DataSource = myTable

            'GridCompleted.Columns("PerformedBy").Visible = False
            'GridCompleted.Columns("ReleasedBy").Visible = False
            GridCompleted.Columns("PatientType").Visible = False
            GridCompleted.Columns("RefID").Visible = False
            GridCompleted.Columns("Section").Visible = False
            GridCompleted.Columns("SubSection").Visible = False
            GridCompleted.Columns("Address").Visible = False
            GridCompleted.Columns("ContactNo").Visible = False
            GridCompleted.Columns("CivilStatus").Visible = False
            GridCompleted.Columns("AccessionNo").Visible = False
            GridCompleted.Columns("ORNo").Visible = False
            GridCompleted.Columns("ChargeSlip").Visible = False
            GridCompleted.Columns("Remarks").Visible = False
            GridCompleted.Columns("Diagnosis").Visible = False
            GridCompleted.Columns("EmailAddress").Visible = False

            'GridCompleted.Columns("Age").Visible = False


            ' Make the grid read-only. 
            GridCompleted.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridCompleted.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridCompleted.OptionsSelection.MultiSelect = True
            GridCompleted.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

            GridCompleted.Columns("SequenceNo").Width = 100
            GridCompleted.Columns("Status").Width = 150
            GridCompleted.Columns("SampleID").Width = 100
            GridCompleted.Columns("PatientID").Width = 100
            GridCompleted.Columns("PatientName").Width = 250
            GridCompleted.Columns("Request").Width = 100
            GridCompleted.Columns("DateOfBirth").Width = 100
            GridCompleted.Columns("Sex").Width = 100
            GridCompleted.Columns("RoomWard").Width = 150
            GridCompleted.Columns("Physician").Width = 250
            GridCompleted.Columns("PerformedBy").Width = 250
            GridCompleted.Columns("ReleasedBy").Width = 250
            GridCompleted.Columns("DateReceived").Width = 100
            GridCompleted.Columns("TimeReceived").Width = 100
            GridCompleted.Columns("DateCheckedIn").Width = 200
            GridCompleted.Columns("DateReleased").Width = 200
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridComplete_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridCompleted.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If (e.Column.FieldName = "SequenceNo") Or (e.Column.FieldName = "Status") Then
            If view.GetRowCellValue(e.RowHandle, "Status") = "Processing" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Released" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Validated" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "Status") = "Printed" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            Else
                e.Appearance.BackColor = Color.Gray
                e.Appearance.BackColor2 = Color.Gray
                e.Appearance.ForeColor = Color.White
            End If
        End If
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        Try
            Dim SQL As String

            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            If rgSelect.SelectedIndex = 0 Then
                'Search SampleID
                SQL = "SELECT
		                `tmpWorklist`.`id` AS SequenceNo,
		                `tmpWorklist`.`status` AS Status,
		                `tmpWorklist`.`sample_id` AS SampleID,
		                `tmpWorklist`.`patient_id` AS PatientID,
		                `tmpWorklist`.`patient_name` AS PatientName, 
		                `tmpWorklist`.`test` AS Request,
		                DATE_FORMAT(`tmpWorklist`.`date`, '%m/%d/%Y') AS DateReceived,
		                `tmpWorklist`.`time` AS TimeReceived,
                        DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
		                `tmpWorklist`.`medtech` AS PerformedBy,
		                `tmpWorklist`.`verified_by` AS ReleasedBy,
		                `tmpWorklist`.`physician` AS Physician,
		                `tmpWorklist`.`bdate` AS DateOfBirth,
		                `tmpWorklist`.`sex` AS Sex,
		                `tmpWorklist`.`age` AS Age,
		                `tmpWorklist`.`dept` AS RoomWard,
		                `tmpWorklist`.`testtype` AS Section,
		                `tmpWorklist`.`sub_section` AS SubSection,
		                `tmpWorklist`.`main_id` AS RefID,
		                `tmpWorklist`.`patient_type` AS PatientType,
                        patient_info.address AS Address,
                        patient_info.contact_no AS ContactNo,
                        patient_info.civil_status AS CivilStatus,
                        additional_info.accession_no AS AccessionNo,
                        additional_info.or_no AS ORNo,
                        additional_info.cs_no AS ChargeSlip,
                        patient_remarks.remarks AS Remarks,
                        patient_remarks.diagnosis AS Diagnosis,
                        email_details.email_address as EmailAddress
                        FROM `tmpWorklist` 
		                Left Join `specimen_tracking` ON `specimen_tracking`.`sample_id` = `tmpWorklist`.`main_id` And specimen_tracking.section = tmpworklist.testtype And specimen_tracking.sub_section = tmpworklist.sub_section
                        Left Join `patient_info` ON`patient_info`.`patient_id` = `tmpWorklist`.`patient_id`
                        Left Join `additional_info` ON `additional_info`.`sample_id` = `tmpWorklist`.`main_id` And additional_info.section = tmpworklist.testtype And additional_info.sub_section = tmpworklist.sub_section
                        Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `tmpWorklist`.`main_id` And patient_remarks.section = tmpworklist.testtype And patient_remarks.sub_section = tmpworklist.sub_section
                        Left Join `email_details` ON `email_details`.`sample_id` = `tmpWorklist`.`main_id` And email_details.section = tmpworklist.testtype And email_details.sub_section = tmpworklist.sub_section
                        WHERE(`tmpWorklist`.`status` = 'Result Received' OR `tmpWorklist`.`status` = 'Validated' OR `tmpWorklist`.`status` = 'Processing') 
                        AND (`tmpworklist`.`sample_id` LIKE '" & txtSearch.Text & "%')
		                And (`tmpWorklist`.`testtype` = `specimen_tracking`.`section`)
		                And (`tmpWorklist`.`sub_section` = `specimen_tracking`.`sub_section`)
		                And (`tmpWorklist`.`testtype` = 'Chemistry')
		                And (`tmpWorklist`.`location` = @Location)
		                ORDER BY `tmpWorklist`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.AddWithValue("@Location", cboLocation.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                'GridView.Columns("PerformedBy").Visible = False
                'GridView.Columns("ReleasedBy").Visible = False
                GridView.Columns("PatientType").Visible = False
                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False
                GridView.Columns("Age").Visible = False
                GridView.Columns("Address").Visible = False
                GridView.Columns("ContactNo").Visible = False
                GridView.Columns("CivilStatus").Visible = False
                GridView.Columns("AccessionNo").Visible = False
                GridView.Columns("ORNo").Visible = False
                GridView.Columns("ChargeSlip").Visible = False
                GridView.Columns("Remarks").Visible = False
                GridView.Columns("Diagnosis").Visible = False
                GridView.Columns("EmailAddress").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridView.Columns("SequenceNo").Width = 100
                GridView.Columns("Status").Width = 150
                GridView.Columns("SampleID").Width = 100
                GridView.Columns("PatientID").Width = 100
                GridView.Columns("PatientName").Width = 250
                GridView.Columns("Request").Width = 100
                GridView.Columns("DateOfBirth").Width = 100
                GridView.Columns("Sex").Width = 100
                GridView.Columns("RoomWard").Width = 150
                GridView.Columns("Physician").Width = 250
                GridView.Columns("PerformedBy").Width = 250
                GridView.Columns("ReleasedBy").Width = 250
                GridView.Columns("DateReceived").Width = 100
                GridView.Columns("TimeReceived").Width = 100
                GridView.Columns("DateCheckedIn").Width = 200
            ElseIf rgSelect.SelectedIndex = 1 Then
                'Search PatientID
                SQL = "SELECT
		                `tmpWorklist`.`id` AS SequenceNo,
		                `tmpWorklist`.`status` AS Status,
		                `tmpWorklist`.`sample_id` AS SampleID,
		                `tmpWorklist`.`patient_id` AS PatientID,
		                `tmpWorklist`.`patient_name` AS PatientName, 
		                `tmpWorklist`.`test` AS Request,
		                DATE_FORMAT(`tmpWorklist`.`date`, '%m/%d/%Y') AS DateReceived,
		                `tmpWorklist`.`time` AS TimeReceived,
                        DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
		                `tmpWorklist`.`medtech` AS PerformedBy,
		                `tmpWorklist`.`verified_by` AS ReleasedBy,
		                `tmpWorklist`.`physician` AS Physician,
		                `tmpWorklist`.`bdate` AS DateOfBirth,
		                `tmpWorklist`.`sex` AS Sex,
		                `tmpWorklist`.`age` AS Age,
		                `tmpWorklist`.`dept` AS RoomWard,
		                `tmpWorklist`.`testtype` AS Section,
		                `tmpWorklist`.`sub_section` AS SubSection,
		                `tmpWorklist`.`main_id` AS RefID,
		                `tmpWorklist`.`patient_type` AS PatientType,
                        patient_info.address AS Address,
                        patient_info.contact_no AS ContactNo,
                        patient_info.civil_status AS CivilStatus,
                        additional_info.accession_no AS AccessionNo,
                        additional_info.or_no AS ORNo,
                        additional_info.cs_no AS ChargeSlip,
                        patient_remarks.remarks AS Remarks,
                        patient_remarks.diagnosis AS Diagnosis,
                        email_details.email_address as EmailAddress
                        FROM `tmpWorklist` 
		                Left Join `specimen_tracking` ON`specimen_tracking`.`sample_id` = `tmpWorklist`.`main_id` And specimen_tracking.section = tmpworklist.testtype And specimen_tracking.sub_section = tmpworklist.sub_section
                        Left Join `patient_info` ON`patient_info`.`patient_id` = `tmpWorklist`.`patient_id`
                        Left Join `additional_info` ON `additional_info`.`sample_id` = `tmpWorklist`.`main_id` And additional_info.section = tmpworklist.testtype And additional_info.sub_section = tmpworklist.sub_section
                        Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `tmpWorklist`.`main_id` And patient_remarks.section = tmpworklist.testtype And patient_remarks.sub_section = tmpworklist.sub_section
                        Left Join `email_details` ON `email_details`.`sample_id` = `tmpWorklist`.`main_id` And email_details.section = tmpworklist.testtype And email_details.sub_section = tmpworklist.sub_section
                        WHERE(`tmpWorklist`.`status` = 'Result Received' OR `tmpWorklist`.`status` = 'Validated' OR `tmpWorklist`.`status` = 'Processing') 
                        AND (`tmpworklist`.`patient_id` LIKE '" & txtSearch.Text & "%')
		                And (`tmpWorklist`.`testtype` = `specimen_tracking`.`section`)
		                And (`tmpWorklist`.`sub_section` = `specimen_tracking`.`sub_section`)
		                And (`tmpWorklist`.`testtype` = 'Chemistry')
		                And (`tmpWorklist`.`location` = @Location)
		                ORDER BY `tmpWorklist`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.AddWithValue("@Location", cboLocation.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                'GridView.Columns("PerformedBy").Visible = False
                'GridView.Columns("ReleasedBy").Visible = False
                GridView.Columns("PatientType").Visible = False
                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False
                GridView.Columns("Age").Visible = False
                GridView.Columns("Address").Visible = False
                GridView.Columns("ContactNo").Visible = False
                GridView.Columns("CivilStatus").Visible = False
                GridView.Columns("AccessionNo").Visible = False
                GridView.Columns("ORNo").Visible = False
                GridView.Columns("ChargeSlip").Visible = False
                GridView.Columns("Remarks").Visible = False
                GridView.Columns("Diagnosis").Visible = False
                GridView.Columns("EmailAddress").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridView.Columns("SequenceNo").Width = 100
                GridView.Columns("Status").Width = 150
                GridView.Columns("SampleID").Width = 100
                GridView.Columns("PatientID").Width = 100
                GridView.Columns("PatientName").Width = 250
                GridView.Columns("Request").Width = 100
                GridView.Columns("DateOfBirth").Width = 100
                GridView.Columns("Sex").Width = 100
                GridView.Columns("RoomWard").Width = 150
                GridView.Columns("Physician").Width = 250
                GridView.Columns("PerformedBy").Width = 250
                GridView.Columns("ReleasedBy").Width = 250
                GridView.Columns("DateReceived").Width = 100
                GridView.Columns("TimeReceived").Width = 100
                GridView.Columns("DateCheckedIn").Width = 200
            ElseIf rgSelect.SelectedIndex = 2 Then
                'Search Patient Name
                SQL = "SELECT
		                `tmpWorklist`.`id` AS SequenceNo,
		                `tmpWorklist`.`status` AS Status,
		                `tmpWorklist`.`sample_id` AS SampleID,
		                `tmpWorklist`.`patient_id` AS PatientID,
		                `tmpWorklist`.`patient_name` AS PatientName, 
		                `tmpWorklist`.`test` AS Request,
		                DATE_FORMAT(`tmpWorklist`.`date`, '%m/%d/%Y') AS DateReceived,
		                `tmpWorklist`.`time` AS TimeReceived,
                        DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
		                `tmpWorklist`.`medtech` AS PerformedBy,
		                `tmpWorklist`.`verified_by` AS ReleasedBy,
		                `tmpWorklist`.`physician` AS Physician,
		                `tmpWorklist`.`bdate` AS DateOfBirth,
		                `tmpWorklist`.`sex` AS Sex,
		                `tmpWorklist`.`age` AS Age,
		                `tmpWorklist`.`dept` AS RoomWard,
		                `tmpWorklist`.`testtype` AS Section,
		                `tmpWorklist`.`sub_section` AS SubSection,
		                `tmpWorklist`.`main_id` AS RefID,
		                `tmpWorklist`.`patient_type` AS PatientType,
                        patient_info.address AS Address,
                        patient_info.contact_no AS ContactNo,
                        patient_info.civil_status AS CivilStatus,
                        additional_info.accession_no AS AccessionNo,
                        additional_info.or_no AS ORNo,
                        additional_info.cs_no AS ChargeSlip,
                        patient_remarks.remarks AS Remarks,
                        patient_remarks.diagnosis AS Diagnosis,
                        email_details.email_address as EmailAddress
                        FROM `tmpWorklist` 
		                Left Join `specimen_tracking` ON`specimen_tracking`.`sample_id` = `tmpWorklist`.`main_id` And specimen_tracking.section = tmpworklist.testtype And specimen_tracking.sub_section = tmpworklist.sub_section
                        Left Join `patient_info` ON`patient_info`.`patient_id` = `tmpWorklist`.`patient_id`
                        Left Join `additional_info` ON `additional_info`.`sample_id` = `tmpWorklist`.`main_id` And additional_info.section = tmpworklist.testtype And additional_info.sub_section = tmpworklist.sub_section
                        Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `tmpWorklist`.`main_id` And patient_remarks.section = tmpworklist.testtype And patient_remarks.sub_section = tmpworklist.sub_section
                        Left Join `email_details` ON `email_details`.`sample_id` = `tmpWorklist`.`main_id` And email_details.section = tmpworklist.testtype And email_details.sub_section = tmpworklist.sub_section
                        WHERE(`tmpWorklist`.`status` = 'Result Received' OR `tmpWorklist`.`status` = 'Validated' OR `tmpWorklist`.`status` = 'Processing') 
                        AND (`tmpworklist`.`patient_name` LIKE '" & txtSearch.Text & "%')
		                And (`tmpWorklist`.`testtype` = `specimen_tracking`.`section`)
		                And (`tmpWorklist`.`sub_section` = `specimen_tracking`.`sub_section`)
		                And (`tmpWorklist`.`testtype` = 'Chemistry')
		                And (`tmpWorklist`.`location` = @Location)
		                ORDER BY `tmpWorklist`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.AddWithValue("@Location", cboLocation.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtList.DataSource = myTable

                'GridView.Columns("PerformedBy").Visible = False
                'GridView.Columns("ReleasedBy").Visible = False
                GridView.Columns("PatientType").Visible = False
                GridView.Columns("RefID").Visible = False
                GridView.Columns("Section").Visible = False
                GridView.Columns("SubSection").Visible = False
                GridView.Columns("Age").Visible = False
                GridView.Columns("Address").Visible = False
                GridView.Columns("ContactNo").Visible = False
                GridView.Columns("CivilStatus").Visible = False
                GridView.Columns("AccessionNo").Visible = False
                GridView.Columns("ORNo").Visible = False
                GridView.Columns("ChargeSlip").Visible = False
                GridView.Columns("Remarks").Visible = False
                GridView.Columns("Diagnosis").Visible = False
                GridView.Columns("EmailAddress").Visible = False

                ' Make the grid read-only. 
                GridView.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridView.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridView.OptionsSelection.MultiSelect = True
                GridView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridView.Columns("SequenceNo").Width = 100
                GridView.Columns("Status").Width = 150
                GridView.Columns("SampleID").Width = 100
                GridView.Columns("PatientID").Width = 100
                GridView.Columns("PatientName").Width = 250
                GridView.Columns("Request").Width = 100
                GridView.Columns("DateOfBirth").Width = 100
                GridView.Columns("Sex").Width = 100
                GridView.Columns("RoomWard").Width = 150
                GridView.Columns("Physician").Width = 250
                GridView.Columns("PerformedBy").Width = 250
                GridView.Columns("ReleasedBy").Width = 250
                GridView.Columns("DateReceived").Width = 100
                GridView.Columns("TimeReceived").Width = 100
                GridView.Columns("DateCheckedIn").Width = 200
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub txtSearch1_TextChanged(sender As Object, e As EventArgs) Handles txtSearch1.TextChanged
        Try
            GridCompleted.Columns.Clear()
            GridCompleted.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridCompleted.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            If rgSelect1.SelectedIndex = 0 Then
                Dim SQL As String = "Select
							`order`.`id` AS SequenceNo,
							`order`.`status` AS Status,
							`order`.`sample_id` AS SampleID,
							`order`.`patient_id` AS PatientID,
							`order`.`patient_name` AS PatientName, 
							`order`.`test` AS Request,
							DATE_FORMAT(`order`.`date`, '%m/%d/%Y') AS DateReceived,
							`order`.`time` AS TimeReceived,
                            DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
                            DATE_FORMAT(STR_TO_DATE( `order`.`dt_released`, '%Y-%m-%d %l:%i:%S %p' ), '%m/%d/%Y %r') AS DateReleased,
                            CONCAT(T1.fname, ' ', T1.mname, ' ', T1.lname, ', ', T1.designation) AS PerformedBy,
                            CONCAT(T2.fname, ' ', T2.mname, ' ', T2.lname, ', ', T2.designation) AS ReleasedBy,
							`order`.`physician` AS Physician,
							`order`.`bdate` AS DateOfBirth,
							`order`.`sex` AS Sex,
							`order`.`age` AS Age,
							`order`.`dept` AS RoomWard,
							`order`.`testtype` AS Section,
							`order`.`sub_section` AS SubSection,
							`order`.`main_id` AS RefID,
							`order`.`patient_type` AS PatientType,
                            patient_info.address As Address,
                            patient_info.contact_no AS ContactNo,
                            patient_info.civil_status As CivilStatus,
                            additional_info.accession_no AS AccessionNo,
                            additional_info.or_no As ORNo,
                            additional_info.cs_no AS ChargeSlip,
                            patient_remarks.remarks As Remarks,
                            patient_remarks.diagnosis AS Diagnosis,
                            email_details.email_address as EmailAddress
                        FROM `order` 
							Left Join `specimen_tracking` ON `specimen_tracking`.`sample_id` = `order`.`main_id` And specimen_tracking.section = `order`.testtype And specimen_tracking.sub_section = `order`.sub_section
                            Left Join `patient_info` ON `patient_info`.`patient_id` = `order`.`patient_id`
                            Left Join `additional_info` ON `additional_info`.`sample_id` = `order`.`main_id` And additional_info.section = `order`.testtype And additional_info.sub_section = `order`.sub_section
                            Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `order`.`main_id` And patient_remarks.section = `order`.testtype And patient_remarks.sub_section = `order`.sub_section
                            Left Join `email_details` ON `email_details`.`sample_id` = `order`.`main_id` And email_details.section = `order`.testtype And email_details.sub_section = `order`.sub_section
                            Left Join `medtech` T1 ON `T1`.`id` = `order`.`medtech`
                            Left Join `medtech_verificator` T2 ON `T2`.`id` = `order`.`verified_by`
                        WHERE(`order`.`status` = 'Printed' OR `order`.`status` = 'Validated' OR `order`.`status` = 'Released') 
                        AND (`order`.`sample_id` LIKE '" & txtSearch1.Text & "%')
						And (`order`.`testtype` = `specimen_tracking`.`section`)
						And (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
						And (`order`.`testtype` = 'Chemistry')
						And (`order`.`location` = @Location)
						And (DATE(DATE_FORMAT(`order`.`dt_released`, '%Y-%m-%d')) BETWEEN @DateFrom AND @DateTo)
						ORDER BY `order`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")
                command.Parameters.AddWithValue("@Location", cboLocation1.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtCompleted.DataSource = myTable

                'GridCompleted.Columns("PerformedBy").Visible = False
                'GridCompleted.Columns("ReleasedBy").Visible = False
                GridCompleted.Columns("PatientType").Visible = False
                GridCompleted.Columns("RefID").Visible = False
                GridCompleted.Columns("Section").Visible = False
                GridCompleted.Columns("SubSection").Visible = False
                GridCompleted.Columns("Address").Visible = False
                GridCompleted.Columns("ContactNo").Visible = False
                GridCompleted.Columns("CivilStatus").Visible = False
                GridCompleted.Columns("AccessionNo").Visible = False
                GridCompleted.Columns("ORNo").Visible = False
                GridCompleted.Columns("ChargeSlip").Visible = False
                GridCompleted.Columns("Remarks").Visible = False
                GridCompleted.Columns("Diagnosis").Visible = False
                GridCompleted.Columns("EmailAddress").Visible = False

                'GridCompleted.Columns("Age").Visible = False


                ' Make the grid read-only. 
                GridCompleted.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridCompleted.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridCompleted.OptionsSelection.MultiSelect = True
                GridCompleted.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridCompleted.Columns("SequenceNo").Width = 100
                GridCompleted.Columns("Status").Width = 150
                GridCompleted.Columns("SampleID").Width = 100
                GridCompleted.Columns("PatientID").Width = 100
                GridCompleted.Columns("PatientName").Width = 250
                GridCompleted.Columns("Request").Width = 100
                GridCompleted.Columns("DateOfBirth").Width = 100
                GridCompleted.Columns("Sex").Width = 100
                GridCompleted.Columns("RoomWard").Width = 150
                GridCompleted.Columns("Physician").Width = 250
                GridCompleted.Columns("PerformedBy").Width = 250
                GridCompleted.Columns("ReleasedBy").Width = 250
                GridCompleted.Columns("DateReceived").Width = 100
                GridCompleted.Columns("TimeReceived").Width = 100
                GridCompleted.Columns("DateCheckedIn").Width = 200
                GridCompleted.Columns("DateReleased").Width = 200
            ElseIf rgSelect1.SelectedIndex = 1 Then
                Dim SQL As String = "Select
							`order`.`id` AS SequenceNo,
							`order`.`status` AS Status,
							`order`.`sample_id` AS SampleID,
							`order`.`patient_id` AS PatientID,
							`order`.`patient_name` AS PatientName, 
							`order`.`test` AS Request,
							DATE_FORMAT(`order`.`date`, '%m/%d/%Y') AS DateReceived,
							`order`.`time` AS TimeReceived,
                            DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
                            DATE_FORMAT(STR_TO_DATE( `order`.`dt_released`, '%Y-%m-%d %l:%i:%S %p' ), '%m/%d/%Y %r') AS DateReleased,
                            CONCAT(T1.fname, ' ', T1.mname, ' ', T1.lname, ', ', T1.designation) AS PerformedBy,
                            CONCAT(T2.fname, ' ', T2.mname, ' ', T2.lname, ', ', T2.designation) AS ReleasedBy,
							`order`.`physician` AS Physician,
							`order`.`bdate` AS DateOfBirth,
							`order`.`sex` AS Sex,
							`order`.`age` AS Age,
							`order`.`dept` AS RoomWard,
							`order`.`testtype` AS Section,
							`order`.`sub_section` AS SubSection,
							`order`.`main_id` AS RefID,
							`order`.`patient_type` AS PatientType,
                            patient_info.address As Address,
                            patient_info.contact_no AS ContactNo,
                            patient_info.civil_status As CivilStatus,
                            additional_info.accession_no AS AccessionNo,
                            additional_info.or_no As ORNo,
                            additional_info.cs_no AS ChargeSlip,
                            patient_remarks.remarks As Remarks,
                            patient_remarks.diagnosis AS Diagnosis,
                            email_details.email_address as EmailAddress
                        FROM `order` 
							Left Join `specimen_tracking` ON `specimen_tracking`.`sample_id` = `order`.`main_id` And specimen_tracking.section = `order`.testtype And specimen_tracking.sub_section = `order`.sub_section
                            Left Join `patient_info` ON `patient_info`.`patient_id` = `order`.`patient_id`
                            Left Join `additional_info` ON `additional_info`.`sample_id` = `order`.`main_id` And additional_info.section = `order`.testtype And additional_info.sub_section = `order`.sub_section
                            Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `order`.`main_id` And patient_remarks.section = `order`.testtype And patient_remarks.sub_section = `order`.sub_section
                            Left Join `email_details` ON `email_details`.`sample_id` = `order`.`main_id` And email_details.section = `order`.testtype And email_details.sub_section = `order`.sub_section
                            Left Join `medtech` T1 ON `T1`.`id` = `order`.`medtech`
                            Left Join `medtech_verificator` T2 ON `T2`.`id` = `order`.`verified_by`
                        WHERE(`order`.`status` = 'Printed' OR `order`.`status` = 'Validated' OR `order`.`status` = 'Released') 
                        AND (`order`.`patient_id` LIKE '" & txtSearch1.Text & "%')
						And (`order`.`testtype` = `specimen_tracking`.`section`)
						And (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
						And (`order`.`testtype` = 'Chemistry')
						And (`order`.`location` = @Location)
						And (DATE(DATE_FORMAT(`order`.`dt_released`, '%Y-%m-%d')) BETWEEN @DateFrom AND @DateTo)
						ORDER BY `order`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")
                command.Parameters.AddWithValue("@Location", cboLocation1.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtCompleted.DataSource = myTable

                'GridCompleted.Columns("PerformedBy").Visible = False
                'GridCompleted.Columns("ReleasedBy").Visible = False
                GridCompleted.Columns("PatientType").Visible = False
                GridCompleted.Columns("RefID").Visible = False
                GridCompleted.Columns("Section").Visible = False
                GridCompleted.Columns("SubSection").Visible = False
                GridCompleted.Columns("Address").Visible = False
                GridCompleted.Columns("ContactNo").Visible = False
                GridCompleted.Columns("CivilStatus").Visible = False
                GridCompleted.Columns("AccessionNo").Visible = False
                GridCompleted.Columns("ORNo").Visible = False
                GridCompleted.Columns("ChargeSlip").Visible = False
                GridCompleted.Columns("Remarks").Visible = False
                GridCompleted.Columns("Diagnosis").Visible = False
                GridCompleted.Columns("EmailAddress").Visible = False

                'GridCompleted.Columns("Age").Visible = False


                ' Make the grid read-only. 
                GridCompleted.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridCompleted.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridCompleted.OptionsSelection.MultiSelect = True
                GridCompleted.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridCompleted.Columns("SequenceNo").Width = 100
                GridCompleted.Columns("Status").Width = 150
                GridCompleted.Columns("SampleID").Width = 100
                GridCompleted.Columns("PatientID").Width = 100
                GridCompleted.Columns("PatientName").Width = 250
                GridCompleted.Columns("Request").Width = 100
                GridCompleted.Columns("DateOfBirth").Width = 100
                GridCompleted.Columns("Sex").Width = 100
                GridCompleted.Columns("RoomWard").Width = 150
                GridCompleted.Columns("Physician").Width = 250
                GridCompleted.Columns("PerformedBy").Width = 250
                GridCompleted.Columns("ReleasedBy").Width = 250
                GridCompleted.Columns("DateReceived").Width = 100
                GridCompleted.Columns("TimeReceived").Width = 100
                GridCompleted.Columns("DateCheckedIn").Width = 200
                GridCompleted.Columns("DateReleased").Width = 200
            ElseIf rgSelect1.SelectedIndex = 2 Then
                Dim SQL As String = "Select
							`order`.`id` AS SequenceNo,
							`order`.`status` AS Status,
							`order`.`sample_id` AS SampleID,
							`order`.`patient_id` AS PatientID,
							`order`.`patient_name` AS PatientName, 
							`order`.`test` AS Request,
							DATE_FORMAT(`order`.`date`, '%m/%d/%Y') AS DateReceived,
							`order`.`time` AS TimeReceived,
                            DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %l:%i:%S %p') AS DateCheckedIn,
                            DATE_FORMAT(STR_TO_DATE( `order`.`dt_released`, '%Y-%m-%d %l:%i:%S %p' ), '%m/%d/%Y %r') AS DateReleased,
                            CONCAT(T1.fname, ' ', T1.mname, ' ', T1.lname, ', ', T1.designation) AS PerformedBy,
                            CONCAT(T2.fname, ' ', T2.mname, ' ', T2.lname, ', ', T2.designation) AS ReleasedBy,
							`order`.`physician` AS Physician,
							`order`.`bdate` AS DateOfBirth,
							`order`.`sex` AS Sex,
							`order`.`age` AS Age,
							`order`.`dept` AS RoomWard,
							`order`.`testtype` AS Section,
							`order`.`sub_section` AS SubSection,
							`order`.`main_id` AS RefID,
							`order`.`patient_type` AS PatientType,
                            patient_info.address As Address,
                            patient_info.contact_no AS ContactNo,
                            patient_info.civil_status As CivilStatus,
                            additional_info.accession_no AS AccessionNo,
                            additional_info.or_no As ORNo,
                            additional_info.cs_no AS ChargeSlip,
                            patient_remarks.remarks As Remarks,
                            patient_remarks.diagnosis AS Diagnosis,
                            email_details.email_address as EmailAddress
                        FROM `order` 
							Left Join `specimen_tracking` ON `specimen_tracking`.`sample_id` = `order`.`main_id` And specimen_tracking.section = `order`.testtype And specimen_tracking.sub_section = `order`.sub_section
                            Left Join `patient_info` ON `patient_info`.`patient_id` = `order`.`patient_id`
                            Left Join `additional_info` ON `additional_info`.`sample_id` = `order`.`main_id` And additional_info.section = `order`.testtype And additional_info.sub_section = `order`.sub_section
                            Left Join `patient_remarks` ON `patient_remarks`.`sample_id` = `order`.`main_id` And patient_remarks.section = `order`.testtype And patient_remarks.sub_section = `order`.sub_section
                            Left Join `email_details` ON `email_details`.`sample_id` = `order`.`main_id` And email_details.section = `order`.testtype And email_details.sub_section = `order`.sub_section
                            Left Join `medtech` T1 ON `T1`.`id` = `order`.`medtech`
                            Left Join `medtech_verificator` T2 ON `T2`.`id` = `order`.`verified_by`
                        WHERE(`order`.`status` = 'Printed' OR `order`.`status` = 'Validated' OR `order`.`status` = 'Released') 
                        AND (`order`.`patient_name` LIKE '" & txtSearch1.Text & "%')
						And (`order`.`testtype` = `specimen_tracking`.`section`)
						And (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
						And (`order`.`testtype` = 'Chemistry')
						And (`order`.`location` = @Location)
						And (DATE(DATE_FORMAT(`order`.`dt_released`, '%Y-%m-%d')) BETWEEN @DateFrom AND @DateTo)
						ORDER BY `order`.`main_id` DESC"

                Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

                command.Parameters.Clear()
                command.Parameters.Add("@DateFrom", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtFrom1.Value, "yyyy-MM-dd")
                command.Parameters.Add("@DateTo", MySql.Data.MySqlClient.MySqlDbType.DateTime).Value = Format(dtTo1.Value, "yyyy-MM-dd")
                command.Parameters.AddWithValue("@Location", cboLocation1.Text)

                Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

                Dim myTable As DataTable = New DataTable
                adapter.Fill(myTable)

                dtCompleted.DataSource = myTable

                'GridCompleted.Columns("PerformedBy").Visible = False
                'GridCompleted.Columns("ReleasedBy").Visible = False
                GridCompleted.Columns("PatientType").Visible = False
                GridCompleted.Columns("RefID").Visible = False
                GridCompleted.Columns("Section").Visible = False
                GridCompleted.Columns("SubSection").Visible = False
                GridCompleted.Columns("Address").Visible = False
                GridCompleted.Columns("ContactNo").Visible = False
                GridCompleted.Columns("CivilStatus").Visible = False
                GridCompleted.Columns("AccessionNo").Visible = False
                GridCompleted.Columns("ORNo").Visible = False
                GridCompleted.Columns("ChargeSlip").Visible = False
                GridCompleted.Columns("Remarks").Visible = False
                GridCompleted.Columns("Diagnosis").Visible = False
                GridCompleted.Columns("EmailAddress").Visible = False

                'GridCompleted.Columns("Age").Visible = False


                ' Make the grid read-only. 
                GridCompleted.OptionsBehavior.Editable = False
                ' Prevent the focused cell from being highlighted. 
                GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
                ' Draw a dotted focus rectangle around the entire row. 
                GridCompleted.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

                GridCompleted.OptionsSelection.MultiSelect = True
                GridCompleted.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

                GridCompleted.Columns("SequenceNo").Width = 100
                GridCompleted.Columns("Status").Width = 150
                GridCompleted.Columns("SampleID").Width = 100
                GridCompleted.Columns("PatientID").Width = 100
                GridCompleted.Columns("PatientName").Width = 250
                GridCompleted.Columns("Request").Width = 100
                GridCompleted.Columns("DateOfBirth").Width = 100
                GridCompleted.Columns("Sex").Width = 100
                GridCompleted.Columns("RoomWard").Width = 150
                GridCompleted.Columns("Physician").Width = 250
                GridCompleted.Columns("PerformedBy").Width = 250
                GridCompleted.Columns("ReleasedBy").Width = 250
                GridCompleted.Columns("DateReceived").Width = 100
                GridCompleted.Columns("TimeReceived").Width = 100
                GridCompleted.Columns("DateCheckedIn").Width = 200
                GridCompleted.Columns("DateReleased").Width = 200
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub DisableControls()

        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT * FROM `user_permission` WHERE `user_id` = '" & CurrEmail & "'"
        reader = rs.ExecuteReader
        While reader.Read
            If reader(3) = 0 Then
                For Each ctrl In Me.Bar.Manager.Items
                    If ctrl.Caption = reader(2).ToString Then
                        ctrl.Enabled = False
                    End If
                Next
            Else
                For Each ctrl In Me.Bar.Manager.Items
                    If ctrl.Caption = reader(2).ToString Then
                        ctrl.Enabled = True
                    End If
                Next
            End If
        End While

        Disconnect()
    End Sub

    Private Sub frmNewOrder_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub frmNewOrder_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cboLocation.Properties.Items.Clear()
        Me.cboLocation1.Properties.Items.Clear()

        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT `location` FROM `location` ORDER BY `id`"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboLocation.Properties.Items.Add(reader(0))
            Me.cboLocation1.Properties.Items.Add(reader(0))
        End While
        Disconnect()

        'Load Location Automatically
        cboLocation.Text = CurrDept
        cboLocation1.Text = CurrDept

        LoadRecords()
        LoadRecordsCompleted()

        DisableControls()
    End Sub

    Private Sub btnView_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnView.ItemClick
        If XTab.SelectedTabPageIndex = 0 Then
            'If Not GridView.GetFocusedRowCellValue(GridView.Columns("Status")) = "Processing" Then
            '    DisplayResult()
            'End If
            DisplayResult()
        ElseIf XTab.SelectedTabPageIndex = 1 Then
            DisplayResultCompleted()
        End If
    End Sub

    Private Sub GridView_DoubleClick(sender As Object, e As EventArgs) Handles GridView.DoubleClick
        'If Not GridView.GetFocusedRowCellValue(GridView.Columns("Status")) = "Processing" Then
        '    DisplayResult()
        'End If
        DisplayResult()
    End Sub

    Private Sub GridCompleted_DoubleClick(sender As Object, e As EventArgs) Handles GridCompleted.DoubleClick
        DisplayResultCompleted()
    End Sub

    Private Sub btnRefresh_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnRefresh.ItemClick
        LoadRecords()
        LoadRecordsCompleted()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.ItemClick
        Me.Close()
    End Sub

    Private Sub cboLimit_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLocation.SelectedIndexChanged
        Try
            LoadRecords()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cboLimit1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLocation1.SelectedIndexChanged
        Try
            LoadRecordsCompleted()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        LoadRecordsCompleted()
    End Sub

    Private Sub cboSection_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboLocation1.SelectedIndexChanged
        Try
            LoadRecordsCompleted()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub XTab_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles XTab.SelectedPageChanged
        If XTab.SelectedTabPageIndex = 0 Then
            btnDelete.Enabled = True
            btnView.Enabled = True
            btnRefresh.Enabled = True
            btnBarcode.Enabled = True
            btnPrint.Enabled = False
            lblCountQueue.Text = "Record Count: " & GridView.RowCount
        ElseIf XTab.SelectedTabPageIndex = 1 Then
            btnView.Enabled = True
            btnRefresh.Enabled = True
            btnDelete.Enabled = False
            btnBarcode.Enabled = False
            btnPrint.Enabled = True
            lblCountQueue.Text = "Record Count: " & GridCompleted.RowCount
        End If
    End Sub

    'Private Sub tmLoadNew_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmLoadNew.Tick
    '    LoadNewResult()
    'End Sub

    'Private Sub LoadNewResult()
    '    Try
    '        Connect()
    '        rs.Connection = conn
    '        rs.CommandType = CommandType.Text
    '        rs.CommandText = "SELECT `status` FROM `tmpresultstatus` WHERE `status` = 'New Result'"
    '        reader = rs.ExecuteReader
    '        reader.Read()
    '        If reader.HasRows Then
    '            Disconnect()
    '            LoadRecords()
    '            UpdateResultStatus()
    '        End If
    '        Disconnect()
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    End Try
    'End Sub

    'Private Sub UpdateResultStatus()
    '    Connect()
    '    rs.Connection = conn
    '    rs.CommandType = CommandType.Text
    '    rs.CommandText = "UPDATE `tmpresultstatus` SET `status` = 'No Result'"
    '    rs.ExecuteNonQuery()
    '    Disconnect()
    'End Sub

    Private Sub btnPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrint.ItemClick
        Try
            If XTab.SelectedTabPageIndex = 0 Then

            ElseIf XTab.SelectedTabPageIndex = 1 Then
                Using myRDLCPrinter As New RDLCPrinterPrintReleased(GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("RefID")).ToString,
                                                                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Section")).ToString,
                                                                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SubSection")).ToString,
                                                                    "", My.Settings.DefaultPrinter)
                    If My.Settings.SaveAsPDF Then
                        Dim byteViewer As Byte() = myRDLCPrinter.LocalReport.Render("PDF")
                        Dim saveFileDialog1 As New SaveFileDialog()
                        saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
                        saveFileDialog1.FilterIndex = 2
                        saveFileDialog1.RestoreDirectory = True
                        Dim newFile As New FileStream(My.Settings.PDFLocation & GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SampleID")).ToString _
                                                        & "_" & GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientName")).ToString & ".pdf", FileMode.Create)
                        newFile.Write(byteViewer, 0, byteViewer.Length)
                        newFile.Close()

                        myRDLCPrinter.Print(1)
                    Else
                        myRDLCPrinter.Print(1)
                    End If
                End Using

                'Activity Logs
                ActivityLogs(GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SampleID")).ToString,
                             GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientID")).ToString,
                             GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientName")).ToString,
                             CurrUser,
                             "Print Result",
                             GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Request")).ToString,
                             GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Section")).ToString,
                             GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SubSection")).ToString)
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub

    Private Sub btnDelete_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnDelete.ItemClick
        If MessageBox.Show("Are you sure you want to reject " & GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString & " order?", "Confirm Reject", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            frmRejectOrder.ID = GridView.GetFocusedRowCellValue(GridView.Columns("SequenceNo")).ToString
            frmRejectOrder.sID = GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString
            frmRejectOrder.pID = GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString
            frmRejectOrder.pName = GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString
            frmRejectOrder.pTest = GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString
            frmRejectOrder.pSection = GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString
            frmRejectOrder.pSubSection = GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString
            frmRejectOrder.ShowDialog()
        End If
        LoadRecords()
    End Sub

    Private Sub btnReject_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnReject.ItemClick
        If MessageBox.Show("Are you sure you want to cancel " & GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString & " order?", "Confirm Cancel", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = vbYes Then
            frmCancelOR.ID = GridView.GetFocusedRowCellValue(GridView.Columns("SequenceNo")).ToString
            frmCancelOR.sID = GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString
            frmCancelOR.pID = GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString
            frmCancelOR.pName = GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString
            frmCancelOR.pTest = GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString
            frmCancelOR.pSection = GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString
            frmCancelOR.pSubSection = GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString
            frmCancelOR.ShowDialog()
        End If

        LoadRecords()
    End Sub

    Private Sub btnBarcode_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnBarcode.ItemClick
        PrintBarcode(GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("DateOfBirth")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("Sex")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString, 1)
        'Activity Logs
        ActivityLogs(GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString,
                     GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString,
                     GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString,
                     CurrUser,
                     "Reprint Barcode",
                     GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString,
                     GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString,
                     GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString)
    End Sub

    Private Sub frm_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        'MainFOrm.aceFecal.Appearance.Normal.BackColor = Color.FromArgb(6, 31, 71)
        MainFOrm.aceChem.Appearance.Normal.BackColor = Color.FromArgb(16, 110, 190)
        MainFOrm.aceChem.Appearance.Normal.ForeColor = Color.FromArgb(255, 255, 255)
    End Sub

    Private Sub frm_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        MainFOrm.lblTitle.Text = ""
        MainFOrm.aceChem.Appearance.Normal.BackColor = Color.FromArgb(240, 240, 240)
        MainFOrm.aceChem.Appearance.Normal.ForeColor = Color.FromArgb(27, 41, 62)
        Me.Dispose()
    End Sub

#Region "DisplayResults"
    '############################################-----For On-Queue Orders-----############################################
    Private Sub DisplayResult()
        'On Error Resume Next
        frmChemNew.cboPathologist.Properties.Items.Clear()
        frmChemNew.cboMedTech.Properties.Items.Clear()
        frmChemNew.cboVerify.Properties.Items.Clear()

        '###########################---Load Basic Patient Details---######################################################
        frmChemNew.mainID = GridView.GetFocusedRowCellValue(GridView.Columns("RefID")).ToString
        frmChemNew.Section = GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString
        frmChemNew.SubSection = GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString
        frmChemNew.PatientID = GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString
        frmChemNew.txtSampleID.Text = GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString
        frmChemNew.txtPatientID.Text = GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString
        frmChemNew.txtName.Text = GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString
        frmChemNew.cboRequest.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString
        frmChemNew.dtReceived.Value = GridView.GetFocusedRowCellValue(GridView.Columns("DateReceived")).ToString
        frmChemNew.tmTimeReceived.Text = GridView.GetFocusedRowCellValue(GridView.Columns("TimeReceived")).ToString
        frmChemNew.cboSex.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Sex")).ToString
        frmChemNew.dtBDate.Text = GridView.GetFocusedRowCellValue(GridView.Columns("DateOfBirth")).ToString
        frmChemNew.cboPatientType.Text = GridView.GetFocusedRowCellValue(GridView.Columns("PatientType")).ToString
        frmChemNew.cboPhysician.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Physician")).ToString
        frmChemNew.cboRoom.Text = GridView.GetFocusedRowCellValue(GridView.Columns("RoomWard")).ToString

        If GridView.GetFocusedRowCellValue(GridView.Columns("PerformedBy")).ToString = "" Then
            frmChemNew.cboMedTech.Text = CurrUser
        Else
            frmChemNew.cboMedTech.Text = GridView.GetFocusedRowCellValue(GridView.Columns("PerformedBy")).ToString
        End If
        If GridView.GetFocusedRowCellValue(GridView.Columns("ReleasedBy")).ToString = "" Then
            frmChemNew.cboVerify.Text = CurrUser
        Else
            frmChemNew.cboVerify.Text = GridView.GetFocusedRowCellValue(GridView.Columns("ReleasedBy")).ToString
        End If

        frmChemNew.txtAddress.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Address")).ToString
        frmChemNew.txtContact.Text = GridView.GetFocusedRowCellValue(GridView.Columns("ContactNo")).ToString
        frmChemNew.cboCS.Text = GridView.GetFocusedRowCellValue(GridView.Columns("CivilStatus")).ToString
        frmChemNew.txtAccession.Text = GridView.GetFocusedRowCellValue(GridView.Columns("AccessionNo")).ToString
        frmChemNew.txtORNo.Text = GridView.GetFocusedRowCellValue(GridView.Columns("ORNo")).ToString
        frmChemNew.txtChargeSlip.Text = GridView.GetFocusedRowCellValue(GridView.Columns("ChargeSlip")).ToString
        frmChemNew.txtRemarks.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Remarks")).ToString
        frmChemNew.txtComment.Text = GridView.GetFocusedRowCellValue(GridView.Columns("Diagnosis")).ToString
        frmChemNew.txtEmail.Text = GridView.GetFocusedRowCellValue(GridView.Columns("EmailAddress")).ToString

        'For Age computation
        Dim Age As String = ""
        Age = GetBDate(GridView.GetFocusedRowCellValue(GridView.Columns("DateOfBirth")))
        frmChemNew.txtAge.Text = Age.Split(" ").GetValue(0)
        frmChemNew.txtClass.Text = Age.Split(" ").GetValue(1)
        '######################################----END-----###############################################################

        'frmChemNew.GetBDate()

        frmChemNew.cboPathologist.SelectedIndex = 0

        'Activity Logs
        ActivityLogs(GridView.GetFocusedRowCellValue(GridView.Columns("SampleID")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("PatientID")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("PatientName")).ToString,
                             CurrUser,
                             "View Result",
                             GridView.GetFocusedRowCellValue(GridView.Columns("Request")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("Section")).ToString,
                             GridView.GetFocusedRowCellValue(GridView.Columns("SubSection")).ToString)
        frmChemNew.ShowDialog()
    End Sub
    '############################################--------------END------------############################################

    '############################################-----For Completed Orders-----############################################
    Private Sub DisplayResultCompleted()
        frmChemOrdered.cboPathologist.Properties.Items.Clear()
        frmChemOrdered.cboMedTech.Properties.Items.Clear()
        frmChemOrdered.cboVerify.Properties.Items.Clear()

        '###########################---Load Basic Patient Details---######################################################
        frmChemOrdered.mainID = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("RefID")).ToString
        frmChemOrdered.Section = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Section")).ToString
        frmChemOrdered.SubSection = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SubSection")).ToString
        frmChemOrdered.PatientID = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientID")).ToString
        frmChemOrdered.txtSampleID.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SampleID")).ToString
        frmChemOrdered.txtPatientID.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientID")).ToString
        frmChemOrdered.txtName.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientName")).ToString
        frmChemOrdered.cboRequest.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Request")).ToString
        frmChemOrdered.dtReceived.EditValue = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("DateReceived")).ToString
        frmChemOrdered.tmTimeReceived.EditValue = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("TimeReceived")).ToString
        frmChemOrdered.tmTimeReleased.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("DateReleased")).ToString
        frmChemOrdered.cboSex.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Sex")).ToString
        frmChemOrdered.dtBDate.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("DateOfBirth")).ToString
        frmChemOrdered.cboPatientType.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientType")).ToString
        frmChemOrdered.cboPhysician.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Physician")).ToString
        frmChemOrdered.cboRoom.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("RoomWard")).ToString

        If GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PerformedBy")).ToString = "" Then
            frmChemOrdered.cboMedTech.Text = CurrUser
        Else
            frmChemOrdered.cboMedTech.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PerformedBy")).ToString
        End If
        If GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("ReleasedBy")).ToString = "" Then
            frmChemOrdered.cboVerify.Text = CurrUser
        Else
            frmChemOrdered.cboVerify.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("ReleasedBy")).ToString
        End If

        frmChemOrdered.txtAddress.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Address")).ToString
        frmChemOrdered.txtContact.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("ContactNo")).ToString
        frmChemOrdered.cboCS.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("CivilStatus")).ToString
        frmChemOrdered.txtAccession.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("AccessionNo")).ToString
        frmChemOrdered.txtORNo.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("ORNo")).ToString
        frmChemOrdered.txtChargeSlip.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("ChargeSlip")).ToString
        frmChemOrdered.txtRemarks.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Remarks")).ToString
        frmChemOrdered.txtComment.Text = GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Diagnosis")).ToString
        frmChemOrdered.txtEmail.Text = GridView.GetFocusedRowCellValue(GridView.Columns("EmailAddress")).ToString

        'For Age computation
        Dim Age As String = ""
        Age = GetBDate(GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("DateOfBirth")).ToString)
        frmChemOrdered.txtAge.Text = Age.Split(" ").GetValue(0)
        frmChemOrdered.txtClass.Text = Age.Split(" ").GetValue(1)
        '######################################----END-----###############################################################

        'Parameters 
        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@PatientID", GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientID")).ToString)
        rs.Parameters.AddWithValue("@MainID", GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("RefID")).ToString)
        rs.Parameters.AddWithValue("@Section", GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Section")).ToString)
        rs.Parameters.AddWithValue("@SubSection", GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SubSection")).ToString)
        rs.Parameters.AddWithValue("@CurrUser", CurrUser)

        'Activity Logs
        ActivityLogs(GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SampleID")).ToString,
                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientID")).ToString,
                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("PatientName")).ToString,
                    CurrUser,
                    "View Archived Result",
                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Request")).ToString,
                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("Section")).ToString,
                    GridCompleted.GetFocusedRowCellValue(GridCompleted.Columns("SubSection")).ToString)

        frmChemOrdered.ShowDialog()
    End Sub

    '############################################--------------END------------############################################

#End Region

End Class