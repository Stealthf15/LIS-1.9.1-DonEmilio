Imports System.IO
Imports System.IO.Ports
Imports System.Threading
Imports System.Xml
Imports System.Text.RegularExpressions

Imports System.Data.SqlClient
Imports System.Globalization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base

Public Class frmImmunoOrdered

    Public MedTechID As String = ""
    Public PathologistID As String = ""
    Public VerifyID As String = ""

    Public mainID As String
    Public MainSampleID As String = ""
    Public FinalMainID As String = ""

    Public PatientID As String = ""
    Public RDate As Date

    Public Channel As String = ""
    Public FLAG As String = ""
    Public RANGE As String = ""

    Public Section As String = ""
    Public SubSection As String = ""

    Public DefaultUnit As Integer = 0
    Public DefaultDiffCount As Integer = 0
    Public Validation As String = ""

    Dim Diff As Double

    Private Sub txtResult_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs)
        If InStr("1234567890.", e.KeyChar) = 0 And Not Chr(AscW(e.KeyChar)) = vbBack Then
            e.KeyChar = ChrW(0)
        End If
    End Sub

    Private Sub frmResultNewChem_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.ItemClick
        Me.Close()
    End Sub

    Private Sub txtAge_KeyPress(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtAge.KeyPress
        If InStr("1234567890", e.KeyChar) = 0 And Not Chr(AscW(e.KeyChar)) = vbBack Then
            e.KeyChar = ChrW(0)
        End If
    End Sub

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
        frmImmunoPrevious.patientID = txtPatientID.Text
        frmImmunoPrevious.section = Section
        frmImmunoPrevious.SubSection = SubSection
        frmImmunoPrevious.Age = txtAge.Text
        frmImmunoPrevious.Sex = cboSex.Text
        frmImmunoPrevious.Classification = txtClass.Text
        frmImmunoPrevious.ShowDialog()
    End Sub

    Private Sub cboVerify_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboVerify.SelectedIndexChanged, cboVerify.TextChanged
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT ID, NAME FROM (SELECT `id` AS ID, CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech_verificator`) AS T1 WHERE T1.`name` = '" & Me.cboVerify.Text & "'"
        reader = rs.ExecuteReader
        reader.Read()
        If reader.HasRows Then
            VerifyID = reader(0).ToString
        End If
        Disconnect()
    End Sub

    Public Sub LoadTest()
        'On Error Resume Next
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            'GridView.Appearance.OddRow.BackColor = Color.Gainsboro
            'GridView.OptionsView.EnableAppearanceOddRow = True
            'GridView.Appearance.EvenRow.BackColor = Color.White
            'GridView.OptionsView.EnableAppearanceEvenRow = True

            Dim SQL As String = "SELECT `result`.`universal_id` AS TestName, 
                                    `result`.flag AS Flag,
                                    `result`.`measurement` AS Result, 
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
                                LEFT JOIN reference_range ON `result`.test_code = reference_range.test_code AND reference_range.machine = `result`.instrument AND (reference_range.classification = @Classification AND reference_range.gender = @Gender AND (@Age BETWEEN reference_range.age_begin and reference_range.age_end))
                                LEFT JOIN specimen ON `result`.test_code = specimen.test_code AND `result`.instrument = specimen.instrument
                                WHERE `result`.`sample_id` = @MainID AND `result`.section = @Section AND `result`.sub_section = @SubSection GROUP BY `result`.test_code ORDER BY specimen.order_no ASC"


            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@MainID", mainID)
            command.Parameters.AddWithValue("@PID", PatientID)
            command.Parameters.AddWithValue("@Section", Section)
            command.Parameters.AddWithValue("@SubSection", SubSection)
            command.Parameters.AddWithValue("@Age", txtAge.Text)
            command.Parameters.AddWithValue("@Gender", cboSex.Text)
            command.Parameters.AddWithValue("@Classification", txtClass.Text)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)
            command.CommandType = CommandType.Text

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtResult.DataSource = myTable

            GridView.Columns("Conventional").Visible = False
            GridView.Columns("RefRange").Visible = False
            GridView.Columns("Units").Visible = False
            GridView.Columns("ReferenceRange").Visible = False
            GridView.Columns("TestCode").Visible = False
            GridView.Columns("ID").Visible = False
            GridView.Columns("HISTestCode").Visible = False
            GridView.Columns("HISMainID").Visible = False
            GridView.Columns("TestGroup").Visible = False
            GridView.Columns("PrintStatus").Visible = False
            GridView.Columns("LowValue").Visible = False
            GridView.Columns("HighValue").Visible = False
            GridView.Columns("ConversionFactor").Visible = False
            GridView.Columns("ConversionMultiplier").Visible = False
            GridView.Columns("DisplayNo").Visible = False

            'Version 0.5.6.6
            'Not allow edit on Grid View Columns to prevent it to display on Results Form or cause of error
            GridView.Columns("TestName").OptionsColumn.AllowEdit = False
            GridView.Columns("Flag").OptionsColumn.AllowEdit = False
            GridView.Columns("Unit").OptionsColumn.AllowEdit = False
            GridView.Columns("ReferenceRange").OptionsColumn.AllowEdit = False
            GridView.Columns("Conventional").OptionsColumn.AllowEdit = False
            GridView.Columns("Units").OptionsColumn.AllowEdit = False
            GridView.Columns("RefRange").OptionsColumn.AllowEdit = False
            GridView.Columns("Instrument").OptionsColumn.AllowEdit = False

            'Custom Column Name
            GridView.Columns("Units").Caption = "Unit"
            GridView.Columns("RefRange").Caption = "Reference Range"

            'Custom Column Name
            GridView.Columns("Conventional").Caption = "Final Result"
            GridView.Columns("Units").Caption = "Unit"
            GridView.Columns("RefRange").Caption = "Reference Range"

            ' Make the grid read-only. 
            'GridView.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridView.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridView.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            For x As Integer = 0 To GridView.RowCount - 1 Step 1
                If GridView.GetRowCellValue(x, GridView.Columns("PrintStatus")) Then
                    GridView.SelectRow(x)
                Else

                End If
            Next

            'GridView.Columns("TestGroup").Group()
            'GridView.Columns("TestGroup").Caption = " "

            LoadRangeAndValues()

        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Public Sub LoadRangeAndValues()
        Try
            For x As Integer = 0 To Me.GridView.RowCount - 1 Step 1

                '#################################################----REFERENCE RANGE & FLAGS----################################################################################
                If Not GridView.GetRowCellValue(x, GridView.Columns("Result")) = "" Then
                    If IsNumeric(GridView.GetRowCellValue(x, GridView.Columns("Result"))) Then
                        If GridView.GetRowCellValue(x, GridView.Columns("LowValue")).ToString() <> "" And GridView.GetRowCellValue(x, GridView.Columns("HighValue")).ToString() <> "" Then
                            If CDbl(GridView.GetRowCellValue(x, GridView.Columns("Result"))) < CDbl(GridView.GetRowCellValue(x, GridView.Columns("LowValue"))) Then
                                FLAG = "L"
                            ElseIf CDbl(GridView.GetRowCellValue(x, GridView.Columns("Result"))) > CDbl(GridView.GetRowCellValue(x, GridView.Columns("HighValue"))) Then
                                FLAG = "H"
                            Else
                                FLAG = ""
                            End If
                        Else
                            FLAG = ""
                        End If
                    Else
                        FLAG = ""
                    End If
                    GridView.SetRowCellValue(x, GridView.Columns("Flag"), FLAG)
                End If
                '#################################################----REFERENCE RANGE & FLAGS----################################################################################

                '#################################################----CONVERTION FACTOR----################################################################################
                If Not GridView.GetRowCellValue(x, GridView.Columns("Result")) = "" Then
                    If IsNumeric(GridView.GetRowCellValue(x, GridView.Columns("Result"))) Then
                        GridView.SetRowCellValue(x, GridView.Columns("Conventional"), FormatNumber(Val(GridView.GetRowCellValue(x, GridView.Columns("Result"))) / Val(GridView.GetRowCellValue(x, GridView.Columns("ConversionFactor"))), Val(GridView.GetRowCellValue(x, GridView.Columns("ConversionMultiplier")))))
                    Else
                        GridView.SetRowCellValue(x, GridView.Columns("Conventional"), GridView.GetRowCellValue(x, GridView.Columns("Result")))
                    End If
                Else
                    GridView.SetRowCellValue(x, GridView.Columns("Conventional"), GridView.GetRowCellValue(x, GridView.Columns("Result")))
                End If
                '#################################################----CONVERTION FACTOR----################################################################################
            Next
        Catch ex As Exception
            Disconnect()
            'MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Public Sub AutoLoadVerificator()
        '###########################---Load Med Tech for Verification---##################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech_verificator` ORDER BY `name`"
        reader = rs.ExecuteReader
        While reader.Read
            cboVerify.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        '######################################----END-----###############################################################
    End Sub

    Private Sub frmFecalOrdered_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.cboPathologist.Text = Pathologist

        If My.Settings.HL7Write = True Then
            btnResend.Visibility = True
        Else
            btnResend.Visibility = False
        End If
    End Sub

    Private Sub frmFecalOrdered_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        AutoLoadDoctor()
        AutoLoadRoom()
        AutoLoadVerificator()
    End Sub

    Private Sub frmFecalOrdered_Activated(sender As Object, e As EventArgs) Handles Me.Activated
        LoadTest()
    End Sub

    Private Sub GridView_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles GridView.CellValueChanged
        Try
            If e.Column.FieldName = "Result" Then
                LoadRangeAndValues()
            End If
        Catch
        End Try
    End Sub

    Private Sub btnAddTest_ItemClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddTest.Click
        frmImmunoAddTest.mainID = mainID
        frmImmunoAddTest.patientID = txtPatientID.Text
        frmImmunoAddTest.TypeResult = "Old"

        frmImmunoAddTest.ShowDialog()
    End Sub

    Private Sub txtAge_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSex.SelectedIndexChanged, cboSex.TextChanged, txtAge.TextChanged, txtClass.SelectedIndexChanged
        'LoadRangeAndValues()\
        LoadRangeAndValues()
    End Sub

    Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        frmImmunoPatientList.Type = "Old"
        frmImmunoPatientList.ShowDialog()
    End Sub

    Private Sub btnDelete_ItemClick(ByVal sender As Object, ByVal e As EventArgs)
        'Try
        '    rs.Parameters.Clear()
        '    rs.Parameters.AddWithValue("@sample_id", mainID)
        '    Dim rows As DataGridViewRow = dtResult.SelectedRows(0)

        '    rs.Parameters.AddWithValue("@id", rows.Cells(11).Value)

        '    DeleteRecordSQL("DELETE FROM `tmpResult` WHERE sample_id = @sample_id AND `id` = @ID")
        '    LoadTest()
        'Catch ex As Exception
        '    MessageBox.Show("No Records Selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        'End Try

    End Sub

    Public Sub AutoLoadDoctor()
        Me.cboPhysician.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT `name` FROM `requestor` ORDER BY `name`"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboPhysician.Properties.Items.Add(reader(0))
        End While
        Disconnect()
    End Sub

    Public Sub AutoLoadRoom()
        Me.cboRoom.Properties.Items.Clear()
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT * FROM `department` ORDER BY `dept_name`"
        reader = rs.ExecuteReader
        While reader.Read
            Me.cboRoom.Properties.Items.Add(reader(1).ToString)
        End While
        Disconnect()
    End Sub

    Private Sub dtBDate_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles dtBDate.EditValueChanged
        GetBDate()
    End Sub

    Public Sub GetBDate()
        Try
            If dtBDate.DateTime = Nothing Then
                Exit Sub
            End If

            Dim Birthday As Date = dtBDate.DateTime
            Dim endDate As Date = Date.Now

            Dim timeSpan As TimeSpan = endDate.Subtract(Birthday)
            Dim totalDays As Integer = timeSpan.TotalDays
            Dim totalYears As Integer = Math.Truncate(totalDays / 365)
            Dim totalMonths As Integer = Math.Truncate((totalDays Mod 365) / 30)
            Dim remainingDays As Integer = Math.Truncate((totalDays Mod 365) Mod 30)

            If totalDays <= 61 Then
                txtClass.Text = "Day(s)"
                txtAge.Text = totalDays

            ElseIf totalDays >= 62 And totalDays <= 364 Then
                txtClass.Text = "Month(s)"
                txtAge.Text = totalMonths

            ElseIf totalDays >= 365 Then
                txtClass.Text = "Year(s)"
                txtAge.Text = totalYears
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnValidate_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnValidate.ItemClick
        Try
            If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            MainSampleID = mainID
            'MainSampleID = ID_Randomizer()

            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@mainID", mainID)
            rs.Parameters.AddWithValue("@MainSampleID", MainSampleID)

            rs.Parameters.AddWithValue("@medtech", cboMedTech.Text)
            rs.Parameters.AddWithValue("@verify", cboVerify.Text)

            rs.Parameters.AddWithValue("@pathologist", PathologistID)
            rs.Parameters.AddWithValue("@medtechid", CurrEmail)
            rs.Parameters.AddWithValue("@verifyid", VerifyID)

            rs.Parameters.AddWithValue("@remarks", txtRemarks.Text)
            rs.Parameters.AddWithValue("@lab_comment", txtComment.Text)

            rs.Parameters.AddWithValue("@status", "Printed")

            rs.Parameters.AddWithValue("@id", SampleID)
            rs.Parameters.AddWithValue("@sample_id", txtSampleID.Text)
            rs.Parameters.AddWithValue("@patient_id", txtPatientID.Text)
            rs.Parameters.AddWithValue("@name", txtName.Text)
            rs.Parameters.AddWithValue("@age", txtAge.Text)
            rs.Parameters.AddWithValue("@type", txtClass.Text)
            rs.Parameters.AddWithValue("@bdate", dtBDate.Text)
            rs.Parameters.AddWithValue("@physician", cboPhysician.Text)
            rs.Parameters.AddWithValue("@room", cboRoom.Text)
            rs.Parameters.AddWithValue("@sex", cboSex.Text)
            rs.Parameters.AddWithValue("@CS", cboCS.Text)
            rs.Parameters.AddWithValue("@address", txtAddress.Text)
            rs.Parameters.AddWithValue("@contact", txtContact.Text)
            rs.Parameters.AddWithValue("@test", cboRequest.Text)
            rs.Parameters.AddWithValue("@patient_type", cboPatientType.Text)
            rs.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtReceived.DateTime, "yyyy-MM-dd")
            rs.Parameters.AddWithValue("@time", tmTimeReceived.Text)
            rs.Parameters.AddWithValue("@date_release", Format(Now, "yyyy-MM-dd") & " " & Now.ToLongTimeString)
            rs.Parameters.AddWithValue("@accession_no", txtAccession.Text)
            rs.Parameters.AddWithValue("@OR_No", txtORNo.Text)
            rs.Parameters.AddWithValue("@CS_No", txtChargeSlip.Text)
            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            UpdateRecordwthoutMSG("UPDATE `patient_info` SET " _
                    & "`patient_id` = @patient_id," _
                    & "`name` = @name," _
                    & "`sex` = @sex," _
                    & "`date_of_birth` = @bdate," _
                    & "`age` = @age," _
                    & "`civil_status` = @CS," _
                    & "`address` = @address," _
                    & "`contact_no` = @contact" _
                    & " WHERE `patient_id` = @patient_id ORDER BY `patient_id` LIMIT 1"
                    )

            UpdateRecordwthoutMSG("UPDATE `order` SET " _
                    & "`sample_id` = @sample_id," _
                    & "`patient_id` = @patient_id," _
                    & "`patient_name` = @name," _
                    & "`sex` = @sex," _
                    & "`bdate` = @bdate," _
                    & "`age` = @age," _
                    & "`type` = @type," _
                    & "`physician` = @physician," _
                    & "`dept` = @room," _
                    & "`test` = @test," _
                    & "`patient_type` = @patient_type," _
                    & "`status` = @status" _
                    & " WHERE main_id = @mainID AND testtype = @Section AND sub_section = @SubSection ORDER BY `main_id` LIMIT 1"
                    )

            UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                    & "`accession_no` = @accession_no," _
                    & "`or_no` = @OR_No," _
                    & "`cs_no` = @CS_No" _
                    & " WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection ORDER BY `sample_id` LIMIT 1"
                    )

            For x As Integer = 0 To GridView.RowCount - 1 Step 1
                If (GridView.IsRowSelected(x)) Then
                    UpdateRecordwthoutMSG("UPDATE `result` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "Result") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '1'" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "' ORDER BY `sample_id` LIMIT 1"
                          )
                Else
                    UpdateRecordwthoutMSG("UPDATE `result` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "Result") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '0'" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "' ORDER BY `sample_id` LIMIT 1"
                          )
                End If
            Next

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "Select `sample_id` FROM `patient_remarks` WHERE `sample_id` = @mainID And `section` = @Section And `sub_section` = @SubSection ORDER BY `sample_id` LIMIT 1"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Disconnect()
                UpdateRecordwthoutMSG("UPDATE `patient_remarks` Set `remarks` = @remarks, `diagnosis` = @lab_comment WHERE `sample_id` = @mainID And `section` = @Section And `sub_section` = @SubSection ORDER BY `sample_id` LIMIT 1")
            Else
                Disconnect()
                SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`remarks`, `diagnosis`, `sample_id`, `section`, `sub_section`) VALUES (@remarks, @lab_comment, @MainSampleID, @Section, @SubSection) ORDER BY `sample_id` LIMIT 1")
            End If
            Disconnect()

            With My.Settings
                If .SQLConnection = True Then
                    UpdateResultiHOMIS()
                End If
            End With

            With My.Settings
                If .HL7Write = True Then
                    CreateHL7File()
                End If
            End With

            'UpdateWorkSheet()

            gcAdditional.Enabled = False
            gcPatient.Enabled = False
            gcRemarks.Enabled = False
            gcSignature.Enabled = False
            gcTest.Enabled = False

            txtRemarks.Enabled = False
            txtComment.Enabled = False

            btnValidate.Enabled = False
            btnEdit.Enabled = True
            btnPreview.Enabled = True
            btnPrintNow.Enabled = True

            mainID = MainSampleID
            frmImmunoWorklist.LoadRecords()

        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

    Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
        frmImmunoRerun.mainID = mainID
        frmImmunoRerun.Section = Section
        frmImmunoRerun.SubSection = SubSection
        'frmChemRerun.PatientID = PatientID
        'frmChemRerun.RDate = RDate
        frmImmunoRerun.ShowDialog()
    End Sub

    Private Sub btnPrintNow_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrintNow.ItemClick
        Try
            If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            Using myRDLCPrinter As New RDLCPrinterPrintReleased(mainID, Section, SubSection, 1, My.Settings.DefaultPrinter)
                If My.Settings.SaveAsPDF Then
                    Dim byteViewer As Byte() = myRDLCPrinter.LocalReport.Render("PDF")
                    Dim saveFileDialog1 As New SaveFileDialog()
                    saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
                    saveFileDialog1.FilterIndex = 2
                    saveFileDialog1.RestoreDirectory = True
                    Dim newFile As New FileStream(CreateFolder(Section) & txtSampleID.Text & "_" & txtName.Text & ".pdf", FileMode.Create)
                    newFile.Write(byteViewer, 0, byteViewer.Length)
                    newFile.Close()

                    myRDLCPrinter.Print(1)
                Else
                    myRDLCPrinter.Print(1)
                End If
            End Using

            With My.Settings
                If .HL7Write = True Then
                    CreateHL7File()
                End If
            End With

            frmImmunoWorklist.LoadRecords()
            frmImmunoWorklist.LoadRecordsCompleted()

            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub btnViewPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnViewPrint.ItemClick
        'For Each rows As DataGridViewRow In GridView.rows
        RPTresults.sample_id = mainID
        PrintPreviewReleased(mainID, "order", "result", 1, Section, SubSection, RPTresults, RPTresults.ReportViewer1)
        'Next
    End Sub

    Private Sub GridView_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridView.RowCellStyle
        If e.Column.FieldName = "TestName" Then
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Default
        Else
            e.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
        End If
    End Sub

    Private Sub btnEdit_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnEdit.ItemClick
        gcAdditional.Enabled = True
        gcTest.Enabled = True
        gcSignature.Enabled = True
        gcPatient.Enabled = True
        btnValidate.Enabled = True
        btnAddTest.Enabled = True
        txtRemarks.Enabled = True
        txtComment.Enabled = True

        btnEdit.Enabled = False
        btnPreview.Enabled = True
        btnViewPrint.Enabled = False
    End Sub

    '######################################### Shift Function #########################################
    Private Function getShift(ByVal shift As TimeSpan)
        Dim readerVar As String

        'Variable that Determine the Shift Next Day
        Dim thrdshft As New TimeSpan(22, 0, 0) '22:00:00
        Dim thrdshft2 As New TimeSpan(7, 0, 0) '07:00:00

        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT * FROM `shift`"
        reader = rs.ExecuteReader
        While reader.Read
            'If Data in database (reader) is greater than equal or less than equal to variable above
            If reader(2) >= thrdshft And reader(3) <= thrdshft2 Then
                If shift > reader(2) Or shift < reader(3) Then
                    readerVar = reader(1).ToString
                    Disconnect()
                    Return readerVar
                    Exit Function
                ElseIf shift < reader(3) Then
                    readerVar = reader(1).ToString
                    Disconnect()
                    Return readerVar
                    Exit Function

                End If
            ElseIf shift > reader(2) Then
                If shift < reader(3) Then
                    readerVar = reader(1).ToString
                    Disconnect()
                    Return readerVar
                    Exit Function
                End If
            End If
        End While
        Disconnect()
        Return shift
    End Function
    '######################################### End of Shift Function #########################################

    Private Sub btnResend_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnResend.ItemClick
        With My.Settings
            If .SQLConnection = True Then
                UpdateResultiHOMIS()
            End If
        End With

        With My.Settings
            If .HL7Write = True Then
                CreateHL7File()
            End If
        End With
    End Sub

