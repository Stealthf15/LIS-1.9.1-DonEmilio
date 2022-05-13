Imports MySql.Data.MySqlClient
Imports System.Text.RegularExpressions
Imports DevExpress.XtraGrid.Views.Grid

Public Class frmImmunoPrevious

    Public TypeResult As String = ""
    Public mainID As String = ""
    Public patientID As String = ""
    Public section As String = ""
    Public SubSection As String = ""

    Public Age As String = ""
    Public Sex As String = ""
    Public Classification As String = ""
    Public FLAG As String = ""

    Dim x As Integer

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
            				        specimen.`order_no` AS DisplayNo,
                                    `result`.`date` AS Date
                                FROM `result`
                                LEFT JOIN reference_range ON `result`.test_code = reference_range.test_code AND reference_range.machine = `result`.instrument AND (reference_range.classification = @Classification AND reference_range.gender = @Gender AND (@Age BETWEEN reference_range.age_begin and reference_range.age_end))
                                LEFT JOIN specimen ON `result`.test_code = specimen.test_code AND `result`.instrument = specimen.instrument
                                WHERE `result`.`patient_id` = @PID AND `result`.section = @Section AND `result`.sub_section = @SubSection GROUP BY `result`.test_code ORDER BY specimen.order_no ASC"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@MainID", mainID)
            command.Parameters.AddWithValue("@PID", patientID)
            command.Parameters.AddWithValue("@Section", section)
            command.Parameters.AddWithValue("@SubSection", SubSection)
            command.Parameters.AddWithValue("@Age", Age)
            command.Parameters.AddWithValue("@Gender", Sex)
            command.Parameters.AddWithValue("@Classification", Classification)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)
            command.CommandType = CommandType.Text

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtResult.DataSource = myTable

            GridView.Columns("Conventional").Visible = False
            GridView.Columns("Units").Visible = False
            GridView.Columns("ReferenceRange").Visible = False
            GridView.Columns("RefRange").Visible = False
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

            GridView.Columns("Date").Group()

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

    Private Sub frmAddTestSemi_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        Me.Dispose()
    End Sub

    Private Sub btnClose_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClose.ItemClick
        Me.Close()
    End Sub

    Private Sub frmResultsNew_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        LoadTest()
    End Sub

End Class