Imports System.IO
Imports System.Globalization
Imports DevExpress.XtraGrid.Views.Grid
Imports DevExpress.XtraGrid.Views.Base
Imports DevExpress.XtraEditors.Repository
Imports DevExpress.XtraEditors

Public Class frmAntigenNew
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
        rs.CommandText = "SELECT * FROM `viewMedTEch` WHERE `name` = '" & Me.cboMedTech.Text & "'"
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
        rs.CommandText = "SELECT * FROM `viewMedTEch` WHERE `name` = '" & Me.cboVerify.Text & "'"
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

            Dim SQL As String = "SELECT `universal_id` AS TestName, `flag` AS Flag, `measurement` AS SI, `units` as Unit,
                `reference_range` as ReferenceRange, `value_conv` AS Conventional, `unit_conv` AS ConvUnit, 
                `ref_conv` AS ConvRefRange,  `instrument` AS Instrument, `test_code` AS TestCode, `id` AS ID, 
                `test_group` AS TestGroup, `his_code` AS HISTestCode, `his_mainid` AS HISMainID, `print_status` AS PrintStatus FROM `tmpresult` 
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
        test.Add("RAT", Antigen)
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

    Private Sub frmResultsNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'TODO: This line of code loads data into the 'Db_sbsi_lis_universalDataSet.rat_lot_no' table. You can move, or remove it, as needed.

        Me.cboPathologist.Text = Pathologist
    End Sub

    Private Sub frmAntigenNew_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        AutoLoadDoctor()
        AutoLoadRoom()
        AutoLoadVerificator()

    End Sub

    Private Sub frmAntigenNew_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        LoadTest()
        ReloadAssay()
    End Sub

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

    Dim LotID As String = ""
    'Public Sub AutoLoadLot()
    '    '###########################---Load Lot No---##################################################
    '    Connect()
    '    rs.Connection = conn
    '    rs.CommandType = CommandType.Text
    '    rs.CommandText = "SELECT id, lot_number, expiry_date FROM rat_lot_no"
    '    reader = rs.ExecuteReader
    '    While reader.Read
    '        'txtLotNumber.Properties.Items.Add(reader(1))
    '        LUELotNumber.Properties.Columns.Add()
    '        LUELotNumber.Properties.Columns.Add(reader(1).ToString)
    '        LUELotNumber.Properties.Columns.Add(reader(2).ToStrings)
    '    End While
    '    Disconnect()
    '    '######################################----END-----###############################################################
    'End Sub

    Private Sub LUELotNumber_TextChanged(sender As Object, e As EventArgs) Handles txtLotNumber.TextChanged
        txtExpiry.Text = txtLotNumber.GetColumnValue(txtLotNumber.Properties.Columns.Item(2))
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

            If totalDays <= 31 Then
                txtClass.Text = "Day(s)"
                txtAge.Text = totalDays

            ElseIf totalDays >= 32 And totalDays <= 364 Then
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
        With My.Settings
            If .AuthenticateRelease = True Then
                frmAuth.Section = "Molecular"
                frmAuth.Method = "Verify"
                frmAuth.ShowDialog()
            Else
                VerifyResult()
            End If
        End With
    End Sub

    Private Sub btnPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnPrint.ItemClick
        With My.Settings
            If .AuthenticateRelease = True Then
                frmAuth.Section = "Molecular"
                frmAuth.Method = "PrintRelease"
                frmAuth.ShowDialog()
            Else
                PrintReleaseResult()
            End If
        End With
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
                ReleaseResultNoPrint()
            End If
        End With
    End Sub

    'Private Sub btnRetrive_Click(sender As Object, e As EventArgs) Handles btnRetrive.Click
    '    frmAntigenRerun.mainID = mainID
    '    frmAntigenRerun.Section = Section
    '    frmAntigenRerun.SubSection = SubSection
    '    frmAntigenRerun.ShowDialog()
    'End Sub

    Private Sub btnViewPrint_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles btnViewPrint.ItemClick
        'For Each rows As DataGridViewRow In GridView.rows
        RPTresults.sample_id = mainID
        PrintPreview(mainID, "tmpworklist", "tmpresult", 1, Section, SubSection, RPTresults, RPTresults.ReportViewer1)
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
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        frmAssyInfo.ShowDialog()
    End Sub

    Public Sub ReloadAssay()
        txtLotNumber.Properties.Columns.Clear()
        'txtLotNumber.Properties.DataSource = Db_sbsi_lis_universalDataSet.rat_lot_no
        'txtLotNumber.Properties.DisplayMember = "lot_number"
        'txtLotNumber.Properties.ValueMember = "id"

        'Me.Rat_lot_noTableAdapter.Fill(Me.Db_sbsi_lis_universalDataSet.rat_lot_no)

        Dim SQL As String = "SELECT `id` AS ID, `lot_number` AS LotNo, `expiry_date` AS ExpiryDate FROM `rat_lot_no` "

        Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

        Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

        Dim myTable As DataTable = New DataTable
        adapter.Fill(myTable)

        txtLotNumber.Properties.DataSource = myTable
        txtLotNumber.Properties.DisplayMember = "LotNo"
        txtLotNumber.Properties.ValueMember = "ID"

    End Sub