#Region "HIS Integration Methods"
    'Method for Creating HL7 File

    Private his_test_code As String
    Public Function getHIS_Test(ByVal accesion_no As String, or_no As String, ByVal cs_no As String)
        Dim additional_info_id As String
        Dim his_test As String
        Dim test_string As String

        rs.Parameters.Clear()
        rs.Parameters.AddWithValue("@accession_no", accesion_no)
        rs.Parameters.AddWithValue("@or_no", or_no)
        rs.Parameters.AddWithValue("@ChargeSlip", cs_no)
        rs.Parameters.AddWithValue("@section", Section)
        rs.Parameters.AddWithValue("@sub_section", cboRequest.Text)

        Disconnect()
        Connect()
        rs.Connection = conn
        rs.CommandText = "SELECT id FROM additional_info WHERE accession_no = @accession_no AND or_no = @or_no AND cs_no = @ChargeSlip AND section = @section AND sub_section = @sub_section"
        reader = rs.ExecuteReader
        If reader.HasRows Then
            While reader.Read
                test_string = ""
                additional_info_id = reader(0).ToString
                Connect1()
                rs1.Connection = conn1
                rs1.CommandText = "SELECT his_test_code FROM hl7_result_his_code WHERE additional_info_id = '" & additional_info_id & "'"
                reader1 = rs1.ExecuteReader
                If reader1.HasRows Then
                    While reader1.Read
                        his_test = reader1(0).ToString + "^~"
                        test_string &= his_test
                    End While
                End If
                Disconnect1()
            End While
        End If
        Disconnect()

        Return test_string
    End Function

    Public Sub CreateHL7File()
        'Check If Date of Birth Is nothing then clear text formating
        If dtBDate.Text = "" Then
            'Clear text formating
            PatientBDate_Out = ""
            '--------------------
        Else
            'Create a format for date of birth (yyyyMMdd)
            Dim dDate As DateTime = DateTime.ParseExact(dtBDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture)
            Dim reformatted As String = dDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
            PatientBDate_Out = reformatted
            '--------------------------------------------
        End If
        '-----------------------------------------------------------
        Dim a As Integer = 0

        Dim HL7Log As FileStream
        Dim HL7Writer As StreamWriter
        Dim HL7File As String = "SBSILIS_" & txtSampleID.Text.Trim & ".HL7"

        HL7Log = New FileStream(My.Settings.HL7Destination & "\" & HL7File, FileMode.Create, FileAccess.Write)
        HL7Writer = New StreamWriter(HL7Log)

        his_test_code = getHIS_Test(txtAccession.Text, txtORNo.Text, txtChargeSlip.Text)

        HL7Writer.WriteLine("MSH|^~\&|BIZBOX|HOSPITAL|BIOTECH|HOSPITAL|" & FormatDateRegex() & "||ORU^RO1|1|P|2.3|||")
        HL7Writer.WriteLine("PID|1||" & txtPatientID.Text & "||" & Replace(txtName.Text, ", ", "^") & "||" & PatientBDate_Out & "|" & cboSex.Text.Substring(0, 1))
        HL7Writer.WriteLine("PV1|1|" & cboPatientType.Text.Substring(0, 1))
        HL7Writer.WriteLine("ORC|1|" & txtAccession.Text & "|||||||" & FormatDateRegex())
        'HL7Writer.WriteLine("OBR|1|" & txtAccession.Text & "||^|||" & FormatDateRegex())

        For x As Integer = 0 To his_test_code.Split("~").Count - 1 Step 1
            HL7Writer.WriteLine("OBR|1|" & txtAccession.Text & "||" & his_test_code.Split("~").GetValue(x).ToString & "|||" & FormatDateRegex())
        Next

        For x As Integer = 0 To GridView.RowCount - 1 Step 1
            'HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "SI") & "|" & GridView.GetRowCellValue(x, "Unit") & "|" & GridView.GetRowCellValue(x, "ReferenceRange") & "|N|||F|||" & FormatDateRegex() & "|")
            HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "Result") & "|||N|||F|||" & FormatDateRegex() & "|")
            a = x + 1
        Next
        'Remarks Section
        HL7Writer.WriteLine("OBX|" & a + 1 & "|NM|Remarks_U||" & txtRemarks.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        HL7Writer.WriteLine("OBX|" & a + 2 & "|NM|Others_U||" & txtComment.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        HL7Writer.Close()
        HL7Log.Close()
    End Sub
    ' End Of HL7

    'Function for update result for iHOMIS
    Public Sub UpdateResultiHOMIS()
        Try
            For x = 0 To GridView.RowCount - 1 Step 1
                Dim SQL As String = "UPDATE hchemres SET " & GridView.GetRowCellValue(x, GridView.Columns("HISTestCode")) & " = '" & GridView.GetRowCellValue(x, GridView.Columns("Result")) & "'" _
                            & " WHERE docointkey = @docointkey"

                ConnectSQL()
                rsSQL.Parameters.Clear()
                rsSQL.Parameters.AddWithValue("@docointkey", GridView.GetRowCellValue(x, GridView.Columns("HISMainID")))
                rsSQL.Connection = connSQL
                rsSQL.CommandType = CommandType.Text
                rsSQL.CommandText = SQL
                rsSQL.ExecuteNonQuery()
                DisconnectSQL()
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            Exit Sub
        End Try
    End Sub
    'End iHOMIS

#End Region

End Class