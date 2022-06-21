Imports System.IO
Imports System.Globalization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors

Public Class frmAntigenOrdered
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

    Private Sub cboMedTech_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboMedTech.SelectedIndexChanged, cboMedTech.TextChanged
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT ID, NAME FROM (SELECT `id` AS ID, CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech`) AS T1 WHERE T1.`name` = '" & Me.cboMedTech.Text & "'"
        reader = rs.ExecuteReader
        reader.Read()
        If reader.HasRows Then
            MedTechID = reader(0).ToString
        End If
        Disconnect()
    End Sub

    Private Sub cboPathologist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboPathologist.SelectedIndexChanged, cboPathologist.TextChanged
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT * FROM `viewPathologist` WHERE `name` = '" & Me.cboPathologist.Text & "'"
        reader = rs.ExecuteReader
        reader.Read()
        If reader.HasRows Then
            PathologistID = reader(0).ToString
        End If
        Disconnect()
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

    'Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click
    '    frmAntigenPrevious.patientID = txtPatientID.Text
    '    frmAntigenPrevious.section = Section
    '    frmAntigenPrevious.SubSection = SubSection
    '    frmAntigenPrevious.ShowDialog()
    'End Sub

    Public Sub LoadTest()
        Try
            GridView.Columns.Clear()
            GridView.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridView.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            'MessageBox.Show(Section)
            'MessageBox.Show(SubSection)

            Dim SQL As String = "SELECT `universal_id` AS TestName, `flag` AS Flag, `measurement` AS SI, `units` as Unit,
                `reference_range` as ReferenceRange, `value_conv` AS Conventional, `unit_conv` AS ConvUnit, 
                `ref_conv` AS ConvRefRange,  `instrument` AS Instrument, `test_code` AS TestCode, `id` AS ID, 
                `test_group` AS TestGroup, `his_code` AS HISTestCode, `his_mainid` AS HISMainID, `print_status` AS PrintStatus FROM `result` 
                WHERE `sample_id` = @MainID AND `section` = @Section AND `sub_section` = @SubSection ORDER BY `order_no`"

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

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtResult.DataSource = myTable

            GridView.Columns("TestCode").Visible = False
            GridView.Columns("Flag").Visible = False
            GridView.Columns("ID").Visible = False
            GridView.Columns("HISTestCode").Visible = False
            GridView.Columns("HISMainID").Visible = False
            GridView.Columns("TestGroup").Visible = False
            GridView.Columns("PrintStatus").Visible = False
            GridView.Columns("ReferenceRange").Visible = False
            GridView.Columns("Conventional").Visible = False
            GridView.Columns("ConvUnit").Visible = False
            GridView.Columns("ConvRefRange").Visible = False
            GridView.Columns("Unit").Visible = False

            'Version 1.6.0.0-beta
            'Not allow edit on Grid View Columns to prevent it to display on Results Form or cause of error
            GridView.Columns("TestName").OptionsColumn.AllowEdit = False
            GridView.Columns("SI").OptionsColumn.AllowEdit = True

            'Custom Column Name
            'GridView.Columns("SI").Caption = "Final Result"
            'GridView.Columns("Units").Caption = "Unit"
            'GridView.Columns("RefRange").Caption = "Reference Range"

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
                    GridView.SelectRow(x)
                End If
            Next

            'GridView.Columns("TestGroup").Group()
            'GridView.Columns("TestGroup").Caption = " "

            CreateDropdown()
            FillDropdown()

            LoadRangeAndValues()

            'DiffCount()
        Finally
            If conn.State = ConnectionState.Open Then
                conn.Close()
            End If
        End Try

    End Sub

    Private test As New Hashtable()

    Private Sub FillDropdown()
        On Error Resume Next

        Dim Antigen() As String = {
            "",
            "POSITIVE",
            "NEGATIVE"
            }
        test.Add("Antigen", Antigen)
        'Dim Btype() As String = {
        '    ControlChars.Quote & "A" & ControlChars.Quote & "Rh POSITIVE",
        '    ControlChars.Quote & "B" & ControlChars.Quote & "Rh POSITIVE",
        '    ControlChars.Quote & "AB" & ControlChars.Quote & "Rh POSITIVE",
        '    ControlChars.Quote & "O" & ControlChars.Quote & "Rh POSITIVE",
        '    ControlChars.Quote & "A" & ControlChars.Quote & "Rh NEGATIVE",
        '    ControlChars.Quote & "B" & ControlChars.Quote & "Rh NEGATIVE",
        '    ControlChars.Quote & "AB" & ControlChars.Quote & "Rh NEGATIVE",
        '    ControlChars.Quote & "O" & ControlChars.Quote & "Rh NEGATIVE"
        '    }
        'test.Add("BType", Btype)

    End Sub

    Private Sub GridView_ShownEditor(ByVal sender As Object, ByVal e As System.EventArgs) Handles GridView.ShownEditor
        On Error Resume Next
        Dim view As GridView = TryCast(sender, GridView)
        If view.FocusedColumn.FieldName = "SI" Then
            Dim edit As ComboBoxEdit = CType(view.ActiveEditor, ComboBoxEdit)
            edit.Properties.Items.Clear()

            Dim parameter As Object = view.GetRowCellValue(view.FocusedRowHandle, "TestCode")
            If test(parameter) IsNot Nothing Then
                edit.Properties.Items.AddRange(CType(test(parameter), ICollection))
            End If
        End If
    End Sub

    Dim WithEvents _riEditor As New RepositoryItemComboBox()

    Private Sub CreateDropdown()
        On Error Resume Next
        dtResult.RepositoryItems.Clear()
        dtResult.RepositoryItems.Add(_riEditor)
        GridView.Columns("SI").ColumnEdit = _riEditor
    End Sub

    Private Sub _riEditor_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles _riEditor.EditValueChanged
        GridView.FocusedColumn = GridView.Columns("SI")
        GridView.ShowEditor()

        Dim edit As ComboBoxEdit = GridView.ActiveEditor

        Dim value As Object = edit.SelectedItem

        For x As Integer = 0 To Me.GridView.RowCount - 1 Step 1
            If GridView.GetRowCellValue(x, GridView.Columns("TestCode")).ToString = "Antigen" Then
                GridView.SetRowCellValue(x, GridView.Columns("Conventional"), value)
            End If
        Next
    End Sub

    'Private Sub DiffCount()
    '    Dim TotalDIFCount As Decimal = 0
    '    Dim RowValue As Double = 0
    '    Try
    '        For x As Integer = 0 To Me.GridView.RowCount - 1 Step 1
    '            '#################################################----TOTAL DIF COUNT----################################################################################
    '            If GridView.GetRowCellValue(x, GridView.Columns("TestGroup")) = "Differential Count" Then
    '                If GridView.GetRowCellValue(x, "Conventional") Is "" Then
    '                Else
    '                    TotalDIFCount += GridView.GetRowCellValue(x, "Conventional")
    '                End If
    '            End If

    '            lblDiffCount.Text = "Total DIF Count:  " & TotalDIFCount

    '            If TotalDIFCount.ToString = 1 Or TotalDIFCount = 100 Then
    '                lblDiffCount.BackColor = Color.DeepSkyBlue
    '            Else
    '                lblDiffCount.BackColor = Color.Crimson
    '            End If

    '            If TotalDIFCount.ToString = 1 Or TotalDIFCount = 100 Then
    '                lblDiffCount.BackColor = Color.DeepSkyBlue
    '            Else
    '                lblDiffCount.BackColor = Color.Crimson
    '            End If
    '            '#################################################----TOTAL DIF COUNT----################################################################################
    '        Next
    '    Catch ex As Exception

    '    End Try
    'End Sub

    Public Sub LoadRangeAndValues()
        Try
            For x As Integer = 0 To Me.GridView.RowCount - 1 Step 1
                rs.Parameters.Clear()
                rs.Parameters.AddWithValue("@patient_id", txtPatientID.Text)
                rs.Parameters.AddWithValue("@TestCode", GridView.GetRowCellValue(x, GridView.Columns("TestCode")))
                rs.Parameters.AddWithValue("@classification", txtClass.Text)
                rs.Parameters.AddWithValue("@gender", cboSex.Text)
                rs.Parameters.AddWithValue("@age", txtAge.Text)
                rs.Parameters.AddWithValue("@main_id", mainID)
                rs.Parameters.AddWithValue("@instrument", GridView.GetRowCellValue(x, GridView.Columns("Instrument")))
                rs.Parameters.AddWithValue("@section", Section)

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
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub

    Private Sub LoadMedTechPatho()
        '###########################---Load Pathologist---################################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `pathologist` ORDER BY `name`"
        reader = rs.ExecuteReader
        While reader.Read
            cboPathologist.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        cboPathologist.SelectedIndex = 0
        '######################################----END-----###############################################################

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

        '###########################---Load Med Tech---##################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech` ORDER BY `fname`"
        reader = rs.ExecuteReader
        While reader.Read
            cboMedTech.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        '######################################----END-----###############################################################

        '###########################---Load Medical Technologist---#######################################################
        If My.Settings.MedTech = 0 Then
            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT ID, NAME FROM (SELECT `id` AS ID, CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech`) AS T1 WHERE T1.`name` = '" & Me.cboMedTech.Text & "'"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                MedTechID = reader(0).ToString
            End If
            Disconnect()
        ElseIf My.Settings.MedTech = 1 Then
            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT ID, NAME FROM (SELECT `id` AS ID, CONCAT(fname, ' ', mname, ' ', lname, ', ', designation) AS `name` FROM `medtech`) AS T1 WHERE T1.`name` = '" & Me.cboMedTech.Text & "'"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                MedTechID = reader(0).ToString
            End If
            Disconnect()
            'cboMedTech.SelectedIndex = 0
        End If
        '######################################----END-----###############################################################
    End Sub

    Private Sub frmResultsNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadMedTechPatho()
        'Load Combobox Data
        AutoLoadDoctor()
        AutoLoadRoom()
        AutoLoadMethodUsed()
        AutoLoadReagent()
        LoadTest()
    End Sub

    Public Sub AutoLoadMethodUsed()
        '#######################################---Load Method Used---####################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT DISTINCT method_used AS `MethodUsed` FROM `rat_lot_no` ORDER BY `method_used`"
        reader = rs.ExecuteReader
        While reader.Read
            txtMethodUsed.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        '######################################----END-----###############################################################
    End Sub

    Public Sub AutoLoadReagent()
        '########################################---Load Reagent---#######################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT DISTINCT reagent AS `Reagent` FROM `rat_lot_no` ORDER BY `reagent`"
        reader = rs.ExecuteReader
        While reader.Read
            txtReagent.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        '######################################----END-----###############################################################
    End Sub

    Public Sub AutoLoadLotNumber()
        '#######################################---Load Method Used---####################################################
        Connect()
        rs.Connection = conn
        rs.CommandType = CommandType.Text
        rs.CommandText = "SELECT DISTINCT lot_number AS `LotNumber` FROM `rat_lot_no` ORDER BY `lot_number`"
        reader = rs.ExecuteReader
        While reader.Read
            txtMethodUsed.Properties.Items.Add(reader(0))
        End While
        Disconnect()
        '######################################----END-----###############################################################
    End Sub

    'Private Sub GridView_CellValueChanged(sender As Object, e As CellValueChangedEventArgs) Handles GridView.CellValueChanged
    '    Try
    '        If e.Column.FieldName = "Result" Then
    '            LoadRangeAndValues()
    '        Else
    '            DiffCount()
    '        End If
    '    Catch
    '    End Try
    'End Sub

    'Private Sub GridView_CustomRowCellEditForEditing(sender As Object, e As CustomRowCellEditEventArgs) Handles GridView.CustomRowCellEditForEditing
    '    Try
    '        If e.Column.FieldName = "Result" Then
    '            LoadRangeAndValues()
    '        Else
    '            DiffCount()
    '        End If
    '    Catch
    '    End Try
    'End Sub

    'Private Sub btnAddTest_ItemClick(ByVal sender As Object, ByVal e As EventArgs) Handles btnAddTest.Click
    '    frmAntigenAddTest.mainID = mainID
    '    frmAntigenAddTest.patientID = txtPatientID.Text
    '    frmAntigenAddTest.TypeResult = "New"
    '    frmAntigenAddTest.SubSection = SubSection
    '    frmAntigenAddTest.Section = "Molecular"
    '    frmAntigenAddTest.ShowDialog()
    'End Sub

    'Private Sub txtAge_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cboSex.SelectedIndexChanged, cboSex.TextChanged, txtAge.TextChanged, txtClass.SelectedIndexChanged
    '    LoadRangeAndValues()
    'End Sub

    'Private Sub btnSearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    frmAntigenPatientList.Type = "New"
    '    frmAntigenPatientList.ShowDialog()
    'End Sub

    'Private Sub btnDelete_ItemClick(ByVal sender As Object, ByVal e As EventArgs)
    '    Try
    '        rs.Parameters.Clear()
    '        rs.Parameters.AddWithValue("@sample_id", mainID)
    '        'Dim rows As DataGridViewRow = dtResult.SelectedRows(0)

    '        'rs.Parameters.AddWithValue("@id", rows.Cells(11).Value)
    '        rs.Parameters.AddWithValue("@id", GridView.GetFocusedRowCellValue("ID").ToString)
    '        rs.Parameters.AddWithValue("@test_code", GridView.GetFocusedRowCellValue("TestCode").ToString)

    '        DeleteRecordSQL("DELETE FROM `tmpResult` WHERE sample_id = @sample_id AND `id` = @ID")
    '        'DeleteRecordSQL("DELETE FROM `lis_order` WHERE sample_id = @sample_id AND `order_code` = @test_code")
    '        'DeleteRecordSQL("DELETE FROM `patient_order` WHERE sample_id = @sample_id AND `id` = @ID")
    '        LoadTest()
    '    Catch ex As Exception
    '        MessageBox.Show("No Records Selected!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
    '    End Try

    'End Sub

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
        'GetBDate()
    End Sub

    Private Sub btnValidate_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnValidate.ItemClick
        GroupControl1.Enabled = True
        GroupControl2.Enabled = True
        GroupControl3.Enabled = True
        GroupControl5.Enabled = True
        GroupControl7.Enabled = True
        btnAddTest.Enabled = True
        txtRemarks.Enabled = True
        txtComment.Enabled = True
        txtMethodUsed.Enabled = True
        txtReagent.Enabled = True
        txtLotNumber.Enabled = True
        txtExpiry.Enabled = True

        btnValidate.Enabled = False
        btnPrint.Enabled = True
        btnViewPrint.Enabled = False
    End Sub

    Private Sub btnPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrint.ItemClick
        Try
            If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                'MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'Exit Sub
                If Me.txtName.Text = "" Then
                    MessageBox.Show("Please Fill Up Patient Name First.", "Patient Name is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ElseIf txtAge.Text = "" Then
                    MessageBox.Show("Please Fill Up Patient Age First.", "Patient Age is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ElseIf cboSex.Text = "" Then
                    MessageBox.Show("Please Fill Up Patient Name First.", "Patient Sex is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                ElseIf Me.cboMedTech.Text = "" Then
                    MessageBox.Show("Please Fill Up MedTech Field Signatory First.", "MedTech Field Signatory is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Else
                    MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If
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
            rs.Parameters.AddWithValue("@medtechid", MedTechID)
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
            rs.Parameters.AddWithValue("@date_release", Format(Now, "yyyy-MM-dd") & " " & tmTimeReleased.Value.ToLongTimeString)
            rs.Parameters.AddWithValue("@accession_no", txtAccession.Text)
            rs.Parameters.AddWithValue("@OR_No", txtORNo.Text)
            rs.Parameters.AddWithValue("@CS_No", txtChargeSlip.Text)
            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            rs.Parameters.AddWithValue("@MethodUsed", txtMethodUsed.Text)
            rs.Parameters.AddWithValue("@Reagent", txtReagent.Text)
            rs.Parameters.AddWithValue("@LotNumber", txtLotNumber.Text)
            rs.Parameters.AddWithValue("@Expiry", txtExpiry.Text)

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT * FROM patient_info WHERE `patient_id` = @PATIENT_ID"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Disconnect()
                SaveRecordwthoutMSG("UPDATE `patient_info` SET " _
                        & "`patient_id` = @patient_id," _
                        & "`name` = @name," _
                        & "`sex` = @sex," _
                        & "`date_of_birth` = @bdate," _
                        & "`age` = @age," _
                        & "`civil_status` = @CS," _
                        & "`address` = @address," _
                        & "`contact_no` = @contact," _
                        & "`sample_id` = @MainSampleID" _
                        & " WHERE `patient_id` = @patient_id"
                        )
            Else
                Disconnect()
                SaveRecordwthoutMSG("INSERT INTO patient_info (patient_id, name, sex, date_of_birth, age, civil_status, address, contact_no, `date`, `dt_released`,`sample_id`) VALUES " _
                        & "(" _
                        & "@patient_id," _
                        & "@name," _
                        & "@sex," _
                        & "@bdate," _
                        & "@age," _
                        & "@CS," _
                        & "@address," _
                        & "@contact," _
                        & "@date," _
                        & "@MainSampleID" _
                        & ")"
                        )
            End If
            Disconnect()

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
                    & "`medtech` = @medtechid," _
                    & "`verified_by` = @medtechid," _
                    & "`test` = @test," _
                    & "`patient_type` = @patient_type," _
                    & "`status` = @status," _
                    & "`date` = @date," _
                    & "`time` = @time," _
                    & "`dt_released` = @date_release," _
                    & "`main_id` = @MainSampleID" _
                    & " WHERE main_id = @mainID AND testtype = @Section AND sub_section = @SubSection"
                    )

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT * FROM `order_pathologist` WHERE `sample_id` = @mainID"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Disconnect()
                UpdateRecordwthoutMSG("UPDATE `order_pathologist` SET " _
                        & "`pathologist_id` = @pathologist," _
                        & "`sample_id` = @MainSampleID" _
                        & " WHERE `sample_id` = @mainID"
                        )
            Else
                Disconnect()
                SaveRecordwthoutMSG("INSERT INTO `order_pathologist` (`sample_id`, `pathologist_id`) VALUES " _
                        & "(" _
                        & "@MainSampleID," _
                        & "@pathologist" _
                        & ")"
                        )
            End If
            Disconnect()

            If Not cboMedTech.Text = "" Then
                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT * FROM `order_medtech` WHERE `sample_id` = @mainID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `order_medtech` SET " _
                            & "`medtech_id` = @medtechid," _
                            & "`sample_id` = @MainSampleID" _
                            & " WHERE `sample_id` = @mainID"
                            )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `order_medtech` (`sample_id`, `medtech_id`) VALUES " _
                            & "(" _
                            & "@MainSampleID," _
                            & "@medtechid" _
                            & ")"
                            )
                End If
                Disconnect()
            End If

            If Not cboVerify.Text = "" Then
                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT * FROM `order_Verified` WHERE `sample_id` = @mainID"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `order_Verified` SET " _
                            & "`medtech_id` = @verifyid," _
                            & "`sample_id` = @MainSampleID" _
                            & " WHERE `sample_id` = @mainID"
                            )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `order_Verified` (`sample_id`, `medtech_id`) VALUES " _
                            & "(" _
                            & "@MainSampleID," _
                            & "@verifyid" _
                            & ")"
                            )
                End If
                Disconnect()
            End If

            If Not txtAccession.Text = "" Or txtORNo.Text = "" Or txtChargeSlip.Text = "" Then
                Connect()
                rs.Connection = conn
                rs.CommandType = CommandType.Text
                rs.CommandText = "SELECT * FROM `additional_info` WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
                reader = rs.ExecuteReader
                reader.Read()
                If reader.HasRows Then
                    Disconnect()
                    UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                                & "`accession_no` = @accession_no," _
                                & "`or_no` = @OR_No," _
                                & "`cs_no` = @CS_No," _
                                & "`sample_id` = @MainSampleID" _
                                & " WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
                                )
                Else
                    Disconnect()
                    SaveRecordwthoutMSG("INSERT INTO `additional_info` (`accession_no`, `or_no`, `cs_no`, section, sub_section, `sample_id`) VALUES " _
                                & "(" _
                                & "@accession_no," _
                                & "@OR_No," _
                                & "@CS_No," _
                                & "@Section," _
                                & "@SubSection," _
                                & "@MainSampleID" _
                                & ")"
                                )
                End If
                Disconnect()
            End If

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT * FROM `rat_assay_info` WHERE `sample_id` = @mainID"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Disconnect()
                UpdateRecordwthoutMSG("UPDATE `rat_assay_info` SET " _
                            & "`method_used` = @MethodUsed," _
                            & "`reagent` = @Reagent," _
                            & "`lot_number` = @LotNumber," _
                            & "`expiry` = @Expiry" _
                            & " WHERE `sample_id` = @MainSampleID"
                            )
            Else
                Disconnect()
                SaveRecordwthoutMSG("INSERT INTO `rat_assay_info` (`method_used`, `reagent`, `lot_number`, `expiry`, `sample_id`) VALUES " _
                            & "(" _
                            & "@MethodUsed," _
                            & "@Reagent," _
                            & "@LotNumber," _
                            & "@Expiry," _
                            & "@MainSampleID" _
                            & ")"
                            )
            End If
            Disconnect()

            For x As Integer = 0 To GridView.RowCount - 1 Step 1
                If (GridView.IsRowSelected(x)) Then
                    UpdateRecordwthoutMSG("UPDATE `result` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "ConvRefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '1'," _
                          & "`sample_id` = @MainSampleID" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "'"
                          )
                Else
                    UpdateRecordwthoutMSG("UPDATE `result` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "ConvRefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '0'," _
                          & "`sample_id` = @MainSampleID" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "'"
                          )
                End If
            Next

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "SELECT `sample_id` FROM `patient_remarks` WHERE `sample_id` = @mainID"
            reader = rs.ExecuteReader
            reader.Read()
            If reader.HasRows Then
                Disconnect()
                UpdateRecordwthoutMSG("UPDATE `patient_remarks` SET `remarks` = @remarks, `diagnosis` = @lab_comment WHERE `sample_id` = @mainID AND `section` = @Section AND `sub_section` = @SubSection")
            Else
                Disconnect()
                SaveRecordwthoutMSG("INSERT INTO `patient_remarks` (`remarks`, `diagnosis`, `sample_id`, `section`, `sub_section`) VALUES (@remarks, @lab_comment, @MainSampleID, @Section, @SubSection)")
            End If
            Disconnect()

            'UpdateWorkSheet()

            GroupControl1.Enabled = False
            GroupControl2.Enabled = False
            GroupControl3.Enabled = False
            GroupControl4.Enabled = False
            GroupControl5.Enabled = False
            GroupControl7.Enabled = False

            txtRemarks.Enabled = False
            txtComment.Enabled = False
            txtMethodUsed.Enabled = False
            txtReagent.Enabled = False
            txtLotNumber.Enabled = False
            txtExpiry.Enabled = False

            btnPrint.Enabled = False
            btnValidate.Enabled = True
            btnPreview.Enabled = True
            btnViewPrint.Enabled = True
            btnPrintNow.Enabled = True

            mainID = MainSampleID
            'frmChemWorklist.LoadRecordsCompleted()

        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

    'Added 02192021
    'Update Button release no bypass printing
    Private Sub btnRelease_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnRelease.ItemClick
        With My.Settings
            If .AuthenticateRelease = True Then
                frmAuth.Section = "Molecular"
                frmAuth.Method = "Release"
                frmAuth.ShowDialog()
            Else
                'ReleaseResultNoPrint()
            End If
        End With
    End Sub

    'Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
    '    frmAntigenRerun.mainID = mainID
    '    frmAntigenRerun.Section = Section
    '    frmAntigenRerun.SubSection = SubSection
    '    frmAntigenRerun.ShowDialog()
    'End Sub

    Private Sub btnPrintNow_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrintNow.ItemClick
        Try
            If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                'MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                'Exit Sub
                If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                    If Me.txtName.Text = "" Then
                        MessageBox.Show("Please Fill Up Patient Name First.", "Patient Name is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ElseIf txtAge.Text = "" Then
                        MessageBox.Show("Please Fill Up Patient Age First.", "Patient Age is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ElseIf cboSex.Text = "" Then
                        MessageBox.Show("Please Fill Up Patient Name First.", "Patient Sex is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    ElseIf Me.cboMedTech.Text = "" Then
                        MessageBox.Show("Please Fill Up MedTech Field Signatory First.", "MedTech Field Signatory is Empty", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                    Exit Sub
                End If
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

            'frmAntigenWorklist.LoadRecords()
            'frmAntigenWorklist.LoadRecordsCompleted()

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

        If GridView.GetRowCellValue(e.RowHandle, "Flag").ToString = "L" Then
            e.Appearance.BackColor = Color.Yellow
            e.Appearance.BackColor2 = Color.Yellow
            e.Appearance.ForeColor = Color.Black
        ElseIf GridView.GetRowCellValue(e.RowHandle, "Flag").ToString = "H" Then
            e.Appearance.BackColor = Color.Crimson
            e.Appearance.BackColor2 = Color.Crimson
            e.Appearance.ForeColor = Color.White
        End If
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
        rs.Parameters.AddWithValue("@section", "Molecular")
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
        'For x As Integer = 0 To GridView.RowCount - 1 Step 1
        '    'HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "SI") & "|" & GridView.GetRowCellValue(x, "Unit") & "|" & GridView.GetRowCellValue(x, "ReferenceRange") & "|N|||F|||" & FormatDateRegex() & "|")
        '    HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "Conventional") & "|||N|||F|||" & FormatDateRegex() & "|")
        '    a = x + 1
        'Next
        'Remarks Section
        HL7Writer.WriteLine("OBX|" & a + 1 & "|NM|Remarks_H||" & txtRemarks.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        HL7Writer.WriteLine("OBX|" & a + 2 & "|NM|Others_H||" & txtComment.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        HL7Writer.Close()
        HL7Log.Close()
    End Sub
    'End Of HL7

    'Function for update result for iHOMIS
    'Public Sub UpdateResultiHOMIS()
    '    Try
    '        For x = 0 To GridView.RowCount - 1 Step 1
    '            Dim SQL As String = "UPDATE hbldrslt SET " & GridView.GetRowCellValue(x, GridView.Columns("HISTestCode")) & " = '" & GridView.GetRowCellValue(x, GridView.Columns("Result")) & "'" _
    '                        & " WHERE docointkey = @docointkey"

    '            ConnectSQL()
    '            rsSQL.Parameters.Clear()
    '            rsSQL.Parameters.AddWithValue("@docointkey", GridView.GetRowCellValue(x, GridView.Columns("HISMainID")))
    '            rsSQL.Connection = connSQL
    '            rsSQL.CommandType = CommandType.Text
    '            rsSQL.CommandText = SQL
    '            rsSQL.ExecuteNonQuery()
    '            DisconnectSQL()
    '        Next
    '    Catch ex As Exception
    '        MessageBox.Show(ex.Message)
    '        Exit Sub
    '    End Try
    'End Sub
    'End iHOMIS
#End Region
End Class