#Region "Procedure for Release"
    Public Sub ReleaseResultNoPrint()
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

            With My.Settings
                If .LISType = "Lite" Then
                    MainSampleID = ID_Randomizer()
                Else
                    MainSampleID = mainID
                End If
            End With

            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@mainID", mainID)
            rs.Parameters.AddWithValue("@MainSampleID", MainSampleID)

            rs.Parameters.AddWithValue("@medtech", cboMedTech.Text)
            rs.Parameters.AddWithValue("@medtechid", MedTechID)

            If My.Settings.AuthenticateRelease = True Then
                rs.Parameters.AddWithValue("@verifyid", UserIDRelease)
                rs.Parameters.AddWithValue("@verify", UserRelease)
            Else
                rs.Parameters.AddWithValue("@verifyid", VerifyID)
                rs.Parameters.AddWithValue("@verify", cboVerify.Text)
            End If

            rs.Parameters.AddWithValue("@pathologist", PathologistID)

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
            rs.Parameters.AddWithValue("@accession_no", txtAccession.Text)
            rs.Parameters.AddWithValue("@OR_No", txtORNo.Text)
            rs.Parameters.AddWithValue("@CS_No", txtChargeSlip.Text)

            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            rs.Parameters.AddWithValue("@MethodUsed", txtMethodUsed.Text)
            rs.Parameters.AddWithValue("@Reagent", txtReagent.Text)
            rs.Parameters.AddWithValue("@LotNumber", txtLotNumber.Text)
            rs.Parameters.AddWithValue("@Expiry", txtExpiry.Text)

            rs.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtReceived.Value, "yyyy-MM-dd")
            rs.Parameters.AddWithValue("@time", tmTimeReceived.Value.ToLongTimeString)
            rs.Parameters.AddWithValue("@date_release", Format(Now, "yyyy-MM-dd") & " " & Now.ToLongTimeString)
            rs.Parameters.AddWithValue("@specimen_tracking_date_time", Now)
            rs.Parameters.AddWithValue("@specimen_tracking_received", Format(dtReceived.Value, "yyyy-MM-dd") & " " & tmTimeReceived.Value.ToLongTimeString)

            rs.Parameters.AddWithValue("@PDFLocation", CreateFolder(Section))


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

            UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
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
                    & "`verified_by` = @verifyid," _
                    & "`test` = @test," _
                    & "`patient_type` = @patient_type," _
                    & "`status` = @status," _
                    & "`dt_released` = @date_release," _
                    & "`main_id` = @MainSampleID" _
                    & " WHERE `main_id` = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )


            UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                            & "`accession_no` = @accession_no," _
                            & "`or_no` = @OR_No," _
                            & "`cs_no` = @CS_No," _
                            & "`sample_id` = @MainSampleID" _
                            & " WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
                            )

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
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '1'," _
                          & "`date` = @date_release," _
                          & "`sample_id` = @MainSampleID" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "'"
                          )
                Else
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '0'," _
                          & "`date` = @date_release," _
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

            'version 1.6.0.0-beta
            'Save the Processed, Validated and Printed Time on 'specimen_tracking' table
            UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                & "`sample_id` = @MainSampleID," _
                & "`received` = @specimen_tracking_received," _
                & "`processing` = @specimen_tracking_date_time," _
                & "`validated` = @specimen_tracking_date_time," _
                & "`printed` = @specimen_tracking_date_time" _
                & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @Subsection"
                )

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `order` (`status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, `main_id`, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section`) " _
                & "SELECT `status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, @MainSampleID, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section` FROM `tmpWorklist` WHERE `main_id` = @MainSampleID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `result` (`universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, `sample_id`, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status`) " _
                & "SELECT `universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, @MainSampleID, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status` FROM `tmpResult` WHERE `sample_id` = @MainSampleID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpWorklist` WHERE `main_id` = @MainSampleID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpResult` WHERE `sample_id` = @MainSampleID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Using myRDLCPrinter As New RDLCPrinterPrintNew(MainSampleID, Section, SubSection, 1, My.Settings.DefaultPrinter)
                If My.Settings.SaveAsPDF Then
                    Dim byteViewer As Byte() = myRDLCPrinter.LocalReport.Render("PDF")
                    Dim saveFileDialog1 As New SaveFileDialog()
                    saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
                    saveFileDialog1.FilterIndex = 2
                    saveFileDialog1.RestoreDirectory = True
                    Dim newFile As New FileStream(CreateFolder(Section) & txtSampleID.Text & "_" & txtName.Text & ".pdf", FileMode.Create)
                    newFile.Write(byteViewer, 0, byteViewer.Length)
                    newFile.Close()

                    '------------------Save Email Details------------------------------
                    Connect()
                    rs.Connection = conn
                    rs.CommandType = CommandType.Text
                    rs.CommandText = ("INSERT INTO `pdf_location` (`sample_id`, `pdf_location`, `section`, `sub_section`) VALUES " _
                            & "(" _
                            & "@mainID," _
                            & "@PDFLocation," _
                            & "@Section," _
                            & "@SubSection" _
                            & ")"
                            )
                    rs.ExecuteNonQuery()
                    Disconnect()
                    '------------------Save Email Details------------------------------
                Else

                End If
            End Using

            With My.Settings
                If .HL7Write = True Then
                    CreateHL7File()
                End If
            End With

            'With My.Settings
            '    If .SQLConnection = True Then
            '        UpdateResultiHOMIS()
            '    End If
            'End With

            With My.Settings
                If .AutoEmail = True Then
                    sendmail(txtEmail.Text,
                             txtName.Text,
                             CreateFolder(Section) & mainID & "_" & txtName.Text & ".PDF")

                    '------------------Save Email Details------------------------------
                    Connect()
                    rs.Connection = conn
                    rs.CommandType = CommandType.Text
                    rs.CommandText = ("UPDATE `email_details` SET `status` = 1 WHERE `sample_id` = @MainSampleID AND `section` = @Section AND `sub_section` = @SubSection")
                    rs.ExecuteNonQuery()
                    Disconnect()
                    '------------------Save Email Details------------------------------
                End If
            End With

            ActivityLogs(txtSampleID.Text,
                         txtPatientID.Text,
                         txtName.Text,
                         CurrUser,
                         "Results Released",
                         cboRequest.Text,
                         Section,
                         SubSection)

            frmMolWorklist.LoadRecords()

            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

    Public Sub PrintReleaseResult()
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

            With My.Settings
                If .LISType = "Lite" Then
                    MainSampleID = ID_Randomizer()
                Else
                    MainSampleID = mainID
                End If
            End With

            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@mainID", mainID)
            rs.Parameters.AddWithValue("@MainSampleID", MainSampleID)

            rs.Parameters.AddWithValue("@medtech", cboMedTech.Text)
            rs.Parameters.AddWithValue("@medtechid", MedTechID)

            If My.Settings.AuthenticateRelease = True Then
                rs.Parameters.AddWithValue("@verifyid", UserIDRelease)
                rs.Parameters.AddWithValue("@verify", UserRelease)
            Else
                rs.Parameters.AddWithValue("@verifyid", VerifyID)
                rs.Parameters.AddWithValue("@verify", cboVerify.Text)
            End If

            rs.Parameters.AddWithValue("@pathologist", PathologistID)

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
            rs.Parameters.AddWithValue("@accession_no", txtAccession.Text)
            rs.Parameters.AddWithValue("@OR_No", txtORNo.Text)
            rs.Parameters.AddWithValue("@CS_No", txtChargeSlip.Text)

            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            rs.Parameters.AddWithValue("@MethodUsed", txtMethodUsed.Text)
            rs.Parameters.AddWithValue("@Reagent", txtReagent.Text)
            rs.Parameters.AddWithValue("@LotNumber", txtLotNumber.Text)
            rs.Parameters.AddWithValue("@Expiry", txtExpiry.Text)

            rs.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtReceived.Value, "yyyy-MM-dd")
            rs.Parameters.AddWithValue("@time", tmTimeReceived.Value.ToLongTimeString)
            rs.Parameters.AddWithValue("@date_release", Format(Now, "yyyy-MM-dd") & " " & Now.ToLongTimeString)
            rs.Parameters.AddWithValue("@specimen_tracking_date_time", Now) 'version 1.6.0.0-beta
            rs.Parameters.AddWithValue("@specimen_tracking_received", Format(dtReceived.Value, "yyyy-MM-dd") & " " & tmTimeReceived.Value.ToLongTimeString)

            rs.Parameters.AddWithValue("@PDFLocation", CreateFolder(Section))


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

            UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
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
                    & "`verified_by` = @verifyid," _
                    & "`test` = @test," _
                    & "`patient_type` = @patient_type," _
                    & "`status` = @status," _
                    & "`dt_released` = @date_release," _
                    & "`main_id` = @MainSampleID" _
                    & " WHERE `main_id` = @mainID AND `testtype` = @Section AND `sub_section` = @SubSection"
                    )

            UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                                & "`accession_no` = @accession_no," _
                                & "`or_no` = @OR_No," _
                                & "`cs_no` = @CS_No," _
                                & "`sample_id` = @MainSampleID" _
                                & " WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
                                )

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
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '1'," _
                          & "`date` = @date_release," _
                          & "`sample_id` = @MainSampleID" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "'"
                          )
                Else
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '0'," _
                          & "`date` = @date_release," _
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

            'version 1.6.0.0-beta
            'Save the Processed, Validated and Printed Time on 'specimen_tracking' table
            UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                & "`sample_id` = @MainSampleID," _
                & "`received` = @specimen_tracking_received," _
                & "`processing` = @specimen_tracking_date_time," _
                & "`validated` = @specimen_tracking_date_time," _
                & "`printed` = @specimen_tracking_date_time" _
                & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @Subsection"
                )

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `order` (`status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, `main_id`, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section`) " _
                & "SELECT `status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, @MainSampleID, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section` FROM `tmpWorklist` WHERE `main_id` = @MainSampleID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `result` (`universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, `sample_id`, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status`) " _
                & "SELECT `universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, @MainSampleID, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status` FROM `tmpResult` WHERE `sample_id` = @MainSampleID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpWorklist` WHERE `main_id` = @MainSampleID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpResult` WHERE `sample_id` = @MainSampleID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Using myRDLCPrinter As New RDLCPrinterPrintNew(MainSampleID, Section, SubSection, 1, My.Settings.DefaultPrinter)
                If My.Settings.SaveAsPDF Then
                    Dim byteViewer As Byte() = myRDLCPrinter.LocalReport.Render("PDF")
                    Dim saveFileDialog1 As New SaveFileDialog()
                    saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
                    saveFileDialog1.FilterIndex = 2
                    saveFileDialog1.RestoreDirectory = True
                    Dim newFile As New FileStream(CreateFolder(Section) & txtSampleID.Text & "_" & txtName.Text & ".pdf", FileMode.Create)
                    newFile.Write(byteViewer, 0, byteViewer.Length)
                    newFile.Close()

                    '------------------Save Email Details------------------------------
                    Connect()
                    rs.Connection = conn
                    rs.CommandType = CommandType.Text
                    rs.CommandText = ("INSERT INTO `pdf_location` (`sample_id`, `pdf_location`, `section`, `sub_section`) VALUES " _
                            & "(" _
                            & "@mainID," _
                            & "@PDFLocation," _
                            & "@Section," _
                            & "@SubSection" _
                            & ")"
                            )
                    rs.ExecuteNonQuery()
                    Disconnect()
                    '------------------Save Email Details------------------------------

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

            'With My.Settings
            '    If .SQLConnection = True Then
            '        UpdateResultiHOMIS()
            '    End If
            'End With

            With My.Settings
                If .AutoEmail = True Then
                    sendmail(txtEmail.Text,
                             txtName.Text,
                             CreateFolder(Section) & mainID & "_" & txtName.Text & ".PDF")

                    '------------------Save Email Details------------------------------
                    Connect()
                    rs.Connection = conn
                    rs.CommandType = CommandType.Text
                    rs.CommandText = ("UPDATE `email_details` SET `status` = 1 WHERE `sample_id` = @MainSampleID AND `section` = @Section AND `sub_section` = @SubSection")
                    rs.ExecuteNonQuery()
                    Disconnect()
                    '------------------Save Email Details------------------------------
                End If
            End With

            frmMolWorklist.LoadRecords()

            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

    Public Sub VerifyResult()
        Try
            If Me.txtName.Text = "" Or cboMedTech.Text = "" Or txtAge.Text = "" Or cboSex.Text = "" Then
                MessageBox.Show("Please verify the data carefully.", "Verification Failed", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Exit Sub
            End If

            With My.Settings
                If .LISType = "Lite" Then
                    MainSampleID = ID_Randomizer()
                Else
                    MainSampleID = mainID
                End If
            End With

            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@mainID", mainID)
            rs.Parameters.AddWithValue("@MainSampleID", MainSampleID)

            rs.Parameters.AddWithValue("@medtech", cboMedTech.Text)
            rs.Parameters.AddWithValue("@medtechid", MedTechID)

            If My.Settings.AuthenticateRelease = True Then
                rs.Parameters.AddWithValue("@verifyid", UserIDRelease)
                rs.Parameters.AddWithValue("@verify", UserRelease)
            Else
                rs.Parameters.AddWithValue("@verifyid", VerifyID)
                rs.Parameters.AddWithValue("@verify", cboVerify.Text)
            End If

            rs.Parameters.AddWithValue("@pathologist", PathologistID)

            rs.Parameters.AddWithValue("@remarks", txtRemarks.Text)
            rs.Parameters.AddWithValue("@lab_comment", txtComment.Text)

            rs.Parameters.AddWithValue("@status", "Validated")

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
            rs.Parameters.AddWithValue("@accession_no", txtAccession.Text)
            rs.Parameters.AddWithValue("@OR_No", txtORNo.Text)
            rs.Parameters.AddWithValue("@CS_No", txtChargeSlip.Text)

            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            rs.Parameters.AddWithValue("@MethodUsed", txtMethodUsed.Text)
            rs.Parameters.AddWithValue("@Reagent", txtReagent.Text)
            rs.Parameters.AddWithValue("@LotNumber", txtLotNumber.Text)
            rs.Parameters.AddWithValue("@Expiry", txtExpiry.Text)

            rs.Parameters.Add("@date", MySql.Data.MySqlClient.MySqlDbType.Date).Value = Format(dtReceived.Value, "yyyy-MM-dd")
            rs.Parameters.AddWithValue("@time", tmTimeReceived.Value.ToLongTimeString)
            rs.Parameters.AddWithValue("@date_release", Format(Now, "yyyy-MM-dd") & " " & Now.ToLongTimeString)
            rs.Parameters.AddWithValue("@specimen_tracking_received", Format(dtReceived.Value, "yyyy-MM-dd") & " " & tmTimeReceived.Value.ToLongTimeString)

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

            UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
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
                    & "`verified_by` = @verifyid," _
                    & "`test` = @test," _
                    & "`patient_type` = @patient_type," _
                    & "`status` = @status," _
                    & "`dt_released` = @date_release," _
                    & "`main_id` = @MainSampleID" _
                    & " WHERE main_id = @mainID AND testtype = @Section AND sub_section = @SubSection"
                    )

            UpdateRecordwthoutMSG("UPDATE `additional_info` SET " _
                            & "`accession_no` = @accession_no," _
                            & "`or_no` = @OR_No," _
                            & "`cs_no` = @CS_No," _
                            & "`sample_id` = @MainSampleID" _
                            & " WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
                            )

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
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '1'," _
                          & "`date` = @date_release," _
                          & "`sample_id` = @MainSampleID" _
                          & " WHERE `sample_id` = @mainID AND `test_code` = '" & GridView.GetRowCellValue(x, "TestCode") & "'"
                          )
                Else
                    UpdateRecordwthoutMSG("UPDATE `tmpresult` SET " _
                          & "`flag` = '" & GridView.GetRowCellValue(x, "Flag") & "'," _
                          & "`measurement` = '" & GridView.GetRowCellValue(x, "SI") & "'," _
                          & "`reference_range` = '" & GridView.GetRowCellValue(x, "ReferenceRange") & "'," _
                          & "`value_conv` = '" & GridView.GetRowCellValue(x, "Conventional") & "'," _
                          & "`ref_conv` = '" & GridView.GetRowCellValue(x, "RefRange") & "'," _
                          & "`patient_id` = @patient_id," _
                          & "`instrument` = '" & GridView.GetRowCellValue(x, "Instrument") & "'," _
                          & "`print_status` = '0'," _
                          & "`date` = @date_release," _
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

            'version 1.6.0.0-beta
            'Save the Processed, Validated and Printed Time on 'specimen_tracking' table
            UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                & "`sample_id` = @MainSampleID," _
                & "`received` = @specimen_tracking_received" _
                & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @Subsection"
                )

            ActivityLogs(txtSampleID.Text,
                         txtPatientID.Text,
                         txtName.Text,
                         CurrUser,
                         "Results Verified",
                         cboRequest.Text,
                         Section,
                         SubSection)

            mainID = MainSampleID

            MessageBox.Show("Transaction No. " & mainID & " has been verified.", "System Message", MessageBoxButtons.OK, MessageBoxIcon.Information)

            frmMolWorklist.LoadRecords()

        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Exit Sub
        End Try
    End Sub

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

            rs.Parameters.Clear()
            rs.Parameters.AddWithValue("@mainID", mainID)
            rs.Parameters.AddWithValue("@Section", Section)
            rs.Parameters.AddWithValue("@SubSection", SubSection)

            rs.Parameters.AddWithValue("@status", "Printed")
            rs.Parameters.AddWithValue("@specimen_tracking_date_time", Now) 'version 1.6.0.0-beta

            rs.Parameters.AddWithValue("@PDFLocation", My.Settings.PDFLocation)

            UpdateRecordwthoutMSG("UPDATE `tmpWorklist` SET " _
                    & "`status` = @status" _
                    & " WHERE main_id = @mainID AND testtype = @Section AND sub_section = @SubSection"
                    )

            'version 1.6.0.0-beta
            'Save the Processed, Validated and Printed Time on 'specimen_tracking' table
            UpdateRecordwthoutMSG("UPDATE `specimen_tracking` SET " _
                & "`sample_id` = @mainID," _
                & "`processing` = @specimen_tracking_date_time," _
                & "`validated` = @specimen_tracking_date_time," _
                & "`printed` = @specimen_tracking_date_time" _
                & " WHERE sample_id = @mainID AND section = @Section AND sub_section = @Subsection"
                )

            Using myRDLCPrinter As New RDLCPrinterPrintNew(MainSampleID, Section, SubSection, 1, My.Settings.DefaultPrinter)
                If My.Settings.SaveAsPDF Then
                    Dim byteViewer As Byte() = myRDLCPrinter.LocalReport.Render("PDF")
                    Dim saveFileDialog1 As New SaveFileDialog()
                    saveFileDialog1.Filter = "*PDF files (*.pdf)|*.pdf"
                    saveFileDialog1.FilterIndex = 2
                    saveFileDialog1.RestoreDirectory = True
                    Dim newFile As New FileStream(CreateFolder(Section) & txtSampleID.Text & "_" & txtName.Text & ".pdf", FileMode.Create)
                    newFile.Write(byteViewer, 0, byteViewer.Length)
                    newFile.Close()

                    '------------------Save PDF Location Details------------------------------
                    Connect()
                    rs.Connection = conn
                    rs.CommandType = CommandType.Text
                    rs.CommandText = ("INSERT INTO `pdf_location` (`sample_id`, `pdf_location`, `section`, `sub_section`) VALUES " _
                            & "(" _
                            & "@mainID," _
                            & "@PDFLocation," _
                            & "@Section," _
                            & "@SubSection" _
                            & ")"
                            )
                    rs.ExecuteNonQuery()
                    Disconnect()
                    '------------------Save PDF Location Details------------------------------

                    myRDLCPrinter.Print(1)
                Else
                    myRDLCPrinter.Print(1)
                End If
            End Using

            Connect()
            rs.Dispose()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `order` (`status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, `main_id`, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section`) " _
                & "SELECT `status`, `sample_id`, `patient_id`, `patient_name`, `age`, `sex`, `bdate`, `physician`, `dept`, `medtech`, `verified_by`, `date`, `time`, `main_id`, `dt_released`, `patient_type`, `test`, `testtype`, `barcode`, `instrument`, `type`, `location`, `sub_section` FROM `tmpWorklist` WHERE `main_id` = @mainID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "INSERT INTO `result` (`universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, `sample_id`, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status`) " _
                & "SELECT `universal_id`, `flag`, `measurement`, `units`, `reference_range`, `value_conv`, `unit_conv`, `ref_conv`, `test_code`, `order_no`, `sample_id`, `test_group`, `date`, `patient_id`, `instrument`, `his_code`, `his_mainid`, `section`, `sub_section`, `print_status` FROM `tmpResult` WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpWorklist` WHERE `main_id` = @mainID AND testtype = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            Connect()
            rs.Connection = conn
            rs.CommandType = CommandType.Text
            rs.CommandText = "DELETE FROM `tmpResult` WHERE `sample_id` = @mainID AND section = @Section AND sub_section = @SubSection"
            rs.ExecuteNonQuery()
            Disconnect()

            With My.Settings
                If .HL7Write = True Then
                    CreateHL7File()
                End If
            End With

            frmMolWorklist.LoadRecords()
            frmMolWorklist.LoadRecordsCompleted()

            Me.Close()
            Me.Dispose()
        Catch ex As Exception
            Disconnect()
            MessageBox.Show(ex.Message, "Mysql Exception", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End Try
    End Sub
#End Region

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
        ''Check If Date of Birth Is nothing then clear text formating
        'If dtBDate.Text = "" Then
        '    'Clear text formating
        '    PatientBDate_Out = ""
        '    '--------------------
        'Else
        '    'Create a format for date of birth (yyyyMMdd)
        '    Dim dDate As DateTime = DateTime.ParseExact(dtBDate.Text, "MM/dd/yyyy", CultureInfo.InvariantCulture)
        '    Dim reformatted As String = dDate.ToString("yyyyMMdd", CultureInfo.InvariantCulture)
        '    PatientBDate_Out = reformatted
        '    '--------------------------------------------
        'End If
        ''-----------------------------------------------------------
        'Dim a As Integer = 0

        'Dim HL7Log As FileStream
        'Dim HL7Writer As StreamWriter
        'Dim HL7File As String = "SBSILIS_" & txtSampleID.Text.Trim & ".HL7"

        'HL7Log = New FileStream(My.Settings.HL7Destination & "\" & HL7File, FileMode.Create, FileAccess.Write)
        'HL7Writer = New StreamWriter(HL7Log)

        'his_test_code = getHIS_Test(txtAccession.Text, txtORNo.Text, txtChargeSlip.Text)



        'HL7Writer.WriteLine("MSH|^~\&|BIZBOX|HOSPITAL|BIOTECH|HOSPITAL|" & FormatDateRegex() & "||ORU^RO1|1|P|2.3|||")
        'HL7Writer.WriteLine("PID|1||" & txtPatientID.Text & "||" & Replace(txtName.Text, ", ", "^") & "||" & PatientBDate_Out & "|" & cboSex.Text.Substring(0, 1))
        'HL7Writer.WriteLine("PV1|1|" & cboPatientType.Text.Substring(0, 1))
        'HL7Writer.WriteLine("ORC|1|" & txtAccession.Text & "|||||||" & FormatDateRegex())
        ''HL7Writer.WriteLine("OBR|1|" & txtAccession.Text & "||^|||" & FormatDateRegex())

        'For x As Integer = 0 To his_test_code.Split("~").Count - 1 Step 1
        '    HL7Writer.WriteLine("OBR|1|" & txtAccession.Text & "||" & his_test_code.Split("~").GetValue(x).ToString & "|||" & FormatDateRegex())
        'Next
        ''For x As Integer = 0 To GridView.RowCount - 1 Step 1
        ''    'HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "SI") & "|" & GridView.GetRowCellValue(x, "Unit") & "|" & GridView.GetRowCellValue(x, "ReferenceRange") & "|N|||F|||" & FormatDateRegex() & "|")
        ''    HL7Writer.WriteLine("OBX|" & x + 1 & "|NM|" & GridView.GetRowCellValue(x, "TestCode") & "||" & GridView.GetRowCellValue(x, "Conventional") & "|||N|||F|||" & FormatDateRegex() & "|")
        ''    a = x + 1
        ''Next
        ''Remarks Section
        'HL7Writer.WriteLine("OBX|" & a + 1 & "|NM|Remarks_H||" & txtRemarks.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        'HL7Writer.WriteLine("OBX|" & a + 2 & "|NM|Others_H||" & txtComment.Text & "|||N|||F|||" & FormatDateRegex() & "|")
        'HL7Writer.Close()
        'HL7Log.Close()
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