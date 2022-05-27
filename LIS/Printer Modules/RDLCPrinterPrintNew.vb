Imports System.IO
Imports System.Data
Imports System.Text
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Collections.Generic
Imports Microsoft.Reporting.WinForms
Imports System.Reflection
Imports MySql.Data.MySqlClient

Public Class RDLCPrinterPrintNew : Implements IDisposable

    Private _ReportInstance As Microsoft.Reporting.WinForms.LocalReport = Nothing
    Private _TempFileName As String = String.Empty
    Private _DefaultPrinterName As String = String.Empty

    Dim MedTech As String = ""
    Dim Verifier As String = ""
    Dim Request As String = ""

    Public Sub New(ByVal SampleID As String, ByVal Section As String, ByVal SubSection As String, ByVal PrintStatus As String, ByVal DefaultPrinterName As String)
        Request = ""

        'reportResourceName = "NameofProject.NameofRDLC"
        Dim reportResourceName = Application.StartupPath & "\Reports\Result\" & SubSection & ".rdlc"
        Dim myReport As New Microsoft.Reporting.WinForms.LocalReport

        Dim myReportParams As List(Of ReportParameter) = Nothing

        myReport = New Microsoft.Reporting.WinForms.LocalReport
        myReport.ReportPath = reportResourceName

        'IF REPORT HAS DATASET WE NEED THIS
        '###################----DataSet----#################################################################
        'Order Table
        myReport.DataSources.Clear()
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
				WHERE (`order`.`status` = 'Printed' OR `order`.`status` = 'Validated' OR `order`.`status` = 'Released') 
				AND (`order`.`testtype` = `specimen_tracking`.`section`)
				AND (`order`.`sub_section` = `specimen_tracking`.`sub_section`)
				AND (`order`.`testtype` = '" & Section & "')
                AND (`order`.`sub_section` = '" & SubSection & "')
				AND (`order`.`main_id` = '" & SampleID & "')
				ORDER BY `order`.`main_id` DESC LIMIT 1"

        Dim adapter = New MySqlDataAdapter(SQL, cn)
        Dim rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        Dim rdlc_rds = New ReportDataSource("DataSet1", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)

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
        myReport.DataSources.Add(result_rdlc_rds)
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
        SQL = "SELECT * FROM `company_profile` ORDER BY `id`"
        adapter = New MySqlDataAdapter(SQL, conn)
        rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        rdlc_rds = New ReportDataSource("DataSet3", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)
        Disconnect()

        'ViewOrderMedtech Table
        Connect()
        SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license` FROM `medtech` WHERE `id` = '" & MedTech & "' ORDER BY `id`"
        adapter = New MySqlDataAdapter(SQL, conn)
        rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        rdlc_rds = New ReportDataSource("DataSet4", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)
        Disconnect()

        'ViewOrderVerified Table
        Connect()
        SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license` FROM `medtech_verificator` WHERE `id` = '" & Verifier & "' ORDER BY `id`"
        adapter = New MySqlDataAdapter(SQL, conn)
        rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        rdlc_rds = New ReportDataSource("DataSet5", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)
        Disconnect()

        'ViewOrderPathologist Table
        Connect()
        SQL = "SELECT `fname`, `mname`, `lname`, `designation`, `license`, `img` FROM `pathologist` WHERE `id` = '" & PathologistID & "' ORDER BY `id`"
        adapter = New MySqlDataAdapter(SQL, conn)
        rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        rdlc_rds = New ReportDataSource("DataSet6", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)
        Disconnect()

        'Assay Information Table
        Connect()
        SQL = "SELECT `sample_id`, `method_used`, `reagent`, `lot_number`, `expiry` FROM `rat_assay_info` WHERE `sample_id` = '" & SampleID & "'"
        adapter = New MySqlDataAdapter(SQL, conn)
        rdlc_table = New DataTable()
        adapter.Fill(rdlc_table)
        rdlc_rds = New ReportDataSource("DataSet7", rdlc_table)
        myReport.DataSources.Add(rdlc_rds)
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
               WHERE additional_info.sample_id = '" & SampleID & "' AND additional_info.section = '" & Section & "' AND additional_info.sub_section = '" & SubSection & "' ORDER BY additional_info.`sample_id`;"
        reader = rs.ExecuteReader
        While reader.Read
            If reader.HasRows Then
                Request &= reader(0).ToString
            End If
        End While
        Disconnect()

        If Section = "Chemistry" Then
            Dim p_Request As New ReportParameter("Request", Request)
            myReport.SetParameters(p_Request)
        End If
        'Dim Print_Status As New ReportParameter("PrintStatus", PrintStatus)
        'myReport.SetParameters(Print_Status)
        '######################----End of Parameters----###############################################################

        myReport.Refresh()
        myReport.ReportEmbeddedResource = reportResourceName

        _ReportInstance = myReport
        _DefaultPrinterName = DefaultPrinterName
    End Sub

    Public ReadOnly Property LocalReport() As LocalReport
        Get
            Return _ReportInstance
        End Get
    End Property

    Public Sub Print(ByVal NumberOfCopies As Integer)
        Export(_ReportInstance)
        m_currentPageIndex = 0
        PrintInteral(NumberOfCopies)
    End Sub

    Private m_currentPageIndex As Integer
    Private m_streams As IList(Of Stream)

    Private Sub Export(ByVal report As LocalReport)
        Dim deviceInfo As String =
          "<DeviceInfo>" +
          "  <OutputFormat>EMF</OutputFormat>" +
          "  <PageWidth>8.5in</PageWidth>" +
          "  <PageHeight>11in</PageHeight>" +
          "  <MarginTop>0.1in</MarginTop>" +
          "  <MarginLeft>0.2in</MarginLeft>" +
          "  <MarginRight>0.2in</MarginRight>" +
          "  <MarginBottom>0.1in</MarginBottom>" +
          "</DeviceInfo>"

        Dim warnings() As Warning = Nothing
        m_streams = New List(Of Stream)()

        Try
            report.Render("Image", deviceInfo, AddressOf CreateStream,
                       warnings)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Dim stream As Stream
        For Each stream In m_streams
            stream.Position = 0
        Next
    End Sub

    Private Function CreateStream(ByVal name As String,
      ByVal fileNameExtension As String,
      ByVal encoding As Encoding, ByVal mimeType As String,
      ByVal willSeek As Boolean) As Stream
        _TempFileName = IO.Path.GetTempPath & "\" & name + "." + fileNameExtension
        Dim stream As Stream = New FileStream(_TempFileName, FileMode.Create)
        m_streams.Add(stream)
        Return stream
    End Function

    Private Sub PrintPage(ByVal sender As Object,
    ByVal ev As PrintPageEventArgs)

        m_streams(m_currentPageIndex).Position = 0

        Dim pageImage As New Metafile(m_streams(m_currentPageIndex))
        ev.Graphics.DrawImage(pageImage, ev.PageBounds)

        m_currentPageIndex += 1
        ev.HasMorePages = (m_currentPageIndex < m_streams.Count)

    End Sub

    Private Sub PrintInteral(ByVal NumberOfCopies As Integer)

        'IF THERE ARE NO STREAMS, WE CANT PRINT
        If m_streams Is Nothing Or m_streams.Count = 0 Then
            Return
        End If

        'CREATE PRINT DOCUMENT
        Dim printDoc As New PrintDocument()

        'IF THERE IS NO DEFAULT PRINTER NAME IN SETTINGS, PROMPT FOR ONE
        If _DefaultPrinterName = String.Empty Then
            Dim myPrintDialog As New PrintDialog
            myPrintDialog.PrinterSettings = printDoc.PrinterSettings
            If myPrintDialog.ShowDialog() = DialogResult.Cancel Then Return
        Else
            'WE HAVE A DEFAULT PRINTER NAME, ASSIGN IT
            printDoc.PrinterSettings.PrinterName = _DefaultPrinterName
        End If

        '    printDoc.PrinterSettings.Copies = CShort(NumberOfCopies)

        'IF PRINTER IS NOT VALID SHOW MESSAGE INDICATING ERROR
        If Not printDoc.PrinterSettings.IsValid Then
            MessageBox.Show("Error printing")
            Return
        End If

        'PRINT OUT THE PAGE TO THE SPECIFIED PRINTER
        AddHandler printDoc.PrintPage, AddressOf PrintPage

        If NumberOfCopies < 1 Then NumberOfCopies = 1
        For i As Integer = 1 To NumberOfCopies
            m_currentPageIndex = 0
            printDoc.Print()
        Next
    End Sub

    Private disposedValue As Boolean = False        ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(ByVal disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Not (m_streams Is Nothing) Then
                    Dim stream As Stream
                    For Each stream In m_streams
                        stream.Close()
                        stream.Dispose()
                    Next
                    m_streams = Nothing
                    If IO.File.Exists(_TempFileName) Then
                        Try
                            IO.File.Delete(_TempFileName)
                        Catch ex As Exception
                        End Try
                    End If
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

#Region " IDisposable Support "
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub
#End Region

End Class