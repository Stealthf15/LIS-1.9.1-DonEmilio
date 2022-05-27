Imports System.IO
Imports System.Data
Imports System.Text
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Collections.Generic
Imports Microsoft.Reporting.WinForms
Imports System.Reflection
Imports MySql.Data.MySqlClient

Module RDLCPrintPreviewReleased

    Dim MedTech As String = ""
    Dim Verifier As String = ""
    Dim Request As String = ""

    Public Sub PrintPreviewReleased(ByVal SampleID As String, ByVal WorkListTable As String, ByVal ResultTable As String, ByVal PrintStatus As String, ByVal Section As String, ByVal SubSection As String, ByVal Frm As Form, ByVal ReportViewerData As Microsoft.Reporting.WinForms.ReportViewer)
        Try
            Request = ""

            'Clear all datasurce
            ReportViewerData.Reset()

            ReportViewerData.LocalReport.DataSources.Clear()

            ReportViewerData.LocalReport.ReportPath = Application.StartupPath & "\Reports\Result\" & SubSection & ".rdlc"

            'Select report according to section

            'IF REPORT HAS DATASET WE NEED THIS
            '###################----DataSet----#################################################################
            'Order Table
            Dim cn = New MySqlConnection(MyConnectionString)

            Dim SQL = "SELECT
					`order`.`id` AS SequenceNo,
					`order`.`status` AS Status,
					`order`.`sample_id` AS SampleID,
					`order`.`patient_id` AS PatientID,
					`order`.`patient_name` AS PatientName, 
					`order`.`test` AS Request,
					DATE_FORMAT(`order`.`date`, '%m/%d/%Y') AS DateReceived,
					`order`.`time` AS TimeReceived,
					DATE_FORMAT(`specimen_tracking`.`extracted`, '%m/%d/%Y %r') AS DateCheckedIn,
					`order`.`dt_released` AS DateReleased,
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
					patient_info.address AS Address,
					patient_info.contact_no AS ContactNo,
					patient_info.civil_status AS CivilStatus,
					additional_info.accession_no AS AccessionNo,
					additional_info.or_no AS ORNo,
					additional_info.cs_no AS ChargeSlip,
					patient_remarks.remarks AS Remarks,
					patient_remarks.diagnosis AS Diagnosis,
					email_details.email_address as EmailAddress
				FROM `order` 
					Left JOIN `specimen_tracking` ON`specimen_tracking`.`sample_id` = `order`.`main_id` And specimen_tracking.section = order.testtype And specimen_tracking.sub_section = order.sub_section
					Left JOIN `patient_info` ON`patient_info`.`patient_id` = `order`.`patient_id`
					Left JOIN `additional_info` ON `additional_info`.`sample_id` = `order`.`main_id` And additional_info.section = order.testtype And additional_info.sub_section = order.sub_section
					Left JOIN `patient_remarks` ON `patient_remarks`.`sample_id` = `order`.`main_id` And patient_remarks.section = order.testtype And patient_remarks.sub_section = order.sub_section
					Left JOIN `email_details` ON `email_details`.`sample_id` = `order`.`main_id` And email_details.section = order.testtype And email_details.sub_section = order.sub_section
					LEFT JOIN `medtech` T1 ON `T1`.`id` = `order`.`medtech`
					LEFT JOIN `medtech_verificator` T2 ON `T2`.`id` = `order`.`verified_by`
				WHERE (`order`.`status` = 'Printed') 
				AND (`order`.`testtype` = `specimen_tracking`.`section`)
				AND (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
				AND (`order`.`testtype` = '" & Section & "')
                AND (`order`.`sub_section` = '" & SubSection & "')
				AND (`order`.`main_id` = '" & SampleID & "')
				ORDER BY `order`.`main_id` DESC"

            Dim adapter = New MySqlDataAdapter(SQL, cn)
            Dim rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            Dim rdlc_rds = New ReportDataSource("DataSet1", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)

            'Result Table
            Connect()
            Dim result_SQL = "SELECT `result`.`universal_id` AS TestName, 
							`result`.flag AS Flag,
							`result`.`measurement` AS SI, 
							reference_range.`si_range` as ReferenceRange, 
							`result`.`units` as Unit,
							`result`.`value_conv` AS Conventional, 
							`result`.`unit_conv` AS Units, 
							reference_range.`conv_range` AS RefRange,  
							`result`.`instrument` AS Instrument, 
							`result`.`test_code` AS TestCode, 
							`result`.`id` AS ID, 
							`result`.`test_group` AS TestGroup, 
							`result`.`his_code` AS HISTestCode, 
							`result`.`his_mainid` AS HISMainID, 
							`result`.`print_status` AS PrintStatus,
							reference_range.`low_value` AS LowValue,
							reference_range.`high_value` AS HighValue,
							specimen.`convertion_factor` AS ConversionFactor,
							specimen.`convertion_multiplier` AS ConversionMultiplier,
							specimen.`order_no` AS DisplayNo
						FROM `result`
							LEFT JOIN reference_range ON `result`.test_code = reference_range.test_code AND reference_range.machine = `result`.instrument
							LEFT JOIN specimen ON `result`.test_code = specimen.test_code AND `result`.instrument = specimen.instrument
						WHERE `result`.`sample_id` = '" & SampleID & "' AND `result`.section = '" & Section & "' AND `result`.sub_section = '" & SubSection & "' AND print_status = '" & PrintStatus & "' GROUP BY `result`.test_code ORDER BY specimen.order_no ASC;"

            Dim result_adapter = New MySqlDataAdapter(result_SQL, conn)
            Dim result_rdlc_table = New DataTable()
            result_adapter.Fill(result_rdlc_table)
            Dim result_rdlc_rds = New ReportDataSource("DataSet2", result_rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(result_rdlc_rds)
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT medtech, verified_by from `order` WHERE `main_id` = '" & SampleID & "' AND testtype = '" & Section & "' AND sub_section = '" & SubSection & "' ORDER BY `main_id` LIMIT 1"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                MedTech = reader(0).ToString
                Verifier = reader(1).ToString
            End If
            Disconnect()

            'Company Profile Table
            Connect()
            SQL = "SELECT * FROM `company_profile`"
            adapter = New MySqlDataAdapter(SQL, conn)
            rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            rdlc_rds = New ReportDataSource("DataSet3", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)
            Disconnect()

            'ViewOrderMedtech Table
            Connect()
            SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license` FROM `medtech` WHERE `id` = '" & MedTech & "' ORDER BY `id`"
            adapter = New MySqlDataAdapter(SQL, conn)
            rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            rdlc_rds = New ReportDataSource("DataSet4", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)
            Disconnect()

            'ViewOrderVerified Table
            Connect()
            SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license` FROM `medtech_verificator` WHERE `id` = '" & Verifier & "' ORDER BY `id`"
            adapter = New MySqlDataAdapter(SQL, conn)
            rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            rdlc_rds = New ReportDataSource("DataSet5", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)
            Disconnect()

            'ViewOrderPathologist Table
            Connect()
            SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license`, `img` FROM `pathologist` WHERE `id` = '" & PathologistID & "' ORDER BY `id`"
            adapter = New MySqlDataAdapter(SQL, conn)
            rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            rdlc_rds = New ReportDataSource("DataSet6", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)
            Disconnect()

            'Assay Information Table
            Connect()
            SQL = "SELECT `sample_id`, `method_used`, `reagent`, `lot_number`, `expiry` FROM `rat_assay_info` WHERE `sample_id` = '" & SampleID & "'"
            adapter = New MySqlDataAdapter(SQL, conn)
            rdlc_table = New DataTable()
            adapter.Fill(rdlc_table)
            rdlc_rds = New ReportDataSource("DataSet7", rdlc_table)
            ReportViewerData.LocalReport.DataSources.Add(rdlc_rds)
            Disconnect()

            '###################----End of DataSet----############################################################

            'IF REPORT HAS PARAMETERS WE NEED THIS
            '###################----Parameters----################################################################
            'Parameterized data to pass in report parameters

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "Select hl7_result_his_code.his_test_code As Request FROM additional_info 
                                LEFT JOIN hl7_result_his_code ON additional_info.id = hl7_result_his_code.additional_info_id
                              WHERE 
                                additional_info.sample_id = '" & SampleID & "' 
                                AND additional_info.section = '" & Section & "' 
                                AND additional_info.sub_section = '" & SubSection & "' 
                              ORDER BY additional_info.sample_id;"
            reader = rs.ExecuteReader
            While reader.Read
                If reader.HasRows Then
                    Request &= reader(0).ToString & ", "
                End If
            End While
            Disconnect()

            If Section = "Chemistry" Then
                Dim p_Request As New ReportParameter("Request", Request)
                ReportViewerData.LocalReport.SetParameters(p_Request)
            End If

            'Dim Print_Status As New ReportParameter("PrintStatus", PrintStatus)
            'ReportViewerData.LocalReport.SetParameters(Print_Status)
            '######################----End of Parameters----###############################################################

            'Set display format
            ReportViewerData.SetDisplayMode(Microsoft.Reporting.WinForms.DisplayMode.PrintLayout)

            'Refresh Report
            ReportViewerData.RefreshReport()
            Frm.Show()
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error Previewing Report", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Exit Sub
        End Try
    End Sub
End Module
