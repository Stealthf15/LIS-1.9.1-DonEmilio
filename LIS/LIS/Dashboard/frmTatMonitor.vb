Imports DevExpress.XtraGrid.Views.Grid

Public Class frmTatMonitor

    Dim Count As Integer = 0
    Dim Section As String = ""

    Public Sub LoadRecordsWarding()
        Try
            GridWarding.Columns.Clear()
            GridWarding.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridWarding.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            GridWarding.Appearance.Row.Font = New Font("Segoe UI", 11)

            Dim SQL As String = "dashboard_tat_warding"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)
            command.CommandType = CommandType.StoredProcedure

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@Section", Section)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtWarding.DataSource = myTable

            GridWarding.Columns("Status").Visible = False
            GridWarding.Columns("RoomWard").Visible = False
            GridWarding.Columns("Section").Visible = False
            GridWarding.Columns("SubSection").Visible = False
            GridWarding.Columns("Priority").Visible = False
            GridWarding.Columns("StartDate").Visible = False
            GridWarding.Columns("EstimatedTime").Visible = False
            GridWarding.Columns("TaTStatus").Visible = False

            ' Make the grid read-only. 
            GridWarding.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridWarding.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridWarding.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridWarding.OptionsSelection.MultiSelect = True
            GridWarding.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadRecordsCheckedIn()
        Try
            GridCheckin.Columns.Clear()
            GridCheckin.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridCheckin.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            GridCheckin.Appearance.Row.Font = New Font("Segoe UI", 11)

            Dim SQL As String = "dashboard_tat_first"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)
            command.CommandType = CommandType.StoredProcedure

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@Section", Section)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtCheckIn.DataSource = myTable

            GridCheckin.Columns("Status").Visible = False
            GridCheckin.Columns("RoomWard").Visible = False
            GridCheckin.Columns("Section").Visible = False
            GridCheckin.Columns("SubSection").Visible = False
            GridCheckin.Columns("Priority").Visible = False
            GridCheckin.Columns("StartDate").Visible = False
            GridCheckin.Columns("EstimatedTime").Visible = False
            GridCheckin.Columns("TaTStatus").Visible = False

            ' Make the grid read-only. 
            GridCheckin.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridCheckin.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridCheckin.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridCheckin.OptionsSelection.MultiSelect = True
            GridCheckin.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadRecordsProcessing()
        Try
            GridProcessing.Columns.Clear()
            GridProcessing.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridProcessing.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            GridProcessing.Appearance.Row.Font = New Font("Segoe UI", 11)

            Dim SQL As String = "dashboard_tat_second"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)
            command.CommandType = CommandType.StoredProcedure

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@Section", Section)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtProcessing.DataSource = myTable

            GridProcessing.Columns("Status").Visible = False
            GridProcessing.Columns("RoomWard").Visible = False
            GridProcessing.Columns("Section").Visible = False
            GridProcessing.Columns("SubSection").Visible = False
            GridProcessing.Columns("Priority").Visible = False
            GridProcessing.Columns("StartDate").Visible = False
            GridProcessing.Columns("EstimatedTime").Visible = False
            GridProcessing.Columns("TaTStatus").Visible = False

            ' Make the grid read-only. 
            GridProcessing.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridProcessing.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridProcessing.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridProcessing.OptionsSelection.MultiSelect = True
            GridProcessing.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Public Sub LoadRecordsCompleted()
        Try
            GridCompleted.Columns.Clear()
            GridCompleted.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center
            GridCompleted.Appearance.HeaderPanel.FontStyleDelta = FontStyle.Bold

            GridCompleted.Appearance.Row.Font = New Font("Segoe UI", 11)

            Dim SQL As String = "dashboard_tat_third"

            Dim command As New MySql.Data.MySqlClient.MySqlCommand(SQL, conn)
            command.CommandType = CommandType.StoredProcedure

            command.Parameters.Clear()
            command.Parameters.AddWithValue("@Section", Section)

            Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter(command)

            Dim myTable As DataTable = New DataTable
            adapter.Fill(myTable)

            dtCompleted.DataSource = myTable

            GridCompleted.Columns("Status").Visible = False
            GridCompleted.Columns("RoomWard").Visible = False
            GridCompleted.Columns("Section").Visible = False
            GridCompleted.Columns("SubSection").Visible = False
            GridCompleted.Columns("Priority").Visible = False
            GridCompleted.Columns("StartDate").Visible = False
            GridCompleted.Columns("EstimatedTime").Visible = False
            GridCompleted.Columns("TaTStatus").Visible = False

            ' Make the grid read-only. 
            GridCompleted.OptionsBehavior.Editable = False
            ' Prevent the focused cell from being highlighted. 
            GridCompleted.OptionsSelection.EnableAppearanceFocusedCell = False
            ' Draw a dotted focus rectangle around the entire row. 
            GridCompleted.FocusRectStyle = DrawFocusRectStyle.RowFullFocus

            GridCompleted.OptionsSelection.MultiSelect = True
            GridCompleted.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect

        Catch ex As Exception
            MessageBox.Show(ex.Message, "System Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub GridCheckin_RowCellStyle(sender As Object, e As RowCellStyleEventArgs) Handles GridCheckin.RowCellStyle, GridProcessing.RowCellStyle, GridCompleted.RowCellStyle
        Dim view As GridView = TryCast(sender, GridView)
        If (e.Column.FieldName = "SampleID") Or (e.Column.FieldName = "PatientName") Or (e.Column.FieldName = "RemainingTime") Then
            If view.GetRowCellValue(e.RowHandle, "RoomWard") = "OPD" Then
                e.Appearance.BackColor = Color.CornflowerBlue
                e.Appearance.BackColor2 = Color.CornflowerBlue
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "RoomWard") = "ER" Then
                e.Appearance.BackColor = Color.ForestGreen
                e.Appearance.BackColor2 = Color.ForestGreen
                e.Appearance.ForeColor = Color.White
            Else
                e.Appearance.BackColor = Color.Orange
                e.Appearance.BackColor2 = Color.Orange
                e.Appearance.ForeColor = Color.White
            End If

            If view.GetRowCellValue(e.RowHandle, "TaTStatus") = "Over TaT" Then
                e.Appearance.BackColor = Color.OrangeRed
                e.Appearance.BackColor2 = Color.OrangeRed
                e.Appearance.ForeColor = Color.White
            ElseIf view.GetRowCellValue(e.RowHandle, "TaTStatus") = "Warning" Then
                e.Appearance.BackColor = Color.RoyalBlue
                e.Appearance.BackColor2 = Color.RoyalBlue
                e.Appearance.ForeColor = Color.White
            End If
        End If

        If view.GetRowCellValue(e.RowHandle, "Priority").ToString = "STAT" Then
            e.Appearance.ForeColor = Color.DarkRed
            e.Appearance.FontStyleDelta = FontStyle.Bold
        End If
    End Sub

    Private Sub tmCheckin_Tick(sender As Object, e As EventArgs) Handles tmCheckin.Tick
        LoadRecordsWarding()
        LoadRecordsCheckedIn()
        LoadRecordsProcessing()
        LoadRecordsCompleted()
    End Sub

    Private Sub tmChangeSection_Tick(sender As Object, e As EventArgs) Handles tmChangeSection.Tick
        Count = Count + 1
        If Count >= 0 And Count <= 30 Then
            Section = "Chemistry"
            lblHeader.Text = Section & " Section"
        ElseIf Count >= 31 And Count <= 60 Then
            Section = "Hematology"
            lblHeader.Text = Section & " Section"
        ElseIf Count >= 61 And Count <= 90 Then
            Section = "Urinalysis"
            lblHeader.Text = Section & " Section"
        ElseIf Count >= 91 And Count <= 120 Then
            Section = "Fecalysis"
            lblHeader.Text = Section & " Section"
        ElseIf Count >= 121 And Count <= 150 Then
            Section = "ImmunoSero"
            lblHeader.Text = Section & " Section"
        ElseIf Count > 150 Then
            Count = 0
        End If
    End Sub
End Class